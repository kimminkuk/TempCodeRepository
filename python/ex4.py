from itertools import accumulate
from bisect import bisect_left

def after_100(month, day, word):
    calendar_day = ["월", "화", "수", "목", "금", "토", "일"]
    calendar = [31, 28, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31]
    calendar_sum = list(accumulate(calendar))
    now_time = calendar_sum[month-1] - calendar[month-1] + day
    future_time = now_time + 99

    pos = bisect_left(calendar_sum, future_time)
    future_day = future_time - calendar_sum[pos-1]
    print(month,"월",day,"일",pos+1,"월",future_day,"일")
    return calendar_sum

after_100(6, 21, "월")