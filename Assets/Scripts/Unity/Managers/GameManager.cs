using System.Collections;
using UnityEngine;


public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public Camera cameraUI;

    public Texture2D cursorTexture;

    public enum Type
    {
        Element,
        Spell
    }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else { Destroy(this.gameObject); }
    }

    private void Start()
    {
        Cursor.SetCursor(cursorTexture,Vector2.zero,CursorMode.Auto);
    }
}
