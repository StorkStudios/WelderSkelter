using System.Collections.Generic;
using UnityEngine;
using StorkStudios.CoreNest;

[System.Serializable]
public class GameManagerHelper
{
    public enum Phase { Menu, Work, Shop, Tutorial, Win, Lose }

    [SerializeField]
    [ReadOnly]
    private int currentDay;

    [SerializeField]
    private List<DayshiftData> dayshifts;

    [SerializeField]
    private SerializedDictionary<Phase, GameObject> phaseParents;

    public bool IsLastDay => currentDay >= dayshifts.Count - 1;

    public void StartGame()
    {
        currentDay = 0;
        SetPhase(Phase.Work);
    }

    public void SetPhase(Phase phase)
    {
        foreach (Phase key in phaseParents.Keys)
        {
            phaseParents[key].SetActive(key == phase);
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
                EndScreenController.Instance.SetScreen(EndScreenController.Screen.Win);
                break;
            case Phase.Lose:
                EndScreenController.Instance.SetScreen(EndScreenController.Screen.Lose);
                break;
        }
    }

    public void StartNextDay()
    {
        currentDay++;
    }
}
