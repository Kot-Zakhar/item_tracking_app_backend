namespace ItTrAp.ManagementService.Domain.Aggregates;

public class Location
{
    public uint Id { get; set; }
    public Guid Code { get; set; }

    public virtual IList<MovableInstance> MovableInstances { get; set; } = new List<MovableInstance>();

    public static Location Create(Guid code)
    {
        return new Location
        {
            Code = code
        };
    }    
}