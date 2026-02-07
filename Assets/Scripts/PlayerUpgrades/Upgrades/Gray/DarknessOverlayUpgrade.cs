using UnityEngine;

namespace GrayUpgrades
{
    [CreateAssetMenu(fileName = "DarknessOverlayUpgrade", menuName = "Upgrades/Gray/DarknessOverlayUpgrade")]
    public class DarknessOverlayUpgrade : Upgrade, IUpgrade<WeldingMask.WeldingMaskModifier>
    {
        [SerializeField]
        private float darknesMultiplier;

        public void ApplyModifier(WeldingMask.WeldingMaskModifier modifier)
        {
            modifier.darknessAlpha *= darknesMultiplier;
        }
    }
}