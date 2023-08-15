using ConsoleApp1.ekko;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    internal class Names
    {
        List<PlayerChampionSelection> names;
        private LeagueApi api;
        public Names(LeagueApi api)
        {
            this.api = api;
        }
        //public async Task<(List<PlayerChampionSelection>, long,string)> GetNames()
        public async Task<(string, string, int)> GetNames()
        {
            names = new List<PlayerChampionSelection>();
            var z = await api.SendAsync(HttpMethod.Get, "/lol-summoner/v1/current-summoner");
            var d = await api.SendAsync(HttpMethod.Get, "/lol-gameflow/v1/session");

            PlayerAccount account = JsonConvert.DeserializeObject<PlayerAccount>(z);
            Root root = JsonConvert.DeserializeObject<Root>(d);


            if (account == null || root == null)
            {
                return ("", "", 0);
            }
            
            GameData gameData = root.gameData;
            if (gameData == null)
            {
                return ("", "", 0);
            }
            string displayName = account.displayName;
            int champid = 0;
            try
            {
               champid  = root.gameData.playerChampionSelections.Where(x => x.summonerInternalName == displayName).ToArray()[0].championId;
            }
            catch (Exception ex)
            {
                return ("", "", 0);
            }
            
            var gameid = root.gameData.gameId;
            long summonerId = account.summonerId;
            int playerCount = gameData.playerChampionSelections.Length;
            if (gameData == null)//||  != 10)
            {
                return ("", "", 0);
            }

            int index = 10;
            for (int i = 0; i < playerCount; i++)
            {
                if (gameData.playerChampionSelections[i].summonerInternalName == account.displayName)
                {
                    index = i;
                }
            }
            if (index == 10)
            {
                return ("", "", 0);
            }
            int team;
            if (index >= playerCount / 2)
            {
                team = 1;
            }
            else
            {
                team = 0;
            }// if player index is >5 then team 2, if less then team 1
            for (int i = (team == 1 ? playerCount / 2 : 0); i < (team == 1 ? playerCount : playerCount / 2); i++)
            {
                //Console.WriteLine(gameData.playerChampionSelections[i]);
                names.Add(gameData.playerChampionSelections[i]);
            }
            //Console.WriteLine(gameData.playerChampionSelections[0]);

            // Access the extracted fields


            // Output the values
            //Console.WriteLine("Display Name: " + displayName);
            //Console.WriteLine("Summoner ID: " + summonerId);
            //return (names, gameData.gameId,team ==0? "Blue" : "Red");
            string roomname = gameid.ToString() + (team == 0 ? "Blue" : "Red");
            return (roomname, displayName, champid);
        }

        public static implicit operator Names((List<PlayerChampionSelection>, long, string) v)
        {
            throw new NotImplementedException();
        }
    }
    public class InGamePlayer
    {
        public string summonerInternalName { get; set; }
        public int championId { get; set; }
    }


    public class PlayerAccount
    {
        public string displayName { get; set; }
        public long summonerId { get; set; }
    }

    public class GameClient
    {
        public string observerServerIp { get; set; }
        public int observerServerPort { get; set; }
        public bool running { get; set; }
        public string serverIp { get; set; }
        public int serverPort { get; set; }
        public bool visible { get; set; }
    }

    public class PlayerChampionSelection
    {
        public int championId { get; set; }
        public int selectedSkinIndex { get; set; }
        public int spell1Id { get; set; }
        public int spell2Id { get; set; }
        public string summonerInternalName { get; set; }
    }

    public class GameData
    {
        public long gameId { get; set; }
        public string gameName { get; set; }
        public bool isCustomGame { get; set; }
        public string password { get; set; }
        public PlayerChampionSelection[] playerChampionSelections { get; set; }
    }

    public class QueueAvailability
    {
        public bool areFreeChampionsAllowed { get; set; }
        public string description { get; set; }
        public string detailedDescription { get; set; }
        public string gameMode { get; set; }
        public bool isRanked { get; set; }
        public bool isTeamBuilderManaged { get; set; }
        public string name { get; set; }
        public int numPlayersPerTeam { get; set; }
        public string queueAvailability { get; set; }
        public bool spectatorEnabled { get; set; }
        public string type { get; set; }
    }

    public class Root
    {
        public GameClient gameClient { get; set; }
        public GameData gameData { get; set; }
        public QueueAvailability queue { get; set; }
        public string phase { get; set; }
    }
}
