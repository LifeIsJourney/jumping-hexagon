using UnityEngine;
using System.Collections;

public class TouchController : MonoBehaviour {

	private int size;
	//Do dai cua Column dai nhat (column 0)
	private int maxColumnLength;

	// Update is called once per frame
	void Update () {
		if (Input.GetMouseButton(0))
		{
			if (GameObject.Find("Hexagon Background").GetComponent<Player>().playerID != 0)
			{
				GameObject nearestCnt = findNearestHexagonContainer(Input.mousePosition);
				if (nearestCnt != null)
				{
					nearestCnt.GetComponent<HexagonController>().onTouched();
				}
			}
			else
			//Kiem tra touch cho menu hien thi len
			{

			}
		}
	}

	//////////////////
	///

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
