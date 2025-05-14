/*  
 *  Fonctionnement et utilité générale du script

    Demande pour joueur les retours sonores du UI
        Par : Malaïka Abevi
        Dernière modification : 14/05/2025
*/
using UnityEngine;
using UnityEngine.EventSystems;

public class RetourAudioUI : MonoBehaviour, IDropHandler
{
    public BanqueAudio banqueAudio;

    //Fonction OnDrop pour faire un retour sonore de l'ajustement du volume d'effets sonore avec souris
    public void OnDrop(PointerEventData data)
    {
        if (data.pointerDrag != null)
        {
            AudioManager.instance.JouerSonBoutonUI(banqueAudio.retourOptionsSfx);
        }
    }

    // Fonction pour jouer le retour sonore de l'ajustement du volume d'effets sonore sur manette
    public void ChangementVolumeManette()
    {
        // On ne veut pas que le son joue de cette manière quand c'est une souris qui contrôle
        if (!Input.GetMouseButton(0))
        {
            AudioManager.instance.JouerSonBoutonUI(banqueAudio.retourOptionsSfx);
        }
    }
}