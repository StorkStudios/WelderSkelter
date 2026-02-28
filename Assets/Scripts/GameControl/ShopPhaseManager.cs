using StorkStudios.CoreNest;
using StorkStudios.DataWaste;
using UnityEngine;

public class ShopPhaseManager : Singleton<ShopPhaseManager>
{
    [SerializeField]
    [ReadOnly]
    private DayshiftData nextDayData;

    public event System.Action ShopPhaseEnded;
    public event System.Action ShopPhaseStarted;

    public DayshiftData NextDayData => nextDayData;

    public void BeginShopPhase(DayshiftData nextDayData)
    {
        this.nextDayData = nextDayData;

        ItemShop.Instance.ShowShop();
        ShopPhaseStarted?.Invoke();
    }

    public void EndShopPhase()
    {
        ShopPhaseEnded?.Invoke();
    }
}
