using UnityEngine;

public class testCinematiques : MonoBehaviour
{
    public ChangementCinematiques changeCine;
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            changeCine.DemarrerCinematique(changeCine.cinematique1);
        }

        if (Input.GetKeyDown(KeyCode.K))
        {
            changeCine.DemarrerCinematique(changeCine.cinematique2);
        }

        if (Input.GetKeyDown(KeyCode.Z))
        {
            changeCine.ArreterCinematique();
        }
    }
}
