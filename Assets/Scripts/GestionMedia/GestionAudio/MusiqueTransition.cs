/*  
 *  Fonctionnement et utilité générale du script

    Demande de changement de musique
        Par : Malaïka Abevi
        Derniï¿½re modification : 15/04/2025
*/
using UnityEngine;

public class MusiqueTransition : MonoBehaviour
{
    public void ChangerMusique(AudioClip clipAudio){
        AudioManager.instance.ChangementMusique(clipAudio);
    }
}
