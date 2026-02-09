using UnityEngine;

namespace BulechUpgrades
{
    [CreateAssetMenu(fileName = "SecondPusherUpgrade", menuName = "Upgrades/Bulech/SecondPusherUpgrade")]
    public class SecondPusherUpgrade : Upgrade, IUpgrade<Pusher.PusherModifier>
    {
        [SerializeField]
        private int pusherCountIncrement = 1;

        public void ApplyModifier(Pusher.PusherModifier modifier)
        {
            modifier.pushersCount += pusherCountIncrement;
        }
    }
}