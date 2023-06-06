using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mouse : MonoBehaviour
{
    void Start()
    {
        Cursor.visible = false;
    }

    // Start is called before the first frame update
    void Update()
    {
        Cursor.visible = false;
        // Get the position of the cursor in screen coordinates
        Vector3 cursorScreenPosition = Input.mousePosition;

        // Convert the cursor position from screen coordinates to world coordinates
        Vector3 cursorWorldPosition = Camera.main.ScreenToWorldPoint(cursorScreenPosition);
        cursorWorldPosition.z = 0f; // Set the Z position to 0 to ensure the crosshair is on the same plane as the game objects

        // Update the crosshair position to follow the cursor
        transform.position = cursorWorldPosition;
    }
}
