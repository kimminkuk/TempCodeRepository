def stock_profic_calc(buy, sell):
    return "{:.2f}".format((sell - buy) / buy * 100)

def stocks_split(stocks):
    try:
        stocks_split = stocks.split(',')
    except:
        return False
    sheet = []
    for stocks_split_line in stocks_split:
        temp = stocks_split_line.split('/')
        temp[2] = int(temp[2])
        sheet.append(temp)
    return sheet

def stock_profit(stocks, sells):
    stocks_sheet = stocks_split(stocks)
    if stocks_sheet == False:
        return 

    d_stocks = {company:price for company, cnt, price in stocks_sheet}
    d_sells = {company[0]:sell_price for company, sell_price in zip(stocks_sheet, sells)}
    for company in d_stocks.keys():
        print(company + "의 수익률 ", stock_profic_calc(d_stocks[company], d_sells[company]))
    
    return 

stocks = "삼성전자/10/85000,카카오/15/130000,LG화학/3/820000,NAVER/5/420000"
sells = [82000, 160000, 835000, 410000]
stock_profit(stocks, sells)