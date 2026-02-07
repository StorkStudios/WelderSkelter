using UnityEngine;

namespace GrayUpgrades
{
    [CreateAssetMenu(fileName = "WeldingPointRadiusUpgrade", menuName = "Upgrades/Gray/WeldingPointRadiusUpgrade")]
    public class WeldingPointRadiusUpgrade : Upgrade, IUpgrade<Welder.WelderModifiers>
    {
        [SerializeField]
        private float radiusMultiplier = 0.1f;

        public void ApplyModifier(Welder.WelderModifiers modifier)
        {
            modifier.maskOnRadiusMultiplier *= radiusMultiplier;
        }
    }
}