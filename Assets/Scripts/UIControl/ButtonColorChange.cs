using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ButtonColorChange : MonoBehaviour , IPointerEnterHandler, IPointerExitHandler
{
    public Text buttonText; // Text component của Button
    public Color normalColor = Color.white; // Màu chữ bình thường
    public Color hoverColor = Color.red; // Màu chữ khi hover

    // Hàm này được gọi khi con trỏ chuột đi vào Button
    public void OnPointerEnter(PointerEventData eventData)
    {
        buttonText.color = hoverColor;
    }

    // Hàm này được gọi khi con trỏ chuột rời khỏi Button
    public void OnPointerExit(PointerEventData eventData)
    {
        buttonText.color = normalColor;
    }
}
