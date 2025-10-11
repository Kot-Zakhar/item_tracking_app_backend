namespace ItTrAp.QueryService.Infrastructure.Models;

public class SnsNotification
{
    public required string Type { get; set; }
    public required string MessageId { get; set; }
    public required string TopicArn { get; set; }
    public required string Message { get; set; }
    public DateTime Timestamp { get; set; }
    public required string UnsubscribeURL { get; set; }
    public required string SignatureVersion { get; set; }
    public required string Signature { get; set; }
    public required string SigningCertURL { get; set; }
}
