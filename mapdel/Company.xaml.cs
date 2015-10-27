using SQLite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
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
    public sealed partial class Company : Page
    {
        public Company()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.
        /// This parameter is typically used to configure the page.</param>
        /// 
        public dynamic srt;
        public dynamic sqq;
        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            var dbpath = ApplicationData.Current.LocalFolder.Path + "/mydb.db";
            var con = new SQLiteAsyncConnection(dbpath);
            await con.CreateTableAsync<Request>();

            var srt = e.Parameter as object;

            string query = string.Format("select * from FuelReg where LoginID={0}", srt);
            List<FuelReg> mylist1 = await con.QueryAsync<FuelReg>(query);
            if(mylist1.Count==1)
            {
                var mys = mylist1[0];
                string query1 = string.Format("select * from Request where CompanyID='{0}'", mys.FuelId);
                List<Request> mylist = await con.QueryAsync<Request>(query1);

                if (mylist.Count == 1)
                {
                    var sp = mylist[0];
                    tb1.Text = "Got request from user " + sp.value;

                }


            }
             sqq = srt;
                        

            

        } 

        public async void Button_Click(object sender, RoutedEventArgs e)
        {
            var dbpath = ApplicationData.Current.LocalFolder.Path + "/mydb.db";
            var con = new SQLiteAsyncConnection(dbpath);
            await con.CreateTableAsync<Response>();

            

            string query = string.Format("select * from FuelReg where LoginID={0}", sqq);
            List<FuelReg> mylist1 = await con.QueryAsync<FuelReg>(query);
            if (mylist1.Count == 1)
            {
                var mys = mylist1[0];
                
                string query1 = string.Format("select * from Request where CompanyID='{0}'", mys.FuelId);
                List<Request> mylist2 = await con.QueryAsync<Request>(query1);

                if (mylist2.Count == 1)
                {
                    var fes = mylist2[0];
                    string query2 = string.Format("select * from UserReg where Userid='{0}'", fes.userresid);
                    List<UserReg> mylist3 = await con.QueryAsync<UserReg>(query2);
                    if(mylist3.Count==1)
                    {
                        var pis = mylist3[0];
                        Response res = new Response();
                        res.UserResponseID = pis.UserID;
                        res.res = "Req accepted";
                        await con.InsertAsync(res);

                        respon.Text = "successfull";

                    }

                }
            }
           
        }

        private void AppBarButton_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(Login));
        }
    }
}
