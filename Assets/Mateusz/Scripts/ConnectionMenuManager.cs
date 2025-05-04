using System.Linq;
using System.Net;
using UnityEngine;
using System.Net.Sockets;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class ConnectionMenuManager : MonoBehaviour
{
    public GameObject startPanel;
    public GameObject appPanel;
    public GameObject ipPanel;
    public GameObject connectedPanel;

    public Text startText;
    public Text startTextShadow;
    public Text ipAddrText;

    private bool _wasConnected = false;

    void Start()
    {
        StartCoroutine(DelayAppConnection());
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
        StartCoroutine(DelayedLoad("TestScene", 5f));
    }


    /* DEV PURP -- TODEL */
    public void DevButtonClicked()
    {
        SanityManager.instance.EnableSpoof(SpoofMode.Normal);

        //----> game screen
        SceneManager.LoadScene("TestScene");
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

    private IEnumerator DelayAppConnection()
    {
        yield return new WaitForSeconds(3f - 1f);

        Color original = startText.color;
        Color originalShadow = startTextShadow.color;

        float t = 0f;
        while (t < 1f)
        {
            float alpha = Mathf.Lerp(1f, 0f, t / 1f);
            startText.color = new Color(original.r, original.g, original.b, alpha);
            startTextShadow.color = new Color(originalShadow.r, originalShadow.g, originalShadow.b, alpha);

            t += Time.deltaTime;
            yield return null;
        }

        startText.color = new Color(original.r, original.g, original.b, 0f);
        startTextShadow.color = new Color(originalShadow.r, originalShadow.g, originalShadow.b, 0f);

        startPanel.SetActive(false);
        appPanel.SetActive(true);
    }
}
