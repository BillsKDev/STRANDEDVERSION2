using UnityEngine;

public class CraftingManager : MonoBehaviour
{
    [SerializeField] Recipe[] _recipes;

    public void TryCrafting()
    {
        foreach (var recipe in _recipes)
        {
            if (IsMatchingRecipe(recipe, Inventory.Instance.CraftingSlot))
            {
                Inventory.Instance.ClearCraftingSlots();

                foreach (var reward in recipe.Rewards)
                    Inventory.Instance.AddItem(reward, InventoryType.Crafting);
                    return;
            }
        }
    }

    bool IsMatchingRecipe(Recipe recipe, ItemSlot[] craftingSlots)
    {
        for (int i = 0; i <recipe.Ingrediants.Count; i++)
        {
            if (recipe.Ingrediants[i] != craftingSlots[i].Item)
                return false;
        }

        for (int i = recipe.Ingrediants.Count; i < craftingSlots.Length; i++)
        {
            if (craftingSlots[i].IsEmpty == false)
                return false;
        }
        
        return true;
    }

#if UNITY_EDITOR
    void OnValidate() => _recipes = Extensions.GetAllInstances<Recipe>();
#endif
}
