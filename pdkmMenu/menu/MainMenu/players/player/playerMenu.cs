using pdkmMenu;
using BepInEx.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using GameNetcodeStuff;
using System.Numerics;


public class PlayerMenu : MonoBehaviour
{
    private guiBase playerMenuGUI;
    private PlayerHostMenu playerHostMenu;

    //private SelfConfig selfConfig;

    //public enum SelfMenuPages
    //{
    //    None,
    //    esp
    //}
    //public SelfMenuPages currentSubmenu = SelfMenuPages.None;

    private void Start()
    {
        playerMenuGUI = gameObject.AddComponent<guiBase>();
        playerMenuGUI.MenuColor = playerMenuGUI.CustomBlue;
        playerMenuGUI.XPercentage = 0.07f*2;
        playerMenuGUI.YPercentage = 0.0f;
        playerHostMenu = gameObject.AddComponent<PlayerHostMenu>();

        //selfConfig = Plugin.SelfSettings;
    }

    int audio_indexPlayer = 0;
    public void update(PlayerControllerB player)
    {
        if (StartOfRound.Instance.localPlayerController.IsServer)
        {
            playerHostMenu.update(player);
        }
        //StartOfRound.Instance.localPlayerController.actualClientId == The id used in rpc calls
        //StartOfRound.Instance.localPlayerController.playerClientId == the players index.
        playerMenuGUI.AddButton("Spectate", () => Spectate.SwitchCamera(ref player));
        playerMenuGUI.AddButton("tp to", () => TeleportMod.TeleportToPos(player.transform.position));
        audio_indexPlayer = playerMenuGUI.AddSlider(0, SoundManager.Instance.syncedAudioClips.Length, audio_indexPlayer, audio_indexPlayer.ToString());
        playerMenuGUI.AddButton("Play Audio", () =>{var pos = player.transform.position;SoundManager.Instance.PlayAudio1AtPositionForAllClients(pos, audio_indexPlayer);});

        int damageAmount = 10;
        playerMenuGUI.AddButton("Damage", () => PlayerMods.ChangePlayerHealh(player, damageAmount));
        playerMenuGUI.AddButton("Heal", () => PlayerMods.ChangePlayerHealh(player, -damageAmount));
        playerMenuGUI.AddButton("kill", () => PlayerMods.KillPlayer(player));
        PlayerMods.Message = playerMenuGUI.TextBox(PlayerMods.Message);
        playerMenuGUI.AddButton("Chat As", () => PlayerMods.SendChatAs(player));
        playerMenuGUI.AddButton("Orbit Items", () => OrbitItemsMod.SetPlayer(player), (OrbitItemsMod.player == player));
        playerMenuGUI.AddButton("End Match as", () => WorldMods.EndMatch((int)player.playerClientId));
        playerMenuGUI.AddButton("Drop All", () => player.DropAllHeldItemsServerRpc());

        playerMenuGUI.AddButton("OpenBigDoor", () => WorldMods.ToggleDoors(player));
        playerMenuGUI.AddButton("EnableTurret", () => WorldMods.ChangeNearestTurret(player, true));
        playerMenuGUI.AddButton("DisableTurret", () => WorldMods.ChangeNearestTurret(player, false));
        playerMenuGUI.AddButton("ExplodeMine", () => WorldMods.ExplodeAllMines(player));
        playerMenuGUI.AddButton("UnlockDoor", () => WorldMods.UnlockAllDoors(player));
        playerMenuGUI.AddButton("KillEnemy", () => WorldMods.KillAllMobs(player));







        playerMenuGUI.ButtonIndex = 0; // Reset index
    }


    //public void SetSubMenu(SelfMenuPages input)
    //{
    //    if (input == currentSubmenu)
    //    {
    //        currentSubmenu = SelfMenuPages.None;
    //    }
    //    else
    //    {
    //        currentSubmenu = input;
    //    }
    //}
}

