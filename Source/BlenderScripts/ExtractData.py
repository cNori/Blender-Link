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
#                                                https://github.com/NoriteSC

#import section
import os
import sys
import bpy

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

        return line
    def compere(self,depth,type,name):
        return self.DataMarker == depth and self.Type == type and self.ObjectName == name

class BLTCashe:
    def __init__(self):
        self.lines = []
        self.isAt = 0

    @property
    def length(self):
        return len(self.lines)
    @property
    def GetLines(self):
        return self.lines

    def deserialize(self, path):
        if not os.path.exists(path):
            return
        self.lines = []
        with open(path, 'r') as file:
            self.deserializeList(file)
                
    def deserializeList(self, file):
        for line in file:
            self.lines.append(Line.deserialize(line.strip()))

        depth = 0
        collection = bpy.context.scene.collection
        self.TreeToFileResursive(collection,depth)
    
    def serialize(self, path):
        with open(path, 'w') as file:
            for line in self.lines:
                file.write(Line.serialize(line) + '\n')
                
    def serializeList(self):
        out = ""
        for line in self.lines:
            out = Line.serialize(line)
        return out
    
    
    def TreeToFileResursive(self,collection,depth):
        self.AddOrSet(depth ,"COLLECTION", collection.name)
        depth += 1
        for c in collection.children:
            self.TreeToFileResursive(c,depth)
        for obj in collection.objects:
            if not obj.parent:
                self.TreeGetChildren(obj,depth)
                
    def TreeGetChildren(self, parent, depth):
        self.AddOrSet(depth,parent.type,parent.name)
        depth += 1
        if parent.animation_data:
            self.AddOrSet(depth,"ANIM_DATA","Animation")
            if len(parent.animation_data.nla_tracks) != 0:
                for nla_track in parent.animation_data.nla_tracks:
                        self.AddOrSet(depth+1,"NLA_TRACK",nla_track.name)
                        for action in nla_track.strips:
                            if action.type == "META":
                                self.AddOrSet(depth+2,"SEQ_STRIP_META",action.name)
                            else:
                                self.AddOrSet(depth+2,action.type,action.name)
            elif parent.animation_data.action:
                self.AddOrSet(depth+1,"CLIP",parent.animation_data.action.name)
                
        for obj in parent.children:
            self.TreeGetChildren(obj,depth)
    
    def compereToCashe(self,other):
        return lines[isAt].compere(other)
        
    def AddOrSet(self, depth, type, name):
        if self.isAt >= len(self.lines):
            self.lines.append(Line().set(depth,False,type,name))
        else:
            if self.lines[self.isAt].compere(depth,type,name):
                self.lines[self.isAt].set(depth,self.lines[self.isAt].Import, type,name)
            else:
                self.lines[self.isAt].set(depth,False,type,name)
        self.isAt += 1;


def main():
    print("[Blender Link] ExtractData.py has started execution")
    print("[Blender Link] [Flax -> Blender] One way hand sake started")

    # grab the ProjectFolder path from args
    pfid = GetArg("--ProjectFolder")
    if pfid != -1:
        ProjectFolder = argv[pfid]
        ProjectCacheFolder =  os.path.join(ProjectFolder,"Cache")
        print("[Blender Link]\nProjectCacheFolder: "+ ProjectCacheFolder)
        itemid = GetArg("--ContentItem")
        if itemid != -1:
            item = argv[itemid][8:]
            print(item)
            path = os.path.join(ProjectCacheFolder,"Blender Link Cache",item)+".BLTCashe"
            directory = os.path.dirname(path)
            # Check whether the specified path exists or not
            if not os.path.exists(directory):
               # Create a new directory because it does not exist
               os.makedirs(directory)
               
            bltcashe = BLTCashe()
            bltcashe.deserialize(path)
            bltcashe.serialize(path)
        else:
            sys.stderr.write("[Blender Link] Faled to get the ContentItem path")

    print("[Blender Link] [Blender -> Flax] One way hand sake started")
    
main()