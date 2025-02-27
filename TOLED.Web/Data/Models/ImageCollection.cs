namespace TOLED.Web.Data.Models
{
    public class ImageCollection
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public virtual IQueryable<PPImage> Images { get; set; } = Enumerable.Empty<PPImage>().AsQueryable();
        public IQueryable<ApplicationUser> Users { get; set; } = Enumerable.Empty<ApplicationUser>().AsQueryable();
    }
}
