/*  
 *  Fonctionnement et utilité générale du script
    
    Script pour la gestion des cinématiques
        Par : Malaïka Abevi
        Dernière modification : 14/05/2025
*/

using System.Collections;
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

  void Start()
  {
    tempsVideo = Mathf.Floor((float)videoPlayer.clip.length);
    cinematiqueEnCours = false;
    cinematiqueTermine = false;
  }

  // void Update()
  // {
  //   tempsJoue = (float)videoPlayer.time;

  //   if (cinematiqueEnCours)
  //   {
  //     if (tempsJoue >= tempsVideo)
  //     {
  //       Debug.Log("//do Stuff");
  //       ArreterCinematique();
  //     }
  //   }
  // }

  //Fonction pour démarrer une cinématiques et en faire la gestion
  public void DemarrerCinematique(VideoClip cinematique)
  {
    videoPlayer.clip = cinematique;
    tempsVideo = Mathf.Floor((float)videoPlayer.clip.length);
    videoPlayer.Play();
    // print(cinematiqueEnCours);
    videoPlayer.playbackSpeed = 1;

    // Time.timeScale = 0;

    cinematiqueTermine = false;
    cinematiqueEnCours = true;
    animHUD.enabled = true;

    animCinematique.SetTrigger("demarrerCine");
    animHUD.SetTrigger("disparaitre");
    // print("La cinematique est partie!");
    // StartCoroutine("RalentirJeu");
    StartCoroutine(RalentirJeu());
    StartCoroutine(ArreterCinematique(tempsVideo));
  }

  //Fonction pour arreter une cinématiques et en faire la gestion
  public IEnumerator ArreterCinematique(float time)
  {
    yield return new WaitForSecondsRealtime(time);
    videoPlayer.Stop();

    // Time.timeScale = 1;

    cinematiqueEnCours = false;
    cinematiqueTermine = true;

    animCinematique.SetTrigger("finirCine");
    animHUD.SetTrigger("afficher");
    Invoke("ReafficherHUD", 5f);

    print("La cinématique est arreter");
    // StartCoroutine("AccelererJeu");
    StartCoroutine(AccelererJeu());
  }

  IEnumerator RalentirJeu()
  {
    Time.timeScale = 0;
    yield break;

    // float scale = 1;
    // while (scale > 0f)
    // {
    //   scale -= 1 / 4f * Time.unscaledDeltaTime;
    //   if (scale < 0)
    //   {
    //     scale = 0;
    //     Time.timeScale = scale;
    //     break;
    //   }

    //   Time.timeScale = scale;

    //   // print(Time.timeScale);
    //   yield return null;
    // }
  }

  IEnumerator AccelererJeu()
  {
    Time.timeScale = 1;
    yield break;

    // float scale = 0;
    // while (scale < 1f)
    // {
    //   scale += 1 / 4f * Time.unscaledDeltaTime;
    //   if (scale > 1)
    //   {
    //     scale = 1;
    //     Time.timeScale = scale;
    //     break;
    //   }

    //   Time.timeScale = scale;
    //   // print(Time.timeScale);
    //   yield return null;
    // }
  }

  void ReafficherHUD()
  {
    animHUD.enabled = false;
  }
}


/*tempsJoue >= tempsVideo*/ 