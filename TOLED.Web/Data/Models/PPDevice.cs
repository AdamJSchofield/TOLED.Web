namespace TOLED.Web.Data.Models
{
    public class PPDevice
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Name { get; set; } = "New Device";
        public IQueryable<ApplicationUser> Users { get; set; } = Enumerable.Empty<ApplicationUser>().AsQueryable();
    }
}
