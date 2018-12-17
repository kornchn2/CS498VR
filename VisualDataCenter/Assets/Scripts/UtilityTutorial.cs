using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UtilityTutorial : MonoBehaviour
{

    public GameObject panel;
    public GameObject user;
    private bool paused;

    // Use this for initialization
    void Start()
    {
        paused = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (OVRInput.GetDown(OVRInput.RawButton.Y))
        {
            panel.transform.position = new Vector3(user.transform.position.x, user.transform.position.y, user.transform.position.z + 3);

            paused = true;

        }
        if (paused)
        {
            panel.SetActive(true);

            Time.timeScale = 0f;
            if (OVRInput.GetDown(OVRInput.RawButton.X))
            {
#if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
#else
                    Application.Quit();
#endif
            }
            else if (OVRInput.GetDown(OVRInput.RawButton.B))
            {
                Time.timeScale = 1f;
                panel.SetActive(false);
                paused = false;
            }
        }
    }
}
