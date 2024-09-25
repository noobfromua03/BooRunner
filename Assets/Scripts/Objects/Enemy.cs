using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour, IPoolObject
{
    [SerializeField] private PoolObjectType objectType;
    [SerializeField] private int boldLevel;
    [field: SerializeField] public MoveUnit MoveUnit { get; private set; }
    public PoolObjectType ObjectType { get => objectType; }
    public int BoldLevel { get => boldLevel; }

    public bool actionDone;

    private List<Color32> colors;
    private ParticleSystem radianceEffect;
    private ParticleSystem scaredEffect;

    private void Start()
    {
        MovementController.instance.AddMoveUnit(MoveUnit);
        MoveUnit.currentPatrolLine = MoveUnit.PointNumber;
        colors = ColorConfig.Instance.Colors;
        GetRadianceEffect();
    }

    public void ChangeLine(int line)
    {
        MoveUnit.PointNumber = line;
        TeleportToPosition();
    }
    private void OnEnable()
    {
        TeleportToPosition();
        actionDone = false;
        if (radianceEffect != null)
            radianceEffect.Play();
    }

    public void TeleportToPosition()
    {
        MovementController.instance.TeleportToPosition(MoveUnit);
    }

    public void GetPatrolLine(List<bool> lines, int line)
    {
        if (lines[1] == true && line != 1)
        {
            MoveUnit.currentPatrolLine = MoveUnit.PointNumber;
            return;
        }

        if (line == 0 && !lines[1] && !lines[2])
            MoveUnit.currentPatrolLine = Random.Range(1, 3);

        else if (line == 0 && lines[2])
            MoveUnit.currentPatrolLine = 1;

        else if (line == 1 && !lines[0] && !lines[2])
            MoveUnit.currentPatrolLine = Random.value <= 0.5f ? 0 : 2;

        else if (line == 1 && !lines[0] && lines[2])
            MoveUnit.currentPatrolLine = 0;

        else if (line == 1 && lines[0] && !lines[2])
            MoveUnit.currentPatrolLine = 2;

        else if (line == 1 && lines[0] && lines[2])
            MoveUnit.currentPatrolLine = 1;

        else if (line == 2 && !lines[0] && !lines[1])
            MoveUnit.currentPatrolLine = Random.Range(0, 2);

        else if (line == 2 && lines[0])
            MoveUnit.currentPatrolLine = 1;
    }

    public bool CompareBold(int essence, int boldLevel)
    {
        if (PlayerData.Instance.IsChillingTouch.Status || PlayerData.Instance.IsPhantomOfTheOpera.Status)
            boldLevel /= 2;
        return essence >= boldLevel;
    }

    public void ActionHandler()
    {
        if (CompareBold(PlayerData.Instance.FearEssence, boldLevel) || PlayerData.Instance.IsTownLegend.Status)
        {
            PlayerData.Instance.RemoveEssence(BoldLevel);
            ScaredJump();
        }
        else
            PlayerData.Instance.RemoveLife(1);
        actionDone = true;
    }

    public void PhantomOfTheOperaHandler()
    {
        if (CompareBold(PlayerData.Instance.FearEssence, boldLevel) || PlayerData.Instance.IsTownLegend.Status)
        {
            PlayerData.Instance.RemoveEssence(BoldLevel);
            ScaredJump();
            actionDone = true;
        }
    }

    public void ScaredJump()
    {
        if (scaredEffect == null)
            GetScareEffect();

        MoveUnit.jump = true;
        scaredEffect.Play();

        AudioManager.Instance.PlayAudioByType(AudioType.Scared, AudioSubType.Sound);
    }


    public bool ActionDone()
        => actionDone;
    public bool ActiveStatus()
        => gameObject.activeSelf;

    private void GetScareEffect()
    {
        var prefab = EffectConfig.Instance.GetEffectByType(EffectType.Scared);
        scaredEffect = Instantiate(prefab, transform).GetComponent<ParticleSystem>();
    }

    private void GetRadianceEffect()
    {
        var prefab = EffectConfig.Instance.GetEffectByType(EffectType.EnemyRadiance);
        radianceEffect = Instantiate(prefab, transform).GetComponent<ParticleSystem>();
        SetRadianceColor();
        radianceEffect.Play();
    }

    private void SetRadianceColor()
    {
        var mainModule = radianceEffect.main;
        ParticleSystem.MinMaxGradient gradient = new();

        if (boldLevel == 0)
            gradient = new ParticleSystem.MinMaxGradient(colors[0]);
        else if (boldLevel == 25)
            gradient = new ParticleSystem.MinMaxGradient(colors[1]);
        else if (boldLevel == 50)
            gradient = new ParticleSystem.MinMaxGradient(colors[2]);
        else if (boldLevel == 75)
            gradient = new ParticleSystem.MinMaxGradient(colors[3]);
        else if (boldLevel == 100)
            gradient = new ParticleSystem.MinMaxGradient(colors[4]);

        mainModule.startColor = gradient;
    }
}
