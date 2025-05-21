/*  
 *  Fonctionnement et utilité générale du script
    
    Gestion de la mort du joueur au niveau du UI
        Par : Malaïka Abevi
        Dernière modification : 17/05/2025
*/
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class GestionMort : MonoBehaviour
{
    public static bool estMort;
    public EventSystem eventSystem;
    public GameObject bntSelected;
    public MusiqueTransition musiqueTransition;
    public BanqueAudio banqueAudio;

    void Start()
    {
        estMort = false;
    }
    public void ArreterJeu()
    {
        if (!estMort)
        {
            print("appel de fin de jeu");
            Invoke("AffichageMort", 1f);
            StartCoroutine(RalentirJeu());
            musiqueTransition.ChangerMusique(banqueAudio.mscMort);
            estMort = true;
        }
    }

    IEnumerator RalentirJeu()
    {
        float scale = 1;
        while (/*scale > 0f*/ !Mathf.Approximately(scale, 1f))
        {
            scale -= 1 / 4f * Time.unscaledDeltaTime;
            if (scale < 0)
            {
                scale = 0;
                Time.timeScale = scale;
                break;
            }

            Time.timeScale = scale;

            print(Time.timeScale);
            yield return null;
        }
    }

    void AffichageMort()
    {
        gameObject.GetComponent<Animator>().SetTrigger("mort");
        gameObject.GetComponent<CanvasGroup>().blocksRaycasts = true;
        gameObject.GetComponent<CanvasGroup>().interactable = true;
        Cursor.lockState = CursorLockMode.Confined;
        eventSystem.SetSelectedGameObject(bntSelected);
    }
}
