using System.Text.Json.Serialization;

namespace DGII.Application.DTOs;

public class ContribuyenteComprobantesResumenDto
{
    [JsonPropertyName("rncCedula")]
    public string RncCedula { get; set; } = string.Empty;

    [JsonPropertyName("nombre")]
    public string Nombre { get; set; } = string.Empty;

    [JsonPropertyName("tipo")]
    public string Tipo { get; set; } = string.Empty;

    [JsonPropertyName("estatus")]
    public string Estatus { get; set; } = string.Empty;

    [JsonPropertyName("totalItbis")]
    public decimal TotalItbis { get; set; }

    [JsonPropertyName("totalItbisFormateado")]
    public string TotalItbisFormateado { get; set; } = string.Empty;

    [JsonPropertyName("comprobantes")]
    public IReadOnlyList<ComprobanteFiscalDto> Comprobantes { get; set; } = Array.Empty<ComprobanteFiscalDto>();
}
