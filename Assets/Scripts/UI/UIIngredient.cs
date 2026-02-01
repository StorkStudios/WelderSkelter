using UnityEngine;
using UnityEngine.UI;

public class UIIngredient : MonoBehaviour
{
    [SerializeField]
    private Image image;

    public void SetSprite(Sprite sprite, Color color)
    {
        image.sprite = sprite;
        image.color = color;
    }
}
