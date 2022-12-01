using System;
using System.Collections;
using System.Collections.Generic;
using System.Web.Caching;

namespace ADDJ.Core
{
    public static class CacheExtentions
    {
        private static object sync = new object();

        /// <summary>
        /// Phiên bản gốc
        /// Executes a method and stores the result in cache using the given cache key. If the data already exists in cache, it returns the data
        /// and doesn't execute the method. Thread safe, although the method parametersn't guaranteed to be thread safe.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="cache"></param>
        /// <param name="cacheKey">Each method has it's own isolated set of cache items,so cacheKeys won't overlap accross methods.</param>
        /// <param name="method"></param>
        /// <param name="expirationSeconds">Lifetime of cache items, in seconds</param>
        private static T GetData<T>(this System.Web.Caching.Cache cache, string cacheKey, int expirationSeconds, Func<T> method)
        {
            //var hash = method.GetHashCode().ToString();
            //var data = (T)cache[hash + cacheKey];
            cacheKey = cacheKey.ToLower();

            var data = (T)cache[cacheKey];

            if (data == null) // Kiểm tra điều kiện null
            {
                data = method();

                if (expirationSeconds > 0 && data != null)
                    lock (sync)
                    {
                        //cache.Insert(hash + cacheKey, data, null, DateTime.Now.AddSeconds(expirationSeconds), Cache.NoSlidingExpiration);
                        cache.Insert(cacheKey, data, null, DateTime.Now.AddSeconds(expirationSeconds), System.Web.Caching.Cache.NoSlidingExpiration);
                    }
            }
            return data;
        }

        public static T Data<T>(this System.Web.Caching.Cache cache, string cacheKey, int expirationSeconds, Func<T> method)
        {
            // var hash = method.GetHashCode().ToString();
            // var data = (T)cache[hash + cacheKey];
            cacheKey = cacheKey.ToLower();

            bool hasCache = false;
            IDictionaryEnumerator enumerator = cache.GetEnumerator();
            while (enumerator.MoveNext())
            {
                if (enumerator.Key.ToString().ToLower() == cacheKey)
                {
                    hasCache = true;
                    break;
                }
            }
            if (hasCache)
            {
                object obj = cache[cacheKey];
                if (obj == System.DBNull.Value) return default(T);
                return (T)cache[cacheKey]; // Nếu tồn tại cache thì lấy ra
            }
            else // Thực hiện cache nếu không tìm thấy khóa phù hợp
            {
                T data = method();
                if (expirationSeconds <= 0) expirationSeconds = (20 * 60); // 20 minutes

                if (data != null)
                {
                    lock (sync)
                    {
                        //cache.Insert(hash + cacheKey, data, null, DateTime.Now.AddSeconds(expirationSeconds), Cache.NoSlidingExpiration);
                        cache.Insert(cacheKey, data, null, DateTime.Now.AddSeconds(expirationSeconds), System.Web.Caching.Cache.NoSlidingExpiration);
                    }
                }
                else
                {
                    lock (sync)
                    {
                        //cache.Insert(hash + cacheKey, data, null, DateTime.Now.AddSeconds(expirationSeconds), Cache.NoSlidingExpiration);
                        cache.Insert(cacheKey, System.DBNull.Value, null, DateTime.Now.AddSeconds(expirationSeconds), System.Web.Caching.Cache.NoSlidingExpiration);
                    }
                }
                return data;
            }
        }

        public static T Data<T>(this System.Web.Caching.Cache cache, string cacheKey, int expirationSeconds, Func<T> method, CacheItemRemovedCallback methodRemove)
        {
            // var hash = method.GetHashCode().ToString();
            // var data = (T)cache[hash + cacheKey];
            cacheKey = cacheKey.ToLower();

            bool hasCache = false;
            IDictionaryEnumerator enumerator = cache.GetEnumerator();
            while (enumerator.MoveNext())
            {
                if (enumerator.Key.ToString().ToLower() == cacheKey)
                {
                    hasCache = true;
                    break;
                }
            }
            if (hasCache)
            {
                object obj = cache[cacheKey];
                if (obj == System.DBNull.Value) return default(T);
                return (T)cache[cacheKey]; // Nếu tồn tại cache thì lấy ra
            }
            else // Thực hiện cache nếu không tìm thấy khóa phù hợp
            {
                T data = method();
                if (expirationSeconds <= 0) expirationSeconds = (20 * 60); // 20 minutes

                if (data != null) // Có dữ liệu, kể cả giá trị System.DBNull.Value
                {
                    lock (sync)
                    {
                        //cache.Insert(hash + cacheKey, data, null, DateTime.Now.AddSeconds(expirationSeconds), Cache.NoSlidingExpiration);
                        cache.Insert(cacheKey, data, null, DateTime.Now.AddSeconds(expirationSeconds), System.Web.Caching.Cache.NoSlidingExpiration, CacheItemPriority.Default, methodRemove);
                    }
                }

                // Không có dữ liệu => Dùng System.DBNull.Value làm đại diện cache
                else
                {
                    lock (sync)
                    {
                        //cache.Insert(hash + cacheKey, data, null, DateTime.Now.AddSeconds(expirationSeconds), Cache.NoSlidingExpiration);
                        cache.Insert(cacheKey, System.DBNull.Value, null, DateTime.Now.AddSeconds(expirationSeconds), System.Web.Caching.Cache.NoSlidingExpiration, CacheItemPriority.Default, methodRemove);
                    }
                }
                return data;
            }
        }

        public static void RemovePrefix(this System.Web.Caching.Cache cache, string prefix)
        {
            IDictionaryEnumerator enumerator = cache.GetEnumerator();
            while (enumerator.MoveNext())
            {
                if (enumerator.Key.ToString().ToLower().IndexOf(prefix.ToLower()) != -1)
                {
                    cache.Remove((string)enumerator.Key);
                }
            }
        }

        public static void Clear(this System.Web.Caching.Cache cache)
        {
            List<string> keys = new List<string>();
            // retrieve application Cache enumerator
            IDictionaryEnumerator enumerator = cache.GetEnumerator();
            // copy all keys that currently exist in Cache
            while (enumerator.MoveNext())
            {
                keys.Add(enumerator.Key.ToString());
            }
            // delete every key from cache
            for (int i = 0; i < keys.Count; i++)
            {
                cache.Remove(keys[i]);
            }
        }

        public static bool IsExist(this System.Web.Caching.Cache cache, string key)
        {
            IDictionaryEnumerator enumerator = cache.GetEnumerator();
            while (enumerator.MoveNext())
            {
                if (enumerator.Key.ToString().ToLower() == key.ToLower())
                {
                    return true;
                }
            }
            return false;
        }
    }
}
