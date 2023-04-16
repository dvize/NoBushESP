using System;
using System.Collections.Generic;
using System.Reflection;
using Aki.Reflection.Patching;
using EFT;
using UnityEngine;

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

        protected override MethodBase GetTargetMethod()
        {
            try
            {
                return typeof(BotGroupClass).GetMethod("CalcGoalForBot");
            }
            catch
            {
                Logger.LogInfo("NoBushESP: Failed to get target method.. target dead or unspawned.");
            }

            return null;
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
                            var ObjectName = hitInfo.transform.parent?.gameObject?.name;
                            //Logger.LogInfo("Object Name: " + ObjectName);
                            //Logger.LogInfo("Object Layer: " + hitInfo.transform.parent?.gameObject?.layer);

                            foreach (string exclusion in ExclusionList.exclusionList)
                            {
                                if ((bool)(ObjectName.ToLower().Contains(exclusion)))
                                {
                                    //Logger.LogDebug("NoBushESP: Blocking Excluded Object Name: " + hitInfo.collider.transform.parent?.gameObject?.name);

                                    if (NoBushESPPlugin.BlockingTypeGoalEnemy.Value == true)
                                    {
                                        bot.Memory.GetType().GetProperty("GoalEnemy").SetValue(bot.Memory, null);
                                        //Logger.LogInfo($"NoBushESP: Blocking GoalEnemy for: {bot.Profile.Info.Settings.Role} at {ObjectName}");

                                        bot.AimingData.LoseTarget();
                                        //Logger.LogDebug("NoBushESP: LoseTarget() AimingData for: " + bot.Profile.Info.Settings.Role);
                                    }
                                    else
                                    {
                                        goalEnemy.GetType().GetProperty("IsVisible").SetValue(goalEnemy, false);
                                        //Logger.LogInfo($"NoBushESP: Setting IsVisible to false for: {bot.Profile.Info.Settings.Role} at {ObjectName}");
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
    }


}