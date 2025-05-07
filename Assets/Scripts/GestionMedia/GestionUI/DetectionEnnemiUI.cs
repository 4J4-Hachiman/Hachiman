using UnityEngine;

public class DetectionEnnemiUI : MonoBehaviour
{
    public NavigationBoussole navigationBoussole;
    // public BanqueAudio banqueAudio;
    public GameObject indicateur;
    GameObject indicateurClone;

    void OnEnable()
    {
        indicateurClone = Instantiate(indicateur, indicateur.transform.position, indicateur.transform.rotation, indicateur.transform.parent);
        indicateurClone.SetActive(true);
    }
    void Update()
    {
        navigationBoussole.IndiquerPosition(gameObject.transform, indicateurClone);
    }
}
