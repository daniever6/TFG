using _Scripts.Utilities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorChanger : GameplayMonoBehaviour<CursorChanger>
{
    [SerializeField] private Texture2D originalCursor;
    [SerializeField] private Texture2D groundcursor;

    private Vector2 originalHotspot = new(24,0);
    private Vector2 groundHotspot = new(25, 13);

    private void Start()
    {
        Cursor.SetCursor(originalCursor, originalHotspot, CursorMode.ForceSoftware);
    }


    private void OnMouseEnter()
    {
        Cursor.SetCursor(groundcursor, groundHotspot, CursorMode.ForceSoftware);
    }

    private void OnMouseExit()
    {
        Cursor.SetCursor(originalCursor, originalHotspot, CursorMode.ForceSoftware);
    }
}
