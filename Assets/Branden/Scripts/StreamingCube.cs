using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class StreamingCube : FlyingCube {
    Animator m_animator = null;

    int m_sectionIndex = -1;
    public int SectionIndex {
        get { return m_sectionIndex; }
        set { m_sectionIndex = value; }
    }

    static Action<int> Cleanup = null;
    static List<StreamingCube> m_inactiveStreamingClones = null;
     
    protected override void Awake() {
        Cleanup += OnCLeanup;
        base.Awake();
    }

    protected override void OnEnable() {
        StartCoroutine(StreamCubes());
        base.OnEnable();
    }

    public override void InitPool(Transform parent) {
        if (m_inactiveStreamingClones == null) {
            StreamingCube cube = null;

            //First time initialize to number of twitter feeds...
            m_inactiveStreamingClones = new List<StreamingCube>();
            for (int i = 0, count = TwitterManager.Instance.NumImgTweets;
                i < count; ++i) {

                cube = (StreamingCube)Instantiate(this, parent);
                cube.Recycle();
            }
        }
    }

    //Stops cubes from animation, removes cubes outside camera bounds...
    public static void CleanupAll(int sectionIndex) {
        if (Cleanup != null) {
            Cleanup(sectionIndex);
        }
    }

    //Brings cube back to default animation state and
    //removes cube if outside camera bounds...
    void OnCLeanup(int sectionIndex) {
        if (!gameObject.activeSelf ||
            m_sectionIndex != sectionIndex) {
            return;
        }

        m_sectionIndex = -1;
        StopCoroutine(StreamCubes());

        if (m_animator == null) {
            m_animator = GetComponentInChildren<Animator>();
        }

        m_animator.SetBool("isEnabled", false);
        m_animator.SetTrigger("cleanup");

        ClipPlanePoints clipPoints = ClipPlanePoints.Instance;

        if (clipPoints.IsOutsideBounds(transform.position)) {
            Recycle();
        }
    }

    public override FlyingCube Clone(Transform parent, Vector3 pos, Vector3 vel) {
        StreamingCube cube = null;

        //Before we clone, let's see if we can recycle a clone...
        if (m_inactiveStreamingClones.Count > 0) {
            cube = m_inactiveStreamingClones[0];
            m_inactiveStreamingClones.Remove(cube);

            Transform cubeTrans = cube.transform;
            cubeTrans.SetParent(parent);
            cubeTrans.position = pos;
            cube.gameObject.SetActive(true);
        }
        else {
            cube = (StreamingCube)Instantiate(this,
               pos, Quaternion.identity, parent);
        }

        cube.Velocity = vel;

        return cube;
    }

    protected override void Recycle() {
        //When initializing, Recycle is sometimes called twice
        //  by FlyingCube Collision Check coroutines. This is
        //  okay, but we need not recycle twice.
        if (!m_inactiveStreamingClones.Contains(this)) {
            m_inactiveStreamingClones.Add(this);
            gameObject.SetActive(false);
        }
    }

    IEnumerator StreamCubes() {
        if (m_animator == null) {
            m_animator = GetComponentInChildren<Animator>();
        }

        ClipPlanePoints clipPoints = ClipPlanePoints.Instance;
        Camera camera = clipPoints.Camera;

        //TODO: Don't hard-code this...
        while (clipPoints.CollidesBottom(transform.position, -.1f)) {
            yield return new WaitForSeconds(.1f);
        }

        m_animator.SetBool("isEnabled", true);

        //TODO: Don't hard-code this...
        while (!clipPoints.CollidesTop(transform.position, -3.3f)) {
            yield return new WaitForSeconds(.1f);
        }

        m_animator.SetBool("isEnabled", false);
    }

    protected override IEnumerator CheckCollision() {
        ClipPlanePoints points = ClipPlanePoints.Instance;
        if (points != null) {
            while (true) {
                if (points.CollidesTop(transform.position)) {
                    break;
                }

                yield return new WaitForSeconds(.5f);
            }

            //TODO: boden, should we recycle this instead?
            Recycle();
        }
    }
}