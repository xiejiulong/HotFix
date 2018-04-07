using System.Collections.Generic;
using UnityEngine;

class CarpAssetBundleLoad : CarpAssetLoad
{
    private Dictionary<string, AssetBundleRequest> requestLoads = new Dictionary<string, AssetBundleRequest>();
    private Dictionary<string, AssetBundleCreateRequest> asyncAssetBundle = new Dictionary<string, AssetBundleCreateRequest>();

    public override void init()
    {
        loadManifest();
    }
    private void loadManifest()
    {
        if(!FileUtil.isExistFile(CarpAssetDefine.assetPath + CarpAssetDefine.manifestName))
        {
            Debug.LogError("ab依赖资源文件未找到~");
            return;
        }
        reqLoadAsset(CarpAssetDefine.manifestName, string.Empty, null, true);
        loadAssetImme(CarpAssetDefine.manifestName, string.Empty, CarpAssetDefine.manifestName, null);
        UnityEngine.Object manifest = assetsStore.getLoadObject(CarpAssetDefine.manifestName, string.Empty);
        if(manifest != null)
        {
            CarpAssetDefine.manifest = manifest as UnityEngine.AssetBundleManifest;
        }
    }

    public override bool checkAsyncAsset()
    {
        if (checkRequestLoad()) {
            return true;
        }
        if (checkExistAsset()) {
            return false;
        }
        if (checkDone()) {
            return true;
        }
        reqAssets(checkAsset,false);
        return true;
    }

    bool checkRequestLoad()
    {
        if (!requestLoads.ContainsKey(checkAsset.assetKey))
        {
            return false;
        }

        if (!isLoadDone(checkAsset.assetKey))
        {
            return true;
        }

        return loadAssetBundle(checkAsset);
    }

    bool isLoadDone(string ab)
    {
        return errorAssets[ab] || requestLoads[ab].isDone;
    }

    bool loadAssetBundle(CarpDefineAB checkAsset)
    {
        UnityEngine.Object loadObj = null;

        if (requestLoads[checkAsset.assetKey] != null && checkAsset.assetName.Equals(string.Empty))
        {
            loadObj = requestLoads[checkAsset.assetKey].allAssets[0];
        }
        else if(requestLoads[checkAsset.assetKey] != null)
        {
            loadObj = requestLoads[checkAsset.assetKey].asset;
        }
        requestLoads.Remove(checkAsset.assetKey);
        errorAssets.Remove(checkAsset.assetKey);
        assetsStore.addLoadObject(checkAsset.ab, checkAsset.assetName, loadObj);
        return false;
    }

    bool checkExistAsset()
    {
        if (!assetsStore.isExistLoadAsset(checkAsset.ab,checkAsset.assetName))
        {
            return false;
        }

        asyncAssetDone(checkAsset);
        willRequest.RemoveAt(checkIndex);
        return true;
    }

    bool checkDone()
    {
        if (!assetsStore.existAssetBundle(checkAsset.ab) && !asyncAssetBundle.ContainsKey(checkAsset.ab))
        {
            return false;
        }
        if (!isDone(checkAsset))
        {
            return true;
        }
        
        AssetBundle ab = assetsStore.getAssetBundle(checkAsset.ab);
        return loadAssetAsync(checkAsset,ab, checkAsset.loadType);
    }

    bool loadAssetAsync(CarpDefineAB checkAsset,AssetBundle assetBundle, System.Type t)
    {
        requestLoads[checkAsset.assetKey] = null;
        if (assetBundle == null)
        {
            errorAssets.Add(checkAsset.assetKey,true);
        }
        else if (checkAsset.assetName.Equals(string.Empty))
        {
            requestLoads[checkAsset.assetKey] = t == null ? assetBundle.LoadAllAssetsAsync() : assetBundle.LoadAllAssetsAsync(t);
            errorAssets.Add(checkAsset.assetKey, false);
        }
        else
        {
            bool exist = assetBundle.Contains(checkAsset.assetName);
            if(exist)
            {
                requestLoads[checkAsset.assetKey] = t == null ? assetBundle.LoadAssetAsync(checkAsset.assetName) : assetBundle.LoadAssetAsync(checkAsset.assetName, t);
            }
            checkError(!exist, string.Format("资源加载错误：（{0}），发生在加载资源内容阶段", checkAsset.assetKey));
            errorAssets.Add(checkAsset.assetKey, !exist);
        }
        return true;
    }

    void loadAssetImme(string ab,string assetName,string assetKey, System.Type t)
    {
        if (assetsStore.isExistLoadAsset(ab, assetName))
        {
            return;
        }

        AssetBundle assetBundle = assetsStore.getAssetBundle(ab);
        UnityEngine.Object obj = loadAsset(assetBundle, assetName,assetKey, t);
        assetsStore.addLoadObject(ab, assetName, obj);
    }

    UnityEngine.Object loadAsset(AssetBundle assetBundle, string assetName,string assetKey, System.Type t)
    {
        UnityEngine.Object obj = null;
        if (assetBundle!=null && t == null)
        {
            obj = assetName.Equals(string.Empty) ? assetBundle.LoadAllAssets()[0] : assetBundle.LoadAsset(assetName);
            checkError(obj==null, string.Format("资源加载错误：（{0}），发生在加载资源内容阶段", assetKey));
        }
        else if(assetBundle!=null)
        {
            obj = assetName.Equals(string.Empty) ? assetBundle.LoadAllAssets(t)[0] : assetBundle.LoadAsset(assetName, t);
            checkError(obj==null, string.Format("资源加载错误：（{0}），发生在加载资源内容阶段", assetKey));
        }
        return obj;
    }


    void checkState()
    {
        checkIndex = 0;
        checkAsset = null;
        enabled = willRequest.Count > 0;
    }

    bool isDone(CarpDefineAB define)
    {
        return assetDone(define.ab) ? isDependDone(define) : false;
    }
    bool isDependDone(CarpDefineAB define)
    {
        bool done = true;
        for (int i = 0; i < define.depends.Length && done; i++)
        {
            done = assetDone(define.depends[i]);
        }
        return done;
    }

    bool assetDone(string ab)
    {
        return assetsStore.existAssetBundle(ab) ? true : asyncAssetDone(ab);
    }

    bool asyncAssetDone(string ab)
    {
        if(!errorAssets[ab] && !asyncAssetBundle[ab].isDone)
        {
            return false;
        }

        assetsStore.addAssetBundle(ab, asyncAssetBundle[ab].assetBundle);
        asyncAssetBundle.Remove(ab);
        errorAssets.Remove(ab);
        return true;
    }

    bool existLoadAsset(string assetName)
    {
        return requestLoads.ContainsKey(assetName);
    }
    void reqAssets(CarpDefineAB define, bool imme)
    {
        for (int i = 0; i < define.depends.Length; i++)
        {
            reqLoadAsset(define.depends[i],string.Empty, null, imme);
        }
        reqLoadAsset(define.ab, define.assetName, define.loadType, imme);
    }

    private void reqLoadAsset(string ab, string assetName, System.Type t, bool imme)
    {
        if (isExistAsset(ab))
        {
            return;
        }

        loadFromFile(ab,imme);
    }

    void loadFromFile(string ab, bool imme)
    {
        if (imme)
        {
            AssetBundle abAssets = AssetBundle.LoadFromFile(CarpAssetDefine.assetPath + ab);
            checkError(abAssets==null, string.Format("资源加载错误：（{0}），发生在加载资源文件阶段", ab));
            assetsStore.addAssetBundle(ab, abAssets);
        }
        else
        {
            AssetBundleCreateRequest abAssets = AssetBundle.LoadFromFileAsync(CarpAssetDefine.assetPath + ab);
            checkError(abAssets.assetBundle==null, string.Format("资源加载错误：（{0}），发生在加载资源文件阶段", ab));
            errorAssets.Add(ab,abAssets.assetBundle==null);
            asyncAssetBundle.Add(ab, abAssets);
        }
    }

    bool isExistAsset(string ab)
    {
        return assetsStore.existAssetBundle(ab) || asyncAssetBundle.ContainsKey(ab);
    }

    public override void assetLoad(string ab, string assetName, System.Type t)
    {
        if (CarpAssetDefine.manifest == null) { return; }
        if (!existAsset(ab))
        {
            Debug.LogError("文件不存在：" + ab);
            return;
        }

        assetsStore.refAsset(ab);
        CarpDefineAB define = createDefineAB(ab, assetName, null, null, t, null, false, false);
        reqAssets(define, true);
        while(!isDone(define)){ }
        while(existLoadAsset(define.assetKey) && waitLoadDone(define)) {}
        loadAssetImme(ab,assetName,define.assetKey,t);
    }

    bool waitLoadDone(CarpDefineAB define)
    {
        if(!isLoadDone(define.assetKey))
        {
            return true;
        }
        return loadAssetBundle(define);
    }

    public override void assetLoadAsync(string ab,string assetName,System.Action<UnityEngine.Object, System.Object> cb,System.Action<CarpDefineAB>doneCB, System.Type t,System.Object arg, bool instantiate, bool frame)
    {
        ab = ab.ToLower();
        if (CarpAssetDefine.manifest == null) { return; }
        if(!existAsset(ab))
        {
            Debug.LogError("文件不存在：" + ab);
            return;
        }

        assetsStore.refAsset(ab);
        CarpDefineAB define = createDefineAB(ab,assetName, cb, doneCB, t,arg,instantiate,frame);
        willRequest.Add(define);
        enabled = true;
    }
    CarpDefineAB createDefineAB(string ab, string assetName,System.Action<UnityEngine.Object, System.Object> cb, System.Action<CarpDefineAB> doneCB, System.Type t, System.Object arg, bool instantiate, bool frame)
    {
        CarpDefineAB define = new CarpDefineAB();
        string[] depends = CarpAssetDefine.manifest.GetAllDependencies(ab);
        define.Bind(ab,assetName, depends, cb, doneCB,t, arg,instantiate,frame);
        return define;
    }
    bool existAsset(string ab)
    {
        Hash128 hash = CarpAssetDefine.manifest.GetAssetBundleHash(ab);
        return !hash.Equals(CarpAssetDefine.emptyHash);
    }
}
