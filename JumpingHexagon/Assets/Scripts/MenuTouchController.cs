using UnityEngine;
using System.Collections;

public class MenuTouchController : MonoBehaviour {

	//co 2 tuto, = 1: dang load tuto1 =2: dang load tuto2
	private int isloadedTutorial;
	private GameObject tuto1;
	private GameObject tuto2;
	private bool isTouchedDown;

	void Start()
	{
		isloadedTutorial = 0;
		tuto1 = (GameObject) Resources.Load("Prefab/Tuto1");
		tuto2 = (GameObject) Resources.Load("Prefab/Tuto2");

		isTouchedDown = false;

		//get preferences MUSIC
		int temp = PlayerPrefs.GetInt("MusicOff");
		if (temp == 0)
		{
			AudioListener.volume = 1;
		}
		else
		{
			//anim
			GameObject.Find("menu3").GetComponent<Animator>().SetBool("onMusic",false);
			GameObject.Find("meny3_off").GetComponent<Animator>().SetBool("onMusic",true);
			AudioListener.volume = 0;
		}
	}

	// Update is called once per frame
	void Update () {
		if (Input.GetMouseButtonUp(0))
		{
			isTouchedDown = false;
			return;
		}
		if (Input.GetMouseButton(0) && !isTouchedDown)
		{
			isTouchedDown = true;
			if (isloadedTutorial == 0)
			{
				Vector2 touchPos = getPointToWorld(Input.mousePosition);
				// Duyet toan bo 4 button
				int luuButton = 0;
				float minDistance = float.MaxValue;
				//Neu xa qua thi cung coi nhu khong click vao Gameobject nao.
				float XA_QUA_ROI_EM_OI = 0.5f;
				for (int i = 1; i <= 4; i++)
				{
					GameObject button = GameObject.Find("menu"+i);
					Vector2 buttonPos = new Vector2(button.transform.position.x, button.transform.position.y);
					float khoangCachButtonvoiDiemTouch = Vector2.Distance(buttonPos, touchPos);
					if (minDistance > khoangCachButtonvoiDiemTouch && khoangCachButtonvoiDiemTouch < XA_QUA_ROI_EM_OI)
					{
						luuButton = i;
						minDistance = khoangCachButtonvoiDiemTouch;
					}
				}

				switch (luuButton)
				{
					case 1:
					{
						//guide
						//load tuto = 0;
						isloadedTutorial = 1;
						Instantiate(tuto1);
						break;
					}
					case 2:
					{
						//single
						PlayerPrefs.SetInt("gameType",1);
						Application.LoadLevel ("LocalPlayerVsPlayerScene");
						break;
					}
					case 3:
					{
						int temp = PlayerPrefs.GetInt("MusicOff");
						if (temp == 0)
						{
							PlayerPrefs.SetInt("MusicOff",1);
							AudioListener.volume = 0;
							//anim
							GameObject.Find("menu3").GetComponent<Animator>().SetBool("onMusic",false);
							GameObject.Find("meny3_off").GetComponent<Animator>().SetBool("onMusic",true);
						}
						else
						{
							PlayerPrefs.SetInt("MusicOff",0);
							AudioListener.volume = 1;
							//anim
							GameObject.Find("menu3").GetComponent<Animator>().SetBool("onMusic",true);
							GameObject.Find("meny3_off").GetComponent<Animator>().SetBool("onMusic",false);
						}
						break;
					}
					case 4:
					{
						//multi
						PlayerPrefs.SetInt("gameType",2);
						Application.LoadLevel ("LocalPlayerVsPlayerScene"); 
						break;
					}
				}
			}
			else if (isloadedTutorial == 1)
			{
				Destroy(GameObject.Find("Tuto1(Clone)"));
				isloadedTutorial = 2;
				Instantiate(tuto2);
			}
			else if (isloadedTutorial == 2)
			{
				Destroy(GameObject.Find("Tuto2(Clone)"));
				isloadedTutorial = 0;
			}
		}
	}

	// Lay ve point to world (vector2) tu mousePosition (vector3)
	private Vector2 getPointToWorld(Vector3 mousePosition)
	{
		Vector3 mousePointedToWorld3 = Camera.main.ScreenToWorldPoint(mousePosition);
		return new Vector2(mousePointedToWorld3.x,mousePointedToWorld3.y);
	}
}
