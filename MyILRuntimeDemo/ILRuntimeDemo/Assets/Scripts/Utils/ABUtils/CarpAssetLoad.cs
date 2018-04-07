using System.Collections.Generic;
using UnityEngine;

class CarpAssetLoad : MonoBehaviour
{
    protected CarpAssetsStore assetsStore;
    protected int checkIndex = 0;
    protected CarpDefineAB checkAsset = null;
    protected List<CarpDefineAB> willRequest;
    //建立这个容器，主要是为了确保一个资源错误的时，不影响后续加载
    protected Dictionary<string, bool> errorAssets = new Dictionary<string, bool>();
    void Awake()
    {
        enabled = false;
        assetsStore = gameObject.GetComponent<CarpAssetsStore>();
        willRequest = new List<CarpDefineAB>();
        DontDestroyOnLoad(this);
    }

    public void Update()
    {
        while (willRequest.Count > checkIndex)
        {
            checkAsset = willRequest[checkIndex];
            checkIndex = checkAsyncAsset() ? ++checkIndex : checkIndex;
        }
        checkState();
    }


    public virtual void init()
    {
    }

    public virtual bool checkAsyncAsset()
    {
        return true;
    }

    public virtual void assetLoad(string ab, string assetName, System.Type t)
    {
    }

    public virtual void assetLoadAsync(string ab, string assetName, System.Action<UnityEngine.Object, System.Object> cb, System.Action<CarpDefineAB> doneCB, System.Type t, System.Object arg, bool instantiate, bool frame)
    {
    }

    void checkState()
    {
        checkIndex = 0;
        checkAsset = null;
        enabled = willRequest.Count > 0;
    }

    protected void asyncAssetDone(CarpDefineAB define)
    {
        define.doneCB(define);
    }

    protected void checkError(bool error,string errorMeg)
    {
        if(error)
        {
            Debug.LogError(errorMeg);
        }
    }
}
