/*  
 *  Fonctionnement et utilité générale du script

    Demande pour joueur les retours sonores du UI
        Par : Malaïka Abevi
        Dernière modification : 26/04/2025
*/
using UnityEngine;
using UnityEngine.EventSystems;

public class RetourAudioUI : MonoBehaviour, IDropHandler
{
    public BanqueAudio banqueAudio;
    public void OnDrop(PointerEventData data)
    {
        if (data.pointerDrag != null)
        {
            AudioManager.instance.JouerSonBoutonUI(banqueAudio.retourOptionsSfx);
        }
    }
}