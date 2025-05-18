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
    public string direction;

    // Update is called once per frame
    void Update()
    {
        /*  
            On vérife si l'élément intéractible suivant est disponible.
            Dans le cas contraire, on change de destination pour l'élément suivant
        */
        if (direction == "Up") SwitchOnSelectUp();
        if (direction == "Down") SwitchOnSelectDown();
        if (direction == "Left") SwitchOnSelectLeft();
        if (direction == "Right") SwitchOnSelectRight();
    }

    void SwitchOnSelectUp()
    {
        if (prochainElementDefaut.interactable && prochainElementDefaut.gameObject.activeInHierarchy)
        {
            navigation.selectOnUp = prochainElementDefaut;
        }
        else
        {
            if (prochainElmNew.interactable && prochainElmNew.gameObject.activeInHierarchy)
            {
                navigation.selectOnUp = prochainElmNew;
            }
            else
            {
                navigation.selectOnUp = prochainElmFinal;
            }
        }
        element.navigation = navigation;
    }

    void SwitchOnSelectDown()
    {
        if (prochainElementDefaut.interactable && prochainElementDefaut.gameObject.activeInHierarchy)
        {
            navigation.selectOnDown = prochainElementDefaut;
        }
        else
        {
            if (prochainElmNew.interactable && prochainElmNew.gameObject.activeInHierarchy)
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

    void SwitchOnSelectLeft()
    {
        if (prochainElementDefaut.interactable && prochainElementDefaut.gameObject.activeInHierarchy)
        {
            navigation.selectOnLeft = prochainElementDefaut;
        }
        else
        {
            if (prochainElmNew.interactable && prochainElmNew.gameObject.activeInHierarchy)
            {
                navigation.selectOnLeft = prochainElmNew;
            }
            else
            {
                navigation.selectOnLeft = prochainElmFinal;
            }
        }
        element.navigation = navigation;
    }

    void SwitchOnSelectRight()
    {
        if (prochainElementDefaut.interactable && prochainElementDefaut.gameObject.activeInHierarchy)
        {
            navigation.selectOnRight = prochainElementDefaut;
        }
        else
        {
            if (prochainElmNew.interactable && prochainElmNew.gameObject.activeInHierarchy)
            {
                navigation.selectOnRight = prochainElmNew;
            }
            else
            {
                navigation.selectOnRight = prochainElmFinal;
            }
        }
        element.navigation = navigation;
    }
}
