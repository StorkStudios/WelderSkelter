using UnityEngine;

namespace BulechUpgrades
{
    [CreateAssetMenu(fileName = "GravityUpgrade", menuName = "Upgrades/Bulech/GravityUpgrade")]
    public class GravityUpgrade : Upgrade, IUpgrade<WeldingPart.WeldingPartModifier>
    {
        [SerializeField]
        private float gravityScaleIncrement;

        public void ApplyModifier(WeldingPart.WeldingPartModifier modifier)
        {
            modifier.gravityScale += gravityScaleIncrement;
        }
    }
}