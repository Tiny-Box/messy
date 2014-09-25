#include "BTREE.h"
#include <stdlib.h>
#include <stdio.h>

BTREE insert(BTREE root, int val)
{
	BTREE newnode;
	BTREE ptr;
	BTREE back;

	newnode = (BTREE)malloc(sizeof(TREENODE));
	newnode->data = val;
	newnode->left = NULL;
	newnode->right = NULL;

	if (root == NULL)
	{
		return newnode;
	}
	else
	{
		ptr = root;
		while(ptr != NULL)
		{
			back = ptr;
			if (ptr->data > val)
			{
				ptr = ptr->left;
			}
			else if(ptr->data < val)
			{
				ptr = ptr->right;
			}
			else
			{
				printf("二叉树中已有该值\n");
				return root;
			}
		}
		if(back->data > val)
		{
			back->left = newnode;
		}
		else
		{
			back->right = newnode;
		}
	}
	return root;

}

BTREE create_BST(int *Array, int len)
{
	
	BTREE root = NULL;

	for (int i = 0; i < len; i++)
	{
		printf("%d\t", Array[i]);
		root = insert(root, Array[i]);
	}
	return root;
}

BTREE find_ptr(BTREE ptr, int val, int *pos)
{
	BTREE back;
	back = ptr;
	*pos = 0;

	if(ptr == NULL|| ptr->data == val)
	{
		return NULL;
	}
	while (ptr != NULL)
	{
		if (ptr->data == val)
		{
			return back;
		}
		else
		{
			back = ptr;
			if (ptr->data > val)
			{
				ptr = ptr->left;
				*pos = -1;
			}
			else
			{
				ptr= ptr->right;
				*pos = 1;
			}
		}
	}
	printf("二叉树中没有该值.");
	return NULL;
}

BTREE find_btree(BTREE ptr,int val,int *pos)
{
    BTREE pre;
    pre = ptr;
    *pos = 0;
 
    //pos 0 根节点  -1 左节点  1 右节点
    while(ptr != NULL){
         
        if(ptr->data == val)
            return pre;
        else{
            //返回父节点
            pre = ptr;
            if(ptr->data > val){
                ptr = ptr->left;
                //左子树
                *pos = -1;
            }else{
                ptr = ptr->right;
                //右子树
                *pos = 1;
            }
        }
    }
    return NULL;
}

BTREE del(BTREE root, int val)
{
	BTREE back;
	BTREE ptr;
	int pos;

	back = find_ptr(root, val, &pos);

	switch (pos)
	{
		case -1:
			{
				ptr = back->left;
				if (ptr->left == NULL)
				{
					back->left = ptr->right;
					free(ptr);
					return root;
				}
				if (ptr->right == NULL)
				{
					back->left = ptr->left;
					free(ptr);
					return root;
				}
				break;
			}
		case 1:
			{
				ptr = back->right;
				if (ptr->left == NULL)
				{
					back->right = ptr->right;
					free(ptr);
					return root;
				}
				if (ptr->right == NULL)
				{
					back->right = ptr->left;
					free(ptr);
					return root;
				}
				break;
			}
		default:
			break;
	}

	//if (ptr->left == NULL)
	//{
	//	back->right = ptr->right;
	//	free(ptr);
	//	return root;
	//}

	//if (ptr->right == NULL)
	//{
	//	back->left = ptr->left;
	//	free(ptr);
	//	return root;
	//}

	BTREE temp = ptr->left;
	back = ptr;
	while (temp->right != NULL)
	{
		back = temp;
		temp = temp->right;
	}
	ptr->data = temp->data;
	back->left = temp->left;
	free(temp);
	return root;
	
}


void Preorder(BTREE temp)
{
	if(temp != NULL)
	{
		printf("%d%s", temp->data, " ");
		Preorder(temp->left);
		Preorder(temp->right);
	}
}


int main()
{
	int Array[]={7,4,2,3,15,35,6,5,45,55,20,22,16,1,14,56,57,58};
    int k;
	BTREE root = NULL;
    k=sizeof(Array)/sizeof(Array[0]);
    printf("建立排序二叉树顺序: \n");
	for (int i = 0; i < k; i++)
	{
		//printf("%d\t", Array[i]);
		root = insert(root, Array[i]);
	}

	Preorder(root);
	printf("\n");

	//int pos;
	//BTREE temp = find_ptr(root, 20, &pos);
	//printf("%d\t", temp->data);
	//printf("%d", pos);
	root = del(root, 6);
	Preorder(root);
	//root = insert(root, 15);
	//Preorder(root);

	//int pos;
	//BTREE temp = NULL;
	//temp = find_ptr(root, 6, &pos);
	//if (temp != NULL)
	//{
	//	printf("%d", temp->data);
	//}
	//root = create_BST(Array, k);

	system("Pause");
}