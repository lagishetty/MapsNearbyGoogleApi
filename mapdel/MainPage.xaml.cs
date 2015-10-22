using Newtonsoft.Json;
using SQLite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Devices.Geolocation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Maps;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=391641

namespace mapdel
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();

            this.NavigationCacheMode = NavigationCacheMode.Required;
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.
        /// This parameter is typically used to configure the page.</param>
        /// 
        public dynamic ite;
        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {

            var dbpath = ApplicationData.Current.LocalFolder.Path + "/mydb.db";
            var con = new SQLiteAsyncConnection(dbpath);
            await con.CreateTableAsync<Response>();

            var srt = e.Parameter as object;
            
            string querys = string.Format("select * from FuelReg where LoginID='{0}'", srt);
            List<FuelReg> mylists = await con.QueryAsync<FuelReg>(querys);

            if (mylists.Count == 1)
            {
                var sp = mylists[0];
                string querys1 = string.Format("select * from Response where CompanyId={0}", sp.FuelId);
                List<Response> mylist1 = await con.QueryAsync<Response>(querys1);
                var pit = mylist1[0];

                tb1.Text = pit.res;
            }


            

            string query = string.Format("select FuelID from FuelReg where FuelID='{0}'",srt);
            List<FuelReg> mylist = await con.QueryAsync<FuelReg>(query);
            if(mylist.Count==1)
            {
                var stp = mylist[0];

                ite = stp.FuelId;
            }
            


            // TODO: Prepare page for display here.

            // TODO: If your application contains multiple pages, ensure that you are
            // handling the hardware Back button by registering for the
            // Windows.Phone.UI.Input.HardwareButtons.BackPressed event.
            // If you are using the NavigationHelper provided by some templates,
            // this event is handled for you.
        }
        
        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            
          map1.MapServiceToken = "AutVzTJ62c1kL9L5ni3TBhm5nr1wGSSxL04Zrsw6DTTUeoTT7VqS1Vup4vFmSYaL";

       
            BasicGeoposition queryHint = new BasicGeoposition();
            
            queryHint.Latitude = 17.4271;
            queryHint.Longitude = 078.4466;
            Geopoint hwPoint1 = new Geopoint(queryHint);

            MapIcon mi = new MapIcon();
            map1.MapElements.Add(mi);
            map1.Center = hwPoint1;
            mi.Location = hwPoint1;
            map1.ZoomLevel = 15;
            map1.LandmarksVisible = true;
            await map1.TrySetViewAsync(hwPoint1, 16);
         
            Uri geocodeRequest = new Uri(string.Format("https://maps.googleapis.com/maps/api/place/nearbysearch/json?location=17.4271,078.4466&radius=500&types=petrol_bunk&key=AIzaSyCguNRWmAkk_Vctf0bePWiY1rIjM399Loo"));

            HttpClient client = new HttpClient();
            
            HttpResponseMessage response = await client.GetAsync(geocodeRequest);
            if(response.IsSuccessStatusCode==true)
            {
                var data = response.Content.ReadAsStringAsync();
                var listdata = JsonConvert.DeserializeObject<mapdel.Class1.RootObject>(data.Result);
                var item = JsonConvert.DeserializeObject<mapdel.Class1.Location>(data.Result);
                var itemmm = listdata.results;
                lv1.ItemsSource = listdata.results;
                lv1.DisplayMemberPath = "name";

                int i = 0;
         
                foreach(var dataob in listdata.results)
                {
                    var items=dataob.geometry;
                    var loct = items.location;

                    var sst=dataob.name;
                    
                    BasicGeoposition bgp = new BasicGeoposition();

                    AppBarButton rd = new AppBarButton();
                    StackPanel st = new StackPanel();

                    bgp.Latitude = loct.lat;
                    bgp.Longitude = loct.lng;
                    Geopoint hwPoint = new Geopoint(bgp);

                    MapControl.SetLocation(rd, hwPoint);
                    MapControl.SetNormalizedAnchorPoint(rd, new Point(0.5, 0.5));
                    
                    map1.Children.Add(rd);
                    
                   

                    rd.Click += (s, args) => rd_Click(s, args, hwPoint,sst);
                   
                    
                }
                
                
            }
        }



        void rd_Click(object sender, RoutedEventArgs e,Geopoint g,string bunkname)
        {
            var dbpath = ApplicationData.Current.LocalFolder.Path + "/mydb.db";
            var con = new SQLiteAsyncConnection(dbpath);


            StackPanel st = new StackPanel();
            RadioButton rb = new RadioButton();

        
            RadioButton rb1 = new RadioButton();

            st.Children.Add(rb);
            st.Children.Add(rb1);
            

            MapControl.SetLocation(st, g);
            map1.Children.Add(st);

            int va;
            if(rb.IsChecked==true)
            {
                va = 3;
            }
            else
            {
                va = 4;
            }
            Request rq = new Request();

            rq.CompanyId = ite;
            rq.value = va;
            rq.uID = ite;
            con.InsertAsync(rq);



            
        }

       

    }
}
