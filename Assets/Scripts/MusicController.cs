using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using  DG.Tweening;

public class MusicController : MonoBehaviour
{
    public static MusicController _instance;
    [SerializeField] private AudioSource _audioSource;
    
    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
        else if (_instance != this)
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }

    public void SetVolume(float value, float speed)
    {
        _audioSource.DOFade(value, speed);
    }
}
