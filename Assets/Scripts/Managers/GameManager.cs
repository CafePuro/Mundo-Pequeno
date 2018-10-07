/*
 * Created on Sat Sep 15 2018 by ZZZeMarcelo
 *
 * Copyright (c) 2018 Café Puro Digital Studio
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    bool paused;

    public enum GameStates {STARTING, RUNNING, PAUSED, SAVING, LOADING, FINISHING };

    public string[] cenas;
    string cenaAtual;

    //Static instance of GameManager which allows it to be accessed by any other script.
    public static GameManager instance = null;                                  

    void Awake()
    {
        Debug.Log("Iniciando o Jogo!");    
        //Check if instance already exists
        if (instance == null)
            //if not, set instance to this
            instance = this;
        //If instance already exists and it's not this:
        else if (instance != this)
            //Then destroy this. This enforces our singleton pattern, meaning there can only ever be one instance of a GameManager.
            Destroy(gameObject);
        //Sets this to not be destroyed when reloading scene
        DontDestroyOnLoad(gameObject);
	}

    public void LoadScene(string cena)
    {
        cenaAtual = cena;
        SceneManager.LoadScene(cena);
    }


    //Método para pausar o jogo
    public bool Paused
    {
        get 
        { 
            // get paused
            return paused; 
        }
        set
        {
            // set paused 
            paused = value;

			if (paused)
			{
                // pause time
                Time.timeScale= 0f;
			} else {
                // unpause Unity
				Time.timeScale = 1f;
            }
        }
    }
}

