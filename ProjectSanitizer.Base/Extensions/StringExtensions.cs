namespace ProjectSanitizer.Base.Extensions
{
    public static class StringExtensions
    {
        public static int TryParseInt(this string text)
        {
            int parsed;
            if (int.TryParse(text, out parsed))
                return parsed;
            else
                return 0;
        }
    }
}
