using UnityEngine;

public class DetectionEnnemiUI : MonoBehaviour
{

    // public GameObject hud;
    public NavigationBoussole navigationBoussole;
    // public BanqueAudio banqueAudio;
    GameObject indicateurEnnemi;

    void OnEnable()
    {
        indicateurEnnemi = Instantiate(navigationBoussole.indicateurEnnemi, navigationBoussole.indicateurEnnemi.transform.position, navigationBoussole.indicateurEnnemi.transform.rotation, navigationBoussole.indicateurEnnemi.transform.parent);
        indicateurEnnemi.SetActive(true);
    }
    void Update()
    {
        navigationBoussole.IndiquerPosition(gameObject.transform, indicateurEnnemi);
    }
}
