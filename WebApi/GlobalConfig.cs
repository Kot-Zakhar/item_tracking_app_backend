using Abstractions;

public class GlobalConfig : IInfrastructureGlobalConfig
{
    public required string PasswordPepper { get; set; }
    public required string Domain { get; set; }
    public required string JwtPrivateKey { get; set; }
    
    public required string AdminEmail { get; set; }
    public required string AdminPassword { get; set; }
    public required string AdminPhone { get; set; }

    public required string UserAvatarUrlTemplate { get; set; }
}