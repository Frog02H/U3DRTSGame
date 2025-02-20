using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyObjectPool
{
    public class PoolBase<T>
    {
        #region fields

        private readonly Func<T> _preloadFunc;
        //预加载的泛式函数
        private readonly Action<T> _getAction;
        //对象在对象池外使用的泛式函数
        private readonly Action<T> _returnAction;
        //对象回归对象池的泛式函数

        /*
         Func<Result>有返回类型；
         Action<T>只有参数类型，不能传返回类型。所以Action<T>的委托函数都是没有返回值的。
         Action本质上其实是Func的一种。
        */

        private Queue<T> _pool = new Queue<T>();
        //对象池存储泛式对象的队列，池中对象
        private List<T> _active = new List<T>();
        //活动池存储泛式对象的队列，活跃对象

        #endregion fields

        #region constructor

        //注下,相当于PoolBase的Init方法
        public PoolBase(Func<T> preloadFunc, Action<T> getAction, Action<T> returnAction, int _preloadCount)
        {
            _preloadFunc = preloadFunc;
            _getAction = getAction;
            _returnAction = returnAction;
            if(preloadFunc == null)
            {
                Debug.LogError("preload function is null");
                return;
            }

            for(int i = 0; i < _preloadCount; i++)
            {
                Return(preloadFunc());
                //提前塞满pool这个对象队列，有助于游戏使用
            }
        }

        #endregion fields

        #region public Methods
        
        //注下,相当于PoolBase的Get方法，从Pool中取所需的对象
        public T Get()
        {
            T item = _pool.Count > 0 ? _pool.Dequeue() : _preloadFunc();
            /* 
                若pool队列中有元素则取出，若无，则执行preloadFunc，大概率是往pool队列中加一个刚new的元素
                使用Count属性获取当前队列内元素的数量。
                Dequeue方法用于从队列的开头（即队首）移除并返回元素，同时改变队列状态。 
            */
            _getAction(item);
            //激活这个元素，供游戏使用
            return item;
            //返回这个对象
        }

        //注下,相当于PoolBase的Set方法，将不销毁就浪费的对象还给Pool
        public void Return(T item)
        {
            _returnAction(item);
            //取消这个元素的激活，游戏就用不了
            _pool.Enqueue(item);
            /*
            pool队列回收这个item。
            使用Enqueue方法将元素添加到队列的末尾（即队尾）。
            */
            _returnAction(item);
            //对象池实际取回这个对象
        }

        //注下,相当于PoolBase的全取回方法（一般用在游戏结束）
        public void ReturnAll()
        {
            foreach(T item in _active.ToArray())
            {
                Return(item);
            }
        }

        #endregion public Methods
    }
}
