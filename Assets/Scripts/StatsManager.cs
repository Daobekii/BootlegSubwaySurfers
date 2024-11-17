using UnityEngine;
using UnityEngine.UI;

public class ScriptManager : MonoBehaviour
{
    public Text coinText;
    public Transform player;
    public Text scoreText;
    public float playerScore = 0;
    public int coinScore;
    private float scoreMultiplier = 4f;
    private float startZ = 0; //nötig, da mein Character nicht bei z = 0 startet
    public Text buffDurationText;
    private BuffHandler buffHandler;

    void Start()
    {
        startZ = player.localPosition.z;
        buffHandler = FindFirstObjectByType<BuffHandler>();
    }

    void Update()
    {
        UpdateScore();

        if (buffHandler != null && buffHandler.IsBuffActive())
        {
            float remainingTime = buffHandler.GetRemainingBuffTime();
            buffDurationText.text = "Buff: " + remainingTime.ToString("F1") + "s";
        }
        else
        {
            buffDurationText.text = ""; // Kein Buff aktiv
        }
    }
    public void AddCoin()
    {
        coinScore++;
        coinText.text = "Coins: " + coinScore;
    }

    void UpdateScore()
    {
        float distanceTraveled = player.localPosition.z - startZ;
        playerScore = distanceTraveled * scoreMultiplier;
        
        playerScore = Mathf.Max(0, playerScore);

        scoreText.text = "Social Credit Score: " + Mathf.FloorToInt(-playerScore).ToString();
    }

}
