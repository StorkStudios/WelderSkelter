using UnityEngine;

namespace BulechUpgrades
{
    [CreateAssetMenu(fileName = "MaskOffEyeInvincibilityUpgrade", menuName = "Upgrades/Bulech/MaskOffEyeInvincibilityUpgrade")]
    public class MaskOffEyeInvincibilityUpgrade : Upgrade, IUpgrade<EyesightManager.EyesightModifier>
    {
        [SerializeField]
        private float eyeDamageMultiplier;
        [SerializeField]
        private float duration;

        public void ApplyModifier(EyesightManager.EyesightModifier modifier)
        {
            modifier.maskOffTemporaryDamageMultipliers.Add((duration, eyeDamageMultiplier));
        }
    }
}