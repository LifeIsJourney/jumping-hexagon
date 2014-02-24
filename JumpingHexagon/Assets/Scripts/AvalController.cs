using UnityEngine;
using System.Collections;

public class AvalController : MonoBehaviour {

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void onFinishedAnimAvalLevel1()
	{
		Destroy(this.gameObject);
	}

	void onFinishedAnimAvalLevel2()
	{
		Destroy(this.gameObject);
	}

	void onFinishGrowAvalLevel1()
	{
		this.gameObject.GetComponent<Animator>().SetBool("Grow",false);
	}

	void onFinishGrowAvalLevel2()
	{
		this.gameObject.GetComponent<Animator>().SetBool("Grow",false);
	}
}
