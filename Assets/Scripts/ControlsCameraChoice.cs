using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ControlsCameraChoice : MonoBehaviour
{
    public GameObject menuCanvas;
    public List<GameObject> cameraPrefabs;
    public List<GameObject> controlPrefabs;

    public GameObject car;

    public TMP_Dropdown camerasDropdown;
    public TMP_Dropdown controlsDropdown;

    bool isMenuOn = true;

    GameObject currentCamera;
    GameObject currentControls;

    int lastCamerasDropdownValue = 0;
    int lastControlsDropdownValue = 0;


    public static ControlsCameraChoice instance;

    private void Awake()
    {
        instance = this;
    }

    
    void Start()
    {
        SetupDropdowns();
        NewCamera();
        NewControls();
    }

   
    void Update()
    {
        CheckForDropdownChange();
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ToggleMenu();
        }
    }

    void ToggleMenu()
    {
        isMenuOn = !isMenuOn;
        menuCanvas.SetActive(isMenuOn);
    }

    public void OnButtonPressOK()
    {
        // NewCamera();
        //NewControls();
        Time.timeScale = 1;
    }


    void SetupDropdowns()
    {
        camerasDropdown.options.Clear();

        foreach (GameObject i in cameraPrefabs)
        {
            camerasDropdown.options.Add(new TMP_Dropdown.OptionData() { text = i.name });
        }

        foreach (GameObject i in controlPrefabs)
        {
            controlsDropdown.options.Add(new TMP_Dropdown.OptionData() { text = i.name });
        }
    }

    public GameObject GetCar()
    {
        return car;
    }
    
    void NewCamera()
    {
        lastCamerasDropdownValue = camerasDropdown.value;
        Destroy(currentCamera);
        currentCamera = Instantiate(cameraPrefabs[camerasDropdown.value]);
    }

    void NewControls()
    {
        lastControlsDropdownValue = controlsDropdown.value;
        Destroy(currentControls);
        currentControls = Instantiate(controlPrefabs[controlsDropdown.value]);
    }

    void CheckForDropdownChange()
    {
        if (lastCamerasDropdownValue != camerasDropdown.value) NewCamera();
        if (lastControlsDropdownValue != controlsDropdown.value) NewControls();
    }
}
