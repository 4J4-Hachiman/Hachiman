/*  
 *  Fonctionnement et utilité générale du script

[Gestionnaire]

    Fonction pour les changements de scènes
        Par : Malaïka Abevi
        Dernière modification : 29/03/2025
*/
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneActiveManager : MonoBehaviour
{
    public void ChangementScene(string nomScene)
    {
        SceneManager.LoadScene(nomScene);
    }
}
