using UnityEngine;

namespace GreenUpgrades
{
    [CreateAssetMenu(fileName = "MoneyIncomeMultUpgrade", menuName = "Upgrades/Green/MoneyIncomeMultUpgrade")]
    public class MoneyIncomeMultUpgrade : Upgrade, IUpgrade<MoneyManager.MoneyManagerModifiers>
    {
        [SerializeField]
        private float incomeMultiplier;

        public void ApplyModifier(MoneyManager.MoneyManagerModifiers modifier)
        {
            modifier.allIncomeMultipler *= incomeMultiplier;
        }
    }
}