namespace DGII.Application.Exceptions;

public class NotFoundException : Exception
{
    public NotFoundException(string message) : base(message)
    {
    }

    public NotFoundException(string entityName, object key)
        : base($"Entidad '{entityName}' con identificador '{key}' no fue encontrada.")
    {
    }
}
