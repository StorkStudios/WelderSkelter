using UnityEngine;

namespace GreenUpgrades
{
    [CreateAssetMenu(fileName = "ItemMoveSpeedUpgrade", menuName = "Upgrades/Green/ItemMoveSpeedUpgrade")]
    public class ItemMoveSpeedUpgrade : Upgrade, IUpgrade<Pusher.PusherModifier>
    {
        [SerializeField]
        private float moveSpeedMultiplier = 0.1f;

        public void ApplyModifier(Pusher.PusherModifier modifier)
        {
            modifier.initialSpeedMultiplier *= moveSpeedMultiplier;
        }
    }
}