using UnityEngine;

namespace BulechUpgrades
{
    [CreateAssetMenu(fileName = "QuestHealingUpgrade", menuName = "Upgrades/Bulech/QuestHealingUpgrade")]
    public class QuestHealingUpgrade : Upgrade, IUpgrade<EyesightManager.EyesightModifier>
    {
        [SerializeField]
        private float missingHealthPercentageHealIncrement = 0.1f;

        public void ApplyModifier(EyesightManager.EyesightModifier modifier)
        {
            modifier.missingHealthPercentageHeal += missingHealthPercentageHealIncrement;
        }
    }
}