using UnityEngine;

namespace BulechUpgrades
{
    [CreateAssetMenu(fileName = "LessItemsMoreMoneyUpgrade", menuName = "Upgrades/Bulech/LessItemsMoreMoneyUpgrade")]
    public class LessItemsMoreMoneyUpgrade : Upgrade, IUpgrade<MoneyManager.MoneyManagerModifiers>
    {
        [SerializeField]
        private float incomeMultiplier;
        [SerializeField]
        private float perItemIncomeMultiplier;

        public void ApplyModifier(MoneyManager.MoneyManagerModifiers modifier)
        {
            modifier.allIncomeMultipler *= incomeMultiplier;
            modifier.perItemIncomeMultiplier *= perItemIncomeMultiplier;
        }
    }
}