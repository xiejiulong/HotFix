using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace HotFix_Project
{
    class AboutPanelScript_hotfix
    {
        public static void Start()
        {
            if (OtherData.s_channelName.CompareTo("huawei") == 0)
            {
                OtherData.s_aboutPanelScript.gameObject.transform.Find("Bg/Scroll View/Viewport/Content1").GetComponent<Text>().text += "幸运大转盘 概率公示：1 金币*500 32.6%、2 记牌器*1 10.15%、3 双倍金币卡 *1 5.89%、4 话费*1 1%、5 金币*800 25.49%、6 记牌器*5 6%、7 喇叭*1 9.6%、8 金币*5000 6.9%、9 蓝钻石*3 2.32%、10 话费*10 0.05%";
            }
        }
    }
}
