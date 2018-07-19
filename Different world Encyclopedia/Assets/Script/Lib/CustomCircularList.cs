
using UnityEngine;

namespace CustomLib
{
    public sealed class CustomCircularQueue<T>
    {
        private class Node
        {
            public T data;
            public Node pLeft;
            public Node pRight;
            public Node(T _data)
            {
                data = _data;
                pLeft = this;
                pRight = this;
            }
        }

        Node head;
        Node tail;
        Node currentData;
        uint sz;
        public CustomCircularQueue()
        {
            head = null;
            tail = null;
            currentData = null;
            sz = 0;
        }

        public void push(T data)
        {
            Node newNode = new Node(data);

            if (head == null)
            {
                currentData = head = newNode;
            } else 
            {
                tail.pRight = newNode;
                newNode.pLeft = tail;

                newNode.pRight = head;
                head.pLeft = newNode;
            }
            tail = newNode;
            sz++;
        }

        public void pop()
        {
            if (head == tail)
            {
                head = tail = null;
            }
            else
            {
                head = head.pRight;
                head.pLeft = tail;

                tail.pRight = head;
            }
            sz--;
        }

        public void goRight()
        {
            currentData = currentData.pRight;
        }

        public void goLeft()
        {
            currentData = currentData.pLeft;
        }

        public T getVaule()
        {
            return currentData.data;
        }
    }
}
