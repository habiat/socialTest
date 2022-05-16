using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;

namespace Social.Web.Models
{
    public static class UserHelper
    {
        /// <summary>
        /// Clear IMemoryCache
        /// </summary>
        /// <param name="cache">Cache</param>
        /// <exception cref="InvalidOperationException">Unable to clear memory cache</exception>
        /// <exception cref="ArgumentNullException">Cache is null</exception>
        public static void Clear(this IMemoryCache cache)
        {
            if (cache == null)
            {
                throw new ArgumentNullException("Memory cache must not be null");
            }
            else if (cache is MemoryCache memCache)
            {
                memCache.Compact(1.0);
                return;
            }
            else
            {
                MethodInfo clearMethod = cache.GetType().GetMethod("Clear", BindingFlags.Instance | BindingFlags.Public);
                if (clearMethod != null)
                {
                    clearMethod.Invoke(cache, null);
                    return;
                }
                else
                {
                    PropertyInfo prop = cache.GetType().GetProperty("EntriesCollection", BindingFlags.Instance | BindingFlags.GetProperty | BindingFlags.NonPublic | BindingFlags.Public);
                    if (prop != null)
                    {
                        object innerCache = prop.GetValue(cache);
                        if (innerCache != null)
                        {
                            clearMethod = innerCache.GetType().GetMethod("Clear", BindingFlags.Instance | BindingFlags.Public);
                            if (clearMethod != null)
                            {
                                clearMethod.Invoke(innerCache, null);
                                return;
                            }
                        }
                    }
                }
            }

            throw new InvalidOperationException("Unable to clear memory cache instance of type " + cache.GetType().FullName);
        }

        public static ClaimsPrincipal GetClaimsPrincipal(this HttpContext httpContext)
        {
            var principal = httpContext.User as ClaimsPrincipal;
            return principal;
        }
        public static string GetSpecificClaim(this ClaimsIdentity claimsIdentity, string claimType)
        {
            var claim = claimsIdentity.Claims.FirstOrDefault(x => x.Type == claimType);

            return (claim != null) ? claim.Value : string.Empty;
        }
        public static string GetEmail(this ClaimsPrincipal claimsPrincipal)
            => claimsPrincipal.FindFirstValue(ClaimTypes.Email);

        public static string GetFirstName(this ClaimsPrincipal claimsPrincipal)
            => claimsPrincipal.FindFirstValue(ClaimTypes.Name);

        public static string GetLastName(this ClaimsPrincipal claimsPrincipal)
            => claimsPrincipal.FindFirstValue(ClaimTypes.Surname);

        public static int GetUserId(this ClaimsPrincipal claimsPrincipal)
            => Convert.ToInt32(claimsPrincipal.FindFirstValue(ClaimTypes.NameIdentifier));
        public static string GetGuid(this ClaimsPrincipal claimsPrincipal)
            => claimsPrincipal.FindFirstValue("UserGuid");
       
     
    }
}
