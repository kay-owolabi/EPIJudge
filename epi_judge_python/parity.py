from test_framework import generic_test


def parity(x: int) -> int:
    x ^= x >> 32
    x ^= x >> 16
    x ^= x >> 8
    x ^= x >> 4
    x ^= x >> 2
    x ^= x >> 1
    return x & 1


def set_parity(l: list) -> list:
    for i in range(pow(2, 16)):
        l.append(parity(i))
    return l


PRECOMPUTED_PARITY = set_parity([])


def parity(x: int) -> int:
    mask_size = 16
    bit_mask = 0xFFFF
    return (PRECOMPUTED_PARITY[x >> (3 * mask_size)] ^
            PRECOMPUTED_PARITY[(x >> (2 * mask_size)) & bit_mask] ^
            PRECOMPUTED_PARITY[(x >> mask_size) & bit_mask] ^
            PRECOMPUTED_PARITY[x & bit_mask])


if __name__ == '__main__':
    exit(generic_test.generic_test_main('parity.py', 'parity.tsv', parity))
