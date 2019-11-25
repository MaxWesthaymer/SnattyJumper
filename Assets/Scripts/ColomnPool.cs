using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ColomnPool : MonoBehaviour {

	#region InspectorFields		
	[SerializeField]private int _columnPoolSize = 5;
	[SerializeField]private GameObject _columnPrefab;
	[SerializeField]private GameObject _giftPrefab;
	[SerializeField]private GameObject _tubePrefab;
	[SerializeField]private GameObject _pongPrefab;
	
	
	[SerializeField]private float _spawnRate = 3f;
	[SerializeField]private float _columnMin = -1;
	[SerializeField]private float _columnMax = 3.5f;
	[SerializeField]private float _colliderPos = 0.7f;
	
	[SerializeField]private Sprite[] _columnSprites;
	#endregion
	
	#region PrivateFields		
	private GameObject[] _colums;
	private GameObject[] _tubes;
	private GameObject[] _pongs;
	private GameObject[] _gifts;
	
	private float _spawnXPosition;
	private float _giftXPosition;
	private float _timeSinceLastSpawned = 3.9f;
	private int _currentColumn = 0;
	private float _lastYPos;
	private int _currentSpawn = 0;
	private int _currentPrefab;
	private int _randomValueToSwitch;
	private bool _isFirst;
	
	private Vector2 _objectPoolPos = new Vector3 (150, 0, 0);
	#endregion
	
	#region UnityMethods	
	private void Start () 
	{
		_colums = new GameObject[_columnPoolSize];
		_isFirst = true;
		for (int i = 0; i < _columnPoolSize; i++) 
		{
			_colums [i] = Instantiate (_columnPrefab, _objectPoolPos, Quaternion.identity);
			_colums [i].gameObject.SetActive(false);
		}

		_gifts = new GameObject[_columnPoolSize];
		for (int i = 0; i < _columnPoolSize; i++) 
		{
			_gifts [i] = Instantiate (_giftPrefab, _objectPoolPos, Quaternion.identity);
			_gifts [i].gameObject.SetActive(false);
		}
		
		_tubes = new GameObject[_columnPoolSize];
		for (int i = 0; i < _columnPoolSize; i++) 
		{
			_tubes [i] = Instantiate (_tubePrefab, _objectPoolPos, Quaternion.identity);
			_tubes[i].SetActive(false);
		}
		_pongs = new GameObject[_columnPoolSize];
		for (int i = 0; i < _columnPoolSize; i++) 
		{
			_pongs [i] = Instantiate (_pongPrefab, _objectPoolPos, Quaternion.identity);
			_pongs[i].SetActive(false);
		}
		_spawnXPosition = GameControl.instance.SpawnXPosition;
		_giftXPosition = GameControl.instance.giftXPosition;
		_randomValueToSwitch = Random.Range(2, 6);
		_currentPrefab = GetNewValue(true);
		if (_currentPrefab == 0)
		{
			ChangeSprite(_colums);
		}
		if (_currentPrefab == 1)
		{
			ChangeSprite(_tubes);
		}
		if (_currentPrefab == 2)
		{
			ChangeSprite(_pongs);
		}
	}
	
	private void Update () 
	{
		SpwnColoms();
	}
	#endregion
	
	#region PrivateMethods	
	private void SpwnColoms()
	{
		_timeSinceLastSpawned += Time.deltaTime;

		if (GameControl.instance.gameOver == false && _timeSinceLastSpawned >= _spawnRate)
		{
			_timeSinceLastSpawned = 0f;
			float spawnYPosition = GetYPos();
			
			AddScore();
			
			switch (_currentPrefab)
			{
			case 0 :
				_colums[_currentColumn].transform.position = new Vector2(_spawnXPosition, spawnYPosition);
				_colums[_currentColumn].SetActive(true);
				AnimateColumns (_colums [_currentColumn]);
				break;
			case  1 :
				_tubes[_currentColumn].transform.position = new Vector2(_spawnXPosition, 0);
				_tubes[_currentColumn].SetActive(true);
				break;
			case  2 :
				_pongs[_currentColumn].transform.position = new Vector2(_spawnXPosition, 0);
				_pongs[_currentColumn].SetActive(true);
				break;				
			}			
			SpwnGift ();
			
			_currentColumn++;
			if (_currentColumn >= _columnPoolSize) 
			{
				_currentColumn = 0;
			}
			_currentSpawn++;
			if (_currentSpawn >= _randomValueToSwitch)
			{
				
				_currentSpawn = 0;
				_randomValueToSwitch = Random.Range(2, 6);
				_currentPrefab = GetNewValue();
				if (_currentPrefab == 0)
				{
					ChangeSprite(_colums);
				}
				if (_currentPrefab == 1)
				{
					ChangeSprite(_tubes);
				}
				if (_currentPrefab == 2)
				{
					ChangeSprite(_pongs);
				}
			}
		}
	}

	private float GetYPos()
	{
		var value = Random.Range(_columnMin, _columnMax);
		var difference = value - _lastYPos;
		if (Mathf.Abs(difference) < 1f)
		{
			return GetYPos();
		}
		_lastYPos = value;
		return value;
	}
	private int GetNewValue(bool isStart = false)
	{
		var value = Random.Range(0, 3);
		
		if (value == _currentPrefab && !isStart)
		{
			return GetNewValue();
		}
		return value;
	}

	private void ChangeSprite(GameObject[] obj)
	{
		var spriteIndex = Random.Range(0, 4);
		foreach (var it in obj)
		{
			var bottomColumn = it.transform.GetChild (0);
			var topColumn = it.transform.GetChild (1);
			bottomColumn.GetComponent<SpriteRenderer>().sprite = _columnSprites[spriteIndex];
			topColumn.GetComponent<SpriteRenderer>().sprite = _columnSprites[spriteIndex];
		}
	}
	private void SpwnGift()
	{
		var chance = Random.value;
		if (chance > 0.6f)
		{
			float spawnYPosition = Random.Range(_columnMin, _columnMax);
			_gifts[_currentColumn].transform.position = new Vector2(_giftXPosition, spawnYPosition);
			_gifts[_currentColumn].SetActive(true);
			_gifts[_currentColumn].transform.DOScale(new Vector3(1.2f, 1.2f, 1.2f), 0.5f).SetLoops(-1, LoopType.Yoyo);
		}
	}

	private void AddScore()
	{
		if (_isFirst)
		{
			_isFirst = false;
		}
		else
		{
			GameControl.instance.BirdScored();
		}
	}
	private void AnimateColumns(GameObject currentColumn) //todo to column class
	{
		var bottomColumn = currentColumn.transform.GetChild (0);
		var topColumn = currentColumn.transform.GetChild (1);
		float yBoundRadius = bottomColumn.GetComponent<BoxCollider2D> ().bounds.size.y / 2;
		float spawnYpos = Camera.main.orthographicSize + yBoundRadius;
		bottomColumn.position = new Vector3 (bottomColumn.position.x, -spawnYpos, bottomColumn.position.z);
		topColumn.position = new Vector3 (topColumn.position.x, spawnYpos, topColumn.position.z);
		bottomColumn.DOLocalMoveY (-4.7f, 0.5f).SetEase(Ease.OutBack);
		topColumn.DOLocalMoveY (8.7f, 0.5f).SetEase(Ease.OutBack);
	}
	#endregion
}
