/*******************************************************************************************
* Author: Jed Bursiek
* Created Date: 07-01-16 
* 
* Description: 
*   Used for displaying cube states.
*******************************************************************************************/

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;

public class PlayerCube : MonoBehaviour
{

    // Use this for initialization
    public GameObject cube;
    public GameObject move_PNG;
    public GameObject down_PNG;
    public GameObject here_PNG;

    public TextMeshPro txt01;
    public TextMeshPro txt02;
    public TextMeshPro txt03;
    public TextMeshPro txt04;

    public int region_id;

    public float scatter_value;

    public int scatter_col_pointer;

    private int _id;


    public int id
    {
        get { return _id; }
        set
        {
            _id = value;
            //Debug.Log("set ambient value " + value);
        }
    }

    void Start()
    {

        init();

        //txt01

    }

    void init()
    {
        move_PNG.transform.DOScale(0, 0);
        down_PNG.SetActive(false);

        hideText();

        scatter_col_pointer = 0;

    }

    public void hideCube()
    {
        cube.SetActive(false);
    }

    public void showCube()
    {
        cube.SetActive(true);
    }

    public void hideAll()
    {
        move_PNG.transform.DOScale(0, 0);
        down_PNG.SetActive(false);

        hideText();
    }

    void initCounter()
    {

        txt01.color = new Color(1, 1, 1);
        txt02.color = new Color(1, 1, 1);
        txt03.color = new Color(1, 1, 1);
        txt04.color = new Color(1, 1, 1);

        txt01.text = "9";
        txt02.text = "8";
        txt03.text = "7";
        txt04.text = "6";

    }

    public void setColor(Color color)
    {

        //Debug.Log("COLOR " + color);

        cube.GetComponent<Renderer>().material.color = color;
    }

    public void doCountdown()
    {
        initCounter();
        StartCoroutine(countDown10());
        //transform.DORotate(new Vector3(0, 90, 0), 1).OnComplete(doCountdown);
    }

    public void doCountdown5()
    {
        StartCoroutine(countDown5());
    }

    public void showPerson()
    {

        //hideText();
        move_PNG.transform.DOScale(0.8f, 0.5f);
    }

    public void hidePerson()
    {

        //hideText();
        move_PNG.transform.DOScale(0, 0.5f);
    }

    public void showArrow()
    {
        //down_PNG.SetActive(true);
    }

    public void scaleDown()
    {
        cube.transform.DOScale(0.0f, 1.0f);

    }

    public void scaleUp()
    {
        cube.transform.DOScale(0.35f, 1.0f);
    }

    IEnumerator countDown10()
    {

        //initCounter();
        //set front ways
        cube.transform.DORotate(new Vector3(0, 0, 0), 1);

        //9
        yield return new WaitForSeconds(1.0F);
        cube.transform.DORotate(new Vector3(0, 90, 0), 1);

        //8
        yield return new WaitForSeconds(1.0F);
        cube.transform.DORotate(new Vector3(0, 180, 0), 1);

        //7
        yield return new WaitForSeconds(1.0F);
        cube.transform.DORotate(new Vector3(0, 270, 0), 1);

        //6
        yield return new WaitForSeconds(1.0F);
        cube.transform.DORotate(new Vector3(0, 360, 0), 1);
        txt01.text = "5";

        //5
        yield return new WaitForSeconds(1.0F);
        cube.transform.DORotate(new Vector3(0, 90, 0), 1);
        txt02.text = "4";

        //4
        yield return new WaitForSeconds(1.0F);
        cube.transform.DORotate(new Vector3(0, 180, 0), 1);
        txt03.text = "3";

        //3
        yield return new WaitForSeconds(1.0F);
        cube.transform.DORotate(new Vector3(0, 270, 0), 1);
        txt04.text = "2";
        txt01.text = "1";
        txt02.text = "0";

        //2
        yield return new WaitForSeconds(1.0F);
        cube.transform.DORotate(new Vector3(0, 360, 0), 1);

        //1
        yield return new WaitForSeconds(1.0F);
        cube.transform.DORotate(new Vector3(0, 90, 0), 1);

    }

    IEnumerator countDown5()
    {

        MainController.Instance.do_player_update = false;
        //transform.DOMoveX(9.0f, 0);
        showPerson();
        transform.DOScale(0, 2).From().SetEase(Ease.OutElastic);

        GetComponent<PlayerCube>().cube.SetActive(true);
        //5
        yield return new WaitForSeconds(1.0F);
        cube.transform.DORotate(new Vector3(0, 90, 0), 1);
        txt02.text = "5";


        MainController.Instance.do_player_update = true;

        //4
        yield return new WaitForSeconds(1.0F);
        cube.transform.DORotate(new Vector3(0, 180, 0), 1);
        txt03.text = "4";

        //3
        yield return new WaitForSeconds(1.0F);
        cube.transform.DORotate(new Vector3(0, 270, 0), 1);
        txt04.text = "3";
        txt01.text = "2";
        txt02.text = "1";

        //2
        yield return new WaitForSeconds(1.0F);
        cube.transform.DORotate(new Vector3(0, 360, 0), 1);

        txt03.text = "0";

        //1
        yield return new WaitForSeconds(1.0F);
        cube.transform.DORotate(new Vector3(0, 90, 0), 1);
        txt04.text = "";
        //0
        yield return new WaitForSeconds(1.0F);
        cube.transform.DORotate(new Vector3(0, 180, 0), 1);

        //0
        yield return new WaitForSeconds(1.0F);
        cube.transform.DORotate(new Vector3(0, 270, 0), 1).OnComplete(hidePerson);

    }

    public void snapToCol()
    {
        //Debug.Log("SNAP TO COL :" + transform.position.x);
        scatter_col_pointer = 0;
        scatter_value = MainController.Instance.scatter_map_list[0];

        MainController.Instance.do_player_update = false;

        for (int i = 0; i < MainController.Instance.scatter_map_list.Count; ++i)
        {
            float col = MainController.Instance.scatter_map_list[i];

            if (transform.position.x > col)
            {

                //Loop through until find closest column
                scatter_col_pointer++;

                Debug.Log("SCSATTER POINTER " + scatter_col_pointer);

                //WHERE TO POSITINO THE CUBE
                scatter_value = col;

            }

        }

        //Debug.Log("SNAP TO COL :" + scatter_value);
        down_PNG.SetActive(false);
        hideText();

        if (scatter_col_pointer > 40)
            scatter_col_pointer = 40;
        else if (scatter_col_pointer < 1)
            scatter_col_pointer = 1;

        //NOW WE KNOW WHICH COL - INCREMENT THAT VALUE FOR VOTE TRACKING
        Core.Instance._cms.scatter_plot_answer_list[scatter_col_pointer - 1] = Core.Instance._cms.scatter_plot_answer_list[scatter_col_pointer - 1] + 1;

        updatePosition(scatter_value, -0.4F);

    }

    void hideText()
    {

        txt01.text = "";
        txt02.text = "";
        txt03.text = "";
        txt04.text = "";

    }


    public void updatePosition(float x, float y)
    {

        //Debug.Log("DOING MOVE " + x + ", " + y);
        //transform.DOKill();

        transform.DOMove(new Vector3(x, y, 0), 1);//.SetEase(Ease.InOutQuad);

        region_id = MainController.setRegion(x);

    }

    // Update is called once per frame
    void Update()
    {

    }

}
