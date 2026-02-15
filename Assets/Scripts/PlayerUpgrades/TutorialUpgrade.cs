using UnityEngine;
using StorkStudios.CoreNest;

public abstract class TutorialUpgrade : Upgrade
{
    [SerializeField]
    private SerializedSet<TutorialManager.TutorialLevel> tutorialLevels;

    public SerializedSet<TutorialManager.TutorialLevel> TutorialLevels => tutorialLevels;
}