using UnityEngine;

[CreateAssetMenu(fileName = "NewItemIntervalUpgrade", menuName = "Scriptable Objects/NewItemIntervalUpgrade")]
public class NewItemIntervalUpgrade : Upgrade, IUpgrade<Pusher.PusherModifier>
{
    [SerializeField]
    private float timeReduction;
    
    public void ApplyModifier(Pusher.PusherModifier modifier)
    {
        modifier.DelayBetweenItemGroups *= 1f - timeReduction;
    }
}
