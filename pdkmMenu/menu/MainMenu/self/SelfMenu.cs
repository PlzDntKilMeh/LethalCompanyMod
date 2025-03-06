using pdkmMenu;
using BepInEx.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;


public class SelfMenu : MonoBehaviour
{
    private guiBase selfMenuGUI;
    private EspMenu espMenu;
    private SelfConfig selfConfig;

    public enum SelfMenuPages
    {
        None,
        esp
    }
    public SelfMenuPages currentSubmenu = SelfMenuPages.None;

    private void Start()
    {
        selfMenuGUI = gameObject.AddComponent<guiBase>();
        selfMenuGUI.MenuColor = selfMenuGUI.CustomBlue;
        selfMenuGUI.XPercentage = 0.07f;
        selfMenuGUI.YPercentage = 0.0f;
        espMenu = gameObject.AddComponent<EspMenu>();
        selfConfig = Plugin.SelfSettings;
    }

    private static int ChallengeVal = int.MaxValue;
    public void update()
    {
        //DisableAllWeather
        selfMenuGUI.AddButton("ESP", () => SetSubMenu(SelfMenuPages.esp));
        selfMenuGUI.AddButton("GodMode", () => { selfConfig.ToggleConfigEntry(selfConfig.GodMode); }, selfConfig.GodMode.Value);
        selfMenuGUI.AddButton("InfAmmo", () => { selfConfig.ToggleConfigEntry(selfConfig.InfAmmo); }, selfConfig.InfAmmo.Value);
        selfMenuGUI.AddButton("InfBattery", () => { selfConfig.ToggleConfigEntry(selfConfig.InfBattery); }, selfConfig.InfBattery.Value);
        selfMenuGUI.AddButton("InfSprint", () => { selfConfig.ToggleConfigEntry(selfConfig.InfSprint); }, selfConfig.InfSprint.Value);
        selfMenuGUI.AddButton("AlwaysName", () => { selfConfig.ToggleConfigEntry(selfConfig.AlwaysShowUserNames); }, selfConfig.AlwaysShowUserNames.Value);
        selfConfig.PlayerSpeed.Value = selfMenuGUI.AddSlider(1.0f, 50.0f, selfConfig.PlayerSpeed.Value, selfConfig.PlayerSpeed.Value.ToString(), selfConfig.SpeedHackEnabled.Value);
        selfMenuGUI.AddButton("SpeedHack", () => { selfConfig.ToggleConfigEntry(selfConfig.SpeedHackEnabled); }, selfConfig.SpeedHackEnabled.Value);

        selfConfig.NightVisionIntensity.Value = selfMenuGUI.AddSlider(1.0f, 50.0f, selfConfig.NightVisionIntensity.Value, selfConfig.NightVisionIntensity.Value.ToString(), selfConfig.NightVision.Value);
        selfMenuGUI.AddButton("NightVision", () => { selfConfig.ToggleConfigEntry(selfConfig.NightVision); }, selfConfig.NightVision.Value);

        selfConfig.GrabDistanceValue.Value = selfMenuGUI.AddSlider(1.0f, 500.0f, selfConfig.GrabDistanceValue.Value, selfConfig.GrabDistanceValue.Value.ToString(), selfConfig.GrabDistance.Value);
        selfMenuGUI.AddButton("GrabDistance", () => { selfConfig.ToggleConfigEntry(selfConfig.GrabDistance); }, selfConfig.GrabDistance.Value);

        selfMenuGUI.AddButton("ToolTips", () => { selfConfig.ToggleConfigEntry(selfConfig.ToolTips); }, selfConfig.ToolTips.Value);

        selfMenuGUI.AddButton("NoClip", () => { NoClipMod.IsNoClip = !NoClipMod.IsNoClip; }, NoClipMod.IsNoClip);
        selfMenuGUI.AddButton("FreeCam", () => { FreeCamMod.IsFreeCam = !FreeCamMod.IsFreeCam; }, FreeCamMod.IsFreeCam);

        //selfMenuGUI.AddButton("HearAll", () => { selfConfig.ToggleConfigEntry(selfConfig.HearAll); }, selfConfig.HearAll.Value);

        selfMenuGUI.AddButton("Teleport to entrance", () => { TeleportMod.TeleportToEntrance(); });
        selfMenuGUI.AddButton("Teleport to ship", () => { TeleportMod.TeleportToShip(); });
        selfMenuGUI.AddButton("Recall", () => { TeleportMod.Recall(); });
        selfMenuGUI.AddButton("Mark", () => { TeleportMod.MarkPos(); });
        selfMenuGUI.AddButton("Tp To Mark", () => { TeleportMod.TeleportToMarkPos(); });

        ChallengeVal = selfMenuGUI.IntTextBox(ChallengeVal);
        selfMenuGUI.AddButton("Submit Challenge scrap", () => { HUDManager.Instance.FillChallengeResultsStats(ChallengeVal);});






        switch (currentSubmenu)
        {
            case SelfMenuPages.None:
                break;
            case SelfMenuPages.esp:
                espMenu.update();
                break;
        }

        selfMenuGUI.ButtonIndex = 0; // Reset index
    }
    public void SetSubMenu(SelfMenuPages input)
    {
        if (input == currentSubmenu)
        {
            currentSubmenu = SelfMenuPages.None;
        }
        else
        {
            currentSubmenu = input;
        }
    }
}

