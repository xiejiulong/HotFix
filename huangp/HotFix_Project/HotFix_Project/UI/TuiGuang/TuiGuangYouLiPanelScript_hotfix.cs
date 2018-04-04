using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotFix_Project
{
    class TuiGuangYouLiPanelScript_hotfix
    {
        public static void Start()
        {
            TuiGuangYouLiPanelScript script = OtherData.s_tuiGuangYouLiPanelScript;

            script.m_text_myTuiGuangCode.text = "我的推广码：" + UserData.myTuiGuangCode;
            script.m_listview_player = script.m_obj_tuiGuangList.GetComponent<ListViewScript>();

            NetLoading.getInstance().Show();

            LogicEnginerScript.Instance.GetComponent<MyTuiGuangYouLiDataRequest>().CallBack = script.onCallBackMyTuiGuangYouLiData;
            LogicEnginerScript.Instance.GetComponent<MyTuiGuangYouLiDataRequest>().OnRequest();

            // 更新的部分
            {
                if (ShieldShare.isShield(OtherData.s_channelName))
                {
                    script.gameObject.transform.Find("Image_bg/Button_yaoqing").localScale = new UnityEngine.Vector3(0,0,0);
                }
            }
        }

        public static void onCallBackMyTuiGuangYouLiData(string data)
        {
            TuiGuangYouLiPanelScript script = OtherData.s_tuiGuangYouLiPanelScript;

            NetLoading.getInstance().Close();

            MyTuiGuangData.getInstance().initJson(data);

            // 更新的部分
            {
                if (MyTuiGuangData.getInstance().getMyTuiGuangDataList().Count == 0)
                {
                    script.m_curShowTab = TuiGuangYouLiPanelScript.CurShowTab.CurShowTab_bulingjiangli;
                }
            }

            script.showTab(script.m_curShowTab);
        }
    }
}
