using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEffectController : MonoBehaviour
{
    [SerializeField] private Transform playerEffectContainer;

    private Dictionary<EffectType, ParticleSystem> Effects = new();

    public void Damaged()
        => UseEffect(EffectType.Damaged, playerEffectContainer);

    public void Immateriality()
        => UseEffect(EffectType.Immateriality, playerEffectContainer);
    public void DarkCloud()
       => UseEffect(EffectType.DarkCloud, playerEffectContainer);
    public void ChillingTouch()
       => UseEffect(EffectType.ChillingTouch, playerEffectContainer);
    public void TownLegend()
       => UseEffect(EffectType.TownLegend, playerEffectContainer);
    public void PhantomOfTheOpera()
       => UseEffect(EffectType.PhantomOfTheOpera, playerEffectContainer);
    public void SlowMotion()
       => UseEffect(EffectType.SlowMotion, playerEffectContainer);

    public void LevelComplete()
    {
        List<Transform> positions = new()
        {
         CreateContainer("Congratulate1", new(-3.5f, 4f, 10f)),
         CreateContainer("Congratulate2", new(3.5f, 4f, 10f)),
         CreateContainer("Congratulate3", new(0f, 7f, 10f))
        };

        List<ParticleSystem> particles = new();

        var prefab = EffectConfig.Instance.GetEffectByType(EffectType.Congratulate);

        for (int i = 0; i < positions.Count; i++)
        {
            var effect = Instantiate(prefab, positions[i]);
            particles.Add(effect.GetComponent<ParticleSystem>());
        }

        particles.ForEach(p => p.Play());

    }

    private void UseEffect(EffectType type, Transform container)
    {
        if (Effects.ContainsKey(type) == false)
            SpawnEffect(type, container);

        if (EffectType.Damaged != type)
            StartCoroutine(BoosterDuration(type));

        Effects[type].Play();
    }

    private void SpawnEffect(EffectType type, Transform container)
    {
        var prefab = EffectConfig.Instance.GetEffectByType(type);
        var effect = Instantiate(prefab, container);
        Effects.Add(type, effect.GetComponent<ParticleSystem>());
    }

    private Transform CreateContainer(string name, Vector3 newPosition)
    {
        GameObject container = new(name);
        container.transform.position = newPosition;
        return container.transform;
    }

    private IEnumerator BoosterDuration(EffectType type)
    {
        Effects[type].gameObject.SetActive(true);
        yield return new WaitForSeconds(PlayerData.BOOSTERS_TIME);
        Effects[type].gameObject.SetActive(false);
    }
}
