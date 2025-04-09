using UnityEngine;

public class DetectionEnnemiUI : MonoBehaviour
{

    public GameObject hud;
    void OnTriggerEnter(Collider infoTrigger)
    {
        if (infoTrigger.tag == "Ennemy")
        {
            hud.GetComponent<NavigationBoussole>().IndiquerPositionEnnemi(infoTrigger.transform);
        }
    }

    public BanqueAudio banqueAudio;
    void Update()
    {
        hud.GetComponent<MusiqueTransition>().ChangerMusique(banqueAudio.mscCombat);
    }
}
