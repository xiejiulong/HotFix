using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotFix_Project
{
    class EnterMainPanelShowManager_hotfix
    {
        public static void init()
        {
            List<EnterMainPanelObj> s_panelObjList = EnterMainPanelShowManager.getInstance().s_panelObjList;
            s_panelObjList.Add(new EnterMainPanelObj("sign", false));
            s_panelObjList.Add(new EnterMainPanelObj("newPlayerTuiGuang", false));
            s_panelObjList.Add(new EnterMainPanelObj("activity", false));
            s_panelObjList.Add(new EnterMainPanelObj("huizhangduihuan", false));

            // 更新的部分
            if (OtherData_hotfix.getIsIosCheck())
            {
                s_panelObjList.Clear();
            }
        }
    }
}
