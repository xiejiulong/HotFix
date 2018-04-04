using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotFix_Project
{
    class GameUtil_hotfix
    {
        public static string getOneTips()
        {
            string str = "";
            {
                int i = RandomUtil.getRandom(1, 3);
                if (i == 1)
                {
                    str = "弹框界面点击屏幕空白处返回";
                }
                else if (i == 2)
                {
                    str = "关注微信公众号：“星焱娱乐”，即送丰厚大礼噢~";

                    if (ShieldWeChat.isShield(OtherData.s_channelName))
                    {
                        str = "欢迎来到疯狂升级 点击喇叭可进行全服喊话哦~~";
                    }
                }
                else if (i == 3)
                {
                    str = "每日19:00、20:00、21:00、22:00整点，普通场（经典、抄底）有话费碎片掉落活动！";
                }

                if (OtherData_hotfix.getIsIosCheck())
                {
                    str = "弹框界面点击屏幕空白处返回";
                }

                return str;
            }
        }
    }
}
