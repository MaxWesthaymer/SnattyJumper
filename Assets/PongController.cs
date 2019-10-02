using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PongController : MonoBehaviour 
{
	#region InspectorFields		
	[SerializeField]private float _speed = 1;
	[SerializeField] private GameObject _ball;
	#endregion
	
	#region PrivateFields	
	private Transform _topPong;
	private Transform _bottomPong;
	private float _yPongRadius;
	private float _yBallRadius;
	private float _cameraBound;
	private float _topBoundary;
	private float _bottomBoundary;
	private bool _moveDown = true;
	#endregion
	
	#region UnityMethods	

	private void Start ()
	{
		Initialize();
	}
		
	private void Update ()
	{
		MoveBall();
	}

	#endregion
	
	#region PrivateMethods	
	private void Initialize()
	{
		_topPong = transform.GetChild (0);
		_bottomPong = transform.GetChild (1);
		_yPongRadius = _topPong.GetComponent<BoxCollider2D> ().bounds.size.y / 2;
		_yBallRadius = _ball.GetComponent<BoxCollider2D> ().bounds.size.y / 2;
		_cameraBound = Camera.main.orthographicSize + _yPongRadius;

		_topBoundary = _topPong.transform.position.y - _yPongRadius - _yBallRadius;
		_bottomBoundary = _bottomPong.transform.position.y + _yPongRadius + _yBallRadius;
		_ball.transform.localPosition = new Vector3(0,Random.Range(_bottomBoundary,_topBoundary),0);	
	}

	private void MoveBall()
	{
		_ball.transform.localPosition += Vector3.down * Time.deltaTime * _speed;
		if (_ball.transform.localPosition.y < _bottomBoundary && _moveDown)
		{
			_speed = -_speed;
			_moveDown = false;
		}
		if (_ball.transform.localPosition.y > _topBoundary && !_moveDown)
		{
			_speed = -_speed;
			_moveDown = true;
		}
		_ball.transform.Rotate(new Vector3(0,0,80 * Time.deltaTime), Space.Self);
	}
	#endregion
}
