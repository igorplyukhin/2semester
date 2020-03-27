import sys

sys.setrecursionlimit(10000)

def cache(f):
    d = {}

    def new_f(n):
        if n not in d:
            d[n] = f(n)
        return d[n]

    return new_f

@cache
def fib(n):
    if n < 2:
        return 1
    return fib(n-1) + fib(n-2)

print(fib(1000))