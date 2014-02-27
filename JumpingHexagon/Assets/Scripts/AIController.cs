using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AIController : MonoBehaviour {

	List<RouteContainer> listAvalMove;

	// Use this for initialization
	void Start () {
		listAvalMove = new List<RouteContainer>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	private void browseAllAvalaibleMove()
	{
		//declare
		int size = gameObject.GetComponent<HexagonContainerGenerator>().size;
		int maxColumnLength = size * 2 - 1;

		//clear list
		listAvalMove.Clear();

		//process
		for (int col = 1-size; col <= size-1; col++)
		{
			for (int row = 0; row < maxColumnLength - Mathf.Abs(col); row++)
			{
				GameObject ctn = GameObject.Find("ctn["+col+","+row+"]");
				//black
				if (gameObject.GetComponent<Player>().whoseContainer(ctn) == 2)
				{
					//list cap 1
					List<GameObject> listHangXom1 = ctn.GetComponent<HexagonController>().getListRelativeHexagonLevel1(ctn);
					foreach (GameObject endCtn in listHangXom1)
					{
						RouteContainer rc = new RouteContainer(ctn, endCtn, 1 + calculateExpectedPoint(endCtn));
						listAvalMove.Add(rc);

					}
					//list cap 2
					List<GameObject> listHangXom2 = ctn.GetComponent<HexagonController>().getListRelativeHexagonLevel2(ctn);
					foreach (GameObject endCtn in listHangXom2)
					{
						RouteContainer rc = new RouteContainer(ctn, endCtn, calculateExpectedPoint(endCtn));
						listAvalMove.Add(rc);
					}
				}
			}
		}
	}

	private int calculateExpectedPoint(GameObject endContainer)
	{
		int point = 0;
		List<GameObject> listHangXom = endContainer.GetComponent<HexagonController>().getHangXom(endContainer);
		foreach (GameObject ctn in listHangXom)
		{
			if (GetComponent<Player>().whoseContainer(ctn) == 1)
				point += 2;
		}
		return point;
	}

	// Trong do
	// [0] la start container
	// [1] la end container
	// [2].name la expected point
	public List<GameObject> getNextMoveOfBlack()
	{
		browseAllAvalaibleMove();
		listAvalMove.Sort(
			delegate(RouteContainer p1, RouteContainer p2) {
				return p2.expectedPoint.CompareTo(p1.expectedPoint);
			}
		);

		int countList = listAvalMove.Count;
		if (countList == 0)
			return null;
		int randomMove = Random.Range(0, countList % 3);

		List<GameObject> result = new List<GameObject>();
		result.Add(listAvalMove[randomMove].startContainer);
		result.Add(listAvalMove[randomMove].endContainer);
		GameObject expectedPts = new GameObject();
		expectedPts.name = listAvalMove[randomMove].expectedPoint.ToString();
		result.Add(expectedPts);
		return result;
	}
}

public class RouteContainer
{
	public GameObject startContainer;
	public GameObject endContainer;
	public int expectedPoint;

	public RouteContainer(GameObject p_startContainer, GameObject p_endContainer, int point)
	{
		startContainer = p_startContainer;
		endContainer = p_endContainer;
		expectedPoint = point;
	}
}