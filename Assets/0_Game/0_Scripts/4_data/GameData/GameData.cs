using UnityEngine;
public static class GameData {

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
}
