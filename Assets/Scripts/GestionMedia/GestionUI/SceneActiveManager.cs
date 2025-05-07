/*  
 *  Fonctionnement et utilité générale du script

[Gestionnaire]

    Fonction pour les changements de scènes
        Par : Malaïka Abevi
        Dernière modification : 06/05/2025
*/
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneActiveManager : MonoBehaviour
{
    public static SceneActiveManager instance;
    public Image fillChargement;
    public Animator animChargement;

    void Start()
    {
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

// await Task.Delay(10000);
// var scene = SceneManager.LoadSceneAsync(nomScene);
// scene.allowSceneActivation = false;

// //Instructions nécéssaires blablabla
// if (!scene.isDone)
// {
//     // fillChargement.fillAmount = scene.progress;
// }
// do
// {
//     // fillChargement.fillAmount = scene.progress;
//     // Debug.Log("Chargement...");
// } while (scene.progress < 0.9f);

// Debug.Log("La scène est chargée");

// await Task.Delay(3000);

// scene.allowSceneActivation = true;
// Time.timeScale = 1;
