import heapq
from typing import List, Tuple

from test_framework import generic_test


def merge_sorted_arrays(sorted_arrays: List[List[int]]) -> List[int]:
    # TODO - you fill in here.
    min_heap: List[Tuple[int, int]] = []
    sorted_arrays_iterators = [iter(x) for x in sorted_arrays]

    for i, it in enumerate(sorted_arrays_iterators):
        first_element = next(it, None)
        if first_element is not None:
            heapq.heappush(min_heap, (first_element, i))

    result = []
    while min_heap:
        smallest_entry, i = heapq.heappop(min_heap)
        smallest_array_iter = sorted_arrays_iterators[i]
        result.append(smallest_entry)
        next_element = next(smallest_array_iter, None)
        if next_element is not None:
            heapq.heappush(min_heap, (next_element, i))
    return result


def merge_sorted_arrays_pythonic(sorted_arrays: List[List[int]]) -> List[int]:
    return list(heapq.merge(*sorted_arrays))


if __name__ == '__main__':
    exit(
        generic_test.generic_test_main('sorted_arrays_merge.py',
                                       'sorted_arrays_merge.tsv',
                                       merge_sorted_arrays))
