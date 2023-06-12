using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CursorScript : MonoBehaviour
{
    InventoryManager im;

    bool placemode;

    [SerializeField]
    Vector3 offset;

    public int index;

    [SerializeField]
    Image showPlacing;
    [SerializeField]
    Canvas gui;

    [SerializeField]
    List<string> placableNames = new List<string>();
    [SerializeField]
    List<Sprite> placableSprites = new List<Sprite>();

    [SerializeField]
    List<int> placableCropIndices = new List<int>();

    List<string> actualPlacables = new List<string>();
    List<Sprite> actualSprites = new List<Sprite>();
    List<int> actualCropIndices = new List<int>();

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        showPlacing.transform.position = Input.mousePosition + offset;
    }

    public void Refresh() 
    {
        actualPlacables.Clear();
        actualSprites.Clear();
        actualCropIndices.Clear();
        for (int i = 0; i < placableNames.Count; i++)
        {
            if (im.CheckItemByName(placableNames[i])) 
            { 
                actualPlacables.Add(placableNames[i]);
                actualSprites.Add(placableSprites[i]);
                actualCropIndices.Add(placableCropIndices[i]);

            }
        }
        if (actualPlacables.Count == 0)
            showPlacing.enabled = false;
        else
        {
            index = Mathf.Clamp(index, 0, actualPlacables.Count);
            showPlacing.sprite = actualSprites[index];
        }
    }

    public void TryClick() 
    {
        if (actualPlacables.Count == 0)
            return;
        RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), new Vector2(0, 0.1f));
        if (hit.collider == null)
            return;
        string handle = hit.transform.tag;

        switch (handle) 
        {
            case "ropespot":
                PlacedRope rp = hit.transform.GetComponent<PlacedRope>();
                if (!rp.built && actualPlacables[index] == "Rope")
                    rp.Build();
                if (!rp.belled && actualPlacables[index] == "Bell")
                    rp.Upgrade();
                break;
            case "cropspot":
                PlacedCrop pc = hit.transform.GetComponent<PlacedCrop>();
                if (pc.active)
                    pc.Pick();
                else
                    pc.Plant(index);
                break;
        }
    }
}
