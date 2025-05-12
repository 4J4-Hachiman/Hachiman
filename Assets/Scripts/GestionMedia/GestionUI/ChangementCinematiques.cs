/*  
 *  Fonctionnement et utilité générale du script
    
    Script pour la gestion des cinématiques
        Par : Malaïka Abevi
        Dernière modification : 11/05/2025
*/
using UnityEngine;
using UnityEngine.Video;

public class ChangementCinematiques : MonoBehaviour
{
  public VideoClip cinematique1;
  public VideoClip cinematique2;
  public VideoPlayer videoPlayer;
  public Animator animCinematique;
  public Animator animHUD;

  public static bool cinematiqueEnCours;
  public static bool cinematiqueTermine;
  public float tempsVideo;
  public float tempsJoue;

  void Start()
  {
    tempsVideo = Mathf.Floor((float)videoPlayer.clip.length);
    cinematiqueEnCours = false;
    cinematiqueTermine = false;
  }

  void Update()
  {
    tempsJoue = (float)videoPlayer.time;
    if (tempsJoue >= tempsVideo)
    {
      Debug.Log("//do Stuff");
      ArreterCinematique();
    }
  }

  public void DemarrerCinematique(VideoClip cinematique)
  {
    videoPlayer.clip = cinematique;
    tempsVideo = Mathf.Floor((float)videoPlayer.clip.length);
    videoPlayer.Play();

    Time.timeScale = 0;

    cinematiqueTermine = false;
    cinematiqueEnCours = true;
    animHUD.enabled = true;

    animCinematique.SetTrigger("demarrerCine");
    animHUD.SetTrigger("disparaitre");
  }

  public void ArreterCinematique()
  {
    videoPlayer.Stop();

    Time.timeScale = 1;

    cinematiqueEnCours = false;
    cinematiqueTermine = true;

    animCinematique.SetTrigger("finirCine");
    animHUD.SetTrigger("afficher");
    Invoke("ReafficherHUD", 5f);

    print("La cinématique est arreter");
  }

  void ReafficherHUD()
  {
    animHUD.enabled = false;
  }
}


/*tempsJoue >= tempsVideo*/ 