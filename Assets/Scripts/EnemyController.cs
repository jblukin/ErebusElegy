using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EnemyController : MonoBehaviour
{

    [SerializeField] protected RuntimeData _runtimeData;

    [SerializeField] protected Material[] _matsArray = new Material[4];

    [SerializeField] protected GameObject _currencyPrefab;

    protected GameObject _playerRef;

    [SerializeField] protected GameObject _DMGNumPrefab;

    protected GameManager GameManager;

    protected Transform _DMGNumTransform;

    [System.NonSerialized] public float health;

    [System.NonSerialized] public int _magicAttribute = 0; //0 is fire, 1 is ice, 2 is lightning, 3 is earth

    protected float _gravity = 4.9f;

    protected float _vertVelo = 0.0f;

    // Start is called before the first frame update
    void Start()
    {

        _magicAttribute = (int) Random.Range(0, 4);

        setBodyColor();

        Debug.Log("Enemy Magic Type: " + _magicAttribute);

        health = 80.0f;

        _DMGNumTransform = transform.GetChild(0);

        GameManager = GameObject.Find("GameManager").GetComponent<GameManager>();

        _playerRef = GameObject.Find("Player");

        GameManager.enemyCount--;

    }

    // Update is called once per frame
    void Update()
    {
        
        if(_runtimeData.currentGameState == GameplayState.InGame) {

        Movement();

        //Debug.Log(health);

        }

        if(transform.position.y < 1.05f || transform.position.y > 10.5f) {


            transform.position = new Vector3(transform.position.x, 1.5f, transform.position.z);

        }

        if(transform.position.x > 17f) {

            transform.position = new Vector3(16.5f, transform.position.y, transform.position.z);

        } else if(transform.position.x < -17f) {

            transform.position = new Vector3(-16.5f, transform.position.y, transform.position.z);

        }

        if(transform.position.z > 17f) {

            transform.position = new Vector3(transform.position.z, transform.position.y, 16.5f);

        } else if(transform.position.z < -17f) {

            transform.position = new Vector3(transform.position.z, transform.position.y, -16.5f);

        }

    }

    void Movement() 
    {

        float speed = 5.0f;

        // if(_magicAttribute == 1) {

        //     speed = 6.5f;

        // }

        transform.LookAt(_playerRef.transform.position);

        if(GetComponent<CharacterController>().isGrounded) { //check whether or not to apply gravity

            _vertVelo = 0f;

        }

        Vector3 move = transform.forward;

        _vertVelo -= _gravity * Time.deltaTime;

        move.y = _vertVelo;

        transform.GetComponent<CharacterController>().Move(move * speed * Time.deltaTime);



    }

    public void calculateDamage()
    {

        //Debug.Log("Initial Health: " + health);
        //Debug.Log("Enemy Magic Type: " + _magicAttribute);

        if (_magicAttribute == 0) {

             if(_runtimeData.currentPlayerMagic == PlayerMagicState.Ice) {

                health -= 2*(10 + _playerRef.GetComponent<PlayerController>()._attackBoost);

                _DMGNumTransform.LookAt(_playerRef.transform.position);

                _DMGNumTransform.Rotate(new Vector3(0.0f, 180.0f, 0.0f));

                _DMGNumPrefab.GetComponent<TextMeshPro>().text = 2*(10 + _playerRef.GetComponent<PlayerController>()._attackBoost) + "";

                Instantiate(_DMGNumPrefab, _DMGNumTransform);

                //Debug.Log("Damange Calculated Health: " + health);

            } else if(_runtimeData.currentPlayerMagic != PlayerMagicState.Fire) {

                health -= (10 + _playerRef.GetComponent<PlayerController>()._attackBoost);

                _DMGNumTransform.LookAt(_playerRef.transform.position);

                _DMGNumTransform.Rotate(new Vector3(0.0f, 180.0f, 0.0f));

                _DMGNumPrefab.GetComponent<TextMeshPro>().text = (10 + _playerRef.GetComponent<PlayerController>()._attackBoost) + "";

                Instantiate(_DMGNumPrefab, _DMGNumTransform);

                //Debug.Log("Damange Calculated Health: " + health);

            }

        } else if (_magicAttribute == 1) {

             if(_runtimeData.currentPlayerMagic == PlayerMagicState.Fire) {

                health -= 2*(12.5f + _playerRef.GetComponent<PlayerController>()._attackBoost);

                _DMGNumTransform.LookAt(_playerRef.transform.position);

                _DMGNumTransform.Rotate(new Vector3(0.0f, 180.0f, 0.0f));

                _DMGNumPrefab.GetComponent<TextMeshPro>().text = 2*(12.5f + _playerRef.GetComponent<PlayerController>()._attackBoost) + "";

                Instantiate(_DMGNumPrefab, _DMGNumTransform);

                //Debug.Log("Damange Calculated Health: " + health);

            } else if(_runtimeData.currentPlayerMagic != PlayerMagicState.Ice) {

                health -=  (10 + _playerRef.GetComponent<PlayerController>()._attackBoost);

                _DMGNumTransform.LookAt(_playerRef.transform.position);

                _DMGNumTransform.Rotate(new Vector3(0.0f, 180.0f, 0.0f));

                _DMGNumPrefab.GetComponent<TextMeshPro>().text = (10 + _playerRef.GetComponent<PlayerController>()._attackBoost) + "";

                Instantiate(_DMGNumPrefab, _DMGNumTransform);

                //Debug.Log("Damange Calculated Health: " + health);

            } 

        } else if (_magicAttribute == 2) {

             if(_runtimeData.currentPlayerMagic == PlayerMagicState.Earth) {

                health -= 2*(10 + _playerRef.GetComponent<PlayerController>()._attackBoost);

                _DMGNumTransform.LookAt(_playerRef.transform.position);

                _DMGNumTransform.Rotate(new Vector3(0.0f, 180.0f, 0.0f));

                _DMGNumPrefab.GetComponent<TextMeshPro>().text = 2*(10 + _playerRef.GetComponent<PlayerController>()._attackBoost) + "";

                Instantiate(_DMGNumPrefab, _DMGNumTransform);

                //Debug.Log("Damange Calculated Health: " + health);

            } else if(_runtimeData.currentPlayerMagic == PlayerMagicState.Fire) {

                health -= (12.5f + _playerRef.GetComponent<PlayerController>()._attackBoost);

                _DMGNumTransform.LookAt(_playerRef.transform.position);

                _DMGNumTransform.Rotate(new Vector3(0.0f, 180.0f, 0.0f));

                _DMGNumPrefab.GetComponent<TextMeshPro>().text = (12.5f + _playerRef.GetComponent<PlayerController>()._attackBoost) + "";

                Instantiate(_DMGNumPrefab, _DMGNumTransform);

                //Debug.Log("Damange Calculated Health: " + health);

            } else if(_runtimeData.currentPlayerMagic != PlayerMagicState.Lightning) {

                health -= (10 + _playerRef.GetComponent<PlayerController>()._attackBoost);

                _DMGNumTransform.LookAt(_playerRef.transform.position);

                _DMGNumTransform.Rotate(new Vector3(0.0f, 180.0f, 0.0f));

                _DMGNumPrefab.GetComponent<TextMeshPro>().text = (10 + _playerRef.GetComponent<PlayerController>()._attackBoost) + "";

                Instantiate(_DMGNumPrefab, _DMGNumTransform);

                //Debug.Log("Damange Calculated Health: " + health);

            }

        } else if (_magicAttribute == 3) {

             if(_runtimeData.currentPlayerMagic == PlayerMagicState.Lightning) {

                health -= 2*(7.5f + _playerRef.GetComponent<PlayerController>()._attackBoost);

                _DMGNumTransform.LookAt(_playerRef.transform.position);

               _DMGNumTransform.Rotate(new Vector3(0.0f, 180.0f, 0.0f));

                _DMGNumPrefab.GetComponent<TextMeshPro>().text = 2*(7.5 + _playerRef.GetComponent<PlayerController>()._attackBoost) + "";

                Instantiate(_DMGNumPrefab, _DMGNumTransform);

                //Debug.Log("Damange Calculated Health: " + health);

            } else if(_runtimeData.currentPlayerMagic == PlayerMagicState.Fire) {

                health -= (10f + _playerRef.GetComponent<PlayerController>()._attackBoost);

                _DMGNumTransform.LookAt(_playerRef.transform.position);

                _DMGNumTransform.Rotate(new Vector3(0.0f, 180.0f, 0.0f));

                _DMGNumPrefab.GetComponent<TextMeshPro>().text = (10f + _playerRef.GetComponent<PlayerController>()._attackBoost) + "";

                Instantiate(_DMGNumPrefab, _DMGNumTransform);

                //Debug.Log("Damange Calculated Health: " + health);

            } else if(_runtimeData.currentPlayerMagic != PlayerMagicState.Earth) {

                health -=  (7.5f + _playerRef.GetComponent<PlayerController>()._attackBoost);

                _DMGNumTransform.LookAt(_playerRef.transform.position);

                _DMGNumTransform.Rotate(new Vector3(0.0f, 180.0f, 0.0f));

                _DMGNumPrefab.GetComponent<TextMeshPro>().text = (7.5f + _playerRef.GetComponent<PlayerController>()._attackBoost) + "";

                Instantiate(_DMGNumPrefab, _DMGNumTransform);

                //Debug.Log("Damange Calculated Health: " + health);

            }

        }

    }

    void setBodyColor() 
    {

        if(_magicAttribute == 0) {

            this.gameObject.GetComponent<Renderer>().material = _matsArray[0];

        } else if (_magicAttribute == 1) {

            this.gameObject.GetComponent<Renderer>().material = _matsArray[1];

        } else if (_magicAttribute == 2) {

            this.gameObject.GetComponent<Renderer>().material = _matsArray[2];

        } else if (_magicAttribute == 3) {

            this.gameObject.GetComponent<Renderer>().material = _matsArray[3];

        }

    }

    public void orbDropOnDeath()
    {

        Instantiate(_currencyPrefab, transform.position, transform.rotation);

        //Debug.Log("Position of Orb: " + _currencyPrefab.transform.position);

    }

    void OnSceneLoaded() 
    {

        Destroy(transform.gameObject);

    }
}
