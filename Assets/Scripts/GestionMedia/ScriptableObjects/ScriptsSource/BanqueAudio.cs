/*  
 *  Fonctionnement et utilit� g�n�rale du script

[Gestionnaire] 

    Stockage de clips audio et fonctions pour jouer du son
        Par : Mala�ka Abevi
        Derni�re modification : 26/03/2025
*/
using UnityEngine;

[CreateAssetMenu(fileName = "BanqueAudio", menuName = "Custom Scriptable Object/Banque Audio")]
public class BanqueAudio : ScriptableObject
{
    [Header("Musique")]
    public AudioClip mscMenuPrincipal;
    public AudioClip mscMenuPause;
    public AudioClip mscJoueurRepere;
    public AudioClip mscCombat;
    public AudioClip mscCombatUhgomi;
    public AudioClip mscCombatKarna;
    public AudioClip mscCombatMasarai;
    public AudioClip mscMort;
    public AudioClip mscGameplay;


    [Header("Effets sonores UI")]
    public AudioClip bntUI;
    public AudioClip retourOptionsSfx;

    [Header("Effets sonores jeu")]
    public AudioClip sOuvertureCoffre;
    public AudioClip sGainVie;
    public AudioClip sGainMana;

    [Header("Effets sonores Hachiman")]
    public AudioClip sMarcheHachiman;
    //public AudioClip courseHachiman;
    public AudioClip ssGruntHachiman;
    public AudioClip sAtkLegereHachiman;
    public AudioClip sAtkLourdeHachiman;
    public AudioClip sStanceBroken;

    [Header("Effets sonores Combat")]
    public AudioClip sSwordClash1;
    public AudioClip sSwordClash2;
    public AudioClip sEnemyHit1;
    public AudioClip sEnemyHit2;
    public AudioClip sEnemyHit3;
    public AudioClip sEnemyHit4;
    public AudioClip sEnemyHit5;
    public AudioClip sEnemyHit6;
    public AudioClip sEnemyHit7;
    public AudioClip sEnemyHit8;
    public AudioClip sEnemyHit9;
    public AudioClip sEnemyHit10;
    public AudioClip sEnemyHit11;

    [Header("Effets sonores Katana")]
    public AudioClip sSwordAirSwing1;
    public AudioClip sSwordAirSwing2;
    public AudioClip sSwordAirSwing3;
    public AudioClip sSwordAirSwing4;

    [Header("Effets sonores ennemis")]
    public AudioClip sGruntEnnemis;
    public AudioClip sAtkLegereEnnemis;
    public AudioClip sAtkLourdeEnnemis;
    public AudioClip sEnemyDeath;

    [Header("Effets sonores Uhgomi")]
    public AudioClip sGruntUhgomi;
    public AudioClip sAtkLegereUhgomi;
    public AudioClip sAtkLourdeUhgomi;

    [Header("Effets sonores Karna")]
    public AudioClip sGruntKarna;
    public AudioClip sAtkLegereKarna;
    public AudioClip sAtkLourdeKarna;

    [Header("Effets sonores Masarai")]
    public AudioClip sGruntMasarai;
    public AudioClip sAtkLegereMasarai;
    public AudioClip sAtkLourdeMasarai;
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