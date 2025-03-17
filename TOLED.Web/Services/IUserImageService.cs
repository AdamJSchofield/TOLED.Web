using Microsoft.EntityFrameworkCore;
using TOLED.Web.Data;
using TOLED.Web.Data.Models;
using TOLED.Web.Encoders;
using TOLED.Web.Forms;

namespace TOLED.Web.Services
{
    public interface IUserImageService
    {
        public Task<ICollection<PPImage>> GetImagesForUserAsync(ApplicationUser owner);
        public Task<PPImage> AddOrUpdateImageAsync(ApplicationUser owner, PPImageFormModel image, bool regenerateDisplay = false);
        public Task<PPImage?> GetImageForUserAsync(ApplicationUser owner, int id);
        public Task<bool> DeleteImageForUserAsync(ApplicationUser owner, int id);
        public Task SavePremiumAsync(int imageId);
    }

    public class UserImageService(ToledDbContext dbContext) : IUserImageService
    {
        public async Task<ICollection<PPImage>> GetImagesForUserAsync(ApplicationUser owner) => await dbContext.Images.Where(i => i.Owner == owner).ToListAsync();

        public async Task<PPImage?> GetImageForUserAsync(ApplicationUser owner, int id) => await dbContext.Images.FirstOrDefaultAsync(i => i.Owner == owner);

        public async Task<PPImage> AddOrUpdateImageAsync(ApplicationUser owner, PPImageFormModel formModel, bool regenerateDisplay = false)
        {
            PPImage? image = null;
            var adding = false;

            // If the form model has an ID, we are updating an existing image
            if (formModel.Id != null)
            {
                image = await GetImageForUserAsync(owner, formModel.Id.Value);
            }

            // If the image is null, we are adding a new image
            if (image == null)
            {
                image = new PPImage();
                adding = true;
            }

            // Always update the image name
            image.Name = formModel.Name;
            image.MutateOptions = formModel.MutateOptions;

            // If the form model has a file, update the raw image data
            if (formModel.File != null)
            {
                using var memoryStream = new MemoryStream();
                using var fileStream = formModel.File.OpenReadStream();
                await fileStream.CopyToAsync(memoryStream);
                image.RawData = memoryStream.ToArray();
            }

            if (adding || regenerateDisplay)
            {
                var displayResult = await Ssd1309Encoder.EncodeImageAsync(image.RawData, image.MutateOptions);
                image.DisplayData = displayResult.DisplayData;
                image.Frames = displayResult.DisplayFrames;
            }

            // If the image is being added, add it to the database
            if (adding)
            {
                image.Owner = owner;
                var entry = await dbContext.Images.AddAsync(image);
                await dbContext.SaveChangesAsync();
                return entry.Entity;
            }

            await dbContext.SaveChangesAsync();

            return image;
        }

        public async Task<bool> DeleteImageForUserAsync(ApplicationUser owner, int id)
        {
            var changed = false;
            var imageToDelete = await GetImageForUserAsync(owner, id);
            if (imageToDelete != null)
            {
                dbContext.Images.Remove(imageToDelete);
                changed = true;
            }

            if (changed)
            {
                await dbContext.SaveChangesAsync();
            }

            return changed;
        }

        public async Task SavePremiumAsync(int imageId)
        {
            var image = await dbContext.Images.FindAsync(imageId);
            if (image != null)
            {
                image.MutateOptions.HasPremium = true;
                await dbContext.SaveChangesAsync();
            }
        }

        public async Task SetActiveImageAsync(ApplicationUser user, int id, bool setActive = true)
        {
            await dbContext.Images.Where(i => i.IsActive).ForEachAsync(i => i.IsActive = false);
            var foundImage = await dbContext.Images.FindAsync(id);
            if (foundImage != null)
            {
                foundImage.IsActive = true;
                await dbContext.SaveChangesAsync();
            }
        }

    }
}
