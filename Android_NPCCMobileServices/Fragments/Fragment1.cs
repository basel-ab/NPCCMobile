
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using ModelLibrary;

namespace Android_NPCCMobileServices
{
	public class Fragment1 : Android.Support.V4.App.Fragment
	{
        TextView stationName;
        LayoutInflater InflaterMain;
		public override void OnCreate (Bundle savedInstanceState)
		{
			base.OnCreate (savedInstanceState);
		}

		public override View OnCreateView (LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
		{
            InflaterMain = inflater;
			View view = inflater.Inflate(Resource.Layout.Fragment1, container, false);
            stationName = view.FindViewById<TextView>(Resource.Id.txtFragment1);
            getWeatherAsync(24, 54);
			return view;
		}

        public async void getWeatherAsync(double l, double t)
        {
            Model mMd = new Model();
            WeatherResult wr = await mMd.GetWeatherReportAsync(l, t);

            stationName.Text = $"Location: {wr.weatherObservation.stationName} \nTemperature: {wr.weatherObservation.temperature}";
        }
        public override void OnResume()
        {
            base.OnResume();
            //string enc = ModelLibrary.Authentication.encrypt("basilt");
            //string dec = ModelLibrary.Authentication.Decrypt(enc);
            //string msg = $"enc: {enc}\ndec: {dec}";
            Toast.MakeText(InflaterMain.Context, "111", ToastLength.Long).Show();
        }
    }
}

