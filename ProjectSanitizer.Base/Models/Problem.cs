namespace ProjectSanitizer.Base.Models
{
    public class Problem
    {
        public Problem(string description)
        {
            Description = description;
        }

        public string Description { get; }
    }
}
