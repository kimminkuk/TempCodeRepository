#KMP, String Search is O(n)
# 0 1 2 3 0 1
# A A A A B B , A B, A A B
#           5
# ↓  
# B A N A N A
#   ↑

# ↓
# A B A A B A B
#   ↑

# ↓  
# A B C D A B E  , pi[4] = 1, pi[5] = 2, pi[6] = 0
#             ↑

# ↓
# A B A A B A B
#   ↑

#         1 2 3 4 5 6 7
# A B C D A B C D A B E E
#                       A B C D A B E
# 0 1 2 3 4 5 6 7 8 9 10 11

#   
# A B C A B A B C D E , A B C , O(n * m) -> O(n + m)
# A B C
# 

def sol(test_sol):
    s_iter = 0
    test_len = len(test_sol)
    pi = [ 0 for i in range(0,test_len) ]
    for i in range(1, test_len):
        while s_iter != 0 and test_sol[i] != test_sol[s_iter]:
            s_iter = pi[s_iter-1] 

        if test_sol[i] == test_sol[s_iter]:
            s_iter += 1
            pi[i] = s_iter

    return pi

def sol2(testArr, test_sol):
    answer = []
    s_iter = 0
    test_sol_len = len(test_sol)
    testArr_len = len(testArr)
    pi = sol(test_sol)
    pi2 = [0 for i in range(0, testArr_len)]
    for i in range(0,testArr_len):
        while s_iter > 0 and testArr[i] != test_sol[s_iter]:
            #s_iter = pi2[s_iter]
            s_iter = pi[s_iter - 1]
        if testArr[i] == test_sol[s_iter]:
            s_iter+=1
            pi2[i] = s_iter
            if s_iter == test_sol_len:
                answer.append(i)
                s_iter = 0
            
    return answer

#test = 'BANANA'
#test = 'ABCDABE'
test = 'ABCDABE'
test_len = len(test)
test2 = 'ABCDABCDABEE'
result = sol2(test2, test)
start = result[0] - test_len
print('e',result, ' s',  start)



# ###
# def sol2(testArr, test_sol):
#     answer = []
#     s_iter = 0
#     test_sol_len = len(test_sol)
#     testArr_len = len(testArr)
#     pi = sol(test_sol)

#     for i in range(0,testArr_len):
#         while s_iter > 0 and testArr[i] != test_sol[s_iter]:
#             s_iter = pi[s_iter - 1]
#         if testArr[i] == test_sol[s_iter]:
#             if s_iter == test_sol_len-1:
#                 answer.append((i+1)-(test_sol_len))
#                 s_iter = pi[s_iter]
#             else:
#                 s_iter += 1
#     return answer

# test = str(input())
# test2 = str(input())
# result = sol2(test, test2)

# print(len(result))
# for i in result:
#     print(i+1, end=' ')
