using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    [SerializeField]
    GameObject DiaParent;


    [SerializeField]
    TextMeshProUGUI diabox;
    [SerializeField]
    Image lPortrait, rPortrait;

    [SerializeField]
    List<Transform> buttons = new List<Transform>();
    [SerializeField]
    List<string> buttonTags = new List<string>();

    [SerializeField]
    List<Sprite> actors = new List<Sprite>();

    public List<string> CurrentConversation = new List<string>();
    public List<string> Tags = new List<string>();
    public int place;

    public bool DiaOpen;

    public bool Blocked;
    //use time.deltatime
    [SerializeField]
    float cooldownTime;
    public float countdownTime;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (countdownTime > 0)
            countdownTime -= Time.deltaTime;

        DiaOpen = DiaParent.activeInHierarchy;
    }


    public bool StartDialogue(List<string> Convo)
    {
        if (Blocked)
            return false;
        CurrentConversation = Convo;
        place = -1;
        if (UpdateDialogue())
            return true;
        DiaParent.SetActive(false);
        return false;
    }

    public bool PushDialogue() 
    {
        if (UpdateDialogue())
            return true;
        DiaParent.SetActive(false);
        return false;
    }

    public void ButtonPushDialogue()
    {
        if (UpdateDialogue())
            return;
        DiaParent.SetActive(false);
        return;
    }

    bool UpdateDialogue()
    {
        if (Blocked || cooldownTime > 0)
            return false;

        //check to see there's something left to say
        place++;
        if (place >= CurrentConversation.Count)
        {
            return false; //Have this call something to close the dialogue too
        }
        //reserved
        string[] tagreqs;
        string[] splitted;
        //info extracted
        string said = "";
        int side = 0;
        int portrait = 0;
        int foundDiaPlace = place;
        //confirmation
        bool foundDia = false;
        //check to make sure there's something left to say that isn't tag locked
        for (int i = place; i < CurrentConversation.Count; i++)
        {
            splitted = CurrentConversation[i].Split('|');
            said = splitted[3];
            portrait = int.Parse(splitted[2]);
            side = int.Parse(splitted[1]);
            tagreqs = splitted[0].Split(',');
            for (int j = 0; j < tagreqs.Length; j++)
            {
                if (!Tags.Contains(tagreqs[j]))
                {
                    continue;
                }
            }
            foundDiaPlace = i;
            foundDia = true;
            break;
        }
        if (!foundDia)
        {
            return false;
        }
        place = foundDiaPlace;
        DiaParent.SetActive(true);
        if (side > 0)
        {
            rPortrait.enabled = true;
            rPortrait.sprite = actors[portrait];
            lPortrait.enabled = false;
        }
        else
        {
            lPortrait.enabled = true;
            lPortrait.sprite = actors[portrait];
            rPortrait.enabled = false;
        }
        string[] options = said.Split(':');
        if (options[0] == "OPTIONS")
            RenderOptions(options);
        else 
        {
            for (int i = 0; i < buttons.Count; i++)
            {
                buttons[i].gameObject.SetActive(false);
            }
            diabox.enabled = true;
            diabox.text = said;
        }
        countdownTime = cooldownTime;
        return true;
    }


    public void AddTag(string tag)
    {
        if (!Tags.Contains(tag))
            Tags.Add("tag");
    }

    //Give you option buttons, set what tags they offer
    void RenderOptions (string[] options)
    {
        Blocked = true;
        string[] actualoptions = new string[options.Length - 1];
        diabox.enabled = false;
        for (int i = 0; i < actualoptions.Length; i++)
        {
            actualoptions[i] = options[i + 1];
        }

        for (int i = 0; i < actualoptions.Length; i++)
        {
            buttons[i].gameObject.SetActive(true);
            buttons[i].GetComponentInChildren<TextMeshProUGUI>().text = actualoptions[i].Split(',')[0];
            buttonTags[i] = actualoptions[i].Split(',')[1];
        }
    }
    public void PressButton(int index) 
    {
        if (Blocked || countdownTime > 0)
            return;

        AddTag(buttonTags[index]);
        Blocked = false;
        PushDialogue();
    }

}
