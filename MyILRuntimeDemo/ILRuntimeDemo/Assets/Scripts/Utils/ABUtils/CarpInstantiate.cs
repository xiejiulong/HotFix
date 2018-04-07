using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CarpInstantiate : MonoBehaviour
{
    public float maxInstantiateTime = 10.0f;
    private float curTimeTotal = 0.0f;

    private int checkIndex = 0;
    private CarpDefineEntity checkAsset;
    private List<CarpDefineEntity> willCreates = new List<CarpDefineEntity>();
    private Dictionary<UnityEngine.Object, string> entitys = new Dictionary<Object, string>();
    private CarpAssetsStore assetsStore;

    void Awake()
    {
        enabled = false;
        assetsStore = gameObject.GetComponent<CarpAssetsStore>();
    }

	void Update ()
    {
        createAsset();
        clearState();
        checkState();
    }

    void createAsset()
    {
        while (isKeepCreate())
        {
            float beginTime = Time.realtimeSinceStartup;
            willCreate();
            curTimeTotal += Time.realtimeSinceStartup - beginTime;
        }
    }

    bool isKeepCreate()
    {
        return willCreates.Count > checkIndex && curTimeTotal < maxInstantiateTime;
    }

    void willCreate()
    {
        checkAsset = willCreates[checkIndex];
        UnityEngine.Object obj = createAssetEntity(checkAsset.asset, checkAsset.ab);
        checkAsset.cb(obj, checkAsset.arg);
        willCreates.RemoveAt(checkIndex);
    }

    void clearState()
    {
        curTimeTotal = 0.0f;
        checkIndex = 0;
    }
    void checkState()
    {
        enabled = willCreates.Count > 0;
    }

    /// <summary>
    /// 立刻实例化一个以存在的资源
    /// asset：已经实例化过的资源
    /// </summary>
    /// <param name="asset"></param>
    /// <returns></returns>
    public UnityEngine.Object instCreateExistEntity(UnityEngine.Object asset)
    {
        if (asset==null || !entitys.ContainsKey(asset)) { return null; }
        string ab = entitys[asset];
        addAssetBundle(ab);
        return createAssetEntity(asset, ab);
    }

    public UnityEngine.Object createAssetEntity(UnityEngine.Object asset,string ab)
    {
        if (asset == null) { return null; }
        UnityEngine.Object obj = GameObject.Instantiate(asset);
        entitys.Add(obj, ab);
        return obj;
    }

    public void removeEntity(UnityEngine.Object asset)
    {
        if (asset==null || !entitys.ContainsKey(asset)) { return; }
        assetsStore.relAsset(entitys[asset]);
        entitys.Remove(asset);
        GameObject.Destroy(asset);
    }

    private void addAssetBundle(string ab)
    {
        assetsStore.refAsset(ab);
    }

    public void clearAssetBundle(string ab)
    {
        assetsStore.relAsset(ab);
    }

    public void willFrameCreateEntity( UnityEngine.Object asset,string ab,System.Action<UnityEngine.Object, System.Object> cb,System.Object arg)
    {
        willCreates.Add(new CarpDefineEntity(asset,ab, cb,arg));
        checkState();
    }

    public void willFrameCreateExistEntity(UnityEngine.Object asset, System.Action<UnityEngine.Object, System.Object> cb, System.Object arg)
    {
        if(asset==null || !entitys.ContainsKey(asset)){return;}
        string ab = entitys[asset];
        addAssetBundle(ab);
        willCreates.Add(new CarpDefineEntity(asset, ab, cb, arg));
        checkState();
    }
}


