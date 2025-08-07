using UnityEngine;
using UnityEngine.Events;

public enum GameMode
{
    MainMenu,
    InGame,
    PauseMenu
}
public class InputManager : MonoBehaviour
{
    public static InputManager Instance { get; private set; }
    private GameInputActions gameInputActions;
    private Vector2 moveInput;

    //EVENTS
    //PlayerActions
    public UnityEvent OnPlayerInputMovePerformed;

    //UIActions


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        gameInputActions = new GameInputActions();
    }
    private void OnEnable()
    {
        gameInputActions.Enable();
        SetGameMode(GameMode.InGame);
    }
    private void OnDisable()
    {
        gameInputActions.Disable();
    }
    public void SetGameMode(GameMode mode)
    {
        switch (mode)
        {
            case GameMode.MainMenu:
                EnablePlayerActions(false);
                EnableUIActions(true);
                break;
            case GameMode.InGame:
                EnablePlayerActions(true);
                EnableUIActions(false);
                break;
            case GameMode.PauseMenu:
                EnablePlayerActions(false);
                EnableUIActions(true);
                break;
            default:
                Debug.LogError("Unknown game mode: " + mode);
                break;
        }
    }
    private void EnablePlayerActions(bool enable)
    {
        if (enable)
        {
            gameInputActions.PlayerActions.Enable();
            gameInputActions.PlayerActions.Move.performed += ctx => 
            {
                moveInput = ctx.ReadValue<Vector2>();
                OnPlayerInputMovePerformed?.Invoke();
            };
        }
        else
        {
            gameInputActions.PlayerActions.Disable();
        }
    }
    private void EnableUIActions(bool enable)
    {
        if (enable)
        {
            gameInputActions.UIActions.Enable();
        }
        else
        {
            gameInputActions.UIActions.Disable();
        }
    }
    public Vector2 GetMoveInput()
    {
        return moveInput;
    }

}
