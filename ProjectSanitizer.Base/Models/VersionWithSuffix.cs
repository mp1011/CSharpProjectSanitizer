using ProjectSanitizer.Base.Extensions;
using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace ProjectSanitizer.Base.Models
{
    public class VersionWithSuffix : IComparable<VersionWithSuffix>
    {
        public Version Version { get; }
        public string Suffix { get; }

        public int SuffixNumber { get; }

        public bool HasSuffix => !string.IsNullOrEmpty(Suffix);

        public VersionWithSuffix(string versionNumber)
        {
            if (string.IsNullOrEmpty(versionNumber))
                Version = new Version(0, 0);
            else
            {
                var split = versionNumber.Split('-');
                Version = Version.Parse(split[0]);

                if (split.Length == 2)
                {
                    Suffix = "-" + split[1];
                    SuffixNumber = Regex.Replace(Suffix, @"[\D]*", "").TryParseInt();
                }
            }
        }

        public override string ToString()
        {
            if (Version.Build == 0 && Version.Revision == 0)
                return Version.ToString(2) + (Suffix ?? "");
            else
                return Version.ToString() + (Suffix ?? "");
        }

        int IComparable<VersionWithSuffix>.CompareTo(VersionWithSuffix other)
        {
            var versionComp = Version.CompareTo(other.Version);
            if (versionComp != 0)
                return versionComp;

            if (HasSuffix && !other.HasSuffix)
                return -1;
            else if (!HasSuffix && other.HasSuffix)
                return 1;
            else
                return SuffixNumber - other.SuffixNumber;
        }

        public override bool Equals(object obj)
        {
            if (obj is VersionWithSuffix otherVersion)
            {
                var numbers = GetVersionNumbers();
                var otherNumbers = otherVersion.GetVersionNumbers();
                foreach (var ix in Enumerable.Range(0, otherNumbers.Length))
                {
                    if (numbers[ix] != otherNumbers[ix])
                        return false;
                }

                return Suffix == otherVersion.Suffix;
            }
            else
                return false;
        }

        private int[] GetVersionNumbers()
        {
            //we count a "missing" number the same as 0
            return new int[] { Version.Major,
                                Math.Max(0, Version.Minor),
                                Math.Max(0, Version.Build),
                                Math.Max(0, Version.Revision),
                                SuffixNumber };
        }

        public override int GetHashCode()
        {
            return Version.GetHashCode();
        }

        public static bool operator ==(VersionWithSuffix first, VersionWithSuffix second)
        {
            return first.Equals(second);
        }

        public static bool operator !=(VersionWithSuffix first, VersionWithSuffix second)
        {
            return !first.Equals(second);
        }
    }
}
