using System.Text.Json.Serialization;

namespace DGII.Application.DTOs;

public class ComprobanteFiscalDto
{
    [JsonPropertyName("rncCedula")]
    public string RncCedula { get; set; } = string.Empty;

    [JsonPropertyName("NCF")]
    public string Ncf { get; set; } = string.Empty;

    [JsonPropertyName("monto")]
    public string Monto { get; set; } = string.Empty;

    [JsonPropertyName("itbis18")]
    public string Itbis18 { get; set; } = string.Empty;
}
