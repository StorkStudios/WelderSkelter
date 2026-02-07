using UnityEngine;

namespace GreenUpgrades
{
    [CreateAssetMenu(fileName = "NewItemIntervalUpgrade", menuName = "Upgrades/Green/NewItemIntervalUpgrade")]
    public class NewItemIntervalUpgrade : Upgrade, IUpgrade<Pusher.PusherModifier>
    {
        [SerializeField]
        private float newItemIntervalMultiplier = 0.1f;

        public void ApplyModifier(Pusher.PusherModifier modifier)
        {
            modifier.itemDelayMultiplier *= newItemIntervalMultiplier;
        }
    }
}