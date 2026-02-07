using UnityEngine;

namespace GrayUpgrades
{
    [CreateAssetMenu(fileName = "ItemMoveSpeedUpgrade", menuName = "Upgrades/Gray/ItemMoveSpeedUpgrade")]
    public class ItemMoveSpeedUpgrade : Upgrade, IUpgrade<WeldingPart.WeldingPartModifier>
    {
        [SerializeField]
        private float moveSpeedMultiplier = 0.1f;

        public void ApplyModifier(WeldingPart.WeldingPartModifier modifier)
        {
            modifier.maskOnMoveSpeedMultiplier *= moveSpeedMultiplier;
        }
    }
}