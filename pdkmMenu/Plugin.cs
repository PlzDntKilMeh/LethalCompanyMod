using BepInEx;
using BepInEx.Logging;
using UnityEngine;
using HarmonyLib;
using BepInEx.Configuration;
using System;
using GameNetcodeStuff;
using System.Linq;
using System.IO;

namespace pdkmMenu;


[BepInPlugin(MyPluginInfo.PLUGIN_GUID, MyPluginInfo.PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION)]
public class Plugin : BaseUnityPlugin
{
    internal static new ManualLogSource Logger;
    internal static Harmony HarmonyInstance = new(MyPluginInfo.PLUGIN_NAME);
    //public static Plugin instance;
    public static ConfigFile configFile;


    public static AntiCheatConfig AntiCheatSettings { get; private set; }
    public static ESPConfig ESPSettings { get; private set; }
    public static MenuConfig MenuSettings { get; private set; }
    public static SelfConfig SelfSettings { get; private set; }
    public static WorldConfig WorldSettings { get; private set; }
    public static HostConfig HostSettings { get; private set; }

    private void Awake()
    {
        // Plugin startup logic
        Logger = base.Logger;
        Logger.LogInfo($"Plugin {MyPluginInfo.PLUGIN_GUID} is loaded!");
        HarmonyInstance.PatchAll();
        configFile = Config;
        AntiCheatSettings = new AntiCheatConfig(Config);
        ESPSettings = new ESPConfig(Config);
        MenuSettings = new MenuConfig(Config);
        SelfSettings = new SelfConfig(Config);
        WorldSettings = new WorldConfig(Config);
        HostSettings = new HostConfig(Config);

    }
    private void Start()
    {
        GameObject MainMenu_Go = new GameObject("MainMenu_Go");
        DontDestroyOnLoad(MainMenu_Go);
        MainMenu_Go.AddComponent<MainMenu>();
        Spectate.Initialize();
    }
    private void OnDestroy()
    {
        Plugin.Logger.LogInfo("Unloading Plugin");
        HarmonyInstance.UnpatchSelf();
    }


    private void Update()
    {
        //configFile.Reload();
        CheckKeys();
        UpdateMods();
        Spectate.Update();
        //ApplyOutlineChams();
    }

    //private void ApplyOutlineChams()
    //{
    //    foreach (SkinnedMeshRenderer smr in FindObjectsOfType<SkinnedMeshRenderer>())
    //    {
    //        //if (smr.gameObject.name.Contains("TargetObjectName"))
    //        //{

    //        //}
    //        // Clone material
    //        if (smr == null || smr.material == null)
    //            continue;

    //        Material newMat = new Material(smr.material);
    //        newMat.color = Color.green;
    //        newMat.SetInt("_ZWrite", 0);
    //        newMat.SetInt("_ZTest", (int)UnityEngine.Rendering.CompareFunction.Always);
    //        newMat.renderQueue = 3100;

    //        Shader shader = Shader.Find("Unlit/Color");
    //        if (shader != null)
    //        {
    //            newMat.shader = shader;
    //        }

    //        smr.material = newMat;
    //    }
    //}
    //public static void ModifyMaterial(Material Mat)
    //{
    //    string assetBundlePath = Path.Combine(Paths.PluginPath, "xray.asset");
    //    AssetBundle bundle = AssetBundle.LoadFromFile(assetBundlePath);
    //    Shader myShader = bundle.LoadAsset<Shader>("MyShader");
    //    Material myMaterial = bundle.LoadAsset<Material>("MyMat");
    //    Mat.shader = myShader;
    //    Mat = myMaterial;
        
    //    bundle.Unload(false); // Optional: Keep it loaded if needed
    //}

    private void CheckKeys()
    {
        ESPSettings.CheckKeys();
        MenuSettings.CheckKeys();
        SelfSettings.CheckKeys();
    }
    private void UpdateMods()
    {
        if (StartOfRound.Instance == null) return;
        if (GameNetworkManager.Instance == null) return;
        PlayerMods.UpdateMod();
        SelfMods.UpdateMod();
        WorldMods.UpdateMod();

    }
    
    public static PlayerControllerB[] GetRealPlayerScripts()
    {
        if (StartOfRound.Instance == null || StartOfRound.Instance.allPlayerScripts == null)
        {
            //startOfRound.allPlayerScripts[0].
            return new PlayerControllerB[0];
        }
        return (from x in StartOfRound.Instance.allPlayerScripts
                where x.isPlayerDead || x.isPlayerControlled
                select x).ToArray<PlayerControllerB>();
    }

    private void OnGUI()
    {

    }

}
