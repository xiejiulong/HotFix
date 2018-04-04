using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Hall
{
    public class OtherData
    {
        public static Login s_login = null;
        public static ResHotFix s_ResHotFix = null;
        public static YanChengMaJiangLogic s_YanChengMaJiangLogic = null;

        public static Vector2 s_screenSize;

        // web测试服
        static string s_webStorageUrl_test = "http://192.168.0.101/HotFix/";

        // web正式服
        static string s_webStorageUrl = "http://192.168.0.101/HotFix/";

        public static string getWebUrl()
        {
            // 优先使用热更新的代码
            if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("OtherData_hotfix", "getWebUrl"))
            {
                string s = (string)ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.OtherData_hotfix", "getWebUrl", null, null);
                return s;
            }

            return s_webStorageUrl_test;
        }
    }
}
