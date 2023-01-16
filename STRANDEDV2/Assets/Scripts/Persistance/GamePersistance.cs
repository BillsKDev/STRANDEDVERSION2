using UnityEngine;

public class GamePersistance : MonoBehaviour
{
    public GameData _gameData;

    void Start() => LoadGame();

    void Update() => SaveGame();

    void SaveGame()
    {
        var data = JsonUtility.ToJson(_gameData);
        PlayerPrefs.SetString("GameData", data);
    }

    void LoadGame()
    {
        var data = PlayerPrefs.GetString("GameData");
        _gameData = JsonUtility.FromJson<GameData>(data);
        if (_gameData == null)
            _gameData = new GameData();

        Inventory.Instance.Bind(_gameData.SlotDatas);
        PlacementManager.Instance.Bind(_gameData.PlaceableDatas);
        InteractionManager.Bind(_gameData.InteractableDatas);
    }
}