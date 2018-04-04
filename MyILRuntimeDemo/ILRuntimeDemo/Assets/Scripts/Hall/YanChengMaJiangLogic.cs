/*************************************************************************************
 * CLR version      : version 5.6.2f1
 * TargetFW Version : 5.0
 * Class Name       :
 * Machine Name     : SC-201609201141 Hp Win7
 * Name Space       :
 * File Name        :
 * Create Date      : 2017/06/09 14:01:32
 * Author           : XieJiulong
 *
 * Modify Date      :
 * Author           :
 * Description      :
 *************************************************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Hall;

public class YanChengMaJiangLogic : MonoBehaviour
{
    void Awake()
    {
        OtherData.s_YanChengMaJiangLogic = this;
    }

    void OnDestroy()
    {
        OtherData.s_YanChengMaJiangLogic = null;
    }

    void Start()
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("YanChengMaJiangLogic_hotfix", "Start"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.YanChengMaJiangLogic_hotfix", "Start", null, null);
        }

        Debug.LogError("进入盐城麻将逻辑 Start...");
    }

    void Update()
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("YanChengMaJiangLogic_hotfix", "Update"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.YanChengMaJiangLogic_hotfix", "Update", null, null);
            return;
        }
    }

    public void DoCoroutine(IEnumerator coroutine)
    {
        StartCoroutine(coroutine);
    }
}