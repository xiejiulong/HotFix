public class CarpDefineEntity
{
    public UnityEngine.Object asset;
    public string ab;
    public System.Action<UnityEngine.Object, System.Object> cb;
    public System.Object arg;

    public CarpDefineEntity(UnityEngine.Object asset, string ab, System.Action<UnityEngine.Object, System.Object> cb, System.Object arg)
    {
        this.asset = asset;
        this.ab = ab;
        this.cb = cb;
        this.arg = arg;
    }
}
