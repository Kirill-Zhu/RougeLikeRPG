using UnityEngine;
public static class GameData {

    const string levelProgressKey = "LevelProgressKey";
    //Coiins
    public static void SetCoinsCount(int value) {
        PlayerPrefs.SetInt("Coins", value);
        Debug.Log($"Coins now is {value}");
    }
    public static int GetConins() {
        return PlayerPrefs.GetInt("Coins");
    }
    public static void AddCoins(int value) {
        int currentCoins = PlayerPrefs.GetInt("Coins");
        currentCoins += value;
        PlayerPrefs.SetInt("Coins", currentCoins);
        PlayerPrefs.Save();
    }

    public static void SpendCoins(int value) {
        int currentCoins = PlayerPrefs.GetInt("Coins");
        currentCoins -= value;
        if (currentCoins < 0) {
            Debug.LogWarning("Not enough coins");
            return;
        }
        PlayerPrefs.SetInt("Coins", currentCoins);
        PlayerPrefs.Save();
    }
    //Progress

    public static void ForceProgressKey(int value) {
        PlayerPrefs.SetInt(levelProgressKey, value);
    }
    public static void SetProgressKey(int newProgressKey) {
        int currentProgress = PlayerPrefs.GetInt(levelProgressKey);

        if (newProgressKey>currentProgress)
            PlayerPrefs.SetInt(levelProgressKey, newProgressKey);
    }
    public static int GetProgressKey() => PlayerPrefs.GetInt(levelProgressKey);

    //
}
