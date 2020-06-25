using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// todo: make generic Interactable class which this inherits some stuff from
// [RequireComponent(typeof(CircleCollider2D))]
public class Npc : MonoBehaviour
{
    [SerializeField] private List<string> lines;
    
    private List<string>.Enumerator _line;
    private CustomPlatformer2DUserControl _player;

    private Collider2D _collider;

    private void OnEnable()
    {
        _line = lines.GetEnumerator();
        _player = GameObject.FindGameObjectWithTag("Player").GetComponent<CustomPlatformer2DUserControl>();

        _collider = GetComponentInChildren<Collider2D>();
        if (!_collider)
            _collider = gameObject.AddComponent<CircleCollider2D>();
        // todo: make this more robust lol
        _collider.isTrigger = true;
    }

    public void Confirm()
    {
        if (!_line.MoveNext())
            _line = lines.GetEnumerator();
        
        DisplayLine(_line.Current);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player"))
            return;
        
        ShowBtnHint();
        _player.Interactable = this;
    }
    
    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("Player"))
            return;
        
        HideBtnHint();
        _player.Interactable = null;
    }
    
    private void ShowBtnHint()
    {
        Debug.Log("show help");
    }
    
    private void HideBtnHint()
    {
        Debug.Log("hide help");
    }

    private static void DisplayLine(string line)
    {
        Debug.Log("display line:");
        Debug.Log(line);
    }
}
