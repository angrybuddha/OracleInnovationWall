using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FlyingCube : TwitterCube {

    [SerializeField]
    Vector3 m_velocity = Vector3.zero;
    public Vector3 Velocity {
        get { return m_velocity; }
        set { m_velocity = value; }
    }

    protected float m_velMultiplier = 1f;
    public float VelMultiplier {
        get { return m_velMultiplier; }
        set { m_velMultiplier = value; }
    }

    static List<FlyingCube> m_inactiveClones = null;

    protected virtual void OnEnable() {
        StartCoroutine(CheckCollision());
    }

    protected override void Update() {
        transform.position += (m_velocity * m_velMultiplier * Time.deltaTime);
        base.Update();
    }

    public virtual void InitPool(Transform parent) {
        //First time initialize to number of twitter feeds...
        if (m_inactiveClones == null) {
            m_inactiveClones = new List<FlyingCube>();

            FlyingCube cube = null;
            for (int i = 0, count = TwitterManager.Instance.NumTweets;
                i < count; ++i) {

                cube = (FlyingCube)Instantiate(this, parent);
                cube.Recycle();
            }
        }
    }

    public virtual FlyingCube Clone(Transform parent, Vector3 pos, Vector3 vel) {
        FlyingCube cube = null;

        //Before we clone, let's see if we can recycle a clone...
        if (m_inactiveClones.Count > 0) {
            cube = m_inactiveClones[0];
            m_inactiveClones.Remove(cube);

            Transform cubeTrans = cube.transform;
            cubeTrans.SetParent(parent);
            cubeTrans.position = pos;
            cube.gameObject.SetActive(true);
        }
        else {
            cube = (FlyingCube)Instantiate(this,
               pos, Quaternion.identity, parent);
        }

        cube.Velocity = vel;

        return cube;
    }

    protected virtual void Recycle() {
        gameObject.SetActive(false);
        m_inactiveClones.Add(this);
    }

    protected virtual IEnumerator CheckCollision() {
        ClipPlanePoints points = ClipPlanePoints.Instance;
        if (points != null) {
            while (true) {
                if (points.CollidesRight(transform.position)) {
                    break;
                }

                yield return new WaitForSeconds(.5f);
            }

            //TODO: boden, should we recycle this instead?
            Recycle();
        }
    }
}