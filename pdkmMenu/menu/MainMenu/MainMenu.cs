﻿using BepInEx.Configuration;
using pdkmMenu;
using UnityEngine;


public enum MainMenuPages
{
    None,
    Self,
    Players,
    Cameras,
    world,
    host,
    company
}
internal class MainMenu : MonoBehaviour
{
    private guiBase mainMenu;
    //private PlayerMenu playerMenu;
    //private AutoHarvestMenu autoHarvestMenu;
    //private AutoDeleteMenu autoDeleteMenu;
    //private CraftHelperMenu craftHelperMenu;
    //private CroprMenu cropMenu;
    //private AutoAttackMenu autoAttackMenu;
    private SelfMenu selfMenu;
    private PlayersMenu playersMenu;
    private CamerasMenu camerasMenu;
    private WorldMenu worldMenu;
    private HostMenu hostMenu;
    private CompanyMenu companyMenu;
    public MainMenuPages currentSubmenu = MainMenuPages.None;




    private void Start()
    {
        


        // Create an instance of the CustomMenuGUI component
        mainMenu = gameObject.AddComponent<guiBase>();
        //playerMenu = gameObject.AddComponent<PlayerMenu>();
        //autoHarvestMenu = gameObject.AddComponent<AutoHarvestMenu>();
        //autoDeleteMenu = gameObject.AddComponent<AutoDeleteMenu>();
        //craftHelperMenu = gameObject.AddComponent<CraftHelperMenu>();
        //cropMenu = gameObject.AddComponent<CroprMenu>();
        //autoAttackMenu = gameObject.AddComponent<AutoAttackMenu>();
        selfMenu = gameObject.AddComponent<SelfMenu>();
        playersMenu = gameObject.AddComponent<PlayersMenu>();
        camerasMenu = gameObject.AddComponent<CamerasMenu>();
        worldMenu = gameObject.AddComponent<WorldMenu>();
        hostMenu = gameObject.AddComponent<HostMenu>();
        companyMenu = gameObject.AddComponent<CompanyMenu>();


        // Set up the menu colors
        mainMenu.MenuColor = mainMenu.CustomBlue;
        //mainMenu.SetTextColor(new Vector4(1.0f, 1.0f, 1.0f, 1.0f)); // White text
        mainMenu.XPercentage = 0.0f;
        mainMenu.YPercentage = 0.0f;

    }
    public bool IsOpened = false;
    private void CheckKeys()
    {
        if (Plugin.MenuSettings.OpenMenu.Value.IsDown())
        {
            IsOpened = !IsOpened;
        }

        if (Plugin.MenuSettings.OpenSelfMenu.Value.IsDown())
        {
            SetSubMenu(MainMenuPages.Self);
        }
        if (Plugin.MenuSettings.OpenPlayersMenu.Value.IsDown())
        {
            SetSubMenu(MainMenuPages.Players);
        }
        if (Plugin.MenuSettings.OpenCamerasMenu.Value.IsDown())
        {
            SetSubMenu(MainMenuPages.Cameras);
        }
        if (Plugin.MenuSettings.OpenworldMenu.Value.IsDown())
        {
            SetSubMenu(MainMenuPages.world);
        }
        if (Plugin.MenuSettings.OpenhostMenu.Value.IsDown())
        {
            SetSubMenu(MainMenuPages.host);
        }



    }
    private void Update()
    {
        CheckKeys();
    }
    private void OnGUI()
    {
        //Plugin.Logger.LogInfo("balls");

        if (!IsOpened) return;

        // Render the GUI
        mainMenu.AddButton("Reload Config", OnButtonClicked);
        mainMenu.AddButton("self", () => SetSubMenu(MainMenuPages.Self));
        mainMenu.AddButton("Players", () => SetSubMenu(MainMenuPages.Players));
        mainMenu.AddButton("Cameras", () => SetSubMenu(MainMenuPages.Cameras));
        mainMenu.AddButton("World", () => SetSubMenu(MainMenuPages.world));
        if (StartOfRound.Instance != null)
        {
            if (StartOfRound.Instance.IsServer)
            {
                mainMenu.AddButton("Host", () => SetSubMenu(MainMenuPages.host));
            }
        }
        mainMenu.AddButton("Company", () => SetSubMenu(MainMenuPages.company));

        //mainMenu.AddButton("Harvest", () => SetSubMenu(MainMenuPages.Harvest));
        //mainMenu.AddButton("Delete", () => SetSubMenu(MainMenuPages.Delete));
        //mainMenu.AddButton("Craft", () => SetSubMenu(MainMenuPages.Craft));
        //mainMenu.AddButton("Crop", () => SetSubMenu(MainMenuPages.Crop));
        //mainMenu.AddButton("Attack", () => SetSubMenu(MainMenuPages.Attack));
        //mainMenu.AddButton("self", () => { showSelf = !showSelf; });

        mainMenu.ButtonIndex = 0; // Reset ButtonIndex to handle dynamic positioning


        switch (currentSubmenu)
        {
            case MainMenuPages.Self:
                selfMenu.update();
                break;
            case MainMenuPages.Players:
                playersMenu.update();
                break;
            case MainMenuPages.Cameras:
                camerasMenu.update();
                break;
            case MainMenuPages.world:
                worldMenu.update();
                break;
            case MainMenuPages.host:
                hostMenu.update();
                break;
            case MainMenuPages.company:
                companyMenu.update();
                break;
            //case MainMenuPages.Player:
            //    playerMenu.update();
            //    break;
            //case MainMenuPages.Harvest:
            //    autoHarvestMenu.update();
            //    break;
            //case MainMenuPages.Delete:
            //    autoDeleteMenu.update();
            //    break;
            //case MainMenuPages.Craft:
            //    craftHelperMenu.update();
            //    break;
            //case MainMenuPages.Crop:
            //    cropMenu.update();
            //    break;
            //case MainMenuPages.Attack:
            //    autoAttackMenu.update();
            //    break;
            case MainMenuPages.None:
                break;
        }
    }

    private void OnButtonClicked()
    {
        Plugin.configFile.Reload();
    }
    public void SetSubMenu(MainMenuPages input)
    {
        if (input == currentSubmenu)
        {
            currentSubmenu = MainMenuPages.None;
        }
        else
        {
            currentSubmenu = input;
        }
    }

}