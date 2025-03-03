using BepInEx.Configuration;
using pdkmMenu;
using UnityEngine;

public class ESPConfig : ConfigCategory
{
    public ConfigEntry<bool> ESP { get; private set; }
    public ConfigEntry<bool> Item { get; private set; }
    public ConfigEntry<bool> Player { get; private set; }
    public ConfigEntry<bool> Enemy { get; private set; }
    public ConfigEntry<bool> Traps { get; private set; }
    public ConfigEntry<bool> Doors { get; private set; }
    public ConfigEntry<KeyboardShortcut> ESPHotKey { get; private set; }

    public ESPConfig(ConfigFile config) : base(config) { }

    protected override void BindConfigs()
    {
        ESP = Bind("ESP", "ESP", false, "Enable ESP");
        ESPHotKey = Bind("ESP", "ESPHotKey", new KeyboardShortcut(KeyCode.None), "ESP Toggle Hotkey");

        Item = Bind("ESP", "Item", true, "Show items in ESP");
        Player = Bind("ESP", "Player", true, "Show players in ESP");
        Enemy = Bind("ESP", "Enemy", true, "Show enemies in ESP");
        Traps = Bind("ESP", "Traps", true, "Show traps in ESP");
        Doors = Bind("ESP", "Doors", true, "Show doors in ESP");
    }


    public override void CheckKeys()
    {
        if (Plugin.ESPSettings.ESPHotKey.Value.IsDown()) ToggleConfigEntry(ESP);
    }
}
