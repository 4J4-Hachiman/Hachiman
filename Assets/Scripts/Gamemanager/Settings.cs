/* 
    Scripte de gestion des paramètres du jeu
        - Cursor lockState;
        - Framerate;

    Par : Yanis Oulmane;
    Dernière modification 01/03/2025;
*/

using UnityEngine;

public class SettingsManager : MonoBehaviour
{    
    [SerializeField] private int framerate;

    void Awake()
    {
        framerate = framerate == 0 ? 30 : framerate;

        Application.targetFrameRate = framerate;

        Cursor.lockState = CursorLockMode.Locked;
    }
}
