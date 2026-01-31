using UnityEngine;

public abstract class Task : ScriptableObject
{
    [SerializeField]
    private string title;

    public string Title => title;

    public abstract void Complete();
}
