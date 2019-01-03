import glob

def generateStargateList():
    #grab all files that contain stargate data
    files = glob.glob("sde-20180713-TRANQUILITY\\sde\\fsd\\universe\\eve\\*\\*\\*\\solarsystem.staticdata")
    out = open("out.txt", "w")

    for file in files:
        data = open(file, 'r').read()
        #split data before and after systemID to get value
        sysID = data.split('solarSystemID:')[1].split('solarSystemNameID:')[0].strip()
        #split data before and after stargate info to get values, then split by line
        data = data.split('stargates:')[1].split('sunTypeID:')[0]
        data = data.split("\n")
        out.write(sysID)
        #write stripped lines to output
        for l in data:
            if(not("typeID" in l)):
                out.write(l.strip() + "\n")

#terrible way to get it into the SQL Database but here it is
def generateCSCode():
    data = open("out.txt", "r").read()
    data = data.split("\n\n")
    for s in data:
        info = s.split("\n")
        ParentSystemId = (info[0])[0:8]
        StargateId = 0
        DestinationStargateId = 0
        XPos = 0
        YPos = 0
        ZPos = 0
        for i in range(int((len(info)-1)/6)):
            StargateId = (info[(i*6)+1])[0:8]
            DestinationStargateId = (info[(i*6)+2])[13:22]
            XPos = (info[(i*6)+4])[2:]
            YPos = (info[(i*6)+5])[2:]
            ZPos = (info[(i*6)+6])[2:]
            cs = open("generatedCS.txt", "a")
            cs.write("new Stargate\n")
            cs.write("{\n")
            cs.write("    ParentSystemId = "+str(ParentSystemId)+",\n")
            cs.write("    StargateId = "+str(StargateId)+",\n")
            cs.write("    DestinationStargateId = "+str(DestinationStargateId)+",\n")
            cs.write("    XPos = "+str(XPos[0:-2])+",\n")
            cs.write("    YPos = "+str(YPos[0:-2])+",\n")
            cs.write("    ZPos = "+str(ZPos[0:-2])+"\n")
            cs.write("},\n")

generateStargateList()
generateCSCode()
