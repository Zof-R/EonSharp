using System;

namespace EonSharp.Logging
{
    public class LogMessage : EventArgs
    {
        public LogMessageType Type;
        public string Prefix;
        public string Message;
        public bool Handled;
        public DateTime TimeStamp;

        public LogMessage()
            : this(LogMessageType.Information, null, null)
        {
        }
        public LogMessage(string message)
            : this(LogMessageType.Information, null, message)
        {
        }
        public LogMessage(string prefix, string message)
            : this(LogMessageType.Information, prefix, message)
        {
        }
        public LogMessage(LogMessageType type, string prefix, string message)
        {
            this.Type = type;
            this.Prefix = prefix;
            this.Message = message;
            this.TimeStamp = DateTime.Now;
        }

        public override string ToString()
        {
            switch (Type)
            {
                case LogMessageType.Warning:
                    return TimeStamp.ToString("yyyy/MM/dd hh:mm:ss ") + "[WARNING]" + this.Prefix + this.Message;
                case LogMessageType.Error:
                    return TimeStamp.ToString("yyyy/MM/dd hh:mm:ss ") + "[ERROR]" + this.Prefix + this.Message;
                case LogMessageType.Information:
                default:
                    return TimeStamp.ToString("yyyy/MM/dd hh:mm:ss ") + this.Prefix + this.Message;
            }
        }
    }
}
