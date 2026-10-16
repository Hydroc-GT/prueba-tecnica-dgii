namespace DGII.Domain.Entities;

public class ComprobanteFiscal
{
    public string RncCedula { get; set; } = string.Empty;
    public string Ncf { get; set; } = string.Empty;
    public decimal Monto { get; set; }
    public decimal Itbis18 { get; set; }

    public Contribuyente? Contribuyente { get; set; }
}
