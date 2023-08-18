using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace SuperUnityBuild.BuildTool
{
    //Code by Mate
    public class AutoUpload : BuildAction, IPostBuildPerPlatformAction
    {
        [BuildTool.FilePath(false, true, "Select program/script to run.")]
        public string scriptPath = "";

        public override void PerBuildExecute(BuildReleaseType releaseType, BuildPlatform platform, BuildArchitecture architecture, BuildScriptingBackend scriptingBackend, BuildDistribution distribution, DateTime buildTime, ref BuildOptions options, string configKey, string buildPath)
        {
            string resolvedScriptPath = BuildProject.ResolvePath(scriptPath, releaseType, platform, architecture, scriptingBackend, distribution, buildTime);
            string pathArg = BuildProject.ResolvePath(buildPath, releaseType, platform, architecture, scriptingBackend, distribution, buildTime);
            string platformArg = platform.platformName;
            string arguments = $"{pathArg} {platformArg}";

            RunScript(resolvedScriptPath, arguments);
        }
        private void RunScript(string scriptPath, string arguments)
        {
            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.FileName = Path.GetFullPath(scriptPath);

            if (!string.IsNullOrEmpty(arguments))
                startInfo.Arguments = arguments;

            Process proc = Process.Start(startInfo);
            proc.WaitForExit();
        }
    }
}
 
