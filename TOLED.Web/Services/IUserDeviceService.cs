using Microsoft.EntityFrameworkCore;
using TOLED.Web.Data;
using TOLED.Web.Data.Models;

namespace TOLED.Web.Services
{
    public interface IUserDeviceService
    {
        Task<ICollection<PPDevice>> GetDevicesForUserAsync(ApplicationUser owner);
        Task<PPDevice> RegisterNewDevice(ApplicationUser owner, PPDevice device);
        Task<PPDevice?> AddUserToDevice(ApplicationUser user, Guid deviceId);
        Task<bool> RemoveUserFromDevice(ApplicationUser owner, ApplicationUser user, Guid deviceId);
        Task<bool> DeleteDeviceAsync(ApplicationUser owner, Guid deviceId);
    }

    public class UserDeviceService(ToledDbContext dbContext) : IUserDeviceService
    {
        public async Task<ICollection<PPDevice>> GetDevicesForUserAsync(ApplicationUser user)
        {
            return await dbContext.Devices
                .Where(d => d.Owner == user || d.Users.Contains(user))
                .ToListAsync();
        }

        public async Task<PPDevice> RegisterNewDevice(ApplicationUser owner, PPDevice device)
        {
            device.Owner = owner;
            await dbContext.Devices.AddAsync(device);
            await dbContext.SaveChangesAsync();
            return device;
        }

        public async Task<PPDevice?> AddUserToDevice(ApplicationUser user, Guid deviceId)
        {
            var device = await dbContext.Devices.FindAsync(deviceId);
            if (device == null)
            {
                return null;
            }
            device.Users.Add(user);
            await dbContext.SaveChangesAsync();
            return device;
        }

        public async Task<bool> RemoveUserFromDevice(ApplicationUser owner, ApplicationUser user, Guid deviceId)
        {
            var device = await dbContext.Devices.FindAsync(deviceId);
            if (device == null || device.Owner != owner)
            {
                return false;
            }
            device.Users.Remove(user);
            await dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteDeviceAsync(ApplicationUser owner, Guid deviceId)
        {
            var device = await dbContext.Devices.FindAsync(deviceId);
            if (device == null || device.Owner != owner)
            {
                return false;
            }
            dbContext.Devices.Remove(device);
            await dbContext.SaveChangesAsync();
            return true;
        }
    }
}
