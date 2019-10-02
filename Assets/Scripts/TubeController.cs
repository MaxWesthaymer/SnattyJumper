using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class TubeController : MonoBehaviour 
{
	#region InspectorFields		
	[SerializeField]private float _speed = 1;
	[SerializeField]private float _distance = 1;
	#endregion
	
	#region PrivateFields	
	private Transform _topTube;
	private Transform _bottomTube;
	private float _yBoundRadius;
	private float _yBound;
	private float _cameraBound;
	#endregion
	
	#region UnityMethods	
	private void Start ()
	{
		Initialize();
	}
		
	private void Update ()
	{
		MoveTubes();
	}

	#endregion
	
	#region PrivateMethods	
	private void Initialize()
	{
		_topTube = transform.GetChild (0);
		_bottomTube = transform.GetChild (1);
		_yBoundRadius = _topTube.GetComponent<BoxCollider2D> ().bounds.size.y / 2;
		_yBound = _topTube.GetComponent<BoxCollider2D> ().bounds.size.y;
		_cameraBound = Camera.main.orthographicSize + _yBoundRadius;
		var random = Random.value;
		_speed = random > 0.5f ? -1 : 1;
	}

	private void MoveTubes()
	{
		_topTube.localPosition += Vector3.down * Time.deltaTime * _speed;
		_bottomTube.localPosition += Vector3.down * Time.deltaTime * _speed;
		
		if (_topTube.localPosition.y > _cameraBound)
		{
			_speed = -_speed;
		}
		if (_bottomTube.localPosition.y < -_cameraBound)
		{
			//var newYpos = _topTube.position.y + _yBound + _distance;
			//_bottomTube.localPosition = new Vector3(0, newYpos ,0);
			_speed = -_speed;
		}
	}
	#endregion
}
