using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BillBoard : MonoBehaviour {
	[SerializeField] bool NotUseBillboard;
	void Update () {
		if(!NotUseBillboard)
		{
			Vector3 p = Camera.main.transform.position;
			p.y = transform.position.y;
			transform.LookAt (p);
		}
		
	}

	public void Active()
	{
		gameObject.SetActive(false);
	}
}