using GameNetcodeStuff;
using Unity.Netcode;
using UnityEngine;
namespace pdkmMenu
{
    public static class OrbitItemsMod
    {
        public static PlayerControllerB player = null;
        public static int OrbitRate = 10;
        public static int itemIndex = 0;
        public static void UpdateMod()
        {
            if (player == null) return;

            PlaceableShipObject[] items = Object.FindObjectsOfType<PlaceableShipObject>();
            if (items == null || items.Length == 0) { itemIndex = 0; return; }

            float orbitRadius = 5.0f;
            float angle = Time.time * OrbitRate + (itemIndex * (Mathf.PI * 2 / items.Length));

            // Calculate the orbit position
            Vector3 orbitOffset = new Vector3(Mathf.Cos(angle), 0.5f, Mathf.Sin(angle)) * orbitRadius;
            Vector3 orbitPosition = player.transform.position + orbitOffset;

            PlaceableShipObject item = items[itemIndex];

            Vector3 rotation = new Vector3(-90, 90, 90);
            //ShipBuildModeManager.Instance.PlaceShipObject(orbitPosition, rotation, item, false);
            ShipBuildModeManager.Instance.PlaceShipObjectServerRpc(orbitPosition, rotation,
                item.parentObject.GetComponent<NetworkObject>(),
                (int)StartOfRound.Instance.localPlayerController.playerClientId);

            // Cycle through items
            itemIndex = (itemIndex + 1) % items.Length;
        }

        public static void SetPlayer(PlayerControllerB Newplayer)
        {
            if (Newplayer == null) return;
            if (Newplayer == player)
            {
                player = null;
                return;
            }
            else
            {
                player = Newplayer;
            }

        }
    }
}