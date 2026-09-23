using UnityEngine;

namespace CristalRush
{
    public class SimpleUI : MonoBehaviour
    {
        GUIStyle hudStyle, bigStyle, smallStyle;

        void EnsureStyles()
        {
            if (hudStyle != null) return;
            hudStyle = new GUIStyle { fontSize = 26, normal = { textColor = Color.white } };
            bigStyle = new GUIStyle { fontSize = 48, alignment = TextAnchor.MiddleCenter, normal = { textColor = Color.white } };
            smallStyle = new GUIStyle { fontSize = 24, alignment = TextAnchor.MiddleCenter, normal = { textColor = Color.white } };
        }

        void OnGUI()
        {
            GameManager gm = GameManager.Instance;
            if (gm == null) return;
            EnsureStyles();

            GUI.Label(new Rect(20, 16, 400, 40), "Cristais: " + gm.CrystalsCollected + " / " + gm.crystalsTotal, hudStyle);
            GUI.Label(new Rect(20, 46, 400, 40), "Tempo: " + Mathf.CeilToInt(Mathf.Max(0f, gm.TimeLeft)) + "s", hudStyle);

            if (gm.State != GameState.Playing)
            {
                GUI.Box(new Rect(Screen.width / 2f - 260, Screen.height / 2f - 90, 520, 180), GUIContent.none);
                GUI.Label(new Rect(Screen.width / 2f - 260, Screen.height / 2f - 60, 520, 60),
                    gm.State == GameState.Won ? "VITÓRIA!" : "FIM DE JOGO", bigStyle);
                GUI.Label(new Rect(Screen.width / 2f - 260, Screen.height / 2f, 520, 40), gm.EndMessage, smallStyle);
                GUI.Label(new Rect(Screen.width / 2f - 260, Screen.height / 2f + 40, 520, 40), "Pressione R para reiniciar", smallStyle);
            }
        }
    }
}
