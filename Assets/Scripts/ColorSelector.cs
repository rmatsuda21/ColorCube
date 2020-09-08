using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColorSelector : MonoBehaviour
{
    public GameObject currentSelected;
    public int currentIndex;

    GameObject pallete, palleteBack;

    public float size = 5f;

    Transform c;
    Transform openLayout;

    List<GameObject> palletes;

    public Pallete selectedPallete = null;

    private void Awake()
    {
        palletes = new List<GameObject>();

        pallete = Resources.Load("Color", typeof(GameObject)) as GameObject;
        palleteBack = Resources.Load("ColorBack", typeof(GameObject)) as GameObject;
    }

    // Start is called before the first frame update
    void Start()
    {
        c = transform.Find("Cube");
        openLayout = transform.Find("OpenLayout");

        //Color Cube
        foreach(Transform child in c.GetComponentsInChildren<Transform>())
        {
            if (child.tag != "IgnoreInManager")
            {
                child.localScale = new Vector3(size * .1f, size * .1f, size * .1f);
            }
        }
        c.GetChild(0).localPosition = new Vector3(0, 0, -size/2);
        c.GetChild(2).localPosition = new Vector3(0, 0, size / 2);

        c.GetChild(1).localPosition = new Vector3(size / 2, 0, 0);
        c.GetChild(3).localPosition = new Vector3(-size / 2, 0, 0);

        c.GetChild(4).localPosition = new Vector3(0, size/2, 0);
        c.GetChild(5).localPosition = new Vector3(0, -size/2, 0);

        c.GetChild(6).localScale = new Vector3(size - .001f, size - .001f, size - .001f);
        c.GetChild(6).localPosition = new Vector3(0, 0, 0);

        c.transform.localPosition = new Vector3(-3, 0, 0);

        //Open Layout
        

        for (int i = 0; i < 6; ++i)
        {
            Color col = Manager.instance.getColor(Manager.instance.getPlayerCols()[i]);
            c.GetChild(i).GetComponent<Renderer>().material.color = col;
            Material mat = new Material(Shader.Find("UI/Default"));
            mat.color = col;
            openLayout.GetChild(i).GetComponent<Image>().material = mat;
            openLayout.GetChild(i).GetComponent<OpenLayoutPanel>().col = col;
        }
    }

    public void createPallete()
    {
        int i = 1;
        GameObject p;
        GameObject pallete;
        GameObject back;
        foreach (int c in Manager.instance.levelCols)
        {
            p = new GameObject("Pallete");
            pallete = Instantiate(this.pallete);
            back = Instantiate(palleteBack);

            pallete.transform.localPosition = Vector3.zero;
            back.transform.localPosition = Vector3.zero;

            back.transform.SetParent(p.transform);
            pallete.transform.SetParent(p.transform);

            pallete.GetComponent<Pallete>().val = c;

            p.transform.parent = transform.GetChild(2);
            p.transform.localPosition = new Vector3(Screen.width/2 * i++ / (Manager.instance.levelCols.Count + 1) - Screen.width/2, -400, 0);

            palletes.Add(p);
        }
    }

    public void colorCurrentFace(float mult)
    {
        if (currentSelected)
        {
            Renderer rend;
            int index = currentSelected.transform.GetSiblingIndex();

            //Cube
            rend = currentSelected.GetComponent<Renderer>();
            rend.material.color = Manager.instance.getColor(Manager.instance.getPlayerCols()[index]) * mult;

            //Open Layout
            rend = openLayout.GetChild(index).GetComponent<Renderer>();
            rend.material.color = Manager.instance.getColor(Manager.instance.getPlayerCols()[index]) * mult;
        }
    }

    public void clearPallete()
    {
        foreach (GameObject p in palletes)
            Destroy(p);
    }

    public void colorAll(){
        if(selectedPallete!=null)
            foreach(OpenLayoutPanel ol in openLayout.GetComponentsInChildren<OpenLayoutPanel>())
                ol.setColor(selectedPallete.col);
    }
}
