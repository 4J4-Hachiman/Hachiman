using UnityEngine;
using UnityEngine.InputSystem;

public class Chest : MonoBehaviour
{
    /* ============================================================== */
    /* ============================================================== */
    /* -------------------- GAMEOBJECT  -------------------- */
    public GameObject itemInChest;
    public GameObject newKatana;
    public GameObject katana;
    public GameObject katanaInChest;
    public GameObject pointLightInChest;
    public GameObject hand;
    public GameObject chestParent;
    public GameObject uiInteraction;
    
    /* -------------------- REFERENCES COMPONENTS -------------------- */
    public JoueursControl1 hachiman;
    public Animator animator;
    public BanqueAudio banqueAudio;
    public AudioSource audioSource;

    /* -------------------- VARIABLES CHEST -------------------- */
    private bool isOpen = false;
    private bool isTaken = false;
    private bool isIdle = false;
    private bool isPlayerInside = false;

    /* ------------------ REFERENCES INPUT SYSTEM ------------------ */
    private PlayerControls inputActions;
    private InputAction inputMouvement;
    private InputAction SprintInput;

    /* -------------------- VARIABLES INT -------------------- */
    public int numbPotionInChest;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        animator = chestParent.GetComponent<Animator>();
        inputActions = new PlayerControls();
        inputActions.Enable();
        inputActions.MapNormale.Interact.performed += Interact;
    }
    void OnEnable()
    {
        inputActions.MapNormale.Interact.performed += Interact;
    }

    void OnDisable()
    {
        inputActions.MapNormale.Interact.performed -= Interact; // Unsubscribe when the object is disabled
    }

    void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.name == "Hachiman")
        {
            Debug.Log("isPlayerInside");
            isPlayerInside = true; // Player has entered the chest's trigger area
        }
    }

    void OnTriggerExit(Collider collision)
    {
        if (collision.gameObject.name == "Hachiman")
        {
            isPlayerInside = false; // Player has exited the chest's trigger area
        }
    }

    private void Interact(InputAction.CallbackContext context)
    {
        if (isPlayerInside)
        {
            Debug.Log("Interact action");
            InteractChest();
        }
    }

    void InteractChest()
    {
        if(!isOpen)
        {
            Debug.Log("open");
            isOpen = true;
            animator.SetTrigger("Open"); 
            uiInteraction.SetActive(false);
            audioSource.PlayOneShot(banqueAudio.sOuvertureCoffre);
        }
        else if(!isTaken) 
        {
            Debug.Log("take");
            isTaken = true;
            if(itemInChest.tag == "Potion")
            {
                Debug.Log("potion");
                hachiman.numbPotion += numbPotionInChest;
            }
            else if(katana != null || katanaInChest != null)
            {
                if(itemInChest.tag == "Katana")
                {
                    GameObject instNewKatana = Instantiate(newKatana, katana.transform.position, katana.transform.rotation);
                    instNewKatana.gameObject.SetActive(true);
                    instNewKatana.transform.SetParent(hand.transform);
                    hachiman.activeKatana.gameObject.SetActive(false);
                    hachiman.activeKatana = instNewKatana;
                }
            }
            
            pointLightInChest.gameObject.SetActive(false);
            itemInChest.SetActive(false);
        }
    }

    void OnCollisionStay(Collision collision)
    {
        if ((collision.gameObject.name == "Hachiman")&&(hachiman.state == JoueursControl1.HachimanState.Idle))
        {
            isIdle = true;
        }
    }
}
