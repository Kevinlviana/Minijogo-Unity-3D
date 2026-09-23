using UnityEngine;

namespace CristalRush

{
    public class CameraRig : MonoBehaviour
    {
        public float Yaw { get; private set; }
        public float Pitch { get; private set; }

        public Transform followTarget;
        public PlayerController playerController;

        public float heightOffset = 1.6f;
        public float sensitivity = 2f;
        public float minPitch = -20f;
        public float maxPitch = 60f;
        public float startPitch = 15f;


        void Start()
        {
            if (playerController != null) Yaw = playerController.transform.eulerAngles.y;
            Pitch = startPitch;
        }

        void LateUpdate()
        {
            if (playerController == null || followTarget == null) return;

            GameManager gm = GameManager.Instance;
            if (gm == null || gm.State == GameState.Playing)
            {
                Vector2 look = GameInput.Look;
                Yaw += look.x * sensitivity;
                Pitch = Mathf.Clamp(Pitch - look.y * sensitivity, minPitch, maxPitch);
            }

            followTarget.position = playerController.transform.position + Vector3.up * heightOffset;
            followTarget.rotation = Quaternion.Euler(Pitch, Yaw, 0f);
        }
    }
}
