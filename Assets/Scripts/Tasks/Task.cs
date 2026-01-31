using System.Collections.Generic;
using UnityEngine;

public abstract class Task : ScriptableObject
{
    [SerializeField]
    private string title;
    [SerializeField]
    private List<WeldingPartData> requiredParts;

    public string Title => title;
    public List<WeldingPartData> RequiredParts => requiredParts;

    public abstract void Complete();
}
