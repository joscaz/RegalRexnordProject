using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Collections;
using TMPro;
using UnityEngine.SceneManagement;

public class TokenResponse {
    public string token;
}


public class LoginButton : MonoBehaviour
{
    public TMP_InputField usernameInputField;
    public TMP_InputField passwordInputField;

    public void OnLoginButtonClicked()
    {
        StartCoroutine(LoginCoroutine());
    }

    private IEnumerator LoginCoroutine()
    {
        // Get the user's login credentials from the input fields
        string email = usernameInputField.text;
        string password = passwordInputField.text;

        string body = "{\"email\":\"" +email +"\",\"password\":\"" +password +"\"}"; //important

        // Make a POST request to the Django backend's API endpoint for user authentication
        UnityWebRequest request = new UnityWebRequest("http://localhost:8000/api/users/log_in/","POST");
        byte[] data = new System.Text.UTF8Encoding().GetBytes(body);
        request.uploadHandler = (UploadHandler)new UploadHandlerRaw(data);
        request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");
        //request.SetRequestHeader("Authorization", "Token ");
        yield return request.SendWebRequest();


        if (request.result == UnityWebRequest.Result.Success)
        {
            // Parse the JSON response into a C# object or Unity component
            string jsonResponse = request.downloadHandler.text;
            Debug.Log(jsonResponse);
            TokenResponse authResponse = JsonUtility.FromJson<TokenResponse>(jsonResponse);
            PlayerPrefs.SetString("Token", authResponse.token);
            PlayerPrefs.SetString("Email", email);
            PlayerPrefs.SetString("Password", password);
            Debug.Log(authResponse.token);
            SceneManager.LoadScene("MainMenu");

            /*if (authResponse.success)
            {
                // Allow the user to access the game
                Debug.Log("Login successful");
                SceneManager.LoadScene("SampleScene");
            }
            else
            {
                // Display an error message
                Debug.LogError("Login failed: " + authResponse.message);
            }*/
        }
        else
        {
            // Display an error message
            Debug.LogError("Login failed: " + request.error);
        }
    }
}

[System.Serializable]
public class AuthResponse
{
    public bool success;
    public string message;
}
