/*  
 *  Fonctionnement et utilité générale du script

    Affichage du niveau de vie et du niveau de mana dans le UI
    Transition fluide des niveaux des barres.
        Par : Malaïka Abevi
        Dernière modification : 05/04/2025
*/
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ControlesVieMana : MonoBehaviour
{
    public Image fillVie;
    public Image fillMana;
    public Image fillVieDelay;
    public Image fillManaDelay;
    public Image fillVieBoss;

    public TextMeshProUGUI textNbPotion;

    // Fonction pour gerer l'affichage de la vie
    public void AffichageNiveauVie(float vieTotale, float vieActuelle)
    {
        float liveAmount = vieActuelle / vieTotale;
        fillVie.fillAmount = liveAmount;
        fillVieDelay.fillAmount = Mathf.Lerp(fillVieDelay.fillAmount, liveAmount, 0.05f);
    }

    // Fonction pour gerer l'affichage du mana/ de l'endurance
    public void AffichageNiveauMana(float manaTotale, float manaActuelle)
    {
        float liveAmount = manaActuelle / manaTotale;
        fillMana.fillAmount = liveAmount;
        fillManaDelay.fillAmount = Mathf.Lerp(fillManaDelay.fillAmount, liveAmount, 0.05f);
    }

    // Fonction pour gÃ©rer l'affichage du nombre de potion
    public void QuantitePotionVie(int nbPotion)
    {
        textNbPotion.text = nbPotion.ToString();
    }
}
