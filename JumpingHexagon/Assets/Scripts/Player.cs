﻿using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {

	//Player1: Violet;
	//Player2: Black;
	//playerID
	// = 0 : Draw
	// = -1: violet win
	// = -2: black win
	public int playerID;

	// Use this for initialization
	void Start () {
		//Violet danh truoc
		GameObject.Find("turnBlack").GetComponent<Animator>().SetBool("isOff",true);
		playerID = 1;
	}

	// Xet xem hexagon truyen vao cua ai
	public int whoseContainer(GameObject container)
	{
		if (container == null)
			return -1;
		GameObject hexagon = container.GetComponent<HexagonController>().getHexagon();
		if (hexagon == null)
			return 0;
		if (hexagon.name.Contains("HexagonViolet"))
			return 1;
		if (hexagon.name.Contains("HexagonBlack"))
			return 2;
		return 0;
	}

	// Xet xem hexagon truyen vao cua ai
	public int whoseHexagon(GameObject hexagon)
	{
		if (hexagon == null)
			return 0;
		if (hexagon.name.Contains("HexagonViolet"))
		    return 1;
		if (hexagon.name.Contains("HexagonBlack"))
			return 2;
		return 0;
	}

	public void changePlayer()
	{
		if (playerID == 1)
		{
			playerID = 2;
			GameObject.Find("turnViolet").GetComponent<Animator>().SetBool("isOff",true);
			GameObject.Find("turnBlack").GetComponent<Animator>().SetBool("isOff",false);
		}
		else
		{
			GameObject.Find("turnBlack").GetComponent<Animator>().SetBool("isOff",true);
			GameObject.Find("turnViolet").GetComponent<Animator>().SetBool("isOff",false);
			playerID = 1;
		}
	}	
}
