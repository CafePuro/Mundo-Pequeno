/*
 * Created on Sun Sep 16 2018 by ZZZeMarcelo
 *
 * Copyright (c) 2018 Café Puro Digital Studio
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public Camera camera;
    Mapa mapa;

    //Transform objectToPlace;
    Buildings[] buildingObjects;
    GameObject placer;
    GameObject buildObj;
    Transform centroDoPlaneta;
    bool build = false;

    JobManager jm;

    //Menus
    public Image[] menus;

    //Clock
    public Text minute;
    public Text hour;
    public Text day;
    private const int TICKS_MAX_FOR_MINUTE = 50;
    int ticks = 0;
    int minutes = 0;
    int hours = 0;
    int days = 0;
 
    //Declaracao de Delegates
    public delegate void SelectTile(Transform transform, int index);
    public static event SelectTile OnClickTile;
   
    void Start()
    {
        buildingObjects = GetComponent<Building>().buildings;
        centroDoPlaneta = GameObject.FindGameObjectWithTag("Planeta").transform;
        jm = JobManager.instance;
        mapa = Mapa.instance;
    }

    void OnEnable()
    {
        NPC_Manager.OnNPCSpawned += DisableSpawnMenu;
        TimeTickSystem.OnTick += Clock;
    }

    void OnDisable()
    {
        NPC_Manager.OnNPCSpawned += DisableSpawnMenu;
        TimeTickSystem.OnTick -= Clock;
    }

    // Update is called once per frame
    void Update ()
    {

        if (Input.GetKeyDown(KeyCode.Tab))
            camera.GetComponent<MoveCamera>().podeNavegar = !camera.GetComponent<MoveCamera>().podeNavegar;

        Vector3 entradaDoTeclado = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
        float rot = entradaDoTeclado.x;

        RaycastHit hit;
        Ray ray = camera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit, 100.0f))
        {
            if (build)
            {
                if (hit.transform.gameObject.layer == 20)
                {
                    placer.transform.position = (hit.point - centroDoPlaneta.position) * 1.001f;
                    placer.transform.rotation = Quaternion.FromToRotation(Vector3.up, hit.normal);
                }
            }

            if(Input.GetMouseButtonDown(1))
            {

            }

            if (Input.GetMouseButtonDown(0))
            {
                if(build)
                {
                    build = false;
                    Job j = new Job(mapa.GetTile(placer.transform.GetChild(0)), placer, buildObj, (TheJob) => { OnBuildMachineComplete(TheJob);}, 10);
                    jm.JobEnqueue(j);
                    Debug.Log("Job Queue Size: " + jm.JobCount());
                }
            }
		}
    }

    //Atualiza o Relogio da Interface
    void Clock(object sender, TimeTickSystem.OnTickEventArgs e)
    {
        ticks++;
        if(ticks >= TICKS_MAX_FOR_MINUTE)
        {
            ticks = 0;
            minutes++;
        }
        if(minutes >= 60)
        {
            minutes = 0;
            hours++;
        }
        if (hours >= 20)
        {
            hours = 0;
            days++;
        }

        minute.text = minutes < 10 ? "0" + minutes.ToString() : minutes.ToString();
        hour.text = hours < 10 ? "0" + hours.ToString() : hours.ToString();
        day.text = days.ToString();
    }



    void OnBuildMachineComplete(Job j)
    {
        Instantiate(j.build, j.placer.transform.position, placer.transform.rotation);
        mapa.ConstroiMapa();
        j.DestroyPlacer();
    }

    public void EnabledMenu(int index)
    {
        menus[index].gameObject.SetActive(!menus[index].gameObject.activeSelf);
    }

    void DisableSpawnMenu()
    {
        DisableMenu(0);
    }

    public void DisableMenu(int index)
    {
        menus[index].gameObject.SetActive(false);
    }
    
    public void BuildMachine(int index)
    {
        build = true;
        buildObj = buildingObjects[index].building;
        placer = (GameObject)Instantiate(buildingObjects[index].placer, Vector3.zero, Quaternion.identity);
        DisableMenu(1);
     }
}
