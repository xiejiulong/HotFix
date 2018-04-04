using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace HotFix_Project
{
    class BuyGoodsPanelScript_hotfix
    {
        public static void buy(int money_type)
        {
            BuyGoodsPanelScript script = OtherData.s_buyGoodsPanelScript;
            BuyGoodsRequest buyGoodsRequest  = LogicEnginerScript.Instance.GetComponent<BuyGoodsRequest>();

            int totalPrice = 0;

            if (money_type == script.m_shopData.money_type)
            {
                totalPrice = script.m_shopData.price * script.m_goods_num;
            }
            else if (money_type == script.m_shopData.money_type2)
            {
                totalPrice = script.m_shopData.price2 * script.m_goods_num;
            }

            switch (money_type)
            {
                case 1:
                    if (UserData.gold < totalPrice)
                    {
                        ToastScript.createToast("金币不足,请前去充值");
                        return;
                    }
                    else
                    {
                        buyGoodsRequest.setGoodsInfo(script.m_shopData.goods_id, script.m_goods_num, money_type);
                        buyGoodsRequest.CallBack = script.onReceive_BuyGoods;
                        buyGoodsRequest.OnRequest();
                    }
                    break;

                case 2:
                    if (UserData.yuanbao < totalPrice)
                    {
                        ToastScript.createToast("元宝不足,请前去充值");
                        return;
                    }
                    else
                    {
                        buyGoodsRequest.setGoodsInfo(script.m_shopData.goods_id, script.m_goods_num, money_type);
                        buyGoodsRequest.CallBack = script.onReceive_BuyGoods;
                        buyGoodsRequest.OnRequest();
                    }
                    break;

                //人民币购买
                case 3:
                    if (!OtherData.s_canRecharge)
                    {
                        ToastScript.createToast("元宝购买暂未开放,敬请期待");
                        return;
                    }

                    if (UserData.IsRealName)
                    {
                        // ios
                        if (OtherData.s_channelName.CompareTo("ios") == 0)
                        {
                            PlatformHelper.pay(PlatformHelper.GetChannelName(), "AndroidCallBack", "GetPayResult", script.SetRequest(script.m_shopData));
                        }
                        // 渠道包
                        else if(AndroidPlatform.isShield(OtherData.s_channelName))
                        {
                            if (!ChannelHelper.Is3RdLogin())
                            {
                                PayTypePanelScript.create(script.m_shopData);
                            }
                            else
                            {
                                PlatformHelper.pay(PlatformHelper.GetChannelName(), "AndroidCallBack", "GetPayResult", script.SetRequest(script.m_shopData));
                            }
                        }
                        // 官方包
                        else
                        {
                            PayTypePanelScript.create(script.m_shopData);
                        }

                        GameObject.Destroy(script.gameObject);
                    }
                    else
                    {
                        // 部分渠道不需要做实名限制
                        if (ShieldShopRealName.isShield(OtherData.s_channelName))
                        {
                            // ios
                            if (OtherData.s_channelName.CompareTo("ios") == 0)
                            {
                                PlatformHelper.pay(PlatformHelper.GetChannelName(), "AndroidCallBack", "GetPayResult", script.SetRequest(script.m_shopData));
                            }
                            // 渠道包
                            else if (AndroidPlatform.isShield(OtherData.s_channelName))
                            {
                                if (!ChannelHelper.Is3RdLogin())
                                {
                                    PayTypePanelScript.create(script.m_shopData);
                                }
                                else
                                {
                                    PlatformHelper.pay(PlatformHelper.GetChannelName(), "AndroidCallBack", "GetPayResult", script.SetRequest(script.m_shopData));
                                }
                            }
                            // 官方包
                            else
                            {
                                PayTypePanelScript.create(script.m_shopData);
                            }

                            GameObject.Destroy(script.gameObject);
                        }
                        else
                        {
                            CommonExitPanelScript commonExit = CommonExitPanelScript.create().GetComponent<CommonExitPanelScript>();
                            commonExit.TextContent.text = "您还未实名,无法购买";
                            commonExit.ButtonClose.gameObject.SetActive(true);
                            commonExit.ButtonConfirm.onClick.AddListener(delegate ()
                            {
                                RealNameScript.create();
                                GameObject.Destroy(commonExit.gameObject);
                            });
                        }
                        
                    }
                    break;

                case 4:
                    if (UserData.medal < totalPrice)
                    {
                        ToastScript.createToast("徽章不足,无法购买");
                        return;
                    }
                    else
                    {
                        buyGoodsRequest.setGoodsInfo(script.m_shopData.goods_id, script.m_goods_num, money_type);
                        buyGoodsRequest.CallBack = script.onReceive_BuyGoods;
                        buyGoodsRequest.OnRequest();
                    }
                    break;
            }
        }
    }
}
