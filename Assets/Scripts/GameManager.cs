using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public enum GameState { Playing, Won, Lost }



public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public PlayerController player;
    public ExitPortal exitPortal;
    public Text crystalsText;
    public Text timerText;
    public GameObject endScreen;
    public Text endTitleText;
    public Text endMessageText;

    public float startTime = 90f;
    public int crystalsTotal;

    public GameState State { get; private set; } = GameState.Playing;
    public float TimeLeft { get; private set; }
    public int CrystalsCollected { get; private set; }
    public string EndMessage { get; private set; } = "";

    void Awake()
    {
        Instance = this;
        Time.timeScale = 1f;
        TimeLeft = startTime;
        crystalsTotal = FindObjectsByType<Collectible>(FindObjectsSortMode.None).Length;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        if (endScreen != null) endScreen.SetActive(false);
        UpdateHud();
    }

    void OnDestroy()
    {
        if (Instance == this) Instance = null;
    }

    void Update()
    {
        if (State == GameState.Playing)
        {
            TimeLeft -= Time.deltaTime;
            if (TimeLeft <= 0f)
            {
                TimeLeft = 0f;
                Lose("O tempo acabou!");
            }
            UpdateHud();
        }
        else if (GameInput.RestartDown)
        {
            Restart();
        }
    }

    void UpdateHud()
    {
        if (crystalsText != null) crystalsText.text = "Cristais: " + CrystalsCollected + " / " + crystalsTotal;
        if (timerText != null) timerText.text = "Tempo: " + Mathf.CeilToInt(Mathf.Max(0f, TimeLeft)) + "s";
    }

    public void CollectCrystal()
    {
        if (State != GameState.Playing) return;
        CrystalsCollected++;
        UpdateHud();
        if (CrystalsCollected >= crystalsTotal && exitPortal != null)
            exitPortal.Unlock();
    }

    public void ReachExit()
    {
        if (State != GameState.Playing) return;
        if (exitPortal != null && !exitPortal.IsUnlocked) return;
        Win("Você escapou com todos os cristais!");
    }

    public void PlayerCaught()
    {
        Lose("Um guarda te pegou!");
    }

    void Win(string message)
    {
        EndMessage = message;
        State = GameState.Won;
        ShowEndScreen("VITÓRIA!");
    }

    void Lose(string reason)
    {
        if (State != GameState.Playing) return;
        EndMessage = reason;
        State = GameState.Lost;
        ShowEndScreen("FIM DE JOGO");
    }

    void ShowEndScreen(string title)
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        if (endScreen != null) endScreen.SetActive(true);
        if (endTitleText != null) endTitleText.text = title;
        if (endMessageText != null) endMessageText.text = EndMessage;
    }

    public void Restart()
    {
        Time.timeScale = 1f;
        Scene s = SceneManager.GetActiveScene();
        SceneManager.LoadScene(s.buildIndex >= 0 ? s.buildIndex : 0);
    }
}

