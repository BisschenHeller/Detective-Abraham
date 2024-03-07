using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class MyCursor : MonoBehaviour
{
    public Texture Cursor_Standard = null;

    public Texture Cursor_Speak = null;

    public Texture Cursor_LookAt = null;

    private SpriteRenderer sprite;

    private Vector2 old_mouse_pos;

    private void Start()
    {
        old_mouse_pos = new Vector2(0, 0);
        sprite = GetComponent<SpriteRenderer>();
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        
        Vector2 mouse_pos = Input.mousePosition;
        Cursor.visible = mouse_pos.x > 1920;
        Vector2 movement = (mouse_pos - old_mouse_pos) *0.01f;
        old_mouse_pos = mouse_pos;
        transform.Translate(movement.x, movement.y, 0);

        transform.localPosition = new Vector3(Mathf.Clamp(transform.localPosition.x, -9, 9), Mathf.Clamp(transform.localPosition.y,-5,5), transform.localPosition.z);
    }
}
