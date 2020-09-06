from typing import List

from test_framework import generic_test


def intersect_two_sorted_arrays(A: List[int], B: List[int]) -> List[int]:
    # TODO - you fill in here.
    result: List[int] = []

    i, j, n, m = 0, 0, len(A), len(B)
    while i < n and j < m:
        if A[i] < B[j]:
            i += 1
        elif A[i] == B[j]:
            if not result or result[-1] != A[i]:
                result.append(A[i])
            i, j = i + 1, j + 1
        else:
            j += 1

    return result


if __name__ == '__main__':
    exit(
        generic_test.generic_test_main('intersect_sorted_arrays.py',
                                       'intersect_sorted_arrays.tsv',
                                       intersect_two_sorted_arrays))
