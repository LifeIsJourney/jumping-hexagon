using UnityEngine;
using System.Collections;

public class CommonVar : MonoBehaviour {

	private static bool activeTouch = true;
	//private
	private float disableTouchTime = 0.3f; //moi lan touch cach nhau 0.3s

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
