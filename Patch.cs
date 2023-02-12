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

        public static WildSpawnType[] bossesList = { WildSpawnType.bossBully, WildSpawnType.bossKilla, WildSpawnType.bossKojaniy, WildSpawnType.bossGluhar, WildSpawnType.bossSanitar, WildSpawnType.bossTagilla,
                WildSpawnType.bossKnight, WildSpawnType.sectantWarrior, WildSpawnType.sectantPriest, WildSpawnType.bossZryachiy };

        public static WildSpawnType[] followersList = { WildSpawnType.followerBully, WildSpawnType.followerKojaniy, WildSpawnType.followerGluharAssault, WildSpawnType.followerGluharSecurity, WildSpawnType.followerGluharScout,
                WildSpawnType.followerGluharSnipe, WildSpawnType.followerSanitar, WildSpawnType.followerTagilla, WildSpawnType.followerBigPipe, WildSpawnType.followerBirdEye, WildSpawnType.followerZryachiy };

        public static WildSpawnType[] pmcList = { WildSpawnType.pmcBot, WildSpawnType.exUsec, WildSpawnType.gifter, NoBushESPPlugin.sptUsec, NoBushESPPlugin.sptBear };

        public static WildSpawnType[] scavList = { WildSpawnType.assault, WildSpawnType.marksman, WildSpawnType.cursedAssault, WildSpawnType.assaultGroup };

        public static List<string> exclusionList = new List<string> { "filbert", "fibert", "tree", "pine", "plant" };
        protected override MethodBase GetTargetMethod()
        {
            return typeof(BotGroupClass).GetMethod("CalcGoalForBot");
        }

        [PatchPostfix]
        public static void PatchPostfix(BotOwner bot)
        {
            //Need when enemy is alerted to player 

            object goalEnemy = bot.Memory.GetType().GetProperty("GoalEnemy").GetValue(bot.Memory);

            if (goalEnemy != null)
            {
                IAIDetails person = (IAIDetails)goalEnemy.GetType().GetProperty("Person").GetValue(goalEnemy);

                if (person.GetPlayer.IsYourPlayer)
                {
                    //skip if config says disabled
                    if (scavList.Contains(bot.Profile.Info.Settings.Role) && NoBushESPPlugin.ScavsStillSee.Value)
                    {
                        return;
                    }

                    if (bossesList.Contains(bot.Profile.Info.Settings.Role) && NoBushESPPlugin.BossesStillSee.Value)
                    {
                        return;
                    }

                    if (followersList.Contains(bot.Profile.Info.Settings.Role) && NoBushESPPlugin.BossFollowersStillSee.Value)
                    {
                        return;
                    }

                    if (pmcList.Contains(bot.Profile.Info.Settings.Role) && NoBushESPPlugin.PMCsStillSee.Value)
                    {
                        return;
                    }

                    RaycastHit hitInfo;
                    LayerMask layermask = 1 << 0 | 1 << 11 | 1 << 26;

                    float maxdistance = Vector3.Distance(bot.Position, person.GetPlayer.Position);

                    if (Physics.SphereCast(bot.Position, NoBushESPPlugin.TestRayRadius.Value, person.GetPlayer.Position, out hitInfo, maxdistance, layermask))
                    {
                        //Logger.LogInfo("Object Name: " + hitInfo.collider.gameObject.name);
                        //Logger.LogInfo("Object Layer: " + hitInfo.collider.gameObject.layer);

                        foreach (string exclusion in exclusionList)
                        {
                            if ((bool)(hitInfo.collider.transform.parent?.gameObject?.name.ToLower().Contains(exclusion)))
                            {
                                //Logger.LogInfo("filbert or Fibert in the way");

                                // Logger.LogInfo("Setting IsVisible to false for: " + bot.Profile.Info.Settings.Role);

                                if (NoBushESPPlugin.BlockingTypeGoalEnemy.Value == true)
                                {
                                    bot.Memory.GetType().GetProperty("GoalEnemy").SetValue(bot.Memory, null);
                                }
                                else
                                {
                                    goalEnemy.GetType().GetProperty("IsVisible").SetValue(goalEnemy, false);
                                }


                            }

                        }
                    }


                }
            }

        }
    }
}