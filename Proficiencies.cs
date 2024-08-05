namespace Proficiencies
{
    using System.Collections.Generic;
    using UnityEngine;
    using BepInEx;
    using HarmonyLib;
    using System;
    using System.Linq;

    [BepInPlugin(GUID, NAME, VERSION)]
    public class Proficiencies : BaseUnityPlugin
    {
        public const string GUID = "com.ehaugw.proficiencies";
        public const string VERSION = "1.0.0";
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