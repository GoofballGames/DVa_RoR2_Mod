using System;
using BepInEx;
using EntityStates;
using R2API;
using R2API.AssetPlus;
using R2API.Utils;
using RoR2;
using RoR2.Skills;
using UnityEngine;

namespace GoofballGames
{
    [BepInDependency("com.bepis.r2api")]
    [BepInPlugin("com.GoofballGames.DVa", "D.Va", "0.1.1")]
    [R2APISubmoduleDependency(nameof(LoadoutAPI), nameof(SurvivorAPI), nameof(AssetPlus))]
    public class DVa : BaseUnityPlugin
    {
        public void Awake()
        {
            //Until I figure out how to make an assetbundle for the characters. #HELP
            var myChar = Resources.Load<GameObject>("prefabs/characterbodies/CommandoBody");

            Languages.AddToken("DVA_DESCRIPTION", "짜증나!" + Environment.NewLine);

            var mySurvivorDef = new SurvivorDef
            {
                bodyPrefab = myChar,
                descriptionToken = "DVA_DESCRIPTION",
                displayPrefab = Resources.Load<GameObject>("Prefabs/Characters/CommandoDisplay"),
                primaryColor = new Color(0.98046875f, 0.12890625f, 0.9765625f),
                unlockableName = "",
            };
            SurvivorAPI.AddSurvivor(mySurvivorDef);

            //TOKKI PRIMARY: FUSION CANNONS
            #region
            Languages.AddToken("TOKKI_PRIMARY_FUSIONCANNONS_NAME", "Fusion Cannons");
            Languages.AddToken("TOKKI_PRIMARY_FUSIONCANNONS_DESCRIPTION", "D.Va's mech is equipped with twin short-range rotating cannons. They lay down continuous, high-damage fire without needing to reload, but slow D.Va’s movement while they’re active.");

            var mySkillDef = ScriptableObject.CreateInstance<SkillDef>();
            mySkillDef.activationState = new SerializableEntityStateType(typeof(GoofballGames.TokkiEntityStates.Tokki_FusionCannons));
            mySkillDef.activationStateMachineName = "Weapon";
            mySkillDef.baseMaxStock = 0;
            mySkillDef.baseRechargeInterval = 0f;
            mySkillDef.beginSkillCooldownOnSkillEnd = true;
            mySkillDef.canceledFromSprinting = true;
            mySkillDef.fullRestockOnAssign = true;
            mySkillDef.interruptPriority = InterruptPriority.Any;
            mySkillDef.isBullets = true;
            mySkillDef.isCombatSkill = true;
            mySkillDef.mustKeyPress = false;
            mySkillDef.noSprint = true;
            mySkillDef.rechargeStock = 0;
            mySkillDef.requiredStock = 0;
            mySkillDef.shootDelay = 0.15f;
            mySkillDef.stockToConsume = 0;
            //mySkillDef.icon = Resources.Load<Sprite>("NotAnActualPath");
            mySkillDef.skillDescriptionToken = "TOKKI_PRIMARY_FUSIONCANNONS_DESCRIPTION";
            mySkillDef.skillName = "TOKKI_PRIMARY_FUSIONCANNONS_NAME";
            mySkillDef.skillNameToken = "TOKKI_PRIMARY_FUSIONCANNONS_NAME";

            var skillLocator = myChar.GetComponent<SkillLocator>();

            //Note; if your character does not originally have a skill family for this, use the following:
            //skillLocator.special = gameObject.AddComponent<GenericSkill>();
            //var newFamily = ScriptableObject.CreateInstance<SkillFamily>();
            //LoadoutAPI.AddSkillFamily(newFamily);
            //skillLocator.special.SetFieldValue("_skillFamily", newFamily);
            //var specialSkillFamily = skillLocator.special.skillFamily;


            //Note; you can change component.primary to component.secondary , component.utility and component.special
            var skillFamily = skillLocator.primary.skillFamily;

            //If this is an alternate skill, use this code.
            // Here, we add our skill as a variant to the exisiting Skill Family.
            Array.Resize(ref skillFamily.variants, skillFamily.variants.Length + 1);
            skillFamily.variants[skillFamily.variants.Length - 1] = new SkillFamily.Variant
            {
                skillDef = mySkillDef,
                unlockableName = "",
                viewableNode = new ViewablesCatalog.Node(mySkillDef.skillNameToken, false, null)

            };

            //Note; if your character does not originally have a skill family for this, use the following:
            //skillFamily.variants = new SkillFamily.Variant[1]; // substitute 1 for the number of skill variants you are implementing

            //If this is the default/first skill, copy this code and remove the //,
            //skillFamily.variants[0] = new SkillFamily.Variant
            //{
            //    skillDef = mySkillDef,
            //    unlockableName = "",
            //    viewableNode = new ViewablesCatalog.Node(mySkillDef.skillNameToken, false, null)
            //};
            #endregion


        }


    }
}