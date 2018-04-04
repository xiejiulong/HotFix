using LitJson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotFix_Project
{
    class PVPGameRoomDataScript_hotfix
    {
        public static void initJson(string json)
        {
            PVPGameRoomDataScript.s_instance.m_dataList.Clear();

            JsonData jsonData = JsonMapper.ToObject(json);

            for (int i = 0; i < jsonData["room_list"].Count; i++)
            {
                PVPGameRoomData temp = new PVPGameRoomData();

                temp.id = (int)jsonData["room_list"][i]["id"];
                temp.gameroomtype = (string)jsonData["room_list"][i]["gameroomtype"];
                temp.gameroomname = (string)jsonData["room_list"][i]["gameroomname"];
                temp.kaisairenshu = (int)jsonData["room_list"][i]["kaisairenshu"];
                temp.baomingrenshu = (int)jsonData["room_list"][i]["baomingrenshu"];

                // 报名费
                {
                    temp.baomingfei = (string)jsonData["room_list"][i]["baomingfei"];

                    if (temp.baomingfei.CompareTo("0") != 0)
                    {
                        List<string> list = new List<string>();
                        CommonUtil.splitStr(temp.baomingfei, list, ':');

                        temp.baomingfei_id = int.Parse(list[0]);
                        temp.baomingfei_num = int.Parse(list[1]);
                    }
                }

                // 奖励
                {
                    temp.reward = (string)jsonData["room_list"][i]["reward"];

                    List<string> list = new List<string>();
                    CommonUtil.splitStr(temp.reward, list, ':');

                    temp.reward_id = int.Parse(list[0]);
                    temp.reward_num = int.Parse(list[1]);
                }

                // 已报名人数增加点
                temp.baomingrenshu += RandomUtil.getRandom(100, 200);

                // 更新的部分
                {
                    // 用于ios审核的场次：5000金币场
                    if (temp.id == 5)
                    {
                        if (OtherData_hotfix.getIsIosCheck())
                        {
                            PVPGameRoomDataScript.s_instance.m_dataList.Add(temp);
                        }
                    }
                    else
                    {
                        PVPGameRoomDataScript.s_instance.m_dataList.Add(temp);
                    }
                }
            }
        }
    }
}
