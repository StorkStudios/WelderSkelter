using StorkStudios.CoreNest;
using UnityEngine;

public class ShopPhaseManager : Singleton<ShopPhaseManager>
{
    [SerializeField]
    [ReadOnly]
    private DayshiftData nextDayData;

    public event System.Action ShopPhaseEnded;

    public DayshiftData NextDayData => nextDayData;

    public void BeginShopPhase(DayshiftData nextDayData)
    {
        this.nextDayData = nextDayData;

        ItemShop.Instance.ShowShop();
    }

    public void EndShopPhase()
    {
        ShopPhaseEnded?.Invoke();
    }
}
