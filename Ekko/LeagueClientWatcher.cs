using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleApp1.ekko
{
    public delegate void OnLeagueClient(LeagueClientWatcher watcher, LeagueClient client);
    public class LeagueClientWatcher
    {
        private readonly PlatformBase _platform;

        public LeagueClientWatcher()
        {
            _platform = RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ? new WindowsPlatform() :
                RuntimeInformation.IsOSPlatform(OSPlatform.OSX) ? new OsxPlatform() : new LinuxPlatform();
            Clients = new List<LeagueClient>();
        }

        public List<LeagueClient> Clients { get; }
        public event OnLeagueClient? OnLeagueClient;
        public event OnLeagueClient? OnLeagueClientExit;

        public async Task Observe(CancellationToken token = new())
        {
            while (true)
            {
                if (token.IsCancellationRequested)
                    return;

                var processes = Process.GetProcesses();

                foreach (var process in processes.Where(p => p.ProcessName is "LeagueClientUx" || p.ProcessName is "LeagueClientUx.exe"))//||p.ProcessName is "LeagueClient.exe" || p.ProcessName is "LeagueClient")) // Wine turns it into an .exe process.
                {
                    if (token.IsCancellationRequested)
                        return;
                    if (Clients.Any(d => d.Pid == process.Id))
                        continue;
                    var info = _platform.ExtractArguments(process.Id);
                    if (info is null)
                        continue;
                    var client = new LeagueClient(process.Id, info, process);
                    process.EnableRaisingEvents = true;
                    process.Exited += (_, _) =>
                    {
                        Clients.Remove(client);
                        OnLeagueClientExitInvoke(client);
                    };
                    Clients.Add(client);
                    OnLeagueClientInvoke(client);

                }
                
            }
        }


        private void OnLeagueClientExitInvoke(LeagueClient client)
        {
            OnLeagueClientExit?.Invoke(this, client);
        }

        private void OnLeagueClientInvoke(LeagueClient client)
        {
            OnLeagueClient?.Invoke(this, client);
        }
    }
}