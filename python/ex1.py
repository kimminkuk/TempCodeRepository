import random
END_NUM = 31
PLAYER_DEFEAT = 1
COM_DEFEAT = 2

def winner_print(winner):
    if winner == COM_DEFEAT:
        print("플레이어 승리!")
    else:
        print("컴퓨터 승리!")

def myValueError():
    print("숫자로 변경할 수 없습니다.")
    print("다시 입력해주세요.")
    return

def myBsError():
    print("입력 가능한 숫자를 해주세요.")
    print("다시 입력해주세요.")
    return

def print_current_num(num):
    if num != False:
        print("현재 숫자: ", num)
    return

def player_input(cur_num):
    check = True
    while check:
        try:
            my_number = list(input("My turn - 숫자를 입력하세요: ").split())
            my_number = [int(num) for num in my_number]
            my_number_copy = sorted(my_number)
            if my_number[0] <= cur_num or my_number != my_number_copy or max(my_number) > END_NUM:
                myBsError()
            else:
                check = False
                
        except:
            myValueError()
            
    return my_number[-1]

def com_input(cur_num):
    com_num = cur_num
    com_range = random.randint(1,3) 
    for com in range(1, com_range + 1): #1,2,3
        print("컴퓨터: ", com_num + com)
        if com_num + com >= END_NUM:
            return False
              
    return com_range + cur_num

def bs31():
    bs_num = 0
    while bs_num < END_NUM:
        bs_num = player_input(bs_num)
        print_current_num(bs_num)
        if bs_num >= END_NUM or bs_num == False:
            winner_print(PLAYER_DEFEAT)
            break

        bs_num = com_input(bs_num) 
        print_current_num(bs_num)
        if bs_num >= END_NUM or bs_num == False:
            winner_print(COM_DEFEAT)
            break

bs31()

#베스킨라빈스 써리원 게임
#------------------
#My turn - 숫자를 입력하세요: 1 2 3
#현재 숫자 : 3
#컴퓨터 : 4
#현재 숫자 : 4
#My turn - 숫자를 입력하세요: 5
#현재 숫자 : 5
#컴퓨터 : 6
#컴퓨터 : 7
#현재 숫자 : 7