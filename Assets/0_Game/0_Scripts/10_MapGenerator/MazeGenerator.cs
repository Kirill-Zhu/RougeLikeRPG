using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using Zenject;
[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class MazeGenerator : MonoBehaviour {
    [System.Serializable]
    public class RoomPrefabs {
        public GameObject startRoom;      // Начальная комната
        public List<GameObject> plainRooms; // Обычные комнаты (коридоры, развилки, арены)
        public GameObject bossRoom;
        public GameObject closeWall;
    }

    [Inject] EnemiesManager enemiesManager;
    [Header("Настройки комнат")]

    public RoomPrefabs roomCollection;
    public int maxRoomsCount = 10; // Сколько всего комнат сгенерировать
    public int minRoomsCount = 5;
    private List<GameObject> spawnedRooms = new List<GameObject>();
    private List<Transform> availableDoors = new List<Transform>();
    List<Room> roomsList = new List<Room>();    

    void Start() {
        GenerateDungeon();

    }
    public Transform PlayerSpawnPointTransform() {
        foreach (Transform child in roomCollection.startRoom.GetComponentsInChildren<Transform>()) {
            if (child.name.Contains("SpawnPoint")) {
                return child;
            }
        }
        Debug.LogError("No Spawn Point in start room");
        return null;
    }

    [ContextMenu("Generate")]
    public async void GenerateDungeon() {
        // Очистка сцены при повторной генерации
        ClearDungeon();

        // 1. Спавним стартовую комнату в нулевых координатах
        GameObject startRoomInstance = Instantiate(roomCollection.startRoom, Vector3.zero, Quaternion.identity, transform);
        spawnedRooms.Add(startRoomInstance);

        // Собираем двери стартовой комнаты
        AddAvailableDoors(startRoomInstance);

        // 2. Генерируем обычные комнаты цепочкой
        int iterations = 0; // Защита от бесконечного цикла
        while (spawnedRooms.Count < maxRoomsCount - 1 && availableDoors.Count > 0 && iterations < 500) {
            iterations++;

            // Выбираем случайную свободную дверь на карте
            int doorIndex = Random.Range(0, availableDoors.Count);
            Transform targetDoor = availableDoors[doorIndex];

            // Выбираем случайный префаб обычной комнаты
            GameObject randomRoomPrefab = roomCollection.plainRooms[Random.Range(0, roomCollection.plainRooms.Count)];

            if (await TryPlaceRoom(randomRoomPrefab, targetDoor)) {
                // Если комната успешно встала, удаляем дверь, которую мы заняли
                availableDoors.RemoveAt(doorIndex);
            }
        }

        // 3. Пытаемся пристыковать комнату босса к самой последней оставшейся двери
        if (availableDoors.Count > 0) {
            var doorTransform = availableDoors[Random.Range(0, availableDoors.Count)];
            await TryPlaceRoom(roomCollection.bossRoom, doorTransform);
            availableDoors.Remove(doorTransform);
        }
        if (availableDoors != null && availableDoors.Count > 0) {

            for(int i =0; i < availableDoors.Count; i++) {
                PlaceClosedWall(availableDoors[i]);
            }
            availableDoors.Clear();
        }

        Debug.Log($"Генерация завершена. Создано комнат: {spawnedRooms.Count}");

        if (spawnedRooms.Count < minRoomsCount) {
            GenerateDungeon();
            return;
        }

        CombineAllChildMeshes();
        GenerateRoomProps();
    }

    void PlaceClosedWall(Transform doorTransform) {
        if (transform.gameObject.name.Contains("BossRoom")) return;

        var obj = Instantiate(roomCollection.closeWall, transform);
        obj.transform.position = doorTransform.position;
        obj.transform.rotation = doorTransform.rotation;
    }
     async UniTask<bool> TryPlaceRoom(GameObject roomPrefab, Transform targetDoor) {
        // Временно создаем комнату в стороне, чтобы рассчитать ее положение
        GameObject tempRoom = Instantiate(roomPrefab, Vector3.down * 500, Quaternion.identity);
        List<Transform> tempRoomDoors = GetDoorsInObject(tempRoom);

        if (tempRoomDoors.Count == 0) {
            Destroy(tempRoom);
            return false;
        }

        // Выбираем случайную дверь у новой комнаты
        Transform newRoomDoor = tempRoomDoors[Random.Range(0, tempRoomDoors.Count)];

        // Рассчитываем поворот только по оси Y (горизонтальная плоскость)
        float angleY = targetDoor.eulerAngles.y + 180f - newRoomDoor.localEulerAngles.y;
        tempRoom.transform.rotation = Quaternion.Euler(0, angleY, 0);

        // Рассчитываем позицию смещения
        Vector3 offset = targetDoor.position - newRoomDoor.position;

        // ОПТИМИЗАЦИЯ: Принудительно зануляем Y-смещение, чтобы комнаты не строились вверх/вниз
        offset.y = 0f;

        // Переносим комнату в вычисленную точку на плоской сетке XZ
        tempRoom.transform.position = offset;

        // Принудительно выравниваем саму комнату по высоте стартовой точки
        Vector3 flatPosition = tempRoom.transform.position;
        flatPosition.y = 0f; // Или transform.position.y стартовой комнаты
        tempRoom.transform.position = flatPosition;

        // --- ПРОВЕРКА НА НАЛОЖЕНИЕ (Overlap) ---
        Bounds newRoomBounds = GetRoomBounds(tempRoom);
        newRoomBounds.Expand(-0.1f);

        // Игнорируем высоту при проверке пересечений комнат (проверяем только X и Z)
        Vector3 extents = newRoomBounds.extents;
        extents.y = 100f; // Делаем проекцию Bounds бесконечно высокой, чтобы пресечь наложения на любом уровне Y
        newRoomBounds.extents = extents;

        foreach (GameObject spawnedRoom in spawnedRooms) {
            Bounds spawnedBounds = GetRoomBounds(spawnedRoom);
            Vector3 spawnedExtents = spawnedBounds.extents;
            spawnedExtents.y = 100f;
            spawnedBounds.extents = spawnedExtents;

            if (spawnedBounds.Intersects(newRoomBounds)) {
                // Найдено пересечение!
                Destroy(tempRoom);
                return false;
            }
        }

        // Если пересечений нет — фиксируем
        tempRoom.transform.parent = transform;
        spawnedRooms.Add(tempRoom);

        Room room = tempRoom.GetComponent<Room>();
        roomsList.Add(room);
        
        room.Initialize(enemiesManager, spawnedRooms.Count - 2);//Dont count Start Room

        foreach (Transform door in tempRoomDoors) {
            if (door != newRoomDoor) {
                availableDoors.Add(door);
            }
        }

        return true;
    }

    private void AddAvailableDoors(GameObject room) {
        availableDoors.AddRange(GetDoorsInObject(room));
    }

    private List<Transform> GetDoorsInObject(GameObject room) {
        List<Transform> doors = new List<Transform>();
        // Скрипт ищет дочерние объекты, у которых имя содержит "DoorWay" (или настройте поиск по тегу/компоненту)
        foreach (Transform child in room.GetComponentsInChildren<Transform>()) {
            if (child.name.Contains("DoorWay")) {
                doors.Add(child);
            }
        }
        return doors;
    }

    private Bounds GetRoomBounds(GameObject room) {
        // Автоматически вычисляет общий размер комнаты по всем её дочерним MeshRenderer'ам или Collider'ам
        var renderers = room.GetComponentsInChildren<MeshRenderer>();
        if (renderers.Length == 0) return new Bounds(room.transform.position, Vector3.one * 5);

        Bounds bounds = renderers[0].bounds;
        for (int i = 1; i < renderers.Length; i++) {
            bounds.Encapsulate(renderers[i].bounds);
        }
        return bounds;
    }

    private void ClearDungeon() {
        foreach (GameObject room in spawnedRooms) {
            if (room != null) Destroy(room);
        }
        foreach (Transform child in transform) {
            Destroy(child.gameObject);
        }
        spawnedRooms.Clear();
        availableDoors.Clear();
        roomsList.Clear();
    }

    [ContextMenu("Combine meshes")]
    public void CombineAllChildMeshes() {
        // 1. Находим все MeshFilter у дочерних объектов
        MeshFilter[] meshFilters = GetComponentsInChildren<MeshFilter>();

        // Создаем массив структур для объединения (исключая сам родительский объект)
        CombineInstance[] combine = new CombineInstance[meshFilters.Length - 1];

        // Сохраняем и временно обнуляем позицию родителя, 
        // чтобы локальные матрицы детей посчитались корректно в мировых координатах
        Vector3 oldPosition = transform.position;
        Quaternion oldRotation = transform.rotation;
        transform.position = Vector3.zero;
        transform.rotation = Quaternion.identity;

        int index = 0;
        //Material sharedMaterial = null;

        for (int i = 0; i < meshFilters.Length; i++) {
            // Пропускаем сам родительский объект, нам нужны только дети
            if (meshFilters[i].gameObject == gameObject) continue;

            //// Запоминаем материал (берем от первого попавшегося ребенка)
            //if (sharedMaterial == null) {
            //    sharedMaterial = meshFilters[i].GetComponent<MeshRenderer>().sharedMaterial;
            //}

            // Заполняем данные для объединения
            combine[index].mesh = meshFilters[i].sharedMesh;
            // Переводим локальные координаты ребенка в мировые координаты матрицы
            combine[index].transform = meshFilters[i].transform.localToWorldMatrix;

            // Выключаем старый дочерний объект, так как его геометрия теперь будет в родителе
            meshFilters[i].gameObject.SetActive(false);

            index++;
        }

        // 2. Создаем новый меш в родительском объекте и применяем настройки
        MeshFilter parentMeshFilter = GetComponent<MeshFilter>();
        parentMeshFilter.mesh = new Mesh();

        // Ограничение Unity по умолчанию — 65к вершин на меш (16-bit). 
        // Переключаем на 32-bit, чтобы можно было объединять миллионы полигонов за раз.
        parentMeshFilter.mesh.indexFormat = UnityEngine.Rendering.IndexFormat.UInt32;

        // ВТОРОЙ АРГУМЕНТ (true) — склеить все в ОДИН саб-меш (это и снижает Draw Calls до 1)
        parentMeshFilter.mesh.CombineMeshes(combine, true, true);

        //// Назначаем родителю общий материал
        //GetComponent<MeshRenderer>().sharedMaterial = sharedMaterial;

        // 3. Возвращаем родителя в исходную позицию на сцене
        transform.position = oldPosition;
        transform.rotation = oldRotation;

        // Опционально: создаем один общий физический коллайдер для всей карты
        if (GetComponent<MeshCollider>() == null) {
            gameObject.AddComponent<MeshCollider>().sharedMesh = parentMeshFilter.mesh;
        } else {
            GetComponent<MeshCollider>().sharedMesh = parentMeshFilter.mesh;
        }

        Debug.Log("Меши успешно объединены в один!");
    }

    void GenerateRoomProps() {
        foreach (var room in roomsList) {

            room.GenerateProps();
        }
    }
}
