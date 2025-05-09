using UnityEngine;

public class Ring : MonoBehaviour
{
    private Animator animator;
    void Awake()
    {
        animator = GetComponent<Animator>();
        animator.SetTrigger("drop");
    }
    void Update()
    {
        
    }
}
