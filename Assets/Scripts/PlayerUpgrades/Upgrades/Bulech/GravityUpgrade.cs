using UnityEngine;

namespace BulechUpgrades
{
    [CreateAssetMenu(fileName = "GravityUpgrade", menuName = "Upgrades/Bulech/GravityUpgrade")]
    public class GravityUpgrade : Upgrade, IUpgrade<WeldingPart.WeldingPartModifier>, IUpgrade<Pusher.PusherModifier>
    {
        [SerializeField]
        private float gravityScaleIncrement;
        [SerializeField]
        private float pusherYForceMultiplier;

        public void ApplyModifier(WeldingPart.WeldingPartModifier modifier)
        {
            modifier.gravityScale += gravityScaleIncrement;
        }

        public void ApplyModifier(Pusher.PusherModifier modifier)
        {
            modifier.pushYForceMultiplier *= pusherYForceMultiplier;
        }
    }
}