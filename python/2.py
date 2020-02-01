bounds = input().split(" ")
a = int(bounds[0])
b = int(bounds[1])
res = []
for i in range(a,b+1):
    if (i*i % 10**len(str(i)) == i):
        res.append(i)

print(*res, sep=' ')
