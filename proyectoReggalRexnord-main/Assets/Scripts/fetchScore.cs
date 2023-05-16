using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Collections;
using TMPro;
using UnityEditor.PackageManager.Requests;

public class LoginResponse
{
    public string user;
    public string token;
    public int user_id;
    public string total_score;
    public float average_score;
}

public class fetchScore : MonoBehaviour
{
    public TextMeshProUGUI totalScoreText;
    public TextMeshProUGUI averageScoreText;

    void Start()
    {
        StartCoroutine(GetUserData());
    }

    private IEnumerator GetUserData()
    {
        // Get the user's email and password from the player prefs
        string email = PlayerPrefs.GetString("Email");
        string password = PlayerPrefs.GetString("Password");

        // Create a JSON object to hold the login credentials
        string body = "{\"email\":\"" + email + "\",\"password\":\"" + password + "\"}"; //important;

        // Make a POST request to the Django backend's log_in API endpoint to retrieve the user ID and token
        UnityWebRequest loginRequest = new UnityWebRequest("http://localhost:8000/api/users/log_in/", "POST");
        byte[] data = new System.Text.UTF8Encoding().GetBytes(body);
        loginRequest.uploadHandler = (UploadHandler)new UploadHandlerRaw(data);
        loginRequest.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        loginRequest.SetRequestHeader("Content-Type", "application/json");
        yield return loginRequest.SendWebRequest();

        if (loginRequest.result == UnityWebRequest.Result.Success)
        {
            // Parse the JSON response to get the user ID and token
            string loginJsonResponse = loginRequest.downloadHandler.text;
            var loginResponse = JsonUtility.FromJson<LoginResponse>(loginJsonResponse);

            totalScoreText.text = "Puntuación total: " + loginResponse.total_score;
            averageScoreText.text = "Puntuación promedio: " + loginResponse.average_score;
        }
        else
        {
            // Display an error message
            Debug.LogError("Failed to log in: " + loginRequest.error);
        }
    }
}
