/*  
 *  Fonctionnement et utilité générale du script
    
    Script la selection d'un bouton par défaut 
    Utile pour les boutons du menu commandes
        Par : Malaïka Abevi
        Dernière modification : 26/04/2025
*/
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
public class BntSelectedDefaut : MonoBehaviour
{

    // public Button bntDefaut; // Le bouton sélectionné par défaut
    // private GameObject itemSelectionne;
    public Color32 couleurSelected;
    public Color32 couleurBase;
    public TextMeshProUGUI[] lesBnts;
    void Start()
    {
        // SelectBnt(bntDefaut);
        // lesBnts[0].color = couleurSelected;
    }

    // void Update()
    // {
    //     // Ré-sélectionner le dernier bouton si rien n'est sélectionné
    //     if (EventSystem.current.currentSelectedGameObject == null && itemSelectionne != null)
    //     {
    //         EventSystem.current.SetSelectedGameObject(itemSelectionne.gameObject);
    //     }
    // }

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
    // public void SelectBnt(GameObject bnt)
    // {
    //     // Sélectionne le nouveau bouton
    //     EventSystem.current.SetSelectedGameObject(bnt.gameObject);

    //     // Met à jour le bouton sélectionné en dernier
    //     itemSelectionne = bnt;
    // }

    // public void ChangementBntClique(GameObject btnClique)
    // {
    //     // Appelé lorsque l'utilisateur clique sur un bouton
    //     SelectBnt(btnClique);
    // }
}




