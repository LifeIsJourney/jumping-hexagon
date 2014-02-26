using UnityEngine;
using System.Collections;

public class CommonVar : MonoBehaviour {

	private static bool activeTouch = true;
	//public
	public float disableTouchTime = 0.1f; //moi lan touch cach nhau 0.3s
	public bool isMusicOn = true;

	void Start()
	{
		//get preferences MUSIC
		int temp = PlayerPrefs.GetInt("MusicOff");
		if (temp == 0)
		{
			isMusicOn = true;
			AudioListener.volume = 1;
		}
		else
		{
			isMusicOn = false;
			AudioListener.volume = 0;
		}
	}

	private IEnumerator enableTouchAfterSeconds(float second)
	{
		yield return new WaitForSeconds(second);
		activeTouch = true;
	}

	public bool isEnablingTouch()
	{
		if (activeTouch)
		{
			activeTouch = false;
			StartCoroutine(enableTouchAfterSeconds(disableTouchTime));
			return true;
		}
		else
			return false;
	}
}
