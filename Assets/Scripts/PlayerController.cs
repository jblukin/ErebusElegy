using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerController : MonoBehaviour
{

    [SerializeField] RuntimeData _runtimeData;

    protected float _mSense = 10f;

    protected float _gravity = 4.9f;

    protected float _vertVelo = 0.0f;

    protected float _tilt = 0f;

    protected float _timer;

    [System.NonSerialized] public float _currentHealth;

    [System.NonSerialized] public float _maxHealth = 100.0f;

    [System.NonSerialized] public float _speed = 8.5f;

    [System.NonSerialized] public float _beamRange = 10.0f;

    [System.NonSerialized] public float _dmgReduction = 0.0f;

    [System.NonSerialized] public float _attackBoost = 0.0f;

    [SerializeField] protected Transform _handTransform;

    [SerializeField] protected LineRenderer _magicBeam;

    [SerializeField] protected Camera _camera;

    [SerializeField] protected Material[] _matsArray;

    [SerializeField] protected EnemyController _enemyRef;

    [SerializeField] protected GameManager GameManager;

    [SerializeField] public Slider _HPBar;

    [SerializeField] public TextMeshProUGUI _HPBarText;

    void Start()
    {

        _maxHealth = 100.0f;

        _speed = 8.5f;

        _beamRange = 15f;

        _dmgReduction = 0.0f;
        
        _attackBoost = 0.0f;

        _runtimeData.currentPlayerMagic = PlayerMagicState.Fire;

        _currentHealth = _maxHealth;

        _HPBar.value = _maxHealth;

        _HPBarText.text = _currentHealth + " / " + _maxHealth;

    }

    // Update is called once per frame
    void Update()
    {

        if(_runtimeData.currentGameState == GameplayState.InGame) {

        Cursor.lockState = CursorLockMode.Locked;

        Aim();

        Shoot();

        Movement();

        } else {

            Cursor.lockState = CursorLockMode.Confined;

        }

        if(_runtimeData.currentGameState == GameplayState.Shop) {

            _HPBar.maxValue = _maxHealth;

            _currentHealth = _maxHealth;

            _HPBar.value = _currentHealth;

            _HPBarText.text = _HPBar.value + " / " + _maxHealth;

        }

    }

    void Aim() //Player camera movment function
    {

        float mx = Input.GetAxis("Mouse X");
        float my = Input.GetAxis("Mouse Y");

        transform.Rotate(Vector3.up, mx * _mSense);

        _tilt -= my * _mSense;
        _tilt = Mathf.Clamp(_tilt, -90, 90);

        _camera.transform.localEulerAngles = new Vector3(_tilt, 0, 0);

    }

    void Movement() //Player positional movement function
    {

        Vector3 mH = transform.right * Input.GetAxis("Horizontal");
        Vector3 mV = transform.forward * Input.GetAxis("Vertical");

        if(GetComponent<CharacterController>().isGrounded) { //check whether or not to apply gravity

            _vertVelo = 0f;

        }

        Vector3 move = mH + mV;

        _vertVelo -= _gravity * Time.deltaTime;

        move.y = _vertVelo;

        if(_runtimeData.currentPlayerMagic == PlayerMagicState.Ice) { //increases movement speed when player is using Ice magic

            GetComponent<CharacterController>().Move(move * (_speed + 1.5f) * Time.deltaTime);

        } else { //standard movement speed calculation

            GetComponent<CharacterController>().Move(move * _speed * Time.deltaTime);

        }

       
    }

    void Shoot() //Player shooting function
    {

        getCurrentBeamColor();

        //float beamRange = 10.0f;
        float fireRate = 0.25f;
        _timer += Time.deltaTime;

        if(_runtimeData.currentPlayerMagic == PlayerMagicState.Lightning) { //increase fire rate when player is using lightning magic

            fireRate = 0.1f;

        }

        if(Input.GetMouseButtonDown(0) && fireRate < _timer) {

            _timer = 0;

            Vector3 beam = _camera.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0f));

            RaycastHit hit;

           // _magicBeam.SetPosition(0, _handTransform.position);

            if(Physics.Raycast(beam, _camera.transform.forward, out hit, _beamRange)) {

                _magicBeam.SetPosition(1, new Vector3(0.0f, 0.0f, _beamRange)); //set to Z distances

                if(hit.transform.gameObject.tag == "Destroyable") {

                    hit.transform.gameObject.GetComponent<EnemyController>().calculateDamage();
                    
                    if(hit.transform.gameObject.GetComponent<EnemyController>().health <= 0) {

                        Destroy(hit.transform.gameObject);

                        hit.transform.gameObject.GetComponent<EnemyController>().orbDropOnDeath();

                        GameManager.killStreak++;

                        GameManager.enemyDeathCount++;


                    }
                    
                }

            } else {

                _magicBeam.SetPosition(1, new Vector3(0.0f, 0.0f, _beamRange));

            }

            StartCoroutine(beamTimer());

        }

    
    }

    void OnCollisionEnter(Collision collision) //Checks for all collisions
    {

        calculatePersonalDamage(collision.collider);

        checkPadCollision(collision.collider);

        if(collision.collider.tag == "Currency") {

            GameManager.addCurrency();

            Destroy(collision.collider.transform.gameObject);
            
        }   

    }

    IEnumerator beamTimer() //Simple timer for time magic beam is visible
    {

        _magicBeam.enabled = true;
        yield return new WaitForSeconds(0.05f);
        _magicBeam.enabled = false;

    }

    void getCurrentBeamColor() //Checks current magic state and sets beam to material of that magic type
    {

        if(_runtimeData.currentPlayerMagic == PlayerMagicState.Fire) {

            _magicBeam.material = _matsArray[0];

        } else if(_runtimeData.currentPlayerMagic == PlayerMagicState.Ice) {

            _magicBeam.material = _matsArray[1];

        } else if(_runtimeData.currentPlayerMagic == PlayerMagicState.Lightning) {

            _magicBeam.material = _matsArray[2];  

        } else if(_runtimeData.currentPlayerMagic == PlayerMagicState.Earth) {

            _magicBeam.material = _matsArray[3];

        }

    }

    void calculatePersonalDamage(Collider collider) 
    {

        if(_runtimeData.currentPlayerMagic == PlayerMagicState.Earth && collider.gameObject.tag == "Destroyable") { //increases defense when player is using earth magic

            _currentHealth -= 2.5f;

            GameManager.killStreak = 0;

            _HPBar.value = _currentHealth;

            _HPBarText.text = _currentHealth + " / " + _maxHealth;
            

        } else if(collider.gameObject.tag == "Destroyable" && (collider.gameObject.GetComponent<EnemyController>()._magicAttribute == 0 
        || collider.gameObject.GetComponent<EnemyController>()._magicAttribute == 2)) {

            _currentHealth -= 7.5f;

            GameManager.killStreak = 0;

            _HPBar.value = _currentHealth;

            _HPBarText.text = _currentHealth + " / " + _maxHealth;


        } else if(collider.gameObject.tag == "Destroyable") {

            _currentHealth -= 5.0f;

            GameManager.killStreak = 0;

            _HPBar.value = _currentHealth;

            _HPBarText.text = _currentHealth + " / " + _maxHealth;

        }


    }

    void checkPadCollision(Collider collider) 
    { //if colliding with a magic pad, changes player's magic type
    
        if(collider.gameObject.tag == "FirePad") {

            _runtimeData.currentPlayerMagic = PlayerMagicState.Fire;

        } else if(collider.gameObject.tag == "IcePad") {

            _runtimeData.currentPlayerMagic = PlayerMagicState.Ice;

        } else if(collider.gameObject.tag == "LightningPad") {

            _runtimeData.currentPlayerMagic = PlayerMagicState.Lightning;

        } else if(collider.gameObject.tag == "EarthPad") {

            _runtimeData.currentPlayerMagic = PlayerMagicState.Earth;

        }

    }

    void OnSceneLoaded() 
    {

        _maxHealth = 100.0f;

        _speed = 8.5f;

        _beamRange = 15f;

        _dmgReduction = 0.0f;
        
        _attackBoost = 0.0f;

        _runtimeData.currentPlayerMagic = PlayerMagicState.Fire;

        _currentHealth = _maxHealth;

        _HPBar.value = _currentHealth;

        _HPBarText.text = _currentHealth + " / " + _maxHealth;

    }

}
