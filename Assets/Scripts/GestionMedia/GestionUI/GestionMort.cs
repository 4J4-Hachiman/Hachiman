/*  
 *  Fonctionnement et utilité générale du script
    
    Gestion de la mort du joueur au niveau du UI
        Par : Malaïka Abevi
        Dernière modification : 01/04/2025
*/
using System.Collections;
using UnityEngine;

public class GestionMort : MonoBehaviour
{

    public void ArreterJeu()
    {
        Invoke("AffichageMort", 1f);
        StartCoroutine(RalentirJeu());
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

    void AffichageMort(){
        gameObject.GetComponent<Animator>().enabled = true;
        Cursor.lockState = CursorLockMode.Confined;
    }
}
