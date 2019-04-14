import glob
import os
import json

#writes list of stargates and info organized by parent system ID
#generateJson() depends on this functions output organization
def generateStargateList():
    #grab all files that contain stargate data
    files = glob.glob("sde\\fsd\\universe\\eve\\*\\*\\*\\solarsystem.staticdata")
    out = open("out.txt", "w")

    for file in files:
        data = open(file, 'r').read()
        #split data before and after systemID to get value
        sysID = data.split('solarSystemID:')[1].split('solarSystemNameID:')[0].strip()
        #split data before and after stargate info to get values, then split by line
        data = data.split('stargates:')[1].split('sunTypeID:')[0].split("\n")
        out.write((os.path.abspath(os.path.join(file, os.pardir))+"\n").split("\\")[-1])
        out.write(sysID)
        #write stripped lines to output
        for l in data:
            if(not("typeID" in l)):
                out.write(l.strip() + "\n")

# define a Stargate class that can be serialized
class Stargate():
    def __init__(self, PSystemId, PSystemName, StargateId, DSystemId, DStargateId, XPos, YPos, ZPos):
        self.ParentSystemId = PSystemId
        self.ParentSystemName = PSystemName
        self.StargateId = StargateId
        self.DestinationSystemId = DSystemId
        self.DestinationStargateId = DStargateId
        self.XPos = XPos
        self.YPos = YPos
        self.ZPos = ZPos

#generates JSON file through the power of j a n k
def generateJson():
    data = open("out.txt", "r").read().split("\n\n")
    data.pop()
    lines = open("out.txt", "r").read().split("\n")

    gates = []
    for s in data:
        info = s.split("\n")
        ParentSystemId = (info[1])[0:8]
        ParentSystemName = info[0]

        for i in range(int((len(info)-1)/6)):
            StargateId = (info[(i*6)+2])[0:8]
            DestinationStargateId = (info[(i*6)+3])[13:22]
            #finds index of DestinationStargateId as a regular gate ID
            gatePos = lines.index(DestinationStargateId+":")
            #for 20 potential gates up, try and find ParentSystemId
            for z in range(0, 20):
                if (len(lines[gatePos-((z*6)+1)]) == 8):
                    DestinationSystemId = lines[gatePos-((z*6)+1)]
                    break
            XPos = (info[(i*6)+5])[2:-2]
            YPos = (info[(i*6)+6])[2:-2]
            ZPos = (info[(i*6)+7])[2:-2]

            gates.append(Stargate(ParentSystemId, ParentSystemName, StargateId, DestinationSystemId, DestinationStargateId, XPos, YPos, ZPos).__dict__)

    generated = open("generated.json", "w")
    json.dump(gates, generated)

generateStargateList()
generateJson()
