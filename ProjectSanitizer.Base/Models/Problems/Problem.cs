using ProjectSanitizer.Services;

namespace ProjectSanitizer.Base.Models
{
    public abstract class Problem
    {
        public abstract SmartStringBuilder Description { get; }

        public override string ToString()
        {
            return Description.ToString();
        }
    }

    public abstract class Problem<T> : Problem
    {
        public T Item { get; }

        public Problem(T item)
        {
            Item = item;
        }
    }
}
