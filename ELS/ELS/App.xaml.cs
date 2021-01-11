using ELS.Data;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ELS
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            //var nav = new NavigationPage(new TodoListPage());
            //nav.BarBackgroundColor = (Color)App.Current.Resources["primaryGreen"];
            //nav.BarTextColor = Color.White;

            //MainPage = nav;

            MainPage = new MainPage();
        }

        static DataAccess database;
        public static DataAccess Database
        {
            get
            {
                if (database == null)
                {
                    database = new DataAccess();
                }
                return database;
            }
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
