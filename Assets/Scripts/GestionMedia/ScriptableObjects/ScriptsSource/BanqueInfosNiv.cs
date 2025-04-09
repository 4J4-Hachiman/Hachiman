/*  
 *  Fonctionnement et utilit� g�n�rale du script

[Gestionnaire] 

    Stockage des informations sur les quêtes et niveaux
        Par : Mala�ka Abevi
        Derni�re modification : 06/04/2025
*/
using UnityEngine;

[CreateAssetMenu(fileName = "BanqueInfosNiv", menuName = "Custom Scriptable Object/Gestion des niveaux")]
public class BanqueInfosNiv : ScriptableObject
{
    [Header("Informations générales")]
    public string[] titreQuete;
    public string[] objectif;
    public int[] nbChapitre;
}



//public class TestScriptableObject : ScriptableObject
//{
//    public GameObject gameObject;
//    [field: SerializeField] public int VoulmeEffets { get; private set; }

//    // Vector2Int defaultRes = 
//    // Vector2Int resolutionLive = new Vector2Int(1920, 1080);

//    // private void UpdateREs(Vector2Int newRes)
//    // {
//    //     resolutionLive = newRes;
//    // }

//}