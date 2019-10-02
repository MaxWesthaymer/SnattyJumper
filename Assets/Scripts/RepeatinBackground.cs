using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;

public class RepeatinBackground : MonoBehaviour {


	[SerializeField] private GameObject[] _objects;
	[SerializeField] private float[] _scrollSpeed;
	[SerializeField] private Sprite[] _groundSprites;
	[SerializeField] private Sprite[] _rocksSprites;
	[SerializeField] private GameObject _skyBack;
	[SerializeField] private Color[] _skyColors;
	
	private List<BoxCollider2D> groundColliders = new List<BoxCollider2D>();
	private List<float> groundHorizontalLenths = new List<float>();
	private int mirrorFlag = 1;
	private Renderer _skyRend;
	
	private void Start () 
	{
		for (int i = 0; i < _objects.Length; i++)
		{
			groundColliders.Add(_objects[i].GetComponent<BoxCollider2D>());
			
			groundHorizontalLenths.Add(groundColliders[i].size.x);
		}
		_skyRend = _skyBack.GetComponent<Renderer>();
		
		SetSkyGradient();
		Setup();
	}
	

	private void Update () 
	{
		for (int i = 0; i < _objects.Length; i++)
		{
			if (_objects[i].transform.position.x < -groundHorizontalLenths[i] ||
			    _objects[i].transform.position.x > groundHorizontalLenths[i]) 
			{
				RepositionBackground (i);
			}
		}
		
		if (GameControl.instance.gameOver == false) 
		{
			for (int i = 0; i < _objects.Length; i++)
			{
				_objects[i].transform.position += new Vector3 (_scrollSpeed[i] * Time.deltaTime * mirrorFlag, 0, 0);
			}
		}
	}

	private void RepositionBackground(int i)
	{
		Vector2 groundOffset = new Vector2 (groundHorizontalLenths[i] * 2f, 0);
		_objects[i].transform.position = (Vector2)_objects[i].transform.position + groundOffset * mirrorFlag;
	}

	private void SetSkyGradient()
	{
		var num = GameControl.instance.SkyColorIndex;
		if (num < 2)
		{
			SetGradient(0, 1);
			return;
		}
		if (num < 4)
		{
			SetGradient(2, 3);
			return;
		}
		if (num < 6)
		{
			SetGradient(4, 5);
			return;
		}
		if (num < 8)
		{
			SetGradient(6, 7);
			return;
		}
	}

	private void SetGradient(int topColor, int bottomColor)
	{
		_skyRend.material.SetColor("_TopColor", _skyColors[topColor]);
		_skyRend.material.SetColor("_BottomColor", _skyColors[bottomColor]);
	}
	private void Setup()
	{
		if (GameControl.instance.loadRandonValue >= 0.5f) 
		{
			for (int i = 1; i < _objects.Length; i++)
			{
				var x = _objects[i].transform.position.x;
				var y = _objects[i].transform.position.y;
				_objects[i].transform.position = new Vector2(-x,y);
			}
			mirrorFlag = -1;
		}
		var yPos = GameControl.instance.SceneRandomValue >= 0.5 ? -4.12f : -3.7f;
		ChangeScene(GameControl.instance.SceneRandomValue >= 0.5 ? _groundSprites : _rocksSprites, yPos);
		
	}

	private void ChangeScene(Sprite[] spritePack, float yPos)
	{
		foreach (var it in _objects)
		{
			var pos = it.transform.position;
			it.transform.position = new Vector3(pos.x, yPos, pos.z);
			
			if (it.name == "Front")
			{
				it.GetComponent<SpriteRenderer>().sprite = spritePack[0];
				continue;
			}
			if (it.name == "Midle")
			{
				it.GetComponent<SpriteRenderer>().sprite = spritePack[1];
				continue;
			}
			if (it.name == "Back")
			{
				it.GetComponent<SpriteRenderer>().sprite = spritePack[2];
			}
		}
	}
	
}
