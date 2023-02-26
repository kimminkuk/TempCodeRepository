import redis
r = redis.Redis(host='localhost', port=6379, db=0)

# ip_address = ['192.168.4.110']
# file_location = ['home/syssw/mkkim/user_firm_sh']

# r.hset('yikc-barrack', 'ip_address', ip_address[0])
# r.hset('yikc-barrack', 'file_location', file_location[0])
ip_address_list = []
new_ip = '192.168.17.158'
print_ip_address = r.hget('yikc-barrack', 'ip_address')
print_file_location = r.hget('yikc-barrack', 'file_location')

ip_address_list = print_ip_address.decode('utf-8').split(',')
delete_ip = '192.168.17.159'

while delete_ip in ip_address_list:
    ip_address_list.remove(delete_ip)

r.hset('yikc-barrack', 'ip_address', ','.join(ip_address_list))