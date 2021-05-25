using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gatherable : MonoBehaviour
{
    private Item gatherableItem;

    #region Components
    private SpriteRenderer sprRen;
    private Animator anim;
    #endregion

    private int interactionsLeft = 5;

    private void Awake()
    {
        sprRen = GetComponentInChildren<SpriteRenderer>();
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    public void Init(Item gatherableItem)
    {
        this.gatherableItem = gatherableItem;
        sprRen.sprite = gatherableItem.image;
        sprRen.color = Color.white;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Interact()
    {
        Debug.Log("Interacted!");
        interactionsLeft--;
        if (interactionsLeft <= 0)
        {
            string itemName;
            if (gatherableItem == null)
            {
                itemName = "Gatherable1";
            }
            else
            {
                itemName = gatherableItem.ItemName;
            }
            InventoryManager.instance.AddItem(itemName, 1);
            Debug.Log("Collected gatherable!");
            Destroy(gameObject);
        }
        else
        {
            Debug.Log($"Got interacted once! {interactionsLeft} interactions left");
        }
    }
}
