using UnityEngine;

namespace BulechUpgrades
{
    [CreateAssetMenu(fileName = "MaskOnMoneyUpgrade", menuName = "Upgrades/Bulech/MaskOnMoneyUpgrade")]
    public class MaskOnMoneyUpgrade : Upgrade, IUpgrade<MoneyTask.MoneyTaskModifier>
    {
        [SerializeField]
        private float incomeMultiplier;
        [SerializeField]
        private float maskOnActivationDuration;

        public void ApplyModifier(MoneyTask.MoneyTaskModifier modifier)
        {
            modifier.maskOnForDurationMultipliers.Add((maskOnActivationDuration, incomeMultiplier));
        }
    }
}