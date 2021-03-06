﻿using UnityEngine;
using System.Collections;

public class TouchController : MonoBehaviour {

	//POS: TOP LEFT, BOTTOM RIGHT
//Violet Win or Draw
	// Back to Menu Pos
	public Vector2 backMenuTopLeftVioletWin = new Vector2(-4.5f,-2.4f);
	public Vector2 backMenuBotRightVioletWin = new Vector2(-1.2f,-3.5f);
	// Restart
	public Vector2 restartTopLeftVioletWin = new Vector2(1.34f,-2.4f);
	public Vector2 restartBotRightVioletWin = new Vector2(4.65f,-3.5f);
//Black Win
	// Back to Menu Pos
	public Vector2 backMenuTopLeftBlackWin = new Vector2(1.23f,3.43f);
	public Vector2 backMenuBotRightBlackWin = new Vector2(4.54f,2.35f);
	// Restart
	public Vector2 restartTopLeftBlackWin = new Vector2(-4.64f,3.43f);
	public Vector2 restartBotRightBlackWin = new Vector2(-1.24f,2.35f);

	//co 2 tuto, = 1: dang load tuto1 =2: dang load tuto2
	private int isloadedTutorial;
	private GameObject tuto1;
	private GameObject tuto2;

	private int size;
	//Do dai cua Column dai nhat (column 0)
	private int maxColumnLength;

	private bool isTouchedDown;

	private int isRunTutorialInNhatMotLan;

	void Start()
	{
		isloadedTutorial = 0;
		isTouchedDown = false;
		tuto1 = (GameObject) Resources.Load("Prefab/Tuto1");
		tuto2 = (GameObject) Resources.Load("Prefab/Tuto2");

		//Play Tutolan dau tien
		isRunTutorialInNhatMotLan = PlayerPrefs.GetInt("isRunTutorialInNhatMotLan");
		if (isRunTutorialInNhatMotLan == 0)
		{
			PlayerPrefs.SetInt("isRunTutorialInNhatMotLan",1);
			isloadedTutorial = 1;
			Instantiate(tuto1);
		}
	}

	// Update is called once per frame
	void Update () {
		if (Input.GetMouseButtonUp(0))
		{
			isTouchedDown = false;
			return;
		}
		if (Input.GetMouseButton(0))
		{
			if (isTouchedDown)
				return;
			else
				isTouchedDown = true;
		}
		else
			return;

		if (isloadedTutorial == 1 && Input.GetMouseButton(0))
		{
			Destroy(GameObject.Find("Tuto1(Clone)"));
			isloadedTutorial = 2;
			Instantiate(tuto2);
		} 
		else if (isloadedTutorial == 2 && Input.GetMouseButton(0))
		{
			Destroy(GameObject.Find("Tuto2(Clone)"));
			isloadedTutorial = 0;
		} 
		else if (isloadedTutorial == 0 && Input.GetMouseButton(0))
		{
			if (GameObject.Find("Hexagon Background").GetComponent<Player>().playerID > 0)
			{
				GameObject nearestCnt = findNearestHexagonContainer(Input.mousePosition);
				if (nearestCnt != null)
				{
					nearestCnt.GetComponent<HexagonController>().onTouched();
				}

				//kiem tra co an vao tutorial hay ko
				Vector2 touchPos = getPointToWorld(Input.mousePosition);
				//Neu xa qua thi cung coi nhu khong click vao Gameobject nao.
				float XA_QUA_ROI_EM_OI = 0.5f;
				GameObject tutorialButton = GameObject.Find("menu1");
				Vector2 buttonPos = new Vector2(tutorialButton.transform.position.x, tutorialButton.transform.position.y);
				float khoangCachButtonvoiDiemTouch = Vector2.Distance(buttonPos, touchPos);
				if (khoangCachButtonvoiDiemTouch < XA_QUA_ROI_EM_OI)
				{
					//guide
					//load tuto = 0;
					isloadedTutorial = 1;
					Instantiate(tuto1);
				}

				GameObject quitButton = GameObject.Find("menu6");
				Vector2 quitButtonPos = new Vector2(quitButton.transform.position.x,quitButton.transform.position.y);
				float khoangCachButtonQuitVoiTouchPos = Vector2.Distance(quitButtonPos, touchPos);
				if (khoangCachButtonQuitVoiTouchPos < XA_QUA_ROI_EM_OI)
				{
					Application.LoadLevel("MenuScene");
				}
			}
			else
			//Kiem tra touch cho menu hien thi len
			{
				Vector2 touchPos = getPointToWorld(Input.mousePosition);
				if (gameObject.GetComponent<Player>().playerID == 0 || gameObject.GetComponent<Player>().playerID == -1)
				{
					//Back menu
					if (isInLine(touchPos.x,backMenuTopLeftVioletWin.x,backMenuBotRightVioletWin.x) &&
					    isInLine(touchPos.y,backMenuTopLeftVioletWin.y,backMenuBotRightVioletWin.y))
					{
						Application.LoadLevel("MenuScene");
						return;
					}
					//restart
					if (isInLine(touchPos.x,restartTopLeftVioletWin.x,restartBotRightVioletWin.x) &&
					    isInLine(touchPos.y,restartTopLeftVioletWin.y,restartBotRightVioletWin.y))
					{
						Application.LoadLevel(Application.loadedLevel);
						return;
					}
				}
				else
				{
					//Back menu
					if (isInLine(touchPos.x,backMenuTopLeftBlackWin.x,backMenuBotRightBlackWin.x) &&
					    isInLine(touchPos.y,backMenuTopLeftBlackWin.y,backMenuBotRightBlackWin.y))
					{
						Application.LoadLevel("MenuScene");
						return;
					}
					//restart
					if (isInLine(touchPos.x,restartTopLeftBlackWin.x,restartBotRightBlackWin.x) &&
					    isInLine(touchPos.y,restartTopLeftBlackWin.y,restartBotRightBlackWin.y))
					{
						Application.LoadLevel(Application.loadedLevel);
						return;
					}
				}
			}
		}
	}

	//////////////////
	///

	// Tra ve xem k co thuoc khoang (x,y) hoac (y,x) khong
	private bool isInLine(float k, float x, float y)
	{
		if (Mathf.Min(x,y) <= k && k <= Mathf.Max(x,y))
			return true;
		return false;
	}

	// Lay ve point to world (vector2) tu mousePosition (vector3)
	private Vector2 getPointToWorld(Vector3 mousePosition)
	{
		Vector3 mousePointedToWorld3 = Camera.main.ScreenToWorldPoint(mousePosition);
		return new Vector2(mousePointedToWorld3.x,mousePointedToWorld3.y);
	}

	// Tim Hexacontainer gan nhat vs diem touch sau khi cham man hinh
	public GameObject findNearestHexagonContainer(Vector3 touchMousePosition)
	{
		//CONST
		size = GameObject.Find("Hexagon Background").GetComponent<HexagonContainerGenerator>().size;
		maxColumnLength = size * 2 - 1;

		// get vector2 of touch pos
		Vector2 touchPos = getPointToWorld(touchMousePosition);

		// Duyet toan bo hexagon
		GameObject nearestContainer = null;
		float minDistance = float.MaxValue;
		//Neu xa qua thi cung coi nhu khong click vao Gameobject nao.
		float XA_QUA_ROI_EM_OI = 0.5f;
		for (int col = 1-size; col <= size-1; col++)
		{
			for (int row = 0; row < maxColumnLength - Mathf.Abs(col); row++)
			{
				GameObject ctn = GameObject.Find("ctn["+col+","+row+"]");
				Vector2 ctnPos = new Vector2(ctn.transform.position.x, ctn.transform.position.y);
				float khoangCachCntvoiDiemTouch = Vector2.Distance(ctnPos, touchPos);
				if (minDistance > khoangCachCntvoiDiemTouch && khoangCachCntvoiDiemTouch < XA_QUA_ROI_EM_OI)
				{
					nearestContainer = ctn;
					minDistance = khoangCachCntvoiDiemTouch;
				}
			}
		}
		return nearestContainer;
	}
}
