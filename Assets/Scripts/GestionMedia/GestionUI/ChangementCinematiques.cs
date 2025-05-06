/*  
 *  Fonctionnement et utilité générale du script
    
    Script pour la gestion des cinématiques
        Par : Malaïka Abevi
        Dernière modification : 06/05/2025
*/
using UnityEngine;
using UnityEngine.Video;

public class ChangementCinematiques : MonoBehaviour
{
    public VideoClip cinematique1;
    public VideoClip cinematique2;
    public VideoPlayer videoPlayer;
    public Animator animCinematique;

    public void DemarrerCinematique(VideoClip cinematique)
    {
        videoPlayer.clip = cinematique;
        videoPlayer.Play();
        animCinematique.SetTrigger("demarrerCine");
    }

    public void ArreterCinematique()
    {
        videoPlayer.Stop();
        animCinematique.SetTrigger("finirCine");
    }
}
