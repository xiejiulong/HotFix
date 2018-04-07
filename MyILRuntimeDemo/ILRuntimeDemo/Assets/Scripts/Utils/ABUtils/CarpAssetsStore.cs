using UnityEngine;
using System.Collections.Generic;

public class CarpAssetsStore:MonoBehaviour
{
    private Dictionary<string, float> loadUseTime = new Dictionary<string, float>();
    private List<string> clearAssets = new List<string>();
    private Dictionary<string, Dictionary<string, UnityEngine.Object>> loadObjects = new Dictionary<string, Dictionary<string, UnityEngine.Object>>();
    private Dictionary<string, AssetBundle> mRequestImme = new Dictionary<string, AssetBundle>();
    private Dictionary<string, int> assetRefTotals = new Dictionary<string, int>();
    private float nowTime = 0.0f;
    private int refCount = 0;

    void Awake()
    {
        enabled = false;
    }

    public void Update()
    {
        nowTime = Time.time;
        foreach (var v in loadUseTime)
        {
            checkRelAsset(v.Key, v.Value);
        }
        clearRelAssets();
        checkState();
    }

    public void unloadUnusedAssets()
    {
        foreach (var v in loadUseTime)
        {
            unlockAsset(v.Key);
        }
        loadUseTime.Clear();
        checkState();
        Resources.UnloadUnusedAssets();
    }

    void checkRelAsset(string ab,float lastTime)
    {
        if (!isExceedReleaseTime(lastTime))
        {
            return;
        }
        unlockAsset(ab);
        clearAssets.Add(ab);
    }

    bool isExceedReleaseTime(float time)
    {
        return nowTime - time >= CarpAssetDefine.maxReleaseTime;
    }

    void clearRelAssets()
    {
        for (int i = 0; i < clearAssets.Count; i++)
        {
            loadUseTime.Remove(clearAssets[i]);
        }
        clearAssets.Clear();
    }

    void checkState()
    {
        enabled = loadUseTime.Count > 0;
    }

    public AssetBundle getAssetBundle(string ab)
    {
        return mRequestImme.ContainsKey(ab) ? mRequestImme[ab] : null;
    }

    public UnityEngine.Object getLoadObject(string res, string assetName)
    {
        return (!loadObjects.ContainsKey(res) || !loadObjects[res].ContainsKey(assetName)) ? null : loadObjects[res][assetName];
    }

    public void addAssetBundle(string assetName, AssetBundle ab)
    {
        mRequestImme[assetName] = ab;
    }

    public void addLoadObject(string ab,string assetName,UnityEngine.Object obj)
    {
        if(!loadObjects.ContainsKey(ab))
        {
            loadObjects.Add(ab,new Dictionary<string, Object>());
        }
        loadObjects[ab][assetName] = obj;
    }

    public bool isExistLoadAsset(string ab,string assetName)
    {
        return loadObjects.ContainsKey(ab) ? loadObjects[ab].ContainsKey(assetName) : false;
    }

    public bool existAssetBundle(string ab)
    {
        return mRequestImme.ContainsKey(ab);
    }

    public void refAsset(string ab)
    {
        changeAssetRef(ab, true);
    }

    public void relAsset(string ab)
    {
        changeAssetRef(ab,false);
    }

    void changeAssetRef(string ab,bool refAsset)
    {
        changeDependAssetRef(ab,refAsset);
        changeAssetRefCount(ab, refAsset);
    }

    void changeDependAssetRef(string ab, bool refAsset)
    {
        if (CarpAssetDefine.manifest == null)
        {
            return;
        }
        string[] depends = CarpAssetDefine.manifest.GetAllDependencies(ab);
        for (int i = 0; i < depends.Length; i++)
        {
            changeAssetRefCount(depends[i], refAsset);
        }
    }

    void changeAssetRefCount(string ab,bool refAsset)
    {
        refCount = refAsset ? refAssetBundle(ab) : relAssetBundle(ab);
        checkUnlock(ab, refCount);
    }

    int refAssetBundle(string ab)
    {
        if (!assetRefTotals.ContainsKey(ab))
        {
            assetRefTotals.Add(ab, 0);
        }
        return ++assetRefTotals[ab];
    }
    int relAssetBundle(string ab)
    {
        if (!assetRefTotals.ContainsKey(ab))
        {
            return -1;
        }
        return --assetRefTotals[ab];
    }

    void checkUnlock(string ab,int refCount)
    {
        if(refCount == -1)
        {
            Debug.LogWarning("想要释放的引用文件：(" + ab + ") 并没有在资源引用容器内!");
            return;
        }

        if(refCount > 0 && loadUseTime.ContainsKey(ab))
        {
            loadUseTime.Remove(ab);
        }
        else if(refCount <= 0)
        {
            loadUseTime[ab] = Time.time;
            checkState();
        }
    }

    void unlockAsset(string ab)
    {
        if (mRequestImme.ContainsKey(ab))
        {
            if(mRequestImme[ab]!=null)
            {
                mRequestImme[ab].Unload(true);
            }
            mRequestImme.Remove(ab);
        }
        else
        {
            loadObjects[ab][string.Empty] = null;
        }

        if(loadObjects.ContainsKey(ab))
        {
            loadObjects.Remove(ab);
        }
        assetRefTotals.Remove(ab);
    }
}
