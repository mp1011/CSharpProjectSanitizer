﻿using ProjectSanitizer.Base.Extensions;
using System;
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
    }
}