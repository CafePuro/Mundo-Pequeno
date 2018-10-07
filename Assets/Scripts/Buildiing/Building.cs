using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : MonoBehaviour {

    Mapa mapa;

    public Buildings[] buildings;

}
[System.Serializable]
public struct Buildings
{
    public string name;
    public GameObject building;
    public GameObject placer;

}
