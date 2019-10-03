using System;
using System.Linq;

namespace ProjectSanitizer.Models.SolutionStructure
{
    public enum DotNetType
    {
        Unknown,
        Framework,
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
            if(versionString.StartsWith("net"))
            {
                var numbers = versionString
                                .Substring(3)
                                .Select(c => c.ToString())
                                .ToArray();

                return new DotNetVersion(DotNetType.Framework,
                                            new Version(string.Join(".", numbers)));
            }
            else 
                throw new System.NotImplementedException("Package handling for common and core not available yet");
        }

        public string ToPackagesConfigString()
        {
            switch(DotNetType)
            {
                case DotNetType.Framework:
                    return $"net{Version.Major}{Version.Minor}{Version.Build}";
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
