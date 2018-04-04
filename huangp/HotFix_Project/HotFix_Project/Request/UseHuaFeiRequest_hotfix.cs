using LitJson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TLJCommon;

namespace HotFix_Project
{
    class UseHuaFeiRequest_hotfix
    {
        public static void OnRequest()
        {
            try
            {
                JsonData jsonData = new JsonData();
                jsonData["tag"] = Consts.Tag_UseHuaFei;
                jsonData["uid"] = UserData.uid;
                jsonData["prop_id"] = LogicEnginerScript.Instance.GetComponent<UseHuaFeiRequest>().m_prop_id;
                jsonData["phone"] = LogicEnginerScript.Instance.GetComponent<UseHuaFeiRequest>().m_phone;

                // 更新的部分
                {
                    if (OtherData.s_channelName.CompareTo("ios") == 0)
                    {
                        jsonData["prop_num"] = 1;
                    }
                    else
                    {
                        jsonData["prop_num"] = LogicEnginerScript.Instance.GetComponent<UseHuaFeiRequest>().m_prop_num;
                    }
                }

                string requestData = jsonData.ToJson();
                LogicEnginerScript.Instance.SendMyMessage(requestData);
            }
            catch (Exception ex)
            {
            }
        }
    }
}
