using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine.UI;

namespace HotFix_Project
{
    class Activity_huafeisuipian_Script_hotfix
    {
        public static void onReceive_HuaFeiSuiPianDuiHuanData(string json)
        {
            Activity_huafeisuipian_Script script = OtherData.s_activity.transform.Find("Bg/Activity/Right_Bg/Activity_huafeisuipian(Clone)").GetComponent<Activity_huafeisuipian_Script>();
            NetLoading.getInstance().Close();

            HuaFeiSuiPianDuiHuanData.getInstance().initJson(json);

            script.loadList();

            // 更新的部分
            {
                script.transform.Find("Text").GetComponent<Text>().text = "每日<color=#FFB900FF>19:00、20:00、21:00、22:00</color>整点，<color=#FFB900FF>普通场（经典、抄底）</color>将掉落话费宝箱，请提前登录进行游戏，可获得<color=#FFB900FF>话费碎片</color>哦！";
            }
        }
    }
}
