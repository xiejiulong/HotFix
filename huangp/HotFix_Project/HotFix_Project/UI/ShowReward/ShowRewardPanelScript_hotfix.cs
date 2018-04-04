using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotFix_Project
{
    class ShowRewardPanelScript_hotfix
    {
        public static void Start()
        {
            if(ShieldShare.isShield(OtherData.s_channelName))
            {
                OtherData.s_showRewardPanelScript.transform.Find("Image_bg").Find("Button_share").localScale = new UnityEngine.Vector3(0, 0, 0);
            }
        }
    }
}
