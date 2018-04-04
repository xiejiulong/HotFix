using LitJson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotFix_Project
{
    class Activity_hotfix
    {
        public static void GetActivityData(string result)
        {
            JsonData jsonData = JsonMapper.ToObject(result);
            Activity.activityDatas = JsonMapper.ToObject<List<Activity.ActivityData>>(jsonData["activityDatas"].ToString());
            
            // 更新的部分
            {
                if (ShieldWeChat.isShield(OtherData.s_channelName))
                {
                    for (int i = (Activity.activityDatas.Count - 1); i >= 0 ; i--)
                    {
                        if (Activity.activityDatas[i].Title.CompareTo("关注微信公众号") == 0)
                        {
                            Activity.activityDatas.RemoveAt(i);
                        }
                    }
                }

                if (ShieldShare.isShield(OtherData.s_channelName))
                {
                    for (int i = (Activity.activityDatas.Count - 1); i >= 0; i--)
                    {
                        if (Activity.activityDatas[i].Title.CompareTo("大礼来袭") == 0)
                        {
                            Activity.activityDatas.RemoveAt(i);
                        }
                    }
                }
            }

            OtherData.s_activity.BtnActivity.onClick.Invoke();
        }

        public static void GetNoticeData(string result)
        {
            NoticelDataScript.getInstance().initJson(result);
            Activity.noticeDatas.Clear();
            foreach (var noticeData in NoticelDataScript.getInstance().getNoticeDataList())
            {
                if (noticeData.type == 1)
                {
                    Activity.noticeDatas.Add(noticeData);
                }
            }

            // 更新的部分
            {
                if (ShieldWeChat.isShield(OtherData.s_channelName))
                {
                    for (int i = (Activity.noticeDatas.Count - 1); i >= 0; i--)
                    {
                        if (Activity.noticeDatas[i].title.CompareTo("实名认证") == 0)
                        {
                            Activity.noticeDatas.RemoveAt(i);
                        }
                    }
                }
            }
        }
    }
}
