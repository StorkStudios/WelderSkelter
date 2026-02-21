using DG.Tweening;
using StorkStudios.CoreNest;
using TMPro;
using UnityEngine;

public class TutorialUI : Singleton<TutorialUI>
{
    [Header("UI References")]
    [SerializeField]
    private TextMeshProUGUI tutorialTextLabel;

    [Header("Typing Settings")]
    [SerializeField]
    [Min(0.001f)]
    private float textFadeSpeed;
    [SerializeField]
    [Min(0.001f)]
    private float textSequenceSpeed;

    public event System.Action TextAnimationEnded;

    public void ShowTutorial(string text)
    {
        if (text == null || string.IsNullOrEmpty(text))
        {
            return;
        }

        tutorialTextLabel.text = "";
        TMP_TextInfo textInfo = tutorialTextLabel.GetTextInfo(text);
        Sequence sequence = textInfo.AnimateTextWordByWord(1 / textFadeSpeed, 1 / textSequenceSpeed);
        sequence = sequence.OnComplete(() => TextAnimationEnded?.Invoke());
    }
}

