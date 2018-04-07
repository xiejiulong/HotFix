using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarpAssets : MonoBehaviour
{
    private static CarpAssets instance;
    public static CarpAssets Instance
    {
        get
        {
            if (instance == null)
            {
                GameObject obj = new GameObject("_CarpAssets_");
                GameObject.DontDestroyOnLoad(obj);
                instance = obj.AddComponent<CarpAssets>();
            }
            return instance;
        }
    }

    CarpAssetLoad assetsLoad;
    CarpAssetsStore assetsStore;
    CarpInstantiate carpInstantiate;
    void Awake()
    {
        enabled = false;
    }

    public void init(string assetPath,bool abAssetType,string manifestName="")
    {
        CarpAssetDefine.assetPath = assetPath;
        CarpAssetDefine.manifestName = manifestName;
        assetsStore = gameObject.AddComponent<CarpAssetsStore>();

        if (abAssetType)
        {
            assetsLoad = gameObject.AddComponent<CarpAssetBundleLoad>();
        }
        else
        {
            assetsLoad = gameObject.AddComponent<CarpResourcesLoad>();
        }

        carpInstantiate = gameObject.AddComponent<CarpInstantiate>();
        assetsLoad.init();
    }

    public void setMaxReleaseTime(float time)
    {
        CarpAssetDefine.maxReleaseTime = time;
    }

    public void setMaxInstantiateTime(float time)
    {
        carpInstantiate.maxInstantiateTime = time;
    }

    //同步加载资源，不实例化资源
    public UnityEngine.Object loadAsset(string ab, System.Type t)
    {
        ab = addLoadAsset(ab, string.Empty, t);
        return assetsStore.getLoadObject(ab, string.Empty);
    }

    public UnityEngine.Object loadAsset(string ab, string assetName, System.Type t)
    {
        ab = addLoadAsset(ab, assetName,t);
        return assetsStore.getLoadObject(ab, assetName);
    }

    //同步加载资源，实例化资源
    public UnityEngine.Object createAsset(string ab, System.Type t)
    {
        ab = addLoadAsset(ab, string.Empty, t);
        return createAssets(ab, string.Empty);
    }

    public UnityEngine.Object createAsset(string ab, string assetName, System.Type t)
    {
        ab = addLoadAsset(ab, assetName, t);
        return createAssets(ab,assetName);
    }

    string addLoadAsset(string ab,string assetName, System.Type t)
    {
        ab = ab.ToLower();
        assetsLoad.assetLoad(ab, assetName, t);
        return ab;
    }

    Object createAssets(string ab,string assetName)
    {
        UnityEngine.Object obj = assetsStore.getLoadObject(ab, assetName);
        return carpInstantiate.createAssetEntity(obj, ab);
    }


    //异步加载资源，不实例化资源
    public void loadAssetAsync(string ab, System.Action<UnityEngine.Object, System.Object> cb, System.Type t, System.Object arg = null)
    {
        assetsLoad.assetLoadAsync(ab,string.Empty,cb,assetAsyncDone,t,arg,false,false);
    }

    public void loadAssetAsync(string ab, string assetName,System.Action<UnityEngine.Object, System.Object> cb, System.Type t, System.Object arg = null)
    {
        assetsLoad.assetLoadAsync(ab, assetName, cb, assetAsyncDone,t, arg, false, false);
    }

    //异步加载资源，实例化资源
    public void createAssetAsync(string ab, System.Action<UnityEngine.Object, System.Object> cb, System.Type t, System.Object arg = null)
    {
        assetsLoad.assetLoadAsync(ab,string.Empty,cb, assetAsyncDone,t, arg, true,false);
    }

    public void createAssetAsync(string ab,string assetName, System.Action<UnityEngine.Object, System.Object> cb, System.Type t, System.Object arg = null)
    {
        assetsLoad.assetLoadAsync(ab, assetName, cb, assetAsyncDone, t,arg, true, false);
    }

    //异步加载资源，通过分帧控制实例化
    public void createAssetFrameAsync(string ab, System.Action<UnityEngine.Object, System.Object> cb, System.Type t, System.Object arg = null)
    {
        assetsLoad.assetLoadAsync(ab,string.Empty,cb, assetAsyncDone,t, arg, true, true);
    }

    public void createAssetFrameAsync(string ab,string assetName, System.Action<UnityEngine.Object, System.Object> cb, System.Type t,System.Object arg = null)
    {
        assetsLoad.assetLoadAsync(ab, assetName, cb, assetAsyncDone,t, arg, true, true);
    }

    //实例化一个资源，通过已经实例化过的资源
    public UnityEngine.Object createExistAsset(UnityEngine.Object asset)
    {
        return carpInstantiate.instCreateExistEntity(asset);
    }

    //分帧控制实例化一个资源，通过已经实例化过的资源
    public void createExistAssetFrame(UnityEngine.Object asset, System.Action<UnityEngine.Object, System.Object> cb, System.Object arg = null)
    {
        carpInstantiate.willFrameCreateExistEntity(asset, cb, arg);
    }

    //清除一个实体资源
    public void removeEntity(UnityEngine.Object asset)
    {
        carpInstantiate.removeEntity(asset);
    }

    //清除一个ab包
    public void clearAssetBundle(string ab)
    {
        ab = ab.ToLower();
        carpInstantiate.clearAssetBundle(ab);
    }

    void assetAsyncDone(CarpDefineAB define)
    {
        UnityEngine.Object obj = assetsStore.getLoadObject(define.ab,define.assetName);

        if(assetBundleLoad(obj,define))
        {
            return;
        }

        if(frameCreateEntity(obj, define))
        {
            return;
        }

        UnityEngine.Object instObj = carpInstantiate.createAssetEntity(obj, define.ab);
        define.callback(instObj, define.arg);
    }

    bool assetBundleLoad(UnityEngine.Object obj,CarpDefineAB define)
    {
        if(define.instantiate)
        {
            return false;
        }
        define.callback(obj, define.arg);
        return true;
    }

    bool frameCreateEntity(UnityEngine.Object obj, CarpDefineAB define)
    {
        if (!define.frame)
        {
            return false;
        }
        carpInstantiate.willFrameCreateEntity(obj, define.ab, define.callback, define.arg);
        return true;
    }

    public void unusedAssets()
    {
        assetsStore.unloadUnusedAssets();
    }
}