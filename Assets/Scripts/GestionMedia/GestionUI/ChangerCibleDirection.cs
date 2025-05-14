/*  
 *  Fonctionnement et utilité générale du script
    
    Script pour modifier la navigation dans le menu option selon la situation
        Par : Malaïka Abevi
        Dernière modification : 14/05/2025
*/
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ChangerCibleDirection : MonoBehaviour
{
    public Selectable element;
    public Selectable prochainElementDefaut;
    public Selectable prochainElmNew;
    public Selectable prochainElmFinal;
    public EventSystem eventSystem;
    public Navigation navigation;

    // Update is called once per frame
    void Update()
    {
        /*  
            On vérife si l'élément intéractible suivant est disponible.
            Dans le cas contraire, on change de destination pour l'élément suivant
        */
        if (prochainElementDefaut.interactable)
        {
            navigation.selectOnDown = prochainElementDefaut;
        }
        else
        {
            if (prochainElmNew.interactable)
            {
                navigation.selectOnDown = prochainElmNew;
            }
            else
            {
                navigation.selectOnDown = prochainElmFinal;
            }
        }
        element.navigation = navigation;
    }
}
