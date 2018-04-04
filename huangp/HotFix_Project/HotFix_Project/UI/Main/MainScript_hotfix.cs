using LitJson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TLJCommon;
using UnityEngine;
using UnityEngine.UI;

namespace HotFix_Project
{
    class MainScript_hotfix
    {
        public static void Start()
        {
            // 新增的
            {
                if (ShieldShare.isShield(OtherData.s_channelName))
                {
                    OtherData.s_mainScript.gameObject.transform.Find("ButtonList/Share").localScale = Vector3.zero;
                }

                if (ShieldWeChat.isShield(OtherData.s_channelName))
                {
                    // 主界面客服按钮
                    OtherData.s_mainScript.gameObject.transform.Find("ButtonList/Kefu").localScale = Vector3.zero;
                }

                // ios审核
                if (OtherData_hotfix.getIsIosCheck())
                {
                    // 转盘
                    OtherData.s_mainScript.gameObject.transform.Find("ButtonList/Button_zhuanpan").localScale = Vector3.zero;

                    // 签到
                    OtherData.s_mainScript.gameObject.transform.Find("ButtonList/Sign").localScale = Vector3.zero;

                    // 徽章兑换
                    OtherData.s_mainScript.gameObject.transform.Find("UserInfo/Medal/Button_medal_duihuan").localScale = Vector3.zero;

                    // 推广有礼
                    OtherData.s_mainScript.gameObject.transform.Find("ButtonList/TuiGuangYouLi").localScale = Vector3.zero;

                    // 活动
                    OtherData.s_mainScript.gameObject.transform.Find("ButtonList/Notice").localScale = Vector3.zero;
                }
            }

            // 禁止多点触摸
            Input.multiTouchEnabled = false;

            ToastScript.clear();

            // 安卓回调
            AndroidCallBack.s_onPauseCallBack = OtherData.s_mainScript.onPauseCallBack;
            AndroidCallBack.s_onResumeCallBack = OtherData.s_mainScript.onResumeCallBack;

            AudioScript.getAudioScript().stopMusic();

            OtherData.s_mainScript.startBgm();
            
            // 逻辑服务器
            {
                if (OtherData.s_mainScript.getLogicEnginerObj() == null)
                {
                    OtherData.s_mainScript.setLogicEnginerObj(LogicEnginerScript.create());
                    LogicEnginerScript.Instance.setOnLogicService_Connect(OtherData.s_mainScript.onSocketConnect_Logic);
                    LogicEnginerScript.Instance.setOnLogicService_Close(OtherData.s_mainScript.onSocketClose_Logic);
                    LogicEnginerScript.Instance.GetComponent<MainRequest>().CallBack = OtherData.s_mainScript.onReceive_Main;
                }
                else
                {
                    LogicEnginerScript.Instance.setOnLogicService_Connect(OtherData.s_mainScript.onSocketConnect_Logic);
                    LogicEnginerScript.Instance.setOnLogicService_Close(OtherData.s_mainScript.onSocketClose_Logic);

                    LogicEnginerScript.Instance.GetComponent<MainRequest>().CallBack = OtherData.s_mainScript.onReceive_Main;

                    if (LogicEnginerScript.Instance.isConnecion())
                    {
                        NetLoading.getInstance().Show();

                        LogicEnginerScript.Instance.GetComponent<GetUserInfoRequest>().OnRequest();
                        LogicEnginerScript.Instance.GetComponent<GetRankRequest>().OnRequest();
                        LogicEnginerScript.Instance.GetComponent<GetSignRecordRequest>().OnRequest();
                        LogicEnginerScript.Instance.GetComponent<GetUserBagRequest>().CallBack = onReceive_GetUserBag;
                        LogicEnginerScript.Instance.GetComponent<GetUserBagRequest>().OnRequest();
                        LogicEnginerScript.Instance.GetComponent<GetEmailRequest>().OnRequest();
                        LogicEnginerScript.Instance.GetComponent<GetNoticeRequest>().OnRequest();
                    }
                    else
                    {
                        NetErrorPanelScript.getInstance().Show();
                        NetErrorPanelScript.getInstance().setOnClickButton(OtherData.s_mainScript.onClickChongLian_Logic);
                        NetErrorPanelScript.getInstance().setContentText("与服务器断开连接，请重新连接");
                    }
                }
            }

            // 游戏打牌服务器
            {
                if (OtherData.s_mainScript.getPlayEnginerObj() == null)
                {
                    OtherData.s_mainScript.setPlayEnginerObj(PlayServiceSocket.create());

                    PlayServiceSocket.s_instance.setOnPlayService_Connect(OtherData.s_mainScript.onSocketConnect_Play);
                    PlayServiceSocket.s_instance.setOnPlayService_Receive(OtherData.s_mainScript.onSocketReceive_Play);
                    PlayServiceSocket.s_instance.setOnPlayService_Close(OtherData.s_mainScript.onSocketClose_Play);

                    PlayServiceSocket.s_instance.startConnect();
                }
                else
                {
                    PlayServiceSocket.s_instance.setOnPlayService_Connect(OtherData.s_mainScript.onSocketConnect_Play);
                    PlayServiceSocket.s_instance.setOnPlayService_Receive(OtherData.s_mainScript.onSocketReceive_Play);
                    PlayServiceSocket.s_instance.setOnPlayService_Close(OtherData.s_mainScript.onSocketClose_Play);
                }
            }

            OtherData.s_mainScript.m_laBaScript = OtherData.s_mainScript.m_laba.GetComponent<LaBaScript>();
        }
        
        public static void onReceive_GetUserBag(string data)
        {
            try
            {
                LogUtil.Log("处理背包回调");
                JsonData jsonData = JsonMapper.ToObject(data);
                var code = (int)jsonData["code"];
                if (code == (int)Consts.Code.Code_OK)
                {
                    UserData.propData = JsonMapper.ToObject<List<UserPropData>>(jsonData["prop_list"].ToString());
                    for (int i = 0; i < PropData.getInstance().getPropInfoList().Count; i++)
                    {
                        PropInfo propInfo = PropData.getInstance().getPropInfoList()[i];
                        for (int j = 0; j < UserData.propData.Count; j++)
                        {
                            UserPropData userPropData = UserData.propData[j];
                            if (propInfo.m_id == userPropData.prop_id)
                            {
                                userPropData.prop_icon = propInfo.m_icon;
                                userPropData.prop_name = propInfo.m_name;
                            }
                        }
                    }
                }
                else
                {
                    ToastScript.createToast("用户背包数据错误");
                    return;
                }
            }
            catch (Exception ex)
            {
                LogUtil.Log("MainScript_hotfix.onReceive_GetUserBag异常：" + ex);
            }
        }
    }
}
