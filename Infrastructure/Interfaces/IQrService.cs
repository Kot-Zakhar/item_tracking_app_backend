using Infrastructure.Models;

namespace Infrastructure.Interfaces;

public interface IQrService
{
    byte[] GetQrCode(QrCodeEntity movableInstance, Guid code);
}