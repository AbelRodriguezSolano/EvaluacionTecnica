namespace EvaluacionTecnica.Domain.Entities;

public class Role : BaseEntity
{
    public string Nombre { get; set; } = string.Empty;
    public ICollection<User> Users { get; set; } = new List<User>();
}
