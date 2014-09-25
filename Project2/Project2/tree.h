#define TREE_TYPE int


struct tree
{
    int data;
    struct tree *left;
    struct tree *right;
};
typedef struct tree treenode;
typedef treenode *btree;

