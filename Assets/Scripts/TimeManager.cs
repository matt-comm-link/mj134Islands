using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TimeManager : MonoBehaviour
{
    public float fastForwardFactor;
    [SerializeField]
    float timeBaseSpeedFactor = 1;

    public bool fastForward;
    public bool Paused;

    string[] daytag = { "Mon", "Tues", "Wed", "Thurs", "Fri", "Sat", "Sun" };

    float time;

    public bool tideHigh;

    public int day;

    public string timeHours;
    public string timeMinutes;
    public string timeHoursTilTide;
    public string timeMinutesTilTide;

    [SerializeField]
    TextMeshProUGUI timeHole, tideHole, dayHole, tideType;
    [SerializeField]
    Image tideBG;
    public float nextTide;

    string ffGraphic = ">>";
    string rGraphic = ">";
    string pGraphic = "||";


    [SerializeField]
    Transform WaterLayer, htlayer;


    [SerializeField]
    TextMeshProUGUI ffButt;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Paused) 
        {
            fastForward = false;
            ffButt.text = pGraphic;
            ffButt.color = Color.red;
            return;
        }
        float timestep = Time.deltaTime * timeBaseSpeedFactor;
        if (fastForward) 
        {
            ffButt.text = rGraphic;
            ffButt.color = Color.cyan;
            timestep *= fastForwardFactor;
        }
        else 
        {
            ffButt.text = ffGraphic;
            ffButt.color = Color.green;
        }
        time += timestep;

        if (time >= nextTide)
            CalcNextTide();

        if (tideHigh) 
        {
            tideType.text = "HIGH";
            tideBG.color = Color.cyan;
        }
        else 
        {
            tideType.text = "LOW";
            tideBG.color = Color.magenta;
        }
        timeMinutesTilTide = ((int)(nextTide - time) % 60).ToString();
        timeHoursTilTide = (((int)(nextTide - time)/ 60) % 24).ToString();

        if (timeMinutesTilTide.Length < 2)
            timeMinutesTilTide = "0" + timeMinutesTilTide;
        if (timeHoursTilTide.Length < 2)
            timeHoursTilTide = "0" + timeHoursTilTide;


        timeMinutes = ((int)time % 60).ToString();
        timeHours = (((int)time / 60) % 24).ToString();

        if (timeMinutes.Length < 2)
            timeMinutes = "0" + timeMinutes;
        if (timeHours.Length < 2)
            timeHours = "0" + timeHours;

        day = (int)time / (60 * 24);

        dayHole.text = daytag[day % 7];
        timeHole.text = timeHours + ":" + timeMinutes;
        tideHole.text = timeHoursTilTide + ":" + timeMinutesTilTide + " Til next Tide";
    }

    void CalcNextTide() 
    {
        fastForward = false;
        tideHigh = !tideHigh;
        WaterLayer.gameObject.SetActive(tideHigh);
        htlayer.gameObject.SetActive(tideHigh);
        nextTide = time + 660;
    }

    public void FastForward() 
    {
        if(!Paused)
            fastForward = !fastForward;

    }


}
