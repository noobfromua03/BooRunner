using UnityEngine;
public class DisableZone : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent<IPoolObject>(out var poolObject))
            (poolObject as MonoBehaviour).gameObject.SetActive(false);
        if (other.gameObject.TryGetComponent<RoadPart>(out var roadPart))
            roadPart.onFinish?.Invoke(roadPart);
    }
}

