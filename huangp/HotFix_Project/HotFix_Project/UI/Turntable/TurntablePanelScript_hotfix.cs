using DG.Tweening;
using LitJson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace HotFix_Project
{
    class TurntablePanelScript_hotfix
    {
        public static void Start()
        {
            TurntablePanelScript script = OtherData.s_turntablePanelScript;

            script.m_ListViewScript = script.m_listView.GetComponent<ListViewScript>();

            script.m_button_free.transform.Find("Text").GetComponent<Text>().text = UserData.myTurntableData.freeCount.ToString();
            script.m_button_huizhang.transform.Find("Text").GetComponent<Text>().text = UserData.myTurntableData.huizhangCount.ToString();

            script.m_text_myLuckyValue.text = UserData.myTurntableData.luckyValue.ToString();

            // 获取转盘数据
            {
                NetLoading.getInstance().Show();

                LogicEnginerScript.Instance.GetComponent<GetTurntableRequest>().CallBack = script.onReceive_GetTurntable;
                LogicEnginerScript.Instance.GetComponent<GetTurntableRequest>().OnRequest();
            }

            // 更新的部分
            {
                if (ShieldWeChat.isShield(OtherData.s_channelName))
                {
                    script.gameObject.transform.Find("Image_bg/Down/Image_weixintishi").localScale = Vector3.zero;
                }

                GameObject LeftObj = script.gameObject.transform.Find("Image_bg/Left").gameObject;

                LeftObj.transform.Find("Text_myLuckyValue").localPosition = new Vector3(6, 42.23f,0);

                LeftObj.transform.Find("Text_tip").localPosition = new Vector3(-173.61f, -24, 0);
                LeftObj.transform.Find("Text_tip").GetComponent<RectTransform>().sizeDelta = new Vector2(361.93f, 66.36f);
                LeftObj.transform.Find("Text_tip").GetComponent<Text>().text = "抽奖增加幸运值，当值满         必得";

                LeftObj.transform.Find("Text_luckyMaxValue").localPosition = new Vector3(-74.4f, -24.7f, 0);

                LeftObj.transform.Find("Image_huafei10").localPosition = new Vector3(17, -23.4f, 0);
            }
        }
    }
}
