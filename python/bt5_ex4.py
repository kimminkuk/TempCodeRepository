from collections import defaultdict

def customer_result(member, member_coupon, member_key):
    print(member)
    for key in member_key:
        print("할인 쿠폰을 받을 회원정보 아이디:")
        print(member_coupon)
    return 

def get_customer_coupon(member, member_key):
    member_vip = []
    for idx, el in enumerate(member[member_key[5]]):
        if int(el) >= 8:
            temp = []
            for key_idx in range(len(member_key)):
                if member_key[key_idx] == '전화번호' and member[member_key[key_idx]][idx] == 'x':
                    temp.append('000-0000-000')
                else:
                    temp.append(member[member_key[key_idx]][idx])
            member_vip.append(temp)
    return member_vip

def good_customer(info):
    d_member = defaultdict(list)
    member_key = ['아이디', '나이', '전화번호', '성별', '지역', '구매횟수']
    try:
        info_split = info.split(',')
    except:
        print("입력 문제 발생")
        return -1

    for idx, el in enumerate(info_split):
        d_member[member_key[idx % 6]].append(el)
    
    coupon_member = get_customer_coupon(d_member, member_key)


    customer_result(d_member, coupon_member, member_key)
    return


info = "abc,21세,010-1234-5678,남자,서울,5,cdb,25세,x,남자,서울,4,bbc,30세,010-2222-3333,여자,서울,3,ccb,29세,x,여자,경기,9,dab,26세,x,남자,인천,8,aab,23세,010-3333-1111,여자,경기,10"
good_customer(info)