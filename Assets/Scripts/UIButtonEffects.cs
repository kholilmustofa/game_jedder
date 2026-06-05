using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UIButtonEffects : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] private Image iconImage; // Tarik objek ButtonIcon ke sini
    
    [Header("Warna Ikon")]
    [SerializeField] private Color normalColor = Color.white; // Warna normal
    [SerializeField] private Color hoverColor = Color.green;   // Warna saat disorot mouse (Hover)
    [SerializeField] private Color pressedColor = Color.gray;  // Warna saat ditekan (Pressed)

    private void Start()
    {
        // Cari otomatis jika belum ditarik di Inspector
        if (iconImage == null)
        {
            iconImage = GetComponentInChildren<Image>();
        }
        
        if (iconImage != null)
        {
            iconImage.color = normalColor;
        }
    }

    // Dipanggil saat mouse masuk area tombol (Hover)
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (iconImage != null)
        {
            iconImage.color = hoverColor;
        }
    }

    // Dipanggil saat mouse keluar area tombol (Normal)
    public void OnPointerExit(PointerEventData eventData)
    {
        if (iconImage != null)
        {
            iconImage.color = normalColor;
        }
    }

    // Dipanggil saat tombol ditekan (Pressed)
    public void OnPointerDown(PointerEventData eventData)
    {
        if (iconImage != null)
        {
            iconImage.color = pressedColor;
        }
    }

    // Dipanggil saat tombol dilepas setelah ditekan
    public void OnPointerUp(PointerEventData eventData)
    {
        if (iconImage != null)
        {
            // Kembali ke warna hover karena kursor masih berada di atas tombol
            iconImage.color = hoverColor;
        }
    }
}
