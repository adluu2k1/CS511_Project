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
            RequestResponse,
            Text,
            Image
        }

        public int ID { get; private set; }
        public string Content { get; private set; }
        public string SenderName { get; private set; }
        public int SenderID { get; private set; }
        public int GroupID { get; private set; }
        public DateTime TimeSent { get; private set; }
        public MessageType Type { get; private set; }

        public Message(MessageType type, int senderID, int groupID, string content, DateTime time)
        {
            ID = new Random().Next(1, short.MaxValue);
            Type = type; Content = content;
            SenderID = senderID; GroupID = groupID;
            TimeSent = time;
            SenderName = App.GetClientDisplayName(SenderID);
        }
    }
}
