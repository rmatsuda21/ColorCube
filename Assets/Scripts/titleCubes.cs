using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class titleCubes : MonoBehaviour
{
    //Draw Color Cube with cubes!
    private GameObject cubePrefab;

    public Manager.Move[][] moveSets;
    public float size = .3f;

    public float animTime = 1.5f;

    public int[] playerCols = { 0, 1, 2, 3, 4, 5 };

    private float startX = -8f;
    private int[] widths = { 6, 4, 1, 4, 6, 6, 6, 5, 5 };
    private int[] startCords = {
        6, 7, //C
        4, 0, //o
        1, 0, //l
        4, 0, //o
        3, 5, //r

        5, 7, //C
        1, 5, //u
        1, 8, //b
        2, 3  //e
    };

    GameObject[] cubes;

    // Start is called before the first frame update
    void Start()
    {
        cubePrefab = Resources.Load("TitleCube", typeof(GameObject)) as GameObject;

        moveSets = new Manager.Move[][]{
            new Manager.Move[]{ Manager.Move.UP, Manager.Move.LEFT, Manager.Move.LEFT, Manager.Move.LEFT, Manager.Move.LEFT, Manager.Move.LEFT, Manager.Move.DOWN, Manager.Move.DOWN, Manager.Move.DOWN, Manager.Move.DOWN, Manager.Move.DOWN, Manager.Move.DOWN, Manager.Move.DOWN, Manager.Move.DOWN, Manager.Move.RIGHT, Manager.Move.RIGHT, Manager.Move.RIGHT, Manager.Move.RIGHT, Manager.Move.RIGHT, Manager.Move.UP }, //C
            new Manager.Move[]{ Manager.Move.UP, Manager.Move.UP, Manager.Move.UP, Manager.Move.LEFT, Manager.Move.LEFT, Manager.Move.LEFT, Manager.Move.DOWN, Manager.Move.DOWN, Manager.Move.DOWN, Manager.Move.RIGHT, Manager.Move.RIGHT, Manager.Move.RIGHT}, //o
            new Manager.Move[]{ Manager.Move.UP, Manager.Move.UP, Manager.Move.UP, Manager.Move.UP, Manager.Move.UP, Manager.Move.UP, Manager.Move.UP, Manager.Move.UP }, //l
            new Manager.Move[]{ Manager.Move.UP, Manager.Move.UP, Manager.Move.UP, Manager.Move.LEFT, Manager.Move.LEFT, Manager.Move.LEFT, Manager.Move.DOWN, Manager.Move.DOWN, Manager.Move.DOWN, Manager.Move.RIGHT, Manager.Move.RIGHT, Manager.Move.RIGHT}, //o
            new Manager.Move[]{ Manager.Move.LEFT, Manager.Move.LEFT, Manager.Move.DOWN, Manager.Move.DOWN, Manager.Move.DOWN, Manager.Move.DOWN, Manager.Move.DOWN }, //r

            new Manager.Move[]{ Manager.Move.UP, Manager.Move.LEFT, Manager.Move.LEFT, Manager.Move.LEFT, Manager.Move.LEFT, Manager.Move.LEFT, Manager.Move.DOWN, Manager.Move.DOWN, Manager.Move.DOWN, Manager.Move.DOWN, Manager.Move.DOWN, Manager.Move.DOWN, Manager.Move.DOWN, Manager.Move.DOWN, Manager.Move.RIGHT, Manager.Move.RIGHT, Manager.Move.RIGHT, Manager.Move.RIGHT, Manager.Move.RIGHT, Manager.Move.UP }, //C
            new Manager.Move[]{ Manager.Move.DOWN, Manager.Move.DOWN, Manager.Move.DOWN, Manager.Move.DOWN, Manager.Move.DOWN, Manager.Move.RIGHT, Manager.Move.RIGHT, Manager.Move.RIGHT, Manager.Move.RIGHT, Manager.Move.UP, Manager.Move.UP, Manager.Move.UP, Manager.Move.UP, Manager.Move.UP }, //u
            new Manager.Move[]{ Manager.Move.DOWN, Manager.Move.DOWN, Manager.Move.DOWN, Manager.Move.DOWN, Manager.Move.DOWN, Manager.Move.DOWN, Manager.Move.DOWN, Manager.Move.DOWN, Manager.Move.RIGHT, Manager.Move.RIGHT, Manager.Move.RIGHT, Manager.Move.RIGHT, Manager.Move.UP, Manager.Move.UP, Manager.Move.UP, Manager.Move.UP, Manager.Move.UP, Manager.Move.LEFT, Manager.Move.LEFT, Manager.Move.LEFT  }, //b
            new Manager.Move[]{ Manager.Move.RIGHT, Manager.Move.RIGHT, Manager.Move.RIGHT, Manager.Move.UP, Manager.Move.UP, Manager.Move.LEFT, Manager.Move.LEFT, Manager.Move.LEFT, Manager.Move.LEFT, Manager.Move.DOWN, Manager.Move.DOWN, Manager.Move.DOWN, Manager.Move.DOWN, Manager.Move.DOWN, Manager.Move.RIGHT, Manager.Move.RIGHT, Manager.Move.RIGHT, Manager.Move.RIGHT, Manager.Move.UP }, //e
        };

        createPlayers();
        startAnims();
    }

    // Update is called once per frame
    void Update()
    {
        //transform.LookAt(Camera.main.transform.position);
    }

    void createPlayers()
    {
        cubes = new GameObject[moveSets.Length];

        int i = 0;

        foreach (Manager.Move[] set in moveSets)
        {
            cubes[i] = Instantiate(cubePrefab);
            cubes[i].GetComponent<TitleCube_Controller>().moveSet = set;
            cubes[i].GetComponent<TitleCube_Controller>().speed = animTime / set.Length;

            cubes[i].transform.parent = transform;

            cubes[i].transform.localPosition = new Vector3(startX + startCords[i * 2]*size, startCords[i * 2 + 1]*size, 0);
            cubes[i].transform.localScale = new Vector3(size, size, size);

            startX += (widths[i] + 1) * size;

            cubes[i].GetComponent<CreateMesh>().CreateCube(playerCols, 0, 0, 50, 9, size, true);
            cubes[i].GetComponent<TitleCube_Controller>().drawFirstCube();
            i++;
        }
    }

    void startAnims()
    {
        int i = 0;
        foreach(GameObject p in cubes)
        {
            p.GetComponent<TitleCube_Controller>().movePlayer(moveSets[i++][0]);
        }
    }
}
