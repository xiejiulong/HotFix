using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DG.Tweening;
using Hall;
using UnityEngine;
using UnityEngine.UI;

namespace HotFix_Project
{
    class YanChengMaJiangLogic_hotfix
    {
        public static void Start()
        {
            UnityEngine.Debug.LogError("Hi，进入YanChengMaJiangLogic_hotfix.Start..");
            YanChengMaJiangLogic logic = OtherData.s_YanChengMaJiangLogic;
            logic.StartCoroutine(ChangeColor());
            logic.StartCoroutine(ChangeSize());

            logic.transform.Find("Desk").gameObject.GetComponent<Image>().color = new Color32(255, 255, 0, 255);
        }

        public static IEnumerator ChangeColor()
        {
            YanChengMaJiangLogic logic = OtherData.s_YanChengMaJiangLogic;

            while (true)
            {
                Transform Players = logic.transform.GetChild(0);

                Debug.LogError("子个数：" + Players.childCount);

                for (int i = 0; i < Players.childCount; i++)
                {
                    Transform child = Players.GetChild(i);
                    if (child)
                    {
                        child.gameObject.GetComponent<Image>().color =
                            new Color32(
                                (byte)UnityEngine.Random.Range(0, 255),
                                (byte)UnityEngine.Random.Range(0, 255),
                                (byte)UnityEngine.Random.Range(0, 255),
                                (byte)UnityEngine.Random.Range(0, 255));
                    }
                    yield return null;
                }

                yield return null;
                Debug.LogError("还在继续更换");
            }
        }

        public static IEnumerator ChangeSize()
        {
            yield return null;
            YanChengMaJiangLogic logic = OtherData.s_YanChengMaJiangLogic;
            Transform Players = logic.transform.GetChild(0);
            for (int i = 0; i < Players.childCount; i++)
            {
                Transform child = Players.GetChild(i);
                if (child)
                {
                    child.DOScale(new Vector3(1.2f, 1.2f, 1.2f), 1f).SetLoops(-1,LoopType.Yoyo);
                }
                yield return null;
            }

        }

        public static void Update()
        {
            UnityEngine.Debug.LogError("Hi，进入YanChengMaJiangLogic_hotfix.Update..");
        }

    }
}
