/*  
 *  Fonctionnement et utilité générale du script
    
    Script pour le contrôle du menu des options (UI/UX)
        Par : Malaïka Abevi
        Dernière modification : 26/04/2025
*/
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SonBntSurvolUI : MonoBehaviour, IPointerEnterHandler, ISelectHandler
{
	public BanqueAudio banqueAudio;
	AudioManager audioManager;

	void Start()
	{
		// On veut trouver le gameObject qui contient le script unique de AudioManager
		audioManager = FindObjectOfType<AudioManager>();
	}

	// Fonction appelée à chaque fois que le bouton est survolé
	public void OnPointerEnter(PointerEventData eventData)
	{
		// Vérifie si le GameObject survolé possède un componant Button
		Button bnt = GetComponent<Button>();
		if (bnt.interactable && bnt.gameObject != EventSystem.current.currentSelectedGameObject)
		{
			// On joue le son des boutons survolés
			audioManager.gameObject.GetComponent<AudioManager>().JouerSonBoutonUI(banqueAudio.bntUI);
		}
	}

	//Détecter quand un élément du UI est séléctionné
	public void OnSelect(BaseEventData eventData)
	{
		// On joue le son des boutons survolés
		if (!Input.GetMouseButtonDown(0))
		{
			audioManager.gameObject.GetComponent<AudioManager>().JouerSonBoutonUI(banqueAudio.bntUI);
		}

	}
}
