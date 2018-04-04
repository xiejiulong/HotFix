using LitJson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

namespace HotFix_Project
{
    class LoginScript_hotfix
    {
        public static void setLogonTypeUI()
        {
            LoginScript script = OtherData.s_loginScript;

            // 更新的部分
            if (ShieldThirdLogin.isShield(OtherData.s_channelName))
            {
                GameUtil.showGameObject(script.m_button_guanfang.gameObject);
                script.m_button_guanfang.transform.localPosition = new Vector3(0, -194.1f, 0);

                GameUtil.hideGameObject(script.m_button_qq.gameObject);
                GameUtil.hideGameObject(script.m_button_wechat.gameObject);
                GameUtil.hideGameObject(script.m_button_defaultLogin.gameObject);
                GameUtil.hideGameObject(script.m_button_3rdLogin.gameObject);

                return;
            }

            bool is3RdLogin = ChannelHelper.Is3RdLogin();
            string channelAllName = ChannelHelper.GetChannelAllName();
            LogUtil.Log("渠道号:" + OtherData.s_channelName + ",渠道名:" + channelAllName);

            bool isThirdLogin = PlatformHelper.IsThirdLogin();
            
            if (is3RdLogin && isThirdLogin)
            {
                return;
            }
            else
            {
                {
                    int defaultLoginType = PlayerPrefs.GetInt("DefaultLoginType", (int)OtherData.s_defaultLoginType);
                    
                    switch (defaultLoginType)
                    {
                        case (int)OtherData.DefaultLoginType.DefaultLoginType_Default:
                            {
                                GameUtil.showGameObject(script.m_button_guanfang.gameObject);
                                GameUtil.showGameObject(script.m_button_qq.gameObject);
                                GameUtil.showGameObject(script.m_button_wechat.gameObject);

                                GameUtil.hideGameObject(script.m_button_defaultLogin.gameObject);
                                GameUtil.hideGameObject(script.m_button_3rdLogin.gameObject);
                            }
                            break;

                        case (int)OtherData.DefaultLoginType.DefaultLoginType_GuanFang:
                            {
                                GameUtil.hideGameObject(script.m_button_guanfang.gameObject);
                                GameUtil.hideGameObject(script.m_button_qq.gameObject);
                                GameUtil.hideGameObject(script.m_button_wechat.gameObject);
                                GameUtil.hideGameObject(script.m_button_3rdLogin.gameObject);

                                GameUtil.showGameObject(script.m_button_defaultLogin.gameObject);

                                script.m_button_defaultLogin.transform.Find("Text_LoginType").GetComponent<Text>().text = "账号登录";
                            }
                            break;

                        case (int)OtherData.DefaultLoginType.DefaultLoginType_QQ:
                            {
                                GameUtil.hideGameObject(script.m_button_guanfang.gameObject);
                                GameUtil.hideGameObject(script.m_button_qq.gameObject);
                                GameUtil.hideGameObject(script.m_button_wechat.gameObject);
                                GameUtil.hideGameObject(script.m_button_3rdLogin.gameObject);

                                GameUtil.showGameObject(script.m_button_defaultLogin.gameObject);

                                script.m_button_defaultLogin.transform.Find("Text_LoginType").GetComponent<Text>().text = "QQ登录";
                            }
                            break;

                        case (int)OtherData.DefaultLoginType.DefaultLoginType_WeChat:
                            {
                                GameUtil.hideGameObject(script.m_button_guanfang.gameObject);
                                GameUtil.hideGameObject(script.m_button_qq.gameObject);
                                GameUtil.hideGameObject(script.m_button_wechat.gameObject);

                                GameUtil.hideGameObject(script.m_button_3rdLogin.gameObject);

                                GameUtil.showGameObject(script.m_button_defaultLogin.gameObject);
                                script.m_button_defaultLogin.transform.Find("Text_LoginType").GetComponent<Text>().text = "微信登录";
                            }
                            break;
                    }
                }
            }
        }

        public static void onReceive_CheckVerisionCode(string data)
        {
            LoginScript script = OtherData.s_loginScript;

            NetLoading.getInstance().Close();

            JsonData jd = JsonMapper.ToObject(data);
            int code = (int)jd["code"];

            if (code == (int)TLJCommon.Consts.Code.Code_OK)
            {
                bool isStopServer = (bool)jd["isStopServer"];
                if (isStopServer)
                {
                    NetErrorPanelScript.getInstance().Show();
                    NetErrorPanelScript.getInstance().setOnClickButton(onServerIsStop);
                    NetErrorPanelScript.getInstance().setContentText("服务器正在维护，请稍后登录。");

                    return;
                }

                OtherData.s_canRecharge = (bool)jd["canRecharge"];
                OtherData.s_canDebug = (bool)jd["canDebug"];

                string apkVersion = jd["apkVersion"].ToString();

                if (OtherData.s_apkVersion.CompareTo(apkVersion) < 0)
                {
                    // ios的暂时不做版本检测
                    //if (OtherData.s_channelName.CompareTo("ios") == 0)
                    //{
                        
                    //}
                    // 评测包不做版本检测
                    if (OtherData.s_channelName.CompareTo("javgame_test") == 0)
                    {

                    }
                    else
                    {
                        NetErrorPanelScript.getInstance().Show();
                        NetErrorPanelScript.getInstance().setOnClickButton(onApkVerisionIsLow);
                        NetErrorPanelScript.getInstance().setContentText("您的客户端版本过低，请更新到最新版本。");
                    }
                }

                {
                    string banbao = jd["banhao"].ToString();
                    PlayerPrefs.SetString("banhao", banbao);
                }
            }
            else
            {
                ToastScript.createToast("服务器内部错误");
            }
        }

        public static void onServerIsStop()
        {
            NetErrorPanelScript.getInstance().Show();
            NetErrorPanelScript.getInstance().setOnClickButton(onServerIsStop);
            NetErrorPanelScript.getInstance().setContentText("服务器正在维护，请稍后登录。");
        }

        public static void onApkVerisionIsLow()
        {
            if (OtherData.s_channelName.CompareTo("ios") != 0)
            {
                PlatformHelper.DownApk();
                GameObject go = GameObject.Find("NetErrorPanel(Clone)");
                if (go != null)
                {
                    GameObject.Destroy(go);
                }
            }
            else
            {
                NetErrorPanelScript.getInstance().Show();
                NetErrorPanelScript.getInstance().setOnClickButton(onApkVerisionIsLow);
                NetErrorPanelScript.getInstance().setContentText("您的客户端版本过低，请更新到最新版本。");
            }
        }

        // 请求注册
        public static void reqQuickRegister()
        {
            LoginScript script = OtherData.s_loginScript;

            if ((script.m_inputAccount_register.text.CompareTo("") == 0) ||
                (script.m_inputSecondPassword_register.text.CompareTo("") == 0) ||
                (script.m_inputPassword_register.text.CompareTo("") == 0))
            {
                ToastScript.createToast("请输入账号密码");
                return;
            }

            if (script.m_inputSecondPassword_register.text.CompareTo(script.m_inputPassword_register.text) != 0)
            {
                ToastScript.createToast("密码不一致");
                return;
            }

            // 检测账号是否合格
            if (SensitiveWordUtil.IsSensitiveWord(script.m_inputAccount_register.text))
            {
                ToastScript.createToast("您的账号有敏感词");

                return;
            }

            if (script.m_inputAccount_register.text.Length > 10)
            {
                ToastScript.createToast("账号长度不可超过10个字符");

                return;
            }

            // 检测密码是否合格
            {
                for (int i = 0; i < script.m_inputPassword_register.text.Length; i++)
                {
                    string str = script.m_inputPassword_register.text[i].ToString();
                    if (((CommonUtil.charToAsc(str) >= 48) && (CommonUtil.charToAsc(str) <= 57) ||
                         ((CommonUtil.charToAsc(str) >= 65) && (CommonUtil.charToAsc(str) <= 90) ||
                          ((CommonUtil.charToAsc(str) >= 97) && (CommonUtil.charToAsc(str) <= 122)))))
                    {
                    }
                    else
                    {
                        ToastScript.createToast("密码格式不对");

                        return;
                    }
                }

                if (script.m_inputPassword_register.text.Length < 6)
                {
                    ToastScript.createToast("密码至少6位");
                    return;
                }

                if (script.m_inputPassword_register.text.Length > 30)
                {
                    ToastScript.createToast("密码不能超过30位");
                    return;
                }
            }

            NetLoading.getInstance().Show();

            {
                JsonData data = new JsonData();

                data["tag"] = TLJCommon.Consts.Tag_QuickRegister;
                data["account"] = script.m_inputAccount_register.text;
                data["password"] = CommonUtil.GetMD5(script.m_inputSecondPassword_register.text);
                data["channelname"] = PlatformHelper.GetChannelName();
                data["mac"] = PlatformHelper.GetMacId();
                LoginServiceSocket.s_instance.sendMessage(data.ToJson());
            }
        }
    }
}
