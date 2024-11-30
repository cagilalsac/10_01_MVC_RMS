using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace BLL.Services.Bases
{
    /// <summary>
    /// Abstract class for HTTP operations.
    /// </summary>
    public abstract class HttpServiceBase
    {
        // Session is used to store data per client on the server side for a limited time.
        // In ASP.NET Core, only byte[], int and string data can be stored in session.
        // We want to store objects in session therefore we are going to convert objects to JSON strings or vice versa.  
        // HttpContextAccessor injection for making HTTP Context related operations such as session or cookie operations:
        private readonly IHttpContextAccessor _httpContextAccessor;

        protected HttpServiceBase(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }



        /// <summary>
        /// This method first gets the string JSON data from session by the provided key. 
        /// If data is not null or empty, JSON data is deserialized to the object of type T 
        /// and the object is returned.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns>T</returns>
        public virtual T GetSession<T>(string key) where T : class, new()
        {
            T instance = null;
            string json = _httpContextAccessor.HttpContext.Session.GetString(key);
            if (!string.IsNullOrWhiteSpace(json))
                instance = JsonConvert.DeserializeObject<T>(json);
            return instance;
        }

        /// <summary>
        /// This method first serializes the object to a string JSON data then
        /// stores the data in the session by the provided key.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="instance"></param>
        public virtual void SetSession<T>(string key, T instance) where T : class, new()
        {
            string json = JsonConvert.SerializeObject(instance);
            _httpContextAccessor.HttpContext.Session.SetString(key, json);
        }

        /// <summary>
        /// This method removes the stored data in the session with the provided key.
        /// </summary>
        /// <param name="key"></param>
        public virtual void RemoveSession(string key) => _httpContextAccessor.HttpContext.Session.Remove(key);

        /// <summary>
        /// This method removes the stored data in the session with all keys.
        /// </summary>
        public virtual void ClearSession() => _httpContextAccessor.HttpContext.Session.Clear();
    }



    /// <summary>
    /// Concrete class for HTTP operations.
    /// </summary>
    public class HttpService : HttpServiceBase
    {
        public HttpService(IHttpContextAccessor httpContextAccessor) : base(httpContextAccessor)
        {
        }
    }
}
