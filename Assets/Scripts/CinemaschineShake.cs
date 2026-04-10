using Unity.Cinemachine;
using UnityEngine;

public class CinemaschineShake : MonoBehaviour
{
    
    private CinemachineCamera cinemachineCamera;
    private float shakeTimer;
    private float shakeTimeTotal;
    private float startingIntensity;


    private void Awake()
    {
        cinemachineCamera = GetComponent<CinemachineCamera>();
    } 

    public void ShakeCamera (float intensity, float time)
    {
        CinemachineBasicMultiChannelPerlin cinemachineBasicMultiChannelPerlin =
            cinemachineCamera.GetComponent<CinemachineBasicMultiChannelPerlin>();

        cinemachineBasicMultiChannelPerlin.AmplitudeGain = intensity;
        shakeTimer = time;
        startingIntensity = intensity;
        shakeTimeTotal = time;
    }

    private void Update()
    {
        if (shakeTimer > 0)
        {
            shakeTimer -= Time.deltaTime;
            CinemachineBasicMultiChannelPerlin cinemachineBasicMultiChannelPerlin =
                cinemachineCamera.GetComponent<CinemachineBasicMultiChannelPerlin>();

            cinemachineBasicMultiChannelPerlin.AmplitudeGain = Mathf.Lerp(startingIntensity, 0f, (1- (shakeTimer / shakeTimeTotal)));
        }
    }

}
