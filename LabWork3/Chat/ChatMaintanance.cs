using System.Collections.Generic;
using System.Net;

namespace Chat
{
    // 0 - вошёл в сеть
    // 1 - вышел из сети
    // 2 - обычное сообщение 
    class ChatMaintanance
    {
        public List<User> UsersList = new List<User>();

        public string AddUser(string Name, IPEndPoint IP)
        {
            UsersList.Add(new User(Name, IP));
            return Name;
        }

        public string NewChecker(string message)
        {
            if (message[0] == '0' && message.Length >= 2)
            {
                var name = message.Substring(1);
                return name + " has just joined us";
            }
            return "";
        }
    }
}