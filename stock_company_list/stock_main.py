import company_konex_list_221230
import company_konex_code_list_221230
import company_list_221230
import company_code_221230

from collections import defaultdict

# 파이썬 클래스 예제 코드
class stockMain:
    def __init__(self):
        self.company_list = company_list_221230.company_names
        self.company_code = company_code_221230.company_code_list
        self.company_konex_list = company_konex_list_221230.company_konex_list
        self.company_konex_code_list = company_konex_code_list_221230.company_konex_code_list
        self.result_company_name_list = []
        self.result_company_code_list = []

    def restore_company_code_six_digit(self, restore_company_code_list):
        for idx, company_code in enumerate(restore_company_code_list):
            cur_len = len(company_code)
            if cur_len != 6:
                add_len = 6 - cur_len
                restore_company_code_list[idx] = ( ( '0' * add_len ) + company_code)
        return
        
    def get_company_list(self):
        
        company_name_dict = defaultdict(list)
        
        for company_name in self.company_konex_list:
            company_name_dict[company_name] += 1        
        
        for company_name in self.company_list:
            company_name_dict[company_name] += 1
            
        for company_name in company_name_dict:
            if company_name_dict[company_name] == 1:
                self.result_company_name_list.append(company_name)
        return
    

my_stock_main = stockMain()
my_stock_main.restore_company_code_six_digit(my_stock_main.company_code)
my_stock_main.restore_company_code_six_digit(my_stock_main.company_konex_code_list)

for company_name in my_stock_main.company_code:
    print(company_name, 'company_code')

for company_name in my_stock_main.company_konex_code_list:
    print(company_name, 'company_konex_code')


