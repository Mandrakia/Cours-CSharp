using SampleWebApi.Common.Entities;
using SampleWebApi.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace SampleWebApi.JsonProxy
{
    public class JsonPostService : IPostService
    {
        public JsonPostService(IHttpClientFactory factory)
        {
            HttpClient = factory.CreateClient("JsonPlaceholder");
        }

        public HttpClient HttpClient { get; }

        public Task CreatePost(Post post)
        {
            throw new NotImplementedException();
        }

        public Task DeletePost(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<Post> GetFullPost(int id)
        {
            var post = await HttpClient.GetFromJsonAsync<Post>($"posts/{id}");
            post.User = await HttpClient.GetFromJsonAsync<User>($"users/{post.UserId}");
            post.Comments = await HttpClient.GetFromJsonAsync<List<Comment>>($"posts/{id}/comments");
            return post;
        }

        public Task<List<Post>> GetShortPosts()
        {
            return HttpClient.GetFromJsonAsync<List<Post>>("posts");
        }

        public Task UpdatePost(Post post)
        {
            throw new NotImplementedException();
        }
    }
}
