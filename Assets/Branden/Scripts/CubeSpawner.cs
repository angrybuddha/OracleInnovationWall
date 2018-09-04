using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class CubeSpawner : MonoBehaviour {

    [SerializeField, Header("For fading text in background...")]
    float m_startFadeDistance = 30f;
    public float StartFadeDistance {
        get { return m_startFadeDistance; }
    }

    [SerializeField]
    float m_amountDistToFade = 30f;
    public float AmountDistToFade {
        get { return m_amountDistToFade; }
    }

    [SerializeField, Header("For randomizing how cubes spawn...")]
    int m_numCubesFacingCamera = 6;

    [SerializeField]
    int m_numCubesVertically = 8;

    [SerializeField]
    int m_numRemovedFromFacing = 1;

    [SerializeField]
    float m_attractBoundsOffset = 0.75f;

    [SerializeField]
    float m_attractBoundsRandOffset = 0.3f;

    [SerializeField]
    float m_minSpawnTime = 4f;

    [SerializeField]
    float m_maxSpawnTime = 128f;

    [SerializeField]
    float m_minAttractSpawnTime = 12f;

    [SerializeField]
    float m_maxAttractSpawnTime = 32f;

    [SerializeField]
    float m_minCubeSpeed = 1f;

    [SerializeField]
    float m_maxCubeSpeed = 3f;

    [SerializeField]
    float m_streamCubeSpeed = .4f;

    [SerializeField]
    float m_streamCubeSpawnTime = 3f;

    [SerializeField, Header("From center of screen...")]
    float m_streamCubeDist = 20f;

    [SerializeField]
    float m_attractCubeSpeed = .25f;

    [SerializeField]
    FlyingCube m_flyingCubePref = null;

    [SerializeField]
    AttractedCube m_attractCubePref = null;

    [SerializeField]
    StreamingCube m_streamCubePref = null;

    List<Vector3> m_flyingSpawnPoints = new List<Vector3>();
    List<Vector3> m_attractSpawnPoints = new List<Vector3>();

    //For active sections on wall...
    bool[] m_activeIndices = {
            false, false, false, false, false
    };

    Vector3[] m_streamCubePositions = {
        Vector3.zero, Vector3.zero, Vector3.zero,
        Vector3.zero, Vector3.zero, Vector3.zero
    };

    float[] m_cubeOffsets = {
        2.1f, 3.5f, 5.15f,
        6.6f, 8.2f, 9.65f
    };

    public float[] CubeOffsets {
        get { return m_cubeOffsets; }
    }

    //For active sections on wall...
    bool[] m_spawnIndices = {
        false, false, false,
        false, false, false
    };

    //For active sections on wall...
    bool[] m_lastSpawnIndices = {
        false, false, false,
        false, false, false
    };

    Coroutine m_streamCubes = null;

    bool m_isAlreadyRunning = false;

    static CubeSpawner m_instance = null;
    public static CubeSpawner Instance {
        get {
            if (m_instance == null) {
                m_instance = FindObjectOfType<CubeSpawner>();
            }
            return m_instance;
        }
    }

    // Use this for initialization
    void Awake () {
        MakeSpawnPoints();
    }

    void MakeSpawnPoints() {
        ClipPlanePoints points = ClipPlanePoints.Instance;
        points.UpdateCameraClipPlanePoints();
        Camera camera = points.Camera;

        Vector3 top = points.FarUpperLeft;
        Vector3 bottom = points.FarLowerLeft;
        Vector3 virtDir = -camera.transform.up;
        Vector3 horizDir = -camera.transform.forward;

        float horizDist = camera.farClipPlane;
        float vertDist = Vector3.Distance(top, bottom); //+1, don't want point at near plane...
        float horizOffset = horizDist /(m_numCubesFacingCamera + 1);
        float vertOffset = vertDist /(m_numCubesVertically - 1);

        //spawn points...
        int lastFacingIndex = m_numCubesFacingCamera - m_numRemovedFromFacing;
        for (int i = 0; i < m_numCubesVertically; ++i) {
            Vector3 spawnPoint = top + (virtDir * vertOffset * i) +
                    (horizDir * horizOffset * lastFacingIndex);
            m_attractSpawnPoints.Add(spawnPoint);

            for (int j = 1, count = m_numCubesFacingCamera -
                m_numRemovedFromFacing; j < count; ++j) {

                spawnPoint = top + (virtDir * vertOffset * i) +
                    (horizDir * horizOffset * j);
                m_flyingSpawnPoints.Add(spawnPoint);
            }
        }
    }

    public void SpawnAmbientCubes() {
        if (m_isAlreadyRunning) {
            TwitterCube.ResetTwitterContent();
            return;
        }

        m_isAlreadyRunning = true;
        m_flyingCubePref.InitPool(transform);
        m_attractCubePref.InitPool(transform);
        m_streamCubePref.InitPool(transform);

        foreach (Vector3 point in m_flyingSpawnPoints) {
            StartCoroutine(SpawnFlyingCubes(m_flyingCubePref, point));
        }

        StartCoroutine(SpawnAttractCubes());
    }

    IEnumerator SpawnAttractCubes() {
        ClipPlanePoints clipPoints = ClipPlanePoints.Instance;
        Camera camera = clipPoints.Camera;

        //HACK: Used to increase attract cube velocity speed, for some time, after app times back in...
        float velMultiplier = 100f;

        /////////////////////////////////////////
        //HACK: Hackkery! Uglyness! :(
        Vector3 firstPoint = m_attractSpawnPoints[0];
        Vector3 lastPoint = m_attractSpawnPoints[m_attractSpawnPoints.Count - 1];
        Vector3 middlePoint = (firstPoint + lastPoint) / 2f;

        //Adding up/down adjustments...
        firstPoint = middlePoint + (camera.transform.up * m_attractBoundsOffset);
        lastPoint = middlePoint - (camera.transform.up * m_attractBoundsOffset);

        do {
            if (AppManager.State == AppManager.AppState.ATTRACT_CUBES) {
                float speed = Random.Range(m_attractCubeSpeed, m_attractCubeSpeed + .1f);  //For versatility...
                FlyingCube cube = m_attractCubePref.Clone(transform, firstPoint +
                    (Vector3.up * Random.Range(0f, -m_attractBoundsRandOffset)),
                    speed * camera.transform.right);

                cube.VelMultiplier = velMultiplier;

                float spawnTime = Random.Range(m_minAttractSpawnTime, m_maxAttractSpawnTime);
                yield return new WaitForSeconds(spawnTime);
                ///////////////////////////////////////////////////////////

                speed = Random.Range(m_attractCubeSpeed, m_attractCubeSpeed + .1f);  //For versatility...
                cube = cube = m_attractCubePref.Clone(transform, lastPoint +
                    (Vector3.up * Random.Range(0f, m_attractBoundsRandOffset)),
                    speed * camera.transform.right);

                cube.VelMultiplier = velMultiplier;

                spawnTime = Random.Range(m_minAttractSpawnTime, m_maxAttractSpawnTime);
                yield return new WaitForSeconds(spawnTime);

            }
            else {
                yield return new WaitForSeconds(.1f);
            }
        } while (true);
    }

    IEnumerator SpawnFlyingCubes(FlyingCube prefab, Vector3 cubePos) {
        ClipPlanePoints clipPoints = ClipPlanePoints.Instance;
        Camera camera = clipPoints.Camera;

        if (!clipPoints.CollidesTop(cubePos) &&
            !clipPoints.CollidesBottom(cubePos)) {

            float cubeSpeed = Random.Range(
            m_minCubeSpeed, m_maxCubeSpeed);

            float firstSpawnTime = 0f;

            //Allows closer cubes to spawn more. If the cube is closer, maxRandMulti will be less.
            float spawnMulti = 0.05f * Mathf.Max(Vector3.Distance(clipPoints.NearUpperLeft, cubePos) /
                 Vector3.Distance(clipPoints.NearUpperLeft, clipPoints.FarUpperLeft), m_minSpawnTime);
            float maxSpawnTime = Mathf.Max(spawnMulti * m_maxSpawnTime, m_minSpawnTime);

            do {
                float spawnTime = Random.Range(firstSpawnTime, maxSpawnTime);
                yield return new WaitForSeconds(spawnTime);
                firstSpawnTime = m_minSpawnTime;
                prefab.Clone(transform, cubePos, cubeSpeed * camera.transform.right);
            } while (true);
        }
    }

    //Cubes that have already been streamed in...
    public void CLeanupAllStreamingCubes() {
        for (int i = 0, count = m_spawnIndices.Length; i < count; ++i) {
            StreamingCube.CleanupAll(i);
        }
    }

    public void ResetAllIndices() {
        for (int i = 0, count = m_activeIndices.Length; i < count; ++i) {
            m_activeIndices[i] = false;
        }
        
        ResetSpawnIndices();
    }

    public void ResetIndex(int index) {
        m_activeIndices[index] = false;
        ResetSpawnIndices();
    }

    public void ResetSpawnIndices() {
        //Resetting all spawn indices...
        for (int i = 0, count = m_spawnIndices.Length; i < count; ++i) {
            m_spawnIndices[i] = false;
        }

        for (int i = 1, count = m_spawnIndices.Length; i < count; ++i) {
            if (m_activeIndices[i - 1]) {
                m_spawnIndices[i - 1] = true;
                m_spawnIndices[i] = true;
            }
        }
    }

    public void SpawnStreamingCube(int index) {
        if (AppManager.IsInAmbientMode) {
            if (AppManager.State == AppManager.AppState.ATTRACT_CUBES) {
                AppManager.State = AppManager.AppState.STREAMING_CUBES;
            }
            
            if (m_activeIndices[index]) {
                return;
            }

            m_activeIndices[index] = true;

            ResetSpawnIndices();

            if (m_streamCubes == null) {
                ClipPlanePoints clipPoints = ClipPlanePoints.Instance;
                Camera camera = clipPoints.Camera;

                //HACK: Hackery! But works!!!
                Vector3 leftDir = (clipPoints.NearLowerLeft -
                    clipPoints.FarLowerLeft).normalized;

                Vector3 startPoint = clipPoints.NearLowerLeft -
                    (leftDir * m_streamCubeDist);

                Vector3 right = camera.transform.right;

                for (int i = 0, count = m_streamCubePositions.Length; i < count; ++i) {
                    Vector3 cubePos = startPoint +
                        (right * m_cubeOffsets[i]);
                    m_streamCubePositions[i] = cubePos;
                }

                m_streamCubes = StartCoroutine(SpawnStreamingCube());
            }
        }
    }

    void SpawnStreamingCube(int index, Vector3 vel) {
        Vector3 cubePos = m_streamCubePositions[index];
        StreamingCube cube = (StreamingCube)m_streamCubePref.Clone(
            transform, cubePos, vel);
        cube.SectionIndex = index;
    }

    IEnumerator SpawnStreamingCube() {
        ClipPlanePoints clipPoints = ClipPlanePoints.Instance;
        Camera camera = clipPoints.Camera;
        Vector3 cubeVel = camera.transform.up * m_streamCubeSpeed;

        do {
            for (int i = 0, count = m_spawnIndices.Length; i < count; ++i) {
                if (m_spawnIndices[i]) {
                    SpawnStreamingCube(i, cubeVel);
                }
                else if (m_lastSpawnIndices[i]) {
                    StreamingCube.CleanupAll(i);
                }

                m_lastSpawnIndices[i] = m_spawnIndices[i];
            }

            yield return new WaitForSeconds(m_streamCubeSpawnTime);
        } while (AppManager.IsInAmbientMode);

        CLeanupAllStreamingCubes();
        ResetAllIndices();
        m_streamCubes = null;
    }

    void DrawSpawnPointGrid() {
        for (int i = 1, count = m_flyingSpawnPoints.Count; i < count; ++i) {
            Vector3 startPoint = m_flyingSpawnPoints[i - 1];
            Vector3 endPoint = m_flyingSpawnPoints[i];
            Debug.DrawLine(startPoint, endPoint, Color.green);
        }

        ClipPlanePoints clipPoints = ClipPlanePoints.Instance;
        Camera camera = clipPoints.Camera;

        if (clipPoints != null) {
            Vector3 leftDir = (clipPoints.NearLowerLeft -
            clipPoints.FarLowerLeft).normalized;
            Vector3 rightDir = (clipPoints.NearLowerRight -
                clipPoints.FarLowerRight).normalized;

            Vector3 startPoint = clipPoints.NearLowerLeft -
                (leftDir * m_streamCubeDist);
            Vector3 endPoint = clipPoints.NearLowerRight -
                (rightDir * m_streamCubeDist);

            Debug.DrawLine(startPoint, endPoint, Color.blue);
        }
    }
}
