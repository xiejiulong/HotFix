using LitJson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotFix_Project
{
    class TaskDataScript_hotfix
    {
        public static void initJson(string json)
        {
            TaskDataScript.s_taskData.m_taskDataList.Clear();

            JsonData jsonData = JsonMapper.ToObject(json);
            TaskDataScript.s_taskData.m_taskDataList = JsonMapper.ToObject<List<TaskData>>(jsonData["task_list"].ToString());

            TaskDataScript.s_taskData.sortTask();

            // 更新的部分
            {
                if (OtherData_hotfix.getIsIosCheck())
                {
                    for (int i = TaskDataScript.s_taskData.m_taskDataList.Count - 1; i >= 0; i--)
                    {
                        if ((TaskDataScript.s_taskData.m_taskDataList[i].task_id == 213)||
                            (TaskDataScript.s_taskData.m_taskDataList[i].task_id == 217)||
                            (TaskDataScript.s_taskData.m_taskDataList[i].task_id == 218) ||
                            (TaskDataScript.s_taskData.m_taskDataList[i].task_id == 204) ||
                            (TaskDataScript.s_taskData.m_taskDataList[i].task_id == 205) ||
                            (TaskDataScript.s_taskData.m_taskDataList[i].task_id == 207) ||
                            (TaskDataScript.s_taskData.m_taskDataList[i].task_id == 210) ||
                            (TaskDataScript.s_taskData.m_taskDataList[i].task_id == 211))
                        {
                            TaskDataScript.s_taskData.m_taskDataList.RemoveAt(i);
                        }
                    }
                }
            }
        }
    }
}
