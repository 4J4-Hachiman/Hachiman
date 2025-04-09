/*  
 *  Fonctionnement et utilit� g�n�rale du script

    Demande de changement de musique
    Demande pour joueur le son des boutons du UI
        Par : Mala�ka Abevi
        Derni�re modification : 30/03/2025
*/
using Unity.VisualScripting;
using UnityEngine;

public class MusiqueTransition : MonoBehaviour
{
    public BanqueAudio banqueAudio;

    public void ChangerMusique(AudioClip clipAudio){
        AudioManager.instance.ChangementMusique(clipAudio);
    }

    public void JouerSonUI(){
        AudioManager.instance.JouerSonBoutonUI(banqueAudio.bntUI);
    }
}
