﻿#if SCRIPTING_DEFINE_UTILITY
using TNRD.Utilities;
using UnityEditor;

namespace AddressableAssets.Editor
{
    public static class AddressablesFullLogs
    {
        private const string AddressablesFullLogsDefine = "ADDRESSABLES_LOG_ALL";

        [MenuItem("Window/Asset Management/Addressables/Logs/Enable addressables full logs")]
        public static void EnableAddressablesFullLogs()
        {
            ScriptingDefineUtility.Add(AddressablesFullLogsDefine);
        }

        [MenuItem("Window/Asset Management/Addressables/Logs/Disable addressables full logs")]
        public static void DisableAddressablesFullLogs()
        {
            ScriptingDefineUtility.Remove(AddressablesFullLogsDefine);
        }
    }
}
#endif