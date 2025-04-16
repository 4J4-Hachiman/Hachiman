/*  
 *  Fonctionnement et utilit� g�n�rale du script

[Gestionnaire]

    Fonction pour les changements de sc�nes
        Par : Mala�ka Abevi
        Derni�re modification : 13/04/2025
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
}
