using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xamarin.Forms;

namespace Cyclist
{
	public partial class App : Application
	{
        public static Mapsui.Forms.MapView m_map { get; set; }

        public App ()
		{
			InitializeComponent();
            m_map = new Mapsui.Forms.MapView();

            MainPage = new NavigationPage(new Cyclist.MainPage());
		}

		protected override void OnStart ()
		{
			// Handle when your app starts
		}

		protected override void OnSleep ()
		{
			// Handle when your app sleeps
		}

		protected override void OnResume ()
		{
			// Handle when your app resumes
		}
	}
}
