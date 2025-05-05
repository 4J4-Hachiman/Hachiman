/*  
 *  Fonctionnement et utilité générale du script
    
    Script pour le fonctionnement de la boussole et des indicateurs d'ennemis, objectifs et objets
        Par : Malaïka Abevi
        Dernière modification : 04/05/2025
*/
using UnityEngine;

public class NavigationBoussole : MonoBehaviour
{
    public GameObject joueur;
    public GameObject cam;
    public Transform refOrientation;
    // public Vector3 positionEnnemi;
    public GameObject indicateurEnnemi;
    public GameObject indicateurQuete;
    public GameObject boussole;
    public Vector3 directionMonde;

    private void Update()
    {
        //######################Gestion des indicateurs d'ennemis
        refOrientation.localEulerAngles = new Vector3(0, cam.transform.localEulerAngles.y, 0);
        refOrientation.position = joueur.transform.position;

        //######################Gestion de la boussole
        //On applique l'angle en Z du monde 3D à la boussole en 2d, donc Y pour la correspondance visuelle
        directionMonde.z = cam.transform.eulerAngles.y;
        boussole.transform.localEulerAngles = directionMonde;
        //Debug.LogError(cam.transform.localEulerAngles);

        refOrientation.localEulerAngles = new Vector3(0, cam.transform.localEulerAngles.y, 0);
        refOrientation.position = joueur.transform.position;
    }

    public void IndiquerPosition(Transform gameObjectSuivi, GameObject indicateur)
    {
        if (gameObjectSuivi.gameObject.activeSelf)
        {
            //######################Gestion des indicateurs d'ennemis
            Vector3 positionGameObject = new Vector3(gameObjectSuivi.position.x, gameObjectSuivi.position.y, gameObjectSuivi.position.z);
            //On détermine la direction du joueur par rapport à l'ennemi
            positionGameObject.y = joueur.transform.position.y;
            Vector3 direction = (positionGameObject - joueur.transform.position).normalized;
            //Puis on récupére l'angle entre la direction (axe z) du joueur et l'ennemi
            // direction.y = 0;
            float angle = Vector3.SignedAngle(direction, refOrientation.transform.forward, Vector3.up);

            //Puis on fait tourner le gradient dans la boussole
            indicateur.transform.localEulerAngles = new Vector3(0, 0, angle);
        }
        else
        {
            Destroy(indicateur, 2f);
        }
    }
}



// public void IndiquerPositionEnnemi(Transform transformEnnemi)
// {
//     Vector3 positionEnnemi = new Vector3(transformEnnemi.position.x, transformEnnemi.position.y, transformEnnemi.position.z);
//     //######################Gestion des indicateurs d'ennemis
//     // refOrientation.localEulerAngles = new Vector3(0, cam.transform.localEulerAngles.y, 0);
//     // refOrientation.position = joueur.transform.position;

//     //On détermine la direction du joueur par rapport à l'ennemi
//     positionEnnemi.y = joueur.transform.position.y;
//     Vector3 direction = (positionEnnemi - joueur.transform.position).normalized;
//     //Puis on récupére l'angle entre la direction (axe z) du joueur et l'ennemi
//     //direction.y = 0;
//     float angle = (Vector3.SignedAngle(direction, refOrientation.transform.forward, Vector3.up));

//     //Puis on fait tourner le gradient dans la boussole
//     indicateurEnnemi.transform.localEulerAngles = new Vector3(0, 0, angle);
// }


// //On détermine la direction du joueur par rapport à l'ennemi
// positionEnnemi.y = joueur.transform.position.y;
// Vector3 direction = (positionEnnemi - (joueur.transform.position)).normalized;
// //Puis on récupére l'angle entre la direction (axe z) du joueur et l'ennemi
// //direction.y = 0;
// float angle = Vector3.SignedAngle(direction, refOrientation.transform.forward, Vector3.up);

// //Puis on fait tourner le gradient dans la boussole
// indicateurEnnemi.transform.localEulerAngles = new Vector3(0, 0, angle);