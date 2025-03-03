using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace pdkmMenu
{
    public class FreeCamMod
    {
        private static CharacterController MyCharacterController = null;
        private static Rigidbody MyRigidbody = null;

        private static GameObject FreeCamHolder = null;
        public static Camera FreeCamera = null;
        private static AudioListener ears;
        private static Vector3 rotateValue = Vector3.zero;
        public static bool _isFreeCam = false;

        public static bool IsFreeCam
        {
            get => _isFreeCam;
            set
            {
                if (_isFreeCam == value) return;

                _isFreeCam = value;

                if (_isFreeCam)
                {
                    NoClipMod._isNoClip = false;
                    NoClipMod.DisableNoClip();

                    EnableFreeCam();
                }
                else
                {
                    DisableFreeCam();
                }
            }
        }

        public static void UpdateMod()
        {
            if (FreeCamHolder == null) CreateFreeCam();
            SetChar();

            if (!IsFreeCam) return;

            UpdateFreeCam();
        }

        private static void SetChar()
        {
            if (MyCharacterController != null) return;

            var player = GameNetworkManager.Instance?.localPlayerController;
            if (player == null)
            {
                Plugin.Logger.LogWarning("Player controller is null!");
                return;
            }

            MyCharacterController = player.GetComponent<CharacterController>();
            MyRigidbody = player.GetComponent<Rigidbody>();

            if (MyCharacterController == null)
                Plugin.Logger.LogWarning("CharacterController not found on player!");
            if (MyRigidbody == null)
                Plugin.Logger.LogWarning("Rigidbody not found on player!");
        }

        private static void CreateFreeCam()
        {
            FreeCamHolder = new GameObject("FreeCamHolder");
            if (FreeCamera == null)
            {
                FreeCamera = FreeCamHolder.AddComponent<Camera>();
                ears = FreeCamHolder.AddComponent<AudioListener>();

                if (ears == null)
                    Plugin.Logger.LogWarning("Failed to add AudioListener!");

                ears.enabled = false;
                FreeCamera.enabled = false;
            }

            if (FreeCamera != null)
            {
                Plugin.Logger.LogInfo("Added camera");
                FreeCamera.cullingMask = 20649983;
            }
            else
            {
                Plugin.Logger.LogError("Failed to add camera");
            }
        }

        private static void UpdateFreeCam()
        {
            if (FreeCamera == null)
            {
                Plugin.Logger.LogError("FreeCamera is null!");
                return;
            }

            float speed = 20f;
            float rotationSpeed = 2f;

            if (Keyboard.current.leftShiftKey.isPressed)
            {
                speed *= 2;
            }

            Vector2 moveInput = Vector2.zero;
            if (Keyboard.current.wKey.isPressed) moveInput += Vector2.up;
            if (Keyboard.current.sKey.isPressed) moveInput += Vector2.down;
            if (Keyboard.current.aKey.isPressed) moveInput += Vector2.left;
            if (Keyboard.current.dKey.isPressed) moveInput += Vector2.right;

            float upDownInput = 0f;
            if (Keyboard.current.spaceKey.isPressed) upDownInput = 0.75f;
            if (Keyboard.current.leftCtrlKey.isPressed) upDownInput = -0.75f;

            Vector2 rotationInput = Mouse.current.delta.ReadValue() * rotationSpeed;

            rotateValue += new Vector3(-rotationInput.y, rotationInput.x, 0f) * 0.1f;
            rotateValue.x = Mathf.Clamp(rotateValue.x, -89f, 89f);

            FreeCamera.transform.eulerAngles = rotateValue;

            Vector3 movement = new Vector3(moveInput.x, upDownInput, moveInput.y) * speed * Time.deltaTime;
            FreeCamera.transform.Translate(movement);
        }

        private static void EnableFreeCam()
        {
            SetChar();
            if (MyCharacterController == null)
            {
                Plugin.Logger.LogError("MyCharacterController is null! Cannot enable FreeCam.");
                return;
            }

            var player = GameNetworkManager.Instance?.localPlayerController;
            if (player == null)
            {
                Plugin.Logger.LogError("Player controller is null! Cannot enable FreeCam.");
                return;
            }

            MyCharacterController.enabled = false;

            if (player.activeAudioListener != null)
                player.activeAudioListener.enabled = false;
            else
                Plugin.Logger.LogWarning("activeAudioListener is null!");

            if (FreeCamera != null)
            {
                FreeCamera.enabled = true;
                FreeCamera.transform.position = MyCharacterController.transform.position;
                FreeCamera.transform.rotation = MyCharacterController.transform.rotation;
            }
            else
            {
                Plugin.Logger.LogError("FreeCamera is null! Cannot enable FreeCam.");
            }

            if (ears != null)
                ears.enabled = true;
            else
                Plugin.Logger.LogWarning("ears (AudioListener) is null!");
        }

        public static void DisableFreeCam()
        {
            var player = GameNetworkManager.Instance?.localPlayerController;
            if (player != null && player.activeAudioListener != null)
            {
                player.activeAudioListener.enabled = true;
            }
            else
            {
                Plugin.Logger.LogWarning("Player or activeAudioListener is null! Cannot restore audio listener.");
            }

            if (MyCharacterController == null)
            {
                Plugin.Logger.LogError("MyCharacterController is null! Cannot disable FreeCam.");
            }
            else
            {
                MyCharacterController.enabled = true;
            }

            if (FreeCamera != null)
                FreeCamera.enabled = false;
            else
                Plugin.Logger.LogWarning("FreeCamera is null!");

            if (ears != null)
                ears.enabled = false;
            else
                Plugin.Logger.LogWarning("ears (AudioListener) is null!");
        }
    }
}
