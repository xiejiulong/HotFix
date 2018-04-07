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
using Hall;
using UnityEngine;

public class SanRenDouLogic : MonoBehaviour
{
    void Awake()
    {
        OtherData.s_SanRenDouLogic = this;
    }

    void OnDestroy()
    {
        OtherData.s_SanRenDouLogic = null;
    }

    // Use this for initialization
    void Start()
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("SanRenDouLogic_hotfix", "Start"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.SanRenDouLogic_hotfix", "Start", null, null);
        }

        Debug.LogError("进入盐城麻将逻辑 Start...");
    }

    // Update is called once per frame
    void Update()
    {
        // 优先使用热更新的代码
        if (ILRuntimeUtil.getInstance().checkDllClassHasFunc("SanRenDouLogic_hotfix", "Update"))
        {
            ILRuntimeUtil.getInstance().getAppDomain().Invoke("HotFix_Project.SanRenDouLogic_hotfix", "Update", null, null);
            return;
        }
    }
}