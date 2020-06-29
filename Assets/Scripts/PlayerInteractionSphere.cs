using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteractionSphere : MonoBehaviour
{
    private CustomPlatformer2DUserControl _player;
    [SerializeField] private SpriteRenderer interactionHint;

    private void OnEnable()
    {
        _player = GameObject.FindGameObjectWithTag("Player").GetComponent<CustomPlatformer2DUserControl>();
        interactionHint.color = new Color(1,1,1,0);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        var interactable = other.GetComponent<Npc>();
        _player.Interactable = interactable;
        interactable.HoverEnter();
        interactionHint.color = new Color(1,1,1,1);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        _player.Interactable = null;
        other.GetComponent<Npc>().HoverExit();
        interactionHint.color = new Color(1,1,1,0);
    }
}
