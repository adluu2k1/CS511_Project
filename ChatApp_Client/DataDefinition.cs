using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ChatApp_Client
{
    public class ClientData
    {
        [JsonInclude]
        public int ID = -1;
        [JsonInclude]
        public string Username = "", Pass = "", DisplayName = "", AvatarPath = "";
    }

    public class GroupData
    {
        [JsonInclude]
        public int ID = -1;
        [JsonInclude]
        public string DisplayName = "", ClientsList = "";
    }
}
