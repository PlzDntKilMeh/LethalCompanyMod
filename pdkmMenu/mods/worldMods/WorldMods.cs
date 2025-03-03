using GameNetcodeStuff;
using System.Linq;
using Unity.Netcode;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;


namespace pdkmMenu
{
    public class WorldMods
    {
        public static void UpdateMod()
        {
            flickerLights();
            ShotGunFire();
        }
        public static void SetCredits(int Amount)
        {
            Terminal terminal = Object.FindObjectOfType<Terminal>(); terminal.groupCredits = Amount;
            LogToChat.LogLocal($"Set credits to <color=#ffbf00>{Amount}</color>");
        }
        public static void SetQuota(int Quota, int DeadLine)
        {

            LogToChat.LogLocal($"Set quota to <color=#ffbf00>{Quota}</color>");
            LogToChat.LogLocal($"Set deadline to <color=#ffbf00>{DeadLine}</color>");

            TimeOfDay.Instance.profitQuota = Quota;
            float OriginalBaseIncrease = TimeOfDay.Instance.quotaVariables.baseIncrease;
            //TimeOfDay.Instance.quotaVariables.baseIncrease;
            //float temp = TimeOfDayInstance.quotaVariables.baseIncrease;
            TimeOfDay.Instance.quotaVariables.deadlineDaysAmount = DeadLine;
            TimeOfDay.Instance.quotaVariables.baseIncrease = 0;
            TimeOfDay.Instance.SetNewProfitQuota();
            TimeOfDay.Instance.quotaVariables.baseIncrease = OriginalBaseIncrease;
        }
        public static void SpawnAll()
        {
            int item_index = 0;
            foreach (UnlockableItem unlockable in StartOfRound.Instance.unlockablesList.unlockables)
            {
                Plugin.Logger.LogInfo(unlockable.unlockableName + " Unlocked: " + unlockable.alreadyUnlocked);
                if (unlockable.alreadyUnlocked == false)
                {
                    int newGroupCreditsAmount;
                    newGroupCreditsAmount = Object.FindObjectOfType<Terminal>().groupCredits;
                    StartOfRound.Instance.BuyShipUnlockableClientRpc(newGroupCreditsAmount, item_index);
                    StartOfRound.Instance.BuyShipUnlockableServerRpc(item_index, newGroupCreditsAmount);
                }
                item_index += 1;
            }
        }
        public static void SendSignalMessage(string Message)
        {
            HUDManager.Instance.UseSignalTranslatorServerRpc(Message);
        }
        public static void StartMatch()
        {
            StartOfRound.Instance.StartGameServerRpc();
        }
        public static void EndMatch(int id = 0)
        {
            StartOfRound.Instance.EndGameServerRpc(id);
        }
        public static void ChangePlanet(int planetIndex)
        {
            LogToChat.LogLocal(StartOfRound.Instance.levels[planetIndex].PlanetName);
            if (planetIndex < StartOfRound.Instance.levels.Length)
            {
                StartOfRound.Instance.ChangeLevelServerRpc(planetIndex, Object.FindObjectOfType<Terminal>().groupCredits);
            }
        }

        public static bool FlickerTog = false;
        private static int lightinterval = 0;
        private static bool LightState = false;
        public static void flickerLights()
        {
            if (!FlickerTog) return;
            ShipLights shipLights = StartOfRound.Instance.shipRoomLights;

            if (shipLights != null)
            {
                if (lightinterval > 10)
                {
                    LightState = !LightState;
                    shipLights.SetShipLightsServerRpc(LightState);
                    lightinterval = 0;
                }
                else
                {
                    lightinterval += 1;
                }
            }
        }

        public static bool TogItems = false;
        public static void ToggleItems()
        {
            TogItems = !TogItems;
            foreach (GrabbableObject grabbableObject in Object.FindObjectsOfType<GrabbableObject>())
            {
                try
                {
                    //grabbableObject.UseItemOnClient();
                    grabbableObject.ItemActivate(TogItems, TogItems);
                    ReflectionUtil.ReflectMethod<GrabbableObject>(grabbableObject, "ActivateItemServerRpc", new object[] { TogItems, TogItems });

                }
                catch (System.Exception ex)
                {
                    Plugin.Logger.LogError(ex);
                }
            }
        }
        

        public static bool FriendGodMode = false;
        public static void GiveFriendsGodMode()
        {
            FriendGodMode = !FriendGodMode;
            LogToChat.LogLocal($"set friends GodMode <color=#ffbf00>{FriendGodMode}</color>");
            StartOfRound.Instance.Debug_ToggleAllowDeathClientRpc(FriendGodMode);
        }

        public static bool TestRoomEnabled = false;
        public static void EnableTestRoom()
        {
            TestRoomEnabled = !TestRoomEnabled;
            StartOfRound.Instance.Debug_EnableTestRoomServerRpc(TestRoomEnabled);
            if (TestRoomEnabled)
            {
                StartOfRound.Instance.testRoom = UnityEngine.Object.Instantiate<GameObject>(StartOfRound.Instance.testRoomPrefab, StartOfRound.Instance.testRoomSpawnPosition.position, StartOfRound.Instance.testRoomSpawnPosition.rotation, StartOfRound.Instance.testRoomSpawnPosition);
                StartOfRound.Instance.testRoom.GetComponent<NetworkObject>().Spawn(false);
            }
            else if (StartOfRound.Instance.testRoom != null)
            {
                if (!StartOfRound.Instance.testRoom.GetComponent<NetworkObject>().IsSpawned)
                {
                    UnityEngine.Object.Destroy(StartOfRound.Instance.testRoom);
                }
                else
                {
                    StartOfRound.Instance.testRoom.GetComponent<NetworkObject>().Despawn(true);
                }
            }
            if (TestRoomEnabled)
            {
                StartOfRound.Instance.Debug_EnableTestRoomClientRpc(TestRoomEnabled, StartOfRound.Instance.testRoom.GetComponent<NetworkObject>());
                return;
            }
            StartOfRound.Instance.Debug_EnableTestRoomClientRpc(TestRoomEnabled, default(NetworkObjectReference));
        }



        public static void KillAllMobs(PlayerControllerB player = null)
        {
            if (player != null)
            {
                EnemyAI nearesEnemy = null;
                float minDistance = Mathf.Infinity;
                foreach (EnemyAI enemy in Object.FindObjectsOfType<EnemyAI>())
                {
                    float distance = Vector3.Distance(player.transform.position, enemy.transform.position);
                    if (distance < minDistance)
                    {
                        minDistance = distance;
                        nearesEnemy = enemy;
                    }
                }
                if (nearesEnemy != null)
                {
                    nearesEnemy.HitEnemyServerRpc(10000, 0, true);
                    nearesEnemy.KillEnemyServerRpc(true);
                }
                return;
            }


            foreach (EnemyAI enemyAI in Object.FindObjectsOfType<EnemyAI>())
            {
                //Logger.LogInfo(EnemyAI);
                enemyAI.HitEnemyServerRpc(10000, 0, true);
                enemyAI.KillEnemyServerRpc(true);
            }
        }

        public static void FixSteamPipes()
        {
            foreach (SteamValveHazard SteamValveHazard in Object.FindObjectsOfType<SteamValveHazard>())
            {
                SteamValveHazard.FixValveServerRpc();
            }
        }

        public static bool doorToggle = false;
        public static void ToggleDoors(PlayerControllerB player = null)
        {
            doorToggle = !doorToggle;

            if (player != null)
            {
                TerminalAccessibleObject nearestDoor = null;
                float minDistance = Mathf.Infinity;
                foreach (TerminalAccessibleObject door in Object.FindObjectsOfType<TerminalAccessibleObject>())
                {
                    float distance = Vector3.Distance(player.transform.position, door.transform.position);
                    if (distance < minDistance)
                    {
                        minDistance = distance;
                        nearestDoor = door;
                    }
                }
                if (nearestDoor != null)
                {

                    bool OpenState = (bool)ReflectionUtil.ReflectField<TerminalAccessibleObject>(nearestDoor, "isDoorOpen");
                    LogToChat.LogLocal($"Nearest door open: <color=#ffbf00>{OpenState}</color>");
                    nearestDoor.SetDoorOpenServerRpc(!OpenState);
                }
            }
            else
            {
                LogToChat.LogLocal($"doors open: <color=#ffbf00>{doorToggle}</color>");
                foreach (TerminalAccessibleObject door in Object.FindObjectsOfType<TerminalAccessibleObject>())
                {
                    door.SetDoorOpenServerRpc(doorToggle);
                }
            }
        }

        public static bool turretToggle = false;
        public static void ToggleTurrets(PlayerControllerB player = null)
        {
            turretToggle = !turretToggle;
            LogToChat.LogLocal($"turrets active: <color=#ffbf00>{turretToggle}</color>");

            if (player != null)
            {
                Turret nearestTurret = null;
                float minDistance = Mathf.Infinity;
                foreach (Turret turret in Object.FindObjectsOfType<Turret>())
                {
                    float distance = Vector3.Distance(player.transform.position, turret.transform.position);
                    if (distance < minDistance)
                    {
                        minDistance = distance;
                        nearestTurret = turret;
                    }
                }
                if (nearestTurret != null)
                {
                    nearestTurret.ToggleTurretServerRpc(turretToggle);
                }
            }
            else
            {
                foreach (Turret turret in Object.FindObjectsOfType<Turret>())
                {
                    turret.ToggleTurretServerRpc(turretToggle);
                }
            }
        }
        public static void ChangeNearestTurret(PlayerControllerB player, bool state)
        {
            if (player != null)
            {
                Turret nearestTurret = null;
                float minDistance = Mathf.Infinity;
                foreach (Turret turret in Object.FindObjectsOfType<Turret>())
                {
                    float distance = Vector3.Distance(player.transform.position, turret.transform.position);
                    if (distance < minDistance)
                    {
                        minDistance = distance;
                        nearestTurret = turret;
                    }
                }
                if (nearestTurret != null)
                {
                    nearestTurret.ToggleTurretServerRpc(state);
                }
            }
        }

        public static void ExplodeAllMines(PlayerControllerB player = null)
        {
            if (player != null)
            {
                Landmine nearestLandmine = null;
                float minDistance = Mathf.Infinity;
                foreach (Landmine landmine in Object.FindObjectsOfType<Landmine>())
                {
                    float distance = Vector3.Distance(player.transform.position, landmine.transform.position);
                    if (distance < minDistance)
                    {
                        minDistance = distance;
                        nearestLandmine = landmine;
                    }
                }
                if (nearestLandmine != null && !nearestLandmine.hasExploded)
                {
                    nearestLandmine.ExplodeMineServerRpc();
                }
            }
            else
            {
                foreach (Landmine landmine in Object.FindObjectsOfType<Landmine>())
                {
                    if (!landmine.hasExploded)
                    {
                        landmine.ExplodeMineServerRpc();
                    }
                }
            }
        }

        public static void UnlockAllDoors(PlayerControllerB player = null)
        {
            if (player != null)
            {
                DoorLock nearestDoorLock = null;
                float minDistance = Mathf.Infinity;
                foreach (DoorLock doorLock in Object.FindObjectsOfType<DoorLock>())
                {
                    float distance = Vector3.Distance(player.transform.position, doorLock.transform.position);
                    if (distance < minDistance)
                    {
                        minDistance = distance;
                        nearestDoorLock = doorLock;
                    }
                }
                if (nearestDoorLock != null)
                {
                    nearestDoorLock.UnlockDoorSyncWithServer();
                }
            }
            else
            {
                foreach (DoorLock doorLock in Object.FindObjectsOfType<DoorLock>())
                {
                    doorLock.UnlockDoorSyncWithServer();
                }
            }
        }
        public static bool FactoryLights = false;

        public static void ToggelFactoryLights()
        {
            FactoryLights = !FactoryLights;
            RoundManager.Instance.SwitchPower(FactoryLights);
        }

        public static void ResetLevel()
        {
            RoundManager.Instance.FinishGeneratingNewLevelServerRpc();
            RoundManager.Instance.FinishGeneratingNewLevelClientRpc();
        }

        public static bool ShotGunToggle = false;
        public static void ToggleShotGuns()
        {
            ShotGunToggle = !ShotGunToggle;
        }

        private static float timeElapsed = 0f;
        public static float interval = 1.0f; // 1 second interval
        public static void ShotGunFire()
        {
            if (!ShotGunToggle) return;
            if (StartOfRound.Instance == null) return;
            timeElapsed += Time.deltaTime; // Increase time elapsed by the time between frames

            if (timeElapsed <= interval)
            {
                return;
            }
            timeElapsed = 0f;

            foreach (ShotgunItem shotty in Object.FindObjectsOfType<ShotgunItem>())
            {
                shotty.ShootGunServerRpc(StartOfRound.Instance.localPlayerController.transform.position, -StartOfRound.Instance.localPlayerController.gameplayCamera.transform.up);
            }
        }




    }
}
