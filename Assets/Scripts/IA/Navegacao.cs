using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Navegacao : MonoBehaviour
{
    Mapa mapa;
    PathRequestManager requestManager;

    void Start()
    {
        mapa = Mapa.instance;
        requestManager = GetComponent<PathRequestManager>();
    }

    public void StartFindPath(Tile startPos, Tile targetPos)
    {
        StartCoroutine(FindPath(startPos, targetPos));
    }

    IEnumerator FindPath(Tile startTile, Tile targetTile)
    {
        Tile[] waypoints = new Tile[0];
        bool pathSuccess = false;


        if (startTile.IsAndavel() && targetTile.IsAndavel())
        {
            Debug.Log("Calculando caminho!");
            Heap<Tile> openSet = new Heap<Tile>(mapa.MaxSize);
            HashSet<Tile> ClosedSet = new HashSet<Tile>();
            openSet.Add(startTile);

            while (openSet.Count > 0)
            {
                Tile currentTile = openSet.RemoveFirst();

                ClosedSet.Add(currentTile);

                if (currentTile == targetTile)
                {
                    pathSuccess = true;
                    break;
                }

                foreach (Tile neighbour in mapa.GetNeigbours(currentTile))
                {
                    if (!neighbour.IsAndavel() || ClosedSet.Contains(neighbour)) continue;
                    int newMovementCostToNeighbour = currentTile.gCost + GetDistance(currentTile, neighbour) + neighbour.data.pesoDeNavegacao;
                    if (newMovementCostToNeighbour < neighbour.gCost || !openSet.Contains(neighbour))
                    {
                        neighbour.gCost = newMovementCostToNeighbour;
                        neighbour.hCost = GetDistance(neighbour, targetTile);
                        neighbour.parent = currentTile;

                        if (!openSet.Contains(neighbour))
                            openSet.Add(neighbour);
                        else
                            openSet.UpdateItem(neighbour);

                    }
                }
            }
        }
        yield return null;
        if (pathSuccess)
        {
            Debug.Log("Caminho Encontrado!");
            waypoints = RetracePath(startTile, targetTile);
        }
        requestManager.FinishedProcessingPath(waypoints, pathSuccess);
    }
    
    Tile[] RetracePath(Tile startTile, Tile endTile)
    {
        List<Tile> path = new List<Tile>();
        Tile currentTile = endTile;
        while(currentTile != startTile)
        {
            path.Add(currentTile);
            currentTile = currentTile.parent;
        }
        Tile[] waypoints = SimplifyPath(path);
        Array.Reverse(waypoints);
        return waypoints; 
    }

    Tile[] SimplifyPath(List<Tile> path)
    {
        List<Tile> waypoints = new List<Tile>();
        Vector2 directionOld = Vector2.zero;
        
        waypoints.Add(path[0]);
        for(int i = 1; i < path.Count; i++)
        {
            Vector2 directionNew = new Vector2(path[i - 1].x - path[i].x, path[i - 1].y - path[i].y);
            if(directionNew != directionOld)
            {
                waypoints.Add(path[i]);
            }
            directionOld = directionNew;
        }
        return waypoints.ToArray();
    }


    int GetDistance(Tile tileA, Tile tileB)
    {
        int dstX = Mathf.Abs(tileA.x - tileB.x);
        int dstY = Mathf.Abs(tileA.y - tileB.y);

        if (dstX > dstY) return 14 * dstY + 10 * (dstX - dstY);
        return 14 * dstX + 10 * (dstY - dstX);
    }
}


