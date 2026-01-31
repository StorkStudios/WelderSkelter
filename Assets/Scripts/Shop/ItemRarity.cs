using UnityEngine;

[CreateAssetMenu(menuName = "Shop/Item Rarity")]
public class ItemRarity : ScriptableObject
{
    [SerializeField]
    private int rarityWeight;

    public int RarityWeight => rarityWeight;
}
