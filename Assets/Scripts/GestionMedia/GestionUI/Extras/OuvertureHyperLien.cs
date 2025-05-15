/*  
 *  Fonctionnement et utilité générale du script

    Fonction pour ouvrir des liens sur le navigateur
        Par : Malaïka Abevi
        Dernière modification : 10/04/2025
*/
using UnityEngine;

public class OuvertureHyperLien : MonoBehaviour
{
    public void OuvrirLien(string hyperlien) => Application.OpenURL(hyperlien);
}
