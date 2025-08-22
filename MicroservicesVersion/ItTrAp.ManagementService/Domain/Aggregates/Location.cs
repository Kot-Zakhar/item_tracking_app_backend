namespace ItTrAp.ManagementService.Domain.Aggregates;

public class Location
{
    public uint Id { get; set; }
    public Guid Code { get; set; }

    public virtual IList<MovableInstance> MovableInstances { get; set; } = [];

    public static Location Create(uint id, Guid code)
    {
        return new Location
        {
            Id = id,
            Code = code,
        };
    }    
}