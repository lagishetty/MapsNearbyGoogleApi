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
    public sealed partial class Login : Page
    {
        public Login()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.
        /// This parameter is typically used to configure the page.</param>
        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            var dbpath = ApplicationData.Current.LocalFolder.Path + "/mydb.db";
            var con = new SQLiteAsyncConnection(dbpath);
            await con.CreateTableAsync<UserReg>();
        }
        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            var dbpath = ApplicationData.Current.LocalFolder.Path + "/mydb.db";
            var con = new SQLiteAsyncConnection(dbpath);
            await con.CreateTableAsync<UserReg>();
            await con.CreateTableAsync<LoginCheck>();
            UserReg m = new UserReg();

            LoginCheck lk = new LoginCheck();
            lk.LoginName = text_reg.Text;
            lk.Password = text_password.Password;
            lk.IsCompany = false;
            m.Name = text_regname.Text;


            string r = "";
            if (radio_male.IsChecked == true)
            {
                r = "Male";

            }
            else
            {
                r = "Female";

            }
            m.Gender = r;
            m.State = ((ComboBoxItem)combo_box.SelectedItem).Content.ToString();
            await con.InsertAsync(lk);
            m.LoginId = lk.LoginId;

            await con.InsertAsync(m);




            MessageDialog md = new MessageDialog("Success");
            await md.ShowAsync();
        }

        private async void Button_Click_1(object sender, RoutedEventArgs e)
        {

            var dbpath = ApplicationData.Current.LocalFolder.Path + "/mydb.db";
            var con = new SQLiteAsyncConnection(dbpath);
            try
            {
                LoginCheck lk = new LoginCheck();

                string query = string.Format("select LoginName,Password,IsCompany,LoginId from LoginCheck where LoginName='{0}' and Password='{1}'", text_user.Text, text_pass.Password);

                List<LoginCheck> mylist = await con.QueryAsync<LoginCheck>(query);


                if (mylist.Count >= 1)
                {
                    lk = mylist[0];
                    if (lk.IsCompany == true)
                    {
                        this.Frame.Navigate(typeof(Company), lk.LoginId);
                    }
                    else
                    {
                        this.Frame.Navigate(typeof(MainPage), lk.LoginId);
                    }

                }
                else
                {
                    var msg = new MessageDialog("no user").ShowAsync();
                }


            }
            catch (Exception ex)
            {
                var msd = new MessageDialog("" + ex).ShowAsync();
            }

        }

        private async void Fuel_button_Click(object sender, RoutedEventArgs e)
        {
            var dbpath = ApplicationData.Current.LocalFolder.Path + "/mydb.db";
            var con = new SQLiteAsyncConnection(dbpath);
            await con.CreateTableAsync<FuelReg>();

            LoginCheck lk = new LoginCheck();
            FuelReg f = new FuelReg();
            lk.LoginName = fuel_name.Text;
            lk.Password = fuel_password.Password;
            lk.IsCompany = true;
            f.CompanyName = company_name.Text;
            f.CompanyAddress = company_address.Text;
            await con.InsertAsync(lk);
            f.LoginId = lk.LoginId;
            await con.InsertAsync(f);

            MessageDialog md = new MessageDialog("Success");
            await md.ShowAsync();
        }
    }
}
