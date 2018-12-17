using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class LinkSubMenu : MonoBehaviour
{
    public bool pressedYes = false;

    public GameObject main;
    private GameObject data;

    public GameObject collided_server;

    public RackObjectData.ObjectData device_one = new RackObjectData.ObjectData();
    public RackObjectData.ObjectData device_two = new RackObjectData.ObjectData();
    private int port_one, port_two;

    public bool menu_active;
    private int menu_option, last_option;

    private float right_stick_horiz, right_stick_vert;
    private bool up, down, left, right, horiz_active, vert_active;

    private Vector3 menu_sleep = new Vector3(0, 0, 0);
    private Vector3 menu_awake = new Vector3(0.0005f, 0.0005f, 0.0005f);
    private Vector3 menu_choice_awake = new Vector3(1, 1, 1);

    float routerMessageTimer = 100.0f;
    float messageDuration = 2.0f;

    GameObject player;

    private GameObject[] menu_options = new GameObject[6];
    GameObject title;
    GameObject menu;
    GameObject device_1;
    GameObject port_1;
    GameObject device_2;
    GameObject port_2;
    GameObject confirm;
    GameObject cancel;

    Color panel_default = Color.white;
    Color panel_highlight = Color.cyan;

    // Use this for initialization
    void Start()
    {
        // player
        player = GameObject.Find("/OVRPlayerController");

        // ojbect data
        data = GameObject.Find("/Racks");

        // Main Context Menu
        main = GameObject.Find("/OVRPlayerController/OVRCameraRig/TrackingSpace/RightHandAnchor/Main Menu");
        menu = GameObject.Find("/OVRPlayerController/OVRCameraRig/TrackingSpace/RightHandAnchor/Create LinkMenu");

        // Main Menu Choices
        title = GameObject.Find("/OVRPlayerController/OVRCameraRig/TrackingSpace/RightHandAnchor/Create LinkMenu/Title");
        device_1 = GameObject.Find("/OVRPlayerController/OVRCameraRig/TrackingSpace/RightHandAnchor/Create LinkMenu/Device 1");
        port_1 = GameObject.Find("/OVRPlayerController/OVRCameraRig/TrackingSpace/RightHandAnchor/Create LinkMenu/Port 1");
        device_2 = GameObject.Find("/OVRPlayerController/OVRCameraRig/TrackingSpace/RightHandAnchor/Create LinkMenu/Device 2");
        port_2 = GameObject.Find("/OVRPlayerController/OVRCameraRig/TrackingSpace/RightHandAnchor/Create LinkMenu/Port 2");
        confirm = GameObject.Find("/OVRPlayerController/OVRCameraRig/TrackingSpace/RightHandAnchor/Create LinkMenu/Confirm");
        cancel = GameObject.Find("/OVRPlayerController/OVRCameraRig/TrackingSpace/RightHandAnchor/Create LinkMenu/Cancel");

        menu_options[0] = device_1;
        menu_options[1] = device_2;
        menu_options[2] = port_1;
        menu_options[3] = port_2;
        menu_options[4] = confirm;
        menu_options[5] = cancel;

        title.GetComponentInChildren<Text>().text = "Link Two Devices";
        device_1.GetComponentInChildren<Text>().text = "Device 1";
        port_1.GetComponentInChildren<Text>().text = "Port 1";
        device_2.GetComponentInChildren<Text>().text = "Device 2";
        port_2.GetComponentInChildren<Text>().text = "Port 2";
        confirm.GetComponentInChildren<Text>().text = "Confirm";
        cancel.GetComponentInChildren<Text>().text = "Cancel";

        // Main Menu Panel Objects
        device_1.GetComponent<Image>().color = panel_highlight;
        device_2.GetComponent<Image>().color = panel_default;
        port_1.GetComponent<Image>().color = panel_default;
        port_2.GetComponent<Image>().color = panel_default;
        confirm.GetComponent<Image>().color = panel_default;
        cancel.GetComponent<Image>().color = panel_default;

        menu_option = 0;

        horiz_active = false;
        vert_active = false;

        device_one = null;
        device_two = null;

        // set menu inactive
        gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        operateMenu();
    }








    void operateMenu()
    {
        debounceInput(right_stick_horiz, right_stick_vert);

        if (menu_active)
        {
            // Menu Functionality (awake)
            right_stick_horiz = OVRInput.Get(OVRInput.Axis2D.SecondaryThumbstick).x;
            right_stick_vert = OVRInput.Get(OVRInput.Axis2D.SecondaryThumbstick).y;


            updateCursor();
            selectOption(menu_option);
        }
    }

    void debounceInput(float horiz, float vert)
    {
        // meant to move left/right
        if (Mathf.Abs(vert) < 0.5f)
        {
            // set inputs for one iteration
            if (left) left = false;
            if (right) right = false;

            if (horiz <= -0.8f && !horiz_active)
            {
                left = true;
                horiz_active = true;
            }
            else if (horiz >= 0.8f && !horiz_active)
            {
                right = true;
                horiz_active = true;
            }
            else if (horiz > -0.5f && horiz < 0.5f)
            {
                horiz_active = false;
            }
        }

        // meant to move up/down
        if (Mathf.Abs(horiz) < 0.5f)
        {
            // set inputs for one iteration
            if (up) up = false;
            if (down) down = false;

            if (vert <= -0.8f && !vert_active)
            {
                down = true;
                vert_active = true;
            }
            else if (vert >= 0.8f && !vert_active)
            {
                up = true;
                vert_active = true;
            }
            else if (vert > -0.5f && vert < 0.5f)
            {
                vert_active = false;
            }
        }
    }

    void updateCursor()
    {
        last_option = menu_option;

        switch (menu_option)
        {
            case 0:     // Device 1
                if (down) menu_option = 2;
                if (right) menu_option = 1;
                break;
            case 1:     // Device 2
                if (left) menu_option = 0;
                if (down) menu_option = 3;
                break;
            case 2:     // Port 1
                if (up) menu_option = 0;
                if (down) menu_option = 4;
                if (right) menu_option = 3;
                break;
            case 3:     // Port 2
                if (up) menu_option = 1;
                if (down) menu_option = 4;
                if (left) menu_option = 2;
                break;
            case 4:     // Confirm
                if (up) menu_option = 2;
                if (down) menu_option = 5;
                break;
            case 5:     // Cancel
                if (up) menu_option = 4;
                break;
        }

        // highlight new option
        if (last_option != menu_option)
        {
            menu_options[last_option].GetComponent<Image>().color = panel_default;
            menu_options[menu_option].GetComponent<Image>().color = panel_highlight;
        }

    }

    void selectOption(int option)
    {
        switch (option)
        {
            case 0: chooseDeviceOne(); break;
            case 1: chooseDeviceTwo(); break;
            case 2: choosePortOne(); break;
            case 3: choosePortTwo(); break;
            case 4: confirmLink(); break;
            case 5:
                if (OVRInput.GetDown(OVRInput.Button.One)) exitMenu();
                break;
        }

        if (OVRInput.GetDown(OVRInput.Button.Two)) exitMenu();
    }

    void chooseDeviceOne()
    {
        if(collided_server != null && OVRInput.GetDown(OVRInput.Button.One))
        {
            for(int i = 0; i < 12; i++)
            {
                if (data.GetComponent<RackObjectData>().routers[i].obj.name == collided_server.name)
                {
                    device_one = data.GetComponent<RackObjectData>().routers[i];
                    for (int j = 0; j < 8; j++)
                    {
                        if (!device_one.ports[j])
                        {
                            data.GetComponent<RackObjectData>().routers[i].ports[j] = true;
                            port_one = j;
                            break;
                        }
                    }
                    break;
                }
                else if (data.GetComponent<RackObjectData>().switches[i].obj.name == collided_server.name)
                {
                    device_one = data.GetComponent<RackObjectData>().switches[i];
                    for (int j = 0; j < 8; j++)
                    {
                        if (!device_one.ports[j])
                        {
                            data.GetComponent<RackObjectData>().switches[i].ports[j] = true;
                            port_one = j;
                            break;
                        }
                    }
                    break;
                }
            }

            if (device_one == null) port_1.GetComponentInChildren<Text>().text = "No Ports";
            else
            {
                device_1.GetComponentInChildren<Text>().text = "ID: " + device_one.name;
                port_1.GetComponentInChildren<Text>().text = "Port " + port_one.ToString();
            }
        }
    }

    void chooseDeviceTwo()
    {
        if (collided_server != null && OVRInput.GetDown(OVRInput.Button.One))
        {
            for (int i = 0; i < 12; i++)
            {
                if (data.GetComponent<RackObjectData>().routers[i].obj.name == collided_server.name)
                {
                    device_two = data.GetComponent<RackObjectData>().routers[i];
                    for (int j = 0; j < 8; j++)
                    {
                        if (!device_two.ports[j])
                        {
                            data.GetComponent<RackObjectData>().routers[i].ports[j] = true;
                            port_two = j;
                            break;
                        }
                    }
                    break;
                }
                else if (data.GetComponent<RackObjectData>().switches[i].obj.name == collided_server.name)
                {
                    device_two = data.GetComponent<RackObjectData>().switches[i];
                    for (int j = 0; j < 8; j++)
                    {
                        if (!device_two.ports[j])
                        {
                            data.GetComponent<RackObjectData>().switches[i].ports[j] = true;
                            port_two = j;
                            break;
                        }
                    }
                    break;
                }
            }

            if (device_two == null) port_2.GetComponentInChildren<Text>().text = "No Ports";
            else
            {
                device_2.GetComponentInChildren<Text>().text = "ID: " + device_two.name;
                port_2.GetComponentInChildren<Text>().text = "Port " + port_one.ToString();
            }
        }
    }

    void choosePortOne()
    {
    }

    void choosePortTwo()
    {
    }

    void confirmLink()
    {
        if (OVRInput.GetDown(OVRInput.Button.One)) {
            pressedYes = true;
        }
        if (device_one != null && device_two != null && OVRInput.GetDown(OVRInput.Button.One))
        {
            pressedYes = true;
            /*
                Call Victor's function
            */
            int p1;
            int a1;
            int p2;
            int a2;
            if (device_one.is_router)
            {
                p1 = 0;
                a1 = port_one;
            }
            else
            {
                p1 = port_one;
                a1 = 0;
            }

            if (device_two.is_router)
            {
                p2 = 0;
                a2 = port_two;
            }
            else
            {
                p2 = port_two;
                a2 = 0;
            }

            StartCoroutine(main.GetComponent<MainMenu>().projectHandle.CreateLink(device_one.id, device_two.id, p1, p2, a1, a2));
            // set up cable
            device_one = null;
            device_two = null;
        }
    }

    void exitMenu()
    {
        menu_active = false;
        gameObject.SetActive(false);
        main.SetActive(true);
    }
}