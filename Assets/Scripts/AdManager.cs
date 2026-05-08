using UnityEngine;
using UnityEngine.Advertisements;

public class AdManager : MonoBehaviour, IUnityAdsInitializationListener, IUnityAdsLoadListener, IUnityAdsShowListener
{

    [Header ("INITIALIZE ADS")]
    [SerializeField] private string _androidGameId;
    [SerializeField] private string _iOSGameId;
    [SerializeField] private bool _testMode;
    private string _gameId;

    [Header ("REWARDED ADS")]
    [SerializeField] private string _androidAdUnitId = "Rewarded_Android";
    [SerializeField] private string _iOSAdUnitId = "Rewarded_iOS";
    private string _adUnitId = null;
    private bool adLoaded = false;

    [Header ("REWARDS")]
    PlaneScript planeScript;
    public bool isRevived = false;
    public bool hasExtraFuel = false;
    GameManagerScript gameManagerScript;
    public bool hasExtraStars = false;

    public static AdManager Instance;

    private void Awake()
    {   
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        InitializeAds();
    }

    public void InitializeAds()
    {   
#if UNITY_IOS
        _gameId = _iOSGameId;
        _adUnitId = _iOSAdUnitId;
#elif UNITY_ANDROID
        _gameId = _androidGameId;
        _adUnitId = _androidAdUnitId;
#elif UNITY_EDITOR
        _gameId = _androidGameId; //Only for testing the functionality in the Editor
        _adUnitId = _androidAdUnitId; //Only for testing the functionality in the Editor
#endif

        if (!Advertisement.isInitialized && Advertisement.isSupported)
        {
            Advertisement.Initialize(_gameId, _testMode, this);
        }
    }

    // IUnityAdsInitializationListener Interfaces ....................................................................
    public void OnInitializationComplete()
    {
        Debug.Log("Unity Ads initialization complete.");
        LoadAd();
    }

    public void OnInitializationFailed(UnityAdsInitializationError error, string message)
    {
        Debug.Log($"Unity Ads Initialization Failed: {error.ToString()} - {message}");
    }
    
    //----------------------- LOAD ADS ----------------------- LOAD ADS ----------------------- LOAD ADS -----------------------
    #region Load Ads
    // Call this public method when you want to get an ad ready to show.
    public void LoadAd() // Code Manuel Written, it is not an interface
    {
        // IMPORTANT! Only load content AFTER initialization (in this example, initialization is handled in a different script).
        Debug.Log("Loading Ad: " + _adUnitId);
        Advertisement.Load(_adUnitId, this);
    }


    // IUnityAdsLoadListener Interfaces .............................................................................
    public void OnUnityAdsAdLoaded(string adUnitId)
    {
        Debug.Log("Ad Loaded: " + adUnitId);
        adLoaded = true;
    }

    public void OnUnityAdsFailedToLoad(string adUnitId, UnityAdsLoadError error, string message)
    {
        Debug.Log($"Error loading Ad Unit {adUnitId}: {error.ToString()} - {message}");
    }
    #endregion

    //----------------------- SHOW ADS ----------------------- SHOW ADS ----------------------- SHOW ADS -----------------------
    #region Show Ads
    //  Implement a method to execute when the user clicks the button:
    public void ShowAd() // Code Manuel Written, it is not an interface
    {   
        if (adLoaded)
        {
            adLoaded = false;
            // Show the ad:
            Advertisement.Show(_adUnitId, this);
        }
        else
        {
            Debug.Log("Ad not loaded yet!");
        } 
    }

    // IUnityAdsShowListener Interfaces .............................................................................
    public void OnUnityAdsShowComplete(string adUnitId, UnityAdsShowCompletionState showCompletionState)
    {
        if (adUnitId.Equals(_adUnitId) && showCompletionState.Equals(UnityAdsShowCompletionState.COMPLETED))
        {
            Debug.Log("Unity Ads Rewarded Ad Completed");
            // Grant a reward.
            GrantAReward();
            LoadAd();
        }
    }

    public void OnUnityAdsShowFailure(string adUnitId, UnityAdsShowError error, string message)
    {
        Debug.Log($"Error showing Ad Unit {adUnitId}: {error.ToString()} - {message}");
    }

    public void OnUnityAdsShowStart(string adUnitId)
    {
        // No Info for now
    }

    public void OnUnityAdsShowClick(string adUnitId)
    {
        // No Info for now
    }
    #endregion

    private void GrantAReward()
    {
        if (isRevived)
        {   
            planeScript = FindFirstObjectByType<PlaneScript>();
            planeScript.Revive();
            isRevived = false;
            Debug.Log("Rewarded with Revive!");
        }
        else if (hasExtraFuel)
        {   
            planeScript = FindFirstObjectByType<PlaneScript>();
            planeScript.setMaxFuel(50);
            hasExtraFuel = false;
            Debug.Log("Rewarded with Extra Fuel!");
        }
        else if (hasExtraStars)
        {   
            gameManagerScript = FindFirstObjectByType<GameManagerScript>();
            gameManagerScript.SetStars(3);
            hasExtraStars = false;
            Debug.Log("Rewarded with Extra Stars!");
        }
    }
}
