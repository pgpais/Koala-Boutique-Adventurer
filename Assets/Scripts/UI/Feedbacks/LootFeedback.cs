using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LootFeedback : MonoBehaviour
{
    [SerializeField] TMP_Text feedbackText;
    [SerializeField] Image feedbackImage;
    [SerializeField] float feedbackTime;
    [SerializeField] Transform panel;

    private void Awake()
    {
        panel.gameObject.SetActive(false);
        InventoryManager.ItemAdded.AddListener((itemName) =>
        {
            Item item = ItemManager.instance.itemsData.GetItemByName(itemName);

            Initialize(itemName, 1, item.sprite);
        });
    }

    public void Initialize(string itemName, int amount, Sprite itemSprite)
    {
        panel.gameObject.SetActive(true);
        feedbackText.text = $"You got {amount}x {itemName}!";
        feedbackImage.sprite = itemSprite;

        StartCoroutine(DisableAfterSeconds(feedbackTime));
    }

    private IEnumerator DisableAfterSeconds(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        panel.gameObject.SetActive(false);
    }
}
