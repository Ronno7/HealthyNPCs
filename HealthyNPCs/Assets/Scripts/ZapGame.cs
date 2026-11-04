using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ZapGame : MonoBehaviour
{
    [SerializeField] private Zapper zapper = null;
    [SerializeField] private Text statsText = null;
    [SerializeField] private GameObject winPanel = null;
    [SerializeField] private Text winText = null;

    private int cubesLeft;
    private float elapsedTime;

    private void Start()
    {
        foreach (NPC npc in FindObjectsByType<NPC>(FindObjectsSortMode.None))
        {
            npc.GetComponent<IHealth>().OnDied += HandleNPCDied;
            cubesLeft++;
        }

        winPanel.SetActive(false);
    }

    private void Update()
    {
        if (Keyboard.current != null && Keyboard.current.rKey.wasPressedThisFrame)
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

        if (cubesLeft > 0)
            elapsedTime += Time.deltaTime;

        statsText.text = "Time  " + elapsedTime.ToString("0.0") + "s\nZaps  " + zapper.ZapCount + "\nCubes  " + cubesLeft;
    }

    private void HandleNPCDied()
    {
        cubesLeft--;

        if (cubesLeft == 0)
        {
            zapper.CanZap = false;
            winText.text = "ALL CUBES DOWN!\n" + elapsedTime.ToString("0.0") + "s with " + zapper.ZapCount + " zaps\n<size=22>Press R to play again</size>";
            winPanel.SetActive(true);
        }
    }
}
