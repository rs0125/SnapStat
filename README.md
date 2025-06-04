# SnapStat: AI-Powered Aim Trainer

**SnapStat** is a Unity-based tool that analyzes your aim deviation and flick behavior using AI, helping FPS players optimize their mouse sensitivity for precision and performance.

![Screenshot 2025-05-31 181419](https://github.com/user-attachments/assets/9679d072-7f49-441c-bdbe-e34f97fe350b)

## Features

* 🔍 **AI-Powered Sensitivity Calibration**
  Automatically suggests your optimal sensitivity based on in-game performance.

* 📊 **Real-Time Deviation Tracking**
  Logs flicks and aim deviations to detect under/overflicking trends.

* 📦 **Seamless Unity Integration**
  Drop-in compatible with FPS projects using Unity Input System.

* 📈 **Detailed Aim Analytics**
  Includes deviation magnitudes, bias direction, consistency score, and more.

* ☁️ **Cloud-Powered Feedback**
  Sends deviation data to an AI model and receives improvement suggestions.

---

## How It Works

1. Player shoots at targets in the scene.
2. Each shot’s deviation from target center is logged.
3. After 8 shots, a POST request sends this data to an AI endpoint.
4. The response includes:

   * Optimal sensitivity
   * Deviation analysis (mean, std dev)
   * Personalized recommendation
5. This is displayed on-screen with full aim insight.

---

## 🛠️ Setup Instructions

1. Clone the repository:

   ```bash
   git clone https://github.com/rs0125/SnapStat.git
   ```
2. Open the project in Unity (6.0 or later recommended).
3. Plug in your own AI endpoint in `DeviationUIManager.cs`.
4. Assign required `TextMeshProUGUI` components in the Inspector.
5. Start the scene and shoot at targets to begin data collection.

---

## 🔧 Configuration

* **DPI**: Set statically in `DeviationUIManager.cs` (default: `800`)
* **Sensitivity**: Dynamically adjustable in-game via `W/S` keys
* **Shot Threshold**: 8 shots per batch before sending to API

---

## 📡 Server API

POST Endpoint: `https://<your-api-url>/predict`
Expected Payload:

```json
{
  "dpi": 800,
  "sensitivity": 2.5,
  "deviations": [{"x": 25.3, "y": 5.2}, ...]
}
```

---

## 🧪 Sample Response

```json
{
  "optimal_sensitivity": 1.53,
  "recommendation": "Consider increasing your sensitivity to reduce underflicking.",
  "deviation_analysis": {
    "mean_deviation_x": -22.7,
    "mean_magnitude": 23.3,
    "primary_direction": "left_bias"
  }
}
```

---

## 📌 Roadmap

* [ ] Sensitivity graphs
* [ ] Crosshair heatmaps
* [ ] Accuracy Assessment (Beyond Threshold Deviation)
* [ ] WebGL Release on Itch.io
* [ ] Player Profiles & Historical Data

---

## 💡 Inspiration

FPS gamers often guess their ideal sensitivity. AimSense eliminates the guesswork using real data and smart feedback.

---

## 🧑‍💻 Author

Made with ❤️ by [rs0125](https://github.com/rs0125)
Pull requests and suggestions welcome!

---
