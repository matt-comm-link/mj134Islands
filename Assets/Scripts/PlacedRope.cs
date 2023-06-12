using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlacedRope : MonoBehaviour
{

    TimeManager tm;
    InventoryManager im;

    Collider2D cl;

    [SerializeField]
    List<SpriteRenderer> rope = new List<SpriteRenderer>();



    [SerializeField]
    SpriteRenderer[] poles;

    [SerializeField]
    SpriteRenderer Bell;

    [SerializeField]
    Sprite hightidepole, lowtidepole;

    public bool built;
    public bool belled;

    // Start is called before the first frame update
    void Start()
    {
        im = GameObject.Find("InventoryScript").GetComponent<InventoryManager>();
        tm = GameObject.Find("OverlayManager").GetComponent<TimeManager>();
        cl = GetComponent<Collider2D>();

        poles = new SpriteRenderer[2];
        poles[0] = transform.Find("Square").GetComponent<SpriteRenderer>();
        poles[1] = transform.Find("Square (5)").GetComponent<SpriteRenderer>();

        for (int i = 0; i < transform.childCount; i++)
        {
            rope.Add(transform.GetChild(i).GetComponent<SpriteRenderer>());
        }
        Bell = rope[rope.Count - 1];
    }

    // Update is called once per frame
    void Update()
    {
        cl.enabled = built;

        for (int i = 0; i < rope.Count; i++)
        {
            rope[i].enabled = built;
        }
        poles[0].enabled = built;
        poles[1].enabled = built;
        Bell.enabled = belled;

        if (tm.tideHigh) 
        {
            poles[0].sprite = hightidepole;
            poles[1].sprite = hightidepole;
        }
        else 
        {
            poles[0].sprite = lowtidepole;
            poles[1].sprite = lowtidepole;
        }


    }

    public void Build() 
    {
        if (im.GetItemByName("Rope").Usable)
            built = true;
    }

    public void Upgrade()
    {
        if (im.GetItemByName("Bell").Usable)
            built = true;
    }
}
