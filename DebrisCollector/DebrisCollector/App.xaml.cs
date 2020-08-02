using DebrisCollector.Pages;
using Microsoft.WindowsAzure.MobileServices;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace DebrisCollector
{
    public partial class App : Application
    {
        public static string DatabaseLocation = string.Empty;

        //Linking azure DB to app
        public static MobileServiceClient MobileService =
            new MobileServiceClient(
            "https://coasstdebristrackerapp.azurewebsites.net");

        public App()
        {
            InitializeComponent();

            MainPage = new HomePage();
        }


        public App(string databaseLocation)
        {
            InitializeComponent();
            MainPage = new HomePage();
            DatabaseLocation = databaseLocation;
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
