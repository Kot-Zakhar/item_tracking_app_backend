namespace Abstractions;

public interface IInfrastructureGlobalConfig
{
    string PasswordPepper { get; }
    string Domain { get; }
    string JwtPrivateKey { get; }
}
