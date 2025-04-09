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

public class MenuManager : MonoBehaviour
{
    public GameObject[] lesMenus;   //Tableau pour enregistrer les canvas pour la sc�nes d'instructions
    public Transform[] lesTargetsCam; //Tableau pour enregistrer les positions des targets de la follow camera
    public GameObject FollowCam;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Confined;
        Time.timeScale = 1;
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

    //Fonction pour attribuer un target � la cam�ra du menu principal qui fait des plans sur le personnage
    public void ChangerTargetCam(Transform target)
    {
        FollowCam.GetComponent<CinemachineCamera>().Target.TrackingTarget = target;
    }
}
