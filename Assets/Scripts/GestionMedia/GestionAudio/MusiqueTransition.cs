/*  
 *  Fonctionnement et utilit� g�n�rale du script

    Demande de changement de musique
        Par : Mala�ka Abevi
        Derni�re modification : 15/04/2025
*/
using Unity.VisualScripting;
using UnityEngine;

public class MusiqueTransition : MonoBehaviour
{
    public void ChangerMusique(AudioClip clipAudio){
        AudioManager.instance.ChangementMusique(clipAudio);
    }
}
