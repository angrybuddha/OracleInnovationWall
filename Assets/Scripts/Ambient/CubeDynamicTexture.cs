using UnityEngine;
using System.Collections;

public class CubeDynamicTexture : MonoBehaviour
{
    //public delegate void OnTwitterEvent(Transform Loc);
    //public static event OnTwitterEvent ApplyTexture;
    //public float scrollSpeed = 0.5F;
    //public Renderer rend;
    private Texture2D texture;

    private string _url;
    public string url
    {
        get { return _url; }
        set
        {
            _url = value;
            //Debug.Log("TEXTURE TO APPLY " + _url);
        }
    }

    public void Start()
    {
        //rend = GetComponent<Renderer>();
    }

    IEnumerator ApplyTexture()
    {
        //Debug.Log("Applying Texture");

        while (true)
        {
            WWW www = new WWW(_url);
            yield return www;
            www.LoadImageIntoTexture(texture);
            paintTopRowWhite();
            break;
        }

    }

    void paintTopRowWhite() {

        Color[] colors = texture.GetPixels();
        Texture2D newTexture = new Texture2D(texture.width, texture.height, TextureFormat.ARGB32, false);

        for (int y = 0, max = texture.height - 1; y < texture.height; ++y) {
            for (int x = 0; x < texture.width; ++x) {
                newTexture.SetPixel(x, y, y >= max ? Color.white : texture.GetPixel(x, y));
            }
        }

        newTexture.Apply();
        //Debug.LogWarning("works");


        //texture = newTexture;
        //Grabs front facing texture
        GetComponent<Renderer>().materials[1].mainTexture = newTexture;

        //Sets wrap mode to not repeating
        GetComponent<Renderer>().materials[1].mainTexture.wrapMode = TextureWrapMode.Clamp;
        //GetComponent<Renderer>().materials[1].mainTexture.m

        offsetTexture();

    }

    void offsetTexture() {

        //offsets the texture
        GetComponent<Renderer>().materials[1].SetTextureOffset("_MainTex", new Vector2(0.0f, 0.5f));

    }
    /*
    public Texture2D texture = null;


    // Use this for initialization
    void Start () {
        Color[] colors = texture.GetPixels ();
        Texture2D newTexture = new Texture2D (texture.width, texture.height * 2);

        for (int i = 0; i < newTexture.Length; ++i) {

        }
    }
*/
    public void Apply()
    {

        //new Texture2D()
        texture = new Texture2D(1, 1, TextureFormat.ARGB32, false);

        //texture.wrapMode = TextureWrapMode.Clamp;

        //Grabs front facing texture
        //GetComponent<Renderer>().materials[1].mainTexture = texture;
        //Sets wrap mode to not repeating
        //GetComponent<Renderer>().materials[1].mainTexture.wrapMode = TextureWrapMode.Clamp;
        
        //Debug.Log("some");
        StartCoroutine(ApplyTexture());
    }

    //public void Update()
    //{
    //   float offset = Time.time * scrollSpeed;
    //  rend.material.SetTextureOffset("_MainTex", new Vector2(0, 0.5f));
    //}

}