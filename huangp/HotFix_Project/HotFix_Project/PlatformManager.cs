using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotFix_Project
{
    class AndroidPlatform
    {
        static bool s_isInit = false;
        static List<string> s_channelNameList = new List<string>();

        static void init()
        {
            s_channelNameList.Add("qihoo360");
            s_channelNameList.Add("huawei");
            s_channelNameList.Add("vivo");
            s_channelNameList.Add("baidu");
            s_channelNameList.Add("baidudk");
            s_channelNameList.Add("baidu91");
            s_channelNameList.Add("baidutb");
            s_channelNameList.Add("oppo");
            s_channelNameList.Add("yyb");
            s_channelNameList.Add("xiaomi");

            s_isInit = true;
        }

        public static bool isShield(string platform)
        {
            if (!s_isInit)
            {
                init();
            }

            for (int i = 0; i < s_channelNameList.Count; i++)
            {
                if (s_channelNameList[i].CompareTo(platform) == 0)
                {
                    return true;
                }
            }

            return false;
        }
    }

    class ShieldShare
    {
        static bool s_isInit = false;
        static List<string> s_channelNameList = new List<string>();
        
        static void init()
        {
            // 永久屏蔽
            s_channelNameList.Add("huawei");

            //s_channelNameList.Add("qihoo360");
            //s_channelNameList.Add("vivo");
            //s_channelNameList.Add("baidu");
            //s_channelNameList.Add("baidudk");
            //s_channelNameList.Add("baidu91");
            //s_channelNameList.Add("baidutb");
            s_channelNameList.Add("oppo");          // 暂时屏蔽，没有key

            if (OtherData_hotfix.getIsIosCheck())
            {
                s_channelNameList.Add("ios");
            }

            s_isInit = true;
        }

        public static bool isShield(string channelName)
        {
            if (!s_isInit)
            {
                init();
            }

            for (int i = 0; i < s_channelNameList.Count; i++)
            {
                if (s_channelNameList[i].CompareTo(channelName) == 0)
                {
                    return true;
                }
            }

            return false;
        }
    }

    class ShieldWeChat
    {
        static bool s_isInit = false;
        static List<string> s_channelNameList = new List<string>();

        static void init()
        {
            //s_channelNameList.Add("qihoo360");
            //s_channelNameList.Add("huawei");
            //s_channelNameList.Add("vivo");
            //s_channelNameList.Add("baidu");
            //s_channelNameList.Add("baidudk");
            //s_channelNameList.Add("baidu91");
            //s_channelNameList.Add("baidutb");

            if (OtherData_hotfix.getIsIosCheck())
            {
                s_channelNameList.Add("ios");
            }

            s_isInit = true;
        }

        public static bool isShield(string channelName)
        {
            if (!s_isInit)
            {
                init();
            }

            for (int i = 0; i < s_channelNameList.Count; i++)
            {
                if (s_channelNameList[i].CompareTo(channelName) == 0)
                {
                    return true;
                }
            }

            return false;
        }
    }

    class ShieldRealName
    {
        static bool s_isInit = false;
        static List<string> s_channelNameList = new List<string>();

        static void init()
        {
            //s_channelNameList.Add("ios");
            //s_channelNameList.Add("qihoo360");
            //s_channelNameList.Add("huawei");
            //s_channelNameList.Add("vivo");
            //s_channelNameList.Add("baidu"); 
            //s_channelNameList.Add("baidudk");
            //s_channelNameList.Add("baidu91");
            //s_channelNameList.Add("baidutb");
            s_channelNameList.Add("oppo");

            s_isInit = true;
        }

        public static bool isShield(string channelName)
        {
            if (!s_isInit)
            {
                init();
            }

            for (int i = 0; i < s_channelNameList.Count; i++)
            {
                if (s_channelNameList[i].CompareTo(channelName) == 0)
                {
                    return true;
                }
            }

            return false;
        }
    }

    class ShieldShopRealName
    {
        static bool s_isInit = false;
        static List<string> s_channelNameList = new List<string>();

        static void init()
        {
            //s_channelNameList.Add("qihoo360");
            //s_channelNameList.Add("huawei");
            //s_channelNameList.Add("vivo");
            //s_channelNameList.Add("baidu");
            //s_channelNameList.Add("baidudk");
            //s_channelNameList.Add("baidu91");
            //s_channelNameList.Add("baidutb");
            s_channelNameList.Add("oppo");

            if (OtherData_hotfix.getIsIosCheck())
            {
                s_channelNameList.Add("ios");
            }

            s_isInit = true;
        }

        public static bool isShield(string channelName)
        {
            if (!s_isInit)
            {
                init();
            }

            for (int i = 0; i < s_channelNameList.Count; i++)
            {
                if (s_channelNameList[i].CompareTo(channelName) == 0)
                {
                    return true;
                }
            }

            return false;
        }
    }

    class ShieldThirdLogin
    {
        static bool s_isInit = false;
        static List<string> s_channelNameList = new List<string>();

        static void init()
        {
            //s_channelNameList.Add("vivo");

            if (OtherData_hotfix.getIsIosCheck())
            {
                s_channelNameList.Add("ios");
            }

            s_isInit = true;
        }

        public static bool isShield(string channelName)
        {
            if (!s_isInit)
            {
                init();
            }

            for (int i = 0; i < s_channelNameList.Count; i++)
            {
                if (s_channelNameList[i].CompareTo(channelName) == 0)
                {
                    return true;
                }
            }

            return false;
        }
    }
}
