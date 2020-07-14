from typing import List
from functools import reduce
from string import digits
from test_framework import generic_test
from test_framework.test_failure import TestFailure


def int_to_string(x: int) -> str:
    # TODO - you fill in here.
    res: List[str] = []
    negative = False
    if x < 0:
        x, negative = -x, True

    ok = True
    while ok:
        res.append(chr(ord('0') + x % 10))
        ok, x = x > 0, x // 10

    return ('-' if negative else '') + ''.join(reversed(res))


def string_to_int(s: str) -> int:
    # TODO - you fill in here.
    """
    res = 0
    negative = False
    for i, ch in enumerate(s):
        if i == 0 and (ch == '-' or ch == '+'):
            negative = ch == '-'
        else:
            res = res * 10 + ord(ch) - ord('0')

    if negative:
        res = -res

    return res
    """
    x = (-1 if s[0] == '-' else 1) * reduce(lambda running_sum, c: running_sum * 10 + digits.index(c),
                                            s[s[0] in '+-':], 0)
    return x


def wrapper(x, s):
    if int(int_to_string(x)) != x:
        raise TestFailure('Int to string conversion failed')
    if string_to_int(s) != x:
        raise TestFailure('String to int conversion failed')


if __name__ == '__main__':
    exit(
        generic_test.generic_test_main('string_integer_interconversion.py',
                                       'string_integer_interconversion.tsv',
                                       wrapper))
