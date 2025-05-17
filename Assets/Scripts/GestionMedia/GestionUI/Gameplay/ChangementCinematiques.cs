/*  
 *  Fonctionnement et utilité générale du script
    
    Script pour la gestion des cinématiques
        Par : Malaïka Abevi
        Dernière modification : 14/05/2025
*/
using UnityEngine;
using UnityEngine.Video;

public class ChangementCinematiques : MonoBehaviour
{
  //Variables pour le fonctionnement des cinématiques
  public VideoPlayer videoPlayer;
  public Animator animCinematique;
  public Animator animHUD;
  public static bool cinematiqueEnCours;
  public static bool cinematiqueTermine;
  public float tempsVideo;
  public float tempsJoue;

  //Videoclips des cinématiques
  public VideoClip cinematiqueIntroduction;
  public VideoClip cinematiqueIndiceBague;
  public VideoClip cinematiqueDecouvertBague;
  public VideoClip cinematiqueIndiceAmulette;
  public VideoClip cinematiqueDecouvertAmulette;
  public VideoClip cinematiquePortail;
  public VideoClip cinematiqueConfrontation;
  public VideoClip cinematiqueFin;


  void Start()
  {
    tempsVideo = Mathf.Floor((float)videoPlayer.clip.length);
    cinematiqueEnCours = false;
    cinematiqueTermine = false;
  }

  void Update()
  {
    tempsJoue = (float)videoPlayer.time;

    if (cinematiqueEnCours)
    {
      if (tempsJoue >= tempsVideo)
      {
        Debug.Log("//do Stuff");
        ArreterCinematique();
      }
    }
  }

  //Fonction pour démarrer une cinématiques et en faire la gestion
  public void DemarrerCinematique(VideoClip cinematique)
  {
    videoPlayer.clip = cinematique;
    tempsVideo = Mathf.Floor((float)videoPlayer.clip.length);
    videoPlayer.Play();
    // print(cinematiqueEnCours);
    videoPlayer.playbackSpeed = 1;

    Time.timeScale = 0;

    cinematiqueTermine = false;
    cinematiqueEnCours = true;
    animHUD.enabled = true;

    animCinematique.SetTrigger("demarrerCine");
    animHUD.SetTrigger("disparaitre");
    print("La cinematique est partie!");
  }

  //Fonction pour arreter une cinématiques et en faire la gestion
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