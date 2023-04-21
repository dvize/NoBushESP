using System;
using System.Collections.Generic;
using System.Reflection;
using Aki.Reflection.Patching;
using EFT;
using UnityEngine;
using HarmonyLib;

namespace NoBushESP
{

    public static class ExclusionList
    {
        public static List<string> exclusionList = new List<string> { "filbert", "fibert", "tree", "pine", "plant", "birch", "collider",
        "timber", "spruce", "bush", "metal", "wood"};
    }

    public class BushPatch : ModulePatch
    {
        private static RaycastHit hitInfo;
        private static LayerMask layermask;
        private static BodyPartClass bodyPartClass;
        private static Vector3 vector;
        private static float magnitude;
        private static string ObjectName;
        protected override MethodBase GetTargetMethod()
        {
            return AccessTools.Method(typeof(BotGroupClass),"CalcGoalForBot");
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
                        float radius = 8.0f;

                        if (Physics.Raycast(new Ray(bodyPartClass.Position, vector), out hitInfo, magnitude, layermask))
                        //if (Physics.SphereCast(bodyPartClass.Position, radius, vector.normalized, out hitInfo, magnitude, layermask))
                        {
                            ObjectName = hitInfo.transform.parent?.gameObject?.name;
                            //Logger.LogInfo("Object Name: " + ObjectName);
                            //Logger.LogInfo("Object Layer: " + hitInfo.transform.parent?.gameObject?.layer);

                            foreach (string exclusion in ExclusionList.exclusionList)
                            {
                                if ((bool)(ObjectName.ToLower().Contains(exclusion)))
                                {
                                    //Logger.LogDebug("NoBushESP: Blocking Excluded Object Name: " + hitInfo.collider.transform.parent?.gameObject?.name);

                                    goalEnemy.GetType().GetProperty("IsVisible").SetValue(goalEnemy, false);
                                    //Logger.LogInfo($"NoBushESP: Setting IsVisible to false for: {bot.Profile.Info.Settings.Role} at {ObjectName}");
                                    bot.AimingData.LoseTarget();


                                    //Try to handle bosses this way.
                                    //set canshootbystate to false
                                    //Logger.LogInfo($"NoBushESP: Call EndShoot() for: {bot.Profile.Info.Settings.Role} at {ObjectName}");

                                    bot.ShootData.EndShoot();

                                    // Get the private setter of the CanShootByState property using AccessTools
                                    var setter = AccessTools.PropertySetter(typeof(GClass546), nameof(GClass546.CanShootByState));

                                    // Use reflection to set the value of the property
                                    setter.Invoke(bot.ShootData, new object[] { false });
                                    return;
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
    }
}