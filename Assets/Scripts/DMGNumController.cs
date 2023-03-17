using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DMGNumController : MonoBehaviour
{

    protected Color _textColor;

    protected bool waiting = false;

    // Start is called before the first frame update
    void Start()
    {
        _textColor = transform.gameObject.GetComponent<TextMeshPro>().color;
    
    }

    // Update is called once per frame
    void Update()
    {
        StartCoroutine(fadeTimer());

        //Debug.Log("Transparency: " + _textColor.a);

        if (_textColor.a <= 0) {

            Destroy(transform.gameObject);

        }
    }

    IEnumerator fadeTimer() 
    {
        if(!waiting) {

            waiting = true;

            _textColor.a -= 0.25f;

            transform.gameObject.GetComponent<TextMeshPro>().color = _textColor;

            yield return new WaitForSeconds(0.16f);

            waiting = false;
        }

    }

}
