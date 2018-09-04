/*******************************************************************************************
* Author: Lane Gresham
* Created Date: 08-12-15 
* 
* Description: 
*   Used for binding all text.
*******************************************************************************************/
using UnityEngine;
using System.Collections;
using TMPro;

public class TMPro_QuestionManager_Bind : MonoBehaviour
{
    public enum TextType
    {
        Question1,
        Question2,
        Answer1Title,
        Answer2Title,
        Answer3Title,
        Answer1Records,
        Answer2Records,
        Answer3Records,
        Answer1Percent,
        Answer2Percent,
        Answer3Percent,
        Fact,
        Answer4Title,
        Proposition,
    }

    [SerializeField]
    private TextType textType;
    public TextType _textType
    {
        get { return textType; }
        set { textType = value; }
    }

    private TextMeshProUGUI tmpro;

    // Use this for initialization
    void Start()
    {
        tmpro = this.GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        switch (_textType)
        {
            case TextType.Question1:
                if (Core.Instance._questionManager._questionState != QuestionManager.QuestionState.ScatterPlot)
                {
                    tmpro.text = Core.Instance._questionManager.Question1;
                }
                else
                {
                    tmpro.text = string.Format("Part 1: {0}", Core.Instance._questionManager.Question1);
                }
                break;
            case TextType.Question2:
                if (Core.Instance._questionManager._questionState != QuestionManager.QuestionState.ScatterPlot)
                {
                    tmpro.text = Core.Instance._questionManager.Question2;
                }
                else
                {
                    tmpro.text = string.Format("Part 2: {0}", Core.Instance._questionManager.Question2);
                }
                break;
            case TextType.Answer1Title:
                tmpro.text = Core.Instance._questionManager.Answer1Title;
                break;
            case TextType.Answer2Title:
                tmpro.text = Core.Instance._questionManager.Answer2Title;
                break;
            case TextType.Answer3Title:
                tmpro.text = Core.Instance._questionManager.Answer3Title;
                break;
            case TextType.Answer4Title:
                tmpro.text = Core.Instance._questionManager.Answer4Title;
                break;
            case TextType.Answer1Records:
                tmpro.text = Core.Instance._questionManager.Answer1Records.ToString();
                break;
            case TextType.Answer2Records:
                tmpro.text = Core.Instance._questionManager.Answer2Records.ToString();
                break;
            case TextType.Answer3Records:
                tmpro.text = Core.Instance._questionManager.Answer3Records.ToString();
                break;
            case TextType.Answer1Percent:
                tmpro.text = string.Format("{0}%",Core.Instance._questionManager.Answer1Percent.ToString());
                break;
            case TextType.Answer2Percent:
                tmpro.text = string.Format("{0}%",Core.Instance._questionManager.Answer2Percent.ToString());
                break;
            case TextType.Answer3Percent:
                tmpro.text = string.Format("{0}%", Core.Instance._questionManager.Answer3Percent.ToString());
                break;
            case TextType.Fact:
                tmpro.text = Core.Instance._questionManager.Fact.ToString();
                break;
            case TextType.Proposition:
                tmpro.text = Core.Instance._questionManager.Proposition.ToString();
                break;
        }
    }
}
