using System;
using UnityEngine;

public static class Logger
{
    public enum LogLevel
    {
        Log,
        Warning,
        Error,
    }
    
    public static void Debug(string message,LogLevel level = LogLevel.Log)
    {
        switch (level)
        {
            case LogLevel.Log:
                UnityEngine.Debug.Log(message);
                break;
            case LogLevel.Warning:
                UnityEngine.Debug.LogWarning(message);
                break;
            case LogLevel.Error:
                UnityEngine.Debug.LogError(message);
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(level), level, null);
        }
    }
}
