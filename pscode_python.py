


def test(_str):
    answer = 0
    
    # A -> 1, Z -> 26   | 1G : 26
    # AA -> 27          | 2G : 1G * 1G + 1G ?
    # AZ -> 27 + 25  52
    # BA -> 27 + 26  53
    
    # AA -> (26^1 * 1) + 1
    # BA -> (26^1 * 2) + 1
    # ZA -> (26^1 * 26) + 1
    # ZZ -> (26^1 * 26) + 25
    # AAA -> (26^2 * 1) + (26^1 * 1) + 1
    # ABA -> (26^2 * 1) + (26^1 * 2) + 1
    # BAA -> (26^2 * 2) + (26^1 * 1) + 1
    
    # 1. check to len(str)
    # 2. for 0 to len
    str_len = len(_str)
    my_alp={"A":1, "B":2, "C":3, "D":4, "E":5, "F":6, "G":7, "H":8,
            "I":9, "J":10, "K":11, "L":12, "M":13, "N":14, "O":15, "P":16,
            "Q":17, "R":18, "S":19, "T":20, "U":21, "V":22, "W":23, "X":24,
            "Y":25, "Z":26} #...
    
    for i in range(str_len):
        if i > 0:
            answer += pow(26, i) * my_alp[_str[str_len - i - 1]]
        else:
            answer += my_alp[_str[str_len - i - 1]]
            
    # A B C D E F G H I J   K L  M  N  O  P  Q  R  S  T   U   V   W   X    Y    Z
    # 1 2 3 4 5 6 7 8 9 10 11 12 13 14 15 16 17 18 19 20  21  22  23  24   25   26

    return answer



_str = "BA"
print(test(_str))
