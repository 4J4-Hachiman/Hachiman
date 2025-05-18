// using System.Collections;
// using UnityEngine;

// public class GestionCineEntree : MonoBehaviour
// {
//     public ChangementCinematiques changementCinematiques;
//     public GameObject tutoriel;
//     public bool introFait;

//     public static GestionCineEntree Instance { get; private set; }

//     private void Awake()
//     {
//         if (Instance != null && Instance != this)
//         {
//             Destroy(gameObject); // Destroy duplicates
//             // return;
//         }

//         Instance = this;
//         DontDestroyOnLoad(gameObject); // Make persistent across scenes

//         if (!introFait)
//         {
//             StartCoroutine("GererCineIntro");
//         }
//     }
//     // void Start()
//     // {
//     //     StartCoroutine("GererCineIntro");
//     // }

//     IEnumerator GererCineIntro()
//     {
//         changementCinematiques.DemarrerCinematique(changementCinematiques.cinematiqueIntroduction);
//         while (!ChangementCinematiques.cinematiqueTermine)
//         {
//             yield return null;
//         }

//         yield return new WaitForSeconds(3);

//         gameObject.GetComponent<Animator>().SetTrigger("fadeOut");

//         yield return new WaitForSeconds(5);

//         tutoriel.SetActive(true);
//         yield return null;
//     }
// }
