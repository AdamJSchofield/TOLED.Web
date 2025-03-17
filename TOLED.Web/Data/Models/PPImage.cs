using Microsoft.EntityFrameworkCore;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp.Processing.Processors.Dithering;
using System.ComponentModel.DataAnnotations.Schema;

namespace TOLED.Web.Data.Models
{
    public class PPImage
    {
        public int Id { get; set; }
        public ApplicationUser Owner { get; set; } = default!;
        public ICollection<ApplicationUser> Users { get; set; } = default!;
        public string Name { get; set; } = default!;
        public bool IsActive { get; set; } = false;
        public virtual byte[] RawData { get; set; } = default!;
        public virtual byte[] DisplayData { get; set; } = default!;
        public int Frames { get; set; } = 1;
        public PPImageMutateOptions MutateOptions { get; set; } = new PPImageMutateOptions();
    }

    [Owned]
    public class PPImageMutateOptions
    {
        public int Id { get; set; }
        public ResizeMode Mode { get; set; } = ResizeMode.Pad;
        public AnchorPositionMode Position { get; set; } = AnchorPositionMode.Center;
        public int Width { get; set; } = 128;
        public int Height { get; set; } = 64;
        public FlipMode FlipMode { get; set; } = FlipMode.None;
        public RotateMode RotateMode { get; set; } = RotateMode.None;
        public string PadColor { get; set; } = Color.Black.ToString();
        public bool InvertColors { get; set; } = false;
        public string DitheringAlglorithm { get; set; } = nameof(KnownDitherings.FloydSteinberg);
        public bool HasPremium { get; set; } = false;

        [NotMapped]
        public static readonly Dictionary<string, IDither> DitheringAlgorithms = new()
        {
            { "Ordered3x3", KnownDitherings.Ordered3x3 },
            { "Bayer2x2", KnownDitherings.Bayer2x2 },
            { "Bayer4x4", KnownDitherings.Bayer4x4 },
            { "Bayer8x8", KnownDitherings.Bayer8x8 },
            { "Bayer16x16", KnownDitherings.Bayer16x16 },
            { "Burks", KnownDitherings.Burks },
            { "Stucki", KnownDitherings.Stucki },
            { "JarvisJudiceNinke", KnownDitherings.JarvisJudiceNinke },
            { "StevensonArce", KnownDitherings.StevensonArce },
            { "SierraLite", KnownDitherings.SierraLite },
            { "Sierra2", KnownDitherings.Sierra2 },
            { "Sierra3", KnownDitherings.Sierra3 },
            { "FloydSteinberg", KnownDitherings.FloydSteinberg }
        };
    }
}
