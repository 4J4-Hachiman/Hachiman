using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AffichageQuete : MonoBehaviour
{
    // public BanqueInfosNiv banqueInfosNiv;
    public static int indexNiveau;
    public static bool niveauComplet;
    public TextMeshProUGUI nbChapitreTxt;
    public TextMeshProUGUI titreQueteTxt;
    public TextMeshProUGUI objectifTxt;
    public GameObject canvasNivComplet;
    public GameObject canvasMort;

    void Start()
    {
        // AfficherQuete();
    }

    public void AfficherQuete()
    {
        // titreQueteTxt.text = banqueInfosNiv.titreQuete[indexNiveau];
        // objectifTxt.text = banqueInfosNiv.objectif[indexNiveau];
    }

    public void AffichageNiveauComplet()
    {
        canvasNivComplet.SetActive(true);
        canvasNivComplet.GetComponent<Animator>().SetBool("transitionNiv", true);
        AfficherQuete();
    }

    public void AffichageMort(){
        canvasMort.SetActive(true);
    }
}
