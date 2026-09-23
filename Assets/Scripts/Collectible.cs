using UnityEngine;

namespace CristalRush
{
    public class Collectible : MonoBehaviour
    {
        public float pickupRadius = 1.2f;

        bool collected;

        void Update()
        {
            GameManager gm = GameManager.Instance;
            if (collected || gm == null || gm.State != GameState.Playing || gm.player == null) return;

            float dist = Vector3.Distance(transform.position, gm.player.transform.position + Vector3.up);
            if (dist <= pickupRadius)
            {
                collected = true;
                gm.CollectCrystal();
                gameObject.SetActive(false);
            }
        }
    }
}
