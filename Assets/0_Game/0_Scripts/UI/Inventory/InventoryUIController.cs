using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUIController : MonoBehaviour
{
    Hero hero;
    float lerpDuration = 1;
    float cellWidth = 100;
    float celHeight = 100;


    int[,] grid;
    int height = 3;
    int width = 3;  
    public void Initialize(Hero hero) {
        this.hero = hero;

        grid = new int[height, width];
    }

    internal void PutItemIntoEmptySlot(Vector2 startPos, Sprite label) {
        for(int x = 0; x < height; x++) 
            for(int y = 0;  y < width; y++) {
                if (grid[x, y] != 0) {
                    continue;
                }
                grid[x,y] = 1;

                //Need refactor 
                GameObject obj =new GameObject();
                obj .transform.parent = transform;
                obj.AddComponent<Image>().sprite = label;
                RectTransform rect = obj.GetComponent<RectTransform>();

                rect.sizeDelta = new Vector2(220, 220);
                rect.position = startPos;
                Vector2 endPos = new Vector2(x * cellWidth, y * celHeight);
                rect.DOAnchorPos(endPos, lerpDuration);
                rect.DOSizeDelta(new Vector2(cellWidth,celHeight), lerpDuration);
                return;
            }
    }
}

