#!/usr/bin/env python

import sys


def f0():
    1//0


def f1():
    1//0


def f2():
    1//0


def f3():
    raise FloatingPointError


def f4():
    31231.9 ** 52315231


def f5():
    1//0


def f6():
    assert False


def f7():
    str.hdghs


def f8():
    raise EnvironmentError


def f9():
    import gasg


def f10():
    s=[]
    s[1]


def f11():
    s=[]
    s[1]


def f12():
    a={}
    a['gasg']


def f13():
    fsdF()


def f14():
    print('f';';fd')


def f15():
    raise ValueError


def f16():
    raise UnicodeError


def check_exception(f, exception):
    try:
        f()
    except exception:
        pass
    else:
        print("Bad luck, no exception caught: %s" % exception)
        sys.exit(1)


check_exception(f0, BaseException)
check_exception(f1, Exception)
check_exception(f2, ArithmeticError)
check_exception(f3, FloatingPointError)
check_exception(f4, OverflowError)
check_exception(f5, ZeroDivisionError)
check_exception(f6, AssertionError)
check_exception(f7, AttributeError)
check_exception(f8, EnvironmentError)
check_exception(f9, ImportError)
check_exception(f10, LookupError)
check_exception(f11, IndexError)
check_exception(f12, KeyError)
check_exception(f13, NameError)
check_exception(f14, SyntaxError)
check_exception(f15, ValueError)
check_exception(f16, UnicodeError)

print("Congratulations, you made it!")
