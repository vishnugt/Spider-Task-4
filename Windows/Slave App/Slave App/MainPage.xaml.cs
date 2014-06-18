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

namespace Slave_App
{
    public sealed partial class MainPage : Page
    {
        Windows.Storage.ApplicationDataContainer localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
        Windows.Storage.StorageFolder localFolder = Windows.Storage.ApplicationData.Current.LocalFolder;
            
        public  MainPage()
        {
            this.InitializeComponent();
           checkupdated();
           
            
        }
        private async void checkupdated() 
        {
            int internalupdatenumber = (int)localSettings.Values["updatenumber"];
            internetcheck();
            await Task.Delay(TimeSpan.FromSeconds(3));
            if (internetconnection != 1) 
            {
                await Task.Delay(TimeSpan.FromSeconds(5));
                connectionlabel.Text = "Retriving Internal Data";
                await Task.Delay(TimeSpan.FromSeconds(2));
                if (localSettings.Values["wholedata"] == null) 
                {
                    connectionlabel.Text = "As this is the first time app is opened, there is no internal data.";
                    return;
                }
                wholedata = (string) localSettings.Values["wholedata"];
                parts1 = wholedata.Split('*');
                for (int i = 1; i < parts1.Length; i++)
                {
                    if (i % 7 == 0)
                        listView1.Items.Add("Data uploaded on " + parts1[i]);

                }
                fetchinternaldata();
                return;
            }
            
            connectionlabel.Text = "Checking whether the app is Up-to-date";
            updatecheck();
            await Task.Delay(TimeSpan.FromSeconds(2));
            int updatenumberinint = Convert.ToInt32(updatenumber);
            if (localSettings.Values["updatenumber"]==null)
            {
                connectionlabel.Text = "App is opened for the first time";
                await Task.Delay(TimeSpan.FromSeconds(1));
                connectionlabel.Text = updatenumber + " number of posts available.  Fetching Data...";
                await Task.Delay(TimeSpan.FromSeconds(2));
                fetchdata();

            }

            else if (internalupdatenumber != updatenumberinint) 
            {
                await Task.Delay(TimeSpan.FromSeconds(2));
                int noofposts = updatenumberinint - internalupdatenumber;
                connectionlabel.Text = noofposts + " number of posts since your last visit.  Fetching Data...";
                fetchdata();

            }

            else
            {
                connectionlabel.Text = "App is up-to-date";
                fetchinternaldata();
            }
            

        }
        public void fetchinternaldata() 
        {

        }
        String[] parts1;

        private async void fetchdata() 
        {
            retrivewholedata();
            await Task.Delay(TimeSpan.FromSeconds(3));
            String[] parts = new String[100];
            String[][] daily = new String[100][];
            //Char[] delim1 = ("###").ToCharArray();
            Char[] delim2 = ("***").ToCharArray();
            //String[] wholeschedule = wholedata.Split(delim1);
           // for (int i = 0; i <20; i++)
            //{
               // parts[i] = wholesdata[i].Split(delim1)[0];
               // listView1.Items.Add(parts[i]);
                
//            } 
            parts1 = wholedata.Split('*');
            for (int i = 1; i < parts1.Length; i++) 
            {
                if(i%7==0)
                listView1.Items.Add("Data uploaded on " + parts1[i]);

            }
            

        }

        private async void internetcheck()
        {
            connectionlabel.Text = "Checking Internet Connection....";
            GetHttpResponse();
            await Task.Delay(TimeSpan.FromSeconds(2));
            if (result == "null")
            {
                connectionlabel.Text = "Connection failed, trying once again";
                await Task.Delay(TimeSpan.FromSeconds(5));
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
        String updatenumber;
        String wholedata;
        private async Task<String> updatecheck()
        {

            HttpClient http = new System.Net.Http.HttpClient();
            HttpResponseMessage response = await http.GetAsync("http://localhost/spidertask4/mastercounter.php");
            updatenumber = await response.Content.ReadAsStringAsync();
            return null;
        }
        private async Task<String> retrivewholedata()
        {

            HttpClient http = new System.Net.Http.HttpClient();
            HttpResponseMessage response = await http.GetAsync("http://localhost/spidertask4/data.php");
            wholedata = await response.Content.ReadAsStringAsync();
            return null;
        }
        
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            checkupdated();
        }

        private void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int index = listView1.SelectedIndex;
            String labeltext = parts1[((index * 7) + 7)] + "  Update\n\n\n";
            labeltext += "1st Hour is " + parts1[(index * 7) + 1] + "\n\n";
            labeltext += "2nd Hour is " + parts1[(index * 7) + 2] + "\n\n";
            labeltext += "3rd Hour is " + parts1[(index * 7) + 3] + "\n\n";
            labeltext += "4th Hour is " + parts1[(index * 7) + 4] + "\n\n";
            labeltext += "5th Hour is " + parts1[(index * 7) + 5] + "\n\n";
            labeltext += "Some Addition Information : \n\t\t\t" + parts1[(index * 7) + 6] + "\n\n";
            label2.Text = labeltext;
            localSettings.Values["wholedata"] = wholedata;
        }
    }
}
