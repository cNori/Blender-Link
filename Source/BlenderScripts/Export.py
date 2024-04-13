#-------------------------------------------------------------------------------------------------------------------------
# ________  ___       _______   ________   ________  _______   ________          ___       ___  ________   ___  __       
#|\   __  \|\  \     |\  ___ \ |\   ___  \|\   ___ \|\  ___ \ |\   __  \        |\  \     |\  \|\   ___  \|\  \|\  \     
#\ \  \|\ /\ \  \    \ \   __/|\ \  \\ \  \ \  \_|\ \ \   __/|\ \  \|\  \       \ \  \    \ \  \ \  \\ \  \ \  \/  /|_   
# \ \   __  \ \  \    \ \  \_|/_\ \  \\ \  \ \  \ \\ \ \  \_|/_\ \   _  _\       \ \  \    \ \  \ \  \\ \  \ \   ___  \  
#  \ \  \|\  \ \  \____\ \  \_|\ \ \  \\ \  \ \  \_\\ \ \  \_|\ \ \  \\  \|       \ \  \____\ \  \ \  \\ \  \ \  \\ \  \ 
#   \ \_______\ \_______\ \_______\ \__\\ \__\ \_______\ \_______\ \__\\ _\        \ \_______\ \__\ \__\\ \__\ \__\\ \__\
#    \|_______|\|_______|\|_______|\|__| \|__|\|_______|\|_______|\|__|\|__|        \|_______|\|__|\|__| \|__|\|__| \|__|
#                                                                                                                        
#-------------------------------------------------------------------------------------------------------------------------
#                                                    writen by Nori_SC

#import section
import os
import sys
import bpy
import math
import mathutils

#code section
argv = sys.argv
def GetArg(argname): 
    if argname in argv: 
        return argv.index(argname) + 1
    else: 
        return -1

class Line:
    def __init__(self):
        self.DataMarker = 0
        self.Import = False
        self.Type = ""
        self.ObjectName = ""
        self.parent = None
        self.Object = None
    def set(self,DataMarker,Import,Type,ObjectName):
        self.DataMarker = DataMarker
        self.Import = Import
        self.Type = Type
        self.ObjectName = ObjectName
        return self

    @staticmethod
    def deserialize(line):
        output = Line()
        for i in range(len(line)):
            if line[i] == '-':
                output.DataMarker = i
            else:

                output.Import = line[i] == 'O'
                i += 2
                skip = line.index(' ', i)
                output.Type = line[i:skip]
                i += skip - i + 1
                output.ObjectName = line[i:]
                break
        return output

    @staticmethod
    def serialize(input):
        builder = []
        builder.append('-' * (input.DataMarker+1))
        builder.append('O' if input.Import else 'X')
        builder.append(' ')
        builder.append(input.Type)
        builder.append(' ')
        builder.append(input.ObjectName)
        line = ''.join(builder)
        print(line)
        return line
    def compere(self,depth,type,name):
        return self.DataMarker == depth and self.Type == type and self.ObjectName == name
    
    def has_type_as_parent(self,type):
        p = self.parent
        while p is not None:
            if p.Type == type:
                return True, p
            p = p.parent
        return False, None
        
class BLTCashe:
    def __init__(self):
        self.lines = []
        self.isAt = 0
        self.Path = ""
    @property
    def length(self):
        return len(self.lines)
    @property
    def GetLines(self):
        return self.lines

    def Export(self, path,exportPath):
        self.lines = []
        with open(path, 'r') as file:
            for line in file:
                self.lines.append(Line.deserialize(line.strip()))
        
        for i in range(len(self.lines)):
            for j in range(i-1, -1, -1):
                if i != j:
                    if self.lines[i].DataMarker > self.lines[j].DataMarker:
                        self.lines[i].parent = self.lines[j]
                        break
        bpy.ops.object.select_all(action='DESELECT')
        depth = 0
        self.Path = exportPath
        collection = bpy.context.scene.collection
        self.TreeToFileResursive(collection,depth)
    
    def TreeToFileResursive(self,collection,depth):
        self.AddOrSet(depth ,"COLLECTION", collection.name,collection)
        depth += 1
        for c in collection.children:
            self.TreeToFileResursive(c,depth)
        for obj in collection.objects:
            if not obj.parent:
                self.TreeGetChildren(obj,depth)
                
    def TreeGetChildren(self, parent, depth):
        thisobjectcashe = self.AddOrSet(depth,parent.type,parent.name,parent)
        
        #ceneter the object
        for c in parent.constraints:
            c.enabled = False
        parent.matrix_world = mathutils.Matrix.Identity(4)
        
        if thisobjectcashe.Import:
            if thisobjectcashe.Type == "MESH":
                sucess ,hit = thisobjectcashe.has_type_as_parent("ARMATURE")
                if sucess:
                    print("\n\n\n\n------------------ARMATURE Export--------------------\n\n\n\n" + str(hit.Import) + " " + hit.ObjectName)
                    if hit.Import:
                        self.ExportAnimature(parent,hit.Object);
                else:
                    self.ExportMesh(parent)
                    
        depth += 1
        if parent.animation_data:
            self.AddOrSet(depth,"ANIM_DATA","Animation",parent.animation_data)
            if len(parent.animation_data.nla_tracks) != 0:
                self.ExportNLA(depth,parent,parent.animation_data.nla_tracks)
            elif parent.animation_data.action:
                if self.AddOrSet(depth+1,"CLIP",parent.animation_data.action.name,parent.animation_data.action).Import:
                    self.ExportClip(parent,parent.animation_data.action)
                
        for obj in parent.children:
            self.TreeGetChildren(obj,depth)
    
    def compereToCashe(self,other):
        return lines[isAt].compere(other)
        
    def AddOrSet(self, depth, type, name,obj):
        self.lines[self.isAt].Object = obj;
        out = self.lines[self.isAt]
        if out.compere(depth,type,name):
            print("Found valid line")
        else:
            print("Found Invalid line !!!!!!!")
        self.isAt += 1;
        return out
    
    def MuteTracks(self,obj):
        for nla_tracks in obj.animation_data.nla_tracks:
            for NlaStrip in nla_tracks.strips:
                NlaStrip.mute = True

    def ExportNLA(self,depth,obj,nla_tracks):
        obj.select_set(True)
        bpy.context.view_layer.objects.active = obj
        # set up Animation directorys for a object
        NLA_Directory = os.path.join(self.Path , obj.name , "Animation","NLA Tracks")
        Clips_Directory = os.path.join(self.Path , obj.name , "Animation","Clips")
        if not os.path.exists(NLA_Directory):
            os.makedirs(NLA_Directory)
        if not os.path.exists(Clips_Directory):
            os.makedirs(Clips_Directory)
        
        # mute all tracks
        for nla_track in nla_tracks:
            nla_track.mute = False
        
        for nla_track in nla_tracks:
            nla_track.mute = False
            # unmute All strips
            for strip in nla_track.strips:
                strip.mute = False
                
            importNLATrack = self.AddOrSet(depth+1,"NLA_TRACK",nla_track.name,nla_track).Import
            if nla_track.strips:
                # export funcion for nla_track here
                if importNLATrack:
                    self.ExportAnimation(os.path.join(NLA_Directory, nla_track.name.replace(".", "_")) + ".fbx")
                #mute All strips
                for strip in nla_track.strips:
                    strip.mute = True
                # [sector end] export nla_track
                # export individual strips
                for strip in nla_track.strips:
                    strip.mute = False
                    Type = strip.type
                    if Type == "META":
                        Type = "SEQ_STRIP_META"
                    # export strip or animation clips funcion here
                    if self.AddOrSet(depth+2, Type, strip.name, strip).Import:
                        self.ExportAnimation(os.path.join(Clips_Directory, strip.name.replace(".", "_")) + ".fbx")
                    strip.mute = True
            nla_track.mute = True
        obj.select_set(False)
        bpy.context.view_layer.objects.active = None

    def ExportAnimation(self,fpath):
        bpy.ops.export_scene.fbx(
        filepath= fpath,
        use_selection=True,
        add_leaf_bones=False,
        bake_anim_use_nla_strips=True,
        bake_anim_use_all_actions=False,
        axis_forward='-Z',
        axis_up='Y',
        object_types = {'ARMATURE'})
        
    def ExportMesh(self,obj):
        print("Exportting Mesh:" +obj.name)
        obj.select_set(True)
        bpy.context.view_layer.objects.active = obj
        directory = os.path.join(self.Path , obj.name)
        # Check whether the specified path exists or not
        if not os.path.exists(directory):
            # Create a new directory because it does not exist
            os.makedirs(directory)
        bpy.ops.export_scene.fbx(
        filepath= os.path.join(directory, obj.name.replace(".", "_")) + ".fbx",
        use_selection=True,
        add_leaf_bones=False,
        bake_anim_use_nla_strips=False,
        bake_anim_use_all_actions=False,
        axis_forward='-Z',
        axis_up='Y',
        use_triangles=True,
        object_types = {'MESH'})
        obj.select_set(False)
        bpy.context.view_layer.objects.active = None
    
    def ExportAnimature(self,obj,anima):
        print("Exportting Animature:" +anima.name)
        obj.select_set(True)
        anima.select_set(True)
        bpy.context.view_layer.objects.active = anima
        directory = os.path.join(self.Path , anima.name)
        # Check whether the specified path exists or not
        if not os.path.exists(directory):
            # Create a new directory because it does not exist
            os.makedirs(directory)
        
        anima.data.pose_position = 'REST'
        bpy.ops.export_scene.fbx(
        filepath= os.path.join(directory, anima.name.replace(".", "_")) + ".fbx",
        use_selection=True,
        add_leaf_bones=False,
        bake_anim_use_nla_strips=False,
        bake_anim_use_all_actions=False,
        axis_forward='-Z',
        axis_up='Y',
        use_triangles=True,
        object_types = {'ARMATURE','MESH'})
        obj.select_set(False)
        anima.select_set(False)
        bpy.context.view_layer.objects.active = None
        obj.data.pose_position = 'POSE'
        
    def ExportClip(self,obj,clip):
        print("Exportting NlaStrip:" +clip.name)
        obj.select_set(True)
        bpy.context.view_layer.objects.active = obj
        obj.animation_data.action = clip
        obj.data.pose_position = 'REST'
        directory = os.path.join(self.Path , obj.name , "Animation","Clips")
        # Check whether the specified path exists or not
        if not os.path.exists(directory):
            # Create a new directory because it does not exist
            os.makedirs(directory)
            
        self.ExportAnimation(os.path.join(directory, clip.name.replace(".", "_")) + ".fbx")
        obj.select_set(False)
        bpy.context.view_layer.objects.active = None

def main():
    print("[Blender Link] ExtractData.py has started execution")
    print("[Blender Link] [Flax -> Blender] One way hand sake started")

    # grab the ProjectFolder path from args
    pfid = GetArg("--ProjectFolder")
    if pfid != -1:
        ProjectFolder = argv[pfid]
        ProjectCacheFolder =  os.path.join(ProjectFolder,"Cache")
        itemid = GetArg("--ContentItem")
        if itemid != -1:
            item = argv[itemid][8:]
            path = os.path.join(ProjectCacheFolder,"Blender Link Cache",item)+".BLTCashe"
            directory = os.path.dirname(path)
            # Check whether the specified path exists or not
            if not os.path.exists(directory):
               # Create a new directory because it does not exist
               os.makedirs(directory)
               
            #export path root
            bltcashe = BLTCashe()
            bltcashe.Export(path,os.path.join(ProjectFolder,argv[itemid]))
        else:
            sys.stderr.write("[Blender Link] Faled to get the ContentItem path")

    print("[Blender Link] [Blender -> Flax] One way hand sake started")
    
main()