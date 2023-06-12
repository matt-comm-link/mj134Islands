using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClamManager : MonoBehaviour
{
    [SerializeField]
    float clamrange;

    List<Transform> clams = new List<Transform>();

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public bool CloseToClam(Transform p) 
    {
        for (int i = 0; i < clams.Count; i++)
        {
            if (Vector3.Distance(p.position, clams[i].position) < clamrange)
                return true;
        }

        return false;
    }

    public void RegisterClam(Transform t) 
    {
        clams.Add(t);
    }
    public void UnRegisterClam(Transform t)
    {
        clams.Remove(t);
    }
}
