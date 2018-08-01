using System;
using System.Collections.Generic;
using Android.Widget;

namespace Android_NPCCMobileServices
{
    public class clsRightMenu
    {
        private ArrayAdapter mRightAdapter;
        private List<string> mRightDataSet;
        private Android.Content.Context context;

        public clsRightMenu(Android.Content.Context sender)
        {
            context = sender;
        }
        public void fillRightSideMenu(ListView mRightDrawer)
        {
            mRightDataSet = new List<string>();
            mRightDataSet.Add("Right Item 1");
            mRightDataSet.Add("Right Item 2");
            mRightAdapter = new ArrayAdapter<string>(context, Android.Resource.Layout.SimpleListItem1, mRightDataSet);
            mRightDrawer.Adapter = mRightAdapter;
        }
    }
}
