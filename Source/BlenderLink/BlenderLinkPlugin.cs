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

using System;
using FlaxEngine;
using FlaxEditor;
using FlaxEditor.Options;
using System.IO;
using FlaxEditor.Content.Import;
using FlaxEditor.Content;
using static FlaxEditor.GUI.Docking.DockHintWindow;

namespace BlenderLink;
/// <summary>
/// </summary>
public class BlenderLinkPlugin : EditorPlugin
{
    /// <summary>
    /// Initializes a new instance of the <see cref="BlenderLinkPlugin"/> class.
    /// </summary>
    public BlenderLinkPlugin() 
    {
        // Initialize plugin description
        _description = new PluginDescription
        {
            Name = "Blender Link",
            Category = "Tools",
            Description = "batch import of models and animations from single blend file,\nAPI for running scripts in blender",
            Author = "Norite SC",
            AuthorUrl = "",
            RepositoryUrl = "",
            IsAlpha = true,
            Version = new Version(0, 1, 5, 0),
        };
    }
    /// <inheritdoc/>
    public AssetProxy[] Proxies =
    [
        new BlenderAssetProxy(),
        new BlenderBackupAssetProxy(),
        new PythonScriptProxy(),
    ];
    /// <inheritdoc/>
    public static string PathToBlenderScripts { get; private set; }
    /// <inheritdoc/>
    public override void InitializeEditor()
    {
        for (int i = 0; i < Proxies.Length; i++)
        {
            Editor.ContentDatabase.AddProxy(Proxies[i]);
        }

        //he he no build in, overload the import entry
        ImportFileEntry.FileTypes["blend"] = Import;
        PathToBlenderScripts = Path.Combine(Globals.ProjectFolder, "Plugins\\Blender Link\\Source\\BlenderScripts\\");
        Editor.Options.AddCustomSettings("Blender Link", new OptionsModule.CreateCustomSettingsDelegate(() => { return BlenderLinkOptions.Options; }));
        Editor.Options.OptionsChanged += Options_OptionsChanged;
        BlenderLinkOptions.Load();
        Editor.ContentDatabase.Rebuild(true);
    }

    private void Options_OptionsChanged(EditorOptions obj)
    {
    }
    /// <inheritdoc/>
    public override void DeinitializeEditor()
    {
        Editor.Options.OptionsChanged -= Options_OptionsChanged;
        Editor.Options.RemoveCustomSettings("Blender Link");
        for (int i = 0; i < Proxies.Length; i++)
        {
            Editor.ContentDatabase.RemoveProxy(Proxies[i]);
        }
        Editor.ContentDatabase.Rebuild(true);
    }

    /// <inheritdoc/>
    public class ImportFileRefrenceEntry : ImportFileEntry
    {
        /// <inheritdoc/>
        public ImportFileRefrenceEntry(ref Request request) : base(ref request)
        {
            request.OutputPath = Path.GetFileNameWithoutExtension(request.OutputPath) + Path.GetExtension(request.InputPath);
            request.IsInBuilt = false;
        }
        /// <inheritdoc/>
        public override bool Import()
        {
            return false;
        }
    }
    private static ImportFileEntry Import(ref Request request)
    {
        return new ImportFileRefrenceEntry(ref request);
    }
}