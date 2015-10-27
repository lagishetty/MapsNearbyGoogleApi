using SQLite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
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
    public sealed partial class Bottles : Page
    {
        public Bottles()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.
        /// This parameter is typically used to configure the page.</param>
        /// 
        public dynamic srr;
        public dynamic geo;
        public dynamic myuser;
        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            var dbpath = ApplicationData.Current.LocalFolder.Path + "/mydb.db";
            var con = new SQLiteAsyncConnection(dbpath);
            await con.CreateTableAsync<Request>();

            List<object> str1 = e.Parameter as List<object>;
            geo= str1[0];
            bname.Text = str1[1].ToString();
            bname_Address.Text = str1[2].ToString();

            srr = str1[3];
            myuser = str1[4];



            

        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {

            var dbpath = ApplicationData.Current.LocalFolder.Path + "/mydb.db";
            var con = new SQLiteAsyncConnection(dbpath);

            string va="";
            if (rb.IsChecked == true)
            {
                va = "3 litres";
            }
            else
            {
                va = "4 litres";
            }

            Windows.Devices.Geolocation.Geopoint gp = geo;

            var geolat=gp.Position.Latitude;
            var geolong=gp.Position.Longitude;
            var logst = srr;

            string query = string.Format("select * from FuelReg where LatValue='{0}' and LongValue='{1}'",geolat,geolong);
            List<FuelReg> mylist = await con.QueryAsync<FuelReg>(query);
            if (mylist.Count == 1)
            {
                
                var pos = mylist[0];

                Request rq = new Request();
                rq.uname = pos.CompanyName;
                rq.value = va;
                rq.CompanyId = pos.FuelId;
                rq.userresid = myuser;
 
                //FuelReg fr = new FuelReg();
                //fr.myval = va;


                await con.InsertAsync(rq);
                var msd = new MessageDialog("Requested fuel :" + va).ShowAsync();
            }
        }

        private void AppBarButton_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(Login));
        }
    }
}
