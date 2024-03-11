using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

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

    [SerializeField]
    private GameObject pen;

    private void Start()
    {
        old_mouse_pos = new Vector2(0, 0);
    }

    public float penSpeed = 300;

    public Vector2 max_pen = new Vector2( 5, 5 );
    public Vector2 offset_pen = new Vector2( 5, 5 );
    public Vector2 minmax_cursor = new Vector2(5, 5);

    // Update is called once per frame
    void Update()
    {
        Vector2 mouse_pos = Input.mousePosition;

        Vector2 relative_mouse_pos = new Vector2(mouse_pos.x / Camera.main.pixelWidth, mouse_pos.y / Camera.main.pixelHeight);
        UnityEngine.Cursor.visible = relative_mouse_pos.x > 1 || relative_mouse_pos.x < 0 || relative_mouse_pos.y > 1 || relative_mouse_pos.y < 0;
        pen.transform.localPosition = relative_mouse_pos * max_pen + offset_pen;

        relative_mouse_pos *= 2;
        relative_mouse_pos -= new Vector2(1, 1);

        Debug.Log(relative_mouse_pos);
        
        old_mouse_pos = mouse_pos;
        
        TEST_CUBE.transform.localPosition = relative_mouse_pos * minmax_cursor;
        TEST_CUBE.transform.localPosition += (Input.GetAxis("Aberrations_Linger") < 0.5 ? real_cam.transform.position : perfect_cam.transform.position);
        TEST_CUBE.transform.localPosition += new Vector3(0,0,1f);

        Vector3 raycast_origin = TEST_CUBE.transform.localPosition;
        raycast_origin.z = -0.5f;

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