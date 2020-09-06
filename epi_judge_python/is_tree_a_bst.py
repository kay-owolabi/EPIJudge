from binary_tree_node import BinaryTreeNode
from test_framework import generic_test


def is_binary_tree_bst(tree: BinaryTreeNode) -> bool:
    # TODO - you fill in here.
    def is_valid_bst(node: BinaryTreeNode, min_val=float('-inf'), max_val=float('inf')) -> bool:
        if not node:
            return True
        elif not min_val <= node.data <= max_val:
            return False
        return is_valid_bst(node.left, min_val, node.data) and is_valid_bst(node.right, node.data, max_val)

    return is_valid_bst(tree)


if __name__ == '__main__':
    exit(
        generic_test.generic_test_main('is_tree_a_bst.py', 'is_tree_a_bst.tsv',
                                       is_binary_tree_bst))
