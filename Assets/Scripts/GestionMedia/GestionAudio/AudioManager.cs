/*  
 *  Fonctionnement et utilit� g�n�rale du script

[Gestionnaire]    
###[SINGLETON] 

    Gestion des pistes audios et transition fluide entre elles
    Contrôle de la piste de musique 1, piste de musique 2 et de la piste pour les vfx du UI
        Par : Mala�ka Abevi
        Derni�re modification : 30/03/2025
*/
using System.Collections;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;
    public AudioSource mPiste1;
    public AudioSource mPiste2;
    public AudioSource vfxPisteUI;
    public float vitesseTransition;
    void Start()
    {
        //Instancier le script / AudioManager et �viter les doublons
        if (instance == null)
        {
            instance = this;
            // DontDestroyOnLoad(gameObject);
        }
        else
        {
            // Destroy(gameObject);
        }
    }

    public void ChangementMusique(AudioClip musiqueChoisie)
    {
        AudioSource pisteEnCours = mPiste1;
        AudioSource pisteChoisie = mPiste2;

        if (!pisteEnCours.isPlaying)
        {
            pisteEnCours = mPiste2;
            pisteChoisie = mPiste1;
        }

        pisteChoisie.clip = musiqueChoisie;

        StopAllCoroutines();
        StartCoroutine(TransitionMusique(pisteChoisie, pisteEnCours));
    }

    IEnumerator TransitionMusique(AudioSource pisteChoisie, AudioSource pisteEnCours)
    {
        pisteChoisie.Play();

        if(pisteEnCours.isPlaying){
        while (pisteEnCours.volume > 0)
        {
            pisteEnCours.volume -= vitesseTransition;
            pisteChoisie.volume += vitesseTransition;
            // print("Le volume de la piste actuelle est à :" + pisteEnCours.volume + ". La nouvelle piste s'embarque avec un volume de :" + pisteChoisie.volume);
            yield return null;
        }

        pisteEnCours.Pause();
        }

        yield return null;
    }
    public void JouerSonBoutonUI(AudioClip sonUI)
    {
        vfxPisteUI.GetComponent<AudioSource>().PlayOneShot(sonUI);
    }

    public void JouerRetourSonSFX(AudioClip sonSlider){
        vfxPisteUI.GetComponent<AudioSource>().PlayOneShot(sonSlider);
    }
}
