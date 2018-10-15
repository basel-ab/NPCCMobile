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
    [Activity()]
    public class HomeActivity : Android.Support.V7.App.AppCompatActivity
    {
        //Declarations 
        private SupportToolbar mToolbar;
        private DrawerToggle mDrawerToggle;
        private DrawerLayout mDrawerLayout;
        private ExpandableListView mLeftDrawer;
        private FrameLayout mFragmentContainer;
        private SupportFragment mCurrentFragment;
        private Fragment1 mFragment1;
        private Stack<SupportFragment> mStackFragments;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Main);

            //toolbar
            mToolbar = FindViewById<SupportToolbar>(Resource.Id.toolbar);
            SetSupportActionBar(mToolbar);

            SupportActionBar.SetHomeButtonEnabled(true);
            SupportActionBar.SetDisplayShowTitleEnabled(false);
            SupportActionBar.SetDisplayHomeAsUpEnabled(true);

            //Fragment
            mFragmentContainer = FindViewById<FrameLayout>(Resource.Id.fragmentContainer);

            //Left side menu
            mDrawerLayout = FindViewById<DrawerLayout>(Resource.Id.drawer_layout);
            mLeftDrawer = FindViewById<ExpandableListView>(Resource.Id.left_drawer);

            mDrawerToggle = new DrawerToggle(
                this,                           //Host Activity
                mDrawerLayout,                  //DrawerLayout
                Resource.String.openDrawer,     //Opened Message
                Resource.String.closeDrawer     //Closed Message
            );

            mDrawerLayout.AddDrawerListener(mDrawerToggle);

            mDrawerToggle.SyncState();

            npcc_LeftSideMenu objLeft = new npcc_LeftSideMenu(this);
            objLeft.fillLeftSideMenu(mLeftDrawer);


            //set fragment
            setFragments();
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {

                case Android.Resource.Id.Home:
                    //The hamburger icon was clicked which means the drawer toggle will handle the event
                    //all we need to do is ensure the right drawer is closed so the don't overlap
                    mDrawerToggle.OnOptionsItemSelected(item);
                    return true;

                case Resource.Id.action_Logout:
                    SecureStorage.Remove("oauth_token");
                    StartActivity(typeof(LoginActivity));
                    Finish();
                    return true;

                default:
                    return base.OnOptionsItemSelected(item);
            }
        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.extra, menu);
            return base.OnCreateOptionsMenu(menu);
        }


        void setFragments()
        {
            mFragment1 = new Fragment1();

            mStackFragments = new Stack<SupportFragment>();

            var trans = SupportFragmentManager.BeginTransaction();

            trans.Add(Resource.Id.fragmentContainer, mFragment1, "Fragment1");
            trans.Commit();


            mCurrentFragment = mFragment1;
        }

        private void ShowFragment(SupportFragment fragment)
        {
            if (fragment.IsVisible)
            {
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

        public override void OnBackPressed()
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

        protected override void OnPostCreate(Bundle savedInstanceState)
        {
            base.OnPostCreate(savedInstanceState);
            mDrawerToggle.SyncState();
        }

        public override void OnConfigurationChanged(Android.Content.Res.Configuration newConfig)
        {
            base.OnConfigurationChanged(newConfig);
            mDrawerToggle.OnConfigurationChanged(newConfig);
        }
    }
}
