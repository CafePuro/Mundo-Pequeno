using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sheet 
{
	//Vamos assumir que cada folha é quadrada e fará contato 
	//com apenas unma folha em cada extremidade. Será identificada
	// com o enum abaixo 
	enum Bordas {NORTE, LESTE, SUL, OESTE};

	//Mapa ao qual a folha pertence]
	Mapa mapa;
	//numero indetificador da folha do mapa
	int sheet;
	//Os tiles que compoem este mapa
	[SerializeField, HideInInspector]
	public Tile[] tiles;
	
	//Mapeia a borda que esta em contato com a borda de outra folha
	struct Contato 
	{
		//Borda desta folha
		Bordas borda;
		//Folha na Qual essa borda esta conectada
		int sheet;
		//Borda que da outra folha que está em contato
		Bordas outraBorda;
	}
	
	public Sheet(Mapa mapa, int sheet, int size)
	{
		this.mapa = mapa;
		this.sheet = sheet;
		this.tiles = new Tile[size];
	}

	public int GetSheetNumber()
	{
		return sheet;
	}

}
