/*************************************************************************************
 * CLR version      : version 5.6.2f1
 * TargetFW Version : 5.0
 * Class Name       :
 * Machine Name     : SC-201609201141 Hp Win7
 * Name Space       :
 * File Name        :
 * Create Date      : 2017/06/09 14:01:32
 * Author           : XieJiulong
 *
 * Modify Date      :
 * Author           :
 * Description      :
 *************************************************************************************/

using Hall;
using System.Collections;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using LitJson;
public class respBase
{
    public static string key = "\"key\"";
    public static string split = ":";
    public static string union = key + split;
    public static string reserv = "";
}

// 大厅推送奖池更新 {"amount":"614881","key":"luckycoinbonuspool"}
public class LuckyCoinBonusPool
{
    public static string ID = respBase.union + "\"luckycoinbonuspool\"";
    public string key = "luckycoinbonuspool";
    public string amount = "";                  // 大厅定义的 是个char sztemp[32]={0};
}

public class Login : MonoBehaviour
{
    public Button m_BtnLogin;
    public InputField m_ifUserName;
    public InputField m_ifPWD;
    public Button m_btnHotFixTest;

    private bool IsDllGetOver = false;

    // 代码版本
    public int m_codeVersion = 1;

    private void Awake()
    {
        OtherData.s_login = this;
    }

    IEnumerator  Start()
    {
        while (!Caching.ready)
            yield return null;

        yield return StartCoroutine("CheckVersion");

        Debug.LogError("Start 123");

        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("Login_hotfix", "Start"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.Login_hotfix", "Start", null, null);
        }

        m_BtnLogin.onClick.AddListener(OnBtnLoginClick);
        m_btnHotFixTest.onClick.AddListener(OnHotFixTest);

        //transform.Find("GoToGame").GetComponent<Button>().onClick.AddListener(OtherData.s_ResHotFix.LoadSanRenDouPanel);
        //transform.Find("GoToGame").GetComponent<Button>().onClick.AddListener(OtherData.s_ResHotFix.LoadYanCHengMaJiangPanel);
    }

    private void OnBtnLoginClick()
    {
        //m_codeVersion = PlayerPrefs.GetInt("Version");

        //ILRuntimeUtil.getInstance().downDll(OtherData.getWebUrl() + "hotfix/HotFix_Project-" + m_codeVersion + ".dll");

        StartCoroutine("CheckVersion");
    }

    private void OnHotFixTest()
    {
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("Login_hotfix", "OnHotFixTest"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.Login_hotfix", "OnHotFixTest", null, null);
        }
    }

    private IEnumerator CheckVersion()
    {
        OtherData.s_login.m_codeVersion = PlayerPrefs.GetInt("Version");
        Debug.LogError("本地版本号 + " + m_codeVersion);
        m_BtnLogin.transform.GetChild(0).GetComponent<Text>().text = "本地版本号 + " + m_codeVersion;

        WWW www = new WWW(OtherData.getWebUrl() + "hotfix/version.txt");
        Debug.LogError("正在获取服务器版本号");
        while (!www.isDone)
        {
            //Debug.LogError("正在下载");
            m_BtnLogin.transform.GetChild(0).GetComponent<Text>().text = "正在下载" + www.progress;
            yield return null;
        }

        if (!string.IsNullOrEmpty(www.error))
            Debug.LogError(www.error);

        byte[] ta = www.bytes;
        www.Dispose();

        string strVersion = Encoding.GetEncoding("GB2312").GetString(ta);
        Debug.Log("获取版本号: " + strVersion);
        m_BtnLogin.transform.GetChild(0).GetComponent<Text>().text = "获取版本号: " + strVersion;

        string[] serverVersion = strVersion.Split('=');
        m_BtnLogin.transform.GetChild(0).GetComponent<Text>().text = "1";
        int iSVersion = int.Parse(serverVersion[1]);
        m_BtnLogin.transform.GetChild(0).GetComponent<Text>().text = "2";

        m_codeVersion = iSVersion;
        ILRuntimeUtil.getInstance().downDll(OtherData.getWebUrl() + "hotfix/HotFix_Project-" + iSVersion + ".dll");

        //// 更新
        //if (m_codeVersion != iSVersion)
        //{
        //    Debug.LogError("版本号不同，开始更新");
        //}
        //else
        //{
        //    Debug.LogError("版本号相同");
        //    m_BtnLogin.transform.GetChild(0).GetComponent<Text>().text = "版本号相同";
        //}
        //m_BtnLogin.transform.GetChild(0).GetComponent<Text>().text = "3";

    }

    private void Update()
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("Login_hotfix", "Update"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.Login_hotfix", "Update", null, null);
            return;
        }

        //Debug.LogError("本地Update");
    }

    private void OnDestroy()
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("Login_hotfix", "OnDestroy"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.Login_hotfix", "OnDestroy", null, null);
            return;
        }

        OtherData.s_login = null;
    }

    public void onDllGetOver()
    {
        IsDllGetOver = true;

        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("Login_hotfix", "onDllGetOver"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.LoginScript_hotfix", "onDllGetOver", null, null);
            return;
        }

        {
            // 禁止多点触摸
            Input.multiTouchEnabled = false;

            // 永不息屏
            Screen.sleepTimeout = SleepTimeout.NeverSleep;

            OtherData.s_screenSize = new Vector2(Screen.width, Screen.height);

            // 安卓回调
            //AndroidCallBack.s_onPauseCallBack = onPauseCallBack;
            //AndroidCallBack.s_onResumeCallBack = onResumeCallBack;
        }

        //ToastScript.clear();

        {
            //GameUtil.hideGameObject(m_debugLog);

            // 用于打印屏幕日志
            //m_debugLogScript = m_debugLog.GetComponent<DebugLogScript>();
        }

        //m_inputAccount.text = PlayerPrefs.GetString("account", "");
        //m_inputPassword.text = PlayerPrefs.GetString("password", "");

        //Set3rdLogin();
        //setLogonTypeUI();
    }
}