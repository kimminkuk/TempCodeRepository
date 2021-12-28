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

#   ↓
# A B A A B A B
#             ↑

#
# A B C D A B C D A B E E
#         A B C D A B E
#

# ↓  
# A B C A B A B C D E , A B C , O(n * m) -> O(n + m)
#   A B C
# ↑

def sol(test_sol, testSearch_sol):
    answer = ''
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

#test = 'BANANA'
#test = 'ABCDABE'
test = 'ABAABAB'
testSearch = str(input()).strip()
result = sol(test, testSearch)
print(result)
