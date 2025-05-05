using System;
using UnityEngine;
using UnityEngine.Playables;

public class testTimelineTuto : MonoBehaviour
{
    public PlayableDirector director;
    public Sprite testImg;
    public string testText;
    public AffichageRecolteItems affichagePopUp;
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Y))
        {
            director.playableGraph.GetRootPlayable(0).SetSpeed(1);
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            affichagePopUp.AfficherItemsRecolte(testImg, testText);
        }
    }
}
