public class GlobalConfig
{
    public required string PasswordPepper { get; set; }
    public required string Domain { get; set; }
    public required string JwtPrivateKey { get; set; }
    
    public required string AdminEmail { get; set; }
    public required string AdminPassword { get; set; }

    public required string MediatrLicenseKey { get; set; }
}