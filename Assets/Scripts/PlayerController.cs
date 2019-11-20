using System;
using UnityEngine;
using DG.Tweening;

public class PlayerController : MonoBehaviour {
	#region Variables

	[SerializeField]private float upForse = 350f;
	[SerializeField]private float boundForce = 20;
	[SerializeField]private bool isDead = false;
	[SerializeField]private float rotateAngle = 20f;
	[SerializeField] private AudioClip playerDeadSnd;

	[SerializeField]private GameObject _giftExplosion;
	private ParticleSystem _particle;
	private AudioSource _giftSound;
	private Rigidbody2D rb2d;
	private Vector3 _side = Vector3.zero;
	private AudioSource _audioSource;


	#endregion

	#region PrivateMethods
	private void Start () 
	{
		rb2d = GetComponent<Rigidbody2D> ();
		Setup ();
		_particle = _giftExplosion.GetComponent<ParticleSystem> ();
		_giftSound = _giftExplosion.GetComponent<AudioSource>();
		_audioSource = GetComponent<AudioSource>();
		InvokeRepeating("StartJump", 0, 0.8f);
	}

	private void Update () 
	{
		if (GameControl.instance.isMainMenu || isDead)
		{
			return;
		}

	    JumpHandler ();
	}

	private void OnCollisionEnter2D(Collision2D other)
	{
		if (other.gameObject.tag == "Bound")
		{
			return;
		}
		
		if (other.gameObject.tag == "Floor")
		{
			rb2d.gravityScale = 0f;	
		}

		if (isDead)
		{
			return;
		}
		
		rb2d.velocity = Vector2.zero;
		isDead = true;
		GameControl.instance.BirdDied ();
		GetComponent<Animator>().Play("Dead");
		_audioSource.clip = playerDeadSnd;
		_audioSource.volume = 0.1f;
		_audioSource.Play();
	}

	private void OnTriggerEnter2D(Collider2D other) 
	{
		if (other.gameObject.CompareTag("Collectable"))
		{
			GameControl.instance.AddGiftPoint ();
			other.gameObject.SetActive (false);
			_giftExplosion.transform.position = other.transform.position;
			_particle.Play ();
			_giftSound.Play();
		}
	}
	private void Setup()
	{
		if (GameControl.instance.loadRandonValue >= 0.5f) 
		{
			rotateAngle = -rotateAngle;
			_side = new Vector3 (0, 180, 0);
			transform.localEulerAngles = _side;
			transform.position = new Vector2 (-transform.position.x, 0);
		} 
	}

	public void JumpHandler()
	{
		
		rb2d.DORotate(-rotateAngle, 0.5f);
		if (Input.GetMouseButtonDown (0)) 
		{
			rb2d.velocity = Vector2.zero;
			rb2d.AddForce (new Vector2 (0, upForse));
			GetComponent<Animator>().Play("Zoom");
			//transform.rotation = Quaternion.identity;
			transform.localEulerAngles = _side;
			_audioSource.Play();
			
			//transform.DORotate(new Vector3(0, 0, -rotateAngle), 1f, RotateMode.LocalAxisAdd);
		}
		if (Input.GetMouseButton (0)) 
		{
			rb2d.velocity += new Vector2 (0, 45 * Time.deltaTime);
			//rb2d.DORotate(rotateAngle, 0.1f);
			transform.localEulerAngles = _side;
		}
		
		if (Input.GetMouseButtonUp(0)) 
		{
			//_audioSource.Play();
		}

	}
	
	private void StartJump()
	{
		if (!GameControl.instance.isMainMenu)
		{
			CancelInvoke("StartJump");
			return;
		}
			GetComponent<Animator>().Play("Zoom");	
			//transform.DORotate(new Vector3(0, 0, -rotateAngle), 1f, RotateMode.LocalAxisAdd);
	}

	#endregion
}
