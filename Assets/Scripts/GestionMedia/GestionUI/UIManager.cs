/*  
 *  Fonctionnement et utilit� g�n�rale du script

[Gestionnaire]    
###[SINGLETON] 

    Stockage des variables statiques et autres
        Par : Mala�ka Abevi
        Derni�re modification : 30/03/2025
*/
using UnityEngine;

public class UIManager : MonoBehaviour
{
    //D�claration de variables
    public static UIManager instance;

    //Conditions pour les options
    public static bool optionsSauvegarder;
    public static bool initialisationFait;

    //Variables pour les options sauvegard�es
    public static float volumeMusiqueSauve;
    public static float volumeSFXSauve;
    public static int indexResolutionSauve;

    //Menu options
    public GameObject menuOptions;

    void Start()
    {
        //Instancier le script / UIManager et �viter les doublons
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        //On affirme que les options ont �t� sauvegard�s
        optionsSauvegarder = true;

        // Initialisation des valeurs de d�part aux variables statiques (d�but de jeu)
        if (initialisationFait == false)
        {
            Debug.LogWarning("Les options ont �t� initialis�s");
            volumeMusiqueSauve = 0;
            volumeSFXSauve = 0;
            indexResolutionSauve = 1;
            //Rendre la condition fausse pour que les variables ne soient plus initialis�s du reste du jeu
            initialisationFait = true;
        }

        //Actualisation des options (pour valeurs et affichages)
        menuOptions.GetComponent<ControlesMenuOptions>().ActualisationOptions();
    }
}
