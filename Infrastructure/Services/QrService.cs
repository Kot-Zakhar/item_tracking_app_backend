using Abstractions;
using Infrastructure.Interfaces;
using Infrastructure.Models;
using QRCoder;

namespace Infrastructure.Services;

public class QrService(IInfrastructureGlobalConfig config) : IQrService
{
    private static readonly Dictionary<QrCodeEntity, string> _qrPathes = new()
    {
        { QrCodeEntity.MovableInstance, "i" },
        { QrCodeEntity.Location, "l" },
    };

    public byte[] GetQrCode(QrCodeEntity entityType, Guid code)
    {
        if (!_qrPathes.TryGetValue(entityType, out var path))
        {
            throw new ArgumentException($"Unknown QR code entity type: {entityType}", nameof(entityType));
        }

        string shortenedCode = code.ToString()[..8];

        string data = $"{config.Domain.TrimEnd('/')}/{path}/{shortenedCode}";
        return GetQrCodeAsync(data);
    }
    
    private byte[] GetQrCodeAsync(string data)
    {
        using (QRCodeGenerator qrGenerator = new QRCodeGenerator())
        using (QRCodeData qrCodeData = qrGenerator.CreateQrCode(data, QRCodeGenerator.ECCLevel.Q))
        using (PngByteQRCode qrCode = new PngByteQRCode(qrCodeData))
        {
            return qrCode.GetGraphic(20);
        }
    }
}