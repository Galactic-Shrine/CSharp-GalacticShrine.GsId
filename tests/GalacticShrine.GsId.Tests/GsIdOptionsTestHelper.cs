using System.Reflection;

namespace GalacticShrine.GsId.Tests;

internal static class GsIdOptionsTestHelper
{
    public static void Reset()
    {
        FieldInfo? LockField = typeof(GsIdOptions).GetField("_IsLocked", BindingFlags.Static | BindingFlags.NonPublic)
            ?? typeof(GsIdOptions).GetField("_isLocked", BindingFlags.Static | BindingFlags.NonPublic);

        LockField?.SetValue(null, false);
        GsIdOptions.Reset();
    }
}
