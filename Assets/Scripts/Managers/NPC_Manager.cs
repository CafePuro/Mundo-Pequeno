using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC_Manager : MonoBehaviour
{

    public delegate void NPCSpawned();
    public static event NPCSpawned OnNPCSpawned;


    public NPC[] npcs;
    public GameObject spawn;

    public void SpawnNPC(int index)
    {
        Instantiate(npcs[index].npc, spawn.transform.position, spawn.transform.rotation);
        if (OnNPCSpawned != null)
            OnNPCSpawned();
    }



}


[System.Serializable]
public struct NPC
{
    public string name;
    public int value;
    public GameObject npc;
    
}
