public class GlobalConfig
{
    public required string Domain { get; set; }
    public required string JwtPrivateKey { get; set; }
    public required string MediatrLicenseKey { get; set; }
    public required string OutboundSnsTopicArn { get; set; }
    public required string UserAvatarUrlTemplate { get; set; }

    public required string AdminEmail { get; set; }
    public required string AdminPhone { get; set; }
}