using UnityEngine;
using System.Collections;

public class MenuTouchController : MonoBehaviour {

	void Start()
	{
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
		if (Input.GetMouseButton(0))
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
					break;
				}
				case 2:
				{
					//single
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
					Application.LoadLevel ("LocalPlayerVsPlayerScene"); 
					break;
				}
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
