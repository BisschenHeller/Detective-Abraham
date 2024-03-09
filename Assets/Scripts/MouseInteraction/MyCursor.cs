using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class MyCursor : MonoBehaviour
{
    public Sprite Cursor_Standard = null;

    public Sprite Cursor_Speak = null;

    public Sprite Cursor_LookAt = null;

    public Sprite Cursor_Aberration = null;

    public GameObject TEST_CUBE = null;

    private Vector2 old_mouse_pos;

    [SerializeField]
    private float magic;

    [SerializeField]
    private Camera perfect_cam = null;

    [SerializeField]
    private Camera real_cam = null;

    [SerializeField]
    private SpriteRenderer cursor_perf = null;

    [SerializeField]
    private SpriteRenderer cursor_imp = null;

    [SerializeField]
    private SpriteRenderer cursor_perf2 = null;

    [SerializeField]
    private SpriteRenderer cursor_imp2 = null;

    [SerializeField]
    private TextMeshProUGUI subtitle = null;

    private void Start()
    {
        old_mouse_pos = new Vector2(0, 0);
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 mouse_pos = Input.mousePosition;
        Cursor.visible = mouse_pos.x > 1920; // So that no one gets cancer
        Vector2 movement = (mouse_pos - old_mouse_pos) * magic;
        old_mouse_pos = mouse_pos;
        transform.Translate(movement.x, movement.y, 0);

        transform.localPosition = new Vector3(Mathf.Clamp(transform.localPosition.x, -9, 9), Mathf.Clamp(transform.localPosition.y,-5,5), transform.localPosition.z);

        if (Input.GetKeyDown(KeyCode.Keypad1))
        {
            SetCursor(InteractionType.Speak);
        }
        if (Input.GetKeyDown(KeyCode.Keypad2))
        {
            SetCursor(InteractionType.Aberration);
        }
        if (Input.GetKeyDown(KeyCode.Keypad3))
        {
            SetCursor(InteractionType.LookAt);
        }
        if (Input.GetKeyDown(KeyCode.Keypad0))
        {
            SetCursor(InteractionType.None);
        }

        Vector3 raycast_origin = (Input.GetAxis("Aberrations_Linger") < 0.5 ? real_cam.transform.position : perfect_cam.transform.position)
            + transform.localPosition * (50.0f / 675.0f);
        raycast_origin.z = -0.5f;

        TEST_CUBE.transform.position = raycast_origin;
        Debug.DrawRay(raycast_origin, new Vector3(0, 0, 0.5f), Color.red,0.01f, true);
        
        
        if (Physics.Raycast(raycast_origin, new Vector3(0,0,1), out RaycastHit hitInfo, 10000.0f) && hitInfo.collider.gameObject.layer == 3)
        {
            //Debug.Log("Raycast_Origin: " + raycast_origin + "    Hit: " + hitInfo.collider.gameObject.name);
            Interactable interactable = hitInfo.collider.gameObject.GetComponent<Interactable>();
            if (interactable)
            {
                if (interactable.GetInteractionType() == InteractionType.Speak)
                {
                    TEST_CUBE.transform.localPosition += new Vector3(0.05f, 0.05f, 0);
                }
                SetCursor(interactable.GetInteractionType());
                subtitle.text = interactable.GetSubtitlePrompt();// + " " + interactable.gameObject.name;
            }
            
        } else
        {
            //Debug.Log("Raycast_Origin: " + raycast_origin + "    No Hit");
            subtitle.text = "";
            SetCursor(InteractionType.None);
        }
    }

    private void SetCursor(InteractionType type)
    {
        switch (type)
        {
            case InteractionType.Speak:
                cursor_imp.sprite = Cursor_Speak;
                cursor_perf.sprite = Cursor_Speak;
                cursor_imp2.sprite = Cursor_Speak;
                cursor_perf2.sprite = Cursor_Speak;
                break;
            case InteractionType.LookAt:
                cursor_imp.sprite = Cursor_LookAt;
                cursor_imp2.sprite = Cursor_LookAt;
                cursor_perf.sprite = Cursor_LookAt;
                cursor_perf2.sprite = Cursor_LookAt;
                break;
            case InteractionType.Aberration:
                cursor_imp.sprite = Cursor_Aberration;
                cursor_imp2.sprite = Cursor_Aberration;
                cursor_perf.sprite = Cursor_Aberration;
                cursor_perf2.sprite = Cursor_Aberration;
                break;
            case InteractionType.None:
                cursor_imp.sprite = Cursor_Standard;
                cursor_imp2.sprite = Cursor_Standard;
                cursor_perf.sprite = Cursor_Standard;
                cursor_perf2.sprite = Cursor_Standard;
                break;
        }

    }
}

public enum InteractionType
{
    Speak, LookAt, Aberration, None
}