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
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Maps;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace mapdel
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class RegisterCompany : Page
    {
        public RegisterCompany()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.
        /// This parameter is typically used to configure the page.</param>
        /// 
        public dynamic mygeo;
        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            var dbpath = ApplicationData.Current.LocalFolder.Path + "/mydb.db";
            var con = new SQLiteAsyncConnection(dbpath);
            await con.CreateTableAsync<FuelReg>();



            map1.MapServiceToken = "AutVzTJ62c1kL9L5ni3TBhm5nr1wGSSxL04Zrsw6DTTUeoTT7VqS1Vup4vFmSYaL";


            Geolocator gl = new Geolocator();
            Geoposition gp = await gl.GetGeopositionAsync();

            BasicGeoposition queryHint = new BasicGeoposition();

            queryHint.Latitude = gp.Coordinate.Latitude;
            queryHint.Longitude = gp.Coordinate.Longitude;
            Geopoint hwPoint1 = new Geopoint(queryHint);
            MapIcon mi = new MapIcon();
            map1.MapElements.Add(mi);
            map1.Center = hwPoint1;
            map1.ZoomLevel = 16;


            Uri geocodeRequest = new Uri(string.Format("https://maps.googleapis.com/maps/api/place/nearbysearch/json?location={0},{1}&radius=1500&type=gas_station&key=AIzaSyBgBxFhipo5V0H2TL3fB9vc6KyvclffFXI", hwPoint1.Position.Latitude, hwPoint1.Position.Longitude)); // 17.4271,078.4466     AIzaSyCguNRWmAkk_Vctf0bePWiY1rIjM399Loo

            HttpClient client = new HttpClient();

            HttpResponseMessage response = await client.GetAsync(geocodeRequest);
            if (response.IsSuccessStatusCode == true)
            {
                var data = response.Content.ReadAsStringAsync();
                var listdata = JsonConvert.DeserializeObject<mapdel.Class1.RootObject>(data.Result);

                foreach (var dataob in listdata.results)
                {
                    var items = dataob.geometry;
                    var loct = items.location;

                    var sst = dataob.name;

                    var bzp = dataob.vicinity;

                    BasicGeoposition bgp = new BasicGeoposition();

                    AppBarButton rd = new AppBarButton();
                    rd.Foreground = new SolidColorBrush(Colors.Black);

                    TextBox tbx = new TextBox();
                    tbx.Text = dataob.name;
                    tbx.FontSize = 12;


                    StackPanel st = new StackPanel();
                    st.Background = new SolidColorBrush(Colors.Black);
                    Uri stq = new Uri(dataob.icon);

                    BitmapIcon bmi = new BitmapIcon();
                    bmi.UriSource = stq;
                    bmi.Height = 20;
                    bmi.Width = 20;

                    bgp.Latitude = loct.lat;
                    bgp.Longitude = loct.lng;
                    Geopoint hwPoint = new Geopoint(bgp);

                    MapControl.SetLocation(tbx, hwPoint);
                    MapControl.SetLocation(rd, hwPoint);
                    MapControl.SetLocation(bmi, hwPoint);

                    MapControl.SetNormalizedAnchorPoint(tbx, new Point(0.5, 0.5));
                    MapControl.SetNormalizedAnchorPoint(rd, new Point(0.5, 0.5));
                    MapControl.SetNormalizedAnchorPoint(bmi, new Point(0.5, 0.5));

                    map1.Children.Add(tbx);
                    map1.Children.Add(rd);
                    map1.Children.Add(bmi);


                    
                    rd.Click += (s, args) => rd_Click(s, args, hwPoint, sst, bzp);


                }
            }
        }

        async void rd_Click(object sender, RoutedEventArgs e, Geopoint g, string bunkname, string address)
        {
            var dbpath = ApplicationData.Current.LocalFolder.Path + "/mydb.db";
            var con = new SQLiteAsyncConnection(dbpath);
            await con.CreateTableAsync<FuelReg>();

            mygeo = g;

            //await con.InsertAsync(fr);

            //btn.Click += (sender, e) => btn_Click(sender, e, gp);

        }

       

        private async void btn_Click(object sender, RoutedEventArgs e)
        {
            var dbpath = ApplicationData.Current.LocalFolder.Path + "/mydb.db";
            var con = new SQLiteAsyncConnection(dbpath);
            await con.CreateTableAsync<FuelReg>();
            await con.CreateTableAsync<LoginCheck>();
            LoginCheck lk = new LoginCheck();
            FuelReg f = new FuelReg();


            lk.LoginName = fuel_name.Text;
            lk.Password = fuel_password.Password;
            lk.IsCompany = true;
            f.CompanyName = company_name.Text;


            Geopoint gg = mygeo;

            f.LatValue = gg.Position.Latitude;
            f.LongValue = gg.Position.Longitude;


            await con.InsertAsync(lk);
            f.LoginId = lk.LoginId;
            await con.InsertAsync(f);

            MessageDialog md = new MessageDialog("Success");
            await md.ShowAsync();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(Login));
        }

       
    }
}
