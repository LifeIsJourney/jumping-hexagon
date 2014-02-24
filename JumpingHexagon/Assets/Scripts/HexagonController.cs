using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HexagonController : MonoBehaviour {

	public static GameObject selectingContainer = null;
	public GameObject aval1Prefab;
	public GameObject aval2Prefab;
	public GameObject hexagonVioletPrefab;
	public GameObject hexagonBlackPrefab;
	public GameObject fireworkOneTime;
	public GameObject smallFireworkOneTime;

	void Start()
	{
		connectPrefab();
	}

	//////////////////////////////////////////

	private void connectPrefab()
	{
		aval1Prefab = (GameObject) Resources.Load("Prefab/avalLevel1");
		aval2Prefab = (GameObject) Resources.Load("Prefab/avalLevel2");
		hexagonVioletPrefab = (GameObject) Resources.Load("Prefab/HexagonViolet");
		hexagonBlackPrefab = (GameObject) Resources.Load("Prefab/HexagonBlack");
		fireworkOneTime = (GameObject) Resources.Load("Prefab/FireworkOneTime");
		smallFireworkOneTime = (GameObject) Resources.Load("Prefab/SmallFireworkOneTime");
	}

	// lay ve doi tuong hexagon ma container dang co
	public GameObject getHexagon()
	{
		for (int i=0; i<transform.childCount; i++)
		{
			if (transform.GetChild(i).gameObject.name.Contains("Hexagon"))
			return transform.GetChild(i).gameObject;
		}
		return null;
	}

	// Lay ve Avalaible dang phu len hexagon container
	public GameObject getAvalaibleObject()
	{
		for (int i=0; i<transform.childCount; i++)
		{
			if (transform.GetChild(i).gameObject.name.Contains("aval"))
				return transform.GetChild(i).gameObject;
		}
		return null;
	}

	// Khong kiem tra, add moi, hoac de len Hexagon cu
	public void addHexagon(GameObject newHexagon)
	{
		//Clear all child hexagon if exist
		removeHexagon();

		//Add new hexagon
		newHexagon.transform.parent = transform;
		newHexagon.transform.localPosition = hexagonVioletPrefab.transform.position;
	}

	// Duoc goi khi Hexagon Container bi click
	public void onTouched()
	{
		if (GameObject.Find("Hexagon Background").GetComponent<CommonVar>().isEnablingTouch() == false)
			return;

		//Neu click vao chinh minh thi return
		if (selectingContainer == this.gameObject)
			return;

		GameObject previousContainer = selectingContainer;
		GameObject childHexagon = getHexagon();

		if (childHexagon != null)
		{
			// player check
			if (GameObject.Find("Hexagon Background").GetComponent<Player>().whoseHexagon(childHexagon) != 
			    GameObject.Find("Hexagon Background").GetComponent<Player>().playerID)
			{
				//play false sound
				gameObject.GetComponents<AudioSource>()[1].Play();
				return;
			}

			//play sound
			gameObject.GetComponents<AudioSource>()[0].Play();

			// Xu ly thang Container truoc
			if (previousContainer != null)
			{
				previousContainer.GetComponent<HexagonController>().onLostTouched();
			}

			//play animation
			getHexagon().GetComponent<Animator>().SetBool("ActiveRotate", true);
			
			// chuyen selecting thanh thang container hien tai
			selectingContainer = this.gameObject;
			
			//active avalaible move
			foreach (GameObject ctn in getListRelativeHexagonLevel1(selectingContainer))
			{
				GameObject aval1 = (GameObject) (Instantiate(aval1Prefab, aval1Prefab.transform.position, aval1Prefab.transform.rotation));
				aval1.transform.parent = ctn.transform;
				aval1.transform.localPosition = aval1.transform.position;
				aval1.GetComponent<Animator>().SetBool("Grow",true);
			}
			foreach (GameObject ctn in getListRelativeHexagonLevel2(selectingContainer))
			{
				GameObject aval2 = (GameObject) (Instantiate(aval2Prefab, aval2Prefab.transform.position, aval2Prefab.transform.rotation));
				aval2.transform.parent = ctn.transform;
				aval2.transform.localPosition = aval2.transform.position;
				aval2.GetComponent<Animator>().SetBool("Grow",true);
			}
		}

		if (childHexagon == null)
		{
			if (previousContainer == null)
			{
				//play false sound
				gameObject.GetComponents<AudioSource>()[1].Play();
				return;
			}

			//Kiem tra co phai la hang xom cap 1 hoac cap 2 khong
			foreach (GameObject ctn in getListRelativeHexagonLevel1(selectingContainer))
			{
				if (ctn == this.gameObject)
				{
					//remove selecting
					selectingContainer.GetComponent<HexagonController>().onLostTouched();
					selectingContainer = null;

					//Sinh hexagon moi giong voi kieu hexagon dang co
					GameObject newHexagon = null;
					if (previousContainer.GetComponent<HexagonController>().getHexagon().name.Contains("HexagonBlack"))
							newHexagon = (GameObject) (Instantiate(hexagonBlackPrefab));
					else if (previousContainer.GetComponent<HexagonController>().getHexagon().name.Contains("HexagonViolet"))
							newHexagon = (GameObject) (Instantiate(hexagonVioletPrefab));
					addHexagon(newHexagon);
					//play small firework
					Vector3 smallfireworkPos = new Vector3(transform.position.x,transform.position.y,smallFireworkOneTime.transform.position.z);
					Instantiate(smallFireworkOneTime, smallfireworkPos, smallFireworkOneTime.transform.rotation);
					//sound spawn
					gameObject.GetComponents<AudioSource>()[2].Play();

					//Bien hexagon chung quanh thanh ban cua no
					foreach(GameObject hangxom1 in getHangXom(this.gameObject))
					{
						if (this.gameObject == hangxom1)
							continue;

						GameObject oldHexagonHangXom1 = hangxom1.GetComponent<HexagonController>().getHexagon();
						if (oldHexagonHangXom1 != null)
						{
							hangxom1.GetComponent<HexagonController>().removeHexagon();
							GameObject newHangXomHexagon = null;
							if (previousContainer.GetComponent<HexagonController>().getHexagon().name.Contains("HexagonBlack"))
							{
								newHangXomHexagon = (GameObject) (Instantiate(hexagonBlackPrefab));
								if (oldHexagonHangXom1.name.Contains("HexagonViolet"))
								{
									//play firework
									Vector3 fireworkPos = new Vector3(hangxom1.transform.position.x,hangxom1.transform.position.y,fireworkOneTime.transform.position.z);
									Instantiate(fireworkOneTime, fireworkPos, fireworkOneTime.transform.rotation);
								}
							}
							else if (previousContainer.GetComponent<HexagonController>().getHexagon().name.Contains("HexagonViolet"))
							{
								newHangXomHexagon = (GameObject) (Instantiate(hexagonVioletPrefab));
								if (oldHexagonHangXom1.name.Contains("HexagonBlack"))
								{
									//play firework
									Vector3 fireworkPos = new Vector3(hangxom1.transform.position.x,hangxom1.transform.position.y,fireworkOneTime.transform.position.z);
									Instantiate(fireworkOneTime, fireworkPos, fireworkOneTime.transform.rotation);
								}
							}
							hangxom1.GetComponent<HexagonController>().addHexagon(newHangXomHexagon);
						}
					}

					//Change player
					GameObject.Find("Hexagon Background").GetComponent<Player>().changePlayer();
					return;
				}
			}

			foreach (GameObject ctn in getListRelativeHexagonLevel2(selectingContainer))
			{
				if (ctn == this.gameObject)
				{
					//remove selecting
					selectingContainer.GetComponent<HexagonController>().onLostTouched();
					selectingContainer = null;

					//Sinh hexagon moi giong voi kieu hexagon dang co
					GameObject newHexagon = null;
					if (previousContainer.GetComponent<HexagonController>().getHexagon().name.Contains("HexagonBlack"))
						newHexagon = (GameObject) (Instantiate(hexagonBlackPrefab));
					else if (previousContainer.GetComponent<HexagonController>().getHexagon().name.Contains("HexagonViolet"))
						newHexagon = (GameObject) (Instantiate(hexagonVioletPrefab));
					addHexagon(newHexagon);
					//play small firework
					Vector3 smallfireworkPos = new Vector3(transform.position.x,transform.position.y,smallFireworkOneTime.transform.position.z);
					Instantiate(smallFireworkOneTime, smallfireworkPos, smallFireworkOneTime.transform.rotation);
					//sound jump
					gameObject.GetComponents<AudioSource>()[3].Play();

					//Xoa Hexagon o previous
					previousContainer.GetComponent<HexagonController>().removeHexagon();

					//Bien hexagon chung quanh thanh ban cua no
					foreach(GameObject hangxom1 in getHangXom(this.gameObject))
					{
						if (this.gameObject == hangxom1)
							continue;
						
						GameObject oldHexagonHangXom1 = hangxom1.GetComponent<HexagonController>().getHexagon();
						if (oldHexagonHangXom1 != null)
						{
							hangxom1.GetComponent<HexagonController>().removeHexagon();
							GameObject newHangXomHexagon = null;
							if (previousContainer.GetComponent<HexagonController>().getHexagon().name.Contains("HexagonBlack"))
							{
								newHangXomHexagon = (GameObject) (Instantiate(hexagonBlackPrefab));
								if (oldHexagonHangXom1.name.Contains("HexagonViolet"))
								{
									//play firework
									Vector3 fireworkPos = new Vector3(hangxom1.transform.position.x,hangxom1.transform.position.y,fireworkOneTime.transform.position.z);
									Instantiate(fireworkOneTime, fireworkPos, fireworkOneTime.transform.rotation);
								}
							}
							else if (previousContainer.GetComponent<HexagonController>().getHexagon().name.Contains("HexagonViolet"))
							{
								newHangXomHexagon = (GameObject) (Instantiate(hexagonVioletPrefab));
								if (oldHexagonHangXom1.name.Contains("HexagonBlack"))
								{
									//play firework
									Vector3 fireworkPos = new Vector3(hangxom1.transform.position.x,hangxom1.transform.position.y,fireworkOneTime.transform.position.z);
									Instantiate(fireworkOneTime, fireworkPos, fireworkOneTime.transform.rotation);
								}
							}
							hangxom1.GetComponent<HexagonController>().addHexagon(newHangXomHexagon);
						}
					}

					//Change player
					GameObject.Find("Hexagon Background").GetComponent<Player>().changePlayer();
					return;
				}
			}

			//play false sound
			gameObject.GetComponents<AudioSource>()[1].Play();
		}
	}

	// Remove hexagon
	public void removeHexagon()
	{
		if (getHexagon() != null)
		{
			Destroy(getHexagon());
		}
	}

	public void onLostTouched()
	{
		//stop animation
		if (getHexagon() != null)
			getHexagon().GetComponent<Animator>().SetBool("ActiveRotate", false);
		//clear avalaible move
		foreach (GameObject ctn in getListRelativeHexagonLevel1(selectingContainer))
			if (ctn.GetComponent<HexagonController>().getAvalaibleObject())
				ctn.GetComponent<HexagonController>().getAvalaibleObject().GetComponent<Animator>().SetBool("Disappear", true);
		foreach (GameObject ctn in getListRelativeHexagonLevel2(selectingContainer))
			if (ctn.GetComponent<HexagonController>().getAvalaibleObject())
				ctn.GetComponent<HexagonController>().getAvalaibleObject().GetComponent<Animator>().SetBool("Disappear", true);
	}

	// lay ve toa do cua Hexagon truyen vao
	private Vector2 getToaDoContainer(GameObject container)
	{
		int mocCol = int.Parse(container.name.Split('[')[1].Split(']')[0].Split(',')[0]);
		int mocRow = int.Parse(container.name.Split('[')[1].Split(']')[0].Split(',')[1]);
		return new Vector2(mocCol,mocRow);
	}

	// Lay ve hang xom cap 1 nhung la de bien cac hexagon chung quanh thanh ban cua no
	public List<GameObject> getHangXom(GameObject centerHexagonContainer)
	{
		Vector2 vectorPosCenterHexagonContainer = getToaDoContainer(centerHexagonContainer);
		int col = (int) (vectorPosCenterHexagonContainer.x);
		int row = (int) (vectorPosCenterHexagonContainer.y);
		
		List<GameObject> result = new List<GameObject>();
		GameObject ctn1 = null;
		GameObject ctn2 = null;
		GameObject ctn3 = null;
		GameObject ctn4 = null;
		GameObject ctn5 = null;
		GameObject ctn6 = null;
		
		ctn1 = GameObject.Find("ctn["+(col)+","+(row-1)+"]");
		ctn2 = GameObject.Find("ctn["+(col)+","+(row+1)+"]");
		if (col > 0)
		{
			ctn3 = GameObject.Find("ctn["+(col+1)+","+(row-1)+"]");
			ctn4 = GameObject.Find("ctn["+(col+1)+","+(row)+"]");
			ctn5 = GameObject.Find("ctn["+(col-1)+","+(row)+"]");
			ctn6 = GameObject.Find("ctn["+(col-1)+","+(row+1)+"]");
		}
		else if (col < 0)
		{
			ctn3 = GameObject.Find("ctn["+(col+1)+","+(row)+"]");
			ctn4 = GameObject.Find("ctn["+(col+1)+","+(row+1)+"]");
			ctn5 = GameObject.Find("ctn["+(col-1)+","+(row-1)+"]");
			ctn6 = GameObject.Find("ctn["+(col-1)+","+(row)+"]");
		}
		else if (col == 0)
		{
			ctn3 = GameObject.Find("ctn["+(col+1)+","+(row)+"]");
			ctn4 = GameObject.Find("ctn["+(col+1)+","+(row-1)+"]");
			ctn5 = GameObject.Find("ctn["+(col-1)+","+(row)+"]");
			ctn6 = GameObject.Find("ctn["+(col-1)+","+(row-1)+"]");
		}
		
		if (ctn1 != null)
			result.Add(ctn1);
		if (ctn2 != null)
			result.Add(ctn2);
		if (ctn3 != null)
			result.Add(ctn3);
		if (ctn4 != null)
			result.Add(ctn4);
		if (ctn5 != null)
			result.Add(ctn5);
		if (ctn6 != null)
			result.Add(ctn6);
		return result;
	}

	public List<GameObject> getListRelativeHexagonLevel1(GameObject centerHexagonContainer)
	{
		Vector2 vectorPosCenterHexagonContainer = getToaDoContainer(centerHexagonContainer);
		int col = (int) (vectorPosCenterHexagonContainer.x);
		int row = (int) (vectorPosCenterHexagonContainer.y);

		List<GameObject> result = new List<GameObject>();
		GameObject ctn1 = null;
		GameObject ctn2 = null;
		GameObject ctn3 = null;
		GameObject ctn4 = null;
		GameObject ctn5 = null;
		GameObject ctn6 = null;

		ctn1 = GameObject.Find("ctn["+(col)+","+(row-1)+"]");
		ctn2 = GameObject.Find("ctn["+(col)+","+(row+1)+"]");
		if (col > 0)
		{
			ctn3 = GameObject.Find("ctn["+(col+1)+","+(row-1)+"]");
			ctn4 = GameObject.Find("ctn["+(col+1)+","+(row)+"]");
			ctn5 = GameObject.Find("ctn["+(col-1)+","+(row)+"]");
			ctn6 = GameObject.Find("ctn["+(col-1)+","+(row+1)+"]");
		}
		else if (col < 0)
		{
			ctn3 = GameObject.Find("ctn["+(col+1)+","+(row)+"]");
			ctn4 = GameObject.Find("ctn["+(col+1)+","+(row+1)+"]");
			ctn5 = GameObject.Find("ctn["+(col-1)+","+(row-1)+"]");
			ctn6 = GameObject.Find("ctn["+(col-1)+","+(row)+"]");
		}
		else if (col == 0)
		{
			ctn3 = GameObject.Find("ctn["+(col+1)+","+(row)+"]");
			ctn4 = GameObject.Find("ctn["+(col+1)+","+(row-1)+"]");
			ctn5 = GameObject.Find("ctn["+(col-1)+","+(row)+"]");
			ctn6 = GameObject.Find("ctn["+(col-1)+","+(row-1)+"]");
		}

		if (ctn1 != null)
			if (ctn1.GetComponent<HexagonController>().getHexagon() == null)
				result.Add(ctn1);
		if (ctn2 != null)
			if (ctn2.GetComponent<HexagonController>().getHexagon() == null)
				result.Add(ctn2);
		if (ctn3 != null)
			if (ctn3.GetComponent<HexagonController>().getHexagon() == null)
				result.Add(ctn3);
		if (ctn4 != null)
			if (ctn4.GetComponent<HexagonController>().getHexagon() == null)
				result.Add(ctn4);
		if (ctn5 != null)
			if (ctn5.GetComponent<HexagonController>().getHexagon() == null)
				result.Add(ctn5);
		if (ctn6 != null)
			if (ctn6.GetComponent<HexagonController>().getHexagon() == null)
				result.Add(ctn6);
		return result;
	}

	public List<GameObject> getListRelativeHexagonLevel2(GameObject centerHexagonContainer)
	{
		Vector2 vectorPosCenterHexagonContainer = getToaDoContainer(centerHexagonContainer);
		int col = (int) (vectorPosCenterHexagonContainer.x);
		int row = (int) (vectorPosCenterHexagonContainer.y);
		
		List<GameObject> result = new List<GameObject>();
		GameObject ctn1 = null;
		GameObject ctn2 = null;
		GameObject ctn3 = null;
		GameObject ctn4 = null;
		GameObject ctn5 = null;
		GameObject ctn6 = null;
		GameObject ctn7 = null;
		GameObject ctn8 = null;
		GameObject ctn9 = null;
		GameObject ctn10 = null;
		GameObject ctn11 = null;
		GameObject ctn12 = null;
		
		ctn1 = GameObject.Find("ctn["+(col)+","+(row-2)+"]");
		ctn2 = GameObject.Find("ctn["+(col)+","+(row+2)+"]");
		if (col > 0)
		{
			ctn3 = GameObject.Find("ctn["+(col+1)+","+(row-2)+"]");
			ctn4 = GameObject.Find("ctn["+(col+1)+","+(row+1)+"]");
			ctn5 = GameObject.Find("ctn["+(col-1)+","+(row-1)+"]");
			ctn6 = GameObject.Find("ctn["+(col-1)+","+(row+2)+"]");
			ctn7 = GameObject.Find("ctn["+(col+2)+","+(row)+"]");
			ctn8 = GameObject.Find("ctn["+(col+2)+","+(row-1)+"]");
			ctn9 = GameObject.Find("ctn["+(col+2)+","+(row-2)+"]");
			if (col > 1)
			{
				ctn10 = GameObject.Find("ctn["+(col-2)+","+(row)+"]");
				ctn11 = GameObject.Find("ctn["+(col-2)+","+(row+1)+"]");
				ctn12 = GameObject.Find("ctn["+(col-2)+","+(row+2)+"]");
			}
			else //col = 1
			{
				ctn10 = GameObject.Find("ctn["+(col-2)+","+(row-1)+"]");
				ctn11 = GameObject.Find("ctn["+(col-2)+","+(row)+"]");
				ctn12 = GameObject.Find("ctn["+(col-2)+","+(row+1)+"]");
			}
		}
		else if (col == 0)
		{
			ctn3 = GameObject.Find("ctn["+(col+1)+","+(row-2)+"]");
			ctn4 = GameObject.Find("ctn["+(col+1)+","+(row+1)+"]");
			ctn5 = GameObject.Find("ctn["+(col-1)+","+(row-2)+"]");
			ctn6 = GameObject.Find("ctn["+(col-1)+","+(row+1)+"]");
			ctn7 = GameObject.Find("ctn["+(col+2)+","+(row)+"]");
			ctn8 = GameObject.Find("ctn["+(col+2)+","+(row-1)+"]");
			ctn9 = GameObject.Find("ctn["+(col+2)+","+(row-2)+"]");
			ctn10 = GameObject.Find("ctn["+(col-2)+","+(row)+"]");
			ctn11 = GameObject.Find("ctn["+(col-2)+","+(row-1)+"]");
			ctn12 = GameObject.Find("ctn["+(col-2)+","+(row-2)+"]");
		}
		else if (col < 0)
		{
			ctn3 = GameObject.Find("ctn["+(col+1)+","+(row-1)+"]");
			ctn4 = GameObject.Find("ctn["+(col+1)+","+(row+2)+"]");
			ctn5 = GameObject.Find("ctn["+(col-1)+","+(row-2)+"]");
			ctn6 = GameObject.Find("ctn["+(col-1)+","+(row+1)+"]");
			ctn10 = GameObject.Find("ctn["+(col-2)+","+(row)+"]");
			ctn11 = GameObject.Find("ctn["+(col-2)+","+(row-1)+"]");
			ctn12 = GameObject.Find("ctn["+(col-2)+","+(row-2)+"]");
			if (col < -1)
			{
				ctn7 = GameObject.Find("ctn["+(col+2)+","+(row)+"]");
				ctn8 = GameObject.Find("ctn["+(col+2)+","+(row+1)+"]");
				ctn9 = GameObject.Find("ctn["+(col+2)+","+(row+2)+"]");
			}
			else //col = -1
			{
				ctn7 = GameObject.Find("ctn["+(col+2)+","+(row-1)+"]");
				ctn8 = GameObject.Find("ctn["+(col+2)+","+(row)+"]");
				ctn9 = GameObject.Find("ctn["+(col+2)+","+(row+1)+"]");
			}
		}

		if (ctn1 != null)
			if (ctn1.GetComponent<HexagonController>().getHexagon() == null)
				result.Add(ctn1);
		if (ctn2 != null)
			if (ctn2.GetComponent<HexagonController>().getHexagon() == null)
				result.Add(ctn2);
		if (ctn3 != null)
			if (ctn3.GetComponent<HexagonController>().getHexagon() == null)
				result.Add(ctn3);
		if (ctn4 != null)
			if (ctn4.GetComponent<HexagonController>().getHexagon() == null)
				result.Add(ctn4);
		if (ctn5 != null)
			if (ctn5.GetComponent<HexagonController>().getHexagon() == null)
				result.Add(ctn5);
		if (ctn6 != null)
			if (ctn6.GetComponent<HexagonController>().getHexagon() == null)
				result.Add(ctn6);
		if (ctn7 != null)
			if (ctn7.GetComponent<HexagonController>().getHexagon() == null)
				result.Add(ctn7);
		if (ctn8 != null)
			if (ctn8.GetComponent<HexagonController>().getHexagon() == null)
				result.Add(ctn8);
		if (ctn9 != null)
			if (ctn9.GetComponent<HexagonController>().getHexagon() == null)
				result.Add(ctn9);
		if (ctn10 != null)
			if (ctn10.GetComponent<HexagonController>().getHexagon() == null)
				result.Add(ctn10);
		if (ctn11 != null)
			if (ctn11.GetComponent<HexagonController>().getHexagon() == null)
				result.Add(ctn11);
		if (ctn12 != null)
			if (ctn12.GetComponent<HexagonController>().getHexagon() == null)
				result.Add(ctn12);
		
		return result;
	}
}
