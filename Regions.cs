using ConsoleApp1.ekko;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    public enum Region
    {
        UNKNOWN,
        EUW,
        EUNE,
        NA,
        TR,
        OCE,
        LAN,
        LAS,
        RU,
        BR,
        JP,
        SG,
        TW,
        TH,
        VN,
        PH
    }

    public enum Platform
    {
        UNKNOWN,
        EUW1,
        EUN1,
        NA1,
        TR1,
        OC1,
        LA1,
        LA2,
        RU,
        BR1,
        JP1,
        SG2,
        TW2,
        TH2,
        VN2,
        PH2
    }
    internal class Regions
    {
        public Regions() { }
        public async Task<string> GetRegion(LeagueApi api)
        {
            var z = await api.SendAsync(HttpMethod.Get, "/rso-auth/v1/authorization/userinfo");
            if (string.IsNullOrWhiteSpace(z))
                return "";

            var resp1 = JsonConvert.DeserializeObject<Userinfo>(z);
            if (resp1 is null)
            {
                return "";
            }
            
            var resp2 = JsonConvert.DeserializeObject<UserinfoActual>(resp1.userinfo);
            if (resp2 is null)
            {
                return "";
            }
            
            var reg = Enum.TryParse(resp2.lol.cpid, out Platform region);
            var reg2 = (Region)region;

            if (!reg)
            {
                Console.WriteLine("Could not figure out region. Setting EUW");
                reg2 = Region.EUW;
            }
            return reg2.ToString();
        }
    }
}
