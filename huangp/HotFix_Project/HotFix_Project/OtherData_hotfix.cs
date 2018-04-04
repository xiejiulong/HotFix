using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotFix_Project
{
    class OtherData_hotfix
    {
        public static bool s_isIosCheck = false;
        public static string s_clientVersion = "1.1.0";

        public static bool getIsIosCheck()
        {
            if (s_isIosCheck)
            {
                if (OtherData.s_channelName.CompareTo("ios") == 0)
                {
                    if (OtherData.s_apkVersion.CompareTo(s_clientVersion) == 0)
                    {
                        return true;
                    }
                }
            }

            return false;
        }
    }
}
