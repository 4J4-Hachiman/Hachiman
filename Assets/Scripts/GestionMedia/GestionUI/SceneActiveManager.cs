/*  
 *  Fonctionnement et utilité générale du script

[Gestionnaire]

    Fonction pour les changements de scènes
        Par : Malaïka Abevi
        Dernière modification : 04/05/2025
*/
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneActiveManager : MonoBehaviour
{
    public static SceneActiveManager instance;
    public Image fillChargement;

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
        await Task.Delay(5000);
        var scene = SceneManager.LoadSceneAsync(nomScene);
        scene.allowSceneActivation = false;

        //Instructions nécéssaires blablabla
        if(!scene.isDone){
            
        }
        do{
            fillChargement.fillAmount = scene.progress;
            Debug.Log("Chargement...");
        }while(scene.progress < 0.9f);

        Debug.Log("La scène est chargée");
        
        await Task.Delay(3000);

        scene.allowSceneActivation = true;
        Time.timeScale = 1;
    }
}
