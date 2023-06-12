using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public enum Shoptype 
{
    Seed,
    Rope,
    Science
}

public class ShopManager : MonoBehaviour
{
    public bool ShopOpened;
    public Shoptype mode;


    public int unlocked;

    public int unlockedSeed;
    public int unlockedRope;
    public int unlockedScience;

    public List<InventoryItem> inventory = new List<InventoryItem>();
    public List<InventoryItem> seedShopinventory = new List<InventoryItem>();
    public List<InventoryItem> ropeShopinventory = new List<InventoryItem>();
    public List<InventoryItem> scienceShopinventory = new List<InventoryItem>();


    [SerializeField]
    GameObject guiParent;

    [SerializeField]
    List<TextMeshProUGUI> iDescriptors = new List<TextMeshProUGUI>();
    [SerializeField]
    List<Image> iIcons = new List<Image>();


    [SerializeField]
    List<Transform> iBuyButtons = new List<Transform>();
    [SerializeField]
    List<Transform> iBuyStackButtons = new List<Transform>();

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

        for (int i = 0; i < 16; i++)
        {
            inventory.Add(new InventoryItem());
        }
    }

    // Update is called once per frame
    void Update()
    {


        guiParent.SetActive(ShopOpened);

        for (int i = 0; i < Mathf.Min(unlocked, iDescriptors.Count); i++)
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
            iBuyButtons[i].gameObject.SetActive(inventory[i].Usable);
            iBuyButtons[i].GetComponentInChildren<TextMeshProUGUI>().text = "£" + inventory[i].basePrice;
            iBuyStackButtons[i].gameObject.SetActive(inventory[i].Usable && inventory[i].maxStack > 1);
            iBuyStackButtons[i].GetComponentInChildren<TextMeshProUGUI>().text = "£" + (inventory[i].basePrice * inventory[i].currentstack);

        }
    }

    //Add in method to procedurally generate new stock each morning

    public void OpenShop(Shoptype m) 
    {
        mode = m;
        switch (mode) 
        {
            case Shoptype.Seed:
                unlocked = unlockedSeed;
                inventory = seedShopinventory;
                break;
            case Shoptype.Rope:
                unlocked = unlockedRope;
                inventory = ropeShopinventory;
                break;
            case Shoptype.Science:
                unlocked = unlockedScience;
                inventory = scienceShopinventory;
                break;
        }
        ShopOpened = true;
    }


    //Add in methods to buy stuff with money
}
