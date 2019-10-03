using ProjectSanitizer.Base.Models.FileModels;
using System.IO;

namespace ProjectSanitizer.Models.FileModels
{
    public class MissingFile : RelativeFile
    {
        public MissingFile(VerifiedFolder projectDirectory, string relativePath) : 
            base(projectDirectory,relativePath)
        {
        }   
    }
}
