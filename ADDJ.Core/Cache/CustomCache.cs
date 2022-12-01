using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace ADDJ.Core.Cache
{
    public class CustomCache
    {
        static Cache<string, object> _instance;
        static Cache<string, object> Instance
        {
            get { return _instance ?? (_instance = new Cache<string, object>(3600)); }
        }

        public static void Add(string key, object value)
        {
            Instance.Put(key, value);

        }

        public static void Add(string key, object value, int timeOut)
        {
            Instance.Put(key, value, timeOut);
        }

        public static void Update(string key, object value)
        {
            Instance.Update(key, value);
        }

        public static void Update(string key, object value, int timeOut)
        {
            Instance.Update(key, value, timeOut);
        }

        public static bool Exists(string key)
        {
            return Instance.Contains(key);
        }

        public static Dictionary<string, object> GetList()
        {
            return Instance.GetList();
        }

        public static int Count()
        {
            return Instance.Count();
        }

        public static bool Remove(string key)
        {
            return Instance.Remove(key);
        }

        public static void RemoveAll()
        {
            Instance.RemoveAll();
        }

        public static object Get(string key)
        {
            if (Exists(key))
            {
                return Instance.Get(key);
            }
            return null;
        }
    }
    public enum CacheEventType
    {
        ADD,
        REMOVE
    }
    public class Cache<K, V>
    {
        public const int CheckInterval = 1000;
        private int _timeoutSeconds;
        private SortedList<Timeout, K> timeouts = new SortedList<Timeout, K>();
        private Dictionary<K, V> items = new Dictionary<K, V>();
        private Timer timer;
        private object locker = new object();
        public delegate void CacheEventHandler(object source, CacheEvent<K, V> e);
        public event CacheEventHandler changed;
        public Cache(int timeoutSeconds) : this(timeoutSeconds, CheckInterval) { }
        public Cache(int timeoutSeconds, int checkInterval)
        {
            this._timeoutSeconds = timeoutSeconds;
            timer = new Timer(PurgeCache, "Cache", checkInterval, checkInterval); //purge cache
        }
        private void PurgeCache(object data)
        {
            lock (locker)
            {
                for (int i = timeouts.Count - 1; i >= 0; i--)
                {
                    if (timeouts.Keys[i].IsExpired())
                    {
                        K key = timeouts.Values[i];
                        timeouts.RemoveAt(i);
                        V value = items[key];
                        items.Remove(key);
                        FireCacheEvent(key, value, CacheEventType.REMOVE);
                    }
                    else
                    {
                        break; //don't need to loop further
                    }
                }
            }
        }

        public int Count()
        {
            return items.Count;
        }

        public Dictionary<K, V> GetList()
        {
            return items;
        }

        public void RemoveAll()
        {
            items.Clear();
        }

        public void Put(K key, V value)
        {
            Put(key, value, _timeoutSeconds);
        }

        public void Put(K key, V value, int timeoutSeconds)
        {
            lock (locker)
            {
                timeouts.Add(new Timeout(timeoutSeconds), key);
                items.Add(key, value);
                FireCacheEvent(key, value, CacheEventType.ADD);
            }
        }

        public void Update(K key, V value)
        {
            Update(key, value, _timeoutSeconds);
        }

        public void Update(K key, V value, int timeoutSeconds)
        {
            lock (locker)
            {
                if (items.ContainsKey(key))
                {
                    items[key] = value;
                }
                else
                {
                    timeouts.Add(new Timeout(timeoutSeconds), key);
                    items.Add(key, value);
                    FireCacheEvent(key, value, CacheEventType.ADD);
                }
            }
        }

        public V Get(K key)
        {
            lock (locker)
            {
                if (items.ContainsKey(key))
                {
                    return items[key];
                }
                return default(V);
            }
        }

        public bool Remove(K key)
        {
            lock (locker)
            {
                if (items.ContainsKey(key))
                {
                    return items.Remove(key);
                }
                return false;
            }
        }
        public bool Contains(K key)
        {
            lock (locker)
            {
                return items.ContainsKey(key);
            }
        }
        private void FireCacheEvent(K key, V value, CacheEventType type)
        {
            if (changed != null)
            {
                changed(this, new CacheEvent<K, V>(key, value, type));
            }
        }
    }
    public class Timeout : IComparable<Timeout>
    {
        private readonly DateTime exitTime;
        public Timeout(int seconds)
        {
            exitTime = DateTime.Now.AddSeconds(seconds);
        }
        public bool IsExpired()
        {
            if (exitTime < DateTime.Now)
            {
                return true;
            }
            return false;
        }
        public int CompareTo(Timeout timeout)
        {
            return timeout.exitTime.CompareTo(exitTime);
        }
        public override string ToString()
        {
            return exitTime.ToString("yyyyMMdd HH:mm:ss");
        }
    }
    public class CacheEvent<K, V> : EventArgs
    {
        private K _key;
        private V _value;
        private readonly CacheEventType _type;
        public CacheEvent(K key, V value, CacheEventType type)
        {
            _key = key;
            _value = value;
            _type = type;
        }
        public K Key
        {
            get { return _key; }
        }
        public V Value
        {
            get { return _value; }
        }
        public CacheEventType Type
        {
            get { return _type; }
        }
    }
}
