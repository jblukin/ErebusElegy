using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{

    //manipulate states and add shop!

    [SerializeField] protected RuntimeData _runtimeData;

    [SerializeField] protected TextMeshProUGUI _currencyText;

    [SerializeField] protected Transform _UIRef;

    [SerializeField] protected PlayerController _playerScriptRef;

    [SerializeField] protected List<Transform> _enemySpawnPoints;

    [SerializeField] protected GameObject _enemyPrefab;

    [SerializeField] protected TextMeshProUGUI _atkUpgCostText;

    [SerializeField] protected TextMeshProUGUI _defUpgCostText;

    [SerializeField] protected TextMeshProUGUI _hpUpgCostText;

    [SerializeField] protected TextMeshProUGUI _rgeUpgCostText;

    [SerializeField] protected TextMeshProUGUI _spdUpgCostText;

    [SerializeField] protected TextMeshProUGUI _atkStatText;

    [SerializeField] protected TextMeshProUGUI _defStatText;

    [SerializeField] protected TextMeshProUGUI _hpStatText;

    [SerializeField] protected TextMeshProUGUI _rgeStatText;

    [SerializeField] protected TextMeshProUGUI _spdStatText;

    [System.NonSerialized] public int playerWealth;

    [System.NonSerialized] public int killStreak = 0; //not fully implemented

    [System.NonSerialized] public int enemyCount;

    [System.NonSerialized] public int enemyDeathCount = 0;

    protected bool _paused = false;

    protected bool _spawnAllowed = true;

    protected int _atkUpgCost;

    protected int _defUpgCost;

    protected int _hpUpgCost;

    protected int _rgeUpgCost;

    protected int _spdUpgCost;

    protected int _initialEnemyCount = 5;

    protected int _currentRound = 0;

    // Start is called before the first frame update
    void Start()
    {
        
        Cursor.lockState = CursorLockMode.Confined;

        _runtimeData.currentGameState = GameplayState.StartUp;

        _atkUpgCost = 1;

        _defUpgCost = 1;

        _hpUpgCost = 1;

        _rgeUpgCost = 1;

        _spdUpgCost = 1;

        enemyCount = _initialEnemyCount;

    }

    // Update is called once per frame
    void Update()
    {
        
        Pause();

        GameOver();

        // Debug.Log("Enemy D Count:" + enemyDeathCount);

        // Debug.Log("Intial Enemy Count:" + _initialEnemyCount);

        // Debug.Log("Enemy Spawned Count:" + enemyCount);

        if(enemyDeathCount == _initialEnemyCount) {

            _runtimeData.currentGameState = GameplayState.Shop;

        }

        if(_runtimeData.currentGameState == GameplayState.InGame) {

            spawnEnemies();

        } else if(_runtimeData.currentGameState == GameplayState.Shop) {

            _UIRef.Find("ShopUI").gameObject.SetActive(true);

            _atkUpgCostText.text = "Cost: " + _atkUpgCost;

            _defUpgCostText.text = "Cost: " + _defUpgCost;

            _hpUpgCostText.text = "Cost: " + _hpUpgCost;

            _rgeUpgCostText.text = "Cost: " + _rgeUpgCost;

            _spdUpgCostText.text = "Cost: " + _spdUpgCost;

            updateStats();

            updateCurrency();

        } else {
            
            _UIRef.Find("ShopUI").gameObject.SetActive(false);

        }

    }

    public void addCurrency() //if colliding with an orb of night, calculate currency added based on combo multiplier (Combo not to be implemented for prototype)
    { 
    
        //No multiplier currently developed - basic addition of currency
        
            string cString = "000";

            playerWealth++;

            if((playerWealth / 10) < 1) {

                cString = "00" + playerWealth;

            } else if((playerWealth / 10) < 10) {

                cString = "0" + playerWealth;

            } else {

                cString = "" + playerWealth;

            }

            _currencyText.text = cString;

    }

    public void updateCurrency() 
    {

        string cString = "000";

        if((playerWealth / 10) < 1) {

                cString = "00" + playerWealth;

            } else if((playerWealth / 10) < 10) {

                cString = "0" + playerWealth;

            } else {

                cString = "" + playerWealth;

            }

            _currencyText.text = cString;


    }

    public void startButton() //Closes TitleUI 
    {

        _UIRef.Find("TitleScreenUI").gameObject.SetActive(false);

        fixRestart();

        _runtimeData.currentGameState = GameplayState.InGame;
        
    }

    public void quitButton() //Closes Game
    {

        Application.Quit();

    }

    void Pause() //Pauses Game
    {

        if(Input.GetButtonDown("Pause")) {

            //Debug.Log("Paused");

            if(!_paused && _runtimeData.currentGameState == GameplayState.InGame) {

                _runtimeData.currentGameState = GameplayState.Paused;

                _paused = true;

                Time.timeScale = 0;

                _UIRef.Find("PauseUI").gameObject.SetActive(true);

            } else {

                _runtimeData.currentGameState = GameplayState.InGame;

                _paused = false;

                Time.timeScale = 1;

                _UIRef.Find("PauseUI").gameObject.SetActive(false);

            }

        }

    }

    void GameOver()
    {

        if(_playerScriptRef._currentHealth <= 0.0f) {

            _runtimeData.currentGameState = GameplayState.GameOver;

            Time.timeScale = 0;

            _UIRef.Find("GameOverUI").gameObject.SetActive(true);

        }

    }

    public void restartButton()
    {

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);

    }

    public void continueButton()
    {

        _UIRef.Find("PauseUI").gameObject.SetActive(false);

        _paused = false;

        _runtimeData.currentGameState = GameplayState.InGame;

        Time.timeScale = 1;

    }

    public void upgradeSpeed() 
    {

        if(_runtimeData.currentGameState == GameplayState.Shop && playerWealth >= _spdUpgCost) {
            
            _playerScriptRef._speed += 0.15f;

            playerWealth = playerWealth - _spdUpgCost;

            _spdUpgCost = _spdUpgCost * 2;

        }

    }

    public void upgradeAttack()
    {

        if(_runtimeData.currentGameState == GameplayState.Shop && playerWealth >= _atkUpgCost) { 
            
            _playerScriptRef._attackBoost += 0.25f;

            playerWealth = playerWealth - _atkUpgCost;
        
            _atkUpgCost = _atkUpgCost * 2;

       
        } 

    } 

    public void upgradeHealth()
    {

        if(_runtimeData.currentGameState == GameplayState.Shop && playerWealth >= _hpUpgCost) { 
            
            _playerScriptRef._maxHealth += 5.0f;

            playerWealth = playerWealth - _hpUpgCost;

            _hpUpgCost = _hpUpgCost * 2;

        }    
 
    }

    public void upgradeDefense() 
    {

        if(_runtimeData.currentGameState == GameplayState.Shop && playerWealth >= _defUpgCost) { 
            
            _playerScriptRef._dmgReduction += 0.25f;

            playerWealth = playerWealth - _defUpgCost;
        
            _defUpgCost = _defUpgCost * 2;

        }

    }

    public void upgradeRange()
    {

        if(_runtimeData.currentGameState == GameplayState.Shop && playerWealth >= _rgeUpgCost) { 
            
            _playerScriptRef._beamRange += 0.5f;

            playerWealth = playerWealth - _rgeUpgCost;
            
            _rgeUpgCost = _rgeUpgCost * 2;

        }   
        
    }

    void spawnEnemies() 
    {

        int spawnPoint = (int) Random.Range(0, _enemySpawnPoints.Count);

        if(enemyCount > 0) {

            if(_spawnAllowed) {

                Instantiate(_enemyPrefab, _enemySpawnPoints[spawnPoint].transform);

                StartCoroutine(spawnTimer());

            }

        }

    }

    public void setupRound() 
    {

        _currentRound++;

        _initialEnemyCount = _initialEnemyCount + _currentRound;

        enemyDeathCount = 0;

        enemyCount = _initialEnemyCount;

        _UIRef.Find("ShopUI").gameObject.SetActive(false);

        _runtimeData.currentGameState = GameplayState.InGame;

        _playerScriptRef._currentHealth = _playerScriptRef._maxHealth;

        _playerScriptRef._HPBar.maxValue = _playerScriptRef._maxHealth;

        _playerScriptRef._HPBar.value = _playerScriptRef._maxHealth;

        _playerScriptRef._HPBarText.text = _playerScriptRef._currentHealth + " / " + _playerScriptRef._maxHealth;

    }

    IEnumerator spawnTimer() 
    {
        _spawnAllowed = false;

        yield return new WaitForSeconds(3.5f);

        _spawnAllowed = true;

    }

    void updateStats() 
    {

        _atkStatText.text = "ATK: " + _playerScriptRef._attackBoost;

        _defStatText.text = "DEF: " + _playerScriptRef._dmgReduction;

        _hpStatText.text = "HP: " + _playerScriptRef._maxHealth;

        _rgeStatText.text = "RGE: " + _playerScriptRef._beamRange;

        _spdStatText.text = "SPD: " + _playerScriptRef._speed;

    }

    void fixRestart() //temp function
    {

                _runtimeData.currentGameState = GameplayState.Paused;

                _paused = true;

                Time.timeScale = 0;

                _runtimeData.currentGameState = GameplayState.InGame;

                _paused = false;

                Time.timeScale = 1;

    }

    void onSceneLoaded() 
    {

        //reset all vairables and states here!

        Cursor.lockState = CursorLockMode.Confined;

        _runtimeData.currentGameState = GameplayState.StartUp;
        
        Time.timeScale = 1;

        _atkUpgCost = 1;

        _defUpgCost = 1;

        _hpUpgCost = 1;

        _rgeUpgCost = 1;

        _spdUpgCost = 1;

        enemyCount = _initialEnemyCount;

        playerWealth = 0;

        enemyDeathCount = 0;

        _paused = false;

        _spawnAllowed = true;

        _initialEnemyCount = 5;

        _currentRound = 0;

    }

}
