using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudsController : MonoBehaviour {

	private void Start ()
	{
		Setup();
	}
	
	private void Setup()
	{
		if (GameControl.instance.loadRandonValue >= 0.5f) 
		{
			transform.position = new Vector2 (-transform.position.x, transform.position.y);
			var ctrl = GetComponentsInChildren<ParticleSystem>();
			foreach (var it in ctrl)
			{
				it.Stop();
				it.Clear();
				var speed = it.main.startSpeed.constant;
				var main = it.main;
				main.startSpeed = new ParticleSystem.MinMaxCurve(-speed);
				main.prewarm = true;
				it.Play();
			}
		} 
	}
}
