using System;

namespace MarkDoc.Core
{
  public interface IMarkDocLogger
  {
    public enum LogType
    {
      Info,
      Error,
      Warning,
      Debug
    }

    public readonly struct LogMessage
    {
      /// <summary>
      /// Log type
      /// </summary>
      public LogType Type { get; }

      /// <summary>
      /// Log message
      /// </summary>
      public string Message { get; }

      /// <summary>
      /// Log time
      /// </summary>
      public DateTime Time { get; }

      /// <summary>
      /// Default constructor
      /// </summary>
      public LogMessage(LogType type, string message)
      {
        Type = type;
        Message = message;
        Time = DateTime.Now;
      }
    }

    event EventHandler<LogMessage> NewLog;

    void Info(string message);
    void Debug(string message);
    void Error(string message);
    void Warning(string message);
  }
}