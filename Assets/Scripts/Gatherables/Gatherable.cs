using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gatherable : MonoBehaviour
{
    private static int animIdleClipHash = Animator.StringToHash("Idle");
    private static int animInteractedClipHash = Animator.StringToHash("Interacted");

    private Item gatherableItem;

    #region Components
    private SpriteRenderer sprRen;
    private Animator anim;
    #endregion

    private int interactionsLeft = 3;

    private void Awake()
    {
        sprRen = GetComponentInChildren<SpriteRenderer>();
        anim = GetComponent<Animator>();
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    public void Init(Item gatherableItem)
    {
        this.gatherableItem = gatherableItem;
        sprRen.sprite = gatherableItem.sprite;
        sprRen.color = Color.white;
        interactionsLeft = gatherableItem.NumberOfInteractions;
        gameObject.name = this.gatherableItem.ItemNameKey;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Interact()
    {
        if (anim.GetCurrentAnimatorStateInfo(0).shortNameHash == animInteractedClipHash)
        {
            return;
        }

        Debug.Log("Interacted!");
        interactionsLeft--;
        if (interactionsLeft <= 0)
        {
            string itemName;

            if (gatherableItem == null)
            {
                itemName = "Basic Mushroom";
            }
            else
            {
                itemName = gatherableItem.ItemNameKey;
            }

            LogsManager.SendLogDirectly(new Log(
                LogType.MushroomCollected,
                new Dictionary<string, string> {
                    { "ItemName", itemName }
                }
            ));

            if (DiseasedManager.instance.DiseasedItem != null)
            {
                if (DiseasedManager.instance.DiseasedItem.ItemNameKey == itemName)
                {
                    LogsManager.SendLogDirectly(new Log(
                        LogType.DiseasedItemCollected,
                        new Dictionary<string, string> {
                        { "ItemName", itemName }
                        }
                    ));
                }
            }

            InventoryManager.instance.AddItem(itemName, 1);
            Debug.Log("Collected gatherable!");
            Destroy(gameObject);
        }
        else
        {
            Debug.Log($"Got interacted once! {interactionsLeft} interactions left");
            anim.SetTrigger(animInteractedClipHash);
        }
    }
}
