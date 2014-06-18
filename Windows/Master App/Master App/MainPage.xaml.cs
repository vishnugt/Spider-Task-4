using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Globalization;

namespace Master_App
{

    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
            internetcheck();
        }
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            
        }

        private async void internetcheck()
        {
            connectionlabel.Text = "Checking Internet Connection....";
            GetHttpResponse();
            await Task.Delay(TimeSpan.FromSeconds(2));
            if (result == "null")
            {
                connectionlabel.Text = "Connection failed, trying once again";
                await Task.Delay(TimeSpan.FromSeconds(8));
                if (result == "null")
                {
                    connectionlabel.Text = "Sorry, no Internet Connection";
                    return;
                }
                else 
                {
                    connectionlabel.Text = "Connection Established";
                    internetconnection = 1;
                }
            }
            connectionlabel.Text = "Connection Established";
            internetconnection = 1;
        }
        String result = "null";
        int internetconnection = 0;
        private async Task<String> GetHttpResponse()
        {

            HttpClient http = new System.Net.Http.HttpClient();
            HttpResponseMessage response = await http.GetAsync("http://localhost/spidertask4/connectiontest.php");
            result = await response.Content.ReadAsStringAsync();
            return null;
        }
        String data;
        String updatetextfromweb;
        private async Task<String> PostData()
        {

            HttpClient http2 = new System.Net.Http.HttpClient();
            HttpResponseMessage response2 = await http2.GetAsync("http://localhost/spidertask4/master.php" + "?data="+data);
            updatetextfromweb = await response2.Content.ReadAsStringAsync();
            return null;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            internetcheck();
            if (internetconnection == 0) 
            {
                updatestatus.Text = "Update Failed";
                return;
            }
            DateTime now = DateTime.Now;
            String date = now.ToString(@"MM/dd/yyyy HH\:mm\:ss.fff");
            data = txt1.Text + "*" + txt2.Text + "*" + txt3.Text + "*" + txt4.Text + "*" + txt5.Text + "*" + txt6.Text +"*"+date;
            PostData();
            updatestatus.Text = "Update Successful";
            
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            internetcheck();
        }
    }
}
