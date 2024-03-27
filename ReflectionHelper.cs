using System;
using HarmonyLib;
using UnityEngine;


namespace dvize.BushNoESP
{
    internal static class ReflectionHelper
    {

        // Method to set property value using reflection
        public static void SetProperty(object target, string propertyName, object value)
        {
            try
            {
                var propInfo = AccessTools.Property(target.GetType(), propertyName);
                propInfo?.SetValue(target, value);
            }
            catch (Exception ex)
            {
                //use unity debug
                Debug.LogError($"Failed to set property {propertyName}: {ex}");
            }
        }

        // Method to invoke a method through reflection
        public static void InvokeMethod(object target, string methodName, object[] parameters = null)
        {
            try
            {
                var method = AccessTools.Method(target.GetType(), methodName);
                method?.Invoke(target, parameters);
            }
            catch (Exception ex)
            {
                Debug.LogError($"Failed to invoke method {methodName}: {ex}");
            }
        }


    }
}
