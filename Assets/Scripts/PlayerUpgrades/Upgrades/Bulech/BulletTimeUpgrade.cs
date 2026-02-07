using UnityEngine;

namespace BulechUpgrades
{
    [CreateAssetMenu(fileName = "BulletTimeUpgrade", menuName = "Upgrades/Bulech/BulletTimeUpgrade")]
    public class BulletTimeUpgrade : Upgrade, IUpgrade<WeldingPart.WeldingPartModifier>
    {
        [SerializeField]
        private float moveSpeedMultiplier = 0.1f;

        public void ApplyModifier(WeldingPart.WeldingPartModifier modifier)
        {
            modifier.lpmMoveSpeedMultiplier *= moveSpeedMultiplier;
        }
    }
}