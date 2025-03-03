using GameNetcodeStuff;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace pdkmMenu
{
    public class HearAllMod
    {

        private static float defaultVoiceRange = 0;
        public static void UpdateMod()
        {
            
            if(GameNetworkManager.Instance.localPlayerController == null) return;

            SetDefaultVoiceRange();
            if (defaultVoiceRange == 0) return;

            if (!Plugin.SelfSettings.HearAll.Value)
            {
                Disabled();
                return;
            }
            Enabled();
        }
        private static void SetDefaultVoiceRange()
        {
            if (defaultVoiceRange == 0)
            {
                if (GameNetworkManager.Instance.localPlayerController.currentVoiceChatAudioSource != null)
                {
                    defaultVoiceRange = GameNetworkManager.Instance.localPlayerController.currentVoiceChatAudioSource.maxDistance;
                }
            }
        }
        private static void Enabled()
        {
            foreach (PlayerControllerB player in GetRealPlayerScripts())
            {
                if (player != null)
                {
                    float distance = Vector3.Distance(player.transform.position, GameNetworkManager.Instance.localPlayerController.transform.position);
                    if (player.currentVoiceChatIngameSettings != null)
                    {
                        player.currentVoiceChatAudioSource.maxDistance = distance + 5;
                    }
                }
            }
        }
        private static void Disabled()
        {
            foreach (PlayerControllerB player in GetRealPlayerScripts())
            {
                if (player != null)
                {
                    //float distance = Vector3.Distance(player.transform.position, GameNetworkManager.Instance.localPlayerController.transform.position);
                    if (player.currentVoiceChatIngameSettings != null)
                    {
                        player.currentVoiceChatAudioSource.maxDistance = defaultVoiceRange;
                    }
                }
            }
        }
        public static PlayerControllerB[] GetRealPlayerScripts()
        {
            if (StartOfRound.Instance == null || StartOfRound.Instance.allPlayerScripts == null)
            {
                //startOfRound.allPlayerScripts[0].
                return new PlayerControllerB[0];
            }
            return (from x in StartOfRound.Instance.allPlayerScripts
                    where x.isPlayerDead || x.isPlayerControlled
                    select x).ToArray<PlayerControllerB>();
        }
    }
    
}
