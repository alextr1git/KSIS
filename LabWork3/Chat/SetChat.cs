using System.Collections.Generic;
using System.Net;

namespace Chat
{
    // 0 - вошёл в сеть
    // 1 - вышел из сети
    // 2 - обычное сообщение 
    class SetChat
    {
        public List<User> UserList = new List<User>();

        public string AddUser(string Name, IPEndPoint IP)
        {
            UserList.Add(new User(Name, IP));
            return Name;
        }

        public string WhatIsThis(string message, IPEndPoint adress)
        {
            if (message[0] == '0' && message.Length >= 2)
            {
                var name = message.Substring(1);
                //AddUser(name, adress);
                return name + " entered chat";
            }
            return "";
        }
    }
}