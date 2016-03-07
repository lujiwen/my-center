using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;

namespace WpfApplication2.Model.Vo
{
    public class QueueFixedLength<T>
    {
        private ObservableCollection<T> queue;
        private int capacity ;
        public int Capacity { get { return capacity; } set { capacity = value; } }
        public ObservableCollection<T> Queue { get { return queue; } set { } }
        public QueueFixedLength(int length)
        {
            capacity = length;
            queue = new ObservableCollection<T>();
        }

        public int add(T o)
        {
            if (queue.Count >= capacity)
            {
                for (int i = 1; i < capacity;i++ )
                {
                    queue[i - 1] = queue[i];
                }
                queue[capacity-1] = o;
            }
            else
            {
                queue.Add(o);
            }
            return queue.Count;
        }
    }
}
