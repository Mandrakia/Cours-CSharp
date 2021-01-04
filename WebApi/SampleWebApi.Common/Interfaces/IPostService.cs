using SampleWebApi.Common.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SampleWebApi.Common.Interfaces
{
    public interface IPostService
    {
        Task<List<Post>> GetShortPosts();
        Task<Post> GetFullPost(int id);
        Task DeletePost(int id);
        Task UpdatePost(Post post);
        Task CreatePost(Post post);

    }
}
