//-------------------------------------------------------------------------------------------------------------------------
// ________  ___       _______   ________   ________  _______   ________          ___       ___  ________   ___  __       
//|\   __  \|\  \     |\  ___ \ |\   ___  \|\   ___ \|\  ___ \ |\   __  \        |\  \     |\  \|\   ___  \|\  \|\  \     
//\ \  \|\ /\ \  \    \ \   __/|\ \  \\ \  \ \  \_|\ \ \   __/|\ \  \|\  \       \ \  \    \ \  \ \  \\ \  \ \  \/  /|_   
// \ \   __  \ \  \    \ \  \_|/_\ \  \\ \  \ \  \ \\ \ \  \_|/_\ \   _  _\       \ \  \    \ \  \ \  \\ \  \ \   ___  \  
//  \ \  \|\  \ \  \____\ \  \_|\ \ \  \\ \  \ \  \_\\ \ \  \_|\ \ \  \\  \|       \ \  \____\ \  \ \  \\ \  \ \  \\ \  \ 
//   \ \_______\ \_______\ \_______\ \__\\ \__\ \_______\ \_______\ \__\\ _\        \ \_______\ \__\ \__\\ \__\ \__\\ \__\
//    \|_______|\|_______|\|_______|\|__| \|__|\|_______|\|_______|\|__|\|__|        \|_______|\|__|\|__| \|__|\|__| \|__|
//                                                                                                                        
//-------------------------------------------------------------------------------------------------------------------------
//                                                    writen by Nori_SC
//                                                https://github.com/NoriteSC

using FlaxEditor.Content;
using FlaxEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static FlaxEditor.GUI.ItemsListContextMenu;

namespace BlenderLink
{
    /// <summary>
    /// </summary>
    public class BlenderInstance
    {
        CreateProcessSettings procSettings;
        string BlenderPath;

        /// <summary>
        /// The item
        /// </summary>
        public ContentItem Item;

        /// <summary>
        /// The execution complited
        /// </summary>
        public Action<BlenderInstance> ExecutionComplited;

        /// <summary>
        /// The script mode
        /// </summary>
        public bool ScriptMode;

        /// <summary>
        /// The path to blender python script
        /// </summary>
        public string PathToBlenderPythonScript = "";

        /// <summary>
        /// The python script arguments
        /// </summary>
        public string PythonScriptArgs = "";

        /// <summary>
        /// </summary>
        /// <param name="item"></param>
        /// <param name="BlenderPath"></param>
        public BlenderInstance(ContentItem item, string BlenderPath)
        {
            BlenderPath = Cheak(item, BlenderPath);
            Item = item;
            this.BlenderPath = BlenderPath;
            Debug.Log(item.NamePath);
        }
        /// <summary>
        /// </summary>
        /// <returns>--cmd Path </returns>
        private string MakeArgPath(string cmd,string Path)
        {
            return " --" + cmd + " \"" + Path + "\"";
        }
        private string MakeArg(string cmd, string arg)
        {
            return " --" + cmd +" "+ arg;
        }
        /// <inheritdoc/>
        public void Run()
        {
            //https://docs.blender.org/manual/en/latest/advanced/command_line/arguments.html

            string Args = null;
            if (ScriptMode)
            {
                Args = "--background " + "\"" + Item.Path + "\"" + " --python \"" + PathToBlenderPythonScript + "\" --  " +//<space><space> end of blender args see https://blender.stackexchange.com/questions/6817/how-to-pass-command-line-arguments-to-a-blender-python-script
                    MakeArgPath("ProjectFolder", Globals.ProjectFolder) +
                    MakeArgPath("ContentItem", Item.NamePath) +
                    MakeArg("CustomArgs", PythonScriptArgs);
            }
            else
            {
                Args = "\"" + Item.Path + "\"";
            }

            BlenderPath = "\"" + BlenderPath + "\"";

            procSettings = new()
            {
                Arguments = Args,
                FileName = BlenderPath,
                HiddenWindow = false,
                WaitForEnd = false,
                LogOutput = ScriptMode,
                SaveOutput = false,
                ShellExecute = false
            };
            Task t = Task.Run(BlenderInstanceTask);
        }
        /// <inheritdoc/>
        public string Cheak(ContentItem item, string blenderPath)
        {
            var bp = BlenderLinkOptions.Options.PathToBlender;
            if (blenderPath == null || blenderPath == "")
            {
                blenderPath = BlenderLinkOptions.FindBlenderExecutable(item.Path);
                if (blenderPath == null)
                {
                    BlenderLinkOptions.ValidatePath();
                }
                else
                {
                    BlenderLinkOptions.Options.PathToBlender = blenderPath;
                }
            }
            return BlenderLinkOptions.Options.PathToBlender;
        }
        Task BlenderInstanceTask()
        {
            int i = Platform.CreateProcess(ref procSettings);
            switch (i)
            {
                case 0:
                    Scripting.RunOnUpdate(() => { ExecutionComplited?.Invoke(this); });
                    break;
                default:
                    Platform.Error("Blender Instance has ben closed (with code " + i.ToString() + ")");
                    break;
            }

            return Task.CompletedTask;
        }
    }
}
