using Infrastructure.Models;

namespace Infrastructure.Interfaces.Services;

public interface IQrService
{
    byte[] GetQrCode(QrCodeEntity movableInstance, Guid code);
}