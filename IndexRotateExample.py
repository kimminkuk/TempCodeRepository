from  random import *
EX_A=[[1,2,4,6,4],[4,2,6,1,3],[7,5,8,5,2]]


N = 5
loop_short = int(N/2)
loop_long = int((N+1)/2)
c_short_add = int(N/2 +1)
c_long_add = int(N/2)
EX_B=[[randint(1,100) for i in range(N)] for j in range(N)]
print(EX_B)

TEMP = [[0 for x in range(loop_short)] for y in range(loop_long)]
print(TEMP)
#Sol1 (Array Copy)
# for y in range(N):
#     for x in range(N):
#         NewX = (y*-1) + (N-1)
#         NewY = x
#         RESULT_B[NewY][NewX]=EX_A[y][x]
#print(RESULT_B)

#Sol2 (Inline Array)

#Save A Site
for y in range(loop_long):
    for x in range(loop_short):
        TEMP[y][x] = EX_B[y][x]

# D to A Site
for y in range(loop_long):
    for x in range(loop_short):
        NewX = y
        NewY = (x*-1) + (N-1)
        EX_B[y][x] = EX_B[NewY][NewX]

# C to D
for y in range(int((N+1)/2), loop_short + int((N+1)/2)):
    for x in range(loop_long):
        NewX = y
        NewY = (x*-1) + (N-1)
        EX_B[y][x] = EX_B[NewY][NewX]

# B to C
for y in range(c_long_add, loop_long+c_long_add):
    for x in range(c_short_add, loop_short + c_short_add):
        NewX = y
        NewY = (x*-1) + (N-1)
        EX_B[y][x] = EX_B[NewY][NewX]

# A to B
for y in range(loop_short):
    for x in range(int(N/2),loop_long+int(N/2)):
        NewX = y
        NewY = (x*-1) + (N-1)
        EX_B[y][x] = TEMP[NewY][NewX]

print (EX_B)




def test2(booked, unbooked):
    answer = []
    un, b = [], []
    def time_convert(time):
        return int(time[0:2]) * 60 + int(time[3:5])
    
    for t, n in booked:
        b.append((time_convert(t),n,'b'))

    for t, n in unbooked:
        un.append((time_convert(t),n,'un'))

    b = sorted(b, key=lambda x: x[0], reverse=False)
    un = sorted(un, key=lambda x: x[0], reverse=False)

    time_reserved = 0
    time_b, time_un = b[0][0], un[0][0]
    if time_b <= time_un:
        time_reserved = time_b
        answer.append(b[0][1])
        b.pop(0)        
    else:
        time_reserved = time_un
        answer.append(un[0][1])
        un.pop(0)    

    while b and un:
        time_b, time_un = b[0][0], un[0][0]
        if time_reserved + 10 >= time_b:
            time_reserved = time_b
            answer.append(b[0][1])
            b.pop(0)
        elif time_reserved > time_un:
            time_reserved = time_un
            answer.append(un[0][1])
            un.pop(0)
        else:
            if time_b <= time_un:
                time_reserved = time_b
                answer.append(b[0][1])
                b.pop(0)
            else:
                time_reserved = time_un
                answer.append(un[0][1])
                un.pop(0)            
    if b:
        while b:
            answer.append(b[0][1])
            b.pop(0)
    elif un:
        while un:
            answer.append(un[0][1])
            un.pop(0)
    
    return answer
    
