using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace HotFix_Project
{
    class PropDetailPanelScript_hotfix
    {
        public static void setPropId(int prop_id)
        {
            try
            {
                PropDetailPanelScript script = GameObject.Find("Canvas_Middle/PropDetailPanel(Clone)").GetComponent<PropDetailPanelScript>();
                script.m_propInfo = PropData.getInstance().getPropInfoById(prop_id);

                if (script.m_propInfo != null)
                {
                    script.m_text_name.text = script.m_propInfo.m_name;
                    script.m_text_desc.text = script.m_propInfo.m_desc;
                    CommonUtil.setImageSprite(script.m_image_icon, GameUtil.getPropIconPath(script.m_propInfo.m_id));

                    if (script.m_propInfo.m_type != 0)
                    {
                        script.m_button_use.transform.localScale = new Vector3(0, 0, 0);
                    }

                    // 一元话费：显示数量调节对象
                    if (script.m_propInfo.m_id == (int)TLJCommon.Consts.Prop.Prop_1yuanhuafei)
                    {
                        int myHuaFei_1 = GameUtil.getMyPropNumById((int)TLJCommon.Consts.Prop.Prop_1yuanhuafei);
                        if (myHuaFei_1 >= 10)
                        {
                            GameUtil.showGameObject(script.m_changeNum);
                            script.m_useNum = 10;

                            script.m_changeNum.transform.Find("Button_jia").GetComponent<Button>().interactable = false;
                        }
                        else if (myHuaFei_1 >= 5)
                        {
                            GameUtil.showGameObject(script.m_changeNum);
                            script.m_useNum = 5;

                            script.m_changeNum.transform.Find("Button_jia").GetComponent<Button>().interactable = false;
                        }

                        script.m_changeNum.transform.Find("Text_num").GetComponent<Text>().text = script.m_useNum.ToString();
                    }

                    if ((script.m_propInfo.m_id == 111) ||
                        (script.m_propInfo.m_id == 112) ||
                        (script.m_propInfo.m_id == 113))
                    {
                        GameObject obj = new GameObject();
                        Text text = obj.AddComponent<Text>();
                        text.text = "完成一次对局方可使用";
                        text.fontSize = 20;
                        CommonUtil.setFontColor(text,0,0,0);
                        text.font = script.transform.Find("Image_bg/Text_desc").GetComponent<Text>().font;
                        text.GetComponent<RectTransform>().sizeDelta = new Vector2(250, 30);

                        obj.transform.SetParent(script.transform.Find("Image_bg"));
                        obj.transform.localPosition = new Vector3(26, 30, 0);
                        obj.transform.localScale = new Vector3(1, 1, 1);
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.Log("----"+ex);
            }
        }
    }
}
