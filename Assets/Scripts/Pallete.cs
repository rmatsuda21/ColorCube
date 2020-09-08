using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Pallete : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{

    private ColorSelector cs;

    Image img;
    Vector3 origPos;
    public Color col;
    public int val = 0;
    Color tCol;

    bool selected  = false;

    void Start()
    {
        col = Manager.instance.cols[val];
        cs = transform.parent.parent.parent.GetComponent<ColorSelector>();

        origPos = transform.position;
        img = GetComponent<Image>();
        tCol = col;
    }

    public void setColor(Color col)
    {
        this.col = col;
        tCol = col;
    }

    public Color getColor() { return col; }

    void Update()
    {
        img.color = Vector4.MoveTowards(img.color, tCol, Time.deltaTime * 10);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        tCol = col * Mathf.Pow(2, -.5f);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        tCol = col;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        ColorSelector cs = transform.parent.parent.parent.GetComponent<ColorSelector>();
        
        if(cs.selectedPallete == this){
            selected = false;
            cs.selectedPallete = null;

            toggleBorder(false);
        }
        else{
            if(cs.selectedPallete != null){
                cs.selectedPallete.toggleBorder(false);
            }
            selected = true;
            cs.selectedPallete = this;

            toggleBorder(true);
        }
    }

    void toggleBorder(bool active){
        Transform[] ts = transform.GetComponentsInChildren<RectTransform>(true);

        foreach(Transform t in ts){
            if(t!=transform)
                t.gameObject.SetActive(active);
        }
    }
}
