using UnityEngine;

namespace GreenUpgrades
{
    [CreateAssetMenu(fileName = "QuestMoneyIncomeUpgrade", menuName = "Upgrades/Green/QuestMoneyIncomeUpgrade")]
    public class QuestMoneyIncomeUpgrade : Upgrade, IUpgrade<MoneyTask.MoneyTaskModifier>
    {
        [SerializeField]
        private float questIncomeMultiplier;

        public void ApplyModifier(MoneyTask.MoneyTaskModifier modifier)
        {
            modifier.questIncomeModifier *= questIncomeMultiplier;
        }
    }
}