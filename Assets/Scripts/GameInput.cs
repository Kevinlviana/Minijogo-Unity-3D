using UnityEngine;
using UnityEngine.InputSystem;

namespace CristalRush
{

    public static class GameInput
    {
        static readonly InputActionMap map;
        static readonly InputAction moveAction;
        static readonly InputAction lookAction;
        static readonly InputAction jumpAction;
        static readonly InputAction restartAction;

        static Vector2 moveValue;
        static Vector2 lookValue;
        static bool jumpQueued;
        static bool restartQueued;

        static GameInput()
        {
            map = new InputActionMap("Gameplay");

            moveAction = map.AddAction("Move", InputActionType.Value);
            moveAction.AddCompositeBinding("2DVector")
                .With("Up", "<Keyboard>/w")
                .With("Up", "<Keyboard>/upArrow")
                .With("Down", "<Keyboard>/s")
                .With("Down", "<Keyboard>/downArrow")
                .With("Left", "<Keyboard>/a")
                .With("Left", "<Keyboard>/leftArrow")
                .With("Right", "<Keyboard>/d")
                .With("Right", "<Keyboard>/rightArrow");
            moveAction.performed += OnMovePerformed;
            moveAction.canceled += OnMoveCanceled;

            lookAction = map.AddAction("Look", InputActionType.Value, "<Mouse>/delta");
            lookAction.performed += OnLookPerformed;
            lookAction.canceled += OnLookCanceled;

            jumpAction = map.AddAction("Jump", InputActionType.Button, "<Keyboard>/space");
            jumpAction.performed += OnJumpPerformed;

            restartAction = map.AddAction("Restart", InputActionType.Button, "<Keyboard>/r");
            restartAction.performed += OnRestartPerformed;

            map.Enable();
        }

        static void OnMovePerformed(InputAction.CallbackContext ctx) =>
            moveValue = Vector2.ClampMagnitude(ctx.ReadValue<Vector2>(), 1f);

        static void OnMoveCanceled(InputAction.CallbackContext ctx) => moveValue = Vector2.zero;

        static void OnLookPerformed(InputAction.CallbackContext ctx) =>
            lookValue = ctx.ReadValue<Vector2>() * 0.1f;

        static void OnLookCanceled(InputAction.CallbackContext ctx) => lookValue = Vector2.zero;

        static void OnJumpPerformed(InputAction.CallbackContext ctx) => jumpQueued = true;

        static void OnRestartPerformed(InputAction.CallbackContext ctx) => restartQueued = true;

        public static Vector2 Move => moveValue;

        public static Vector2 Look => lookValue;

        public static bool JumpDown
        {
            get
            {
                if (!jumpQueued) return false;
                jumpQueued = false;
                return true;
            }
        }

        public static bool RestartDown
        {
            get
            {
                if (!restartQueued) return false;
                restartQueued = false;
                return true;
            }
        }
    }
}
