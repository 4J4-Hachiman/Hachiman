/*  
 *  Fonctionnement et utilit� g�n�rale du script
    
    Script pour le contr�le du menu des options (UI/UX)
        Par : Mala�ka Abevi
        Derni�re modification : 30/03/2025
*/
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using TMPro;
using UnityEngine.UI;

public class ControlesMenuOptions : MonoBehaviour
{
    //Variables pour les options par d�faut
    public float volumeMusiqueDefaut = 0;
    public float volumeSFXDefaut = 0;
    public int indexResolutionDefaut = 1;

    //Variables pour les options actuellement manipul�es par le joueur
    public float volumeMusique;
    public float volumeSFX;
    public int indexResolution;

    //�l�ments � manipuler
    public AudioMixer audioMixerMusique;    //Audiomixer pour la musique
    public AudioMixer audioMixerSFX;    //Audiomixer pour la musique

    public TextMeshProUGUI avertissementNonSauvegarde;
    public TextMeshProUGUI messageSauvegarde;
    public TextMeshProUGUI messageReinitalisation;
    public Image cadrePopUp;

    //Liste pour enregister les diff�rentes r�solutions pour le jeu
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
        if (!UIManager.optionsSauvegarder)
        {
            avertissementNonSauvegarde.enabled = true;
        }
        else{
            avertissementNonSauvegarde.enabled = false;
        }
    }

    //Fonction pour ajuster le volume de la musique � l'aide du slider dans le menu "Options"
    public void AjusterVolumeMusique(float volume)
    {
        audioMixerMusique.SetFloat("volume", volume);
        volumeMusique = volume;
        UIManager.optionsSauvegarder = false;
    }

    //Fonction pour ajuster le volume des effets sonores � l'aide du slider dans le menu "Options"
    public void AjusterVolumeSFX(float volume)
    {
        audioMixerSFX.SetFloat("volume", volume);
        volumeSFX = volume;
        UIManager.optionsSauvegarder = false;
    }

    //Fonction pour changer la r�solution du jeu pour l'�cran
    public void AjusterResolution(int index)
    {
        Screen.SetResolution(resolution[index].largeur, resolution[index].largeur, true);
        indexResolution = index;
        UIManager.optionsSauvegarder = false;
    }

    //Fonction pour sauvegarder les options
    public void SauvegarderOptions()
    {
        UIManager.volumeMusiqueSauve = volumeMusique;
        UIManager.volumeSFXSauve = volumeSFX;
        UIManager.indexResolutionSauve = indexResolution;
        // On indique que les options sont sauvegard�es
        print("Volume de musique sauvegard� : " + UIManager.volumeMusiqueSauve);
        print("Volume de SFX sauvegard� : " + UIManager.volumeSFXSauve);
        print("R�solution sauvegard�e : " + UIManager.indexResolutionSauve);
        UIManager.optionsSauvegarder = true;
        avertissementNonSauvegarde.enabled = false;
    }

    //Fonction pour r�initialiser les options
    public void ReinitialiserOptions()
    {
        volumeMusique = volumeMusiqueDefaut;
        volumeSFX = volumeSFXDefaut;
        indexResolution = indexResolutionDefaut;
        // On veut mettre � jour les options avec les valeurs par d�faut
        controleurVolMusique.value = volumeMusiqueDefaut;
        controleurVolSFX.value = volumeSFXDefaut;
        controleurResolution.value = indexResolutionDefaut;
    }

    // Fonction pour la mise � jour des options selon les options enregistr�es par l'utilisateur
    public void ActualisationOptions()
    {
        audioMixerMusique.SetFloat("volume", UIManager.volumeMusiqueSauve);
        controleurVolMusique.value = UIManager.volumeMusiqueSauve;

        audioMixerSFX.SetFloat("volume", UIManager.volumeSFXSauve);
        controleurVolSFX.value = UIManager.volumeSFXSauve;

        Vector2Int res = new(1920, 1080);

        CollectionResolutions resolutions = new CollectionResolutions();

        Screen.SetResolution(resolution[UIManager.indexResolutionSauve].largeur, resolution[UIManager.indexResolutionSauve].largeur, true);
        // Screen.SetResolution(resolutions.resDfaut.x, res.y, true);
        controleurResolution.value = UIManager.indexResolutionSauve;
    }

    /**********************************************************************************************************************************************************************************************/
    //Classe pour enregistrer et r�f�rencer plus facilement des valeurs de largeur et de hauteur pour la r�solution
    [System.Serializable]
    public class CollectionResolutions
    {
        public int largeur;
        public int hauteur;
        // public Vector2Int resDfaut = new Vector2Int(1920, 1080);
        // public Vector2Int resLive = new Vector2Int(1280, 720);
    }
}


