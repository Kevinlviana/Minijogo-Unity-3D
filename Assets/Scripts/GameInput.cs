using UnityEngine;
using UnityEngine.InputSystem;

public class GameInput : MonoBehaviour
{
    static GameInput instance;

    InputActionMap map;
    InputAction moveAction;
    InputAction lookAction;
    InputAction jumpAction;
    InputAction restartAction;

    Vector2 move;
    Vector2 look;
    bool jumpDown;
    bool restartDown;

    public static Vector2 Move => instance != null ? instance.move : Vector2.zero;
    public static Vector2 Look => instance != null ? instance.look : Vector2.zero;

    public static bool JumpDown => instance != null && instance.Consume(ref instance.jumpDown);

    public static bool RestartDown => instance != null && instance.Consume(ref instance.restartDown);

    bool Consume(ref bool flag)
    {
        bool value = flag;
        flag = false;
        return value;
    }

    void Awake() // Por algum motivo o jeito tradicional de input não estava funcionando, então estou usando o Input System da Unity por codigo.
    {
        instance = this; 

        map = new InputActionMap("Player");

        moveAction = map.AddAction("Move", InputActionType.Value, expectedControlLayout: "Vector2");
        moveAction.AddCompositeBinding("2DVector")
            .With("Up", "<Keyboard>/w").With("Up", "<Keyboard>/upArrow")
            .With("Down", "<Keyboard>/s").With("Down", "<Keyboard>/downArrow")
            .With("Left", "<Keyboard>/a").With("Left", "<Keyboard>/leftArrow")
            .With("Right", "<Keyboard>/d").With("Right", "<Keyboard>/rightArrow");
        moveAction.performed += OnMove;
        moveAction.canceled += OnMove;

        lookAction = map.AddAction("Look", InputActionType.Value, "<Mouse>/delta");
        lookAction.performed += OnLook;
        lookAction.canceled += OnLook;

        jumpAction = map.AddAction("Jump", InputActionType.Button, "<Keyboard>/space");
        jumpAction.performed += OnJump;

        restartAction = map.AddAction("Restart", InputActionType.Button, "<Keyboard>/r");
        restartAction.performed += OnRestart;
    }

    void OnEnable() => map?.Enable();
    void OnDisable() => map?.Disable();

    void OnDestroy()
    {
        if (instance == this) instance = null;
        moveAction.performed -= OnMove;
        moveAction.canceled -= OnMove;
        lookAction.performed -= OnLook;
        lookAction.canceled -= OnLook;
        jumpAction.performed -= OnJump;
        restartAction.performed -= OnRestart;
        map?.Dispose();
    }

    void OnMove(InputAction.CallbackContext ctx) => move = ctx.ReadValue<Vector2>();
    void OnLook(InputAction.CallbackContext ctx) => look = ctx.ReadValue<Vector2>() * 0.1f;
    void OnJump(InputAction.CallbackContext ctx) => jumpDown = true;
    void OnRestart(InputAction.CallbackContext ctx) => restartDown = true;
}
