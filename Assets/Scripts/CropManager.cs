using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
public class CropManager : MonoBehaviour
{
    public List<Sprite> HighTideSprites = new List<Sprite>();
    public List<Sprite> LowTideSprites = new List<Sprite>();


    public static CropManager current;





}

public struct Bell
{
    public Transform Start;
    public Transform End;


}

public struct Rope
{
    public Transform Start;
    public Transform End;

}
