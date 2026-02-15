using UnityEngine;

namespace TutorialUpgrades
{
    [CreateAssetMenu(fileName = "PusherDisableUpgrade", menuName = "Upgrades/Tutorial/PusherDisableUpgrade")]
    public class PusherMovementDisableUpgrade : TutorialUpgrade, IUpgrade<Pusher.PusherModifier>
    {
        public void ApplyModifier(Pusher.PusherModifier modifier)
        {
            modifier.disableMovement = true;
        }
    }
}