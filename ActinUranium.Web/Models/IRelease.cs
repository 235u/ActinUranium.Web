using System;

namespace ActinUranium.Web.Models
{
    public interface IRelease
    {
        DateTime ReleaseDate { get; }

        Image GetPrimaryImage();
    }
}
