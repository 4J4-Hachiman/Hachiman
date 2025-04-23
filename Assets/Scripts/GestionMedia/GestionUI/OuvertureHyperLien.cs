/*  
 *  Fonctionnement et utilit� g�n�rale du script

    Fonction pour ouvrir des liens sur le navigateur
        Par : Mala�ka Abevi
        Derni�re modification : 10/04/2025
*/
using UnityEngine;

public class OuvertureHyperLien : MonoBehaviour
{
    public void OuvrirLien(string hyperlien) => Application.OpenURL(hyperlien);
}
