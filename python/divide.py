#!/usr/bin/env python3


def long_division(dividend, divider):
    if divider == 0:
        print("Can't divide by zero")
        return

    if dividend < divider:
        print(dividend, "|", divider, sep="")
        print(dividend, "|", 0, sep="")
        return

    last_printed_pos = 0
    zeros = 0
    idents_count = 0
    position = 0
    result = dividend // divider
    str_result = str(result)
    str_dividend = str(dividend)
    rest = dividend - divider * result

    lower_n = get_next_lower_n(0, str_result, divider)
    position += int_len(lower_n)
    upper_n = int(str_dividend[:position])
    if divider == 1:
        idents_count += 1
    print(dividend, "|", divider, sep="")
    print(lower_n, " " * (int_len(dividend) - int_len(lower_n)), "|", str_result, sep="")
    last_printed_len = int_len(lower_n)

    for i in range(len(str_result) - 1):
        idents_count += int_len(upper_n) - int_len(upper_n - lower_n)
        upper_n = get_next_upper_n(position, upper_n, lower_n, str_dividend)
        lower_n = get_next_lower_n(i + 1, str_result, divider)
        position += 1
        if upper_n != 0 and lower_n != 0:
            if (i != len(str_result) - 2):
                idents_count -= zeros
                zeros = 0
            print(" " * idents_count, upper_n, sep="")
            print(" " * (idents_count + int_len(upper_n) - int_len(lower_n)), lower_n, sep="")
            last_printed_pos = idents_count + int_len(upper_n) - int_len(lower_n)
            last_printed_len = int_len(lower_n)
            if divider == 1:
                idents_count += 1
        elif lower_n == 0:
            zeros += 1
            idents_count += 1

    idents_count += int_len(upper_n) - int_len(upper_n - lower_n)
    if rest != 0:
        print(" " * idents_count, rest, sep="")
    else:
        print(" " * (last_printed_pos + last_printed_len - 1), rest, sep="")


def int_len(a):
    return len(str(a))


def get_next_lower_n(position, str_result, divider):
    return int(str_result[position]) * int(divider)


def get_next_upper_n(position, prev_upper_n, prev_lower_n, str_dividend):
    return int(str(int(prev_upper_n) - int(prev_lower_n)) + str_dividend[position])


def main():
    print(long_division(123, 123))
    print()
    print(long_division(1, 1))
    print()
    print(long_division(15, 3))
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
    print(long_division(472319, 46))
    print()
    print(long_division(425934261694251, 12345678))
    print()
    print(long_division(1000025, 25))


if __name__ == '__main__':
    main()
