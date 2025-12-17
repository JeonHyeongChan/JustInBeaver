using UnityEngine;

public class CaptureObject : MonoBehaviour
{
    public float gatherDuration = 2f;

    private DropTable dropTable;

    private void Awake()
    {
        dropTable = GetComponent<DropTable>();
    }

    public void OnGatherComplete()
    {
        //드랍
        if (dropTable != null && dropTable.dropItem != null)
        {
            int count = Random.Range(dropTable.minCount, dropTable.maxCount + 1);

            for (int i = 0; i < count; i++)
            {
                Vector3 p = transform.position + Vector3.up * 0.5f;
                ItemManager.Instance.SpawnItem(dropTable.dropItem, p, Quaternion.identity);
            }
        }
        //갈무리 대상 오브젝트를 풀로 반환
        PoolManager.Instance.Despawn(gameObject);
    }
}
