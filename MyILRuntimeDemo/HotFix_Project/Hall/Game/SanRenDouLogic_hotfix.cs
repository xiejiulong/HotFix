using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DG.Tweening;
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
                GameObject desk = logic.transform.Find("Desk").gameObject;
                if (desk)
                {
                    desk.GetComponent<Image>().color = new Color32(255,0,2,255);
                    desk.transform.DOLocalMove(new Vector3(200, 200), 1.0F).SetLoops(-1,LoopType.Yoyo);
                }
            }
        }
        public static void Update()
        {

        }
    }
}
