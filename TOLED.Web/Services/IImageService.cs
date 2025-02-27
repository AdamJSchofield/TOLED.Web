using Microsoft.EntityFrameworkCore;
using TOLED.Web.Data;
using TOLED.Web.Data.Models;
using TOLED.Web.Encoders;
using TOLED.Web.Forms;

namespace TOLED.Web.Services
{
    public interface IImageService
    {
        public Task<IEnumerable<PPImage>> GetImagesAsync();
        public Task<PPImage> AddOrUpdateImageAsync(PPImageFormModel image, bool regenerateDisplay = false);
        public Task<PPImage?> GetImageAsync(int id);
        public Task SetActiveImageAsync(int id, bool setActive = true);
        public Task<PPImage?> GetActiveImageAsync();
        public Task<bool> DeleteImageAsync(int id);
        public Task SavePremiumAsync(int imageId);
    }

    public class ImageService(ToledDbContext dbContext) : IImageService
    {
        public async Task<IEnumerable<PPImage>> GetImagesAsync() => await dbContext.Images.ToListAsync();

        public async Task<PPImage?> GetImageAsync(int id) => await dbContext.Images.FindAsync(id);

        public async Task<PPImage> AddOrUpdateImageAsync(PPImageFormModel formModel, bool regenerateDisplay = false)
        {
            PPImage? image = null;
            var adding = false;

            // If the form model has an ID, we are updating an existing image
            if (formModel.Id != null)
            {
                image = await dbContext.Images.FindAsync(formModel.Id);
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
                image = await AddImageAsync(image);
            }

            await dbContext.SaveChangesAsync();

            return image;
        }

        internal async Task<PPImage> AddImageAsync(PPImage image)
        {
            var addedImage = await dbContext.Images.AddAsync(image);
            await dbContext.SaveChangesAsync();
            return addedImage.Entity;
        }

        public async Task<PPImage?> GetActiveImageAsync() => await dbContext.Images.FirstOrDefaultAsync(x => x.IsActive);

        public async Task SetActiveImageAsync(int id, bool setActive = true)
        {
            await dbContext.Images.Where(i => i.IsActive).ForEachAsync(i => i.IsActive = false);
            var foundImage = await dbContext.Images.FindAsync(id);
            if (foundImage != null)
            {
                foundImage.IsActive = true;
                await dbContext.SaveChangesAsync();
            }
        }

        public async Task<bool> DeleteImageAsync(int id)
        {
            var changed = false;
            var imageToDelete = dbContext.Images.Find(id);
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

    }
}
