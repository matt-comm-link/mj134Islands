using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public enum InventoryState 
{
    closed,
    open,
    sell
}


public class InventoryManager : MonoBehaviour
{
    public ShopManager shop;

    public List<InventoryItem> inventory = new List<InventoryItem>();
    public int Money;

    public InventoryState mode;

    public TextMeshProUGUI moneyCounter;

    [SerializeField]
    GameObject guiParent;


    [SerializeField]
    List<TextMeshProUGUI> iDescriptors = new List<TextMeshProUGUI>();
    [SerializeField]
    List<Image> iIcons = new List<Image>();


    [SerializeField]
    List<Transform> iSellButtons = new List<Transform>();
    [SerializeField]
    List<Transform> iSellStackButtons = new List<Transform>();

    [SerializeField]
    List<string> iconNames = new List<string>();
    [SerializeField]
    List<Sprite> iconSprites = new List<Sprite>();

    Dictionary<string, Sprite> icons = new Dictionary<string, Sprite>();

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < Mathf.Min(iconNames.Count, iconSprites.Count); i++)
        {
            icons.Add(iconNames[i], iconSprites[i]);
        }

        for (int i = 0; i < guiParent.transform.childCount; i++)
        {
            inventory.Add(new InventoryItem());
        }
    }

    // Update is called once per frame
    void Update()
    {
        moneyCounter.text = "£" + Money;

        if (shop.ShopOpened)
            mode = InventoryState.closed;

        //This is all for setting up the render/button access state of the inventory
        guiParent.SetActive(mode != InventoryState.closed);

        for (int i = 0; i < iDescriptors.Count; i++)
        {
            iDescriptors[i].enabled = inventory[i].Usable;
            iDescriptors[i].text = inventory[i].name;
            if (inventory[i].maxStack > 1)
                iDescriptors[i].text += "\n" + inventory[i].currentstack + "/" + inventory[i].maxStack;

            iIcons[i].enabled = inventory[i].Usable;
            if (icons.ContainsKey(inventory[i].name))
                iIcons[i].sprite = icons[inventory[i].name];
            else
                iIcons[i].sprite = iconSprites[0];
            if(mode == InventoryState.sell) 
            {
                iSellButtons[i].gameObject.SetActive(inventory[i].Usable);
                iSellButtons[i].GetComponentInChildren<TextMeshProUGUI>().text = "£" + inventory[i].basePrice;
                iSellStackButtons[i].gameObject.SetActive(inventory[i].Usable);
                iSellStackButtons[i].GetComponentInChildren<TextMeshProUGUI>().text = "£" + (inventory[i].basePrice * inventory[i].currentstack);

            }

        }

    }

    public bool AddItem(InventoryItem item) 
    {
        int firstunusued = inventory.Count;

        for (int i = 0; i < inventory.Count; i++)
        {
            if(inventory[i].name == item.name && inventory[i].currentstack < inventory[i].maxStack) 
            {
                int add = item.currentstack;
                if (add + inventory[i].currentstack > inventory[i].maxStack)
                    add = inventory[i].maxStack - inventory[i].currentstack;
                inventory[i].currentstack += add;
                item.currentstack -= add;
                if (item.currentstack <= 0)
                    return true;
                
            }

            if (firstunusued == inventory.Count && !inventory[i].Usable)
                firstunusued = i;
        }
        if(firstunusued != inventory.Count) 
        {
            inventory.Add(new InventoryItem(item));
            return true;
        }

        return false;
    }

    public InventoryItem GetItemAtIndex(int i) 
    {
        InventoryItem retItem = new InventoryItem();
        if(inventory[i].Usable && inventory[i].currentstack > 0)
        {
            retItem = new InventoryItem(inventory[i]);
            inventory[i] = new InventoryItem();
        }
        return retItem;
    }
    public void SellItemAtIndex(int i)
    {
       
        if (inventory[i].Usable && inventory[i].currentstack > 0)
        {
            inventory[i].currentstack--;
            Money += inventory[i].basePrice;
            if(inventory[i].currentstack == 0)
                inventory[i] = new InventoryItem();
            return;
        }
        return;
    }
    public void SellStackAtIndex(int i)
    {

        if (inventory[i].Usable && inventory[i].currentstack > 0)
        {
            Money += inventory[i].basePrice * inventory[i].currentstack;
            inventory[i] = new InventoryItem();
            return;
        }
        return;
    }
    public bool CheckItemAtIndex(int i)
    {
        if (inventory[i].Usable && inventory[i].currentstack > 0)
        {
            return true;
        }
        return false;
    }
    public InventoryItem GetItemByName(string n)
    {
        InventoryItem retItem = new InventoryItem();
        for (int i = 0; i < inventory.Count; i++)
        {
            if(inventory[i].Usable && inventory[i].name == n && inventory[i].currentstack > 0) 
            {
                retItem = new InventoryItem(inventory[i]);
                inventory[i] = new InventoryItem();
                break;
            }
        }
        return retItem;
    }
    public bool SellItemByName(string n)
    {
        for (int i = 0; i < inventory.Count; i++)
        {
            if (inventory[i].Usable && inventory[i].name == n && inventory[i].currentstack > 0)
            {
                inventory[i].currentstack--;
                Money += inventory[i].basePrice;
                if (inventory[i].currentstack == 0)
                    inventory[i] = new InventoryItem();
                return true;
            }
        }
        return false;
    }
    public bool SellStackByName(string n) 
    {
        for (int i = 0; i < inventory.Count; i++)
        {
            if (inventory[i].Usable && inventory[i].name == n && inventory[i].currentstack > 0)
            {
                Money += inventory[i].basePrice * inventory[i].currentstack;
                inventory[i] = new InventoryItem();
                return true;
            }
        }
        return false;
    }

    public bool CheckItemByName(string n)
    {
        for (int i = 0; i < inventory.Count; i++)
        {
            if (inventory[i].Usable && inventory[i].name == n && inventory[i].currentstack > 0)
            {
                return true;
            }
        }
        return false;
    }

    public void ToggleInventory() 
    {
        if (mode != InventoryState.closed)
            mode = InventoryState.closed;
        else
            mode = InventoryState.open;
    }
}


[System.Serializable]
public class InventoryItem 
{
    public bool Usable;
    public string name;
    public string conferTag;
    public int maxStack;
    public int currentstack;
    public int basePrice;

    public InventoryItem()
    {
        Usable = false;
        name = "";
        conferTag = "";
        maxStack = 0;
        currentstack = 0;
        basePrice = 0;
    }

    public InventoryItem(bool u, string n, string t, int m, int c, int b) 
    {
        Usable = u;
        name = n;
        conferTag = t;
        maxStack = m;
        currentstack = c;
        basePrice = b;
    }

    public InventoryItem(InventoryItem i)
    {
        Usable = i.Usable;
        name = i.name;
        conferTag = i.conferTag;
        maxStack = i.maxStack;
        currentstack = i.currentstack;
        basePrice = i.basePrice;
    }
}