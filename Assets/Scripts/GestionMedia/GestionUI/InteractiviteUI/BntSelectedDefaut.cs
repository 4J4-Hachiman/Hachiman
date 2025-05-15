/*  
 *  Fonctionnement et utilité générale du script
    
    Script la selection d'un bouton par défaut 
    Utile pour les boutons du menu commandes
        Par : Malaïka Abevi
        Dernière modification : 26/04/2025
*/
using UnityEngine;
using TMPro;
public class BntSelectedDefaut : MonoBehaviour
{

    // public Button bntDefaut; // Le bouton sélectionné par défaut
    public Color32 couleurSelected;
    public Color32 couleurBase;
    public TextMeshProUGUI[] lesBnts;

    public void SelectionChangerBnt(int index)
    {
        foreach (TextMeshProUGUI bnt in lesBnts)
        {
            bnt.color = couleurBase;
        }

        lesBnts[index].color = couleurSelected;
    }

    public void ResetCouleurBnts()
    {
        foreach (TextMeshProUGUI bnt in lesBnts)
        {
            bnt.color = couleurBase;
        }
    }
}




