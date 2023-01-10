using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatApp_Client
{
    public class Message
    {
        public enum MessageType
        {
            Text,
            Image
        }

        public string Content { get; private set; }
        public string Sender { get; private set; }
        public MessageType Type { get; private set; }

        public Message(MessageType type, string sender, string content)
        {
            Type = type; Sender = sender; Content = content;
        }
    }
}
