using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PathRequestManager : MonoBehaviour
{

    Queue<PathRequest> pathRequestQueue = new Queue<PathRequest>();
    PathRequest currentPathRequest;

    static PathRequestManager instance;
    Navegacao navegacao;

    bool isProcessingPath;

    void Awake()
    {
        instance = this;
        navegacao = GetComponent<Navegacao>();
    }

    public static void RequestPath(Tile pathStart, Tile pathEnd, Action<Tile[], bool> callback)
    {
        Debug.Log("New Request");
        Debug.Log("Path Start: " + pathStart.x + "," + pathStart.y);
        Debug.Log("Path End: " + pathEnd.x + "," + pathEnd.y);
        PathRequest newRequest = new PathRequest(pathStart, pathEnd, callback);
        instance.pathRequestQueue.Enqueue(newRequest);
        instance.TryProcessNext();
    }

    void TryProcessNext()
    {
        if(!isProcessingPath && pathRequestQueue.Count > 0)
        {
            currentPathRequest = pathRequestQueue.Dequeue();
            isProcessingPath = true;
            navegacao.StartFindPath(currentPathRequest.pathStart, currentPathRequest.pathEnd);
        }
    }

    public void FinishedProcessingPath(Tile[] path, bool success)
    {
        currentPathRequest.callback(path, success);
        isProcessingPath = false;
        TryProcessNext();
    }

    struct PathRequest
    {
        public Tile pathStart;
        public Tile pathEnd;
        public Action<Tile[], bool> callback;

        public PathRequest(Tile _start, Tile _end, Action<Tile[], bool> _callback)
        {
            pathStart = _start;
            pathEnd = _end;
            callback = _callback;
        }
    }
}
