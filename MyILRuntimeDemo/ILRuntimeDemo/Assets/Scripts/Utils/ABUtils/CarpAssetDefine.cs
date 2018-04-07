using UnityEngine;
public class CarpAssetDefine
{
    public static AssetBundleManifest manifest;
    public static string manifestName = "AssetBundles";
    public static Hash128 emptyHash = Hash128.Parse("00000000000000000000000000000000");


    public static string assetPath = Application.persistentDataPath +"/";
   

    //资源释放时间
    public static float maxReleaseTime = 10.0f;
}
