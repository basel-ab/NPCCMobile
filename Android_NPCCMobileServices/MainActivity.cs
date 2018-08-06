using System;
using Android.App;
using Android.Views;
using Android.Widget;
using Android.OS;
using SupportToolbar = Android.Support.V7.Widget.Toolbar;
using Android.Support.V4.Widget;
using System.Collections.Generic;
using SupportFragment = Android.Support.V4.App.Fragment;
using Xamarin.Essentials;

namespace Android_NPCCMobileServices
{
    [Activity ()]
    public class MainActivity : Android.Support.V7.App.AppCompatActivity
	{
        private SupportToolbar mToolbar;
		private MyActionBarDrawerToggle mDrawerToggle;
		private DrawerLayout mDrawerLayout;
        private ExpandableListView mLeftDrawer;
		private ListView mRightDrawer;
		private FrameLayout mFragmentContainer;
		private SupportFragment mCurrentFragment;
		private Fragment1 mFragment1;
		private Fragment2 mFragment2;
        private Fragment3 mFragment3;
		private Stack<SupportFragment> mStackFragments;

        protected override void OnCreate(Bundle savedInstanceState)
		{
            base.OnCreate (savedInstanceState);
	
			// Set our view from the "main" layout resource
			SetContentView (Resource.Layout.Main);

			mToolbar = FindViewById<SupportToolbar>(Resource.Id.toolbar);
			mDrawerLayout = FindViewById<DrawerLayout>(Resource.Id.drawer_layout);
            mLeftDrawer = FindViewById<ExpandableListView>(Resource.Id.left_drawer);
			mRightDrawer = FindViewById<ListView>(Resource.Id.right_drawer);
            mFragmentContainer = FindViewById<FrameLayout>(Resource.Id.fragmentContainer);

			SetSupportActionBar(mToolbar);
            setFragments();

            mLeftDrawer.Tag = 0;
            mRightDrawer.Tag = 1;

            clsLeftSideMenu objLeft = new clsLeftSideMenu(this);
            objLeft.fillLeftSideMenu(mLeftDrawer);

            clsRightMenu objRight = new clsRightMenu(this);
            objRight.fillRightSideMenu(mRightDrawer);
			

			mDrawerToggle = new MyActionBarDrawerToggle(
				this,							//Host Activity
				mDrawerLayout,					//DrawerLayout
				Resource.String.openDrawer,		//Opened Message
				Resource.String.closeDrawer		//Closed Message
			);

            mDrawerLayout.AddDrawerListener(mDrawerToggle);

			SupportActionBar.SetHomeButtonEnabled(true);
			SupportActionBar.SetDisplayShowTitleEnabled(true);
            SupportActionBar.SetDisplayHomeAsUpEnabled(true);
            SupportActionBar.SetLogo(Resource.Drawable.logo);

			mDrawerToggle.SyncState();

   //         if (savedInstanceState != null)
			//{
   //             if (savedInstanceState.GetString("DrawerState") == "Opened")
			//	{
			//		SupportActionBar.SetTitle(Resource.String.openDrawer);
			//	}

			//	else
			//	{
			//		SupportActionBar.SetTitle(Resource.String.closeDrawer);
			//	}
			//}

			//else
			//{
			//	//This is the first the time the activity is ran
			//	SupportActionBar.SetTitle(Resource.String.closeDrawer);
			//}
		}


        void setFragments()
        {
            mFragment1 = new Fragment1();
            mFragment2 = new Fragment2();
            mFragment3 = new Fragment3();

            mStackFragments = new Stack<SupportFragment>();

            var trans = SupportFragmentManager.BeginTransaction();

            trans.Add(Resource.Id.fragmentContainer, mFragment3, "Fragment3");
            trans.Hide(mFragment3);

            trans.Add(Resource.Id.fragmentContainer, mFragment2, "Fragment2");
            trans.Hide(mFragment2);

            trans.Add(Resource.Id.fragmentContainer, mFragment1, "Fragment1");
            trans.Commit();

            SupportActionBar.Title =" - " + Resources.GetString(Resource.String.Frag1Title);
            mCurrentFragment = mFragment1;
        }

			
		public override bool OnOptionsItemSelected (IMenuItem item)
		{		
			switch (item.ItemId)
			{

			case Android.Resource.Id.Home:
				//The hamburger icon was clicked which means the drawer toggle will handle the event
				//all we need to do is ensure the right drawer is closed so the don't overlap
				mDrawerLayout.CloseDrawer (mRightDrawer);
				mDrawerToggle.OnOptionsItemSelected(item);
				return true;

			case Resource.Id.action_help:
				if (mDrawerLayout.IsDrawerOpen(mRightDrawer))
				{
					//Right Drawer is already open, close it
					mDrawerLayout.CloseDrawer(mRightDrawer);
				}

				else
				{
					//Right Drawer is closed, open it and just in case close left drawer
					mDrawerLayout.OpenDrawer (mRightDrawer);
                        mDrawerLayout.CloseDrawer(mLeftDrawer);
				}

				return true;

			    case Resource.Id.action_fragment1:				
                    ShowFragment(mFragment1);
                    SupportActionBar.Title = " - " + Resources.GetString(Resource.String.Frag1Title);
                    return true;

			    case Resource.Id.action_fragment2:
                    ShowFragment(mFragment2);
                    SupportActionBar.Title = " - " + Resources.GetString(Resource.String.Frag2Title);
                    return true;

			    case Resource.Id.action_fragment3:
                    ShowFragment(mFragment3);
                    SupportActionBar.Title = " - " + Resources.GetString(Resource.String.Frag3Title);
                    return true;
                case Resource.Id.action_fragment5:
                    StartActivity(typeof(Fragment5));
                    SupportActionBar.Title = " - " + Resources.GetString(Resource.String.Frag5Title);
                    return true;

                case Resource.Id.action_Logout:
                    SecureStorage.Remove("oauth_token");
                    StartActivity(typeof(LoginActivity));
                    Finish();
                    return true;

			    default:
				    return base.OnOptionsItemSelected (item);
			}
		}
			
		private void ShowFragment (SupportFragment fragment)
		{
			if (fragment.IsVisible){
				return;
			}

			var trans = SupportFragmentManager.BeginTransaction();

			trans.SetCustomAnimations(Resource.Animation.slide_in, Resource.Animation.slide_out, Resource.Animation.slide_in, Resource.Animation.slide_out);

			fragment.View.BringToFront();
			mCurrentFragment.View.BringToFront();

			trans.Hide(mCurrentFragment);
			trans.Show(fragment);

			trans.AddToBackStack(null);
			mStackFragments.Push(mCurrentFragment);
			trans.Commit();

			mCurrentFragment = fragment;
		}
			
		public override void OnBackPressed ()
		{
			
			if (SupportFragmentManager.BackStackEntryCount > 0)
			{
				SupportFragmentManager.PopBackStack();
				mCurrentFragment = mStackFragments.Pop();
			}

			else
			{
				base.OnBackPressed();
			}				
		}

		public override bool OnCreateOptionsMenu (IMenu menu)
		{
			MenuInflater.Inflate (Resource.Menu.action_menu, menu);
			return base.OnCreateOptionsMenu (menu);
		}

		protected override void OnSaveInstanceState (Bundle outState)
		{
			if (mDrawerLayout.IsDrawerOpen((int)GravityFlags.Left))
			{
				outState.PutString("DrawerState", "Opened");
			}

			else
			{
				outState.PutString("DrawerState", "Closed");
			}

			base.OnSaveInstanceState (outState);
		}

		protected override void OnPostCreate (Bundle savedInstanceState)
		{
			base.OnPostCreate (savedInstanceState);
			mDrawerToggle.SyncState();
		}

		public override void OnConfigurationChanged (Android.Content.Res.Configuration newConfig)
		{
			base.OnConfigurationChanged (newConfig);
			mDrawerToggle.OnConfigurationChanged(newConfig);
		}
	}
}


