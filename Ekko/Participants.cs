using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1.ekko
{
    public class Participant
    {
        public object activePlatform { get; set; }
        public string cid { get; set; }
        public string game_name { get; set; }
        public string game_tag { get; set; }
        public bool muted { get; set; }
        public string name { get; set; }
        public string pid { get; set; }
        public string puuid { get; set; }
        public string region { get; set; }
    }

    public class Participants
    {
        public List<Participant>? participants { get; set; }
    }
    internal class GetParticipants
    {
        public GetParticipants() { }
        public async Task<string[]> GetNames(LeagueApi api)
        {
            var participants = await api.SendAsync(HttpMethod.Get, "/chat/v5/participants/champ-select");  //"/chat/v5/participants/champ-select"
            if (string.IsNullOrWhiteSpace(participants))
                return new string[0];

            var participantsJson = JsonConvert.DeserializeObject<Participants>(participants);
            if (participantsJson?.participants is null)
                return new string[0];

            var names = participantsJson.participants.Select(x => x.name).ToArray();
            return names;
        }
    }
}
