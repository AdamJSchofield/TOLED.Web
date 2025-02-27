using TOLED.Web.Data.Models;

namespace TOLED.Web.Services
{
    public interface IImageCollectionService
    {
        public Task<bool> AddUserToCollection(ApplicationUser user, Guid collectionId);
        public Task<bool> RemoveUserFromCollection(ApplicationUser user, Guid collectionId);
        public Task<ImageCollection> GetCollectionForUser(ApplicationUser user, Guid collectionId);
    }
}
