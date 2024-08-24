namespace Proficiencies
{
    using System.Collections.Generic;
    using BepInEx;
    using HarmonyLib;
    using System.Linq;
    using System.Reflection;

    [BepInPlugin(GUID, NAME, VERSION)]
    public class Proficiencies : BaseUnityPlugin
    {
        public const string GUID = "com.ehaugw.proficiencies";
        public const string VERSION = "1.0.1";
        public const string NAME = "Proficiencies";

        public static List<IWeaponProficiencyOnCharacter> IWeaponProficiencyOnCharacterSources = new List<IWeaponProficiencyOnCharacter>();
        public static List<IWeaponProficiencyFromItem> IWeaponProfiencyFromItemSources = new List<IWeaponProficiencyFromItem>();

        internal void Awake()
        {
            var harmony = new Harmony(GUID);
            harmony.PatchAll();

        }

        [HarmonyPatch(typeof(Item), "Description", MethodType.Getter)]
        //[HarmonyAfter(new string[] {"com.sinai.sideloader"})]
        public class Item_Description
        {
            [HarmonyPostfix]
            public static void Postfix(Item __instance, ref string __result)
            {
                var weaponProficiency = __instance.GetWeaponProficiency();
                if (weaponProficiency != 0)
                {
                    __result = "Weapon Proficiency: " + weaponProficiency + "\n" + __result;
                }
            }
        }

        //[HarmonyPatch(typeof(ItemDetailsDisplay), "ShowDetails")]
        ////[HarmonyAfter(new string[] {"com.sinai.sideloader"})]
        //public class ItemDetailsDisplay_ShowDetails
        //{
        //    [HarmonyPostfix]
        //    public static void Postfix(ItemDetailsDisplay __instance, ref int detailCount, Equipment ___cachedEquipment)
        //    {
        //        if (___cachedEquipment != null)
        //        {
        //            var weaponProficiency = ___cachedEquipment.GetWeaponProficiency();
                    
        //            if (weaponProficiency > 0)
        //            {
        //                MethodInfo privMethod = __instance.GetType().GetMethod("GetRow", BindingFlags.NonPublic | BindingFlags.Instance);
        //                ItemDetailRowDisplay row = privMethod.Invoke(__instance, new object[] { detailCount }) as ItemDetailRowDisplay;
                        
        //                //__instance.GetRow(detailCount).SetInfo(LocalizationManager.Instance.GetLoc("ItemStat_DamageModifier"), this.damageList, false, true);
        //                row.SetInfo("Weapon Proficiency", weaponProficiency);
        //            }
        //        }
        //    }
        //}
    }
    public static class ProficiencyExtensions
    {
        public static float GetWeaponProficiency(this Item item)
        {
            var sum = Proficiencies.IWeaponProfiencyFromItemSources.Select(modifier => modifier.Apply(item)).Sum();
            return sum;
        }

        public static float GetTotalWeaponProficiency(this Character character)
        {
            float original = new EquipmentSlot.EquipmentSlotIDs[] {
                EquipmentSlot.EquipmentSlotIDs.Helmet,
                EquipmentSlot.EquipmentSlotIDs.Foot,
                EquipmentSlot.EquipmentSlotIDs.Chest,
                EquipmentSlot.EquipmentSlotIDs.Back,
                EquipmentSlot.EquipmentSlotIDs.LeftHand,
                EquipmentSlot.EquipmentSlotIDs.RightHand,
            }.Select(slot => character?.Inventory?.GetEquippedItem(slot)?.GetWeaponProficiency() ?? 0).Sum();
            float result = original;
            foreach (var weapon in Proficiencies.IWeaponProficiencyOnCharacterSources)
            {
                weapon.Apply(character, original, ref result);
            }
            return result;
        }
    }
}