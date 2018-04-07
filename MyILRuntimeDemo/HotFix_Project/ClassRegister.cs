using System.Collections.Generic;

namespace HotFix_Project
{
    /// <summary>
    /// 所有主工程需要更新的函数都必须在这里注册
    /// 如果不注册，即使写了更新的函数也不会生效
    /// </summary>
    internal class ClassRegister
    {
        private static List<string> s_funcList = new List<string>();
        private static bool s_hasInit = false;

        /// <summary>
        /// 为某个脚本添加需要热更的函数
        /// </summary>
        private static void init()
        {
            // 全局变量热更
            {
                s_funcList.Add("OtherData_hotfix.getWebUrl");
            }

            // 登录
            {
                // Login_hotfix
                s_funcList.Add("Login_hotfix.Start");
                s_funcList.Add("Login_hotfix.Update");
                s_funcList.Add("Login_hotfix.OnDestroy");
                s_funcList.Add("Login_hotfix.OnHotFixTest");
            }

            // 盐城麻将
            {
                s_funcList.Add("YanChengMaJiangLogic_hotfix.Start");
                s_funcList.Add("YanChengMaJiangLogic_hotfix.Update");
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public static List<string> getFuncList()
        {
            if (!s_hasInit)
            {
                init();

                s_hasInit = true;
            }

            return s_funcList;
        }

        /*funcName:类名.函数名 如：“MedalExplainPanelScript.onClickSetPsw”
         * 如果有的函数有多个重载，则这样写：MedalExplainPanelScript.onClickSetPsw(1)、MedalExplainPanelScript.onClickSetPsw(2)
         * init函数注册的时候也得按照上面的对应关系
         */
        public static bool checkClassHasFunc(string funcName)
        {
            if (!s_hasInit)
            {
                init();

                s_hasInit = true;
            }

            for (int i = 0; i < s_funcList.Count; i++)
            {
                if (s_funcList[i].CompareTo(funcName) == 0)
                {
                    return true;
                }
            }

            return false;
        }
    }
}