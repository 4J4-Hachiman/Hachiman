/*  
 *  Fonctionnement et utilit� g�n�rale du script

    Demande pour joueur les retours sonores du UI
        Par : Mala�ka Abevi
        Derni�re modification : 15/04/2025
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

    public void JouerSonUI()
    {
        AudioManager.instance.JouerSonBoutonUI(banqueAudio.bntUI);
    }
}