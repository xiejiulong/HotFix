using System.Collections.Generic;
using UnityEngine;
using System.IO;

class CarpResourcesLoad : CarpAssetLoad
{
    private Dictionary<string, ResourceRequest> asyncLoads = new Dictionary<string, ResourceRequest>();
    public override bool checkAsyncAsset()
    {
        if (checkExistAsset())
        {
            return false;
        }
        if (checkDone())
        {
            return true;
        }
        reqLoadAsset(checkAsset.ab,checkAsset.loadType,false);
        return true;
    }

    bool checkDone()
    {
        if (!asyncLoads.ContainsKey(checkAsset.ab))
        {
            return false;
        }
        if (!isDone(checkAsset.ab))
        {
            return true;
        }

        return addAsyncAsset(checkAsset.ab);
    }

    bool checkExistAsset()
    {
        if (!assetsStore.isExistLoadAsset(checkAsset.ab, string.Empty))
        {
            return false;
        }

        asyncAssetDone(checkAsset);
        willRequest.RemoveAt(checkIndex);
        return true;
    }

    private void reqLoadAsset(string res, System.Type t, bool imme)
    {
        if (isExistAsset(res))
        {
            return;
        }

        if (imme)
        {
            UnityEngine.Object obj = t == null ? Resources.Load(res) : Resources.Load(res, t);
            checkError(obj==null,"资源加载错误："+res);
            assetsStore.addLoadObject(res, string.Empty, obj);
        }
        else
        {
            ResourceRequest req = t == null ? Resources.LoadAsync(res) : Resources.LoadAsync(res, t);
            checkError(req.asset==null, "资源加载错误：" + res);
            errorAssets.Add(res,req.asset==null);
            asyncLoads.Add(res, req);
        }
    }

    bool isExistAsset(string ab)
    {
        return assetsStore.isExistLoadAsset(ab, string.Empty) || asyncLoads.ContainsKey(ab);
    }

    bool addAsyncAsset(string res)
    {
        assetsStore.addLoadObject(res, string.Empty, asyncLoads[res].asset);
        asyncLoads.Remove(res);
        errorAssets.Remove(res);
        return true;
    }

    bool isDone(string res)
    {
        return assetsStore.isExistLoadAsset(res, string.Empty) || errorAssets[res] || asyncLoads[res].isDone;
    }

    public override void assetLoad(string res, string assetName, System.Type t)
    {
        assetsStore.refAsset(res);
        reqLoadAsset(res,t,true);
        while (!isDone(res)) { }
        while (existLoadAsset(res) && waitLoadDone(res)) { }
    }

    bool existLoadAsset(string res)
    {
        return asyncLoads.ContainsKey(res);
    }

    bool waitLoadDone(string res)
    {
        return !isLoadDone(res) ? true : addAsyncAsset(res);
    }

    bool isLoadDone(string res)
    {
        return asyncLoads[res].isDone;
    }

    public override void assetLoadAsync(string res, string assetName, System.Action<UnityEngine.Object, System.Object> cb, System.Action<CarpDefineAB> doneCB, System.Type t, System.Object arg, bool instantiate, bool frame)
    {
        res = res.ToLower();
        assetsStore.refAsset(res);
        CarpDefineAB define = createDefineAB(res, string.Empty, cb, doneCB, t, arg, instantiate, frame);
        willRequest.Add(define);
        enabled = true;
    }

    CarpDefineAB createDefineAB(string res, string assetName, System.Action<UnityEngine.Object, System.Object> cb, System.Action<CarpDefineAB> doneCB, System.Type t, System.Object arg, bool instantiate, bool frame)
    {
        CarpDefineAB define = new CarpDefineAB();
        define.Bind(res, assetName, null, cb, doneCB, t, arg, instantiate, frame);
        return define;
    }
}
