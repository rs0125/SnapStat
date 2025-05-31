using UnityEngine;
using TMPro;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;

public class DeviationUIManager : MonoBehaviour
{
    public static DeviationUIManager Instance;

    public TextMeshProUGUI deviationLogText;
    public TextMeshProUGUI responseText;
    public float sensitivity = 2f;

    private List<Vector2> deviations = new List<Vector2>();

    void Awake()
    {
        Instance = this;
    }

    public void AddDeviation(Vector3 deviation)
    {
        Vector2 simplified = new Vector2(deviation.x *1000, deviation.y * 1000);
        deviations.Add(simplified);

        if (deviationLogText != null)
        {
            string jsonPreview = "[" + string.Join(", ", deviations.ConvertAll(d => $"({d.x:F2}, {d.y:F2})")) + "]";
            deviationLogText.text = "Deviation Log:\n" + jsonPreview;
        }

        if (deviations.Count == 8)
        {
            StartCoroutine(PostDeviationData());
        }
    }

    IEnumerator PostDeviationData()
    {
        DeviationPayload payload = new DeviationPayload
        {
            dpi = 800,
            sensitivity = sensitivity,
            deviations = new List<DeviationEntry>()
        };

        foreach (var d in deviations)
        {
            payload.deviations.Add(new DeviationEntry { x = d.x, y = d.y });
        }

        string json = JsonUtility.ToJson(payload);
        byte[] jsonBytes = System.Text.Encoding.UTF8.GetBytes(json);

        UnityWebRequest request = new UnityWebRequest("https://d2d7-122-179-17-89.ngrok-free.app/predict", "POST");
        request.uploadHandler = new UploadHandlerRaw(jsonBytes);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            Debug.Log("POST successful: " + request.downloadHandler.text);
            DisplayResponse(request.downloadHandler.text);
        }
        else
        {
            Debug.LogError("POST failed: " + request.responseCode + " - " + request.error);
        }

        deviations.Clear(); // Clear the list after sending
    }

    void DisplayResponse(string jsonResponse)
    {
        try
        {
            ResponseData response = JsonUtility.FromJson<ResponseData>(jsonResponse);

            if (responseText != null)
            {
                responseText.text =
                    $"Optimal Sensitivity: {response.optimal_sensitivity:F3}\n" +
                    $"Confidence Score: {response.confidence_score:P0}\n" +
                    $"Sensitivity Change: {response.sensitivity_change:F3}\n" +
                    $"Change Percentage: {response.change_percentage:F1}%\n\n" +
                    $"Recommendation:\n{response.recommendation}\n\n" +
                    $"Deviation Analysis:\n" +
                    $"- Mean X: {response.deviation_analysis.mean_deviation_x:F2}\n" +
                    $"- Mean Y: {response.deviation_analysis.mean_deviation_y:F2}\n" +
                    $"- Mean Magnitude: {response.deviation_analysis.mean_magnitude:F2}\n" +
                    $"- Std X: {response.deviation_analysis.std_deviation_x:F2}\n" +
                    $"- Std Y: {response.deviation_analysis.std_deviation_y:F2}\n" +
                    $"- Std Magnitude: {response.deviation_analysis.std_magnitude:F2}\n" +
                    $"- Total Shots: {response.deviation_analysis.total_shots}\n" +
                    $"- Consistency Score: {response.deviation_analysis.consistency_score:F3}\n" +
                    $"- Primary Direction: {response.deviation_analysis.primary_direction}\n\n" +
                    $"Bias Analysis:\n" +
                    $"- Horizontal Bias: {response.deviation_analysis.bias_analysis.horizontal_bias}\n" +
                    $"- Vertical Bias: {response.deviation_analysis.bias_analysis.vertical_bias}\n" +
                    $"- Overall Accuracy: {response.deviation_analysis.bias_analysis.overall_accuracy}\n\n" +
                    $"Timestamp: {response.timestamp}\n" +
                    $"Model Version: {response.model_version}";
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError("Failed to parse response JSON: " + e.Message);
            if (responseText != null)
                responseText.text = "Error parsing server response.";
        }
    }

    [System.Serializable]
    public class DeviationEntry
    {
        public float x;
        public float y;
    }

    [System.Serializable]
    public class DeviationPayload
    {
        public int dpi;
        public float sensitivity;
        public List<DeviationEntry> deviations;
    }

    [System.Serializable]
    public class ResponseData
    {
        public float optimal_sensitivity;
        public float confidence_score;
        public float sensitivity_change;
        public float change_percentage;
        public string recommendation;
        public DeviationAnalysis deviation_analysis;
        public string timestamp;
        public string model_version;
    }

    [System.Serializable]
    public class DeviationAnalysis
    {
        public float mean_deviation_x;
        public float mean_deviation_y;
        public float mean_magnitude;
        public float std_deviation_x;
        public float std_deviation_y;
        public float std_magnitude;
        public int total_shots;
        public float consistency_score;
        public string primary_direction;
        public BiasAnalysis bias_analysis;
    }

    [System.Serializable]
    public class BiasAnalysis
    {
        public string horizontal_bias;
        public string vertical_bias;
        public string overall_accuracy;
    }
}
