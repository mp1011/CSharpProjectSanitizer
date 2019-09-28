using ProjectSanitizer.Base.Models.FileModels;
using ProjectSanitizer.Models.FileModels;

namespace ProjectSanitizer.Extensions
{
    public static class IFileExtensions
    {
        public static bool PathBeginsWith(this IFile file, VerifiedFolder folder)
        {
            if (folder == null)
                return false;

            var thisPath = file.FullName.ToLower();
            var folderPath = folder.FullName.ToLower();
            return thisPath.StartsWith(folderPath);
        }
    }
}
