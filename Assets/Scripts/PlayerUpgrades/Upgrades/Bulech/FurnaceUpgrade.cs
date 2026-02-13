using UnityEngine;

namespace BulechUpgrades
{
    [CreateAssetMenu(fileName = "FurnaceUpgrade", menuName = "Upgrades/Bulech/FurnaceUpgrade")]
    public class FurnaceUpgrade : Upgrade, IUpgrade<Furnace.Modifiers>, IUpgrade<MoneyManager.MoneyManagerModifiers>
    {
        [SerializeField]
        private float incomeMultiplier;
        [SerializeField]
        private float incomeMultiplierDurationIncrement;

        public void ApplyModifier(Furnace.Modifiers modifier)
        {
            modifier.active = true;
        }

        public void ApplyModifier(MoneyManager.MoneyManagerModifiers modifier)
        {
            modifier.furnaceIncomeMultiplier *= incomeMultiplier;
            modifier.furnaceIncomeMultiplierDuration += incomeMultiplierDurationIncrement;
        }
    }
}