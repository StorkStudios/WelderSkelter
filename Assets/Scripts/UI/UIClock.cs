using TMPro;
using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(AudioSource))]
public class UIClock : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI clock;
    [SerializeField]
    private Color normal;
    [SerializeField]
    private Color accent;
    [SerializeField]
    private float accentStartTime;
    [SerializeField]
    private AudioClip tickingAudioClip;
    [SerializeField]
    private AudioClip endAudioClip;
    [SerializeField]
    private int tickingStartTime;

    private AudioSource audioSource;
    private int lastTickedSecond = 0;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        lastTickedSecond = tickingStartTime;
    }

    private void Update()
    {
        float timeLeft = WorkPhaseManager.Instance.CurrentData.dayLength - WorkPhaseManager.Instance.DayTimer;
        int minutes = (int)(timeLeft / 60);
        int seconds = (int)(timeLeft % 60);
        if (seconds < 10)
        {
            clock.text = $"{minutes}:0{seconds}";
        }
        else
        {
            clock.text = $"{minutes}:{seconds}";
        }
        if (timeLeft <= accentStartTime && seconds % 2 == 0)
        {
            clock.color = accent;
        }
        else
        {
            clock.color = normal;
        }

        if (WorkPhaseManager.Instance.CurrentData.dayLength > 0)
        {
            if ((int)timeLeft <= 0 && !audioSource.isPlaying)
            {
                lastTickedSecond = tickingStartTime;
                audioSource.PlayOneShot(endAudioClip);
            }
            else if ((int)timeLeft <= lastTickedSecond && (int)timeLeft > 0)
            {
                lastTickedSecond--;
                audioSource.PlayOneShot(tickingAudioClip);
            }
        }
    }
}
