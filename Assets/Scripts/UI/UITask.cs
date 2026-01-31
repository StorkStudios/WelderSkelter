using UnityEngine;
using TMPro;

public class UITask : MonoBehaviour
{
    [SerializeField]
    private UIIngredient ingredientPrefab;
    [SerializeField]
    private CanvasGroup group;
    [SerializeField]
    private TextMeshProUGUI title;
    [SerializeField]
    private TextMeshProUGUI money;
    [SerializeField]
    private TextMeshProUGUI upgrade;
    [SerializeField]
    private RectTransform ingredientsParent;

    private Task task;

    public void SetTask(Task task)
    {
        if (this.task == task)
        {
            return;
        }

        this.task = task;

        if (task == null)
        {
            group.alpha = 0;
            return;
        }

        group.alpha = 1;
        title.text = task.Title;
        foreach (Transform ingredient in ingredientsParent)
        {
            Destroy(ingredient.gameObject);
        }
        foreach (WeldingPartData part in task.RequiredParts)
        {
            Instantiate(ingredientPrefab.gameObject, ingredientsParent).GetComponent<UIIngredient>().SetSprite(part.UISprite);
        }

        money.gameObject.SetActive(false);
        upgrade.gameObject.SetActive(false);
        switch (task)
        {
            case MoneyTask moneyTask:
                money.text = $"{moneyTask.Money}zł";
                money.gameObject.SetActive(true);
                break;
            case UpgradeTask upgradeTask:
                upgrade.text = upgradeTask.Item.Title;
                upgrade.gameObject.SetActive(true);
                break;
        }
    }
}
