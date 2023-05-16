using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using UnityEngine.SceneManagement;

public class LogoutButton : MonoBehaviour
{

    private string logoutUrl = "http://localhost:8000/api/users/log_out/";

    public void OnClick()
    {
        StartCoroutine(Logout());
    }

    IEnumerator Logout()
    {
        string authToken = PlayerPrefs.GetString("Token");

        UnityWebRequest request = UnityWebRequest.Post(logoutUrl, "POST");
        request.SetRequestHeader("Authorization", "Bearer " + authToken);
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            Debug.Log("Logout successful");
            SceneManager.LoadScene("login");
        }
        else
        {
            Debug.Log("Logout failed: " + request.error);
        }
    }
}
