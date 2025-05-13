/*  
 *  Fonctionnement et utilité générale du script
    
    Script l'affichage des items récoltés (notification)
        Par : Malaïka Abevi
        Dernière modification : 04/05/2025
*/
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class AffichageRecolteItems : MonoBehaviour
{
    public Image imgItem;
    public TextMeshProUGUI nomItem;
    public GameObject popUp;

    public void AfficherItemsRecolte(Sprite img, string nom)
    {
        imgItem.sprite = img;
        nomItem.text = nom;
        popUp.GetComponent<Animator>().SetTrigger("popUp");
    }
}

    // public Vector3 positionBasePopUp;
    // void OnEnable()
    // {
    //     popUp.GetComponent<RectTransform>().anchoredPosition = positionBasePopUp;
    // }