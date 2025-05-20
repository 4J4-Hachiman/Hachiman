using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class Chest : MonoBehaviour, IDataSaveable
{

    [field: SerializeField] private string id;
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
    public AffichageRecolteItems affichageRecolteItems;
    public Sprite spriteItem;
    public string stringItemName;

    /* -------------------- VARIABLES CHEST -------------------- */
    private bool isOpen = false;
    private bool isTaken = false;
    private bool isIdle = false;
    private bool canTake = false;
    private bool isPlayerInside = false;

    /* ------------------ REFERENCES INPUT SYSTEM ------------------ */
    private PlayerControls inputActions;
    private InputAction inputMouvement;
    private InputAction SprintInput;

    /* -------------------- VARIABLES INT -------------------- */
    public int numbPotionInChest;

    void Awake()
    {
        id = $"chest_{SceneManager.GetActiveScene().name}_{transform.parent.parent.name.ToLower()}";

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
            //Debug.Log("isPlayerInside");
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
            //Debug.Log("Interact action");
            InteractChest();
        }
    }

    void InteractChest()
    {
        if (!isOpen)
        {
            //Debug.Log("open");
            isOpen = true;
            animator.SetTrigger("Open");
            GetComponents<AudioSource>()[0].PlayOneShot(banqueAudio.sOuvertureCoffre);
            Invoke("CanTake", 2.1f);
        }
        else if (!isTaken && canTake)
        {
            //Debug.Log("take");
            if (itemInChest.tag == "Potion")
            {
                isTaken = true;
                //Debug.Log("potion");
                hachiman.numbPotion += numbPotionInChest;
                affichageRecolteItems.AfficherItemsRecolte(spriteItem, stringItemName);
            }
            else if (katana != null || katanaInChest != null)
            {
                if (itemInChest.tag == "Katana" && hachiman.isArmed)
                {
                    isTaken = true;
                    GameObject instNewKatana = Instantiate(newKatana, katana.transform.position, katana.transform.rotation);
                    instNewKatana.gameObject.SetActive(true);
                    instNewKatana.transform.SetParent(hand.transform);
                    hachiman.activeKatana.gameObject.SetActive(false);
                    hachiman.activeKatana = instNewKatana;
                    hachiman.katanaList.Add(instNewKatana);
                    affichageRecolteItems.AfficherItemsRecolte(spriteItem, stringItemName);
                    hachiman.ShowKatanaIcon();
                }
            }
            if (isTaken)
            {
                pointLightInChest.gameObject.SetActive(false);
                itemInChest.SetActive(false);
                uiInteraction.SetActive(false);
                GetComponents<AudioSource>()[1].PlayOneShot(banqueAudio.sObtainItem);
            }
        }
    }

    void OnCollisionStay(Collision collision)
    {
        if ((collision.gameObject.name == "Hachiman") && (hachiman.state == JoueursControl1.HachimanState.Idle))
        {
            isIdle = true;
        }
    }

    public void LoadData(GameData data)
    {
        isOpen = data.chestsStatesOpen.GetKey(id, isOpen);
        isTaken = data.chestsStatesItemPicked.GetKey(id, isTaken);
        if (isOpen)
        {
            animator.SetTrigger("Open");
            uiInteraction.SetActive(false);
        }
        if (isTaken && itemInChest.CompareTag("Katana"))
        {
            // GameObject katana = Instantiate(newKatana);
            // if (!hachiman.katanaList.Contains(katana))
            // {
            //     hachiman.katanaList.Add(katana);
            //     katana.transform.parent = hand.transform;
            // }
            pointLightInChest.SetActive(false);
            itemInChest.SetActive(false);
        }
    }

    public void SaveData(ref GameData data)
    {
        data.chestsStatesOpen.SetPair(id, isOpen);
        data.chestsStatesItemPicked.SetPair(id, isTaken);
    }

    void CanTake()
    {
        canTake = true;
    }
}