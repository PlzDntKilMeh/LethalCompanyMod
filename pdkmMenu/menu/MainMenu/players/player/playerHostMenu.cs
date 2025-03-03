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


public class PlayerHostMenu : MonoBehaviour
{
    private guiBase playerHostMenuGUI;
    //private SelfConfig selfConfig;

    //public enum SelfMenuPages
    //{
    //    None,
    //    esp
    //}
    //public SelfMenuPages currentSubmenu = SelfMenuPages.None;

    private void Start()
    {
        playerHostMenuGUI = gameObject.AddComponent<guiBase>();
        playerHostMenuGUI.MenuColor = playerHostMenuGUI.CustomBlue;
        playerHostMenuGUI.XPercentage = 0.07f * 3;
        playerHostMenuGUI.YPercentage = 0.0f;
        //selfConfig = Plugin.SelfSettings;
    }

    public void update(PlayerControllerB player)
    {
        HostMenu(player);
        playerHostMenuGUI.ButtonIndex = 0; // Reset index
    }
    private void HostMenu(PlayerControllerB player)
    {
        if (!StartOfRound.Instance.localPlayerController.IsServer) return;
        playerHostMenuGUI.AddButton("Heal", () => player.HealClientRpc());
        playerHostMenuGUI.AddButton("Make Limp", () => player.MakeCriticallyInjuredClientRpc());
        playerHostMenuGUI.AddButton("Disable jetpack", () => player.DisableJetpackModeClientRpc());
        playerHostMenuGUI.AddButton("Lighting strike", () => { RoundManager.Instance.LightningStrikeClientRpc(player.transform.position); RoundManager.Instance.LightningStrikeServerRpc(player.transform.position); });

        playerHostMenuGUI.AddButton("BreakLegs sfx", () => player.BreakLegsSFXClientRpc());
        playerHostMenuGUI.AddButton("LandFromJump sfx", () => player.LandFromJumpClientRpc(true));
        playerHostMenuGUI.AddButton("Jump sfx", () => player.PlayerJumpedClientRpc());
        AddSpawnEnemy(player);
        AddObjectSpawnButton(player);
        //UpdatePlayerPositionServerRpc(Vector3 newPos, bool inElevator, bool inShipRoom, bool exhausted, bool isPlayerGrounded)
        //playerMenuGUI.AddButton("TpToMe", () => ReflectionUtil.ReflectMethod<PlayerControllerB>(player, "UpdatePlayerPositionClientRpc", new object[] { StartOfRound.Instance.localPlayerController.transform.position, false, false, false, false }));
        //playerMenuGUI.AddButton("MakeSpin", () => PlayerSpinBot.ModifyPlayer(player), (PlayerSpinBot.playerControllerBs.Contains(player)));


    }
    private static int combinedMobIndex = 0;
    private List<SpawnableEnemyWithRarity> allEnemies = new List<SpawnableEnemyWithRarity>();
    private void AddSpawnEnemy(PlayerControllerB player)
    {
        // Build combined list
        allEnemies.Clear();
        allEnemies = SpawnHelper.GetallEnemies();
        // Setup UI
        string label = "No enemies available";
        bool active = false;

        if (allEnemies.Count > 0)
        {
            // Ensure index is within valid range
            combinedMobIndex = Mathf.Clamp(combinedMobIndex, 0, allEnemies.Count - 1);

            label = $"{allEnemies[combinedMobIndex].enemyType.enemyName}";
            active = true;
        }

        combinedMobIndex = playerHostMenuGUI.AddSlider(0, allEnemies.Count - 1, combinedMobIndex, label);
        playerHostMenuGUI.AddButton("Spawn Near", () => { SpawnHelper.SpawnEnemy(combinedMobIndex, player); }, active);
        playerHostMenuGUI.AddButton("Spawn there(laggy)", () => { SpawnHelper.SpawnEnemy(combinedMobIndex, player, false); }, active);
    }
    private static int ObjectIndex = 0;
    private void AddObjectSpawnButton(PlayerControllerB player)
    {
        string label = "temp";
        bool active = false;
        if (RoundManager.Instance.currentLevel.spawnableMapObjects == null || RoundManager.Instance.currentLevel.spawnableMapObjects.Length == 0)
        {
            label = "error";
            active = false;
        }
        else
        {
            // Ensure planetIndex is within valid range
            ObjectIndex = Mathf.Clamp(ObjectIndex, 0, RoundManager.Instance.currentLevel.spawnableMapObjects.Length - 1);

            label = RoundManager.Instance.currentLevel.spawnableMapObjects[ObjectIndex].prefabToSpawn.name.ToString();
            active = true;
        }

        ObjectIndex = playerHostMenuGUI.AddSlider(0, RoundManager.Instance.currentLevel.spawnableMapObjects.Length - 1, ObjectIndex, label, active);
        playerHostMenuGUI.AddButton("Spawn Object", () => { SpawnHelper.SpawnObject(ObjectIndex, player.transform.position); }, active);
    }
}

