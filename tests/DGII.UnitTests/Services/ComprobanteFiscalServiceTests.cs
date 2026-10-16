using DGII.Application.Exceptions;
using DGII.Application.Services;
using DGII.Domain.Entities;
using DGII.Domain.Interfaces;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;

namespace DGII.UnitTests.Services;

public class ComprobanteFiscalServiceTests
{
    private readonly Mock<IComprobanteFiscalRepository> _comprobanteRepoMock;
    private readonly Mock<IContribuyenteRepository> _contribuyenteRepoMock;
    private readonly Mock<ILogger<ComprobanteFiscalService>> _loggerMock;
    private readonly ComprobanteFiscalService _service;

    public ComprobanteFiscalServiceTests()
    {
        _comprobanteRepoMock = new Mock<IComprobanteFiscalRepository>();
        _contribuyenteRepoMock = new Mock<IContribuyenteRepository>();
        _loggerMock = new Mock<ILogger<ComprobanteFiscalService>>();
        _service = new ComprobanteFiscalService(
            _comprobanteRepoMock.Object,
            _contribuyenteRepoMock.Object,
            _loggerMock.Object);
    }

    [Fact]
    public async Task GetResumenPorContribuyenteAsync_ShouldCalculateExactItbis216_ForPdfTestCase()
    {
        // Arrange
        var rnc = "98754321012";
        var contribuyente = new Contribuyente
        {
            RncCedula = rnc,
            Nombre = "JUAN PEREZ",
            Tipo = "PERSONA FISICA",
            Estatus = "activo"
        };

        var comprobantes = new List<ComprobanteFiscal>
        {
            new() { RncCedula = rnc, Ncf = "E310000000001", Monto = 200.00m, Itbis18 = 36.00m },
            new() { RncCedula = rnc, Ncf = "E310000000002", Monto = 1000.00m, Itbis18 = 180.00m }
        };

        _contribuyenteRepoMock
            .Setup(r => r.GetByRncAsync(rnc, It.IsAny<CancellationToken>()))
            .ReturnsAsync(contribuyente);

        _comprobanteRepoMock
            .Setup(r => r.GetByRncAsync(rnc, It.IsAny<CancellationToken>()))
            .ReturnsAsync(comprobantes);

        // Act
        var result = await _service.GetResumenPorContribuyenteAsync(rnc);

        // Assert
        result.Should().NotBeNull();
        result.RncCedula.Should().Be(rnc);
        result.Nombre.Should().Be("JUAN PEREZ");
        result.TotalItbis.Should().Be(216.00m);
        result.TotalItbisFormateado.Should().Be("$216.00");
        result.Comprobantes.Should().HaveCount(2);
        result.Comprobantes[0].Ncf.Should().Be("E310000000001");
        result.Comprobantes[0].Monto.Should().Be("200.00");
        result.Comprobantes[0].Itbis18.Should().Be("36.00");
        result.Comprobantes[1].Ncf.Should().Be("E310000000002");
        result.Comprobantes[1].Monto.Should().Be("1000.00");
        result.Comprobantes[1].Itbis18.Should().Be("180.00");
    }

    [Fact]
    public async Task GetResumenPorContribuyenteAsync_ShouldReturnZeroItbis_WhenTaxpayerHasNoReceipts()
    {
        // Arrange
        var rnc = "123456789";
        var contribuyente = new Contribuyente
        {
            RncCedula = rnc,
            Nombre = "FARMACIA TU SALUD",
            Tipo = "PERSONA JURIDICA",
            Estatus = "inactivo"
        };

        _contribuyenteRepoMock
            .Setup(r => r.GetByRncAsync(rnc, It.IsAny<CancellationToken>()))
            .ReturnsAsync(contribuyente);

        _comprobanteRepoMock
            .Setup(r => r.GetByRncAsync(rnc, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<ComprobanteFiscal>());

        // Act
        var result = await _service.GetResumenPorContribuyenteAsync(rnc);

        // Assert
        result.TotalItbis.Should().Be(0.00m);
        result.TotalItbisFormateado.Should().Be("$0.00");
        result.Comprobantes.Should().BeEmpty();
    }

    [Fact]
    public async Task GetResumenPorContribuyenteAsync_ShouldThrowNotFoundException_WhenTaxpayerDoesNotExist()
    {
        // Arrange
        var rnc = "00000000000";
        _contribuyenteRepoMock
            .Setup(r => r.GetByRncAsync(rnc, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Contribuyente?)null);

        // Act
        var act = async () => await _service.GetResumenPorContribuyenteAsync(rnc);

        // Assert
        await act.Should().ThrowAsync<NotFoundException>();
    }

    [Theory]
    [InlineData("123")]
    [InlineData("invalid-rnc")]
    [InlineData("1234567890123")]
    public async Task GetResumenPorContribuyenteAsync_ShouldThrowValidationException_WhenRncFormatIsInvalid(string invalidRnc)
    {
        // Act
        var act = async () => await _service.GetResumenPorContribuyenteAsync(invalidRnc);

        // Assert
        await act.Should().ThrowAsync<ValidationException>();
        _contribuyenteRepoMock.Verify(r => r.GetByRncAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task GetComprobantesByRncAsync_ShouldLogErrorAndRethrow_WhenRepositoryFails()
    {
        // Arrange
        var rnc = "98754321012";
        _contribuyenteRepoMock
            .Setup(r => r.ExistsAsync(rnc, It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        _comprobanteRepoMock
            .Setup(r => r.GetByRncAsync(rnc, It.IsAny<CancellationToken>()))
            .ThrowsAsync(new InvalidOperationException("DB error"));

        // Act
        var act = async () => await _service.GetComprobantesByRncAsync(rnc);

        // Assert
        await act.Should().ThrowAsync<InvalidOperationException>();

        _loggerMock.Verify(
            x => x.Log(
                LogLevel.Error,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => true),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
            Times.Once);
    }
}
