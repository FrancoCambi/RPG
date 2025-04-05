using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class CharacterPanelButton : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{

    [SerializeField]
    private Image background;

    // Start is called before the first frame update
    void Start()
    {
        background = GetComponent<Image>();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            CharacterPanel.Instance.OpenClose();
        }


    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        background.color = new Color(255, 128, 0, 255);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        background.color = Color.white;
    }
}
