using UnityEngine;
using System.Collections;

//[ExecuteInEditMode]
public class RenderToCubeMap : MonoBehaviour
{
    public Cubemap cubemap;
    public Material currentMaterial;
    public float updateRate = 1f;
    [SerializeField]
    private Transform renderFromPosition;
    private float minz = -1f;

    void LateUpdate()
    {
        if (Time.time - updateRate > minz)
        {
            RenderMe();
            currentMaterial.SetTexture("_Cube", cubemap);
            GetComponent<Renderer>().material = currentMaterial;
        }
    }

    void RenderMe()
    {
        GameObject go = new GameObject("CubemapCamera" + Random.seed);
        go.AddComponent<Camera>();

        go.GetComponent<Camera>().backgroundColor = Color.black;
        go.GetComponent<Camera>().cullingMask = ~(1 << 8);
        go.GetComponent<Camera>().transform.position = renderFromPosition.position;
        if (renderFromPosition.GetComponent<Renderer>())
            go.transform.position = renderFromPosition.GetComponent<Renderer>().bounds.center;

        go.GetComponent<Camera>().transform.rotation = Quaternion.identity;

        go.GetComponent<Camera>().RenderToCubemap(cubemap);

        DestroyImmediate(go);
    }
}
