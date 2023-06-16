using System;
using System.Collections.Generic;
using System.Reflection;
using Aki.Reflection.Patching;
using EFT;
using EFT.Ballistics;
using EFT.Interactive;
using HarmonyLib;
using UnityEngine;

namespace NoBushESP
{
    public class BushPatch : ModulePatch
    {
        private static RaycastHit hitInfo;
        private static LayerMask layermask;
        private static BodyPartClass bodyPartClass;
        private static Vector3 vector;
        private static MaterialType tempMaterial;
        private static float magnitude;
        private static string ObjectName;

        private static readonly List<string> exclusionList = new List<string> { "filbert", "fibert", "tree", "pine", "plant", "birch", "collider",
        "timber", "spruce", "bush", "metal", "wood"};
    
        private static readonly List<MaterialType> extraMaterialList = new List<MaterialType> { MaterialType.Glass, MaterialType.GlassVisor, MaterialType.GlassShattered, MaterialType.Concrete, MaterialType.GrassHigh};
        protected override MethodBase GetTargetMethod()
        {
            return AccessTools.Method(typeof(BotGroupClass), "CalcGoalForBot");
        }

        [PatchPostfix]

        public static void PatchPostfix(BotOwner bot)
        {
            try
            {
                //Need when enemy is alerted to player 

                object goalEnemy = bot.Memory.GetType().GetProperty("GoalEnemy").GetValue(bot.Memory);

                if (goalEnemy != null)
                {
                    IAIDetails person = (IAIDetails)goalEnemy.GetType().GetProperty("Person").GetValue(goalEnemy);

                    if (person.GetPlayer.IsYourPlayer)
                    {
                        layermask = LayerMaskClass.HighPolyWithTerrainMaskAI;
                        bodyPartClass = bot.MainParts[BodyPartType.head];
                        vector = person.MainParts[BodyPartType.head].Position - bodyPartClass.Position;
                        magnitude = vector.magnitude;

                        if (Physics.Raycast(new Ray(bodyPartClass.Position, vector), out hitInfo, magnitude, layermask))
                        {
                            ObjectName = hitInfo.transform.parent?.gameObject?.name;

                            foreach (string exclusion in exclusionList)
                            {
                                if ((bool)(ObjectName.ToLower().Contains(exclusion)))
                                {
                                    blockShooting(bot, goalEnemy);
                                    return;
                                }

                            }

                            tempMaterial = hitInfo.transform.gameObject.GetComponentInParent<BallisticCollider>().TypeOfMaterial;
                            //look for component in parent for BallisticsCollider and then check material type
                            if (Vector3.Distance(hitInfo.transform.position, bodyPartClass.Position) > 50)
                            {
                                foreach(MaterialType material in extraMaterialList)
                                {
                                    if (tempMaterial == material)
                                    {
                                        blockShooting(bot, goalEnemy);
                                        return;
                                    }

                                }
                            }
                        }



                    }
                }

            }
            catch
            {
                //Logger.LogInfo("NoBushESP: Failed Post Patch");
            }

        }


        private static void blockShooting(BotOwner bot, object goalEnemy)
        {
            goalEnemy.GetType().GetProperty("IsVisible").SetValue(goalEnemy, false);

            bot.AimingData.LoseTarget();
            bot.ShootData.EndShoot();

            // Get the private setter of the CanShootByState property using AccessTools
            var setter = AccessTools.PropertySetter(typeof(GClass546), nameof(GClass546.CanShootByState));

            // Use reflection to set the value of the property
            setter.Invoke(bot.ShootData, new object[] { false });

        }

    }
}