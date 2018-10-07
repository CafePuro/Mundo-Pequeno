using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Tile : IHeapItem<Tile>
{
	public enum TileType {TERRA, GRAMA, ROCHA, AGUA};

	//Guarda os vertices que formam o Quad
	Vector3 tileCenter;
	//Tipo do tile
	TileType tileType = TileType.TERRA;
	//Guarda o pedaco do planeta onde o tile se encontra e a posicao dele
	public int x;
	public int y;

    //Navegação
    public int gCost;
    public int hCost;
    public Tile parent;
    int heapIndex;


    public Data data = new Data();

	//A folha ao qual o tile pertence
	Sheet sheet;

	public Tile(Sheet sheet, int x, int y,Vector3 tileCenter, TileType tileType, float modificadorDeVelocidade, bool andavel)
	{
		data.modificadorDeVelocidade = modificadorDeVelocidade;
        data.pesoDeNavegacao = 0;
        data.andavel = andavel;
		this.tileType = tileType;
		this.tileCenter = tileCenter;
		this.sheet = sheet;
		this.x = x;
		this.y = y;
	}

	public Tile(Sheet sheet, int x, int y, TileType tileType, Vector3 tileCenter, bool andavel)
	{
		data.modificadorDeVelocidade = 1f;
        data.pesoDeNavegacao = 0;
        data.andavel = andavel;
		this.tileType = tileType;
		this.tileCenter = tileCenter;
		this.sheet = sheet;
		this.x = x;
		this.y = y;
	}
	public Tile(Sheet sheet, int x, int y, Vector3 tileCenter, bool andavel)
	{
		data.modificadorDeVelocidade = 0f;
        data.pesoDeNavegacao = 1;
        data.andavel = andavel;
		this.tileCenter = tileCenter;
		this.sheet = sheet;
		this.x = x;
		this.y = y;

	}

	public int GetSheetNumber()
	{
		return this.sheet.GetSheetNumber();
	}
	public TileType GetTileType()
	{
		return this.tileType;
	}

	public void SetTileType(TileType tileType)
	{
		this.tileType = tileType;
	}
	
	public Vector3 GetTileCenter()
	{
		return this.tileCenter;
	}
	
    public bool IsAndavel()
    {
        return this.data.andavel;
    }

	public struct Data 
	{
		public float modificadorDeVelocidade;
        public int pesoDeNavegacao;
        public bool andavel;
	}

    //Navegação A*
    public int fCost
    {
        get 
        {
            return gCost + hCost;
        }
    }

    //interface  do heap
    public int HeapIndex
    {
        get
        {
            return heapIndex;
        }
        set
        {
            heapIndex = value;
        }
    }

    public int CompareTo(Tile tileToCompare)
    {
        int compare = fCost.CompareTo(tileToCompare.fCost);
        if(compare == 0)
        {
            compare = hCost.CompareTo(tileToCompare.hCost);
        }
        return -compare;
    }

}
