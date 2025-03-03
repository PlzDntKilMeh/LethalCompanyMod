using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

namespace pdkmMenu
{
    public class NoClipMod
    {
        private static CharacterController MyCharacterController = null;
        private static Rigidbody MyRigidbody = null;
        private static float MyCharacterControllerRadius = 0;
        public static bool _isNoClip = false;

        public static bool IsNoClip
        {
            get => _isNoClip;
            set
            {
                if (_isNoClip == value) return;

                _isNoClip = value;

                if (_isNoClip)
                {
                    FreeCamMod._isFreeCam = false;
                    FreeCamMod.DisableFreeCam();
                    EnableNoClip();
                }
                else
                {
                    DisableNoClip();
                }
            }
        }

        public static void UpdateMod()
        {
            if (StartOfRound.Instance.localPlayerController == null) return;
            SetChar();
            if (IsNoClip)
            {
                EnableNoClip();
                NoClipMove();

            }
        }

        private static void SetChar()
        {
            var player = StartOfRound.Instance.localPlayerController;
            if (player == null) return;

            MyCharacterController = player.GetComponent<CharacterController>();
            MyRigidbody = player.GetComponent<Rigidbody>();

            if (MyCharacterController != null && MyCharacterControllerRadius == 0)
            {
                MyCharacterControllerRadius = MyCharacterController.radius;
            }
        }

        

        private static void NoClipMove()
        {
            if (MyCharacterController == null || MyRigidbody == null) return;
            HandleMovement();
        }

        private static void HandleMovement()
        {
            if (GameNetworkManager.Instance.localPlayerController == null) return;

            float speed = Keyboard.current.leftShiftKey.isPressed ? 40f : 20f;
            Vector3 moveDirection = Vector3.zero;

            if (Keyboard.current.wKey.isPressed) moveDirection += Vector3.forward;
            if (Keyboard.current.sKey.isPressed) moveDirection += Vector3.back;
            if (Keyboard.current.aKey.isPressed) moveDirection += Vector3.left;
            if (Keyboard.current.dKey.isPressed) moveDirection += Vector3.right;
            if (Keyboard.current.spaceKey.isPressed) moveDirection += Vector3.up;
            if (Keyboard.current.leftCtrlKey.isPressed) moveDirection += Vector3.down;

            moveDirection.Normalize();  // Prevent diagonal speed boost
            moveDirection *= speed * Time.deltaTime;

            GameNetworkManager.Instance.localPlayerController.transform.Translate(moveDirection);
        }


        private static void EnableNoClip()
        {
            if (MyCharacterController == null || MyRigidbody == null) return;

            StartOfRound.Instance.allowLocalPlayerDeath = false;
            MyCharacterController.enabled = false;
            GameNetworkManager.Instance.localPlayerController.isInHangarShipRoom = false;
            MyRigidbody.useGravity = false;
            MyRigidbody.isKinematic = true;

            //HandleMovement();
        }
        public static void DisableNoClip()
        {
            if (MyCharacterController == null || MyRigidbody == null) return;

            MyCharacterController.enabled = true;
            MyCharacterController.radius = MyCharacterControllerRadius;
            //MyRigidbody.useGravity = true;
            //MyRigidbody.isKinematic = false;

            StartOfRound.Instance.StartCoroutine(DisableGodMode());
        }

        private static IEnumerator DisableGodMode()
        {
            yield return new WaitForSeconds(2.0f);
            StartOfRound.Instance.allowLocalPlayerDeath = Plugin.SelfSettings.GodMode.Value;
        }
    }
}
