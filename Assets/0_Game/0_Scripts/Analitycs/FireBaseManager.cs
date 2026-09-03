using Firebase;
using Firebase.Database;
using Firebase.Analytics;
using Firebase.Extensions;
using UnityEngine;

public class FireBaseManager : MonoBehaviour {

    FirebaseApp app;
    void Start() {

        Firebase.FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task => {
            var dependencyStatus = task.Result;
            if (dependencyStatus == Firebase.DependencyStatus.Available) {
                // Create and hold a reference to your FirebaseApp,
                // where app is a Firebase.FirebaseApp property of your application class.
                app = Firebase.FirebaseApp.DefaultInstance;
                Debug.Log($"{app.Name}");
                TestDatabaseConnection();
                

                // Set a flag here to indicate whether Firebase is ready to use by your app.
            } else {
                UnityEngine.Debug.LogError(System.String.Format(
                  "Could not resolve all Firebase dependencies: {0}", dependencyStatus));
                // Firebase Unity SDK is not safe to use here.
            }
        });
    }

    void TestDatabaseConnection() {
        string databaseUrl = "https://rouguelikerpg-default-rtdb.firebaseio.com/";
        DatabaseReference dbRef = FirebaseDatabase.GetInstance(databaseUrl).RootReference;

        // Write a test value to verify the network connection
        dbRef.Child("connection_test").SetValueAsync("Unity is Connected!").ContinueWithOnMainThread(task => {
            if (task.IsCompletedSuccessfully) {
                Debug.Log("Success! Written test data to Firebase Realtime Database.");
            } else {
                Debug.LogError($"Failed to write test data: {task.Exception}");
            }
        });
    }

    public void LevelStart(string levelName) {

        FirebaseAnalytics.LogEvent(FirebaseAnalytics.EventLevelStart, new Parameter(FirebaseAnalytics.ParameterLevelName, levelName));
    }
}
