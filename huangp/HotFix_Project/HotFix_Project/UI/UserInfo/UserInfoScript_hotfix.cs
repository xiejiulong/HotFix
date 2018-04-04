using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace HotFix_Project
{
    class UserInfoScript_hotfix
    {
        public static void Start()
        {
            UserInfoScript.Instance = OtherData.s_userInfoScript;
            OtherData.s_userInfoScript.InitUI();

            if(ShieldRealName.isShield(OtherData.s_channelName))
            {
                Transform bg = OtherData.s_userInfoScript.transform.Find("Bg");

                bg.transform.Find("Identity").Find("ButtonBindPhone").localScale = Vector3.zero;
                bg.transform.Find("Identity").Find("ButtonChangePhone").localScale = Vector3.zero;
                bg.transform.Find("Identity").Find("ButtonRealName").localScale = Vector3.zero;
                bg.transform.Find("Identity").Find("AlreadyRealName").localScale = Vector3.zero;
            }
        }
    }
}
