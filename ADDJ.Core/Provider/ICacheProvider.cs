﻿using System.Collections.Generic;

namespace ADDJ.Core.Provider
{
    public interface ICacheProvider
    {
        object Get(string key);
        bool Exists(string key);
        void Add(string key, object value);
        void AddWithTimeOut(string key, object value, int timeout);
        void Update(string key, object value);
        void UpdateWithTimeOut(string key, object value, int timeout);
        void Remove(string key);
        void ClearCache();
        IDictionary<string, object> GetAll();
    }
}
