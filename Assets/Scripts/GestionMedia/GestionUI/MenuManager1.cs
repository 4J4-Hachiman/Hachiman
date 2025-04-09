/*  
 *  Fonctionnement et utilit� g�n�rale du script

[Gestionnaire]    

    Gestion de l'état de pause du jeu et des menus affichés
        Par : Mala�ka Abevi
        Derni�re modification : 30/03/2025
*/
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager1 : MonoBehaviour
{
    public GameObject[] lesMenus;   //Tableau pour enregistrer les canvas pour la sc�nes d'instructions
    public Transform[] lesTargetsCam; //Tableau pour enregistrer les positions des targets de la follow camera
    public GameObject FollowCam;
    public bool enPause;
    public GameObject menuPause;
    public GameObject HUD;
    Scene sceneActuelle;

    void Start()
    {
        sceneActuelle = SceneManager.GetActiveScene();
        print(sceneActuelle.name);
        enPause = false;
    }

    void Update()
    {
        //Le bouton P (pause) sera seulement utilisable dans la sc�ne de jeu
        if (sceneActuelle.name == "_Level_1_Scene" || sceneActuelle.name == "_TestChangementScene")
        {
            //print("C'est la partie");
            if (Input.GetKeyDown(KeyCode.P))
            {
                MettreEnPause();
            }
        }
        if(sceneActuelle.name == "_Level_1_Scene"){
            Cursor.lockState = CursorLockMode.Confined;
        }
    }

    //Fonction pour changer de menu affich�
    public void ChangerMenu(GameObject menuChoisi) {
        foreach (GameObject menu in lesMenus)
        {
            if (menu.name == menuChoisi.name)
            {
                menu.SetActive(true);
            }
            else
            {
                menu.SetActive(false);
            }
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
            HUD.SetActive(true);
            Time.timeScale = 1;
            Cursor.lockState = CursorLockMode.Locked;
        }
    }

    //Fonction pour attribuer un target � la cam�ra du menu principal qui fait des plans sur le personnage
    public void ChangerTargetCam(Transform target)
    {
        FollowCam.GetComponent<CinemachineCamera>().Target.TrackingTarget = target;
    }
}
