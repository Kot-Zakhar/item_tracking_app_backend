public class GlobalConfig
{
    public required string Domain { get; set; }
    public required string JwtPrivateKey { get; set; }
    public required string MediatrLicenseKey { get; set; }
    public required string OutboundSnsTopicArn { get; set; }
}