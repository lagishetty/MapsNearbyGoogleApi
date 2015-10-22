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
        public dynamic ite;
        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            var dbpath = ApplicationData.Current.LocalFolder.Path + "/mydb.db";
            var con = new SQLiteAsyncConnection(dbpath);
            await con.CreateTableAsync<Request>();

            var srt = e.Parameter as object;

            string query = string.Format("select * from FuelReg where LoginID='{0}'", srt);
            List<FuelReg> mylist = await con.QueryAsync<FuelReg>(query);

            if(mylist.Count==1)
            {
                var sp = mylist[0];
                string query1 = string.Format("select * from Request where CompanyId={0}",sp.FuelId);
                List<Request> mylist1 = await con.QueryAsync<Request>(query1);
                var pit = mylist1[0];
                var ist = pit.value;
                tb1.Text = ist.ToString();
                //lv1.ItemsSource = ist;
                lv1.DisplayMemberPath = "value";
            }
            

            

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var dbpath = ApplicationData.Current.LocalFolder.Path + "/mydb.db";
            var con = new SQLiteAsyncConnection(dbpath);

            respon.Text = "successfull";
            Response rp = new Response();
            rp.res = respon.Text;


        }
    }
}
