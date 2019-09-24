using System;

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

        public override string ToString()
        {
            return $".NET {DotNetType} v{Version}";
        }
    }
}
