using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HexagonContainerGenerator : MonoBehaviour {

	//Do dai canh cua Hexagon
	public int size;
	public GameObject containerPrefab;
	public GameObject hexagonVioletPrefab;
	public GameObject hexagonBlackPrefab;
	public float updatePointPeriod = 0.2f;

	public int violetPointNow = 3;
	public int blackPointNow = 3;

	public GameObject violetWinPrefab;
	public GameObject blackWinPrefab;
	public GameObject drawPrefab;
	
	private static bool isGenerated = false;
	private int maxColumnLength;
	private float nextActionTime = 0f;

//TEMP
	//moc cua hexagon cao nhat ung voi canh = 5
	private float mocX = 0f;
	private float moc5Y = 3.425563f;
	private float moc6Y = 4.274875f;


	private float offsetChieuDoc = -0.850359f;
	private float offsetChieuNgang = 0.7502445f;
	private float offsetCheo = -0.42518f;

	// Use this for initialization
	void Start () {
		generatorHexagonContainers();
		initSomeHexagon();
	}

	void Update() {
		if (Time.time > nextActionTime)
		{
			nextActionTime += updatePointPeriod;
			updateGamePoint();
		}
	}

	private void generatorHexagonContainers()
	{
		if (!isGenerated)
		{
			//Do dai cua Column dai nhat (column 0)
			maxColumnLength = size * 2 - 1;
			float mocY = moc5Y;
			switch (size)
			{
				case 5:
				{
					mocY = moc5Y;
					break;
				}
				case 6:
				{
					mocY = moc6Y;
					break;
				}
			}

			for (int col = 1-size; col <= size-1; col++)
			{
				for (int row = 0; row < maxColumnLength - Mathf.Abs(col); row++)
				{
					GameObject container = (GameObject) (Instantiate(containerPrefab, 
                 		new Vector3(mocX + offsetChieuNgang * col, mocY + offsetChieuDoc * row + offsetCheo * Mathf.Abs(col),containerPrefab.transform.position.z), 
                    		containerPrefab.transform.rotation));
					container.name = "ctn["+col+","+row+"]";
				}
			}
		}
		isGenerated = true;
	}

	// Tao mot vai hexagon ban dau
	private void initSomeHexagon()
	{
		//3 Violet
		GameObject hexaViolet1 = (GameObject) (Instantiate(hexagonVioletPrefab, hexagonVioletPrefab.transform.position, hexagonVioletPrefab.transform.rotation));
		GameObject.Find("ctn[0,0]").GetComponent<HexagonController>().addHexagon(hexaViolet1);

		GameObject hexaViolet2 = (GameObject) (Instantiate(hexagonVioletPrefab, hexagonVioletPrefab.transform.position, hexagonVioletPrefab.transform.rotation));
		GameObject.Find("ctn["+(size-1)+","+(size-1)+"]").GetComponent<HexagonController>().addHexagon(hexaViolet2);

		GameObject hexaViolet3 = (GameObject) (Instantiate(hexagonVioletPrefab, hexagonVioletPrefab.transform.position, hexagonVioletPrefab.transform.rotation));
		GameObject.Find("ctn["+(1-size)+","+(size-1)+"]").GetComponent<HexagonController>().addHexagon(hexaViolet3);

		//3 Blacks
		GameObject hexaBlack1 = (GameObject) (Instantiate(hexagonBlackPrefab, hexagonBlackPrefab.transform.position, hexagonBlackPrefab.transform.rotation));
		GameObject.Find("ctn[0,"+(size * 2 - 2)+"]").GetComponent<HexagonController>().addHexagon(hexaBlack1);
		
		GameObject hexaBlack2 = (GameObject) (Instantiate(hexagonBlackPrefab, hexagonBlackPrefab.transform.position, hexagonBlackPrefab.transform.rotation));
		GameObject.Find("ctn["+(size-1)+",0]").GetComponent<HexagonController>().addHexagon(hexaBlack2);
		
		GameObject hexaBlack3 = (GameObject) (Instantiate(hexagonBlackPrefab, hexagonBlackPrefab.transform.position, hexagonBlackPrefab.transform.rotation));
		GameObject.Find("ctn["+(1-size)+",0]").GetComponent<HexagonController>().addHexagon(hexaBlack3);
	}

	public void updateGamePoint()
	{	
		int blackPoint = 0;
		int violetPoint = 0;
		int playerID = gameObject.GetComponent<Player>().playerID;

		for (int col = 1-size; col <= size-1; col++)
			for (int row = 0; row < maxColumnLength - Mathf.Abs(col); row++)
			{
				GameObject ctn = GameObject.Find("ctn["+col+","+row+"]");
				if (ctn != null)
				{
					GameObject hexagon = ctn.GetComponent<HexagonController>().getHexagon();
					if (hexagon != null)
					{
						if (hexagon.gameObject.name.Contains("HexagonViolet"))
							violetPoint ++;
						else
							blackPoint++;
					}
				}
			}
		violetPointNow = violetPoint;
		blackPointNow = blackPoint;
		GameObject.Find("BlackPoint").GetComponent<TextMesh>().text = "" + blackPoint;
		GameObject.Find("VioletPoint").GetComponent<TextMesh>().text = "" + violetPoint;

		//Check win
		int winPlayerID = isGameOver();

		if (playerID == 0)
			return;

		if (winPlayerID == 1)
		{
			gameObject.GetComponent<Player>().playerID = 0;
			//violet win
			GameObject.Find("turnBlack").GetComponent<Animator>().SetBool("isOff",true);
			Instantiate(violetWinPrefab, violetWinPrefab.transform.position, violetWinPrefab.transform.rotation);
			return;
		}
		if (winPlayerID == 2)
		{
			gameObject.GetComponent<Player>().playerID = 0;
			//black win
			GameObject.Find("turnViolet").GetComponent<Animator>().SetBool("isOff",true);
			Instantiate(blackWinPrefab, blackWinPrefab.transform.position, blackWinPrefab.transform.rotation);
			return;
		}
		
		//Hoa CHUA LAM
		if (winPlayerID == 3)
		{
			gameObject.GetComponent<Player>().playerID = 0;
			if (playerID == 1)
				GameObject.Find("turnViolet").GetComponent<Animator>().SetBool("isOff",true);
			else if (playerID == 2)
				GameObject.Find("turnBlack").GetComponent<Animator>().SetBool("isOff",true);
			Instantiate(drawPrefab, drawPrefab.transform.position, drawPrefab.transform.rotation);
			playerID = 0;
			return;
		}
	}

	// Tra ve ID nguoi thang
	public int isGameOver()
	{
		int size = GameObject.Find("Hexagon Background").GetComponent<HexagonContainerGenerator>().size;
		int maxColumnLength = size * 2 - 1;
		bool hasAvalaibleMoveVioletPlayer = false;
		bool hasAvalaibleMoveBlackPlayer = false;
		int playerID = gameObject.GetComponent<Player>().playerID;

		for (int col = 1-size; col <= size-1; col++)
		{
			for (int row = 0; row < maxColumnLength - Mathf.Abs(col); row++)
			{
				GameObject ctn = GameObject.Find("ctn["+col+","+row+"]");
				HexagonController controller = ctn.GetComponent<HexagonController>();
				GameObject hexagon = controller.getHexagon();
				
				if (hasAvalaibleMoveBlackPlayer && hasAvalaibleMoveVioletPlayer)
					break;
				
				//Neu o do khong co hexagon thi bo qua
				if (hexagon == null)
					continue;
				else
				{
					if (hexagon.name.Contains("HexagonViolet"))
					{
						if (hasAvalaibleMoveVioletPlayer)
							continue;
						if (controller.getListRelativeHexagonLevel1(ctn).Count > 0)
						{
							hasAvalaibleMoveVioletPlayer = true;
							continue;
						}
						if (controller.getListRelativeHexagonLevel2(ctn).Count > 0)
						{
							hasAvalaibleMoveVioletPlayer = true;
							continue;
						}
					}
					
					if (hexagon.name.Contains("HexagonBlack"))
					{
						if (hasAvalaibleMoveBlackPlayer)
							continue;
						if (controller.getListRelativeHexagonLevel1(ctn).Count > 0)
						{
							hasAvalaibleMoveBlackPlayer = true;
							continue;
						}
						if (controller.getListRelativeHexagonLevel2(ctn).Count > 0)
						{
							hasAvalaibleMoveBlackPlayer = true;
							continue;
						}
					}
				}
			}
		}

		
		if ((!hasAvalaibleMoveVioletPlayer && playerID == 1) || (!hasAvalaibleMoveBlackPlayer && playerID == 2))
		{
			//Tinh diem
			if (violetPointNow > blackPointNow)
				return 1;
			else if (violetPointNow == blackPointNow)
				return 3;
			else if (blackPointNow > violetPointNow)
				return 2;
		}
		
		return 0;
	}
}
