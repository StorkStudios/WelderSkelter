using UnityEngine;

namespace GrayUpgrades
{
    [CreateAssetMenu(fileName = "NewItemIntervalUpgrade", menuName = "Upgrades/Gray/NewItemIntervalUpgrade")]
    public class NewItemIntervalUpgrade : Upgrade, IUpgrade<Pusher.PusherModifier>
    {
        [SerializeField]
        private float newItemIntervalMultiplier = 0.1f;

        public void ApplyModifier(Pusher.PusherModifier modifier)
        {
            modifier.maskOnItemDelayMultiplier *= newItemIntervalMultiplier;
        }
    }
}