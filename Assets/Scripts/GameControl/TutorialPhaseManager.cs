using UnityEngine;
using StorkStudios.CoreNest;

[RequireComponent(typeof(AudioSource))]
public class TutorialPhaseManager : Singleton<TutorialPhaseManager>
{
    [SerializeField]
    private UnityEngine.UI.Button closeTutorialButton;

    public event System.Action TutorialPhaseEnded;

    private AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        closeTutorialButton.onClick.AddListener(EndTutorialPhase);
        TutorialUI.Instance.TextAnimationEnded += () => closeTutorialButton.interactable = true;
    }

    public void BeginTutorialPhase(string tutorialText)
    {
        TutorialUI.Instance.ShowTutorial(tutorialText);
        closeTutorialButton.interactable = false;
        if (audioSource != null)
        {
            audioSource.Play();
        }
    }

    public void EndTutorialPhase()
    {
        TutorialPhaseEnded?.Invoke();
    }
}
