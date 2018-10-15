using System;
using System.Collections.Generic;
using Android.Runtime;
using Android.Widget;

namespace Android_NPCCMobileServices
{
    public class npcc_LeftSideMenu
    {
        private const string Val1 = "Val1";
        private const string Val2 = "Val2";
        private const string DEPT_NAME = "dept_name";
        private Android.Content.Context context;

        public npcc_LeftSideMenu(Android.Content.Context sender)
        {
            context = sender;
        }


        public void fillLeftSideMenu(ExpandableListView mLeftDrawer)
        {
            string[] departments = new string[] { "Development", "Support", "Sales", "Management" };

            List<clsExpandableMenuChild> objExpandable = new List<clsExpandableMenuChild>()
            {
                new clsExpandableMenuChild() { objVal1="Sekhar Srinivasan", objVal2="Full Time" },
                new clsExpandableMenuChild() { objVal1="Hima Bindu", objVal2="Full Time" },
                new clsExpandableMenuChild() { objVal1="Lokeswari", objVal2="Part Time" },
                new clsExpandableMenuChild() { objVal1="Divya", objVal2="Part Time" },
            };

            using (JavaList<IDictionary<string, object>> groupData = new JavaList<IDictionary<string, object>>())
            using (JavaList<IList<IDictionary<string, object>>> childData = new JavaList<IList<IDictionary<string, object>>>())
            {
                foreach (string dept in departments)
                {
                    using (var curGroupMap = new JavaDictionary<string, object>())
                    {
                        groupData.Add(curGroupMap);
                        curGroupMap.Add(DEPT_NAME, dept);
                        using (var children = new JavaList<IDictionary<string, object>>())
                        {
                            foreach (clsExpandableMenuChild emp in objExpandable)
                            {
                                using (var curChildMap = new JavaDictionary<string, object>())
                                {
                                    children.Add(curChildMap);
                                    curChildMap.Add(Val1, emp.objVal1);
                                    curChildMap.Add(Val2, emp.objVal2);
                                }
                            }
                            childData.Add(children);
                        }
                    }
                }

                IExpandableListAdapter xadapter = new ExpandableMenuAdapter(
                    context,
                    groupData,
                    Resource.Layout.ExpandableMenuLayout,
                    new string[] { DEPT_NAME },
                    new int[] { Resource.Id. txtGroup},
                    childData,
                    0,
                    null,
                    new int[] { },
                    mLeftDrawer
                    );
                mLeftDrawer.SetAdapter(xadapter);
            }

        }

    }
}
