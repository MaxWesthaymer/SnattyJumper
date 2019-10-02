using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollingObject : MonoBehaviour {

	private float _scrollSpeed;
	void Start()
	{
		_scrollSpeed = GameControl.instance.scrollSpeed;
	}

	void Update () 
	{
		if (GameControl.instance.gameOver == false) 
		{
			transform.position += new Vector3 (_scrollSpeed * Time.deltaTime, 0, 0);
		}
	}
}
