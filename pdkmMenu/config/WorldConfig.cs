using BepInEx.Configuration;
using pdkmMenu;
using UnityEngine;

public class WorldConfig : ConfigCategory
{
    public ConfigEntry<KeyboardShortcut> StartMatchHotKey { get; private set; }
    public ConfigEntry<KeyboardShortcut> KillAllEnemyHotKey { get; private set; }
    public ConfigEntry<KeyboardShortcut> ShotGunHotKey { get; private set; }
    

    public WorldConfig(ConfigFile config) : base(config) { }

    protected override void BindConfigs()
    {
        StartMatchHotKey = Bind("World", "StartMatchHotKey", new KeyboardShortcut(KeyCode.None), "StartMatch HotKey");
        KillAllEnemyHotKey = Bind("World", "KillAllEnemyHotKey", new KeyboardShortcut(KeyCode.None), "KillAllEnemyHotKey");
        ShotGunHotKey = Bind("World", "ShotGunHotKey", new KeyboardShortcut(KeyCode.None), "ShotGun machineGun");
    }


    public override void CheckKeys()
    {
        if (Plugin.WorldSettings.StartMatchHotKey.Value.IsDown()) WorldMods.StartMatch();
        if (Plugin.WorldSettings.KillAllEnemyHotKey.Value.IsDown()) WorldMods.KillAllMobs();
        if (Plugin.WorldSettings.ShotGunHotKey.Value.IsDown()) WorldMods.ToggleShotGuns();
    }
}
