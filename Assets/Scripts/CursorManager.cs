using UnityEngine;

public class CursorManager : MonoBehaviour
{
    [Header("Cursor Textures")]
    [SerializeField] private Texture2D gameplayCursor; // Tarik Cursor_04 ke sini
    [SerializeField] private Texture2D menuCursor;     // Tarik Cursor_01 ke sini

    private void Start()
    {
        // Set kursor gameplay di awal game
        SetGameplayCursor();
    }

    public void SetGameplayCursor()
    {
        if (gameplayCursor != null)
        {
            // Hotspot ditaruh di tengah gambar (lebar/2, tinggi/2)
            Vector2 hotspot = new Vector2(gameplayCursor.width / 2f, gameplayCursor.height / 2f);
            Cursor.SetCursor(gameplayCursor, hotspot, CursorMode.Auto);
        }
    }

    public void SetMenuCursor()
    {
        if (menuCursor != null)
        {
            // Hotspot di pojok kiri atas (0, 0)
            Cursor.SetCursor(menuCursor, Vector2.zero, CursorMode.Auto);
        }
    }
}
