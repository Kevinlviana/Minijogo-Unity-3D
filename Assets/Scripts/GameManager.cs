using UnityEngine;
using UnityEngine.SceneManagement;

namespace CristalRush
{
    public enum GameState { Playing, Won, Lost }

    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }

        public PlayerController player;
        public ExitPortal exitPortal;

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
            }
            else if (GameInput.RestartDown)
            {
                Restart();
            }
        }

        public void CollectCrystal()
        {
            if (State != GameState.Playing) return;
            CrystalsCollected++;
            if (CrystalsCollected >= crystalsTotal && exitPortal != null)
                exitPortal.Unlock();
        }

        public void ReachExit()
        {
            if (State != GameState.Playing) return;
            if (exitPortal != null && !exitPortal.IsUnlocked) return;
            EndMessage = "Você escapou com todos os cristais!";
            State = GameState.Won;
            Unlock();
        }

        public void PlayerCaught()
        {
            Lose("Um guarda te pegou!");
        }

        void Lose(string reason)
        {
            if (State != GameState.Playing) return;
            EndMessage = reason;
            State = GameState.Lost;
            Unlock();
        }

        void Unlock()
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

        public void Restart()
        {
            Time.timeScale = 1f;
            Scene s = SceneManager.GetActiveScene();
            SceneManager.LoadScene(s.buildIndex >= 0 ? s.buildIndex : 0);
        }
    }
}
