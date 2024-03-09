using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CompromiseMono : MonoBehaviour
{
    public CompromiseID compID = CompromiseID.NONE;

    [SerializeField]
    private Sprite crossed;

    [SerializeField]
    private Sprite uncrossed;

    [SerializeField]
    private Image background;

    [SerializeField]
    private Image cross_gameObject;

    [SerializeField]
    private TextMeshProUGUI textMeshProUGUI;

    public bool active = true;

    void Start()
    {
        
    }
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.V))
        {
            SetActive(true);
        }
        if (Input.GetKeyDown(KeyCode.U))
        {
            SetActive(false);
        }
        SetActive(active);
    }

    public void SetActive(bool acti)
    {
        // Background Transparency
        background.color = acti ? new Color(1, 1, 1, 0.5f) : new Color(1, 1, 1, 0.25f);

        // Text Transparency
        textMeshProUGUI.color = acti ? new Color(1, 1, 1, 1) : new Color(1, 1, 1, 0.5f);

        // Cross Image
        cross_gameObject.sprite = acti ? crossed : uncrossed;

        active = acti;
    }

    public void SetCompromiseID(CompromiseID id)
    {
        this.compID = id;
        textMeshProUGUI.text = GetCompromiseText(id);
    }

    private string GetCompromiseText(CompromiseID id)
    {
        switch (id)
        {
            case CompromiseID.acknowledge_dirty_kitchen:
                return "Joshua was a messy guy";
            case CompromiseID.acknowledge_joshuas_death:
                return "Joshua is dead";
            case CompromiseID.acknowledge_wrong_clock:
                return "The cock.";
            default:
                return "NO STRING FOUND";
        }
    }
}
