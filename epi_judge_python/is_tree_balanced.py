import collections

from binary_tree_node import BinaryTreeNode
from test_framework import generic_test


def is_height_balanced(left: BinaryTreeNode, right: BinaryTreeNode) -> bool:
    if not left and not right:
        return True
    if not left:
        return right.left is None and right.right is None
    if not right:
        return left.left is None and left.right is None
    return is_height_balanced(right.left, right.right) and is_height_balanced(left.left, left.right)


def is_balanced_binary_tree(tree: BinaryTreeNode) -> bool:
    if not tree:
        return True
    return is_height_balanced(tree.left, tree.right)

    """
    BalancedStatusWithHeight = collections.namedtuple('BalancedStatusWithHeight', ('balanced', 'height'))

    # First value of the return value indicates if tree is balanced, and if
    # balanced the second value of the return value is the height of tree.
    def check_balanced(node):
        if not node:
            return BalancedStatusWithHeight(balanced=True, height=-1)

        left_result = check_balanced(node.left)
        if not left_result.balanced:
            return left_result

        right_result = check_balanced(node.right)
        if not right_result.balanced:
            return right_result

        is_balanced = abs(left_result.height - right_result.height) <= 1
        height = max(left_result.height, right_result.height) + 1
        return BalancedStatusWithHeight(is_balanced, height)

    return check_balanced(tree).balanced
    """


if __name__ == '__main__':
    exit(
        generic_test.generic_test_main('is_tree_balanced.py',
                                       'is_tree_balanced.tsv',
                                       is_balanced_binary_tree))
