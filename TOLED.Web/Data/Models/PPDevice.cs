namespace TOLED.Web.Data.Models
{
    public class PPDevice
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid DeviceSecret { get; set; } = Guid.NewGuid();
        public string Name { get; set; } = "New Device";
        public PPDeviceStatus Status { get; set; } = PPDeviceStatus.Offline;
        public ICollection<ApplicationUser> Users { get; set; } = new List<ApplicationUser>();
        public ApplicationUser Owner { get; set; } = default!;
        public PPImage? ActiveImage { get; set; }
    }

    public enum PPDeviceStatus
    {
        Online,
        Offline
    }
}
