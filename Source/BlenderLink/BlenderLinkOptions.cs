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

using FlaxEditor;
using FlaxEditor.Options;
using FlaxEngine;
using FlaxEngine.GUI;
using FlaxEngine.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;

namespace BlenderLink
{
    /// <summary>
    /// </summary>
    [CustomEditor(typeof(Editor<BlenderLinkOptions>))]
    public sealed class BlenderLinkOptions
    {
        /// <inheritdoc/>
        [NoSerialize] 
        public static BlenderLinkOptions Options = new BlenderLinkOptions();
        [NoSerialize]
        private static string m_PathToBlender;
        [NoSerialize]
        private static bool m_isNew;
        [NoSerialize]
        private static Window w;
        /// <summary>
        /// The path to blender
        /// </summary>
        [Serialize]
        public string PathToBlender 
        {
            get => m_PathToBlender;
            set { 
                m_PathToBlender = value;
                Editor.Instance.Options.Options.CustomSettings["Blender Link"] = JsonSerializer.Serialize(Options, typeof(object));
                Editor.Instance.Options.Apply(Editor.Instance.Options.Options);
            }
        }
        /// <summary>
        /// </summary>
        [HideInEditor, Serialize]
        public bool isNew { get => m_isNew; set => m_isNew = value; }

        internal static string FindBlenderExecutable(string path)
        {
            string outvall = new string(' ', 1024);
            unsafe
            {
#if PLATFORM_WINDOWS
                IntPtr pathPtr = Marshal.StringToHGlobalAnsi(path);
                IntPtr stringEmptyPtr = Marshal.StringToHGlobalAnsi(string.Empty);
                IntPtr executablePtr = Marshal.StringToHGlobalAnsi(outvall);
                // grab shit from C++ windows API;
                var errorcode = FindExecutable(pathPtr, stringEmptyPtr, executablePtr);
                switch (errorcode)
                {
                    case 2:
                        Debug.LogError("[BlenderLink.FindBlenderExecutable] The specified file was not found."); return null;
                    case 3:
                        Debug.LogError("[BlenderLink.FindBlenderExecutable] The specified path is invalid."); return null;
                    case 5:
                        Debug.LogError("[BlenderLink.FindBlenderExecutable] The specified file cannot be accessed."); return null;
                    case 8:
                        Debug.LogError("[BlenderLink.FindBlenderExecutable] The system is out of memory or resources."); return null;
                    case 31:
                        Debug.LogError("[BlenderLink.FindBlenderExecutable] There is no association for the specified file type with an executable file."); return null;
                }
                outvall = Marshal.PtrToStringAnsi(executablePtr);
                if (outvall == null)
                    return null;
                Marshal.FreeHGlobal(pathPtr);
                Marshal.FreeHGlobal(stringEmptyPtr);
                Marshal.FreeHGlobal(executablePtr);
                outvall = outvall.Substring(0, outvall.Length - ("-launcher.exe").Length);
                outvall += ".exe";
#else
//unsupported for now
outvall == null;
#endif
            }
            return outvall;
        }
        internal static void Load()
        {
            if (Editor.Instance.Options.Options.CustomSettings.TryGetValue("Blender Link", out string v))
            {
                Options = JsonSerializer.Deserialize<BlenderLinkOptions>(v);
            }
            if (Options.PathToBlender == null || Options.PathToBlender == "")
            {
                Options.isNew = false;
                var asset = Content.Load<Texture>(Path.Combine(Globals.ProjectFolder, "Plugins\\Blender-Link\\Content\\BlenderLink.Icon.flax"));
                if(asset == null) 
                {
                    Platform.Error("[BlenderLink] Invalid directory to Blender Link it expect the \"Plugins\\Blender-Link\"");
                    return;
                }
                CreateWindowSettings createWindowSettings = new CreateWindowSettings()
                {
                    IsRegularWindow = false,
                    IsTopmost = true,
                    Title = "Blender Link",
                    Size = (asset.Size / 3f) + new Float2(-20, 20),
                    AllowInput = true,
                    ShowInTaskbar = true,
                    StartPosition = WindowStartPosition.CenterScreen,
                    ActivateWhenFirstShown = true,
                };
                w = Platform.CreateWindow(ref createWindowSettings);
                Image image = new Image()
                {
                    Brush = new TextureBrush(asset),
                    AnchorPreset = AnchorPresets.TopLeft,
                    Size = asset.Size / 3f,
                };
                Button closebutton = new Button()
                {
                    BackgroundBrush = new SpriteBrush(Editor.Instance.Icons.Cross12),
                    AnchorPreset = AnchorPresets.TopRight,
                    Size = Editor.Instance.Icons.Cross12.Size,
                    Location = new Float2(-Editor.Instance.Icons.Cross12.Size.X, 0),
                    BackgroundColor = Color.White,
                    HasBorder = false,
                };
                closebutton.ButtonClicked += (Button b) =>
                {
                    w.Close();
                };
                var s = Style.Current.FontMedium.MeasureText("Select blender path");
                Button Selectblenderpathbutton = new Button()
                {
                    //BackgroundBrush = new SpriteBrush(Editor.Instance.Icons.),
                    AnchorPreset = AnchorPresets.BottomCenter,
                    Size = s,
                    Font = new FontReference(Style.Current.FontMedium),
                    Location = new Float2(-(s.X / 2), (-s.Y) + -10),
                    //BackgroundColor = Color.White,
                    HasBorder = true,
                    Text = "Select blender path",
                };
                Selectblenderpathbutton.ButtonClicked += (Button b) =>
                {
                    if (!FileSystem.ShowOpenFileDialog(w, null, "blender\0blender.*\0", false, "Select blender exe", out var files))
                    {
                        Options.PathToBlender = files[0];
                        w.Close();
                    }
                };

                w.GUI.AddChild(image);
                w.GUI.AddChild(closebutton);
                w.GUI.AddChild(Selectblenderpathbutton);
                w.GUI.PerformLayout(true);
                w.Show();
            }
            else
            {
                Options.PathToBlender = ValidatePath();
            }
        }

        internal static string ValidatePath()
        {
            validateAgain:
            //we can check extensions because on linux everything is file ...
            //so just for safety sake check the name of of the file
            //as a software is constant so blender is named blender for executable ? 
            if (Options.PathToBlender.Contains("blender"))
            {
                if (File.Exists(Options.PathToBlender))
                {
                    return Options.PathToBlender;
                }
            }
            if (!FileSystem.ShowOpenFileDialog(w, null, "blender\0blender.*\0", false, "Select blender exe", out var files))
            {
                Options.PathToBlender = files[0];
                goto validateAgain;
            }
            return Options.PathToBlender;
        }
#if PLATFORM_WINDOWS
        [DllImport("shell32.dll")]
        static unsafe extern int FindExecutable(IntPtr lpFile, IntPtr lpDirectory, IntPtr lpResult);
#endif
    }
}
