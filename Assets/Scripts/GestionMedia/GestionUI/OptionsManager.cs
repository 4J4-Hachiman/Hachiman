/*  
 *  Fonctionnement et utilité générale du script

[Gestionnaire]    
###[SINGLETON] 

    Stockage des variables statiques et autres
        Par : Malaïka Abevi
        Dernière modification : 30/03/2025
*/
using UnityEngine;

public class OptionsManager : MonoBehaviour
{
    //Déclaration de variables
    public static OptionsManager instance;

    //Conditions pour les options
    public static bool optionsSauvegarder;
    public static bool initialisationFait;

    //Variables pour les options sauvegardées
    public static float volumeMusiqueSauve;
    public static float volumeSFXSauve;
    public static int indexResolutionSauve;

    //Menu options
    public GameObject menuOptions;

    void Start()
    {
        //Instancier le script / UIManager et éviter les doublons
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        //On affirme que les options ont été sauvegardés
        optionsSauvegarder = true;

        // Initialisation des valeurs de départ aux variables statiques (début de jeu)
        if (initialisationFait == false)
        {
            Debug.LogWarning("Les options ont été initialisés");
            volumeMusiqueSauve = 0;
            volumeSFXSauve = 0;
            indexResolutionSauve = 1;
            //Rendre la condition fausse pour que les variables ne soient plus initialisés du reste du jeu
            initialisationFait = true;
        }

        //Actualisation des options (pour valeurs et affichages)
        menuOptions.GetComponent<ControlesMenuOptions>().ActualisationOptions();
    }
}
