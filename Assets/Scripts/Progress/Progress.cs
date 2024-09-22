using UnityEngine;

[System.Serializable]
public partial class Progress : ProgressBase<Progress>
{
    [SerializeField] private InventoryProgress inventory = new InventoryProgress();
    [SerializeField] private OptionsSave options = new OptionsSave();
    public static InventoryProgress Inventory { get => instance.inventory; set => instance.inventory = value;}
    public static OptionsSave Options { get => instance.options; set => instance.options = value;}
    
}