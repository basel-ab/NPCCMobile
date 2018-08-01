using System;
using System.Collections.Generic;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace Android_NPCCMobileServices
{
    public class ExpandableMenuAdapter:SimpleExpandableListAdapter
    {
        private const string Val1 = "Val1";
        private const string Val2 = "Val2";
        private Context context;
        int previousGroup = -1;
        private readonly JavaList<IDictionary<string, object>> _groupData;
        private readonly JavaList<IList<IDictionary<string, object>>> _childData;

        ExpandableListView listView;
        public ExpandableMenuAdapter(Context context, JavaList<IDictionary<string, object>> groupData, int groupLayout,
            string[] groupFrom, int[] groupTo, JavaList<IList<IDictionary<string, object>>> childData, int childLayout, string[] childFrom, int[] childTo, ExpandableListView listView) : base(context, groupData, groupLayout, groupFrom, groupTo, childData, childLayout, childFrom, childTo)
        {
            this.context = context;
            this._groupData = new JavaList<IDictionary<string, object>>(groupData);
            this._childData = new JavaList<IList<IDictionary<string, object>>>(childData);
            this.listView = listView;
        }

        public override Java.Lang.Object GetChild(int groupPosition, int childPosition)
        {
            var group = this._groupData[groupPosition];
            var child = this._childData[groupPosition][childPosition];
            return (Java.Lang.Object)child;
        }

        public override View NewChildView(bool isLastChild, ViewGroup parent)
        {
            LayoutInflater layoutInflater = (LayoutInflater)context.GetSystemService(Context.LayoutInflaterService);
            return layoutInflater.Inflate(Resource.Layout.ExpandableMenuChildLayout, null, false);
        }

        public override View GetChildView(int groupPosition, int childPosition, bool isLastChild, View convertView, ViewGroup parent)
        {
            View v = base.GetChildView(groupPosition, childPosition, isLastChild, convertView, parent);

            var txtVal1 = (TextView)v.FindViewById(Resource.Id.txtVal1);
            var txtVal2 = (TextView)v.FindViewById(Resource.Id.txtVal2);

            txtVal1.SetText((string)((IDictionary<string, object>)GetChild(groupPosition, childPosition))[Val1], TextView.BufferType.Normal);
            txtVal2.SetText((string)((IDictionary<string, object>)GetChild(groupPosition, childPosition))[Val2], TextView.BufferType.Normal);

            if (listView.CheckedItemPosition == childPosition)
            {
                v.SetBackgroundColor(Android.Graphics.Color.ParseColor("0xAOFF8000"));
            }
            else
            {
                v.SetBackgroundColor(Android.Graphics.Color.Transparent);
            }

            return v;
        }

        public override void OnGroupExpanded(int groupPosition)
        {
            if(groupPosition != previousGroup)
                listView.CollapseGroup(previousGroup);
            previousGroup = groupPosition;
        }


    }
}
