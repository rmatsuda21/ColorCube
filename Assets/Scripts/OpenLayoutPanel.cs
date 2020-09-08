using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class OpenLayoutPanel : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler,IPointerClickHandler 
{

    private ColorSelector cs;

    Image img;
    Vector3 origPos;
    public Color col;
    Color tCol;
    Transform[] gos;
    void Start()
    {
        gos = transform.GetComponentsInChildren<Transform>();
        foreach(Transform g in gos)
            if(g!=transform)
                g.gameObject.SetActive(false);

        cs = transform.parent.parent.GetComponent<ColorSelector>();

        origPos = transform.position;
        img = GetComponent<Image>();
        tCol = col;
        img.color = Color.white;
    }

    public void setColor(Color col)
    {
        this.col = col;
        tCol = col;
        img.color = col;
        Manager.instance.setPlayerCol(transform.GetSiblingIndex(), cs.selectedPallete.val);
    }

    public Color getColor() { return col; }

    void Update()
    {
        // img.color = Vector4.MoveTowards(img.color, tCol, Time.deltaTime * 1.5f);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        // tCol = col * Mathf.Pow(2, -.5f);
        foreach(Transform g in gos)
            if(g!=transform)
                g.gameObject.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        // tCol = col;
        foreach(Transform g in gos)
            if(g!=transform)
                g.gameObject.SetActive(false);    
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if(cs.selectedPallete != null){
            setColor(cs.selectedPallete.col);
        }    
    }

}
