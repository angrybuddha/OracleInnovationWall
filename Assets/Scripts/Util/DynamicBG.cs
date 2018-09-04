using UnityEngine;
using System.Collections;
using DG.Tweening;

public class DynamicBG : MonoBehaviour
{


    private string _url;
    public string url
    {
        get { return _url; }
        set
        {
            _url = value;

        }
    }

    IEnumerator ApplyTexture()
    {

        var texture = new Texture2D(2, 2, TextureFormat.RGBAHalf, false);
        GetComponent<CanvasRenderer>().SetTexture(texture);

        while (true)
        {
            WWW www = new WWW(_url);
            yield return www;

            /*if (resize)
            {
                float dim = (float)www.texture.height / (float)www.texture.width;
                if (dim < 1)
                {
                    transform.DOScaleY(dim, 0);
                }
                else
                {
                    dim = 1f / dim;
                    transform.DOScaleX(dim, 0);
                }
            }*/

            www.LoadImageIntoTexture(texture);
            break;
        }

    }

    public void Apply()
    {
        StartCoroutine(ApplyTexture());
    }

}