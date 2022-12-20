using Aki.Reflection.Patching;
using EFT;
using System;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace NoBushESP
{

    public class AIESPPatcherAimPlayer : ModulePatch
    {

        public static WildSpawnType[] bossesList = { WildSpawnType.bossBully, WildSpawnType.bossKilla, WildSpawnType.bossKojaniy, WildSpawnType.bossGluhar, WildSpawnType.bossSanitar, WildSpawnType.bossTagilla,
                WildSpawnType.bossKnight, WildSpawnType.sectantWarrior, WildSpawnType.sectantPriest };

        public static WildSpawnType[] followersList = { WildSpawnType.followerBully, WildSpawnType.followerKojaniy, WildSpawnType.followerGluharAssault, WildSpawnType.followerGluharSecurity, WildSpawnType.followerGluharScout,
                WildSpawnType.followerGluharSnipe, WildSpawnType.followerSanitar, WildSpawnType.followerTagilla, WildSpawnType.followerBigPipe, WildSpawnType.followerBirdEye };

        public static WildSpawnType[] pmcList = { WildSpawnType.pmcBot, WildSpawnType.exUsec, WildSpawnType.gifter, (WildSpawnType)AIPatcherPlugin.sptUsec, (WildSpawnType)AIPatcherPlugin.sptBear };

        public static WildSpawnType[] scavList = { WildSpawnType.assault, WildSpawnType.marksman, WildSpawnType.cursedAssault, WildSpawnType.assaultGroup };
        
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
                    if (scavList.Contains(bot.Profile.Info.Settings.Role) && AIPatcherPlugin.config.ScavsStillSee)
                    {
                        return;
                    }

                    if (bossesList.Contains(bot.Profile.Info.Settings.Role) && AIPatcherPlugin.config.BossesStillSee)
                    {
                        return;
                    }

                    if (followersList.Contains(bot.Profile.Info.Settings.Role) && AIPatcherPlugin.config.BossFollowersStillSee)
                    {
                        return;
                    }

                    if (pmcList.Contains(bot.Profile.Info.Settings.Role) && AIPatcherPlugin.config.PMCsStillSee)
                    {
                        return;
                    }

                    RaycastHit hitInfo;
                    float radius = 1.0f;  //need to tweak this for width
                    float maxdistance = 137f; // what is bots vision range.  137f
                    LayerMask layermask = 1 << 0 | 1 << 11 | 1 << 26;


                    //bool isVisible = (bool)goalEnemy.GetType().GetProperty("IsVisible").GetValue(goalEnemy);
                    
                    if (Physics.SphereCast(bot.Position, radius, person.GetPlayer.Position, out hitInfo, maxdistance, layermask))
                    {
                        //Logger.LogInfo("Object Name: " + hitInfo.collider.gameObject.name);
                        //Logger.LogInfo("Object Layer: " + hitInfo.collider.gameObject.layer);

                        if ((hitInfo.collider.transform.parent?.gameObject?.name?.Contains("filbert") == true) || (hitInfo.collider.transform.parent?.gameObject?.name?.Contains("fibert") == true))
                        {
                            //Logger.LogInfo("filbert or Fibert in the way");

                           // Logger.LogInfo("Setting IsVisible to false for: " + bot.Profile.Info.Settings.Role);
                            goalEnemy.GetType().GetProperty("IsVisible").SetValue(goalEnemy, false);
                            
                            //bot.Memory.GetType().GetProperty("GoalEnemy").SetValue(bot.Memory, null);
                        }

                    }

                }
            }

        }
    }
}