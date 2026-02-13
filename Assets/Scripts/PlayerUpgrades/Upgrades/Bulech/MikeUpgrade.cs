using UnityEngine;

namespace BulechUpgrades
{
    [CreateAssetMenu(fileName = "MikeUpgrade", menuName = "Upgrades/Bulech/MikeUpgrade")]
    public class MikeUpgrade : Upgrade, IUpgrade<Pusher.PusherModifier>
    {
        [SerializeField]
        private int mikesCountIncrement = 1;

        public void ApplyModifier(Pusher.PusherModifier modifier)
        {
            modifier.mikesCount += mikesCountIncrement;
        }
    }
}