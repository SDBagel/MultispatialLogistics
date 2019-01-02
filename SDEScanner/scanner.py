import glob

def generateStargateList():
    files = glob.glob("sde-20180713-TRANQUILITY\\sde\\fsd\\universe\\eve\\*\\*\\*\\solarsystem.staticdata")
    out = open("out.txt", "w")
    for file in files:
        data = open(file, 'r').read()
        sysID = data.split('solarSystemID:')[1].split('solarSystemNameID:')[0].strip()
        data = data.split('stargates:')[1].split('sunTypeID:')[0]
        data = data.split("\n")
        out.write(sysID+"\n")
        for l in data:
            if(not("typeID" in l)):
                out.write(l.strip() + "\n")

generateStargateList()
