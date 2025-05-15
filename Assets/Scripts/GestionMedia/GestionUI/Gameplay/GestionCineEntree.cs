using System.Collections;
using UnityEngine;

public class GestionCineEntree : MonoBehaviour
{
    public ChangementCinematiques changementCinematiques;
    public GameObject tutoriel;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine("GererCineIntro");
    }

    IEnumerator GererCineIntro(){
        changementCinematiques.DemarrerCinematique(changementCinematiques.cinematiqueIntroduction);
        while(!ChangementCinematiques.cinematiqueTermine){
            yield return null;
        }

        yield return new WaitForSeconds(3);

        gameObject.GetComponent<Animator>().SetTrigger("fadeOut");

        yield return new WaitForSeconds(5);

        tutoriel.SetActive(true);
        yield return null;
    }
}
