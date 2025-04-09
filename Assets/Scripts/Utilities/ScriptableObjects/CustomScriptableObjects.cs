using UnityEngine;

/*
    Scriptable object pour la gestion des paramètres 
    de l'utilisateur.
    
    ************************************************************
    Par: Yanis Oulmane;
    Dernière modification: 25/03/2025;
*/

namespace Custom.CSO
{
    /// <summary>Sciptable pour memoriser les donnes des settings du jeu.</summary>
    [CreateAssetMenu(fileName = "Settings", menuName = "Custom Scriptable Objects/User Settings")]
    [System.Serializable]
    public class Settings : ScriptableObject
    {   
        /* ------------ VARIABLES ------------ */
        public readonly int defaultFps = 60;
        /* ======================================== */
        public Settings()
        {   

        }
        /* ======================================== */

        /* ------------- METHODS ------------- */
        public void SetFramerate(int framerate) 
        {
            Application.targetFrameRate = framerate == 0 ? defaultFps : framerate;
        }

        public int GetCurrentFramerate()
        {
            return Application.targetFrameRate;
        }
    }
}


