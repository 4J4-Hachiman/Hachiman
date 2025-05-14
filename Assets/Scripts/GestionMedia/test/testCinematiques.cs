using UnityEngine;

public class testCinematiques : MonoBehaviour
{
    public ChangementCinematiques changeCine;
    public Sprite imgTest;
    public string texteTest;

    public AffichageRecolteItems affichageItemsPop;
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            changeCine.DemarrerCinematique(changeCine.cinematiqueIntroduction);
        }

        if (Input.GetKeyDown(KeyCode.K))
        {
            changeCine.DemarrerCinematique(changeCine.cinematiquePortail);
        }

        if (Input.GetKeyDown(KeyCode.Z))
        {
            // changeCine.ArreterCinematique();
            // affichageItemsPop.AfficherItemsRecolte(imgTest, texteTest);
            // print("C'est sensé marcher");
        }
    }
}
