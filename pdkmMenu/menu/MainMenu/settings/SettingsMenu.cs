using pdkmMenu;
using Unity.Netcode;
using UnityEngine;


public class SettingsMenu : MonoBehaviour
{
    private guiBase settingsMenuGUI;

    private void Start()
    {
        settingsMenuGUI = gameObject.AddComponent<guiBase>();
        settingsMenuGUI.MenuColor = settingsMenuGUI.CustomBlue;
        settingsMenuGUI.XPercentage = 0.07f;
        settingsMenuGUI.YPercentage = 0.0f;
    }


    private static float companyBuyingRate = 0f;
    public void update()
    {
        settingsMenuGUI.ButtonIndex = 0; // Reset index
        settingsMenuGUI.AddButton("AutoStart", () => { Plugin.MenuSettings.AutoStart.Value = !Plugin.MenuSettings.AutoStart.Value; }, Plugin.MenuSettings.AutoStart.Value);
        settingsMenuGUI.AddButton("AutoStartOnline", () => { Plugin.MenuSettings.AutoStart.Value = !Plugin.MenuSettings.AutoStart.Value; }, Plugin.MenuSettings.AutoStartOnline.Value);
    }
}

