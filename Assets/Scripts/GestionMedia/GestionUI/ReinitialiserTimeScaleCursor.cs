using UnityEngine;

public class ReinitialiserTimeScaleCursor : MonoBehaviour
{
    void Start()
    {
        Cursor.lockState = CursorLockMode.Confined;
        Time.timeScale = 1;
    }
}
