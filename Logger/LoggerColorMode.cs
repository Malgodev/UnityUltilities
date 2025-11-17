using UnityEngine;

namespace Malgo.Utilities.Logger
{
    public enum LoggerColorMode
    {
        NoColor = 0,
        CategoryColor = 1,
        LogLevelColor = 2,
        FullMessageColor = 3,
    }

    public enum LogLevel
    {
        None = 0,
        Debug = 1,
        Info = 2,
        Warning = 3,
        Error = 4,
        Critical = 5
    }
}
