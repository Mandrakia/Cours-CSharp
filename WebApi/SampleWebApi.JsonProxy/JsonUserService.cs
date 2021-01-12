using SampleWebApi.Common.Entities;
using SampleWebApi.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace SampleWebApi.JsonProxy
{
    public class JsonUserService : IUserService
    {
        public HttpClient HttpClient { get; set; }

        public JsonUserService(IHttpClientFactory factory) //Expliqué plus tard
        {
            if(factory != null)
            HttpClient = factory.CreateClient("JsonPlaceholder"); //Expliqué plus tard
        }
        public Task CreateUser(User usr)
        {
            throw new NotImplementedException();
        }

        public Task DeleteUser(User usr)
        {
            throw new NotImplementedException();
        }

        public async Task<User> GetFullUser(int id)
        {
            var user = await HttpClient.GetFromJsonAsync<User>($"users/{id}");
            user.Posts = await HttpClient.GetFromJsonAsync<List<Post>>($"users/{id}/posts");
            return user;
        }

        public Task<List<User>> GetShortUsers()
        {
            return HttpClient.GetFromJsonAsync<List<User>>("users");
        }

        public Task UpdateUser(User usr)
        {
            throw new NotImplementedException();
        }
    }
}
