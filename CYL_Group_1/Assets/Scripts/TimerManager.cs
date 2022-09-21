using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class TimerManager : MonoBehaviour
{
    [SerializeField] int timeLeft = 360;
    [SerializeField] TextMeshProUGUI timerText;
    public bool paused = false;

    void Start()
    {
        InvokeRepeating(nameof(DecreaseTimer), 0, 1);
    }

    void DecreaseTimer()
    {
        if (paused) return;

        if (timeLeft <= 0)
        {
            gameObject.GetComponent<DeathHandler>().HandleDeath();
            return;
        }

        timeLeft--;

        int i = timeLeft;
        int minutes = 0;
        int seconds = 0;
        while (i > 0)
        {
            if (i >= 60)
            {
                i -= 60;
                minutes++;
            }
            else
            {
                seconds = i;
                break;
            }
        }

        string secondsString;
        if (seconds <= 9)
        {
            secondsString = $"0{seconds}";
        }
        else
        {
            secondsString = $"{seconds}";
        }

        timerText.text = $"{minutes}:{secondsString}";
    }
}
