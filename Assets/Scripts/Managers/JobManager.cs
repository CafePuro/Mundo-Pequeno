using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JobManager : MonoBehaviour {

    Queue<Job> jobQueue;

    public static JobManager instance = null;
    void Awake()
    {
        if (instance == null)
        {
            //if not, set instance to this
            instance = this;
        }
        //If instance already exists and it's not this:
        else if (instance != this)
        {
            //Then destroy this. This enforces our singleton pattern, meaning there can only ever be one instance of a GameManager.
            Destroy(gameObject);
        }
        //Sets this to not be destroyed when reloading scene
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        jobQueue = new Queue<Job>();
    }

    public void JobEnqueue(Job j)
    {
        jobQueue.Enqueue(j);
    }

    public Job JobDequeue()
    {
        return jobQueue.Dequeue();
    }

    public int JobCount()
    {
        return jobQueue.Count;
    }


}
