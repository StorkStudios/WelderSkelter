using StorkStudios.CoreNest;
using UnityEngine;

[CreateAssetMenu(menuName = "Shop/Item")]
public class Item : ScriptableObject
{
    [SerializeField]
    private string title;
    [SerializeField]
    private Color color;

    [SerializeField]
    [EditObjectInInspector]
    private ItemRarity rarity;

    [SerializeField]
    [EditObjectInInspector]
    private Upgrade upgrade;

    [SerializeField]
    private int cost;

    public string Title => title;
    public Color Color => color;
    public ItemRarity Rarity => rarity;
    public Upgrade Upgrade => upgrade;
    public int Cost => cost;
}
