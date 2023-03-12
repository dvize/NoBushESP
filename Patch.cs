using Aki.Reflection.Patching;
using EFT;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace NoBushESP
{

    public class BushPatch : ModulePatch
    {

        public static List<string> exclusionList = new List<string> { "filbert", "fibert", "tree", "pine", "plant", "birch", "collider", 
            "timber", "spruce", "bush", "metal", "wood", "ballistic", "terrain"};
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
                        
                        RaycastHit hitInfo;
                        LayerMask layermask = LayerMaskClass.HighPolyWithTerrainMask;

                        float maxdistance = Vector3.Distance(bot.Position, person.GetPlayer.Position);
                        Vector3 direction = person.GetPlayer.Transform.position - bot.Transform.position;

                        if (Physics.Raycast(bot.Position, direction, out hitInfo, maxdistance, layermask))
                        {
                            //Logger.LogInfo("Object Name: " + hitInfo.transform.parent?.gameObject?.name);
                            //Logger.LogInfo("Object Layer: " + hitInfo.transform.parent?.gameObject?.layer);

                            foreach (string exclusion in exclusionList)
                            {
                                if ((bool)(hitInfo.collider.transform.parent?.gameObject?.name.ToLower().Contains(exclusion)))
                                {
                                    //Logger.LogDebug("NoBushESP: Blocking Excluded Object Name: " + hitInfo.collider.transform.parent?.gameObject?.name);

                                    if (NoBushESPPlugin.BlockingTypeGoalEnemy.Value == true)
                                    {
                                        bot.Memory.GetType().GetProperty("GoalEnemy").SetValue(bot.Memory, null);
                                        //Logger.LogDebug("NoBushESP: Blocking GoalEnemy for: " + bot.Profile.Info.Settings.Role);

                                        bot.AimingData.LoseTarget();
                                        //Logger.LogDebug("NoBushESP: LoseTarget() AimingData for: " + bot.Profile.Info.Settings.Role);
                                    }
                                    else
                                    {
                                        goalEnemy.GetType().GetProperty("IsVisible").SetValue(goalEnemy, false);
                                        //Logger.LogDebug("NoBushESP: Setting IsVisible to false for: " + bot.Profile.Info.Settings.Role);
                                    }


                                }

                            }
                        }


                    }
                }

            }
            catch
            {
                Logger.LogInfo("NoBushESP: Cannot Assign Brain Because Enemy is Dead or Unspawned");
            }

        }
    }




}