using System;
using Malgo.Utilities.DataStructure;
using UnityEngine;

namespace Malgo.Utilities.Logger
{
    [CreateAssetMenu(fileName = "LoggerData", menuName = "MalgoUtilities/Logger/LoggerData")]
    public class LoggerData : ScriptableObject
    {
        private bool isMainLogger = false;
        
        public SerializableDictionary<LogLevel, Color> LogColor 
            = new SerializableDictionary<LogLevel, Color>();
        
        public LogLevel MinimumLogLevel = LogLevel.Debug;
        public LoggerColorMode ColorMode = LoggerColorMode.FullMessageColor;

        private void OnEnable()
        {
            LogColor[LogLevel.Debug] = Color.gray;
            LogColor[LogLevel.Info] = Color.white;
            LogColor[LogLevel.Warning] = Color.yellow;
            LogColor[LogLevel.Error] = Color.red;
            LogColor[LogLevel.Critical] = new Color(0.5f, 0f, 0f);
        }

        public void ApplyConfig()
        {
            Logger.MinimumLogLevel = MinimumLogLevel;
            Logger.ColorMode = ColorMode;
            Logger.LogLevelColors = LogColor.ToDictionary();
        }
    }
}
