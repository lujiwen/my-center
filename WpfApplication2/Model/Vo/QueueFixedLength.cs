using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;

namespace WpfApplication2.Model.Vo
{
    public class QueueFixedLength<T>
    {
        private ObservableCollection<T> queue1;
        private ObservableCollection<T> queue2;
        private int capacity ;
        public int Capacity { get { return capacity; } set { capacity = value; } }
        public ObservableCollection<T> Queue { get { return queue2; } set { } }
        public QueueFixedLength(int length)
        {
            capacity = length;
            queue1 = new ObservableCollection<T>();
            queue2 = new ObservableCollection<T>();
        }

        public int add(T o)
        {
            if (queue1.Count >= capacity)
            {
                for (int i = 1; i < capacity;i++ )
                {
                    queue1[i - 1] = queue1[i];
                }
                queue1[capacity-1] = o;
            }
            else
            {
                queue1.Add(o);
            }

            //将队列中的元素倒着放进另一个队列，这样使得在屏幕上打印的信息可以后来的放在上面
            queue2.Clear();
            for(int i=queue1.Count-1;i>=0;i--)
            {
                queue2.Add(queue1[i]);
            }
            return queue2.Count;
        }
    }
}
