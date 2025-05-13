/*  
 *  Fonctionnement et utilité générale du script
    
    Script pour mettre en pause le jeu
        Par : Malaïka Abevi
        Dernière modification : 10/05/2025
*/
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class GestionPause : MonoBehaviour
{
    public bool enPause;
    public GameObject menuPause;

    public CanvasGroup HUD;
    // public GameObject HUD;
    public VideoPlayer videoCinematiques;
    public GameObject menuOptions;
    public GameObject menuCommandes;
    public Scene scenePartie;

    void Start()
    {
        enPause = false;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P) || Input.GetKeyDown(KeyCode.Escape) && !SceneActiveManager.changementSceneEnCours && !GestionMort.estMort)
        {
            MettreEnPause();
        }
    }

    //Fonction pour mettre en pause le jeu 
    public void MettreEnPause()
    {
        if (!enPause)
        {
            print("le jeu est en pause");
            enPause = true;
            menuPause.SetActive(true);
            HUD.alpha = 0;
            Time.timeScale = 0;
            Cursor.lockState = CursorLockMode.Confined;
            videoCinematiques.Pause();
        }
        else
        {
            print("le jeu est en cours");
            enPause = false;
            menuPause.SetActive(false);
            menuCommandes.SetActive(false);
            menuOptions.SetActive(false);
            HUD.alpha = 1;

            if (!ChangementCinematiques.cinematiqueEnCours)
            {
                Time.timeScale = 1;
            }
            else
            {
                if(!ChangementCinematiques.cinematiqueTermine){
                    videoCinematiques.Play();
                }
            }

            Cursor.lockState = CursorLockMode.Locked;
        }
    }
}
