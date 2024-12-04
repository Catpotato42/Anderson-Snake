using UnityEngine;
using UnityEngine.EventSystems;

public class GlobalButtonDeselector : MonoBehaviour
{
    //full disclosure this is a ChatGPT script
    void Update()
    {
        // Check for mouse button clicks or touches
        if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1) || Input.touchCount > 0)
        {
            // Clear the currently selected UI object
            EventSystem.current.SetSelectedGameObject(null);
        }
    }
}