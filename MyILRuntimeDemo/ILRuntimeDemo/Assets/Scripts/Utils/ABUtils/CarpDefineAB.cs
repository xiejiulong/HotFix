public class CarpDefineAB
{
    public string ab { get; private set; }
    public string assetName { get; private set; }
    public string assetKey { get; private set; }
    public string[] depends { get; private set; }
    public System.Action<UnityEngine.Object, System.Object> callback { get; private set; }
    public System.Action<CarpDefineAB> doneCB { get; private set; }
    public System.Object arg { get; private set; }
    public System.Type loadType { get; private set; }
    public bool frame { get; private set; }
    public bool instantiate { get; private set; }

    public void Bind(string ab, string assetName,string[] denc, System.Action<UnityEngine.Object, System.Object> cb, System.Action<CarpDefineAB> doneCB, System.Type t,
                    System.Object arg,bool instantiate,bool frame)
    {
        this.ab = ab;
        this.assetName = assetName;
        this.assetKey = ab +"_"+ assetName;
        this.depends = denc;
        this.callback = cb;
        this.doneCB = doneCB;
        this.arg = arg;
        this.loadType = t;
        this.instantiate = instantiate;
        this.frame = frame;
    }
}