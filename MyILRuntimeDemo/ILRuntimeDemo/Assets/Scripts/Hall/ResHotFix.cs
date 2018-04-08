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
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class ResHotFix : MonoBehaviour
{
    private int m_localResVersion = 1;

    private const string AssetBundlesPath = "http://192.168.1.100:8080/HotFix/hotfix/assetbundle/";
    private const string PathOfResVersion = AssetBundlesPath + "resversion.txt";

    private const string PathOfLoginPanel = AssetBundlesPath + "Android/loginpanel";
    private const string PathOfYanCHengMaJiang = AssetBundlesPath + "Android/yanchengmajiangpanel";
    private const string PathOfSanRenDou = AssetBundlesPath + "Android/sanrendoupanel";

    private AssetBundle myLoadedAssetBundleForLoginPanel;
    private AssetBundle myLoadedAssetBundleForSanRenDou;
    private AssetBundle myLoadedAssetBundleForYanCHengMaJiang;
    private AssetBundle myLoadedAssetBundleForXueLiuChengHe;

    private GameObject LoginPanel = null;
    private GameObject SanRenDouPanel = null;
    private GameObject YanChengMaJiang = null;
    private GameObject XueLiuChengHePanel = null;

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

    private string GetPathOfSanRenDouPanel()
    {
        string strPlatformName = GetPlatFormName();
        return AssetBundlesPath + strPlatformName + "/sanrendoupanel";
    }

    private string GetPathOfXueLiuChengHePanel()
    {
        string strPlatformName = GetPlatFormName();
        return AssetBundlesPath + strPlatformName + "/xueliuchenghepanel";
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

    private IEnumerator Start()
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

    private IEnumerator CheckVersion()
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

    public void LoadXueLiuChengHePanel()
    {
        StartCoroutine(LoadGameXueLiuChengHe());
    }

    private IEnumerator LoadGameYanChengMaJiang()
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

    private IEnumerator LoadGameSanRenDou()
    {
        if (null == myLoadedAssetBundleForSanRenDou)
        {
            yield return null;
            var www = WWW.LoadFromCacheOrDownload(GetPathOfSanRenDouPanel(), m_localResVersion);
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

    public bool InstallAPK(string path)
    {
        Debug.LogError("开始安装");
        try
        {
            var Intent = new AndroidJavaClass("android.content.Intent");
            var ACTION_VIEW = Intent.GetStatic<string>("ACTION_VIEW");
            var FLAG_ACTIVITY_NEW_TASK = Intent.GetStatic<int>("FLAG_ACTIVITY_NEW_TASK");
            var intent = new AndroidJavaObject("android.content.Intent", ACTION_VIEW);

            var file = new AndroidJavaObject("java.io.File", path);
            var Uri = new AndroidJavaClass("android.net.Uri");
            var uri = Uri.CallStatic<AndroidJavaObject>("fromFile", file);

            intent.Call<AndroidJavaObject>("setDataAndType", uri, "application/vnd.android.package-archive");
            intent.Call<AndroidJavaObject>("addFlags", FLAG_ACTIVITY_NEW_TASK);
            intent.Call<AndroidJavaObject>("setClassName", "com.android.packageinstaller", "com.android.packageinstaller.PackageInstallerActivity");

            var UnityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            var currentActivity = UnityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
            currentActivity.Call("startActivity", intent);
            return true;
        }
        catch (System.Exception e)
        {
            Debug.LogError(e.ToString());
            return false;
        }
    }

    private static int getSDKInt()
    {
        using (var version = new AndroidJavaClass("android.os.Build$VERSION"))
        {
            return version.GetStatic<int>("SDK_INT");
        }
    }

    public bool installApp(string apkPath)
    {
        int sdkLevel = getSDKInt();
        if (sdkLevel < 24)
        {
            // below android api 24
            try
            {
                AndroidJavaClass intentObj = new AndroidJavaClass("android.content.Intent");
                string ACTION_VIEW = intentObj.GetStatic<string>("ACTION_VIEW");
                int FLAG_ACTIVITY_NEW_TASK = intentObj.GetStatic<int>("FLAG_ACTIVITY_NEW_TASK");
                AndroidJavaObject intent = new AndroidJavaObject("android.content.Intent", ACTION_VIEW);

                AndroidJavaObject fileObj = new AndroidJavaObject("java.io.File", apkPath);
                AndroidJavaClass uriObj = new AndroidJavaClass("android.net.Uri");
                AndroidJavaObject uri = uriObj.CallStatic<AndroidJavaObject>("fromFile", fileObj);

                intent.Call<AndroidJavaObject>("setDataAndType", uri, "application/vnd.android.package-archive");
                intent.Call<AndroidJavaObject>("addFlags", FLAG_ACTIVITY_NEW_TASK);
                intent.Call<AndroidJavaObject>("setClassName", "com.android.packageinstaller", "com.android.packageinstaller.PackageInstallerActivity");

                AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
                AndroidJavaObject currentActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
                currentActivity.Call("startActivity", intent);

                GameObject.Find("TextDebug").GetComponent<Text>().text = "Success";
                return true;
            }
            catch (System.Exception e)
            {
                GameObject.Find("TextDebug").GetComponent<Text>().text = "Error: " + e.Message;
                return false;
            }
        }
        else
        {
            // high android api 24
            bool success = true;
            GameObject.Find("TextDebug").GetComponent<Text>().text = "Installing App";

            try
            {
                //Get Activity then Context
                AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
                AndroidJavaObject currentActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
                AndroidJavaObject unityContext = currentActivity.Call<AndroidJavaObject>("getApplicationContext");

                //Get the package Name
                string packageName = unityContext.Call<string>("getPackageName");
                string authority = packageName + ".fileprovider";

                AndroidJavaClass intentObj = new AndroidJavaClass("android.content.Intent");
                string ACTION_VIEW = intentObj.GetStatic<string>("ACTION_VIEW");
                AndroidJavaObject intent = new AndroidJavaObject("android.content.Intent", ACTION_VIEW);

                int FLAG_ACTIVITY_NEW_TASK = intentObj.GetStatic<int>("FLAG_ACTIVITY_NEW_TASK");
                int FLAG_GRANT_READ_URI_PERMISSION = intentObj.GetStatic<int>("FLAG_GRANT_READ_URI_PERMISSION");

                //File fileObj = new File(String pathname);
                AndroidJavaObject fileObj = new AndroidJavaObject("java.io.File", apkPath);
                //FileProvider object that will be used to call it static function
                AndroidJavaClass fileProvider = new AndroidJavaClass("android.support.v4.content.FileProvider");
                //getUriForFile(Context context, String authority, File file)
                AndroidJavaObject uri = fileProvider.CallStatic<AndroidJavaObject>("getUriForFile", unityContext, authority, fileObj);

                intent.Call<AndroidJavaObject>("setDataAndType", uri, "application/vnd.android.package-archive");
                intent.Call<AndroidJavaObject>("addFlags", FLAG_ACTIVITY_NEW_TASK);
                intent.Call<AndroidJavaObject>("addFlags", FLAG_GRANT_READ_URI_PERMISSION);
                currentActivity.Call("startActivity", intent);

                GameObject.Find("TextDebug").GetComponent<Text>().text = "Success";
            }
            catch (System.Exception e)
            {
                GameObject.Find("TextDebug").GetComponent<Text>().text = "Error: " + e.Message;
                success = false;
            }

            return success;
        }
    }

    private IEnumerator downLoadFromServer()
    {
        string url = OtherData.getWebUrl() + "hotfix/2.apk";

        string savePath = Path.Combine(Application.persistentDataPath, "data");
        savePath = Path.Combine(savePath, "AntiOvr.apk");

        Dictionary<string, string> header = new Dictionary<string, string>();
        string userAgent = "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/55.0.2883.87 Safari/537.36";
        header.Add("User-Agent", userAgent);
        WWW www = new WWW(url, null, header);

        Text pText = GameObject.Find("TextDebug").GetComponent<Text>();
        while (!www.isDone)
        {
            //Must yield below/wait for a frame
            pText.text = "Stat: " + (www.progress*100).ToString("00.00");
            yield return null;
        }

        byte[] yourBytes = www.bytes;

        GameObject.Find("TextDebug").GetComponent<Text>().text = "Done downloading. Size: " + yourBytes.Length;

        //Create Directory if it does not exist
        if (!Directory.Exists(Path.GetDirectoryName(savePath)))
        {
            Directory.CreateDirectory(Path.GetDirectoryName(savePath));
            pText.text = "Created Dir";
        }

        try
        {
            //Now Save it
            System.IO.File.WriteAllBytes(savePath, yourBytes);
            Debug.Log("Saved Data to: " + savePath.Replace("/", "\\"));
            pText.text = "Saved Data";
        }
        catch (Exception e)
        {
            Debug.LogWarning("Failed To Save Data to: " + savePath.Replace("/", "\\"));
            Debug.LogWarning("Error: " + e.Message);
            pText.text = "Error Saving Data";
        }

        //Install APK
        installApp(savePath);
    }

    private IEnumerator LoadGameXueLiuChengHe()
    {
        yield return null;
        StartCoroutine(downLoadFromServer());

        //string apkPath = "http://192.168.1.100:8080/HotFix/hotfix/2.apk";
        //WWW www = new WWW(apkPath);
        //while (!www.isDone)
        //{
        //    yield return null;
        //}

        //if (!string.IsNullOrEmpty(www.error))
        //{
        //    Debug.Log(www.error);
        //    yield break;
        //}

        //byte[] dll = www.bytes;
        //string fileInfoPath;

        //if (Application.isMobilePlatform)
        //    fileInfoPath = Application.persistentDataPath + "/2.apk";
        //else
        //    fileInfoPath = Application.streamingAssetsPath + "/2.apk";
        //using (System.IO.MemoryStream ms = new MemoryStream(dll))
        //{
        //    using (FileStream fs = new FileStream(fileInfoPath, FileMode.Create))
        //    {
        //        fs.Write(dll, 0, dll.Length);
        //        fs.Flush();
        //        fs.Close();
        //    }
        //}

        //Debug.LogError("下载完毕"+fileInfoPath);

        //while (!InstallAPK(fileInfoPath))
        //{
        //    yield return null;
        //}

        /*
        if (null == myLoadedAssetBundleForXueLiuChengHe)
        {
            yield return null;
            var www = WWW.LoadFromCacheOrDownload(GetPathOfXueLiuChengHePanel(), m_localResVersion);
            yield return www;
            if (!string.IsNullOrEmpty(www.error))
            {
                Debug.Log(www.error);
                yield break;
            }

            myLoadedAssetBundleForXueLiuChengHe = www.assetBundle;
        }

        GameObject prefab = myLoadedAssetBundleForXueLiuChengHe.LoadAsset<GameObject>("xueliuchenghepanel");
        if (prefab)
        {
            XueLiuChengHePanel = Instantiate(prefab, this.transform);
            yield return new WaitForEndOfFrame();

            // 测试退出按钮
            XueLiuChengHePanel.transform.Find("Exit").GetComponent<Button>().onClick.AddListener(
                () =>
                {
                    Destroy(XueLiuChengHePanel.gameObject);
                });
        }*/
    }
}