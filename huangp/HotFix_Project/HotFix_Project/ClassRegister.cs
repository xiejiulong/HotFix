using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotFix_Project
{
    /*
     * 所有主工程需要更新的函数都必须在这里注册
     * 如果不注册，即使写了更新的函数也不会生效
     */
    class ClassRegister
    {
        static List<string> s_funcList = new List<string>();
        static bool s_hasInit = false;

        static void init ()
        {
            // Request
            {
                // TaskDataScript_hotfix
                s_funcList.Add("TaskDataScript_hotfix.initJson");
                
                // UseHuaFeiRequest_hotfix
                s_funcList.Add("UseHuaFeiRequest_hotfix.OnRequest");
            }

            // UI.PVPChoice
            {
                // PVPChoiceScript_hotfix
                s_funcList.Add("PVPChoiceScript_hotfix.Start");
            }

            // UI.Game
            {
                // GameResultPanelScript_hotfix
                s_funcList.Add("GameResultPanelScript_hotfix.Start");
                
                // PVPEndPanelScript_hotfix
                s_funcList.Add("PVPEndPanelScript_hotfix.Start");

                // PVPGameResultPanelScript_hotfix
                s_funcList.Add("PVPGameResultPanelScript_hotfix.Start");
            }

            // UI.Login
            {
                // LoginScript_hotfix
                s_funcList.Add("LoginScript_hotfix.setLogonTypeUI");
                s_funcList.Add("LoginScript_hotfix.onReceive_CheckVerisionCode");  // 为了ios加的
                s_funcList.Add("LoginScript_hotfix.reqQuickRegister");
            }

            // UI.Main
            {
                // MainScript
                s_funcList.Add("MainScript_hotfix.Start");
            }

            // UI.Bag
            {
                // BagPanelScript_hotfix
                s_funcList.Add("BagPanelScript_hotfix.onInitializeItem");

                // PropDetailPanelScript_hotfix
                s_funcList.Add("PropDetailPanelScript_hotfix.setPropId");
            }

            // UI.MedalDuiHuanPanel
            {
                // MedalDuiHuanPanelScript_hotfix
                s_funcList.Add("MedalDuiHuanPanelScript_hotfix.Start");
            }

            // UI.TuiGuang
            {
                // TuiGuangYouLiPanelScript_hotfix
                s_funcList.Add("TuiGuangYouLiPanelScript_hotfix.Start");
                s_funcList.Add("TuiGuangYouLiPanelScript_hotfix.onCallBackMyTuiGuangYouLiData");
            }

            // UI.Activity
            {
                // Activity_hotfix
                s_funcList.Add("Activity_hotfix.GetActivityData");
                s_funcList.Add("Activity_hotfix.GetNoticeData");

                // Activity_huafeisuipian_Script_hotfix
                s_funcList.Add("Activity_huafeisuipian_Script_hotfix.onReceive_HuaFeiSuiPianDuiHuanData");
            }

            // UI.About
            {
                // AboutPanelScript_hotfix
                s_funcList.Add("AboutPanelScript_hotfix.Start");
            }

            // UI.MedalExplain
            {
                // MedalExplainPanelScript_hotfix
                s_funcList.Add("MedalExplainPanelScript_hotfix.Start");

                // SetSecondPswPanelScript_hotfix
                s_funcList.Add("SetSecondPswPanelScript_hotfix.Start");
            }

            // UI.Shop
            {
                // BuyGoodsPanelScript_hotfix
                s_funcList.Add("BuyGoodsPanelScript_hotfix.buy");
            }

            // UI.ShowReward
            {
                // ShowRewardPanelScript_hotfix
                s_funcList.Add("ShowRewardPanelScript_hotfix.Start");
            }

            // UI.UserInfo
            {
                // UserInfoScript_hotfix
                s_funcList.Add("UserInfoScript_hotfix.Start");
            }

            // UI.Turntable
            {
                // TurntablePanelScript_hotfix
                s_funcList.Add("TurntablePanelScript_hotfix.Start");
            }

            // Utils
            {
                // GameUtil_hotfix
                s_funcList.Add("GameUtil_hotfix.getOneTips");
            }

            // Commons
            {
                // EnterMainPanelShowManager_hotfix
                s_funcList.Add("EnterMainPanelShowManager_hotfix.init");
            }

            // Data
            {
                s_funcList.Add("PVPGameRoomDataScript_hotfix.initJson");
            }
        }

        public static List<string> getFuncList()
        {
            if (!s_hasInit)
            {
                init();

                s_hasInit = true;
            }

            return s_funcList;
        }

        /*funcName:类名.函数名 如：“MedalExplainPanelScript.onClickSetPsw”
         * 如果有的函数有多个重载，则这样写：MedalExplainPanelScript.onClickSetPsw(1)、MedalExplainPanelScript.onClickSetPsw(2)
         * init函数注册的时候也得按照上面的对应关系
         */
        public static bool checkClassHasFunc(string funcName)
        {
            if (!s_hasInit)
            {
                init();

                s_hasInit = true;
            }

            for (int i = 0; i < s_funcList.Count; i++)
            {
                if (s_funcList[i].CompareTo(funcName) == 0)
                {
                    return true;
                }
            }

            return false;
        }
    }
}
