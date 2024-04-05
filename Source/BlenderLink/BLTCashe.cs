using FlaxEditor.Surface.Archetypes;
using FlaxEngine;
using FlaxEngine.GUI;

using System.Collections.Generic;
using System.IO;
using System.Text;

namespace BlenderLink
{
    /// <summary>
    /// 
    /// </summary>
    public partial class BLTCashe
    {

        List<Line> lines;

        /// <summary>
        /// </summary>
        public int Lenght => lines.Count;
        /// <summary>
        /// Initializes a new instance of the <see cref="BLTCashe"/> class.
        /// </summary>
        public BLTCashe() { }
        /// <summary>
        /// Initializes a new instance of the <see cref="BLTCashe"/> class.
        /// </summary>
        /// <param name="path">The path.</param>
        public BLTCashe(string path) { Deserialize(path); }
        /// <summary>
        /// Deserializes the specified path.
        /// </summary>
        /// <param name="path">The path.</param>
        public void Deserialize(string path)
        {
            lines = new List<Line>();
            string[] file = File.ReadAllLines(path);
            for (int i = 0; i < file.Length; i++)
            {
                lines.Add(Line.Deseralize(file[i]));
                for (int j = lines.Count-1; j >= 0; j--)
                {
                    if (i != j)
                    {
                        if (lines[i].DataMarker > lines[j].DataMarker)
                        {
                            lines[i].Parent = lines[j];
                            break;
                        }
                    }
                }
            }
        }
        /// <summary>
        /// Seralizes the specified path.
        /// </summary>
        /// <param name="path">The path.</param>
        public void Seralize(string path)
        {
            string[] file = new string[lines.Count];
            //Debug.Log("Flax Seralize cashe");
            for (int i = 0; i < file.Length; i++)
            {
                file[i] = Line.Seralize(lines[i]);
                //Debug.Log(file[i]);
            }
            //Debug.Log("Flax end Seralize cashe");
            File.WriteAllLines(path, file);
        }
        public Line GetCasheLine(int id)
        {
            return lines[id];
        }
    }

    public partial class BLTCashe
    {
        internal static Texture action = Content.Load<Texture>(Path.Combine(Globals.ProjectFolder, "Plugins\\Blender Link\\Content\\BlederIcons\\action.flax"));
        internal static Texture anim = Content.Load<Texture>(Path.Combine(Globals.ProjectFolder, "Plugins\\Blender Link\\Content\\BlederIcons\\anim.flax"));
        internal static Texture nla = Content.Load<Texture>(Path.Combine(Globals.ProjectFolder, "Plugins\\Blender Link\\Content\\BlederIcons\\nla.flax"));
        internal static Texture seq_strip_meta = Content.Load<Texture>(Path.Combine(Globals.ProjectFolder, "Plugins\\Blender Link\\Content\\BlederIcons\\seq_strip_meta.flax"));
        internal static Texture outliner = Content.Load<Texture>(Path.Combine(Globals.ProjectFolder, "Plugins\\Blender Link\\Content\\BlederIcons\\outliner.flax"));
        internal static Texture outliner_collection = Content.Load<Texture>(Path.Combine(Globals.ProjectFolder, "Plugins\\Blender Link\\Content\\BlederIcons\\outliner_collection.flax"));
        internal static Texture outliner_data_armature = Content.Load<Texture>(Path.Combine(Globals.ProjectFolder, "Plugins\\Blender Link\\Content\\BlederIcons\\outliner_data_armature.flax"));
        internal static Texture outliner_data_camera = Content.Load<Texture>(Path.Combine(Globals.ProjectFolder, "Plugins\\Blender Link\\Content\\BlederIcons\\outliner_data_camera.flax"));
        internal static Texture outliner_data_curve = Content.Load<Texture>(Path.Combine(Globals.ProjectFolder, "Plugins\\Blender Link\\Content\\BlederIcons\\outliner_data_curve.flax"));
        internal static Texture outliner_data_empty = Content.Load<Texture>(Path.Combine(Globals.ProjectFolder, "Plugins\\Blender Link\\Content\\BlederIcons\\outliner_data_empty.flax"));
        internal static Texture outliner_data_font = Content.Load<Texture>(Path.Combine(Globals.ProjectFolder, "Plugins\\Blender Link\\Content\\BlederIcons\\outliner_data_font.flax"));
        internal static Texture outliner_data_gp_layer = Content.Load<Texture>(Path.Combine(Globals.ProjectFolder, "Plugins\\Blender Link\\Content\\BlederIcons\\outliner_data_gp_layer.flax"));
        internal static Texture outliner_data_greasepencil = Content.Load<Texture>(Path.Combine(Globals.ProjectFolder, "Plugins\\Blender Link\\Content\\BlederIcons\\outliner_data_greasepencil.flax"));
        internal static Texture outliner_data_hair = Content.Load<Texture>(Path.Combine(Globals.ProjectFolder, "Plugins\\Blender Link\\Content\\BlederIcons\\outliner_data_hair.flax"));
        internal static Texture outliner_data_lattice = Content.Load<Texture>(Path.Combine(Globals.ProjectFolder, "Plugins\\Blender Link\\Content\\BlederIcons\\outliner_data_lattice.flax"));
        internal static Texture outliner_data_light = Content.Load<Texture>(Path.Combine(Globals.ProjectFolder, "Plugins\\Blender Link\\Content\\BlederIcons\\outliner_data_light.flax"));
        internal static Texture outliner_data_lightprobe = Content.Load<Texture>(Path.Combine(Globals.ProjectFolder, "Plugins\\Blender Link\\Content\\BlederIcons\\outliner_data_lightprobe.flax"));
        internal static Texture outliner_data_mesh = Content.Load<Texture>(Path.Combine(Globals.ProjectFolder, "Plugins\\Blender Link\\Content\\BlederIcons\\outliner_data_mesh.flax"));
        internal static Texture outliner_data_meta = Content.Load<Texture>(Path.Combine(Globals.ProjectFolder, "Plugins\\Blender Link\\Content\\BlederIcons\\outliner_data_meta.flax"));
        internal static Texture outliner_data_pointcloud = Content.Load<Texture>(Path.Combine(Globals.ProjectFolder, "Plugins\\Blender Link\\Content\\BlederIcons\\outliner_data_pointcloud.flax"));
        internal static Texture outliner_data_speaker = Content.Load<Texture>(Path.Combine(Globals.ProjectFolder, "Plugins\\Blender Link\\Content\\BlederIcons\\outliner_data_speaker.flax"));
        internal static Texture outliner_data_surface = Content.Load<Texture>(Path.Combine(Globals.ProjectFolder, "Plugins\\Blender Link\\Content\\BlederIcons\\outliner_data_surface.flax"));
        internal static Texture outliner_data_volume = Content.Load<Texture>(Path.Combine(Globals.ProjectFolder, "Plugins\\Blender Link\\Content\\BlederIcons\\outliner_data_volume.flax"));
        internal static Texture outliner_ob_armature = Content.Load<Texture>(Path.Combine(Globals.ProjectFolder, "Plugins\\Blender Link\\Content\\BlederIcons\\outliner_ob_armature.flax"));
        internal static Texture outliner_ob_camera = Content.Load<Texture>(Path.Combine(Globals.ProjectFolder, "Plugins\\Blender Link\\Content\\BlederIcons\\outliner_ob_camera.flax"));
        internal static Texture outliner_ob_curve = Content.Load<Texture>(Path.Combine(Globals.ProjectFolder, "Plugins\\Blender Link\\Content\\BlederIcons\\outliner_ob_curve.flax"));
        internal static Texture outliner_ob_empty = Content.Load<Texture>(Path.Combine(Globals.ProjectFolder, "Plugins\\Blender Link\\Content\\BlederIcons\\outliner_ob_empty.flax"));
        internal static Texture outliner_ob_font = Content.Load<Texture>(Path.Combine(Globals.ProjectFolder, "Plugins\\Blender Link\\Content\\BlederIcons\\outliner_ob_font.flax"));
        internal static Texture outliner_ob_force_field = Content.Load<Texture>(Path.Combine(Globals.ProjectFolder, "Plugins\\Blender Link\\Content\\BlederIcons\\outliner_ob_force_field.flax"));
        internal static Texture outliner_ob_greasepencil = Content.Load<Texture>(Path.Combine(Globals.ProjectFolder, "Plugins\\Blender Link\\Content\\BlederIcons\\outliner_ob_greasepencil.flax"));
        internal static Texture outliner_ob_group_instance = Content.Load<Texture>(Path.Combine(Globals.ProjectFolder, "Plugins\\Blender Link\\Content\\BlederIcons\\outliner_ob_group_instance.flax"));
        internal static Texture outliner_ob_hair = Content.Load<Texture>(Path.Combine(Globals.ProjectFolder, "Plugins\\Blender Link\\Content\\BlederIcons\\outliner_ob_hair.flax"));
        internal static Texture outliner_ob_image = Content.Load<Texture>(Path.Combine(Globals.ProjectFolder, "Plugins\\Blender Link\\Content\\BlederIcons\\outliner_ob_image.flax"));
        internal static Texture outliner_ob_lattice = Content.Load<Texture>(Path.Combine(Globals.ProjectFolder, "Plugins\\Blender Link\\Content\\BlederIcons\\outliner_ob_lattice.flax"));
        internal static Texture outliner_ob_light = Content.Load<Texture>(Path.Combine(Globals.ProjectFolder, "Plugins\\Blender Link\\Content\\BlederIcons\\outliner_ob_light.flax"));
        internal static Texture outliner_ob_lightprobe = Content.Load<Texture>(Path.Combine(Globals.ProjectFolder, "Plugins\\Blender Link\\Content\\BlederIcons\\outliner_ob_lightprobe.flax"));
        internal static Texture outliner_ob_mesh = Content.Load<Texture>(Path.Combine(Globals.ProjectFolder, "Plugins\\Blender Link\\Content\\BlederIcons\\outliner_ob_mesh.flax"));
        internal static Texture outliner_ob_meta = Content.Load<Texture>(Path.Combine(Globals.ProjectFolder, "Plugins\\Blender Link\\Content\\BlederIcons\\outliner_ob_meta.flax"));
        internal static Texture outliner_ob_pointcloud = Content.Load<Texture>(Path.Combine(Globals.ProjectFolder, "Plugins\\Blender Link\\Content\\BlederIcons\\outliner_ob_pointcloud.flax"));
        internal static Texture outliner_ob_speaker = Content.Load<Texture>(Path.Combine(Globals.ProjectFolder, "Plugins\\Blender Link\\Content\\BlederIcons\\outliner_ob_speaker.flax"));
        internal static Texture outliner_ob_surface = Content.Load<Texture>(Path.Combine(Globals.ProjectFolder, "Plugins\\Blender Link\\Content\\BlederIcons\\outliner_ob_surface.flax"));
        internal static Texture outliner_ob_volume = Content.Load<Texture>(Path.Combine(Globals.ProjectFolder, "Plugins\\Blender Link\\Content\\BlederIcons\\outliner_ob_volume.flax"));


        /// <summary>
        /// Makes the UI.
        /// </summary>
        /// <param name="parent">The parent.</param>
        /// <returns>size </returns>
        public Float2 MakeUI(ContainerControl parent)
        {
            int maxe = 0;
            for (int i = 0; i < lines.Count; i++)
            {
                lines[i].MakeEntry(parent, i);
                if (lines[i].DataMarker > maxe)
                {
                    maxe = lines[i].DataMarker;
                }
            }
            return new Float2(maxe * Line.EntrySize, lines.Count * Line.EntrySize);
        }

        /// <summary>
        /// <br></br><b>Types</b>:
        /// <see cref="Line.ObjectType"/>
        ///
        /// <br></br><b>Control:</b>
        /// <br></br>X	= Dont export
        /// <br></br>O	= Export
        /// <br></br>-...	= Sub Data Marker
        /// <br></br>-	= Data Marker
        ///
        ///
        /// <br></br><b>Data layout per line</b>
        /// <br></br>[Sub Data Markers][Control:ExportFlag][spacebar char][Type][spacebar char][Name of the object]
        ///
        /// <br></br> <b>Example file</b>
        /// <br></br> X COLLECTION Scene Collection
        /// <br></br> -X COLLECTION Collection
        /// <br></br> --X COLLECTION Collection 2
        /// <br></br> --X LIGHT Sun
        /// <br></br> --X MESH MultiTool
        /// <br></br> --X EMPTY Empty
        /// <br></br> ---X CAMERA Camera
        /// <br></br> -X ARMATURE Robot
        /// <br></br> --X ANIMATION Idle.001
        /// <br></br> --X ANIMATION Walk_Foroward
        /// <br></br> --X ANIMATION Walk_Foroward_Right
        /// <br></br> --X ANIMATION Idle
        /// <br></br> --X ANIMATION Multytool_Hold
        /// <br></br> --X ANIMATION Multytool_Use
        /// <br></br> --X ANIMATION MultyToolSwapBatery
        /// <br></br> --X MESH RobotMesh
        /// </summary>
        public class Line
        {
            internal const float EntrySize = 20;
            /// <summary>
            /// </summary>
            public enum ObjectType
            {
                /// <summary>
                /// invalid data or unkown type
                /// </summary>
                None,
                //blender tags https://blender.stackexchange.com/questions/125114/how-to-get-the-class-of-selected-object-in-blender-2-8                
                /// <summary>
                /// The mesh
                /// </summary>
                MESH,
                /// <summary>
                /// The curve
                /// </summary>
                CURVE,
                /// <summary>
                /// The surface
                /// </summary>
                SURFACE,
                /// <summary>
                /// The meta
                /// </summary>
                META,
                /// <summary>
                /// The font
                /// </summary>
                FONT,
                /// <summary>
                /// The armature
                /// </summary>
                ARMATURE,
                /// <summary>
                /// The lattice
                /// </summary>
                LATTICE,
                /// <summary>
                /// The empty
                /// </summary>
                EMPTY,
                /// <summary>
                /// The gpencil
                /// </summary>
                GPENCIL,
                /// <summary>
                /// The camera
                /// </summary>
                CAMERA,
                /// <summary>
                /// The light
                /// </summary>
                LIGHT,
                /// <summary>
                /// The speaker
                /// </summary>
                SPEAKER,
                /// <summary>
                /// The light probe
                /// </summary>
                LIGHT_PROBE,
                /// <summary>
                /// The volume
                /// </summary>
                VOLUME,
                /// <summary>
                /// The animation clip ak. action
                /// </summary>
                CLIP,
                /// <summary>
                /// </summary>
                ANIM_DATA,
                //custom tags                
                /// <summary>
                /// The collection
                /// </summary>
                COLLECTION,
                /// <summary>
                /// The animation
                /// </summary>
                NLA_TRACK,
                /// <summary>
                /// The meta NLA strip
                /// </summary>
                SEQ_STRIP_META,
            }
            /// <summary>
            /// ObjectType From the string.
            /// </summary>
            /// <param name="str">The string.</param>
            /// <returns></returns>
            public static ObjectType FromString(string str)
            {
                if ("MESH" == str) { return ObjectType.MESH; }
                if ("CURVE" == str) { return ObjectType.CURVE; }
                if ("SURFACE" == str) { return ObjectType.SURFACE; }
                if ("META" == str) { return ObjectType.META; }
                if ("FONT" == str) { return ObjectType.FONT; }
                if ("ARMATURE" == str) { return ObjectType.ARMATURE; }
                if ("LATTICE" == str) { return ObjectType.LATTICE; }
                if ("EMPTY" == str) { return ObjectType.EMPTY; }
                if ("GPENCIL" == str) { return ObjectType.GPENCIL; }
                if ("CAMERA" == str) { return ObjectType.CAMERA; }
                if ("LIGHT" == str) { return ObjectType.LIGHT; }
                if ("SPEAKER" == str) { return ObjectType.SPEAKER; }
                if ("LIGHT_PROBE" == str) { return ObjectType.LIGHT_PROBE; }
                if ("COLLECTION" == str) { return ObjectType.COLLECTION; }
                if ("NLA_TRACK" == str) { return ObjectType.NLA_TRACK; }
                if ("CLIP" == str) { return ObjectType.CLIP; }
                if ("ANIM_DATA" == str) { return ObjectType.ANIM_DATA; }
                if ("VOLUME" == str) { return ObjectType.VOLUME; }
                if ("SEQ_STRIP_META" == str) { return ObjectType.SEQ_STRIP_META; }

                return ObjectType.None;
            }
            /// <summary>
            /// The data marker
            /// </summary>
            public int DataMarker;
            /// <summary>
            /// The import
            /// </summary>
            public bool Import;
            /// <summary>
            /// The type
            /// </summary>
            public ObjectType Type;
            /// <summary>
            /// The object name
            /// </summary>
            public string ObjectName;

            public CheckBox ImportCheckBox { get; private set; }
            public Line Parent { get; internal set; }
            #region Seralisation
            /// <summary>
            /// Deseralizes the specified line.
            /// </summary>
            /// <param name="line">The line.</param>
            /// <returns></returns>
            public static Line Deseralize(string line)
            {
                Line output = new Line();
                for (int i = 0; i < line.Length; i++)
                {
                    if (line[i] == '-')
                    {
                        //↓
                        //[Sub Data Markers][Control:ExportFlag][spacebar char][Type][spacebar char][Name of the object]
                        output.DataMarker = i;
                    }
                    else
                    {
                        //Debug.Log(line);
                        //                  ↓
                        //[Sub Data Markers][Control:ExportFlag][spacebar char][Type][spacebar char][Name of the object]
                        output.Import = line[i] == 'O';
                        i++;
                        //                                      ↓
                        //[Sub Data Markers][Control:ExportFlag][spacebar char][Type][spacebar char][Name of the object]
                        i++;
                        //                                                     ↓
                        //[Sub Data Markers][Control:ExportFlag][spacebar char][Type][spacebar char][Name of the object]
                        var skip = line.IndexOf(' ', i);
                        output.Type = FromString(line.Substring(i, skip - i));
                        i += skip - i;
                        //                                                           ↓
                        //[Sub Data Markers][Control:ExportFlag][spacebar char][Type][spacebar char][Name of the object]
                        i++;
                        //                                                                          ↓
                        //[Sub Data Markers][Control:ExportFlag][spacebar char][Type][spacebar char][Name of the object]

                        //Debug.Log(line);
                        output.ObjectName = line[i..];

                        break;//we are done
                    }
                }
                return output;
            }

            /// <summary>
            /// Seralizes the specified input.
            /// </summary>
            /// <param name="input">The input.</param>
            /// <returns></returns>
            public static string Seralize(Line input)
            {
                StringBuilder builder = new StringBuilder();
                //↓
                //[Sub Data Markers][Control:ExportFlag][spacebar char][Type][spacebar char][Name of the object]
                builder.Append(new string('-', input.DataMarker+1));
                //                  ↓
                //[Sub Data Markers][Control:ExportFlag][spacebar char][Type][spacebar char][Name of the object]
                builder.Append(input.Import ? 'O' : 'X');
                //                                      ↓
                //[Sub Data Markers][Control:ExportFlag][spacebar char][Type][spacebar char][Name of the object]
                builder.Append(' ');
                //                                                     ↓
                //[Sub Data Markers][Control:ExportFlag][spacebar char][Type][spacebar char][Name of the object]
                builder.Append(input.Type.ToString());
                //                                                           ↓
                //[Sub Data Markers][Control:ExportFlag][spacebar char][Type][spacebar char][Name of the object]
                builder.Append(' ');
                //                                                                          ↓
                //[Sub Data Markers][Control:ExportFlag][spacebar char][Type][spacebar char][Name of the object]
                builder.Append(input.ObjectName);
                return builder.ToString();
            }
            #endregion
            #region UI            
            /// <summary>
            /// Makes the UI entry.
            /// </summary>
            /// <param name="parent">The parent.</param>
            /// <param name="ID">The identifier.</param>
            public void MakeEntry(ContainerControl parent, int ID)
            {
                var bg = parent.AddChild<Image>();
                bg.Size = new Float2(1000000, EntrySize);
                bg.Location = new(0, ID * EntrySize);
                if (ID % 2 == 0)
                {
                    bg.BackgroundColor = Style.Current.LightBackground.RGBMultiplied(0.5f);
                }


                Texture texture = null;
                bool EnabledImportCheckBox;
                Color color = Color.White;
                switch (Type)
                {
                    case ObjectType.None: EnabledImportCheckBox = false; break;
                    case ObjectType.MESH: texture = outliner_ob_mesh; EnabledImportCheckBox = true; color = Color.DarkOrange; break;
                    case ObjectType.CURVE: texture = outliner_ob_curve; EnabledImportCheckBox = false; color = Color.DarkOrange; break;
                    case ObjectType.SURFACE: texture = outliner_ob_surface; EnabledImportCheckBox = false; color = Color.DarkOrange; break;
                    case ObjectType.META: texture = outliner_ob_meta; EnabledImportCheckBox = false; color = Color.DarkOrange; break;
                    case ObjectType.FONT: texture = outliner_ob_font; EnabledImportCheckBox = false; color = Color.DarkOrange; break;
                    case ObjectType.ARMATURE: texture = outliner_ob_armature; EnabledImportCheckBox = true; color = Color.DarkOrange; break;
                    case ObjectType.LATTICE: texture = outliner_ob_lattice; EnabledImportCheckBox = false; color = Color.DarkOrange; break;
                    case ObjectType.EMPTY: texture = outliner_ob_empty; EnabledImportCheckBox = false; color = Color.DarkOrange; break;
                    case ObjectType.GPENCIL: texture = outliner_ob_greasepencil; EnabledImportCheckBox = false; color = Color.DarkOrange; break;
                    case ObjectType.CAMERA: texture = outliner_ob_camera; EnabledImportCheckBox = false; color = Color.DarkOrange; break;
                    case ObjectType.LIGHT: texture = outliner_ob_light; EnabledImportCheckBox = false; color = Color.DarkOrange; break;
                    case ObjectType.SPEAKER: texture = outliner_ob_speaker; EnabledImportCheckBox = false; color = Color.DarkOrange; break;
                    case ObjectType.LIGHT_PROBE: texture = outliner_ob_lightprobe; EnabledImportCheckBox = false; color = Color.DarkOrange; break;
                    case ObjectType.COLLECTION: texture = outliner_collection; EnabledImportCheckBox = true; break;
                    case ObjectType.NLA_TRACK: texture = nla; EnabledImportCheckBox = true; break;
                    case ObjectType.CLIP: texture = action; EnabledImportCheckBox = true; break;
                    case ObjectType.ANIM_DATA: texture = anim; EnabledImportCheckBox = false; color = Color.Lime; break;
                    case ObjectType.SEQ_STRIP_META: texture = seq_strip_meta; EnabledImportCheckBox = true; color = Color.Purple; break;
                    case ObjectType.VOLUME: texture = outliner_data_volume; EnabledImportCheckBox = false; break;
                    default: EnabledImportCheckBox = false; break;
                }

                float fex = DataMarker * EntrySize;

                ImportCheckBox = parent.AddChild<SCheckBox>();
                ImportCheckBox.Size = new(EntrySize);
                ImportCheckBox.BoxSize = EntrySize;
                ImportCheckBox.Location = new(fex, ID * EntrySize);
                ImportCheckBox.Pivot = Float2.Zero;
                ImportCheckBox.Checked = Import && EnabledImportCheckBox;
                ImportCheckBox.Enabled = EnabledImportCheckBox;
                ImportCheckBox.StateChanged += (CheckBox c) =>
                {
                    var p = Parent;
                    if (Parent != null)
                    {
                        switch (Type)
                        {
                            case ObjectType.CLIP:
                            case ObjectType.ANIM_DATA:
                            case ObjectType.COLLECTION:
                            case ObjectType.NLA_TRACK:
                            case ObjectType.SEQ_STRIP_META:
                                p.ImportCheckBox.Checked = c.Checked;
                                p = p.Parent;
                                break;
                            default:
                                break;
                        }
                    }
                    Import = c.Checked;
                };


                fex += EntrySize;

                var ObjectNameLabel = parent.AddChild<Label>();
                ObjectNameLabel.Size = new(ObjectNameLabel.Size.X, EntrySize);
                ObjectNameLabel.Pivot = Float2.Zero;
                ObjectNameLabel.Text = ObjectName;
                ObjectNameLabel.HorizontalAlignment = TextAlignment.Near;

                if (texture != null)
                {
                    var ObjectIcon = parent.AddChild<Image>();
                    ObjectIcon.Size = new(EntrySize);
                    ObjectIcon.Location = new(fex, ID * EntrySize);
                    ObjectIcon.Brush = new TextureBrush(texture);
                    ObjectIcon.Color = color;
                    ObjectNameLabel.Location = new(ObjectIcon.X + EntrySize, ID * EntrySize);
                }
                else
                {
                    ObjectNameLabel.Location = new(fex, ID * EntrySize);
                    var TypeLabel = parent.AddChild<Label>();
                    TypeLabel.Size = new(TypeLabel.Size.X, EntrySize);
                    TypeLabel.Location = new(200, ID * EntrySize);
                    TypeLabel.Pivot = Float2.Zero;
                    TypeLabel.Text = Type.ToString();
                }
            }
            private class SCheckBox : CheckBox
            {
                public bool hasShiftDown;
                public override bool OnKeyDown(KeyboardKeys key)
                {
                    if (key == KeyboardKeys.Shift)
                    {
                        hasShiftDown = true;
                    }
                    return base.OnKeyDown(key);
                }
                public override void OnKeyUp(KeyboardKeys key)
                {
                    if (key == KeyboardKeys.Shift)
                    {
                        hasShiftDown = false;
                    }
                    base.OnKeyUp(key);
                }
            }
            #endregion

            public bool HasTypeAsParent(ObjectType type,out Line hit)
            {
                var p = this.Parent;
                while (p != null)
                {
                    if(p.Type == type)
                    {
                        hit = p;
                        return true;
                    }
                    p = p.Parent;
                }
                hit = null;
                return false;
            }
        }

    }
}
