namespace ProjectSanitizer.Models.FileModels
{
    public interface IFile
    {
        string Name { get; }
        string FullName { get; }
        void Delete();
        bool Exists { get; }
    }
}
