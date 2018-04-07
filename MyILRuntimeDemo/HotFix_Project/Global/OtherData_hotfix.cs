using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using UnityEngine;

namespace HotFix_Project
{
    class OtherData_hotfix
    {
        public static string getWebUrl()
        {
            string strHotProjectDllWebUrl = "http://192.168.1.100:8080/HotFix/";
            UnityEngine.Debug.Log("getWebUrl 获取HotFix Dll url");

            return strHotProjectDllWebUrl;
        }
    }
}
