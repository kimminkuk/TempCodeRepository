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

    