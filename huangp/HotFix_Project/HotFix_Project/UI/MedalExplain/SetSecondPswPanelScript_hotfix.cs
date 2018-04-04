using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine.UI;

namespace HotFix_Project
{
    class SetSecondPswPanelScript_hotfix
    {
        public static void Start()
        {
            LogicEnginerScript.Instance.GetComponent<SetSecondPswRequest>().CallBack = OtherData.s_setSecondPswPanelScript.onReceive_SetSecondPsw;

            // 更新的部分
            if (OtherData_hotfix.getIsIosCheck())
            {
                OtherData.s_setSecondPswPanelScript.transform.Find("Image_bg").Find("Text_tip").GetComponent<Text>().text = "徽章密码用于安全校验，为保障您的权益请设置您的徽章密码。";
            }
        }
    }
}
