using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AttractedCube : FlyingCube {
    Animator m_animator = null;

    static List<AttractedCube> m_inactiveAttractedClones = null;

    Coroutine m_attractToCamera = null;

    protected override void OnEnable() {
        m_attractToCamera = StartCoroutine(AttractToCamera());
        base.OnEnable();
    }

    void OnDisable() {
        m_attractToCamera = null;
    }

    protected override void Update() {
        if (m_attractToCamera != null) {
            if (AppManager.State != AppManager.AppState.ATTRACT_CUBES) {
                ClipPlanePoints clipPoints = ClipPlanePoints.Instance;

                if (clipPoints.IsOutsideBounds(transform.position)) {
                    Recycle();
                }
                else {
                    StopCoroutine(m_attractToCamera);
                    m_attractToCamera = null;

                    m_animator.SetTrigger("cleanup");
                    m_animator.SetBool("isEnabled", false);
                }
            }
        }

        base.Update();
    }

    public override void InitPool(Transform parent) {
        if (m_inactiveAttractedClones == null) {
            //First time initialize to number of twitter feeds...
            m_inactiveAttractedClones = new List<AttractedCube>();
            for (int i = 0, count = TwitterManager.Instance.NumImgTweets;
                i < count; ++i) {

                AttractedCube cube = (AttractedCube)Instantiate(this, parent);
                cube.Recycle();
            }
        }
    }

    public override FlyingCube Clone(Transform parent, Vector3 pos, Vector3 vel) {
        AttractedCube cube = null;

        //Before we clone, let's see if we can recycle a clone...
        if (m_inactiveAttractedClones.Count > 0) {
            cube = m_inactiveAttractedClones[0];
            m_inactiveAttractedClones.Remove(cube);

            Transform cubeTrans = cube.transform;
            cubeTrans.SetParent(parent);
            cubeTrans.position = pos;
            cube.gameObject.SetActive(true);
        }
        else {
            cube = (AttractedCube)Instantiate(this,
               pos, Quaternion.identity, parent);
        }

        cube.Velocity = vel;
        return cube;
    }

    protected override void Recycle() {
        //When initializing, Recycle is sometimes called twice
        //  by FlyingCube Collision Check coroutines. This is
        //  okay, but we need not recycle twice.
        if (!m_inactiveAttractedClones.Contains(this)) {
            gameObject.SetActive(false);
            m_inactiveAttractedClones.Add(this);
        }
    }

    IEnumerator AttractToCamera() {
        if (m_animator == null) {
            m_animator = GetComponentInChildren<Animator>();
        }

        ClipPlanePoints clipPoints = ClipPlanePoints.Instance;
        Camera camera = clipPoints.Camera;

        while (clipPoints.CollidesLeft(transform.position, 3.7f)) {
            yield return new WaitForSeconds(.1f);
        }

        m_velMultiplier = 1f;

        //TODO: Don't hard-code this...
        while (clipPoints.CollidesLeft(transform.position, -.8f)) {
            yield return new WaitForSeconds(.1f);
        }

        m_animator.SetFloat("speed", Velocity.magnitude);
        m_animator.SetBool("isEnabled", true);

        //TODO: Don't hard-code this...
        while (!clipPoints.CollidesRight(transform.position, -4.4f)) {
            yield return new WaitForSeconds(.1f);
        }

        m_animator.SetBool("isEnabled", false);
    }
}