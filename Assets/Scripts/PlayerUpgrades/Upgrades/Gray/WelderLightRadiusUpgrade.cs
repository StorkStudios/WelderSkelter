using UnityEngine;

namespace GrayUpgrades
{
    [CreateAssetMenu(fileName = "WelderLightRadiusUpgrade", menuName = "Upgrades/Gray/WelderLightRadiusUpgrade")]
    public class WelderLightRadiusUpgrade : Upgrade, IUpgrade<WeldingMask.WeldingMaskModifier>
    {
        [SerializeField]
        private float scaleMultiplier;

        public void ApplyModifier(WeldingMask.WeldingMaskModifier modifier)
        {
            modifier.darknessScale *= scaleMultiplier;
        }
    }
}