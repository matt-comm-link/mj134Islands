using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public enum Location 
{
    Farm,
    TownSquare,
    TownShop,
    TownRopeShop,
    WizardShack,
    TurnipPath,
    TurnipFarm
}

public class VisualNovelNavigator : MonoBehaviour
{
    [SerializeField]
    Transform Map;

    [SerializeField]
    Image background;

    [SerializeField]
    TextMeshProUGUI localeName;


    [SerializeField]
    List<string> LocationNames = new List<string>();

    //0 Farm,
    //1 TownSquare
    //2 TownShop
    //3 TownRopeShop
    //4 WizardShack
    //5 TurnipPath
    //6 TurnipFarm
    public int location;

    [SerializeField]
    List<Transform> LocationMapMarkers = new List<Transform>();
    [SerializeField]
    List<Transform> LocationButtons = new List<Transform>();
    [SerializeField]
    List<Transform> LocationNavButtons = new List<Transform>();
    [SerializeField]
    List<Sprite> LocationBackgrounds = new List<Sprite>();


    private void Update()
    {

    }

    public void SwapLocation(int l) 
    {
        location = l;
        background.sprite = LocationBackgrounds[location];
        localeName.text = LocationNames[location];
        background.enabled = location != 0;
        Map.gameObject.SetActive(location != 0);
        for (int i = 0; i < LocationMapMarkers.Count; i++)
        {
            LocationMapMarkers[i].gameObject.SetActive(location == i);
            LocationButtons[i].gameObject.SetActive(location == i);
            LocationNavButtons[i].gameObject.SetActive(location == i);
            LocationMapMarkers[i].gameObject.SetActive(location == i);

        }
    }
}
