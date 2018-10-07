/*
 * Created on Fri Sep 07 2018 by ZZZeMarcelo
 *
 * Copyright (c) 2018 Café Puro Digital Studio
 * Esse Script monta o mapa de navegação baseado nos vertices e triangulos para montar os 
 *  quads ou tiles navegáveis
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Monta um mapa das posições navegaveis e construiveis

public class Mapa : MonoBehaviour
{
    float raioDeColisao;
    public bool drawGizmos = false;

	//Referencia ao planeta e as malhas que o compoem.
	//O numero de malhas do planeta comporá  número de
	//folhas (sheets) do mapa
	Planeta planeta;
    Transform centroDoPlaneta;
	MeshFilter[] malhas;

    //Mascara para o terreno não andavel
    public LayerMask mascaraNaoAndavel;

	//Folhas do mapa
	Sheet[] sheets;

    //Montando a tabela de tipos de terreno e pesos
    public PesosDeMovimento[] pesosDeMovimento;

    //Desenha os gizmos n editor?
    public bool desenhaGizmoz = true;

    //armazena a resolucao das partes do planeta
    int res;

    //instancia do mapa acessivel ao jogo todo
    public static Mapa instance = null; 
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
        centroDoPlaneta = GameObject.FindGameObjectWithTag("Planeta").GetComponent<Transform>();
       // planeta = centroDoPlaneta .GameObject.FindGameObjectWithTag("Planeta").GetComponent<Planeta>();
        planeta = centroDoPlaneta.gameObject.GetComponent<Planeta>();
    }

	void Start()
	{
        ConstroiMapa();
	}

	void OnEnable()
	{
		Planeta.OnPlanetaPronto += ConstroiMapa;
	}

	void OnDisable()
	{
		Planeta.OnPlanetaPronto += ConstroiMapa;
	}

    public int MaxSize
    {
        get
        {
            return (res * res);
        }
    }

	public Tile GetTileAt(int sheet, int x, int y)
	{
		if( sheet < 0 || sheet > sheets.Length || x < 0 || x > res || y < 0 || y > res)
		{
			Debug.Log("Tile out of Range.");
		}
		for(int i = 0; i < sheets[sheet].tiles.Length; i++)
		{
			if(sheets[sheet].tiles[i].x == x && sheets[sheet].tiles[i].y == y)
			{
				return sheets[sheet].tiles[i];
			}
		}
		return null;
	}

    public Tile GetTile(Transform t)
    {
        Tile tile = null;
        RaycastHit hit;
        if (Physics.Raycast(t.position, (centroDoPlaneta.position - t.position).normalized, out hit, 1f))
        {
            tile = GetTile(GetSheet(hit.transform.gameObject.name), hit.triangleIndex);
        }
        return tile;
    }

    public Tile GetTile(int sheet, int triIndex)
    {
        return sheets[sheet].tiles[triIndex / 2];
    }

    int GetSheet(string name)
    {
        return int.Parse(name.Substring(5, 1));
    }

    public void ConstroiMapa()
	{
        Debug.Log("Mapeando!!");
        malhas = planeta.meshFilters;
		if (malhas != null)
		{
			res = planeta.resolution;
            raioDeColisao = (Mathf.PI * planeta.shapeSettings.RaioDoPlaneta) / (2 * res);
            sheets = new Sheet[malhas.Length];
			if (malhas != null || malhas.Length > 0)
			{		
				int indiceMalha = 0;

				foreach(MeshFilter malha in malhas)
				{
                    if (malha.gameObject.activeSelf)
                    {
                        Vector3[] vertices = planeta.meshFilters[indiceMalha].sharedMesh.vertices;
                        sheets[indiceMalha] = new Sheet(this, indiceMalha,  (res - 1) * (res -1));
                        int indexVert = 0;
                        int indexTile = 0;
                        for (int x = 0; x < res - 1; x++)
                        {
                            for (int y = 0; y < res - 1; y++)
                            {
                                Vector3[] vert = new Vector3[4];
                                vert[0] = vertices[indexVert];
                                vert[1] = vertices[indexVert + 1];
                                vert[2] = vertices[indexVert + res + 1];
                                vert[3] = vertices[indexVert + res];

                                //Vector3 tileCenter = Utilidades.CalculateCentroid(vert);
                                Vector3 tileCenter = new Vector3((vert[0].x + vert[1].x + vert[2].x + vert[3].x) / 4, (vert[0].y + vert[1].y + vert[2].y + vert[3].y) / 4, (vert[0].z + vert[1].z + vert[2].z + vert[3].z) / 4);

                                bool andavel = !(Physics.CheckSphere(tileCenter, raioDeColisao, mascaraNaoAndavel));
                                
                                sheets[indiceMalha].tiles[indexTile] = new Tile(sheets[indiceMalha], x, y, tileCenter, andavel);
                                indexVert++;
                                indexTile++;
                            }
                            indexVert++;

                        }
                        indiceMalha++;
                    }
				}
			}
		}
		else
			Debug.LogError("malhas é null!!");

	}

    void BlurPesoMovimento(int blurSize)
    {
        int kernelSize = blurSize * 2 + 1;
        int kernelExtents = (kernelSize - 1) / 2;

        int[,] pesoPassHor = new int[res - 1, res - 1];
        int[,] pesoPassVer = new int[res - 1, res - 1];

        for(int y = 0; y < res -1; y++)
        {
            for(int x = -kernelExtents; x <= kernelExtents; x++)
            {
                int sampleX = Mathf.Clamp(x, 0, kernelExtents);
                pesoPassHor[0, y] += sheets[0].tiles[sampleX * (res -1) + y].data.pesoDeNavegacao;
            }
            for (int x = 1; x < res-1; x++)
            {
                int removeIndex = Mathf.Clamp(x - kernelExtents - 1, 0, res - 1);
                int AddIndex = Mathf.Clamp(x + kernelExtents, 0, res - 1 - 1);
                pesoPassHor[x,y] = pesoPassHor[x - 1, y] - sheets[0].tiles[removeIndex * (res -1) + y].data.pesoDeNavegacao + sheets[0].tiles[AddIndex * (res -1) + y].data.pesoDeNavegacao;
            }
        }
        for(int x = 0; x < res -1; x++)
        {
            for(int y = -kernelExtents; y <= kernelExtents; y++)
            {
                int sampleY = Mathf.Clamp(y, 0, kernelExtents);
                pesoPassVer[x, 0] += pesoPassHor[x, sampleY];
            }
            for (int y = 1; y < res-1; y++)
            {
                int removeIndex = Mathf.Clamp(y - kernelExtents - 1, 0, res - 1);
                int AddIndex = Mathf.Clamp(y + kernelExtents, 0, res - 1 - 1);
                pesoPassVer[x,y] = pesoPassVer[x, y - 1] - pesoPassHor[x, removeIndex] + pesoPassHor[x, AddIndex];
                int blurPesoMovimento = Mathf.RoundToInt((float)pesoPassVer[x,y] / (kernelSize * kernelSize));
                sheets[0].tiles[x * (res -1) + y].data.pesoDeNavegacao = blurPesoMovimento;
            }
        }

    }

    public List<Tile> GetNeigbours(Tile tile)
    {
        List<Tile> neighbours =  new List<Tile>();
        int tileIndex;

        for (tileIndex = 0; tileIndex < sheets[tile.GetSheetNumber()].tiles.Length; tileIndex++)
        {
            if (sheets[tile.GetSheetNumber()].tiles[tileIndex].x == tile.x && sheets[tile.GetSheetNumber()].tiles[tileIndex].y == tile.y)
                break;
        }

        for(int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                if (x == 0 && y == 0) continue;
                int checkX = tile.x + x;
                int checkY = tile.y + y;
                if (checkX >= 0 && checkX < res - 1 && checkY >= 0 && checkY < res - 1)
                {
                    if (sheets[tile.GetSheetNumber()].tiles[tileIndex + (res - 1)  * x + y].x == checkX && sheets[tile.GetSheetNumber()].tiles[tileIndex + (res - 1) * x + y].y == checkY)
                        neighbours.Add(sheets[tile.GetSheetNumber()].tiles[tileIndex + (res - 1) * x + y]);
                }

            }
        }

        return neighbours;
    }
	
	public Vector2 GetTilePosition(int sheet, int triIndex)
	{
        return new Vector2(sheets[sheet].tiles[triIndex / 2].x, sheets[sheet].tiles[triIndex / 2].y);
	}

    public Vector3 GetTileWorldPosition(int sheet, int triIndex)
    {
        return sheets[sheet].tiles[triIndex / 2].GetTileCenter(); 
    }

    public void OnDrawGizmos()
    {
        if (desenhaGizmoz)
        {
            if (sheets != null)
            {
                float raio = (Mathf.PI * planeta.shapeSettings.RaioDoPlaneta) / (2 * res);
                for (int j = 0; j < sheets.Length; j++)
                    if (sheets[j] != null)
                        for (int i = 0; i < sheets[j].tiles.Length; i++)
                        {
                            if (!sheets[j].tiles[i].IsAndavel())
                                Gizmos.color = Color.black;
                            else if (sheets[j].tiles[i].x == 0)
                                Gizmos.color = Color.green;
                            else if (sheets[j].tiles[i].x == res - 2)
                                Gizmos.color = Color.red;
                            else if (sheets[j].tiles[i].y == 0)
                                Gizmos.color = Color.yellow;
                            else if (sheets[j].tiles[i].y == res - 2)
                                Gizmos.color = Color.blue;
                            else
                                Gizmos.color = Color.white;

                            if (sheets[j].tiles[i] != null && raio > 0 && drawGizmos)
                                Gizmos.DrawCube(sheets[j].tiles[i].GetTileCenter(), Vector3.one * raio * .9f);
                        }
            }
        }
    }
}

[System.Serializable]
public struct PesosDeMovimento
{
    public Tile.TileType tipoDePiso;
    public int pesoDoMovimento;
    public bool andavel;
}