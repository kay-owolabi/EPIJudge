from test_framework import generic_test
import collections


def is_letter_constructible_from_magazine(letter_text: str,
                                          magazine_text: str) -> bool:
    # TODO - you fill in here.
    letter_dic = collections.defaultdict(int)
    magazine_dic = collections.defaultdict(int)

    for text in letter_text:
        letter_dic[text] += 1
    for text in magazine_text:
        magazine_dic[text] += 1

    for text, count in letter_dic.items():
        if magazine_dic[text] < count:
            return False
    return True


if __name__ == '__main__':
    exit(
        generic_test.generic_test_main(
            'is_anonymous_letter_constructible.py',
            'is_anonymous_letter_constructible.tsv',
            is_letter_constructible_from_magazine))
