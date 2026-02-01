using UnityEngine;

[CreateAssetMenu(fileName = "WeldingPartData", menuName = "Scriptable Objects/WeldingPartData")]
public class WeldingPartData : ScriptableObject
{
    [SerializeField]
    private GameObject prefab;
    [SerializeField]
    private Sprite uiSprite;

    public GameObject Prefab => prefab;
    public Sprite UISprite => uiSprite;

    public GameObject Instantiate(Vector2 position, Quaternion rotation, Transform parent)
    {
        GameObject instance = Instantiate(prefab, position, rotation);
        instance.transform.parent = parent;
        instance.GetComponent<WeldingPart>().Data = this;
        return instance;
    }
}
