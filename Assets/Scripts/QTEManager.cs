﻿using UnityEngine;
using System.Collections;

public class QTEManager : MonoBehaviour {

	private static QTEManager instancia;

	private float tiempo_qte;

	private const int _TIA = 0;
	private const int _MUDO = 1;
	private const int _BIGOTE = 2;
	private const int _LOCO = 3;

	private int jugador_actual;
	private int jugadores_activos;

	public FaceButtons facebuttons;

	public GameObject prefab_velo;
	private GameObject[] velos = new GameObject[4];

	void Awake()
	{
		instancia = this;
	}

	// Use this for initialization
	void Start () 
	{

	}
	
	// Update is called once per frame
	void Update () 
	{

	}

	public void iniciar_ronda()
	{
		activar_jugadores();
		asignar_QTE(_TIA, Random.Range(0,4));
	}

	void activar_jugadores ()
	{
		jugador_actual = _TIA;
		jugadores_activos = 4;

		for(int i=0; i<jugadores_activos; i++)
		{
			GameController.Instancia.getJugadorIndex(i).GetComponent<ValidadorQTE>().activar();
			GameController.Instancia.getJugadorIndex(i).GetComponent<Carrera>().velocity = 0;
			GameController.Instancia.getPistaIndex(i).GetComponent<Animator>().speed = 0;

		}
	}

	void asignar_QTE(int jugador, int boton)
	{
		posicionar_facebuttons(jugador);
		facebuttons.asignar_boton(boton);
		GameController.Instancia.getJugadorIndex(jugador).GetComponent<ValidadorQTE>().asignar(boton);
	}

	void posicionar_facebuttons (int jugador)
	{
		facebuttons.gameObject.transform.position = new Vector3(-4.5f + (jugador%2) * 9, 2.5f - (jugador/2) * 5,-3);
	}

	public void asignar_QTE_siguiente()
	{
		if(jugadores_activos > 0)
		{
			do
			{
				jugador_actual = (jugador_actual + 1)%4;
			}
			while(GameController.Instancia.getJugadorIndex(jugador_actual).GetComponent<ValidadorQTE>().HaPerdido == true);
			facebuttons.resetear_escalas();
			asignar_QTE(jugador_actual, Random.Range(0,4));
		}
		else
		{
			facebuttons.resetear_escalas();
		}
	}

	public void declarar_error (int jugador)
	{
		QTEManager.Instancia.Jugadores_activos -= 1;
		velos[jugador-1] = Instantiate(prefab_velo, new Vector3(-4.5f + ((jugador-1)%2) * 9, 2.5f - ((jugador-1)/2) * 5,-3), Quaternion.identity) as GameObject;

		QTEManager.Instancia.asignar_QTE_siguiente();
	}

	public static QTEManager Instancia 
	{
		get 
		{
			return instancia;
		}
	}

	public int Jugadores_activos {
		get {
			return jugadores_activos;
		}
		set {
			jugadores_activos = value;
		}
	}
}
