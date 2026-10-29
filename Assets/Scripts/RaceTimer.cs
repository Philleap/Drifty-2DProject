using UnityEngine;
using TMPro;

public class RaceTimer : MonoBehaviour
{
    public TMP_Text timerText;
    public TMP_Text bestTimeText;

    public float currentTime = 0f;
    public float bestTime = 0f;

    private bool isRunning = false;

    void Start()
    {
        bestTime = PlayerPrefs.GetFloat("BestTime", -1f);

        UpdateUI();
    }

    void Update()
    {
        if (isRunning)
        {
            currentTime += Time.deltaTime;
            UpdateUI();
        }
    }

    public void StartRace()
    {
        currentTime = 0f;
        isRunning = true;
    }

    public void FinishRace()
    {
        if (!isRunning)
            return;

        isRunning = false;

        if (bestTime < 0 || currentTime < bestTime)
        {
            bestTime = currentTime;

            PlayerPrefs.SetFloat("BestTime", bestTime);
            PlayerPrefs.Save();
        }

        UpdateUI();
    }

    void UpdateUI()
    {
        timerText.text = FormatTime(currentTime);

        if (bestTime < 0)
            bestTimeText.text = "Best: --:--.--";
        else
            bestTimeText.text = "Best: " + FormatTime(bestTime);
    }

    string FormatTime(float time)
    {
        int minutes = Mathf.FloorToInt(time / 60f);
        float seconds = time % 60f;

        return $"{minutes:00}:{seconds:00.00}";
    }
}