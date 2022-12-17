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
                    RaycastHit hitInfo;
                    float radius = 1.0f;  //need to tweak this for width
                    float maxdistance = 137f; // what is bots vision range.  137f
                    LayerMask layermask = 1 << 0 | 1 << 11 | 1 << 26;


                    //bool isVisible = (bool)goalEnemy.GetType().GetProperty("IsVisible").GetValue(goalEnemy);

                    if (Physics.SphereCast(bot.gameObject.transform.position, radius, person.GetPlayer.gameObject.transform.forward, out hitInfo, maxdistance, layermask))
                    {
                        //Logger.LogInfo("Object Name: " + hitInfo.collider.gameObject.name);
                        //Logger.LogInfo("Object Layer: " + hitInfo.collider.gameObject.layer);

                        if ((hitInfo.collider.transform.parent?.gameObject?.name?.Contains("filbert") == true) || (hitInfo.collider.transform.parent?.gameObject?.name?.Contains("fibert") == true))
                        {
                            //Logger.LogInfo("filbert or Fibert in the way");
                            bot.Memory.GetType().GetProperty("GoalEnemy").SetValue(bot.Memory, null);
                        }

                    }

                }
            }

        }
    }
}