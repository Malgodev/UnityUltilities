using System;
using System.Collections.Generic;
using UnityEngine;

namespace Malgo.Utilities.Logger
{
    public static class Logger
    {
        public static LogLevel MinimumLogLevel = LogLevel.Info;
        public static LoggerColorMode ColorMode = LoggerColorMode.FullMessageColor;

        public static Dictionary<LogLevel, Color> LogLevelColors = new Dictionary<LogLevel, Color>
        {
            { LogLevel.Debug, Color.gray },
            { LogLevel.Info, Color.white },
            { LogLevel.Warning, Color.yellow },
            { LogLevel.Error, Color.red },
            { LogLevel.Critical, new Color(0.5f, 0f, 0f) }
        };
        
        public static void Log(string message, LogLevel level = LogLevel.Info, string category = "General")
        {
            #if ENABLE_LOGGING
            if (level < MinimumLogLevel) return;
            
            Color logColor = LogLevelColors[level];
            string colorHex = ColorUtility.ToHtmlStringRGB(logColor);

            string fullMessage = "";

            switch (ColorMode)
            {
                case LoggerColorMode.FullMessageColor:
                    fullMessage = $"[<color=#{colorHex}>{level}</color>] {message}";
                    break;
                default:
                    fullMessage = $"[{level}] {message}";
                    break;
            }

            switch (level)
            {
                case LogLevel.Warning:
                    Debug.LogWarning(fullMessage);
                    break;
                case LogLevel.Error:
                case LogLevel.Critical:
                    Debug.LogError(fullMessage);            
                    break;
                default:
                    Debug.Log(fullMessage);
                    break;
            }
            #endif
        }

        public static void LogDebug(string message, string category = "General")
        {
            Log(message, LogLevel.Info, category);
        }
        
        public static void LogInfo(string message, string category = "General")
        {
            Log(message, LogLevel.Info, category);
        }
        
        public static void LogWarning(string message, string category = "General")
        {
            Log(message, LogLevel.Info, category);
        }
        
        public static void LogError(string message, string category = "General")
        {
            Log(message, LogLevel.Error, category);
        }
        
        public static void LogCritical(string message, string category = "General")
        {
            Log(message, LogLevel.Info, category);
        }
    }
}
