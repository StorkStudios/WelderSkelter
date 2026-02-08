using UnityEngine;

namespace BulechUpgrades
{
    [CreateAssetMenu(fileName = "MaskHealingUpgrade", menuName = "Upgrades/Bulech/MaskHealingUpgrade")]
    public class MaskHealingUpgrade : Upgrade, IUpgrade<EyesightManager.EyesightModifier>
    {
        [SerializeField]
        private float maskOnMaxHealthPercentageHealIncrement = 0.1f;

        public void ApplyModifier(EyesightManager.EyesightModifier modifier)
        {
            modifier.maxHealthPercentageHeal += maskOnMaxHealthPercentageHealIncrement;
        }
    }
}