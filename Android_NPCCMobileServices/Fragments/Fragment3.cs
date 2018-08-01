using System;
using Android.Views;
using Android.Widget;
using Android.Graphics;

namespace Android_NPCCMobileServices
{
	public class Fragment3 : Android.Support.V4.App.Fragment, View.IOnTouchListener
	{
		private FrameLayout mFragment4Container;
		private float mLastPosY;

		public override void OnCreate (Android.OS.Bundle savedInstanceState)
		{
			base.OnCreate (savedInstanceState);
		}

		public override Android.Views.View OnCreateView (Android.Views.LayoutInflater inflater, Android.Views.ViewGroup container, Android.OS.Bundle savedInstanceState)
		{
			View view = inflater.Inflate(Resource.Layout.Fragment3, container, false);

			Button button = view.FindViewById<Button>(Resource.Id.btnFragment4);
			mFragment4Container = view.FindViewById<FrameLayout>(Resource.Id.fragment4Container);

			var trans = Activity.SupportFragmentManager.BeginTransaction();
			trans.Add(mFragment4Container.Id, new Fragment4(), "Fragment4");
			trans.Commit();

			button.Click += (object sender, EventArgs e) => 
			{
				
				if (mFragment4Container.TranslationY + 2 >= mFragment4Container.Height)
				{
					var interpolator = new Android.Views.Animations.OvershootInterpolator(5);
					mFragment4Container.Animate().SetInterpolator(interpolator)
						.TranslationYBy(-200)
						.SetDuration(500);
				}
			};


			mFragment4Container.SetOnTouchListener(this);
			return view;
		}

		public bool OnTouch (View v, MotionEvent e)
		{
			switch (e.Action)
			{
			case MotionEventActions.Down:

				mLastPosY = e.GetY();
				return true;

			case MotionEventActions.Move:
				
				var currentPosition = e.GetY();
				var deltaY = mLastPosY - currentPosition;

				var transY = v.TranslationY;

				transY -= deltaY;

				if (transY < 0)
				{
					transY = 0;
				}

				v.TranslationY = transY;

				return true;
			
			default:
				return v.OnTouchEvent(e);
			}
		}
	}
}

