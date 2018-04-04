using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace HotFix_Project
{
    class PVPGameResultPanelScript_hotfix
    {
        public static void Start()
        {
            if (ShieldShare.isShield(OtherData.s_channelName))
            {
                GameObject.Find("Canvas_Middle").transform.Find("PVPGameResultPanel(Clone)").Find("Image_bg").Find("Button_share").localScale = new UnityEngine.Vector3(0, 0, 0);
            }
        }
    }
}
