/*  
 *  Fonctionnement et utilité générale du script

[Gestionnaire]

    Fonction pour les changements de scènes
        Par : Malaïka Abevi
        Dernière modification : 06/05/2025
*/
using System.Collections;
using UnityEditor.SearchService;
using UnityEditor.Timeline;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneActiveManager : MonoBehaviour
{
    public static SceneActiveManager instance;
    public Image fillChargement;
    public Animator animChargement;
    public PlayableDirector timeline;

    UnityEngine.SceneManagement.Scene sceneActuelle;
    public static bool changementSceneEnCours;

    void Start()
    {
        sceneActuelle = SceneManager.GetActiveScene();
        changementSceneEnCours = false;
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    //Fonction pour démarrer la coroutine du chargement de scène
    public void ChargerScene(string nomScene)
    {
        StartCoroutine(ChargementAsyncScene(nomScene));

    }

    IEnumerator ChargementAsyncScene(string nomScene)
    {
        if (sceneActuelle.name == "_MenuIntro")
        {
            // Gamemanager.newgame
        }
        changementSceneEnCours = true;
        Time.timeScale = 1;
        timeline.gameObject.SetActive(true);
        yield return new WaitForSeconds(7);

        AsyncOperation scene = SceneManager.LoadSceneAsync(nomScene);

        scene.allowSceneActivation = false;

        do
        {
            fillChargement.fillAmount = scene.progress;
            Debug.Log("Chargement... : " + scene.progress);
            if (scene.progress >= 0.9f)
            {
                yield return new WaitForSeconds(1);
                fillChargement.fillAmount = 1f;
                yield return new WaitForSeconds(1);
                animChargement.SetTrigger("fadeOut");
                yield return new WaitForSeconds(3);
                scene.allowSceneActivation = true;
                Time.timeScale = 1;
            }
            yield return null;
        } while (!scene.isDone);

        Debug.Log("La scène est chargée");

        yield return null;
    }
}
