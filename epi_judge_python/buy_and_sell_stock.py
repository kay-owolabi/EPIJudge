from typing import List
from test_framework import generic_test


def buy_and_sell_stock_once(prices: List[float]) -> float:
    # TODO - you fill in here.
    max_profit, min_so_far = 0.0, float('inf')
    for price in prices:
        if price < min_so_far:
            min_so_far = price
        elif max_profit < price - min_so_far:
            max_profit = price - min_so_far
    return max_profit


if __name__ == '__main__':
    exit(
        generic_test.generic_test_main('buy_and_sell_stock.py',
                                       'buy_and_sell_stock.tsv',
                                       buy_and_sell_stock_once))
