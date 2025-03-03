using pdkmMenu;
using BepInEx.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;


public class EspMenu : MonoBehaviour
{
    private guiBase espMenuGUI;

    private void Start()
    {
        espMenuGUI = gameObject.AddComponent<guiBase>();
        espMenuGUI.MenuColor = espMenuGUI.CustomBlue;

        //0.06f 
        espMenuGUI.XPercentage = 0.14f;
        espMenuGUI.YPercentage = 0.0f;
    }

    private void CheckKeys()
    {
        if (Plugin.ESPSettings.ESPHotKey.Value.IsDown()) UpdateEsp();
    }


    private void UpdateEsp()
    {
        Plugin.ESPSettings.ESP.Value = !Plugin.ESPSettings.ESP.Value;
    }

    public void update()
    {
        CheckKeys();
        ESPConfig ESPSettings = Plugin.ESPSettings;
        espMenuGUI.AddButton("ESP", () => ESPSettings.ToggleConfigEntry(ESPSettings.ESP), ESPSettings.ESP.Value);
        espMenuGUI.AddButton("Item", () => ESPSettings.ToggleConfigEntry(ESPSettings.Item), ESPSettings.Item.Value);
        espMenuGUI.AddButton("Player", () => ESPSettings.ToggleConfigEntry(ESPSettings.Player), ESPSettings.Player.Value);
        espMenuGUI.AddButton("Enemy", () => ESPSettings.ToggleConfigEntry(ESPSettings.Enemy), ESPSettings.Enemy.Value);
        espMenuGUI.AddButton("Traps", () => ESPSettings.ToggleConfigEntry(ESPSettings.Traps), ESPSettings.Traps.Value);
        espMenuGUI.AddButton("Doors", () => ESPSettings.ToggleConfigEntry(ESPSettings.Doors), ESPSettings.Doors.Value);

        espMenuGUI.ButtonIndex = 0; // Reset index
    }
}

