/*  
 *  Fonctionnement et utilit� g�n�rale du script

    Affichage du niveau de vie et du niveau de mana dans le UI
    Transition fluide des niveaux des barres.
        Par : Mala�ka Abevi
        Derni�re modification : 05/04/2025
*/
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ControlesVieMana : MonoBehaviour
{
    public Image fillVie;
    public Image fillMana;
    public Image fillVieBoss;

    public void AffichageNiveauVie(float vieTotale, float vieActuelle, float changementVie)
    {
        float depart = vieActuelle / vieTotale;
        float cible = (vieActuelle + changementVie) / vieTotale;
        StopCoroutine(TransitionVie(vieTotale, depart, cible));
        StartCoroutine(TransitionVie(vieTotale, depart, cible));
    }

    public void AffichageNiveauMana(float manaTotale, float manaActuelle, float changementMana)
    {
        float depart = manaActuelle / manaTotale;
        float cible = (manaActuelle + changementMana) / manaTotale;
        StopCoroutine(TransitionMana(manaTotale, depart, cible));
        StartCoroutine(TransitionMana(manaTotale, depart, cible));
    }


    IEnumerator TransitionVie(float vieTotale, float depart, float cible)
    {
        float pourcentage = 0.01f;
        bool transitionFait = false;
        if (cible < depart)
        {
            transitionFait = true;
            while (fillVie.fillAmount > cible)
            {
                Debug.LogError(fillVie.fillAmount + " vs " + cible);
                fillVie.fillAmount = Mathf.Lerp(depart, cible, pourcentage);
                pourcentage += 0.01f;
                yield return null;
            }
        }
        else if (cible > depart && !transitionFait)
        {
            while (fillVie.fillAmount < cible)
            {
                fillVie.fillAmount = Mathf.Lerp(depart, cible, pourcentage);
                pourcentage += 0.01f;
                yield return null;
            }
        }

        yield return null;
    }

    IEnumerator TransitionMana(float manaTotale, float depart, float cible)
    {
        float pourcentage = 0.01f;
        bool transitionFait = false;
        if (cible < depart)
        {
            transitionFait = true;
            while (fillMana.fillAmount > depart / manaTotale || fillMana.fillAmount != depart / manaTotale)
            {

                fillMana.fillAmount = Mathf.Lerp(depart, cible, pourcentage);
                pourcentage += 0.01f;
                yield return null;
            }
        }
        else if (cible > depart && !transitionFait)
        {
            while (fillMana.fillAmount < depart / manaTotale || fillMana.fillAmount != depart / manaTotale)
            {
                fillMana.fillAmount = Mathf.Lerp(depart, cible, pourcentage);
                pourcentage += 0.01f;
                yield return null;
            }
        }
        yield return null;
    }

    // public void AffichageNiveauVieBoss(float vieBossTotale, float vieBossActuelle)
    // {
    //     fillVieBoss.fillAmount = vieBossActuelle / vieBossTotale;
    // }

    // public void AugmenterCapaciteVie()
    // {

    // }

    // public void AugmenterCapaciteMana()
    // {

    // }
}
