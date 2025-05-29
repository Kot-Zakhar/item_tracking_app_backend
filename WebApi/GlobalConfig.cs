using Abstractions;

public class GlobalConfig : IInfrastructureGlobalConfig
{
    public required string PasswordPepper { get; set; }
    public required string Domain { get; set; }
    public required string JwtPrivateKey { get; set; }

    public GlobalConfig(IConfiguration configuration)
    {
        configuration.Bind(this);
    }
}