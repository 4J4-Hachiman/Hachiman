/*  
 *  Fonctionnement et utilité générale du script
    
    Gestion de la FinPartie du joueur au niveau du UI
        Par : Malaïka Abevi
        Dernière modification : 17/05/2025
*/
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class GestionFinPartie : MonoBehaviour
{
    public static bool partieFini;
    public EventSystem eventSystem;
    public GameObject bntFirst;

    void Start()
    {
        partieFini = false;
    }
    public void ArreterJeu()
    {
        Invoke("AffichageFinPartie", 1f);
        StartCoroutine(RalentirJeu());
        partieFini = true;
    }

    IEnumerator RalentirJeu()
    {
        float scale = 1;
        while (scale > 0f)
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

    void AffichageFinPartie()
    {
        gameObject.GetComponent<Animator>().SetTrigger("finPartie");
        Cursor.lockState = CursorLockMode.Confined;
    }
}
