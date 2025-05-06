using UnityEngine;
using UnityEngine.Video;

public class ChangementCinematiques : MonoBehaviour
{
    public VideoClip cinematique1;
    public VideoClip cinematique2;
    public VideoPlayer videoPlayer;
    public Animator animCinematique;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    public void DemarrerCinematique(VideoClip cinematique){
        videoPlayer.clip = cinematique;
        videoPlayer.Play();
        animCinematique.SetTrigger("demarrerCine");
    }

    public void ArreterCinematique(){
        animCinematique.SetTrigger("arreterCine");
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
