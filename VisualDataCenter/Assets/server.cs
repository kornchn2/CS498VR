using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class server : MonoBehaviour {

    public bool highlight;
    public GameObject hand;
    public Renderer mesh;

    public GameObject collided_server;

    GameObject link_menu;

    Color temp;

    // Use this for initialization
    void Start () {
        BoxCollider boxCollider = gameObject.GetComponent<BoxCollider>();
        boxCollider.isTrigger = true;
        hand = GameObject.Find("/OVRPlayerController/OVRCameraRig/TrackingSpace/RightHandAnchor");
        link_menu = GameObject.Find("/OVRPlayerController/OVRCameraRig/TrackingSpace/RightHandAnchor/Create LinkMenu");
        highlight = false;
        mesh = gameObject.GetComponent<MeshRenderer>();
        temp = mesh.material.color;
    }
	
	// Update is called once per frame
	void Update () {
        if (highlight && link_menu.activeSelf)
        {
            mesh.material.color = Color.magenta;
        }
        else mesh.material.color = temp; 
	}
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == hand)
        {
            highlight = true;
            link_menu.GetComponent<LinkSubMenu>().collided_server = gameObject;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == hand)
        {
            highlight = false;
            link_menu.GetComponent<LinkSubMenu>().collided_server = null;
        }
    }
}
