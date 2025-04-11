/*  
 *  Fonctionnement et utilit� g�n�rale du script

[Gestionnaire]

    Fonction pour les changements de sc�nes
        Par : Mala�ka Abevi
        Derni�re modification : 10/04/2025
*/
using System.Collections;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneActiveManager : MonoBehaviour
{
    public static SceneActiveManager instance;

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

    // public void ChangementScene(string nomScene)
    // {
    //     // SceneManager.LoadScene(nomScene);
    //     // Use a coroutine to load the Scene in the background
    //     // StartCoroutine(LoadYourAsyncScene(nomScene));
    // }

    public async void ChargerScene(string nomScene){
        var scene = SceneManager.LoadSceneAsync(nomScene);
        scene.allowSceneActivation = false;

        //Instructions nécéssaires blablabla
        if(!scene.isDone){
            
        }
        do{
            Debug.Log("Chargement...");
        }while(scene.progress < 0.9f);

        Debug.Log("La scène est chargée");
        
        await Task.Delay(15000);

        scene.allowSceneActivation = true;

    }
    // IEnumerator LoadYourAsyncScene(string nomScene)
    // {
    //     // The Application loads the Scene in the background as the current Scene runs.
    //     // This is particularly good for creating loading screens.
    //     // You could also load the Scene by using sceneBuildIndex. In this case Scene2 has
    //     // a sceneBuildIndex of 1 as shown in Build Settings.

    //     AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("Scene2");

    //     // Wait until the asynchronous scene fully loads
    //     while (!asyncLoad.isDone)
    //     {
    //         yield return null;
    //     }
    // }
}
