using UnityEngine;

namespace CristalRush
{
    public class ExitPortal : MonoBehaviour
    {
        public Color lockedColor = new Color(1f, 0.15f, 0.15f);
        public Color unlockedColor = new Color(0.2f, 1f, 0.4f);
        public Renderer glowRenderer;
        public float triggerRadius = 2f;
       
        public bool IsUnlocked { get; private set; }

        void Start()
        {
            if (glowRenderer != null) glowRenderer.material.color = lockedColor;
        }

        public void Unlock()
        {
            IsUnlocked = true;
            if (glowRenderer != null) glowRenderer.material.color = unlockedColor;
        }

        void Update()
        {
            transform.Rotate(0f, 60f * Time.deltaTime, 0f, Space.World);

            GameManager gm = GameManager.Instance;
            if (gm == null || gm.State != GameState.Playing || gm.player == null || !IsUnlocked) return;

            float dist = Vector3.Distance(transform.position, gm.player.transform.position);
            if (dist <= triggerRadius) gm.ReachExit();
        }
    }
}
