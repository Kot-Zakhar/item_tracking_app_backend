public class GlobalConfig
{
    public required string Domain { get; set; }
    public required string JwtPrivateKey { get; set; }
    public required string MediatrLicenseKey { get; set; }
    public required string SqsUrl { get; set; }
    public required string LocationServiceAddress { get; set; }
    public required string InventoryServiceAddress { get; set; }
    public required string ManagementServiceAddress { get; set; }
    public required string UserServiceAddress { get; set; }
}