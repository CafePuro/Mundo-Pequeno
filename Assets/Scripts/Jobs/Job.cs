/*
 * Created on Mon Oct 08 2018 by ZZZeMarcelo
 *
 * Copyright (c) 2018 Café Puro Digital Studio
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Job
{

    public Tile Tile {get; protected set;}
    public GameObject placer;
    public GameObject build;
    float jobTime;

    Action<Job> cbJobComplete;
    Action<Job> cbJobCancel;

    public Job(Tile _tile, GameObject _placer, GameObject _build, Action<Job> cbJobComplete,  float JobTime = 1f)
    {
        this.Tile = _tile;
        this.cbJobComplete += cbJobComplete;
        this.placer = _placer;
        this.build = _build;
    }

    public void RegisterJobCompleteCallBack(Action<Job> cb)
    {
        this.cbJobComplete += cb;
    }

    public void RegisterJobCancelCallBack(Action<Job> cb)
    {
        this.cbJobCancel += cb;
    }

    public void DoWork(float workTime)
    {
        Debug.Log("Do Work! " + workTime);
        jobTime -= workTime;
        if(jobTime <= 0)
        {
            if(cbJobComplete != null)
                cbJobComplete(this);
        }
    }

    public void CancelJob()
    {
        if(cbJobCancel != null)
            cbJobCancel(this);
    }
    public void DestroyPlacer()
    {
        GameObject.Destroy(placer);
    }

}
