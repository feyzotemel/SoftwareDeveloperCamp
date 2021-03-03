using Core.Utilities.IoC;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using System.Text.RegularExpressions;
using System.Linq;

namespace Core.CrossCuttingConcerns.Caching.Microsoft
{
    public class MemoryCacheManager : ICacheManager
    {

        //Adapter pattern:var olan sistemi kendi sistemine göre uyarlamaktır.
        //Kod içinde de _memoryCache.Set vs kullanılabilir ama yarın birgün 
        //cache manageri microsoft haricinde birşeye geçirmek istediğimizde kolay olur.
        IMemoryCache _memoryCache;
        public MemoryCacheManager()
        {
            //GetService kırmızı olursa business e nuget paket olarak 
            //Microsoft.Extensions.DependencyInjection kur ve yukarıya elinle 
            //"using Microsoft.Extensions.DependencyInjection;" ekle lamba çıkmıyor.
            _memoryCache = ServiceTool.ServiceProvider.GetService<IMemoryCache>();
        }
        public void Add(string key, object value, int duration)
        {
            _memoryCache.Set(key, value, TimeSpan.FromMinutes(duration));
        }

        public T Get<T>(string key)
        {
            return _memoryCache.Get<T>(key);
        }

        public object Get(string key)
        {
            return _memoryCache.Get(key);
        }

        public bool IsAdd(string key)
        {
            //"Out _" demek cache de bool dışında bir değer çıktı verme anlamına gelir
            return _memoryCache.TryGetValue(key, out _);
        }

        public void Remove(string key)
        {
            _memoryCache.Remove(key);
        }

        public void RemoveByPattern(string pattern)
        {
            //Ezbere bilmek gerekmez. MemoryCache in dökümantasyonundan bakılabilir.
            var cacheEntriesCollectionDefinition = typeof(MemoryCache).GetProperty("EntriesCollection", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            //GetService kırmızı olursa business e nuget paket olarak 
            //Microsoft.Extensions.DependencyInjection kur ve yukarıya elinle 
            //"using Microsoft.Extensions.DependencyInjection;" ekle lamba çıkmıyor.
            var cacheEntriesCollection = cacheEntriesCollectionDefinition.GetValue(_memoryCache) as dynamic;
            List<ICacheEntry> cacheCollectionValues = new List<ICacheEntry>();

            foreach (var cacheItem in cacheEntriesCollection)
            {
                ICacheEntry cacheItemValue = cacheItem.GetType().GetProperty("Value").GetValue(cacheItem, null);
                cacheCollectionValues.Add(cacheItemValue);
            }

            var regex = new Regex(pattern, RegexOptions.Singleline | RegexOptions.Compiled | RegexOptions.IgnoreCase);
            var keysToRemove = cacheCollectionValues.Where(d => regex.IsMatch(d.Key.ToString())).Select(d => d.Key).ToList();

            foreach (var key in keysToRemove)
            {
                _memoryCache.Remove(key);
            }
        }
    }
}
