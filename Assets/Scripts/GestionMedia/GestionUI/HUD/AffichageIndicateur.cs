using UnityEngine;

public class AffichageIndicateur : MonoBehaviour
{
    public GameObject ObjetSuivi;
    void Start()
    {
        gameObject.GetComponent<Animator>().SetTrigger("apparition");
    }

    void Update()
    {
        if (ObjetSuivi != null)
        {
            if (!ObjetSuivi.activeSelf || ObjetSuivi == null)
            {
                gameObject.GetComponent<Animator>().SetTrigger("disparition");
                Invoke("DestructionIndicateur", 3f);
                print("Détruit");
            }
        }
    }

    void DestructionIndicateur()
    {
        Destroy(gameObject);
    }
}
// StartCoroutine(ApparitionIndicateur());

// IEnumerator ApparitionIndicateur()
// {
//     // float alpha = 0;
//     // // indicateur.GetComponent<Image>().color = new Color(255f, 255f, 255f, alpha);

//     // while (alpha < 1)
//     // {
//     //     alpha += 0.01f;
//     //     gameObject.GetComponent<Image>().color = new Color(1, 1, 1, alpha);
//     //     print(alpha);
//     //     yield return null;
//     // }

//     yield return null;
// }

// Update is called once per frame
