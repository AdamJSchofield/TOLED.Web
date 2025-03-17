using TOLED.Web.Data.Models;

namespace TOLED.Web.Services
{
    /// <summary>
    /// Service for responding to requests for device data from devices themselves.
    /// </summary>
    public interface IDeviceDataService
    {
        public Task<PPImage> GetActiveImageForDeviceAsync(Guid deviceId);
    }
}
