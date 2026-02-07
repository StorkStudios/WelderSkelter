using UnityEngine;

[CreateAssetMenu(fileName = "ScrapSellMoneyUpgrade", menuName = "Upgrades/ScrapSellMoneyUpgrade")]
public class ScrapSellMoneyUpgrade : Upgrade, IUpgrade<MoneyManager.MoneyManagerModifiers>
{
    [SerializeField]
    private float upgradeValue;

    public void ApplyModifier(MoneyManager.MoneyManagerModifiers modifier)
    {
        modifier.scrapSellMoneyMultipler *= 1 + upgradeValue;
    }
}
