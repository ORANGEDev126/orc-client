using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using VoltstroStudios.UnityWebBrowser.Core;

public class TestWebCommunicator : MonoBehaviour
{
    [SerializeField] private BaseUwbClientManager ClientManager;
    private WebBrowserClient WebBrowserClient;

    private void Start()
    {
        WebBrowserClient = ClientManager.browserClient;
    }

    public void LoadCustomSite()
    {
        WebBrowserClient.LoadUrl("http://3.35.41.153/login");
        WebBrowserClient.OnLoadFinish += WebBrowserClient_OnLoadFinish;
    }

    private void WebBrowserClient_OnLoadFinish(string url)
    {
        if (url.Contains("code="))
        {
            StartCoroutine(GetText(url));
        }
    }

    public void OnButtonClicked()
    {
        //StartCoroutine(GetText());
        LoadCustomSite();
    }

    IEnumerator GetText(string url = "")
    {
        UnityWebRequest www = UnityWebRequest.Get(url);
        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.Log(www.error);
        }
        else
        {
            // Show results as text
            Debug.Log(www.downloadHandler.text);

            // Or retrieve results as binary data
            byte[] results = www.downloadHandler.data;
        }
    }
}