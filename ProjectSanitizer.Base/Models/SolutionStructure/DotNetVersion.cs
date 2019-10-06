using System;
using System.Linq;

namespace ProjectSanitizer.Models.SolutionStructure
{
    public enum DotNetType
    {
        Unknown,
        Framework,
        Client,
        Core,
        Standard
    }

    public class DotNetVersion
    {
        public DotNetType DotNetType { get; }
        public Version Version { get; }

        public DotNetVersion(DotNetType type, Version version)
        {
            DotNetType = type;
            Version = version;
        }

        public static DotNetVersion TryParse(string versionString)
        {
            if(string.IsNullOrEmpty(versionString))
            {
                return new DotNetVersion(DotNetType.Unknown, new Version(1, 0, 0));
            }
            else if(versionString.StartsWith("net"))
            {
                var leftRight = versionString.Split('-');
                var numbers = leftRight[0]
                                   .Substring(3)
                                   .Select(c => c.ToString())
                                   .ToArray();

                if (leftRight.Length == 2 && leftRight[1] == "client")
                {
                    return new DotNetVersion(DotNetType.Client,
                                                new Version(string.Join(".", numbers)));
                }
                else
                {
                    return new DotNetVersion(DotNetType.Framework,
                                                new Version(string.Join(".", numbers)));
                }
            }
            else 
                throw new System.NotImplementedException("Package handling for common and core not available yet");
        }

        public string ToPackagesConfigString()
        {
            switch(DotNetType)
            {
                case DotNetType.Framework:
                    if(Version.Build >= 0)
                        return $"net{Version.Major}{Version.Minor}{Version.Build}";
                    else
                        return $"net{Version.Major}{Version.Minor}";
                default:
                    throw new System.NotImplementedException("Package handling for common and core not available yet");
            }
        }

        public override string ToString()
        {
            return $".NET {DotNetType} v{Version}";
        }
    }
}
