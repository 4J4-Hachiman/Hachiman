/*  
 *  Fonctionnement et utilité générale du script

    Fonction pour définir un élément sélectionné quand il n'y en a pas 
    Nécéssaire pour la navigation avec manette
        Par : Malaïka Abevi
        Dernière modification : 14/05/2025
*/
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SwitchSelectedUI : MonoBehaviour
{
    public Selectable prochainSelected;
    public EventSystem eventSystem;

    void Update()
    {
        if(EventSystem.current.currentSelectedGameObject == null){
            eventSystem.SetSelectedGameObject(prochainSelected.gameObject);
        }
    }
}
