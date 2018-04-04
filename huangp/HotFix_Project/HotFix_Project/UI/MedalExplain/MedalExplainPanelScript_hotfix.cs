using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace HotFix_Project
{
    class MedalExplainPanelScript_hotfix
    {
        // Use this for initialization
        public static void Start()
        {
            MedalExplainPanelScript script = OtherData.s_medalExplainPanelScript;

            if (UserData.isSetSecondPsw)
            {
                CommonUtil.setImageSprite(script.m_button_setSecondPsw.transform.Find("Image").GetComponent<Image>(), "Sprites/MedalExplain/anniu_zi_ysz");
                script.m_button_setSecondPsw.transform.Find("Image").GetComponent<Image>().SetNativeSize();
                GameObject.Destroy(script.m_button_setSecondPsw.GetComponent<Button>());
            }
            else
            {
                CommonUtil.setImageSprite(script.m_button_setSecondPsw.transform.Find("Image").GetComponent<Image>(), "Sprites/MedalExplain/anniu_zi_szmm");
                script.m_button_setSecondPsw.transform.Find("Image").GetComponent<Image>().SetNativeSize();
            }

            // 更新的部分
            if (ShieldWeChat.isShield(OtherData.s_channelName))
            {
                script.transform.Find("Bg").Find("Text (2)").GetComponent<Text>().text = "用途：可用于商城道具兑换";
                script.transform.Find("Bg").Find("Text (3)").GetComponent<Text>().text = "徽章密码：用于安全校验";
            }
        }
    }
}
