using UnityEngine;
using UnityEngine.Advertisements;

public class AdManager : MonoBehaviour, IUnityAdsInitializationListener, IUnityAdsLoadListener, IUnityAdsShowListener
{

    [Header ("INITIALIZE ADS")]
    [SerializeField] private string _androidGameId;
    [SerializeField] private string _iOSGameId;
    [SerializeField] private bool _testMode;
    [SerializeField] private bool _enableLogs = true;
    private string _gameId;

    [Header ("REWARDED ADS")]
    [SerializeField] private string _androidAdUnitId = "Rewarded_Android";
    [SerializeField] private string _iOSAdUnitId = "Rewarded_iOS";
    private string _adUnitId = null;
    private bool adLoaded = false;
    private bool isLoading = false;

    [Header ("REWARDS")]
    PlaneScript planeScript;
    public bool isRevived = false;
    public bool hasExtraFuel = false;
    QuestManager questManager;
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

        if (!Advertisement.isSupported)
        {
            Log("Unity Ads is not supported on this device.");
            return;
        }

        if (string.IsNullOrEmpty(_gameId) || string.IsNullOrEmpty(_adUnitId))
        {
            Log("Unity Ads Game ID or Ad Unit ID is empty.");
            return;
        }

        Log("Initializing Unity Ads. Game ID: " + _gameId + " Ad Unit ID: " + _adUnitId + " Test Mode: " + _testMode);

        if (Advertisement.isInitialized)
        {
            LoadAd();
            return;
        }

        Advertisement.Initialize(_gameId, _testMode, this);
    }

    // IUnityAdsInitializationListener Interfaces ....................................................................
    public void OnInitializationComplete()
    {
        Log("Unity Ads initialization complete.");
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
        if (!Advertisement.isInitialized)
        {
            Log("Unity Ads is not initialized yet. LoadAd skipped.");
            return;
        }

        if (isLoading || adLoaded)
        {
            return;
        }

        isLoading = true;
        Log("Loading Ad: " + _adUnitId);
        Advertisement.Load(_adUnitId, this);
    }


    // IUnityAdsLoadListener Interfaces .............................................................................
    public void OnUnityAdsAdLoaded(string adUnitId)
    {
        if (!adUnitId.Equals(_adUnitId))
        {
            return;
        }

        Log("Ad Loaded: " + adUnitId);
        isLoading = false;
        adLoaded = true;
    }

    public void OnUnityAdsFailedToLoad(string adUnitId, UnityAdsLoadError error, string message)
    {
        isLoading = false;
        adLoaded = false;
        Debug.Log($"Error loading Ad Unit {adUnitId}: {error.ToString()} - {message}");
        Invoke(nameof(LoadAd), 5f);
    }
    #endregion

    //----------------------- SHOW ADS ----------------------- SHOW ADS ----------------------- SHOW ADS -----------------------
    #region Show Ads
    //  Implement a method to execute when the user clicks the button:
    public void ShowAd() // Code Manuel Written, it is not an interface
    {   
        if (!Advertisement.isInitialized)
        {
            Log("Ad is not initialized yet!");
            InitializeAds();
            return;
        }

        if (adLoaded)
        {
            adLoaded = false;
            Log("Showing Ad: " + _adUnitId);
            Advertisement.Show(_adUnitId, this);
        }
        else
        {
            Debug.Log("Ad not loaded yet!");
            LoadAd();
        } 
    }

    // IUnityAdsShowListener Interfaces .............................................................................
    public void OnUnityAdsShowComplete(string adUnitId, UnityAdsShowCompletionState showCompletionState)
    {
        if (adUnitId.Equals(_adUnitId) && showCompletionState.Equals(UnityAdsShowCompletionState.COMPLETED))
        {
            //Debug.Log("Unity Ads Rewarded Ad Completed");
            // Grant a reward.
            GrantAReward();
        }

        LoadAd();
    }

    public void OnUnityAdsShowFailure(string adUnitId, UnityAdsShowError error, string message)
    {
        adLoaded = false;
        Debug.Log($"Error showing Ad Unit {adUnitId}: {error.ToString()} - {message}");
        Invoke(nameof(LoadAd), 5f);
    }

    public void OnUnityAdsShowStart(string adUnitId)
    {
        Log("Ad started: " + adUnitId);
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
            if (planeScript != null)
            {
                planeScript.Revive();
            }

            isRevived = false;
            //Debug.Log("Rewarded with Revive!");
        }
        else if (hasExtraFuel)
        {   
            planeScript = FindFirstObjectByType<PlaneScript>();
            if (planeScript != null)
            {
                planeScript.setMaxFuel(50);
            }

            hasExtraFuel = false;
            //Debug.Log("Rewarded with Extra Fuel!");
        }
        else if (hasExtraStars)
        {   
            questManager = FindFirstObjectByType<QuestManager>();
            if (questManager != null)
            {
                questManager.AddStars(6);
            }

            hasExtraStars = false;
            //Debug.Log("Rewarded with Extra Stars!");
        }
    }

    private void Log(string message)
    {
        if (_enableLogs)
        {
            Debug.Log("[AdManager] " + message);
        }
    }
}
