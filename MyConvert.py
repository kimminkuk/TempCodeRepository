class structConvert:
    def __init__(self):
        return

    def findStructName(self, strcutName):
        if len(strcutName) > 0 and strcutName[0] =="}":
            lastStrindex = strcutName.find(';')
            findSlashAndNewStr = ''
            if strcutName.find('//') == -1:
                findSlashAndNewStr = strcutName+'  //<<<'
                return [ strcutName[1:lastStrindex].strip(), findSlashAndNewStr]
            else:
                findSlashAndNewStr = strcutName.replace('//', '//<<<')
                tIdx = findSlashAndNewStr.find('//<<<')
                tLen = len('//<<<')
                L_findSlashAndNewStr = len(findSlashAndNewStr)
                findSlashAndNewStr2 = ''

                for fSANS_idx in range(L_findSlashAndNewStr):
                    if tIdx+tLen <  fSANS_idx <= tIdx+tLen+3 and findSlashAndNewStr[fSANS_idx] == ' ':
                        pass
                    else:
                        findSlashAndNewStr2 += findSlashAndNewStr[fSANS_idx]
                return [ strcutName[1:lastStrindex].strip(), findSlashAndNewStr2]
            
        else:
            return [-1]

    def findMyComment_Ver1(self, TextLine):
        if TextLine.find('//!<') != -1:
            f_idx_temp = TextLine.replace('//!<', '//')
            return f_idx_temp
        else:
            return TextLine

    def structNameMake(self, textName):
        f = open(textName, 'rt', encoding='UTF8')
        step_1 = []
        for f_idx in f:
            temp = f_idx.strip()
            if len(temp) > 0 and temp[0] == "}":
                lastStrindex = temp.find(';')
                step_1.append(temp[1:lastStrindex].strip())
        
        f2 = open("_Update_"+textName, "w", encoding='UTF8')
        for step1_idx in step_1:
            data = '//>>>'+step1_idx+'\n'
            f2.write(data)
        f.close()
        f2.close()
        return

    def structNameMake_Ver2(self, textName):
        f = open(textName, 'rt', encoding='UTF8')
        f2 = open("_Ver2_"+textName, "w", encoding='UTF8')
        step_1 = []
        check = 0
        for f_idx in f:
            fileReadToStrip = f_idx.strip()
            if len(fileReadToStrip) > 0 and fileReadToStrip.find("typedef") >= 0 and check == 0:
                check = 1
                step_1.append(f_idx)
            else:
                if check == 0:
                    if f_idx.find("//!<") != -1:
                        f2.write(f_idx)
                    else:
                        f_idx_temp = f_idx.replace('//!<', '//')
                        f2.write(f_idx_temp)
                elif check == 1:
                    sName = self.findStructName(fileReadToStrip)
                    
                    if len(sName) == 2:
                        step_1.append(sName[1])
                        step_1.insert(0, '//>>> '+sName[0]+'\n')
                        check = 0
                        for step_1_idx in step_1:
                            f2.write(step_1_idx)
                        step_1.clear()
                    else:
                        step_1.append(f_idx)
        f.close()
        f2.close()
        self.UnionDelete("_Ver2_"+textName)
        return

    def UnionDelete(self, textName):
        f = open(textName, 'rt', encoding='UTF8')
        textNameToHeaderFile = textName[:-4]
        textNameToHeaderFile = textNameToHeaderFile+'.h'
        f2 = open(textNameToHeaderFile, "w", encoding='UTF8')
        check = 0
        for f_idx in f:
            fileReadToStrip = f_idx.strip()
            if len(fileReadToStrip) > 0 and fileReadToStrip.find("union") >= 0 and check == 0:
                check = 1

                f2.write(self.findMyComment_Ver1(f_idx))
            else:
                if check == 0:
                    f2.write(f_idx)
                elif check == 1:
                    if f_idx.find('//!<') != -1:
                        f_idx = f_idx.replace('//!<', '//')
                    f2.write(f_idx)
                    if fileReadToStrip[:2] == "};":
                        check = 0

        f.close()
        f2.close()

        return

#textName="TgcfMemoryMap.txt"
#textName="PeUnitMemoryMap.txt"
#textName="FcmUnitMemoryMap.txt"
textName="BusIfMemoryMap.txt"
#textName="FirmMemoryMap.txt"
#textName="AlpgUnitMemoryMap.txt"
#textName="MfifUnitMemoryMap.txt"
#textName="DpsUnitMemoryMap.txt"
myConvert = structConvert()
myConvert.structNameMake_Ver2(textName)

