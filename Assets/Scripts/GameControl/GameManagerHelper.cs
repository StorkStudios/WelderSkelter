using StorkStudios.CoreNest;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using SceneEnum = StorkStudios.CoreNest.Scene;

[System.Serializable]
public class GameManagerHelper
{
    public enum Phase { Menu, Work, Shop, Tutorial, Win, Lose, Init, Exit }

    [SerializeField]
    [ReadOnly]
    private int currentDay;

    [SerializeField]
    private List<DayshiftData> dayshifts;

    [SerializeField]
    private SerializedDictionary<Phase, GameObject> phaseParents;

    public bool IsLastDay => currentDay >= dayshifts.Count - 1;

    private ScreenMaskManager screenMaskManager;

    private Phase currentPhase = Phase.Init;

    private bool fadeEnded;

    public IEnumerator StartGame()
    {
        currentDay = 0;
        return SetPhase(Phase.Work);
    }

    public IEnumerator SetPhase(Phase phase)
    {
        if (screenMaskManager == null)
        {
            screenMaskManager = phaseParents[Phase.Init].GetComponent<ScreenMaskManager>();
        }

        yield return null;

        if (currentPhase != Phase.Init || phase == Phase.Tutorial)
        {
            fadeEnded = false;
            phaseParents[Phase.Init].SetActive(true);
            screenMaskManager.FadeIn(() => fadeEnded = true);
            yield return new WaitUntil(() => fadeEnded);
        }

        if (phase != Phase.Exit)
        {   
            foreach (Phase key in phaseParents.Keys)
            {
                phaseParents[key].SetActive(key == phase);
            }

            fadeEnded = false;
            phaseParents[Phase.Init].SetActive(true);
            screenMaskManager.FadeOut(() => phaseParents[Phase.Init].SetActive(false));
        }

        switch (phase)
        {
            case Phase.Work:
                WorkPhaseManager.Instance.BeginWorkPhase(dayshifts[currentDay]);
                break;
            case Phase.Shop:
                ShopPhaseManager.Instance.BeginShopPhase(dayshifts[currentDay + 1]);
                break;
            case Phase.Win:
                //Win and lose screen use the same object so they may have been disabled before (depends on order in dictionary)
                phaseParents[Phase.Win].SetActive(true);
                EndScreenController.Instance.SetScreen(EndScreenController.Screen.Win);
                break;
            case Phase.Lose:
                phaseParents[Phase.Lose].SetActive(true);
                EndScreenController.Instance.SetScreen(EndScreenController.Screen.Lose);
                break;
        }

        currentPhase = phase;
    }

    public IEnumerator BackToMenu()
    {
        yield return SetPhase(Phase.Exit);
        SceneManager.LoadScene(SceneEnum.SampleScene.GetBuildIndex());
    }

    public void StartNextDay()
    {
        currentDay++;
    }
}
