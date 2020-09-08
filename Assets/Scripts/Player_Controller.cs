using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Controller : MonoBehaviour
{
    private CreateMesh cm;
    private Animator anim;

    public AnimationCurve curve;

    public float speed = 1f;

    bool isAnim = false;

    int bottomFace = 5;

    //PS for when player rolls
    private GameObject rollPS;

    /*
     * Top & Buttom face is index 4 & 5 respectively
     * Other faces for in order from [0-3] going ccw
     */

    void Start()
    {
        rollPS = Instantiate(Resources.Load("RollPS", typeof(GameObject)) as GameObject);

        anim = transform.GetComponent<Animator>();
        cm = GetComponent<CreateMesh>();
    }

    void Update()
    {
        if (!isAnim)
        {
            if (Input.GetKey(KeyCode.DownArrow))
            {
                Manager.instance.movePlayer(Manager.Move.DOWN, this);
            }
            else if (Input.GetKey(KeyCode.UpArrow))
            {
                Manager.instance.movePlayer(Manager.Move.UP, this);
            }
            else if (Input.GetKey(KeyCode.LeftArrow))
            {
                Manager.instance.movePlayer(Manager.Move.LEFT, this);
            }
            else if (Input.GetKey(KeyCode.RightArrow))
            {
                Manager.instance.movePlayer(Manager.Move.RIGHT, this);
            }
        }
    }

    public void movePlayer(Manager.Move move)
    {
        if (move==Manager.Move.DOWN)
        {
            StartCoroutine(rollCube(speed, Vector3.back, Vector3.right, -90));
        }
        if (move == Manager.Move.UP)
        {
            StartCoroutine(rollCube(speed, Vector3.forward, Vector3.right, 90));
        }
        if (move == Manager.Move.LEFT)
        {
            StartCoroutine(rollCube(speed, Vector3.left, Vector3.forward, 90));
        }
        if (move == Manager.Move.RIGHT)
        {
            StartCoroutine(rollCube(speed, Vector3.right, Vector3.forward, -90));
        }
    }

    //Animate rolling
    IEnumerator rollCube(float animTime, Vector3 distance, Vector3 rotAxis, float angle = 90)
    {
        isAnim = true;
        float time = 0;

        Vector3 fromPos = transform.position;
        Quaternion fromRot = transform.rotation;

        while(time < animTime)
        {
            time += Time.deltaTime;
            time = Mathf.Clamp(time, 0, animTime);

            //Calculate roll pos based on animation curve
            float t = curve.Evaluate(map(time, 0, animTime, 0, 1));

            transform.rotation = Quaternion.Slerp(fromRot, Quaternion.AngleAxis(angle, rotAxis) * fromRot, t);

            //Calculate height offset from rotation
            float h = Mathf.Sin(Mathf.Deg2Rad*map(time, 0, animTime, 45, 135)) * Mathf.Sqrt(2) * .5f - .5f;

            transform.position = Vector3.Lerp(fromPos, fromPos + distance, t) + new Vector3(0, h, 0);
            yield return new WaitForEndOfFrame();
        }

        isAnim = false;

        //Check new bottom face
        Vector3[] faces = { -transform.forward, transform.right, transform.forward, -transform.right, transform.up, -transform.up};
        for (int i = 0; i < faces.Length; ++i)
        {
            if (System.Math.Abs(Vector3.Angle(faces[i], Vector3.down)) <= 0)
            {
                bottomFace = i;
                break;
            }
        }

        //Light current block that it's on top of
        Manager.instance.lightCurrentBlock();

        //Check bottom face color vs floor color at pos
        Manager.instance.checkLoss(bottomFace);

        //Check if win
        Manager.instance.checkWin();

        //Activate PS
        rollPS.transform.position = transform.position - Vector3.up * Manager.instance.cellSize / 2;

        ParticleSystemRenderer rend = rollPS.GetComponent<ParticleSystemRenderer>();
        Color col = Manager.instance.getColor(Manager.instance.getCurrentBoard()[Manager.instance.playerX][Manager.instance.playerY]);
        col.a = 1;

        rend.material.color = col * Mathf.Pow(2, 2);

        rollPS.GetComponent<ParticleSystem>().Play();
    }

    float map(float s, float a1, float a2, float b1, float b2)
    {
        return b1 + (s - a1) * (b2 - b1) / (a2 - a1);
    }

    public void removePS()
    {
        Destroy(rollPS);
    }
}
