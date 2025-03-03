using GameNetcodeStuff;
using pdkmMenu;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;


public class Turret_Label : ObjectLabel<Turret>
{
    protected override string GetEntityLabel() => targetObject.name;
    protected override bool ShouldShow() => Plugin.ESPSettings.ESP.Value &&  Plugin.ESPSettings.Traps.Value;
    protected override Color TextColor => Color.red;

}

public class Landmine_Label : ObjectLabel<Landmine>
{
    protected override string GetEntityLabel() => targetObject.name;
    protected override bool ShouldShow() => Plugin.ESPSettings.ESP.Value && Plugin.ESPSettings.Traps.Value;

    protected override Color TextColor => Color.red;
}
public class PlayerControllerB_Label : ObjectLabel<PlayerControllerB>
{
    protected override string GetEntityLabel() => targetObject.playerUsername;
    protected override bool ShouldShow() => Plugin.ESPSettings.ESP.Value && Plugin.ESPSettings.Player.Value;

    protected override Color TextColor => Color.white;
}
public class Grabbable_Label : ObjectLabel<GrabbableObject>
{
    protected override string GetEntityLabel() => targetObject.name;
    protected override bool ShouldShow() => Plugin.ESPSettings.ESP.Value && Plugin.ESPSettings.Item.Value;

    protected override Color TextColor => Color.green;
}
public class EnemyAI_Label : ObjectLabel<EnemyAI>
{
    protected override string GetEntityLabel() => targetObject.enemyType.name;
    protected override bool ShouldShow() => Plugin.ESPSettings.ESP.Value && Plugin.ESPSettings.Enemy.Value;

    protected override Color TextColor => Color.red;
}
public class Entrace_Label : ObjectLabel<EntranceTeleport>
{
    protected override string GetEntityLabel() => "door";
    protected override bool ShouldShow() => Plugin.ESPSettings.ESP.Value && Plugin.ESPSettings.Doors.Value;

    //protected override Vector3 GetWorldPos() => targetObject.entrancePoint.position;
    protected override Color TextColor => Color.white;
}