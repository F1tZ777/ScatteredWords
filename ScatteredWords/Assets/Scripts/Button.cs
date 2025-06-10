using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class Button : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public UnityEvent Hover, OutHover;
    public TMP_Text text;

    // Start is called before the first frame update
    void Start()
    {
        Hover.AddListener(OnHover);
        OutHover.AddListener(OffHover);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        Hover.Invoke();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        OutHover.Invoke();
    }

    void OnHover()
    {
        text.color = Color.white;
    }

    void OffHover()
    {
        text.color= Color.black;
    }

    public void Disappear()
    {
        this.gameObject.SetActive(false);
    }    
}
