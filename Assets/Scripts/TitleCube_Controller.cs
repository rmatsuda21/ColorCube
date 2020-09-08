using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleCube_Controller : MonoBehaviour
{
    private CreateMesh cm;
    private Animator anim;

    public AnimationCurve curve;

    public float speed = .1f;
    public float size = .3f;

    public Manager.Move[] moveSet;
    private int setIndex = 0;

    bool isAnim = false;

    int bottomFace = 0;

    GameObject tilePrefab;

    /*
     * Top & Buttom face is index 4 & 5 respectively
     * Other faces for in order from [0-3] going ccw
     */

    void Awake()
    {
        size = .3f;

        tilePrefab = Resources.Load("Tile", typeof(GameObject)) as GameObject;

        anim = transform.GetComponent<Animator>();
        cm = GetComponent<CreateMesh>();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
            speed = 0.00000000000001f;
    }

    public void movePlayer(Manager.Move move)
    {
        if (move == Manager.Move.DOWN)
        {
            StartCoroutine(rollCube(speed, Vector3.down * size, Vector3.right, -90));
        }
        if (move == Manager.Move.UP)
        {
            StartCoroutine(rollCube(speed, Vector3.up * size, Vector3.right, 90));
        }
        if (move == Manager.Move.LEFT)
        {
            StartCoroutine(rollCube(speed, Vector3.left * size, Vector3.up, 90));
        }
        if (move == Manager.Move.RIGHT)
        {
            StartCoroutine(rollCube(speed, Vector3.right * size, Vector3.up, -90));
        }
    }

    public void drawFirstCube()
    {
        GameObject title = GameObject.CreatePrimitive(PrimitiveType.Cube);
        title.transform.parent = transform.parent;
        title.GetComponent<MeshRenderer>().material.color = cm.cols[bottomFace];
        title.GetComponent<MeshRenderer>().shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
        title.GetComponent<MeshRenderer>().receiveShadows = false;

        title.transform.localScale = transform.localScale;
        title.transform.localPosition = transform.localPosition - Vector3.back * size;
    }

    //Animate rolling
    IEnumerator rollCube(float animTime, Vector3 distance, Vector3 rotAxis, float angle = 90)
    {
        isAnim = true;
        float time = 0;

        Vector3 fromPos = transform.localPosition;
        Quaternion fromRot = transform.rotation;

        while (time < animTime)
        {
            time += Time.deltaTime;
            time = Mathf.Clamp(time, 0, animTime);

            //Calculate roll pos based on animation curve
            float t = curve.Evaluate(map(time, 0, animTime, 0, 1));

            transform.rotation = Quaternion.Slerp(fromRot, Quaternion.AngleAxis(angle, rotAxis) * fromRot, t);

            //Calculate height offset from rotation
            float h = Mathf.Sin(Mathf.Deg2Rad * map(time, 0, animTime, 45, 135)) * Mathf.Sqrt(2) * size/2 - size/2;

            transform.localPosition = Vector3.Lerp(fromPos, fromPos + distance, t) + new Vector3(0, h, 0);
            yield return new WaitForEndOfFrame();
        }

        isAnim = false;

        //Check new bottom face
        Vector3[] faces = { -transform.forward, transform.right, transform.forward, -transform.right, transform.up, -transform.up };
        for (int i = 0; i < faces.Length; ++i)
        {
            if (System.Math.Abs(Vector3.Angle(faces[i], Vector3.back)) <= 0)
            {
                bottomFace = i;
                break;
            }
        }

        GameObject title = Instantiate(tilePrefab);
        title.transform.parent = transform.parent;
        title.GetComponent<Renderer>().material.color = cm.cols[bottomFace];
        title.GetComponent<MeshRenderer>().shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
        title.GetComponent<MeshRenderer>().receiveShadows = false;

        title.transform.localScale = transform.localScale;
        title.transform.localPosition = transform.localPosition - Vector3.back * size;

        setIndex++;
        if (setIndex < moveSet.Length)
            movePlayer(moveSet[setIndex]);
    }

    float map(float s, float a1, float a2, float b1, float b2)
    {
        return b1 + (s - a1) * (b2 - b1) / (a2 - a1);
    }
}
