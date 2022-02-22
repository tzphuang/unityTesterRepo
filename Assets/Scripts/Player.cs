using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public ParticleSystem PSystem;
    public Material[] materials;
    private int materialCounter = 0;
    Renderer _renderer;
    private float horizontalInput;

    //this is actually the Z part of Vector3
    private float verticalInput;

    // Start is called before the first frame update
    void Start()
    {
        _renderer = GetComponent<Renderer>();
        _renderer.enabled = true;
        //Debug.Log("materials length: " + materials.Length);
        _renderer.sharedMaterial = materials[0];

        PSystem.Stop();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            RotatePlayerColor();
        }

        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");
    }


    void RotatePlayerColor()
    {
        if(materialCounter +1 >= materials.Length)
        {
            materialCounter = 0;
        }
        else
        {
            materialCounter++;
        }

        _renderer.sharedMaterial = materials[materialCounter];
    }

    private void FixedUpdate()
    {
        //if the player falls out of the screen, decrease lives
        if (!_renderer.isVisible)
        {
            GameManager.Instance.Lives--;
            Destroy(gameObject);
        }

        //fixed update for player movement so the player can continue to move
        //properly regardless of frame rate
        GetComponent<Rigidbody>().velocity = new Vector3(horizontalInput, GetComponent<Rigidbody>().velocity.y, verticalInput);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.name == "Lava")
        {
            PSystem.Play();
        }
        
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.name == "Lava")
        {
            PSystem.Stop();
        }
    }
}
