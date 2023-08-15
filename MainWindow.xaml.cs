using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Policy;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Web.WebView2.Core;


namespace WPFSample
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            
            webView.CoreWebView2Ready += Webview_CoreWebView2Ready;
            webView.NavigationCompleted += Loaded;
            InitializeAsync();

        }

        private void Webview_CoreWebView2Ready(object sender, EventArgs e)
        {
            
            webView.CoreWebView2.OpenDevToolsWindow();
        }

        private void Webview_WebMessageRecieved(object sender, CoreWebView2WebMessageReceivedEventArgs e)
        {
            string msg = e.TryGetWebMessageAsString();
            if (msg == "Join")
            {
                alma1 alma1 = new alma1();
                alma1.MainTask(webView);
            }
            
        }

        private void Loaded(object sender, CoreWebView2NavigationCompletedEventArgs e)
        {
            
            //webView.ExecuteScriptAsync("document.getElementById('roomNumber').value = \"alma\"");
            //webView.ExecuteScriptAsync("document.getElementById('goRoom').click();");
        }

        async void InitializeAsync()
        {
            await webView.EnsureCoreWebView2Async(null);
            webView.CoreWebView2.WebMessageReceived += Webview_WebMessageRecieved;

            //await webView.CoreWebView2.AddScriptToExecuteOnDocumentCreatedAsync("window.chrome.webview.postMessage(window.document.URL);");
            await webView.CoreWebView2.AddScriptToExecuteOnDocumentCreatedAsync("window.chrome.webview.addEventListener(\'message\', event => alert(event.data));");
            
            
            
            
            //await webView.ExecuteScriptAsync($"document.getElementById('roomNumber').value = \"{result.Item1}\"");
            //await webView.ExecuteScriptAsync($"document.getElementById('summonerName').value = \"{result.Item2}\"");
            //await webView.ExecuteScriptAsync($"document.getElementById('championId').value = \"{result.Item3}\"");
        }

        void UpdateAddressBar(object sender, CoreWebView2WebMessageReceivedEventArgs args)
        {

        }


        private void ButtonGo_Click(object sender, RoutedEventArgs e)
        {
        }



    }

}

