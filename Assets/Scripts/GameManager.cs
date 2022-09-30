using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    private PlanetController _selectedPlanet;
    [SerializeField] private GameObject SettledPlanetUI;
    [SerializeField] private GameObject UnsettledPlanetUI;
    [SerializeField] private float stateUpdateFrequency = 10.0f;
    [SerializeField] private float uiUpdateFrequency = 1.0f;
    private GameObject _startingPlanet;
    private GameObject _planetUIInstance;
    private List<PlanetController> _planets;

    public static string CurrentScene;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            CurrentScene = SceneManager.GetActiveScene().name;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    void StartGame()
    {
        _planets = new List<PlanetController>();
        InvokeRepeating(nameof(RunLogicUpdate), 2.0f, stateUpdateFrequency);
    }

    private void RunLogicUpdate()
    {
        foreach (var planet in _planets)
        {
            if (planet.IsSettled())
            {
                planet.RunPlanetUpdates();
            }
        }
    }

    private void RunUIUpdate()
    {
        if (_planetUIInstance != null && _selectedPlanet != null)
        {
            UpdatePlanetUI();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("space") && CurrentScene == "MainMenu")
        {
            ChangeScene("MainGame");
            StartGame();
        }
        else if (Input.GetKeyDown(KeyCode.B) && CurrentScene != "MainMenu")
        {
            ChangeScene("MainMenu");
        }
    }

    public void ChangeScene(string sceneName)
    {
        CloseCurrentUI();
        CancelInvoke(nameof(RunLogicUpdate));
        _planets = null;
        CurrentScene = sceneName;
        SceneManager.LoadScene(sceneName);
    }

    public void AddPlanet(PlanetController planet)
    {
        _planets.Add(planet);
    }
    
    public void SelectPlanet(PlanetController planet)
    {
        if (_planetUIInstance != null)
        {
            CloseCurrentUI();
        }
        _selectedPlanet = planet;

        GameObject planetUI;
        if (_selectedPlanet.IsSettled())
        {
            planetUI = Instantiate(SettledPlanetUI);

        }
        else
        {
            planetUI = Instantiate(UnsettledPlanetUI);
        }
        PlanetUIController uiControllerComp = planetUI.GetComponent<PlanetUIController>();
        uiControllerComp.CustomSetup(planet);
        
        _planetUIInstance = planetUI;
        InvokeRepeating(nameof(RunUIUpdate), 0f, uiUpdateFrequency);
    }

    private void UpdatePlanetUI()
    {
        PlanetUIController uiControllerComp = _planetUIInstance.GetComponent<PlanetUIController>();
        uiControllerComp.SetUserInterfaceValues();
    }

    public void CloseCurrentUI()
    {
        CancelInvoke(nameof(RunUIUpdate));
        DestroyImmediate(_planetUIInstance);
        _selectedPlanet = null;
    }

    public GameObject GetStarterPlanet()
    {
        return _startingPlanet;
    }

    public void SetStarterPlanet(GameObject planet)
    {
        _startingPlanet = planet;
    }
}
