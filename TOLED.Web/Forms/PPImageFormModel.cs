using FluentValidation;
using Microsoft.AspNetCore.Components.Forms;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp.Processing.Processors.Dithering;
using TOLED.Web.Data.Models;

namespace TOLED.Web.Forms
{
    public class PPImageFormModel
    {
        public PPImageFormModel(PPImage? image = null)
        {
            if (image != null)
            {
                Id = image.Id;
                Name = image.Name;
                RawData = image.RawData;
                Frames = image.Frames;
                IsActive = image.IsActive;
                MutateOptions = image.MutateOptions;
            }
        }

        public int? Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public IBrowserFile? File { get; set; }
        public byte[]? RawData { get; set; }
        public int Frames { get; set; } = 1;
        public bool IsActive { get; set; } = false;
        public PPImageMutateOptions MutateOptions { get; set; } = new PPImageMutateOptions();
    }

    /// <summary>
    /// A standard AbstractValidator which contains multiple rules and can be shared with the back end API
    /// </summary>
    /// <typeparam name="PPImageFormModel"></typeparam>
    public class PPImageFormModelValidator : AbstractValidator<PPImageFormModel>
    {
        public PPImageFormModelValidator(bool adding = false)
        {
            RuleFor(x => x.Name)
                .NotEmpty()
            .Length(1, 100);
            if (adding)
            {
                RuleFor(x => x.File)
                    .NotEmpty();
            }
            When(x => x.File != null, () =>
            {
                RuleFor(x => x.File!.Size).LessThanOrEqualTo(5000000).WithMessage("The maximum file size is ~5 MB");
            });
        }

        public Func<object, string, Task<IEnumerable<string>>> ValidateValue => async (model, propertyName) =>
        {
            var result = await ValidateAsync(ValidationContext<PPImageFormModel>.CreateWithOptions((PPImageFormModel)model, x => x.IncludeProperties(propertyName)));
            if (result.IsValid)
                return Array.Empty<string>();
            return result.Errors.Select(e => e.ErrorMessage);
        };
    }
}
