using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace HotFix_Project
{
    class MedalDuiHuanPanelScript_hotfix
    {
        public static void Start()
        {
            MedalDuiHuanPanelScript script = OtherData.s_medalDuiHuanPanelScript;
            script.m_listView_duihuanshangpin = script.m_obj_duihuanshangpin.GetComponent<ListViewScript>();
            script.m_listView_duihuanjilu = script.m_obj_duihuanjilu.GetComponent<ListViewScript>();

            script.m_text_myMedalNum.text = UserData.medal.ToString();

            NetLoading.getInstance().Show();

            LogicEnginerScript.Instance.GetComponent<GetMedalDuiHuanRewardRequest>().CallBack = script.onCallBackGetMedalDuiHuanReward;
            LogicEnginerScript.Instance.GetComponent<GetMedalDuiHuanRewardRequest>().OnRequest();
            
            // 更新的部分
            {
                if (ShieldWeChat.isShield(OtherData.s_channelName))
                {
                    script.gameObject.transform.Find("Image_bg/Image_down/Text_tip").localScale = Vector3.zero;
                }
            }
        }
    }
}
