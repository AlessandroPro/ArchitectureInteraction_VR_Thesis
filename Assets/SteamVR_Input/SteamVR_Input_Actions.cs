//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Valve.VR
{
    using System;
    using UnityEngine;
    
    
    public partial class SteamVR_Input
    {
        
        public static SteamVR_Action_Boolean @__actions_default_in_Teleport;
        
        public static SteamVR_Action_Boolean @__actions_default_in_Grab;
        
        public static SteamVR_Action_Skeleton @__actions_default_in_SkeletonLeftHand;
        
        public static SteamVR_Action_Skeleton @__actions_default_in_SkeletonRightHand;
        
        public static SteamVR_Action_Vector2 @__actions_default_in_PushArrow;
        
        public static SteamVR_Action_Vibration @__actions_default_out_Haptic;
        
        public static void Dynamic_InitializeActions()
        {
            SteamVR_Input.@__actions_default_in_Teleport.Initialize();
            SteamVR_Input.@__actions_default_in_Grab.Initialize();
            SteamVR_Input.@__actions_default_in_SkeletonLeftHand.Initialize();
            SteamVR_Input.@__actions_default_in_SkeletonRightHand.Initialize();
            SteamVR_Input.@__actions_default_in_PushArrow.Initialize();
            SteamVR_Input.@__actions_default_out_Haptic.Initialize();
        }
        
        public static void Dynamic_InitializeInstanceActions()
        {
            Valve.VR.SteamVR_Input.@__actions_default_in_Teleport = ((SteamVR_Action_Boolean)(SteamVR_Input_References.GetAction("__actions_default_in_Teleport")));
            Valve.VR.SteamVR_Input.@__actions_default_in_Grab = ((SteamVR_Action_Boolean)(SteamVR_Input_References.GetAction("__actions_default_in_Grab")));
            Valve.VR.SteamVR_Input.@__actions_default_in_SkeletonLeftHand = ((SteamVR_Action_Skeleton)(SteamVR_Input_References.GetAction("__actions_default_in_SkeletonLeftHand")));
            Valve.VR.SteamVR_Input.@__actions_default_in_SkeletonRightHand = ((SteamVR_Action_Skeleton)(SteamVR_Input_References.GetAction("__actions_default_in_SkeletonRightHand")));
            Valve.VR.SteamVR_Input.@__actions_default_in_PushArrow = ((SteamVR_Action_Vector2)(SteamVR_Input_References.GetAction("__actions_default_in_PushArrow")));
            Valve.VR.SteamVR_Input.@__actions_default_out_Haptic = ((SteamVR_Action_Vibration)(SteamVR_Input_References.GetAction("__actions_default_out_Haptic")));
            Valve.VR.SteamVR_Input.actions = new Valve.VR.SteamVR_Action[] {
                    Valve.VR.SteamVR_Input.@__actions_default_in_Teleport,
                    Valve.VR.SteamVR_Input.@__actions_default_in_Grab,
                    Valve.VR.SteamVR_Input.@__actions_default_in_SkeletonLeftHand,
                    Valve.VR.SteamVR_Input.@__actions_default_in_SkeletonRightHand,
                    Valve.VR.SteamVR_Input.@__actions_default_in_PushArrow,
                    Valve.VR.SteamVR_Input.@__actions_default_out_Haptic};
            Valve.VR.SteamVR_Input.actionsIn = new Valve.VR.SteamVR_Action_In[] {
                    Valve.VR.SteamVR_Input.@__actions_default_in_Teleport,
                    Valve.VR.SteamVR_Input.@__actions_default_in_Grab,
                    Valve.VR.SteamVR_Input.@__actions_default_in_SkeletonLeftHand,
                    Valve.VR.SteamVR_Input.@__actions_default_in_SkeletonRightHand,
                    Valve.VR.SteamVR_Input.@__actions_default_in_PushArrow};
            Valve.VR.SteamVR_Input.actionsOut = new Valve.VR.SteamVR_Action_Out[] {
                    Valve.VR.SteamVR_Input.@__actions_default_out_Haptic};
            Valve.VR.SteamVR_Input.actionsVibration = new Valve.VR.SteamVR_Action_Vibration[] {
                    Valve.VR.SteamVR_Input.@__actions_default_out_Haptic};
            Valve.VR.SteamVR_Input.actionsPose = new Valve.VR.SteamVR_Action_Pose[0];
            Valve.VR.SteamVR_Input.actionsBoolean = new Valve.VR.SteamVR_Action_Boolean[] {
                    Valve.VR.SteamVR_Input.@__actions_default_in_Teleport,
                    Valve.VR.SteamVR_Input.@__actions_default_in_Grab};
            Valve.VR.SteamVR_Input.actionsSingle = new Valve.VR.SteamVR_Action_Single[0];
            Valve.VR.SteamVR_Input.actionsVector2 = new Valve.VR.SteamVR_Action_Vector2[] {
                    Valve.VR.SteamVR_Input.@__actions_default_in_PushArrow};
            Valve.VR.SteamVR_Input.actionsVector3 = new Valve.VR.SteamVR_Action_Vector3[0];
            Valve.VR.SteamVR_Input.actionsSkeleton = new Valve.VR.SteamVR_Action_Skeleton[] {
                    Valve.VR.SteamVR_Input.@__actions_default_in_SkeletonLeftHand,
                    Valve.VR.SteamVR_Input.@__actions_default_in_SkeletonRightHand};
            Valve.VR.SteamVR_Input.actionsNonPoseNonSkeletonIn = new Valve.VR.SteamVR_Action_In[] {
                    Valve.VR.SteamVR_Input.@__actions_default_in_Teleport,
                    Valve.VR.SteamVR_Input.@__actions_default_in_Grab,
                    Valve.VR.SteamVR_Input.@__actions_default_in_PushArrow};
        }
    }
}
