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

using System.Collections;
using System.Collections.Generic;
using System.Text;
using Hall;
using ILRuntime.Runtime.Debugger.Protocol;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;

public class ResHotFix : MonoBehaviour
{
    private int m_localResVersion = 1;

    private const string AssetBundlesPath = "http://192.168.1.100:8080/HotFix/hotfix/assetbundle/";
    private const string PathOfResVersion = AssetBundlesPath+"resversion.txt";

    private const string PathOfLoginPanel      = AssetBundlesPath+"Android/loginpanel";
    private const string PathOfYanCHengMaJiang = AssetBundlesPath+"Android/yanchengmajiangpanel";
    private const string PathOfSanRenDou       = AssetBundlesPath+"Android/sanrendoupanel";

    private AssetBundle myLoadedAssetBundleForLoginPanel;
    private AssetBundle myLoadedAssetBundleForSanRenDou;
    private AssetBundle myLoadedAssetBundleForYanCHengMaJiang;

    private GameObject LoginPanel = null;
    private GameObject SanRenDouPanel = null;
    private GameObject YanChengMaJiang = null;

    public int GetLocalResVersion()
    {
        return m_localResVersion;
    }

    private string GetPathOfResVersion()
    {
        return AssetBundlesPath + "resversion.txt";
    }

    private string GetPathOfLoginPanel()
    {
        string strPlatformName = GetPlatFormName();
        return AssetBundlesPath + strPlatformName + "/loginpanel";
    }

    private string GetPathOfYanChengMaJiangPanel()
    {
        string strPlatformName = GetPlatFormName();
        return AssetBundlesPath + strPlatformName + "/yanchengmajiangpanel";
    }

    private string GetPathOfSanRenDou()
    {
        string strPlatformName = GetPlatFormName();
        return AssetBundlesPath + strPlatformName + "/sanrendoupanel";
    }

    private string GetPlatFormName()
    {
        switch (Application.platform)
        {
            case RuntimePlatform.Android:
                return "Android";
            case RuntimePlatform.IPhonePlayer:
                return "iOS";
            case RuntimePlatform.WindowsPlayer:
            case RuntimePlatform.WindowsEditor:
                return "StandaloneWindows";
            default:
                Debug.LogError("GetPlatFormName Error");
                return "unknow plat form";
        }
    }

    private void Awake()
    {
        OtherData.s_ResHotFix = this;
    }

    private void OnDestroy()
    {
        OtherData.s_ResHotFix = null;
    }

    IEnumerator Start()
    {
        while (!Caching.ready)
            yield return null;
        Debug.Log("开始。。。检查资源更新...");

        yield return StartCoroutine(CheckVersion());

        Debug.Log("加载新资源..." + m_localResVersion);
        var www = WWW.LoadFromCacheOrDownload(GetPathOfLoginPanel(), m_localResVersion);
        yield return www;
        if (!string.IsNullOrEmpty(www.error))
        {
            Debug.Log(www.error);
            yield break;
        }

        Debug.Log("加载新资源...完毕");

        myLoadedAssetBundleForLoginPanel = www.assetBundle;
        GameObject prefab = myLoadedAssetBundleForLoginPanel.LoadAsset<GameObject>("LoginPanel");
        if (prefab)
        {
            Debug.Log("生成登录面板...");
            LoginPanel = Instantiate(prefab, this.transform);
            Debug.Log("生成登录面板...完毕");

            while (!OtherData.IsDllLoadOver)
            {
                yield return null;
            }
            Debug.Log("Dll load over......");
            yield return new WaitForEndOfFrame();
        }
    }

    IEnumerator CheckVersion()
    {
        yield return null;
        WWW wwwVersion = new WWW(GetPathOfResVersion());
        while (!wwwVersion.isDone)
        {
            yield return null;
        }

        if (!string.IsNullOrEmpty(wwwVersion.error))
        {
            Debug.LogError(wwwVersion.error);
        }

        byte[] ta = wwwVersion.bytes;
        wwwVersion.Dispose();
        string strVersion = Encoding.GetEncoding("GB2312").GetString(ta);
        string[] serverVersion = strVersion.Split('=');
        int iSVersion = int.Parse(serverVersion[1]);
        m_localResVersion = iSVersion;
        Debug.Log("资源版本号：" + m_localResVersion);
    }

    public void LoadSanRenDouPanel()
    {
        StartCoroutine(LoadGameSanRenDou());
    }

    public void LoadYanCHengMaJiangPanel()
    {
        StartCoroutine(LoadGameYanChengMaJiang());
    }

    IEnumerator LoadGameYanChengMaJiang()
    {
        if (null == myLoadedAssetBundleForYanCHengMaJiang)
        {
            yield return null;
            var www = WWW.LoadFromCacheOrDownload(GetPathOfYanChengMaJiangPanel(), m_localResVersion);
            yield return www;
            if (!string.IsNullOrEmpty(www.error))
            {
                Debug.Log(www.error);
                yield break;
            }

            myLoadedAssetBundleForYanCHengMaJiang = www.assetBundle;
        }


        GameObject prefab = myLoadedAssetBundleForYanCHengMaJiang.LoadAsset<GameObject>("YanCHengMaJiangPanel");
        if (prefab)
        {
            YanChengMaJiang = Instantiate(prefab, this.transform);
            yield return new WaitForEndOfFrame();

            // 测试退出按钮
            YanChengMaJiang.transform.Find("Exit").GetComponent<Button>().onClick.AddListener(
                () =>
                {
                    Destroy(YanChengMaJiang.gameObject);
                });
        }
    }

    IEnumerator LoadGameSanRenDou()
    {
        if (null == myLoadedAssetBundleForSanRenDou)
        {
            yield return null;
            var www = WWW.LoadFromCacheOrDownload(GetPathOfSanRenDou(), m_localResVersion);
            yield return www;
            if (!string.IsNullOrEmpty(www.error))
            {
                Debug.Log(www.error);
                yield break;
            }

            myLoadedAssetBundleForSanRenDou = www.assetBundle;
        }

        GameObject prefab = myLoadedAssetBundleForSanRenDou.LoadAsset<GameObject>("SanRenDouPanel");
        if (prefab)
        {
            SanRenDouPanel = Instantiate(prefab, this.transform);
            yield return new WaitForEndOfFrame();

            // 测试退出按钮
            SanRenDouPanel.transform.Find("Exit").GetComponent<Button>().onClick.AddListener(
                () =>
                {
                    Destroy(SanRenDouPanel.gameObject);
                });
        }
    }
}