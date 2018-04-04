using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace HotFix_Project
{
    class PVPChoiceScript_hotfix
    {
        public static void Start()
        {
            PVPChoiceScript script = OtherData.s_pvpChoiceScript;
            script.m_ListViewScript = script.m_listView.GetComponent<ListViewScript>();

            if (PVPChoiceScript.s_isShowBiSaiChang)
            {
                script.showHuaFeiChang();
                script.showMyBaoMingFei(false);
            }
            else
            {
                script.showJinBiChang();
                script.showMyBaoMingFei(true);
            }

            // 更新的部分
            {
                if (OtherData_hotfix.getIsIosCheck())
                {
                    script.transform.Find("Image_bg/Image_tab_bg/Button_tiaozhansai").localPosition = Vector3.zero;
                    script.transform.Find("Image_bg/Image_tab_bg/Image").localPosition = Vector3.zero;
                    script.transform.Find("Image_bg/Image_tab_bg").GetComponent<RectTransform>().sizeDelta = Vector2.zero;

                    script.transform.Find("Image_bg/Image_tab_bg/Button_huafeisai").localScale = Vector3.zero;
                }
            }
        }
    }
}
