namespace EvaluacionTecnica.Domain.Entities;

public abstract class BaseEntity
{
    public int Id { get; set; }
    public string Usuario_Transaccion { get; set; } = string.Empty;
    public DateTime Fecha_Transaccion { get; set; }
}
