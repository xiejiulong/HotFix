using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hall;
using UnityEngine;
using UnityEngine.UI;

namespace HotFix_Project
{
    class SanRenDouLogic_hotfix
    {
        public static void Start()
        {
            SanRenDouLogic logic = OtherData.s_SanRenDouLogic;
            if (logic)
            {
                logic.transform.Find("Desk").gameObject.GetComponent<Image>().color = new Color32(255,0,2,255);
            }
        }
        public static void Update()
        {

        }
    }
}
