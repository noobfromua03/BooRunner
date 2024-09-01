using UnityEngine;

[System.Serializable]
public partial class Progress : ProgressBase<Progress>
{
    [SerializeField] private InventoryProgress inventory = new InventoryProgress();

    public static InventoryProgress Inventory { get => instance.inventory; set => instance.inventory = value;}
}