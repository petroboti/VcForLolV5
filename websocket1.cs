using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Specialized;

namespace LobbyReveal
{

    
    class myWebhook
    {
       public void sendWebHook(string url,string msg,string username)
        {
            Http.Post(url, new NameValueCollection()
            {
                {
                    "username",
                    username
                },
                {
                    "content",
                    msg
                }
            });
        }
    }

    class Http
    {
        public static byte[] Post(string url, NameValueCollection pairs) 
        {
            using (WebClient webClient = new WebClient())
            {
                return webClient.UploadValues(url,pairs);
            }
        }
    }
}
