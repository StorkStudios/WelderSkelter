using UnityEngine;

namespace GreenUpgrades
{
    [CreateAssetMenu(fileName = "ScrapMoneyIncomeUpgrade", menuName = "Upgrades/Green/ScrapMoneyIncomeUpgrade")]
    public class ScrapMoneyIncomeUpgrade : Upgrade, IUpgrade<MoneyManager.MoneyManagerModifiers>
    {
        [SerializeField]
        private float scrapIncomeMultiplier;

        public void ApplyModifier(MoneyManager.MoneyManagerModifiers modifier)
        {
            modifier.scrapSellMoneyMultipler *= scrapIncomeMultiplier;
        }
    }
}