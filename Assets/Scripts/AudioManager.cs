using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] AudioClip[] audioClips;
    static AudioManager instance;
    private AudioSource myAudioSource;

    void Awake()
    {
        ManageSingleton();
    }

    void ManageSingleton()
    {
        
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {   
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    void Start()
    {
        myAudioSource = GetComponent<AudioSource>();
    }

    public void PlayFlapSFX()
    {
        myAudioSource.PlayOneShot(audioClips[0]);
    }

    public void PlayImpactSFX()
    {
        myAudioSource.PlayOneShot(audioClips[1]);
        Invoke ("PlayFallingSFX", 0.3f);
    }

    public void PlayFallingSFX()
    {
        myAudioSource.PlayOneShot(audioClips[2]);
    }

    public void PlayExplosionSFX()
    {
        myAudioSource.PlayOneShot(audioClips[3]);
    }

    public void PlayCollectFuelSFX()
    {
        myAudioSource.PlayOneShot(audioClips[4]);
    }

    public void PlayEnemyHitSFX()
    {
        myAudioSource.PlayOneShot(audioClips[5]);
    }

    public void PlaySwooshSFX()
    {
        myAudioSource.PlayOneShot(audioClips[6]);
    }

    public void PlayQuestCompletedSFX()
    {
        myAudioSource.PlayOneShot(audioClips[7]);
    }




}
