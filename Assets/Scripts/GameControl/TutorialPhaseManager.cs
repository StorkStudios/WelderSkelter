using UnityEngine;
using StorkStudios.CoreNest;

public class TutorialPhaseManager : Singleton<TutorialPhaseManager>
{
    [SerializeField]
    private UnityEngine.UI.Button closeTutorialButton;

    public event System.Action TutorialPhaseEnded;

    private void Start()
    {
        closeTutorialButton.onClick.AddListener(EndTutorialPhase);
        TutorialUI.Instance.TextAnimationEnded += () => closeTutorialButton.interactable = true;
    }

    public void BeginTutorialPhase(string tutorialText)
    {
        TutorialUI.Instance.ShowTutorial(tutorialText);
        closeTutorialButton.interactable = false;
    }

    public void EndTutorialPhase()
    {
        TutorialPhaseEnded?.Invoke();
    }
}
