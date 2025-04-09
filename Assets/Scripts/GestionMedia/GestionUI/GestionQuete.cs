using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GestionQuete : MonoBehaviour
{
    public BanqueInfosNiv banqueInfosNiv;
    public static int indexNiveau;
    public static bool niveauComplet;
    public TextMeshProUGUI nbChapitreTxt;
    public TextMeshProUGUI titreQueteTxt;
    public TextMeshProUGUI objectifTxt;
    public Toggle stadeObjectifToggle;
    public GameObject canvasNivComplet;
    public GameObject canvasMort;

    void Start()
    {
        indexNiveau = 0;
        AfficherQuete();
    }

    public void AfficherQuete(/*int nbChapitre, string titreQuete, string objectif, bool completionObjectif*/)
    {
        nbChapitreTxt.text = "Chapitre " + banqueInfosNiv.nbChapitre[indexNiveau];
        titreQueteTxt.text = banqueInfosNiv.titreQuete[indexNiveau];
        objectifTxt.text = banqueInfosNiv.objectif[indexNiveau];
    }

    public void CompleterQuete()
    {
        stadeObjectifToggle.isOn = true;
    }

    public void AffichageNiveauComplet()
    {
        canvasNivComplet.SetActive(true);
        canvasNivComplet.GetComponent<Animator>().SetBool("transitionNiv", true);
        niveauComplet = true;
        stadeObjectifToggle.isOn = false;
        indexNiveau++;
        AfficherQuete();
    }

    public void AffichageMort(){
        canvasMort.SetActive(true);
    }
}
