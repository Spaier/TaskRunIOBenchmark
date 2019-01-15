using System.IO;

namespace TaskRunIO
{
    public static class TestImage
    {
        public static readonly string ImageName = Path.GetFullPath(@"../../../IMG_20180905_220918");

        public const string ImageExt = ".jpg";

        public static readonly string ImagePath = ImageName + ImageExt;
    }
}
