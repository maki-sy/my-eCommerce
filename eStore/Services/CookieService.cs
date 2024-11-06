using Newtonsoft.Json;

namespace eStore.Services
{
    public class CookieService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CookieService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public T GetObjectFromJson<T>(string key)
        {
            var cookieValue = _httpContextAccessor.HttpContext.Request.Cookies[key];

            if (string.IsNullOrEmpty(cookieValue))
                return default(T);

            return JsonConvert.DeserializeObject<T>(cookieValue);
        }
        public void AddObjectToListInCookie<T>(string key, T newObject, int? expireTime)
        {
            // Get the current list from the cookie (if exists)
            var currentList = GetObjectFromJson<List<T>>(key) ?? new List<T>();

            // Add the new object to the list
            currentList.Add(newObject);

            // Serialize the updated list and store it back to the cookie
            var options = new CookieOptions
            {
                Expires = expireTime.HasValue ? DateTime.Now.AddMinutes(expireTime.Value) : DateTime.Now.AddMinutes(60)
            };
            var jsonValue = JsonConvert.SerializeObject(currentList);
            _httpContextAccessor.HttpContext.Response.Cookies.Append(key, jsonValue, options);
        }
        public List<T> GetObjectListFromCookie<T>(string key)
        {
            var cookieValue = _httpContextAccessor.HttpContext.Request.Cookies[key];

            if (string.IsNullOrEmpty(cookieValue))
                return new List<T>(); // Return an empty list if no cookie exists

            return JsonConvert.DeserializeObject<List<T>>(cookieValue);
        }
        public void ClearCookie(string key)
        {
            var options = new CookieOptions
            {
                Expires = DateTimeOffset.UtcNow.AddDays(-1) // Set expiration to the past
            };
            _httpContextAccessor.HttpContext.Response.Cookies.Append(key, "", options);
        }
    }
}
