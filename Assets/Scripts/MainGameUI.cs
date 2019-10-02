using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class MainGameUI : MonoBehaviour
{
	[SerializeField] private GameObject _startCanvas;
	[SerializeField] private Text _scoreStartScene;
	[SerializeField] private Text _giftsStartScene;
	[SerializeField] private Button _leaderBtnMenu;
	[SerializeField] private Button _soundBtn;
	[SerializeField] private Button _rateBtn;
	[SerializeField] private Sprite[] _soudSprites;
	
	[SerializeField] private GameObject _sceneCanvas;
	[SerializeField] private GameObject _recordTxt;	
	[SerializeField] private Slider _bar;
	[SerializeField] private Text _toRecordTxt;
	
	[SerializeField] private GameObject _gameOverCanvas;
	[SerializeField]private Text _gameOverScore;
	[SerializeField]private Text _gameOverBest;	
	[SerializeField]private Text _gameOverGifts;	
	[SerializeField] private Button _retryBtn;
	[SerializeField] private Button _shareBtn;
	[SerializeField] private Button _leaderGameOver;
	
	
	[SerializeField]private Text _scoreText;
	[SerializeField]private Text _giftCounter;
	
	
	[SerializeField] private GameObject _continueCanvas;
	[SerializeField]private Button _yesBtn;	
	[SerializeField] private Button _noBtn;
	[SerializeField] private Text _continueCost;
	
	
	
	
	private bool _newRecord;
	
	void Start () {
		_scoreStartScene.text = GameControl.instance._bestScore.ToString("00");
		_giftsStartScene.text = _giftCounter.text = GameControl.instance._gifts.ToString ("00");
		_leaderBtnMenu.onClick.AddListener(() =>{ GooglePlay._instance.LogIn(); });
		_soundBtn.onClick.AddListener(() => { MuteAudio(); });
		_rateBtn.onClick.AddListener(() =>{ GameControl.instance.RateUs(); });
	}

	private void Update()
	{
		BackButtonHandler();
	}

	public void StartGame()
	{
		GameControl.instance.StartGame();
		_startCanvas.SetActive (false);
		_sceneCanvas.SetActive(true);
		_scoreText.text = GameControl.instance._score.ToString ("00");
	}

	public void ChangeScore(int value)
	{
		_scoreText.text = value.ToString ("00");
		_scoreText.rectTransform.DOPunchScale(new Vector3(0.1f, 0.1f, 0.1f), 0.5f);
		if (value > GameControl.instance._bestScore && !_newRecord)
		{
			_toRecordTxt.text = "record!";
			_recordTxt.gameObject.GetComponentInChildren<ParticleSystem>().Play();
			
			StartCoroutine(ActivateShortTime(_recordTxt, 1f));
			_newRecord = true;
			
		}
	}
	
	public void ChangeGift(int value)
	{
		_giftCounter.text = value.ToString ("00");
	}
	
	public void GameOver(int score, int best, int gifts, int continueCost)
	{
		if (continueCost <= gifts && score >= 5)
		{
			ShowContinueWindow(continueCost);
		}
		else
		{
			_gameOverCanvas.SetActive(true);
		}		
		_gameOverScore.text = "Score: " + score;
	     _gameOverBest.text = "Best: " + best;
		_gameOverGifts.text = gifts.ToString("00");
		_shareBtn.onClick.AddListener(() => { GameControl.instance.ShareText();});
		_leaderGameOver.onClick.AddListener(() =>{ GooglePlay._instance.LogIn(); });
		_retryBtn.onClick.AddListener(() =>{ GameControl.instance.Reload(); });
		//_gameOverCanvas.GetComponent<Animator>().Play("ButtonUp");
	}
	
	public void ShowContinueWindow(int continueCost)
	{
		_continueCanvas.SetActive(true);
		_continueCost.text = continueCost.ToString();
		_yesBtn.onClick.AddListener(() => { GameControl.instance.ContinueReload();});
		_noBtn.onClick.AddListener(() =>{ _continueCanvas.SetActive(false); _gameOverCanvas.SetActive(true);});
		//_gameOverCanvas.GetComponent<Animator>().Play("ButtonUp");
	}
	
	private IEnumerator ActivateShortTime(GameObject go, float time)
	{
		go.SetActive(true);
		go.GetComponent<AudioSource>().Play();
		yield return new WaitForSeconds(time);
		go.SetActive(false);
	}
	
	public void ChangeBar(float value)
	{
		_bar.DOValue(value, 0.5f);
	}
		
	
	private void BackButtonHandler()
	{
		if (Input.GetKeyDown(KeyCode.Escape) && GameControl.instance.gameOver)
		{
			GameControl.instance.Reload();
			return;
		}
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			Application.Quit();
		}
		
	}
	
	public void MuteAudio()
	{		
		AudioListener.volume = AudioListener.volume < 0.1f ? 1.0f : 0.0f;
		_soundBtn.gameObject.GetComponent<Image>().sprite = AudioListener.volume < 0.1f ? _soudSprites[0] : _soudSprites[1];
	}
}
