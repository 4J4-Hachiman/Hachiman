using UnityEngine;
using UnityEngine.SceneManagement;

public class GestionPause : MonoBehaviour
{
    public bool enPause;
    public GameObject menuPause;
    public GameObject HUD;
    public GameObject menuOptions;
    public GameObject menuCommandes;
    public Scene scenePartie;

    void Start()
    {
        enPause = false;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
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
            HUD.SetActive(false);
            Time.timeScale = 0;
            Cursor.lockState = CursorLockMode.Confined;
        }
        else
        {
            print("le jeu est en cours");
            enPause = false;
            menuPause.SetActive(false);
            menuCommandes.SetActive(false);
            menuOptions.SetActive(false);
            HUD.SetActive(true);
            Time.timeScale = 1;
            Cursor.lockState = CursorLockMode.Locked;
        }
    }
}
