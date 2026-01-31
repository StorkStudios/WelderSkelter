using UnityEngine;

public abstract class Upgrade : ScriptableObject
{
    [SerializeField]
    private string description;

    public string Description => description;
}
