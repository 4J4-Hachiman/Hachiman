/*  
 *  Fonctionnement et utilité générale du script
    
    Script pour le contrôle du menu des options (UI/UX)
        Par : Malaïka Abevi
        Dernière modification : 26/04/2025
*/
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using TMPro;
using UnityEngine.UI;

public class ControlesMenuOptions : MonoBehaviour
{
    //Variables pour les options par défaut
    public float volumeMusiqueDefaut = 0;
    public float volumeSFXDefaut = 0;
    public int indexResolutionDefaut = 1;

    //Variables pour les options actuellement manipulées par le joueur
    public float volumeMusique;
    public float volumeSFX;
    public int indexResolution;

    //Éléments à manipuler
    public AudioMixer audioMixerMusique;    //Audiomixer pour la musique
    public AudioMixer audioMixerSFX;    //Audiomixer pour la musique

    public TextMeshProUGUI avertissementNonSauvegarde;
    public TextMeshProUGUI message;
    public Image cadrePopUp;
    public Button bntSauv;
    public Button bntReinit;
    public TextMeshProUGUI textBntSauve;
    public TextMeshProUGUI textBntReinit;

    public Color32 couleurIndisponible;
    public Color32 couleurBase;
    //Liste pour enregister les différentes résolutions pour le jeu
    public List<CollectionResolutions> resolution = new List<CollectionResolutions>();

    //Gestion de l'apparence du UI des options
    [SerializeField] private Slider controleurVolMusique;
    [SerializeField] private Slider controleurVolSFX;
    [SerializeField] private TMP_Dropdown controleurResolution;

    private void Start()
    {
        ActualisationOptions();
        avertissementNonSauvegarde.enabled = false;
    }

    void Update()
    {
        if (!OptionsManager.optionsSauvegarder)
        {
            avertissementNonSauvegarde.enabled = true;
            bntSauv.interactable = true;
            textBntSauve.color = couleurBase;
        }
        else
        {
            avertissementNonSauvegarde.enabled = false;
            bntSauv.interactable = false; 
            textBntSauve.color = couleurIndisponible;
        }

        // On veut désactiver les boutons lorsque qu'il n'est pas utile de cliquer dessus
        // l'utilisateur comprendra que c'est paramètres sont déjà sur les valeurs par défauts
        if (volumeMusique == 0 && volumeSFX == 0 && indexResolution == 1)
        {
            bntReinit.interactable = false;
            textBntReinit.color = couleurIndisponible;
        }
        else
        {
            bntReinit.interactable = true;
            textBntReinit.color = couleurBase;
        }
    }

    //Fonction pour ajuster le volume de la musique à l'aide du slider dans le menu "Options"
    public void AjusterVolumeMusique(float volume)
    {
        audioMixerMusique.SetFloat("volume", volume);
        volumeMusique = volume;
        OptionsManager.optionsSauvegarder = false;
    }

    //Fonction pour ajuster le volume des effets sonores à l'aide du slider dans le menu "Options"
    public void AjusterVolumeSFX(float volume)
    {
        audioMixerSFX.SetFloat("volume", volume);
        volumeSFX = volume;
        OptionsManager.optionsSauvegarder = false;
    }

    //Fonction pour changer la résolution du jeu pour l'écran
    public void AjusterResolution(int index)
    {
        Screen.SetResolution(resolution[index].largeur, resolution[index].largeur, true);
        indexResolution = index;
        OptionsManager.optionsSauvegarder = false;
    }

    //Fonction pour sauvegarder les options
    public void SauvegarderOptions()
    {
        OptionsManager.volumeMusiqueSauve = volumeMusique;
        OptionsManager.volumeSFXSauve = volumeSFX;
        OptionsManager.indexResolutionSauve = indexResolution;
        // On indique que les options sont sauvegardées
        // print("Volume de musique sauvegardé : " + OptionsManager.volumeMusiqueSauve);
        // print("Volume de SFX sauvegardé : " + OptionsManager.volumeSFXSauve);
        // print("Résolution sauvegardée : " + OptionsManager.indexResolutionSauve);
        OptionsManager.optionsSauvegarder = true;
        avertissementNonSauvegarde.enabled = false;
        message.text = "Vos paramètres ont été sauvegardés";
        cadrePopUp.GetComponent<Animator>().SetTrigger("popUp");
    }

    //Fonction pour réinitialiser les options
    public void ReinitialiserOptions()
    {
        volumeMusique = volumeMusiqueDefaut;
        volumeSFX = volumeSFXDefaut;
        indexResolution = indexResolutionDefaut;
        // On veut mettre à jour les options avec les valeurs par défaut
        controleurVolMusique.value = volumeMusiqueDefaut;
        controleurVolSFX.value = volumeSFXDefaut;
        controleurResolution.value = indexResolutionDefaut;

        message.text = "Vos parametrès ont été réinitialisés";
        cadrePopUp.GetComponent<Animator>().SetTrigger("popUp");
    }

    // Fonction pour la mise é jour des options selon les options enregistrées par l'utilisateur
    public void ActualisationOptions()
    {
        audioMixerMusique.SetFloat("volume", OptionsManager.volumeMusiqueSauve);
        controleurVolMusique.value = OptionsManager.volumeMusiqueSauve;

        audioMixerSFX.SetFloat("volume", OptionsManager.volumeSFXSauve);
        controleurVolSFX.value = OptionsManager.volumeSFXSauve;

        Vector2Int res = new(1920, 1080);

        CollectionResolutions resolutions = new CollectionResolutions();

        Screen.SetResolution(resolution[OptionsManager.indexResolutionSauve].largeur, resolution[OptionsManager.indexResolutionSauve].largeur, true);
        // Screen.SetResolution(resolutions.resDfaut.x, res.y, true);
        controleurResolution.value = OptionsManager.indexResolutionSauve;
    }

    /**********************************************************************************************************************************************************************************************/
    //Classe pour enregistrer et référencer plus facilement des valeurs de largeur et de hauteur pour la résolution
    [System.Serializable]
    public class CollectionResolutions
    {
        public int largeur;
        public int hauteur;
    }
}


