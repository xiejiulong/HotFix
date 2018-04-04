using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace HotFix_Project
{
    class BagPanelScript_hotfix
    {
        public static void onInitializeItem(GameObject go, int dataindex)
        {
            Text propName = go.transform.Find("PropName").GetComponent<Text>();
            Image propImage = go.transform.Find("PropImage").GetComponent<Image>();
            propName.text = UserData.propData[dataindex].prop_name + "*" + UserData.propData[dataindex].prop_num;
            propImage.sprite = Resources.Load<Sprite>("Sprites/Icon/Prop/" + UserData.propData[dataindex].prop_icon);


            Button button = go.GetComponent<Button>();
            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(delegate ()
            {
                // 显示道具详情
                PropDetailPanelScript.create(UserData.propData[dataindex].prop_id, OtherData.s_bagPanelScript);
            });

            // 更新的部分
            {
                propName.GetComponent<RectTransform>().sizeDelta = new Vector2(92,46);
            }
        }
    }
}
