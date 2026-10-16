using DGII.Application.Exceptions;
using DGII.Application.Services;
using DGII.Domain.Entities;
using DGII.Domain.Interfaces;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;

namespace DGII.UnitTests.Services;

public class ContribuyenteServiceTests
{
    private readonly Mock<IContribuyenteRepository> _repositoryMock;
    private readonly Mock<ILogger<ContribuyenteService>> _loggerMock;
    private readonly ContribuyenteService _service;

    public ContribuyenteServiceTests()
    {
        _repositoryMock = new Mock<IContribuyenteRepository>();
        _loggerMock = new Mock<ILogger<ContribuyenteService>>();
        _service = new ContribuyenteService(_repositoryMock.Object, _loggerMock.Object);
    }

    [Fact]
    public async Task GetAllContribuyentesAsync_ShouldReturnAllContribuyentes_WhenRecordsExist()
    {
        // Arrange
        var contribuyentes = new List<Contribuyente>
        {
            new() { RncCedula = "98754321012", Nombre = "JUAN PEREZ", Tipo = "PERSONA FISICA", Estatus = "activo" },
            new() { RncCedula = "123456789", Nombre = "FARMACIA TU SALUD", Tipo = "PERSONA JURIDICA", Estatus = "inactivo" }
        };

        _repositoryMock
            .Setup(r => r.GetAllAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(contribuyentes);

        // Act
        var result = await _service.GetAllContribuyentesAsync();

        // Assert
        result.Should().NotBeNull();
        result.Should().HaveCount(2);
        result[0].RncCedula.Should().Be("98754321012");
        result[0].Nombre.Should().Be("JUAN PEREZ");
        result[1].RncCedula.Should().Be("123456789");
    }

    [Fact]
    public async Task GetContribuyenteByRncAsync_ShouldReturnContribuyente_WhenExists()
    {
        // Arrange
        var rnc = "98754321012";
        var entity = new Contribuyente
        {
            RncCedula = rnc,
            Nombre = "JUAN PEREZ",
            Tipo = "PERSONA FISICA",
            Estatus = "activo"
        };

        _repositoryMock
            .Setup(r => r.GetByRncAsync(rnc, It.IsAny<CancellationToken>()))
            .ReturnsAsync(entity);

        // Act
        var result = await _service.GetContribuyenteByRncAsync(rnc);

        // Assert
        result.Should().NotBeNull();
        result.RncCedula.Should().Be(rnc);
        result.Nombre.Should().Be("JUAN PEREZ");
        result.Tipo.Should().Be("PERSONA FISICA");
        result.Estatus.Should().Be("activo");
    }

    [Fact]
    public async Task GetContribuyenteByRncAsync_ShouldThrowNotFoundException_WhenNotExists()
    {
        // Arrange
        var rnc = "99999999999";
        _repositoryMock
            .Setup(r => r.GetByRncAsync(rnc, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Contribuyente?)null);

        // Act
        var act = async () => await _service.GetContribuyenteByRncAsync(rnc);

        // Assert
        await act.Should().ThrowAsync<NotFoundException>()
            .WithMessage($"*{rnc}*");
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData("12345")]
    [InlineData("12345678A")]
    [InlineData("987543210123")]
    public async Task GetContribuyenteByRncAsync_ShouldThrowValidationException_WhenRncFormatIsInvalid(string invalidRnc)
    {
        // Act
        var act = async () => await _service.GetContribuyenteByRncAsync(invalidRnc);

        // Assert
        await act.Should().ThrowAsync<ValidationException>();
        _repositoryMock.Verify(r => r.GetByRncAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task GetAllContribuyentesAsync_ShouldLogErrorAndRethrow_WhenRepositoryFails()
    {
        // Arrange
        _repositoryMock
            .Setup(r => r.GetAllAsync(It.IsAny<CancellationToken>()))
            .ThrowsAsync(new InvalidOperationException("DB error"));

        // Act
        var act = async () => await _service.GetAllContribuyentesAsync();

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
