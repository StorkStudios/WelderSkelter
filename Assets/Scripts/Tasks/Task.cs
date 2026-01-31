using UnityEngine;

public abstract class Task : ScriptableObject
{
    [SerializeField]
    private string title;

    public abstract void Complete();
}
