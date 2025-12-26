using UnityEngine;

public class DropTable : MonoBehaviour
{
    public ItemData dropItem;
    public int minCount = 1;
    public int maxCount = 1;

    public void Drop(Vector3 pos)
    {
        if (dropItem == null)
        {
            Debug.LogError($"[DropTable] dropItem Null {name}");
            return;
        }

        int count = Random.Range(minCount, maxCount + 1);

        for (int i = 0; i < count; i++)
        {
            ItemManager.Instance.SpawnPickupByItemId(
                dropItem.itemId,
                pos + Random.insideUnitSphere * 0.3f
            );
            
            SoundManager.Instance?.PlaySFX(dropItem.spawnSFX); // 사운드
        }
    }
}
