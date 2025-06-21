namespace Abstractions;

public interface IInfrastructureGlobalConfig
{
    string PasswordPepper { get; }
    string Domain { get; }
    string JwtPrivateKey { get; }
    string AdminEmail { get; }
    string AdminPassword { get; }
    string AdminPhone { get; }
    string UserAvatarUrlTemplate { get; }
}
