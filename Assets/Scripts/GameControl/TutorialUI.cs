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

        Color color = tutorialTextLabel.color;
        color.a = 0;
        tutorialTextLabel.color = color;
        for (int i = 0; i < tutorialTextLabel.textInfo.characterCount; i++)
        {
            tutorialTextLabel.textInfo.SetCharacterAlpha(i, 0);
        }
        TMP_TextInfo textInfo = tutorialTextLabel.GetTextInfo(text);
        Sequence sequence = textInfo.AnimateTextWordByWord(1 / textFadeSpeed, 1 / textSequenceSpeed);
        sequence = sequence.OnComplete(() => TextAnimationEnded?.Invoke());
    }
}

