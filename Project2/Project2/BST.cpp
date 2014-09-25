//#include "tree.h"
//#include <assert.h>
//#include <stdio.h>
//#include <stdlib.h>
//#include <iostream>
//
//static int n;
//static int m;
//
//btree root;
//
//
//
///*
//    往链表二叉树插入数据
//*/
//btree insertnode(btree root, int val)
//{
//    btree newnode;
//    btree current;
//    btree backnode;
// 
//    newnode = (btree)malloc(sizeof(treenode));
//    newnode->data = val;
//    newnode->right = NULL;
//    newnode->left = NULL;
// 
//    if(root == NULL)
//    {
//        return newnode;
//    }else{
//        current  = root;
//        while(current != NULL)
//        {
//            backnode = current;
//            if(val > current->data)
//                current = current->right;
//            else
//                current = current->left;
//        }
//        if(backnode->data > val)
//            backnode->left = newnode;
//        else
//            backnode->right = newnode;
//    }
//    return root;
//}
// 
////创建链表的二叉树
//btree create_sbtree(int *data, int len)
//{
//    int i;
//    btree root = NULL;
// 
//    for(i=0;i<len;i++)
//    {
//        //printf("%d\t",data[i]);
//        root = insertnode(root,data[i]);
//    }
//    return root;
//}
//
//btree find_btree(btree ptr,int val,int *pos)
//{
//    btree pre;
//    pre = ptr;
//    *pos = 0;
// 
//    //pos 0 根节点  -1 左节点  1 右节点
//    while(ptr != NULL){
//         
//        if(ptr->data == val)
//            return pre;
//        else{
//            //返回父节点
//            pre = ptr;
//            if(ptr->data > val){
//                ptr = ptr->left;
//                //左子树
//                *pos = -1;
//            }else{
//                ptr = ptr->right;
//                //右子树
//                *pos = 1;
//            }
//        }
//    }
//    return NULL;
//}
//
//btree del_btree(btree root,int val)
//{
//    //删除节点的父节点
//    btree pre;
//    //需要删除的节点指针
//    btree ptr;
//    //子节点指针
//    btree next;
//    //删除位置是左子树还是右子树
//    int pos;
// 
//    //查找父节点
//    pre = find_btree(root,val,&pos);
// 
//    //未找到数据
//    if(pre == NULL)
//        return root;
// 
//    //确定需要删除的节点
//    switch(pos){
//        //左子树
//        case -1:
//            ptr = pre->left;
//            break;
//        //root
//        case 0:
//            ptr = pre;
//            break;
//        case 1:
//            ptr = pre->right;
//            break;
//    }
// 
//    //如果没有左子树
//    if(ptr->left == NULL){
//        //是否是根节点
//        if(ptr != pre)
//        {
//            pre->right = ptr->right;
//        }else{
//            root = root->right;
//        }
//        //printf("#%d,%d",pre->data,ptr->data);
//        //if(ptr->right == NULL) printf("ptr->right exits");
//        free(ptr);
//        return root;
//    }
// 
//    //如果没有右子树
//    if(ptr->right == NULL){
//        //是否是根节点
//        if(ptr != pre)
//        {
//            pre->left = ptr->left;
//        }else{
//            root = root->left;
//        }
//        free(ptr);
//        return root;
//    }
// 
//    /*
//    如果有左子树和右子树
//    思路：
//        找到左子树的最右树代替要删除的节点
//        删除next就行
//    */
//    pre = ptr;
//    //寻找左子树的最右叶子节点
//    next = ptr->left;
//    while(next->right != NULL)
//    {
//        pre = next;
//        next = next->right;
//    }
//    //要删除节点的值等于左子树的最右子节点
//    ptr->data = next->data;
//    //printf("ptr->data:%d#,ptr->right->data:%d#ptr->left->data:%d",ptr->data,ptr->right->data,ptr->left->data);
//    if(pre->left == next)
//    {
//        pre->left = next->left;
//    }else{
//        pre->right = next->right;
//    }
//    free(next);
//    return root;
//}
//
//void Preorder(btree temp)    //这是先序遍历二叉树，采用了递归的方法。
//{
//    if(temp!=NULL)
//    {
//        std::cout << temp->data << " ";
//        Preorder(temp->left);
//        Preorder(temp->right);
//    }
//}
//
//void display1() 
//{
//	Preorder(root); 
//	std::cout << std::endl;
//}
//
//void printbtree(btree root)
//{
//    btree ptr;
// 
//    printf("\nroot is :%d\n",root->data);
// 
//    ptr = root->left;
//    printf("输出左子树:\n");
//    while(ptr != NULL)
//    {
//        printf("%d\n",ptr->data);
//        if(ptr->right != NULL)
//        {
//            printf("%d\n",ptr->right->data);
//        }
//        ptr = ptr->left;
//    }
// 
//    ptr = root->right;
//    printf("输出右子树:\n");
//    while(ptr != NULL)
//    {
//        printf("%d\n",ptr->data);
//        if(ptr->left != NULL)
//        {
//            printf("%d\n",ptr->left->data);
//        }
//        ptr = ptr->right;
//    }
//}
//
//int main()
//{
//	//if(find(1))
//	//	std::cout << "1" << std::endl;
//	//int array[]={7,4,2,3,15,35,6,45,55,20,1,14,56,57,58};
// //   int k;
// //   k=sizeof(array)/sizeof(array[0]);
// //   std::cout<<"建立排序二叉树顺序: "<<std::endl;
// //   //for(int i=0;i<k;i++)
// //   //{
// //   //    std::cout<<array[i]<<" ";
// //   //    create_Btree(array[i]);
// //   //}
//	//create_Btree(array, k);
//	//std::cout<<std::endl;
//	//std::cout<<std::endl<<"先序遍历序列: "<<std::endl;
// //   display1();
//
//	//BTREE testP = findP(root,15, 0);
//	//std::cout << testP->data << std::endl;
//	////display1();
//	int data[10] = {5,6,4,8,2,3,7,1,9};
//	int array[]={7,4,2,3,15,35,6,45,55,20,1,14,56,57,58};
//	int k;
//    k=sizeof(array)/sizeof(array[0]);
//    //create a bree
// 
//    root = create_sbtree(array, k);
//	//printbtree(root);
//	display1();
//
//	//btree temp = find_btree(root, 4, 0);
//	//std::cout<< temp->data << std::endl;
//
//	printf("\n");
//    printf("#################删除叶子节点:%d#################\n",8);
//    del_btree(root,15);
//	display1();
//	//printbtree(root);
//
//	system("PAUSE");
//	return 0;
//}