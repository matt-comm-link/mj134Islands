using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlacedCrop : MonoBehaviour
{

    public int inactiveLayer, activeLayer;

    CursorScript cs;

    TimeManager tm;

    ClamManager m;
    SpriteRenderer sr;
    Collider2D cl;

    public bool active;

    public int type; //RGBY

    [SerializeField]
    List<Sprite> aboveWaterSeedlings = new List<Sprite>();
    [SerializeField]
    List<Sprite> noWaterSeedlings = new List<Sprite>();
    [SerializeField]
    List<Sprite> aboveWaterGrown = new List<Sprite>();
    [SerializeField]
    List<Sprite> noWaterGrown = new List<Sprite>();

    [SerializeField]
    List<float> GrownThreshs;
    [SerializeField]
    List<float> PickThreshs;


    bool clam;

    public bool edibleAfterGrown;

    public float age;
    


    // Start is called before the first frame update
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        cl = GetComponent<Collider2D>();
        m = GameObject.Find("Clam Manager").GetComponent<ClamManager>();
        tm = GameObject.Find("OverlayManager").GetComponent<TimeManager>();

    }

    // Update is called once per frame
    void Update()
    {
        if (cl.OverlapPoint(Camera.main.ScreenToWorldPoint(Input.mousePosition)) && Input.GetKeyDown(KeyCode.Mouse0))
        {
            if (active)
                Pick();
            else
                Plant(cs.index);
        }
        if (tm.tideHigh) 
        {
            if(age > GrownThreshs[type]) 
                sr.sprite = aboveWaterGrown[type];
            else
                sr.sprite = aboveWaterSeedlings[type];
        }
        else
        {
            if (age > GrownThreshs[type])
                sr.sprite = noWaterGrown[type];
            else
                sr.sprite = noWaterSeedlings[type];
        }


        sr.enabled = active;
        cl.enabled = active;

        if (active)
            gameObject.layer = activeLayer;
        else
            gameObject.layer = inactiveLayer;

        if (tm.Paused || !active)
            return;

        float ageup = Time.deltaTime;
        if (tm.fastForward)
            ageup *= tm.fastForwardFactor;
        //If near clam, double rate
        if (m.CloseToClam(transform))
            ageup *= 2;
        //If clam no need to grow
        if (clam)
            ageup = 0;
        age += ageup;

    }


    public int Pick() 
    {
        if(!clam && active && age > PickThreshs[type]) 
        {
            active = false;
            age = 0;
            return type;
        }
        return -1;
    }
    public bool Eat()
    {
        if (active && age < GrownThreshs[type])
        {
            active = false;
            age = 0;
            if (type == 4)
                m.UnRegisterClam(transform);

            return true;
        }
        return false;
    }
    public bool Plant(int t) 
    {
        if(!active) 
        {
            type = t;
            active = true;

            if (type == 4)
                m.RegisterClam(transform);
        }
        return false;
    }

}
