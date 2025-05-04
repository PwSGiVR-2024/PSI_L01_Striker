using System.Linq;
using System.Net;
using UnityEngine;
using System.Net.Sockets;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class ConnectionMenuManager : MonoBehaviour
{
    public GameObject appPanel;
    public GameObject ipPanel;
    public GameObject connectedPanel;

    public Text ipAddrText;

    private bool _wasConnected = false;

    void Start()
    {

    }

    void Update()
    {
        bool now = WebsocketConnector.Instance.DeviceConnected;
        if (now && !_wasConnected)
        {
            ShowConnectedPanel();
        }
        _wasConnected = now;
    }

    public void AppPanelButtonClicked()
    {
        ipAddrText.text = GetUserIpAddr();

        appPanel.SetActive(false);
        ipPanel.SetActive(true);
    }

    private void ShowConnectedPanel()
    {
        ipPanel.SetActive(false);
        connectedPanel.SetActive(true);

        //game screen --->
        //StartCoroutine(DelayedLoad(game, 5f));
    }


    /* DEV PURP -- TODEL */
    public void DevButtonClicked()
    {
        SanityManager.instance.EnableSpoof(SpoofMode.Normal);

        //----> game screen
        //SceneManager.LoadScene(game);
    }


    //Helpers
    private string GetUserIpAddr()
    {
        var host = Dns.GetHostEntry(Dns.GetHostName());

        var ip = host.AddressList.FirstOrDefault(a => a.AddressFamily == AddressFamily.InterNetwork && !IPAddress.IsLoopback(a));

        return ip.ToString() ?? "192.168.0.143";
    }

    private IEnumerator DelayedLoad(string nextSceene, float delay)
    {
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene(nextSceene);
    }
}
