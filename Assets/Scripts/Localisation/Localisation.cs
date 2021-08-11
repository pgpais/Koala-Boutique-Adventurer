using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Localisation
{
    public static Language currentLanguage { get; private set; } = Language.Portuguese;

    public static Dictionary<Language, Dictionary<StringKey, string>> languageStringDictionary = new Dictionary<Language, Dictionary<StringKey, string>>(){
        {
            Language.English, new Dictionary<StringKey, string>(){
                {StringKey.Class_Ranger_Name, "Ranger"},
                {StringKey.Class_Rogue_Name, "Rogue"},
                {StringKey.Class_Warrior_Name, "Warrior"},

                {StringKey.Item_RedMushroom_Name, "Red Mushroom"},
                {StringKey.Item_SlimMushroom_Name, "Slim Mushroom"},
                {StringKey.Item_TreeMushroom_Name, "Tree Mushroom"},
                {StringKey.Item_AlgaeMushroom_Name, "Algae Mushroom"},
                {StringKey.Item_BasicMushroom_Name, "Basic Mushroom"},
                {StringKey.Item_GnomeMushroom_Name, "Gnome Mushroom"},
                {StringKey.Item_HairyMushroom_Name, "Hairy Mushroom"},
                {StringKey.Item_ShellMushroom_Name, "Shell Mushroom"},
                {StringKey.Item_DragonMushroom_Name, "Dragon Mushroom"},
                {StringKey.Item_PurpleMushroom_Name, "Purple Mushroom"},

                {StringKey.Item_Berries_Name, "Berries"},
                {StringKey.Item_CoalOre_Name, "Coal"},
                {StringKey.Item_Fish_Name, "Fish"},
                {StringKey.Item_GoldOre_Name, "Gold Ore"},
                {StringKey.Item_IronOre_Name, "Iron Ore"},
                {StringKey.Item_Meat_Name, "Meat"},
                {StringKey.Item_WoodLog_Name, "Wood Log"},

                {StringKey.Item_GoldIngot_Name, "Gold Ingot"},
                {StringKey.Item_IronIngot_Name, "Iron Ingot"},
                {StringKey.Item_CoalProcessed_Name, "Processed Coal"},
                {StringKey.Item_WoodPlank_Name, "Wood Plank"},

                {StringKey.Item_Coin_Name, "Coin"},
                {StringKey.Item_Gem_Name, "Gem"},

                {StringKey.HUD_LootFeedback, "You got $AMOUNT$x $ITEM$!"},

                {StringKey.UI_DailyQuestDescription, "Sell the number of items shown"},
                {StringKey.UI_DailyQuestReward, "Increased Damage"},

                {StringKey.Buff_Afraid_Name, "Afraid"},
                {StringKey.Buff_Blessing_Name, "Blessing"},
                {StringKey.Buff_LuckyCharm_Name, "Lucky Charm"},
                {StringKey.Buff_DeflectingArmor_Name, "Deflecting Armor"},
                {StringKey.Buff_IronBoots_Name, "Iron Boots"},
                {StringKey.Buff_LifeSteal_Name, "Life Steal"},
                {StringKey.Buff_Poison_Name, "Poison"},
                {StringKey.Buff_Spread_Name, "Spread"},

                {StringKey.Unlock_Buff_Altar_Name, "Altar"},
                {StringKey.Unlock_Buff_Afraid_Name, "Afraid"},
                {StringKey.Unlock_Buff_Blessing_Name, "Blessing"},
                {StringKey.Unlock_Buff_LuckyCharm_Name, "Lucky Charm"},
                {StringKey.Unlock_Buff_DeflectingArmor_Name, "Deflecting Armor"},
                {StringKey.Unlock_Buff_IronBoots_Name, "Iron Boots"},
                {StringKey.Unlock_Buff_LifeSteal_Name, "Life Steal"},
                {StringKey.Unlock_Buff_Poison_Name, "Poison"},
                {StringKey.Unlock_Buff_Spread_Name, "Spread"},

                {StringKey.Unlock_Item_RedMushroom_Name, "Red Mushroom"},
                {StringKey.Unlock_Item_SlimMushroom_Name, "Slim Mushroom"},
                {StringKey.Unlock_Item_TreeMushroom_Name, "Tree Mushroom"},
                {StringKey.Unlock_Item_AlgaeMushroom_Name, "Algae Mushroom"},
                {StringKey.Unlock_Item_GnomeMushroom_Name, "Gnome Mushroom"},
                {StringKey.Unlock_Item_HairyMushroom_Name, "Hairy Mushroom"},
                {StringKey.Unlock_Item_ShellMushroom_Name, "Shell Mushroom"},
                {StringKey.Unlock_Item_DragonMushroom_Name, "Dragon Mushroom"},
                {StringKey.Unlock_Item_PurpleMushroom_Name, "Purple Mushroom"},

                {StringKey.Unlock_Item_FoodMarket_Name, "Food Market"},
                {StringKey.Unlock_Item_Groceries_Name, "Groceries"},
                {StringKey.Unlock_Item_Jeweler_Name, "Jeweler"},

                {StringKey.Unlock_Stats_Cooking_Name, "Cooking"},
                {StringKey.Unlock_Stats_DancingLessons_Name, "Dancing Lessons"},
                {StringKey.Unlock_Stats_TrainingDummy_Name, "Training Dummy"},

                {StringKey.Unlock_Class_Rogue_Name, "Rogue"},
                {StringKey.Unlock_Class_Warrior_Name, "Warrior"},
            }
        },
        {
            Language.Portuguese, new Dictionary<StringKey, string>(){
                {StringKey.Class_Ranger_Name, "Arqueiro"},
                {StringKey.Class_Rogue_Name, "Assassino"},
                {StringKey.Class_Warrior_Name, "Guerreiro"},

                {StringKey.Item_RedMushroom_Name, "Cogumelo Vermelho"},
                {StringKey.Item_SlimMushroom_Name, "Cogumelo Fino"},
                {StringKey.Item_TreeMushroom_Name, "Cogumelo Árvore"},
                {StringKey.Item_AlgaeMushroom_Name, "Cogumelo Alga"},
                {StringKey.Item_BasicMushroom_Name, "Cogumelo Básico"},
                {StringKey.Item_GnomeMushroom_Name, "Cogumelo Gnomo"},
                {StringKey.Item_HairyMushroom_Name, "Cogumelo Peludo"},
                {StringKey.Item_ShellMushroom_Name, "Cogumelo Concha"},
                {StringKey.Item_DragonMushroom_Name, "Cogumelo Dragão"},
                {StringKey.Item_PurpleMushroom_Name, "Cogumelo Roxo"},

                {StringKey.Item_Berries_Name, "Frutas"},
                {StringKey.Item_CoalOre_Name, "Carvão"},
                {StringKey.Item_Fish_Name, "Peixe"},
                {StringKey.Item_GoldOre_Name, "Minério de Ouro"},
                {StringKey.Item_IronOre_Name, "Minério de Ferro"},
                {StringKey.Item_Meat_Name, "Carne"},
                {StringKey.Item_WoodLog_Name, "Madeira"},

                {StringKey.Item_GoldIngot_Name, "Ouro"},
                {StringKey.Item_IronIngot_Name, "Ferro"},
                {StringKey.Item_CoalProcessed_Name, "Carvão Processado"},
                {StringKey.Item_WoodPlank_Name, "Tábua de Madeira"},

                {StringKey.Item_Coin_Name, "Moeda"},
                {StringKey.Item_Gem_Name, "Gema"},

                {StringKey.HUD_LootFeedback, "Você ganhou $AMOUNT$x $ITEM$!"},

                {StringKey.UI_DailyQuestDescription, "Vende o número de itens mostrados"},
                {StringKey.UI_DailyQuestReward, "Mais Dano"},

                {StringKey.Buff_Afraid_Name, "Medo"},
                {StringKey.Buff_Blessing_Name, "Benção"},
                {StringKey.Buff_LuckyCharm_Name, "Amuleto da Sorte"},
                {StringKey.Buff_DeflectingArmor_Name, "Armadura Deflectiva"},
                {StringKey.Buff_IronBoots_Name, "Botas de Ferro"},
                {StringKey.Buff_LifeSteal_Name, "Vampirismo"},
                {StringKey.Buff_Poison_Name, "Veneno"},
                {StringKey.Buff_Spread_Name, "Mais Tiros"},

                {StringKey.Unlock_Buff_Altar_Name, "Altar"},
                {StringKey.Unlock_Buff_Afraid_Name, "Medo"},
                {StringKey.Unlock_Buff_Blessing_Name, "Benção"},
                {StringKey.Unlock_Buff_LuckyCharm_Name, "Amuleto da Sorte"},
                {StringKey.Unlock_Buff_DeflectingArmor_Name, "Armadura Deflectiva"},
                {StringKey.Unlock_Buff_IronBoots_Name, "Botas de Ferro"},
                {StringKey.Unlock_Buff_LifeSteal_Name, "Vampirismo"},
                {StringKey.Unlock_Buff_Poison_Name, "Veneno"},
                {StringKey.Unlock_Buff_Spread_Name, "Mais Tiros"},

                {StringKey.Unlock_Item_RedMushroom_Name, "Cogumelo Vermelho"},
                {StringKey.Unlock_Item_SlimMushroom_Name, "Cogumelo Fino"},
                {StringKey.Unlock_Item_TreeMushroom_Name, "Cogumelo Árvore"},
                {StringKey.Unlock_Item_AlgaeMushroom_Name, "Cogumelo Alga"},
                {StringKey.Unlock_Item_GnomeMushroom_Name, "Cogumelo Gnomo"},
                {StringKey.Unlock_Item_HairyMushroom_Name, "Cogumelo Peludo"},
                {StringKey.Unlock_Item_ShellMushroom_Name, "Cogumelo Concha"},
                {StringKey.Unlock_Item_DragonMushroom_Name, "Cogumelo Dragão"},
                {StringKey.Unlock_Item_PurpleMushroom_Name, "Cogumelo Roxo"},

                {StringKey.Unlock_Item_FoodMarket_Name, "Mercado de Alimentos"},
                {StringKey.Unlock_Item_Groceries_Name, "Comida"},
                {StringKey.Unlock_Item_Jeweler_Name, "Joalheiro"},

                {StringKey.Unlock_Stats_Cooking_Name, "Cozinhar"},
                {StringKey.Unlock_Stats_DancingLessons_Name, "Lições de Dança"},
                {StringKey.Unlock_Stats_TrainingDummy_Name, "Boneco de Treino"},

                {StringKey.Unlock_Class_Rogue_Name, "Assassino"},
                {StringKey.Unlock_Class_Warrior_Name, "Guerreiro"},
            }
        }
    };



    public static void SetLanguage(Language language)
    {
        currentLanguage = language;
    }

    public static string Get(StringKey key)
    {
        return Get(key, currentLanguage);
    }
    public static string Get(StringKey key, Language language)
    {
        if (languageStringDictionary.ContainsKey(language))
        {
            if (languageStringDictionary[language].ContainsKey(key))
            {
                return languageStringDictionary[language][key];
            }
            else
            {
                Debug.LogError(string.Format("Localisation: Key {0} not found in language {1}", key, language));
            }
        }
        else
        {
            Debug.LogError("Language not found");
        }
        return "";
    }
}

public enum Language
{
    English,
    Portuguese
}

public enum StringKey
{
    // Unlocks
    Unlock_Buff_Afraid_Name,
    Unlock_Buff_Afraid_Description,
    Unlock_Buff_Blessing_Name,
    Unlock_Buff_Blessing_Description,

    Unlock_Buff_DeflectingArmor_Name,
    Unlock_Buff_DeflectingArmor_Description,
    Unlock_Buff_IronBoots_Name,
    Unlock_Buff_IronBoots_Description,
    Unlock_Buff_LifeSteal_Name,
    Unlock_Buff_LifeSteal_Description,
    Unlock_Buff_Poison_Name,
    Unlock_Buff_Poison_Description,
    Unlock_Buff_Spread_Name,
    Unlock_Buff_Spread_Description,

    Unlock_Class_Rogue_Name,
    Unlock_Class_Rogue_Description,
    Unlock_Class_Warrior_Name,
    Unlock_Class_Warrior_Description,

    Unlock_Item_AlgaeMushroom_Name,
    Unlock_Item_AlgaeMushroom_Description,
    Unlock_Item_DragonMushroom_Name,
    Unlock_Item_DragonMushroom_Description,
    Unlock_Item_GnomeMushroom_Name,
    Unlock_Item_GnomeMushroom_Description,
    Unlock_Item_HairyMushroom_Name,
    Unlock_Item_HairyMushroom_Description,
    Unlock_Item_PurpleMushroom_Name,
    Unlock_Item_PurpleMushroom_Description,
    Unlock_Item_RedMushroom_Name,
    Unlock_Item_RedMushroom_Description,
    Unlock_Item_ShellMushroom_Name,
    Unlock_Item_ShellMushroom_Description,
    Unlock_Item_SlimMushroom_Name,
    Unlock_Item_SlimMushroom_Description,
    Unlock_Item_TreeMushroom_Name,
    Unlock_Item_TreeMushroom_Description,

    Unlock_Shop_IronWorkshop_Name,
    Unlock_Shop_IronWorkshop_Description,
    Unlock_Shop_WoodWorkshop_Name,
    Unlock_Shop_WoodWorkshop_Description,

    Unlock_Stats_TrainingDummy_Name,
    Unlock_Stats_TrainingDummy_Description,
    Unlock_Stats_Cooking_Name,
    Unlock_Stats_Cooking_Description,
    Unlock_Stats_DancingLessons_Name,
    Unlock_Stats_DancingLessons_Description,

    // Buffs
    Buff_Afraid_Name,
    Buff_Afraid_Description,
    Buff_Blessing_Name,
    Buff_Blessing_Description,

    Buff_DeflectingArmor_Name,
    Buff_DeflectingArmor_Description,
    Buff_IronBoots_Name,
    Buff_IronBoots_Description,
    Buff_LifeSteal_Name,
    Buff_LifeSteal_Description,
    Buff_Poison_Name,
    Buff_Poison_Description,
    Buff_Spread_Name,
    Buff_Spread_Description,

    // Mushrooms
    Item_AlgaeMushroom_Name,
    Item_BasicMushroom_Name,
    Item_DragonMushroom_Name,
    Item_GnomeMushroom_Name,
    Item_HairyMushroom_Name,
    Item_PurpleMushroom_Name,
    Item_RedMushroom_Name,
    Item_ShellMushroom_Name,
    Item_SlimMushroom_Name,
    Item_TreeMushroom_Name,

    // Lootables
    Item_Berries_Name,
    Item_CoalOre_Name,
    Item_Fish_Name,
    Item_GoldOre_Name,
    Item_IronOre_Name,
    Item_Meat_Name,
    Item_WoodLog_Name,

    // Processed
    Item_GoldIngot_Name,
    Item_IronIngot_Name,
    Item_CoalProcessed_Name,
    Item_WoodPlank_Name,

    // Valuables
    Item_Coin_Name,
    Item_Gem_Name,

    // Classes
    Class_Ranger_Name,
    Class_Rogue_Name,
    Class_Warrior_Name,

    HUD_LootFeedback,

    UI_DailyQuestDescription,
    UI_DailyQuestReward,
    Buff_LuckyCharm_Name,
    Buff_LuckyCharm_Description,
    Unlock_Buff_LuckyCharm_Name,
    Unlock_Buff_LuckyCharm_Description,
    Unlock_Buff_Altar_Name,
    Unlock_Buff_Altar_Description,
    Unlock_Item_FoodMarket_Name,
    Unlock_Item_FoodMarket_Description,
    Unlock_Item_Groceries_Name,
    Unlock_Item_Groceries_Description,
    Unlock_Item_Jeweler_Name,
    Unlock_Item_Jeweler_Description,
}
