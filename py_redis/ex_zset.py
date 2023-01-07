import redis
import datetime

print("make Company table")
# 2500개의 회사를 만든다.
company_max = 2500
company_ex_info = "x/" * 16 + "x"
company_redis_list = []
company_redis_dict = {}
for i in range(1, company_max + 1):
    #company_redis_list.append(["company:" + str(i) + "&" + company_ex_info])
    #company_redis_dict["company:" + str(i)] = company_ex_info
    company_redis_dict["company:" + str(i) + "&" + company_ex_info] = i

print("python redis start")

rd = redis.Redis(host='localhost', port=6379, db=0)

# 파이썬 현재 날짜 가져오기 코드
now = datetime.datetime.now()
cur_date = now.strftime('%Y-%m-%d')

# cur_date = "2023-01-08"


# python redis zset example code
# company_redis_list add code
#rd.zadd(cur_date, *company_redis_list)
rd.zadd(cur_date, company_redis_dict)

# and read code
#read_company_redis_list = rd.zrange(cur_date, 0, -1, withscores=True)
read_company_redis_dict = rd.zrange(cur_date, 0, -1, withscores=True)

file_name = "company_redis_dict_" + cur_date + ".txt"
file = open(file_name, "w")

#read_company_redis_dict의 데이터 읽어오기
for idx, value in enumerate(read_company_redis_dict):
    file.write(str(idx) + " " + str(value)+"\n")
file.close()

print("python redis end")