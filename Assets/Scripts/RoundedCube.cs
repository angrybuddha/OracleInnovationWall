/*******************************************************************************************
* Author: Jed Bursiek
* Created Date: 07-01-16 
* 
* Description: 
*   Used for ambient twitter cube animations and rendering of text/images
*******************************************************************************************/
using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using DG.Tweening;
using System.Text.RegularExpressions;

public class RoundedCube : MonoBehaviour {

    private float _speed = 0.5f;
    private float _ypos;
    private bool _ambient;
    private bool _floater;
    private Sequence mySequence;

    public GameObject image_quad;
    private bool _hasImage;

    float amplitudeZ = 20.0f;
    float omegaZ = 0.2f;
    bool looked = false;
    float _index;

    float limit;

    public GameObject cube_grp;

    // Use this for initialization

    public bool ambient
    {
        get { return _ambient; }
        set
        {
            _ambient = value;
            //Debug.Log("set ambient value " + value);
        }
    }

    public bool hasImage
    {
        get { return _hasImage; }
        set
        {
            _hasImage = value;
            //Debug.Log("set ambient value " + value);
        }
    }

    public bool floater
    {
        get { return _floater; }
        set
        {
            _floater = value;
            //Debug.Log("set ambient value " + value);
        }
    }

    public float index
    {
        get { return _index; }
        set
        {
            _index = value;
            //Debug.Log("set ambient value " + value);
        }
    }

    public float ypos
    {
        get { return _ypos; }
        set
        {
            _ypos = value;

        }
    }

    public float speed
    {
        get { return _speed; }
        set
        {
            _speed = value;

        }
    }

    private void initCube(float initx)
    {
        _ambient = false;

        transform.DOMove(new Vector3(initx, -3.0f, -20f), 0);

        transform.DOScale(0, 0);

        if (hasImage)//SHOW THE IMAGE ABOUT HALFWAY UP
        {
            cube_grp.transform.DORotate(Vector3.up * 0, 0.0f);
        }

        //gameObject.SetActive(true);

    }

    void Start()
    {

        index = transform.position.x;

        limit = 35;// Core.Instance._settings.Kinect_area_X / 2;

    }

    public void hideSelf()
    {

        transform.DOScale(0, 1).SetEase(Ease.OutQuad).OnComplete(deActivate);

    }

    public void showSelf()
    {

        transform.DOScale(1, 1).SetEase(Ease.OutQuad);
        gameObject.SetActive(true);

    }

    void deActivate()
    {

        //gameObject.SetActive(false);

    }
    
    //COMMANDS FOR REGION CUBE CASCADE - BEGIN

    public void DoZoom(float targx, float targy = -2.0f)
    {

        //Debug.Log("SHOWING CUBES");

        looked = false;

        gameObject.SetActive(false);

        initCube(targx);

        transform.DOMove(new Vector3(targx, targy, -22), 2).SetEase(Ease.Linear).OnComplete(doShow);

        transform.DOScale(0.8F, 2f).SetEase(Ease.OutQuad);

        gameObject.SetActive(true);

    }

    private void doShow()
    {

        transform.DOMoveY(1.8F, 12).SetEase(Ease.Linear).OnComplete(DoZoomOut);

        if (hasImage)//SHOW THE IMAGE ABOUT HALFWAY UP
        {
            cube_grp.transform.DORotate(Vector3.up * 90, 3.0f).SetDelay(5).SetEase(Ease.InOutQuad);
        }

        transform.DOMoveZ(10.0f, 30).SetDelay(10.0f).SetEase(Ease.OutQuad);//move yourself

    }

    public void DoZoomOut()
    {

        transform.DOMoveY(Random.Range(-1.0f, 1.0f), 15).SetDelay(5).SetEase(Ease.OutQuad);

        transform.DOScale(0.0f, 5).SetEase(Ease.InQuad);//move yourself

        transform.DOMoveX(transform.position.x + 5.0f, 10).SetEase(Ease.InQuad);//move yourself

    }

    //COMMANDS FOR REGION CUBE CASCADE - END

    
    //COMMANDS FOR SCREENERS - FLOAT CLOSE UP
    public void doFloat()
    {

        _floater = true;
        index = -20;

    }

    void lookForward()
    {

        if (hasImage)
            cube_grp.transform.DORotate(Vector3.up*90, 6.0f).SetEase(Ease.InOutQuad).OnComplete(lookBack);//Rotate(Vector3.up * _index / 30);

    }

    void lookBack()
    {

        Vector3 targ = Vector3.up * Random.Range(0, 15);
        cube_grp.transform.DORotate(targ, 6.0f).SetEase(Ease.InOutQuad).SetDelay(1.0f);//Rotate(Vector3.up * _index / 30);

    }

    // Update is called once per frame
	void Update () {


        if (_ambient) { 
            transform.position += (Vector3.right * Time.deltaTime * speed);
            if (transform.position.x > limit)
            {

                transform.position = new Vector3(-limit, transform.position.y, transform.position.z);

            }
        }


        if (_floater)
        {

            _index += Time.deltaTime / 2;

            float z = 10 + amplitudeZ * Mathf.Cos(omegaZ * _index);

            transform.position = new Vector3(_index, _ypos, -z);

            if (_index > -4 && !looked)
            {

                looked = true;
                lookForward();

            }

        }
       
	}

    public void setTweetText(string text)
    {
        Regex regex = new Regex(@"(@.+?)([^a-zA-Z0-9\_\-][^@]+)");
        Match match = regex.Match(text);
        if (match.Success)
        {
            text = "<color=red>" + match.Groups[1] + "</color>" + match.Groups[2];
        }

        do
        {
            match = match.NextMatch();
            if (match.Success)
            {
                text += "<color=red>" + match.Groups[1] + "</color>" + match.Groups[2];
            }

        }

        while (match.Success);

        GetComponentInChildren<Text>().text = text;

    }

    public void KillTween()
    {

        transform.DOKill();
    
    } 


}
