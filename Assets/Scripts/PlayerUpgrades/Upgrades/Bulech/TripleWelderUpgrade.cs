using UnityEngine;

namespace BulechUpgrades
{
    [CreateAssetMenu(fileName = "TripleWelderUpgrade", menuName = "Upgrades/Bulech/TripleWelderUpgrade")]
    public class TripleWelderUpgrade : Upgrade, IUpgrade<Welder.WelderModifiers>
    {
        [SerializeField]
        private float welderPositionRadiusIncrement;
        [SerializeField]
        private int welderCountIncrement;

        public void ApplyModifier(Welder.WelderModifiers modifier)
        {
            modifier.welderPositionRadius += welderPositionRadiusIncrement;
            modifier.welderCount += welderCountIncrement;
        }
    }
}