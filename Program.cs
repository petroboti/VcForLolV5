using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ConsoleApp1;
using LobbyReveal;
using Microsoft.VisualBasic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Microsoft.Web.WebView2.Core;
using Microsoft.Web.WebView2.Wpf;
using ConsoleApp1.ekko;
using System.Diagnostics;
using System.Linq;

public class Userinfo
{
    public string userinfo { get; set; }
}
public class currentPlatformId
{
    public string platform { get; set; }
}
public class Lol
{
    public string cpid { get; set; }
}

public class UserinfoActual
{
    public Lol lol { get; set; }

}
class gameData
{
    public string gamedata { get; set; }
}



public class alma1
{
    private string region;
    private string[] lobbyNames;
    public async Task MainTask(WebView2 webView)
    {
        var token = new CancellationTokenSource();
        var watcher = new LeagueClientWatcher();
        
        string roomname = "";
        string displayname = "";
        int champid = 0;
        
        watcher.OnLeagueClient += async (clientWatcher, client) =>
        {
            token.Cancel();
            var api1 = new LeagueApi(client.ClientAuthInfo.RiotClientAuthToken, client.ClientAuthInfo.RiotClientPort);
            var api2 = new LeagueApi(client.ClientAuthInfo.RemotingAuthToken, client.ClientAuthInfo.RemotingPort);
            var process = Process.GetProcesses();
            var p = process.Where(p => p.ProcessName is "LeagueClientUx" || p.ProcessName is "LeagueClientUx.exe").ToArray();
            await webView.ExecuteScriptAsync($"document.getElementById('MainText').innerHTML = \"{p.Length}\"");
            Regions regions = new Regions();
            region = await regions.GetRegion(api1);

            Names inGameNames = new Names(api2);
            (string, string, int) result;
            result = await inGameNames.GetNames();
            //await webView.ExecuteScriptAsync("document.getElementById('MainText').innerHTML = \"No game detected yet!\"");
            //var processes2 = processes.Where(p => p.ProcessName == "League of Legends" || p.ProcessName == "League of Legends.exe").ToArray().Length;
            if(result == ("", "", 0))
            {
                webView.Reload();
            }
            else
            {
                await webView.ExecuteScriptAsync($"document.getElementById('roomNumber').value = \"{result.Item1}\"");
                await webView.ExecuteScriptAsync($"document.getElementById('summonerName').value = \"{result.Item2}\"");
                await webView.ExecuteScriptAsync($"document.getElementById('championId').value = \"{result.Item3}\"");
                await webView.ExecuteScriptAsync("document.getElementById('goRoom').click();");
            }
            //await webView.ExecuteScriptAsync("document.getElementById('MainText').innerHTML = \"VoiceChat\"");

            
        };
        //Console.WriteLine("Waiting for league client!");
        await watcher.Observe(token.Token);
        //await webView.ExecuteScriptAsync($"document.getElementById('championId').value = \"geci\"");
        await Task.Delay(-1);
    }
}