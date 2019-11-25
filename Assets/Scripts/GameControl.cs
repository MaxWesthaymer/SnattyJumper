using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;


public class GameControl : MonoBehaviour {
	#region PublicFields
	public static GameControl instance;
	public bool gameOver;
	public bool isMainMenu;	
	public float scrollSpeed = -3f;
	public float giftXPosition = 4f;
	public GameObject _player;
	[SerializeField] private  MainGameUI _mainGameUi;
	[SerializeField] private AudioSource music;
	#endregion

	#region Propierties
	public float SpawnXPosition { get; set; }
	public int _score { get; set; }
	public int _gifts { get; set; }
	public int _bestScore { get; set; }
	public int SkyColorIndex { get; set; }
	public float loadRandonValue { get; set;}
	public float SceneRandomValue { get; set;}
	#endregion
	
	#region PrivateFields
	private ShareAndRate _shareAndRate;
	private bool _isContinue;
	private int _isContinueFlag;
	private int _continueCost;
	private ColomnPool colomnPool;
	#endregion
	

	#region UnityMethods	
	void Awake () 
	{	
		if (instance == null)	
			instance = this;
		
		else if(instance != this)
			Destroy (gameObject);

		colomnPool = GetComponent<ColomnPool> ();
		
		Load();
		if (_isContinue)
		{
			loadRandonValue = PlayerPrefs.GetFloat("loadrandom", 0.1f);
			SceneRandomValue = PlayerPrefs.GetFloat("scenerandom", 0.1f);
			SkyColorIndex = PlayerPrefs.GetInt("skycolor", 0);
			_score = PlayerPrefs.GetInt("currentscore", 0);
		}
		else
		{
			loadRandonValue = Random.value;
			SceneRandomValue = Random.value;
			SkyColorIndex = Random.Range(0, 8);
		}		
	}

	void Start()
	{
		Application.targetFrameRate = 60;
		_shareAndRate = GetComponent<ShareAndRate>();
		_player.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
		isMainMenu = true;
		colomnPool.enabled = false;
		SpawnXPosition = Camera.main.aspect * Camera.main.orthographicSize + 1f;	
		MusicController._instance.SetVolume(0.4f, 0.5f);
		Setup ();
		PurposeChanger();
		if (_isContinue)
		{
			_mainGameUi.StartGame();
		}
	}

	private void OnApplicationQuit()
	{
	
	}

	#endregion

	#region PrivateMethods
	void Setup()
	{
		if (loadRandonValue >= 0.5) 
		{
			SpawnXPosition = -SpawnXPosition;
			giftXPosition = -giftXPosition;
			scrollSpeed = -scrollSpeed;
		}
	}

	#endregion

	#region PublicMethods
	public void BirdScored()
	{
		if (gameOver) 
		{
			return;
		}
		_score++;	
		_mainGameUi.ChangeScore(_score);	
		PurposeChanger();	
	}

	public void AddGiftPoint()
	{
		if (gameOver) 
		{
			return;
		}
		_gifts++;
		_mainGameUi.ChangeGift(_gifts);
	}

	public void BirdDied()
	{
		if (gameOver)
		{
			return;
		}

		_continueCost = _isContinue ? 100 : 20;
		_mainGameUi.GameOver(_score,_bestScore,_gifts, _continueCost);
		gameOver = true;
		MusicController._instance.SetVolume(0.05f, 0.5f);
		Save();
	}

	public void StartGame()
	{		
		_player.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
		isMainMenu = false;
		colomnPool.enabled = true;	
	}

	private void Save()
	{
		if (_score > _bestScore)
		{
			_bestScore = _score;
			PlayerPrefs.SetInt("Score", _bestScore);
		}
		PlayerPrefs.SetInt("gifts",_gifts);		
		PlayerPrefs.SetFloat("loadrandom", loadRandonValue);
		PlayerPrefs.SetFloat("scenerandom", SceneRandomValue);
		PlayerPrefs.SetInt("skycolor", SkyColorIndex);	
	}

	public void ContinueReload()
	{
		_gifts -= _continueCost;
		PlayerPrefs.SetInt("gifts",_gifts);	
		PlayerPrefs.SetInt("currentscore", _score);
		PlayerPrefs.SetInt("isContinue", 1);
		SceneManager.LoadScene (SceneManager.GetActiveScene ().buildIndex);
		if (_score <= 3)
		{
			return;
		}
		AdMobController._instance.GameOver();
	}
	
	public void Reload()
	{
		PlayerPrefs.SetInt("isContinue", 0);
		SceneManager.LoadScene (SceneManager.GetActiveScene ().buildIndex);
		if (_score <= 3)
		{
			return;
		}
		AdMobController._instance.GameOver();
	}	
	
	private void Load()
	{
		_bestScore = PlayerPrefs.GetInt("Score", 0);
		_gifts = PlayerPrefs.GetInt("gifts", 0);
		_isContinueFlag = PlayerPrefs.GetInt("isContinue", 0);
		_isContinue = _isContinueFlag > 0;
		if (_isContinue)
		{
			PlayerPrefs.DeleteKey("isContinue");
		}
	}

	private void PurposeChanger()
	{
		float i = _score / ((float)_bestScore + 1f);
		_mainGameUi.ChangeBar(i);
	}
	
	public void ShareText()
	{
		_shareAndRate.OnAndroidTextSharingClick();
	}
	
	public void RateUs()
	{
		_shareAndRate.RateUs();
	}
	#endregion
}


