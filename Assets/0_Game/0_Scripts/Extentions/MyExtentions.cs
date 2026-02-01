
using UnityEngine;

public static class MyExtentions
{
    public static Vector3 WithY( this Vector3 target, float YValue) {
        target.y = YValue;
        return target;
    }
    public static void WithX(this Vector3 target, float XValue) {
        var pos = target;
        pos.x = XValue;
        target= pos;
    }
    public static void WithZ(this Vector3 target, float ZValue) {
        var pos = target;
        pos.z = ZValue;
        target= pos;
    }
}
