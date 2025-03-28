using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Playables;
using UnityEngine.UI;
//using InTheHand.Net.Sockets;

public class DeviceConnection : MonoBehaviour
{
    //wifi anim
    private bool searching = true;
    public Image searchImg;
    public Sprite[] wifiAnimSprites;
    public float frameRate = 100f;
    private int currentFrame = 0;
    private float timer = 0f;

    //devices
    public Transform devicesPanel;
    public GameObject deciveComp;
    private List<string> names = new List<string>() { "Test device 1", "Test device 2" };

    //connection
    public GameObject connectButton;
    public DeviceItemHover currentSelected = null;

    void Start()
    {

        PopulateList();
    }

    void Update()
    {
        AnimateSearch();
    }

    private void AnimateSearch()
    {
        if (wifiAnimSprites == null || wifiAnimSprites.Length == 0) return;

        timer += Time.deltaTime;
        if (timer >= frameRate)
        {
            timer -= frameRate;
            currentFrame = (currentFrame + 1) % wifiAnimSprites.Length;
            searchImg.sprite = wifiAnimSprites[currentFrame];
        }
    }

    private void SearchForDevices()
    {

    }

    private void DisplayDevices()
    {
        searchImg.enabled = false;


    }

    void PopulateList()
    {
        foreach (string name in names)
        {
            GameObject newItem = Instantiate(deciveComp, devicesPanel);

            Text nameText = newItem.GetComponentInChildren<Text>();
            if (nameText != null)
            {
                nameText.text = name;
            }

            RectTransform rt = newItem.GetComponent<RectTransform>();
            if (rt != null)
            {
                rt.sizeDelta = new Vector2(rt.sizeDelta.x, 90);
            }

            DeviceItemHover itemHover = newItem.GetComponent<DeviceItemHover>();
            if (itemHover != null)
            {
                itemHover.manager = this;
            }
        }
    }

    public void ShowConnectButton(DeviceItemHover selectedDevice)
    {
        if (currentSelected != null && currentSelected != selectedDevice)
        {
            currentSelected.Deselect();
        }

        currentSelected = selectedDevice;
        selectedDevice.Select();

        if (connectButton != null)
        {
            connectButton.SetActive(true);
        }

        Text deviceNameText = selectedDevice.GetComponentInChildren<Text>();
        if (deviceNameText != null)
        {
            Debug.Log("Selected --- > " + deviceNameText.text);
        }
    }

}
