using pdkmMenu;
using BepInEx.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using GameNetcodeStuff;
using Unity.Netcode;
using BepInEx;


public class HostMenu : MonoBehaviour
{
    private guiBase hostMenuGUI;

    private void Start()
    {
        hostMenuGUI = gameObject.AddComponent<guiBase>();
        hostMenuGUI.MenuColor = hostMenuGUI.CustomBlue;
        hostMenuGUI.XPercentage = 0.07f;
        hostMenuGUI.YPercentage = 0.0f;
    }

    private static int Quota = 300;
    private static int DeadLine = 3;
    public void update()
    {
        Plugin.HostSettings.Credits.Value = hostMenuGUI.IntTextBox(Plugin.HostSettings.Credits.Value);
        hostMenuGUI.AddButton("Spawn Items", () => { ShowSpawnItems = !ShowSpawnItems; }, ShowSpawnItems);
        if (ShowSpawnItems)
        {
            LoadItemList();
        }
        hostMenuGUI.AddButton("Set Credits", ()=> { WorldMods.SetCredits(Plugin.HostSettings.Credits.Value); });


        Quota = hostMenuGUI.IntTextBox(Quota);
        DeadLine = hostMenuGUI.IntTextBox(DeadLine);
        hostMenuGUI.AddButton("Quota/Deadline", () => { WorldMods.SetQuota(Quota, DeadLine); });


        hostMenuGUI.AddButton("Friends Godmode", () => { WorldMods.GiveFriendsGodMode(); }, WorldMods.FriendGodMode);
        hostMenuGUI.AddButton("Test Room", () => { WorldMods.EnableTestRoom(); }, WorldMods.TestRoomEnabled);
        hostMenuGUI.AddButton("Eject", () => { StartOfRound.Instance.ManuallyEjectPlayersServerRpc(); });
        
        AddObjectSpawnButton();
        hostMenuGUI.AddButton("Despawn Objects", () => { SpawnHelper.DespawnObjects(); });
        AddSpawnEnemy();
        hostMenuGUI.AddButton("Kill All Mobs", () =>{WorldMods.KillAllMobs();});
        hostMenuGUI.AddButton("ResetLevel", () =>{WorldMods.ResetLevel();});
        
        hostMenuGUI.ButtonIndex = 0; // Reset index
        hostMenuGUI.ContainterIndex_x = 0;
        hostMenuGUI.ContainterIndex_y = 0;
    }
    private static bool ShowSpawnItems = false;

    private static int combinedMobIndex = 0;
    private List<SpawnableEnemyWithRarity> allEnemies = new List<SpawnableEnemyWithRarity>();

    private void AddSpawnEnemy()
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

        combinedMobIndex = hostMenuGUI.AddSlider(0, allEnemies.Count - 1, combinedMobIndex, label);
        hostMenuGUI.AddButton("Spawn Near", () => { SpawnHelper.SpawnEnemy(combinedMobIndex, StartOfRound.Instance.localPlayerController); }, active);
        hostMenuGUI.AddButton("Spawn Here(laggy)", () => { SpawnHelper.SpawnEnemy(combinedMobIndex, StartOfRound.Instance.localPlayerController, false); }, active);
    }

    private static int ObjectIndex = 0;
    private void AddObjectSpawnButton()
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

        ObjectIndex = hostMenuGUI.AddSlider(0, RoundManager.Instance.currentLevel.spawnableMapObjects.Length - 1, ObjectIndex, label, active);
        hostMenuGUI.AddButton("Spawn Object", () => { SpawnHelper.SpawnObject(ObjectIndex, StartOfRound.Instance.localPlayerController.transform.position); }, active);
    }



    private static Vector2 scrollPosition = Vector2.zero;
    private static string text = "";
    private void LoadItemList()
    {
        text = hostMenuGUI.TextBox(text);
        List<Item> list = StartOfRound.Instance.allItemsList.itemsList;
        if (list == null || list.Count == 0)
        {
            Debug.Log("Item list is empty or not loaded yet.");
            return;
        }

        int rowLen = 8;
        scrollPosition = GUI.BeginScrollView(
            new Rect(Screen.width * 0.15f,
                     0,
                    hostMenuGUI.ContainterImgX * rowLen + (Screen.width * 0.01f),
                    hostMenuGUI.ContainterImgY * 2 + hostMenuGUI.ContainterImgY),
            scrollPosition,
            new Rect(0,
                     0,
                     hostMenuGUI.ContainterImgX * rowLen,
                     (list.Count / (rowLen - 1)) * hostMenuGUI.ContainterImgY + hostMenuGUI.ContainterImgY));


        foreach (Item item in list)
        {
            if (item == null) continue;
            string name = item.itemName;
            if (!name.ToLower().Contains(text.ToLower()) && !text.IsNullOrWhiteSpace())
            {
                continue;
            }

            Sprite icon = item.itemIcon;
            Texture2D image = icon.texture;


            bool selected = true;
            
            //Color originalColor = GUI.backgroundColor;
            Color originalColor = hostMenuGUI.MenuColor;

            //GUI.backgroundColor = selected ? Color.blue : Color.red;
            hostMenuGUI.MenuColor = (selected ? hostMenuGUI.CustomBlue : hostMenuGUI.CustomRed);

            hostMenuGUI.AddImgListItem(image, name, () => Clicked(item), rowLen - 1);

            //GUI.backgroundColor = originalColor;
            hostMenuGUI.MenuColor = originalColor;
        }

        GUI.EndScrollView();
    }
    private static void Clicked(Item item)
    {
        GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(item.spawnPrefab, StartOfRound.Instance.localPlayerController.transform.position, Quaternion.identity, StartOfRound.Instance.propsContainer);
        gameObject.GetComponent<GrabbableObject>().fallTime = 0f;
        int scrap_value = (int)((float)UnityEngine.Random.Range(gameObject.GetComponent<GrabbableObject>().itemProperties.minValue + 25, gameObject.GetComponent<GrabbableObject>().itemProperties.maxValue + 35) * RoundManager.Instance.scrapValueMultiplier);
        gameObject.GetComponent<GrabbableObject>().SetScrapValue(scrap_value);
        gameObject.GetComponent<NetworkObject>().Spawn(false);
    }

}

