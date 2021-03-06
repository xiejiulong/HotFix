﻿using ILRuntime.Runtime.Enviorment;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
using System;
using LitJson;

namespace Hall
{
    public class ILRuntimeUtil: MonoBehaviour
    {
        static ILRuntimeUtil s_instance = null;
        static ILRuntime.Runtime.Enviorment.AppDomain s_appdomain = null;
        public string m_url = "";

        private const string DLL_VERSION = "dll_version";

        static List<string> s_funcList = new List<string>();
        private string fileInfoPath;

        public static ILRuntimeUtil getInstance()
        {
            return s_instance;
        }

        private void Awake()
        {
            s_instance = this;
        }

        public ILRuntime.Runtime.Enviorment.AppDomain getAppDomain()
        {
            return s_appdomain;
        }

        public void downDll(string url)
        {
            Debug.Log("dll下载地址：" + url);

            m_url = url;
            if (s_appdomain == null)
            {
                s_appdomain = new ILRuntime.Runtime.Enviorment.AppDomain();
            }

            if (Application.isMobilePlatform)
                fileInfoPath = Application.persistentDataPath + "/hotfix.dll";
            else
                fileInfoPath = Application.streamingAssetsPath + "/hotfix.dll";

            Debug.Log("dll保存地址：" + fileInfoPath);
            string dllVersion = PlayerPrefs.GetString(DLL_VERSION, "");

            if (string.IsNullOrEmpty(dllVersion) || !dllVersion.Equals(url) || !File.Exists(fileInfoPath))
            {
                Debug.Log("下载dll");
                StartCoroutine(LoadHotFixAssembly(url));
            }
            else
            {
                Debug.Log("加载缓存dll");
                using (FileStream fs = new FileStream(fileInfoPath, FileMode.Open))
                {
                    s_appdomain.LoadAssembly(fs, null, new Mono.Cecil.Pdb.PdbReaderProvider());
                }
                InitializeILRuntime();
                OnHotFixLoaded();
            }
        }

        IEnumerator LoadHotFixAssembly(string url)
        {
            WWW www = new WWW(url);

            while (!www.isDone)
            {
                yield return null;
            }

            try
            {
                if (!string.IsNullOrEmpty(www.error))
                {
                    UnityEngine.Debug.LogError(www.error);
                }

                byte[] dll = www.bytes;
                www.Dispose();

                if (!Directory.Exists(Path.GetDirectoryName(fileInfoPath)))
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(fileInfoPath));
                }
                using (System.IO.MemoryStream ms = new MemoryStream(dll))
                {
                    using (FileStream fs = new FileStream(fileInfoPath, FileMode.Create))
                    {
                        s_appdomain.LoadAssembly(ms, null, new Mono.Cecil.Pdb.PdbReaderProvider());
                        fs.Write(dll, 0, dll.Length);
                        PlayerPrefs.SetString(DLL_VERSION, url);
                    }
                }

                InitializeILRuntime();
                OnHotFixLoaded();
            }
            catch (Exception ex)
            {
                Debug.Log("LoadHotFixAssembly异常：" + ex);

                //NetLoading.getInstance().Close();

                //NetErrorPanelScript.getInstance().Show();
                //NetErrorPanelScript.getInstance().setOnClickButton(onClickDownDll);
                //NetErrorPanelScript.getInstance().setContentText("获取文件失败，请重新获取");
            }
        }

        void InitializeILRuntime()
        {
            //这里做一些ILRuntime的注册
            s_appdomain.DelegateManager.RegisterMethodDelegate<int>();

            //带返回值的委托的话需要用RegisterFunctionDelegate，返回类型为最后一个
            s_appdomain.DelegateManager.RegisterFunctionDelegate<int, string>();

            //Action<string> 的参数为一个string
            s_appdomain.DelegateManager.RegisterMethodDelegate<string>();
            s_appdomain.DelegateManager.RegisterMethodDelegate<System.String>();

            //s_appdomain.DelegateManager.RegisterDelegateConvertor<GetUserBagRequest.GetUserBagCallBack>((act) =>
            //{
            //    return new GetUserBagRequest.GetUserBagCallBack((result) =>
            //    {
            //        ((System.Action<System.String>)act)(result);
            //    });
            //});

            s_appdomain.DelegateManager.RegisterDelegateConvertor<UnityEngine.Events.UnityAction<float>>((action) =>
            {
                return new UnityEngine.Events.UnityAction<float>((a) => { ((System.Action<float>)action)(a); });
            });

            s_appdomain.DelegateManager.RegisterDelegateConvertor<UnityEngine.Events.UnityAction>((act) =>
            {
                return new UnityEngine.Events.UnityAction(() => { ((Action)act)(); });
            });

            //s_appdomain.DelegateManager.RegisterDelegateConvertor<NetErrorPanelScript.OnClickButton>((act) =>
            //{
            //    return new NetErrorPanelScript.OnClickButton(() => { ((Action)act)(); });
            //});

            s_appdomain.DelegateManager.RegisterDelegateConvertor<DG.Tweening.TweenCallback>((act) =>
            {
                return new DG.Tweening.TweenCallback(() => { ((Action)act)(); });
            });

            // add xiejl@2018.3.31
            //LitJson.JsonMapper.RegisterILRuntimeCLRRedirection(s_appdomain);

            s_appdomain.RegisterCrossBindingAdaptor(new CoroutineAdapter());

        }

        void OnHotFixLoaded()
        {
            Debug.Log("OnHotFixLoaded");

            /*
             * 保存dll所有的类-函数数据，这样只用调用一次dll就行了，不用每次都要问一下dll是否有xxx类-函数
             * 避免来回调用dll浪费时间
             */
            {
                s_funcList.Clear();
                List<string> funcList =
                    (List<string>)s_appdomain.Invoke("HotFix_Project.ClassRegister", "getFuncList", null, null);
                if (funcList != null)
                {
                    for (int i = 0; i < funcList.Count; i++)
                    {
                        Debug.Log("dll包含的类-函数：" + funcList[i]);
                        s_funcList.Add(funcList[i]);
                    }
                }
                else
                {
                    Debug.Log("没有dll可用");
                }
            }

            //NetLoading.getInstance().Close();
            PlayerPrefs.SetInt("Version", OtherData.s_login.m_codeVersion);
            Debug.LogError("设置本地版本号" + OtherData.s_login.m_codeVersion);
            OtherData.s_login.onDllGetOver();
        }

        public static bool checkClassHasFunc(string funcName)
        {
            for (int i = 0; i < s_funcList.Count; i++)
            {
                if (s_funcList[i].CompareTo(funcName) == 0)
                {
                    return true;
                }
            }

            return false;
        }

        /*funcName:类名.函数名 如：“MedalExplainPanelScript.onClickSetPsw”
        * 如果有的函数有多个重载，则这样写：MedalExplainPanelScript.onClickSetPsw(1)、MedalExplainPanelScript.onClickSetPsw(2)
        */
        public bool checkDllClassHasFunc(string className, string funcName)
        {
            if (s_appdomain == null)
            {
                s_appdomain = new ILRuntime.Runtime.Enviorment.AppDomain();
            }

            string param = className + "." + funcName;

            return checkClassHasFunc(param);
        }
    }
}
