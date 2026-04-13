using UnityEngine;

public class DropTable : MonoBehaviour
{
    public ItemData dropItem;
    public int minCount = 0;
    public int maxCount = 0;

    public void Drop(Vector3 pos)
    {
        if (dropItem == null)
        {
            return;
        }
        int count = minCount+maxCount;
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
