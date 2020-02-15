#!/usr/bin/env python3


def long_division(dividend, divider):
    if divider == 0:
        print("Can't divide by zero")
        return

    if dividend < divider:
        print(dividend, "|", divider, sep="")
        print(dividend, "|", 0, sep="")
        return

    idents_count = 0
    position = 0
    result = dividend // divider
    str_result = str(result)
    str_dividend = str(dividend)
    rest = dividend - divider * result

    lower_n = get_next_lower_n(0, str_result, divider)
    position += int_len(lower_n)
    upper_n = int(str_dividend[:position])
    idents_count += int_len(upper_n) - int_len(upper_n - lower_n)
    if divider == 1:
        idents_count += 1
    print(dividend, "|", divider, sep="")
    print(lower_n, " " * (int_len(dividend) - int_len(lower_n)), "|", str_result, sep="")

    for i in range(len(str_result) - 1):
        upper_n = get_next_upper_n(position, upper_n, lower_n, str_dividend)
        if upper_n != 0:
            position += 1
            print(" " * idents_count, upper_n, sep="")

        lower_n = get_next_lower_n(i + 1, str_result, divider)
        if lower_n != 0:
            print(" " * idents_count, lower_n, sep="")
            idents_count += int_len(upper_n) - int_len(upper_n - lower_n)
            if divider == 1:
                idents_count += 1

        elif str_dividend[i] != 0:
            idents_count += 1
    if rest != 0:
        print(" " * idents_count, rest, sep="")


def int_len(a):
    return len(str(a))


def get_next_lower_n(position, str_result, divider):
    return int(str_result[position]) * int(divider)


def get_next_upper_n(position, prev_upper_n, prev_lower_n, str_dividend):
    return int(str(int(prev_upper_n) - int(prev_lower_n)) + str_dividend[position])


def main():
    print(long_division(1000025, 25))
    print()
    print(long_division(1, 1))
    print()
    print(long_division(12345, 25))
    print()
    print(long_division(3, 15))
    print()
    print(long_division(12345, 25))
    print()
    print(long_division(1234, 1423))
    print()
    print(long_division(87654532, 1))
    print()
    print(long_division(24600, 123))
    print()
    print(long_division(4567, 1234567))
    print()
    print(long_division(246001, 123))
    print()
    print(long_division(100000, 50))
    print()
    print(long_division(123456789, 531))
    print()
    print(long_division(1000025, 25))


if __name__ == '__main__':
    main()
