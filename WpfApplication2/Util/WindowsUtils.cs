using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media;

namespace Project208Home.Views.ArtWorks208
{
    public class WindowsUtils
    {
        public static T GetChildObject<T>(DependencyObject obj, string name) where T : FrameworkElement
        {
            DependencyObject child = null;
            T grandChild = null;

            for (int i = 0; i <= VisualTreeHelper.GetChildrenCount(obj) - 1; i++)
            {
                child = VisualTreeHelper.GetChild(obj, i);

                if (child is T && (((T)child).Name == name | string.IsNullOrEmpty(name)))
                {
                    return (T)child;
                }
                else
                {
                    grandChild = GetChildObject<T>(child, name);
                    if (grandChild != null)
                        return grandChild;
                }
            }
            return null;
        }
        /// <summary>
        /// 如果与上一次点击时间相隔小于指定时间，则点击无效
        /// </summary>
        /// <param name="clickTime"></param>
        public static Boolean setClick(DateTime clickTime, DateTime lastClicktime)
        {
            Boolean canClick = false ;
            TimeSpan time = clickTime - lastClicktime;

            //if (time.TotalSeconds > Constants.PumpCanOperate)//点击时间间隔小于指定点击间隔，则可点击
            //{
            //    canClick = true;
            //    lastClicktime = clickTime;
            //}
            return canClick;
        }
    }
}
