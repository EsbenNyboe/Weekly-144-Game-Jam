using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems; // 1
using UnityEngine.UI;

public class UserInterfaceButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Image image;
    public static Color colorNormal;

    public Color colorHover;
    public static bool colorLoaded;

    void OnEnable()
    {
        if (image == null)
            image = GetComponent<Image>();

        if (!colorLoaded)
        {
            colorLoaded = true;
            colorNormal = image.color;
        }
    }
    void OnDisable()
    {
        image.color = colorNormal;
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        image.color = colorHover;
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        image.color = colorNormal;
    }
    public void ImageClicked()
    {
        image.color = colorNormal;
    }
}