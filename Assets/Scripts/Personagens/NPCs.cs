//NPC
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class NPCs : MonoBehaviour
{
    //posicao do NPC no mapa
    Mapa mapa = Mapa.instance; //referencia ao mapa
    public Tile Tile { get; protected set;}

    //Movimento
    public float speed = 5.0f;
    public float accuracy = .1f;
    Vector3 moveAmount;
    Vector3 smoothVelocity;
    Rigidbody NPCrigidbody;
    bool aCaminho = false;

    //Navegaçao
    Tile targetTile;
    int targetIndex;
    Tile[] path;

    //Se o NPC nao tem um job essa variavel é null
    Job jobOnPlayer = null;

    //Desenha os gizmos n editor?
    public bool desenhaGizmoz = true;

	void Start()
	{
        NPCrigidbody = GetComponent<Rigidbody>();
        Tile = mapa.GetTile(transform);
    }

    void Update()
    {
        if (jobOnPlayer == null)
            jobOnPlayer = GetAJob();
        else
            {
                if(!aCaminho && Tile != jobOnPlayer.Tile)
                {
                    jobOnPlayer.RegisterJobCompleteCallBack(OnJobEnded);
                    jobOnPlayer.RegisterJobCancelCallBack(OnJobEnded);
                    SetNPCDestination(jobOnPlayer.Tile);
                    aCaminho = true;
                }
                if (Tile == jobOnPlayer.Tile)
                {
                    aCaminho = false;
                    jobOnPlayer.DoWork(Time.deltaTime);
                }
            }
     }

    Job GetAJob()
    {
        if(jobOnPlayer == null && JobManager.instance.JobCount() > 0)
        {
            return JobManager.instance.JobDequeue();
        }
        return null;
    }

    public void SetNPCDestination(Tile targetTile)
    {
        PathRequestManager.RequestPath(Tile, targetTile, OnPathFound);
    }

    public void OnPathFound(Tile[] newPath, bool pathSuccessful)
    {
        if(pathSuccessful)
        {
            Debug.Log("Tentando seguir o caminho");
            path = newPath;
            targetIndex = 0;
            StopCoroutine("FollowPath");
            StartCoroutine("FollowPath");
        }
    }


    public IEnumerator FollowPath()
    {
        Debug.Log("Seguindo o caminho");
        Vector3 currentWaypoint = path[0].GetTileCenter();
        Debug.Log("Path Size: " + path.Length);
        while (true)
        {
            if ((transform.position - currentWaypoint).magnitude <= accuracy)
            {
                targetIndex++;
                if (targetIndex == path.Length)
                {
                    Debug.Log("Cheguei!");
                    Tile = mapa.GetTile(transform);
                    yield break;
                }
                currentWaypoint = path[targetIndex].GetTileCenter();
            }
            Vector3 direcao = (currentWaypoint - transform.position).normalized;

            //direcao do waypoint
            Debug.DrawLine(transform.position, direcao, Color.blue);

            direcao = new Vector3(direcao.x, 0, direcao.z);
            Vector3 targetMoveAmount = direcao * speed;

            //desenha a direção do movimento no editor
            Debug.DrawLine(transform.position, direcao, Color.red);
            
            moveAmount = Vector3.SmoothDamp(moveAmount, targetMoveAmount, ref smoothVelocity, .15f);
            NPCrigidbody.MovePosition(NPCrigidbody.position + transform.TransformDirection(moveAmount) * Time.fixedDeltaTime);
            Tile = mapa.GetTile(transform);
            yield return null;
        }
    }

    void OnJobEnded(Job j)
    {
        if(j != jobOnPlayer)
        {
            Debug.LogError("Job não pertence a este personagem.");
            return;
        }
        Debug.Log("NPC " + gameObject.name + " Acabou o Job " + j);
        jobOnPlayer = null;
    }


    public void OnDrawGizmos()
    {
        if(desenhaGizmoz)
            if (path != null)
                for (int i = targetIndex; i < path.Length; i++)
                {
                    Gizmos.color = Color.black;
                    Gizmos.DrawCube(path[i].GetTileCenter(), Vector3.one);
                    if (i == targetIndex)
                        Gizmos.DrawLine(transform.position, path[i].GetTileCenter());
                    else
                        Gizmos.DrawLine(path[i - 1].GetTileCenter(), path[i].GetTileCenter());
                }
    }
}
