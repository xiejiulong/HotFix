using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using Hall;
using DG.Tweening;
using LitJson;

namespace HotFix_Project
{
    class Login_hotfix
    {
        private static bool bTween = false;
        public static void Start()
        {
            UnityEngine.Debug.LogError("Hi，进入Start HotFix...Start");

            Login login = OtherData.s_login;
            login.transform.Find("GoToGame_SAN_REN_DOU").GetComponent<Button>().onClick.AddListener(OnGoTo_SAN_REN_DOU);
            login.transform.Find("GoToGame_YAN_CHENG_MA_JIANG").GetComponent<Button>().onClick.AddListener(OnGoto_YAN_CHENG_MA_JIANG);
            login.transform.Find("GoToGame_XUE_LIU_CHENG_HE").GetComponent<Button>().onClick.AddListener(OnGoTo_XueLiuChengHe);

        }

        public static void OnGoTo_SAN_REN_DOU()
        {
            OtherData.s_ResHotFix.LoadSanRenDouPanel();
        }

        public static void OnGoto_YAN_CHENG_MA_JIANG()
        {
            OtherData.s_ResHotFix.LoadYanCHengMaJiangPanel();
        }

        public static void OnGoTo_XueLiuChengHe()
        {
            OtherData.s_ResHotFix.LoadXueLiuChengHePanel();
        }

        public static void Update()
        {
            if (!bTween)
            {
                UnityEngine.Debug.LogError("Hi，进入Update HotFix111111111111111111111...");
                Login login = OtherData.s_login;
                login.m_BtnLogin.transform.GetChild(0).GetComponent<Text>().color = new Color32(255, 255, 0, 255);
                login.m_BtnLogin.transform.GetChild(0).GetComponent<Text>().text = "这是 login hot fix";

                bTween = true;
                login.m_BtnLogin.transform.DOLocalMoveX(400, 5).SetLoops(-1, LoopType.Yoyo);
                login.m_ifPWD.transform.DOLocalMoveX(200, 1).SetLoops(-1, LoopType.Yoyo);
                login.m_ifUserName.transform.DOLocalMoveY(300, 2).SetLoops(-1, LoopType.Yoyo);

                login.m_ifUserName.text = "版本号是错的？：" + login.m_codeVersion.ToString();

                //login.m_ifPWD.text = "唐浩吃屎";
                if (OtherData.s_ResHotFix == null)
                {
                    login.m_ifPWD.text = "资源更新1";
                }
                else
                {
                    login.m_ifPWD.text = "资源更新2";

                    ResHotFix rhf = OtherData.s_ResHotFix;
                    Debug.LogError("资源版本号：" + rhf.GetLocalResVersion());
                    login.m_ifPWD.text = rhf.GetLocalResVersion().ToString();
                }
            }
        }

        public static void OnDestroy()
        {
            UnityEngine.Debug.LogError("Hi，进入OnDestroy HotFix11111111111111111111111...");
        }

        public static void OnHotFixTest()
        {
            Login login = OtherData.s_login;
            login.m_btnHotFixTest.transform.DOLocalMoveX(-500, 3).SetLoops(-1, LoopType.Yoyo);

            // 测试 json
            string strTest = "{\"amount\":\"198806011154\",\"key\":\"luckycoinbonuspool\"}";

            LuckyCoinBonusPool msg = JsonUtility.FromJson<LuckyCoinBonusPool>(strTest);
            Debug.Log(msg.amount);

            login.m_btnHotFixTest.transform.GetChild(0).GetComponent<Text>().color = new Color32(128, 255, 255, 255);

            login.m_ifUserName.text = "版本号是对的了？：" + login.m_codeVersion.ToString();
        }
    }
}
