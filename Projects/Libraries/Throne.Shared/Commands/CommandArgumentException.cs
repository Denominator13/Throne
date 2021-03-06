using System;
using System.Runtime.Serialization;

namespace Throne.Framework.Commands
{
    [Serializable]
    public class CommandArgumentException : ArgumentException
    {
        public CommandArgumentException()
        {
        }

        public CommandArgumentException(string message)
            : base(message)
        {
        }

        public CommandArgumentException(string message, Exception inner)
            : base(message, inner)
        {
        }

        protected CommandArgumentException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}