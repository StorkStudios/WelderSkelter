using UnityEngine;

[CreateAssetMenu(fileName = "WeldingPartData", menuName = "Scriptable Objects/WeldingPartData")]
public class WeldingPartData : ScriptableObject
{
    [SerializeField]
    private GameObject prefab;
    public GameObject Prefab => prefab;

    public GameObject Instantiate(Vector2 position, Quaternion rotation)
    {
        GameObject instance = Instantiate(prefab, position, rotation);
        instance.GetComponent<WeldingPart>().Data = this;
        return instance;
    }
}
