﻿using SampleWebApi.JsonProxy;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace SampleWebApi.Test
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var srv = new JsonUserService();
            var client = new HttpClient()
            {
                BaseAddress = new Uri("https://jsonplaceholder.typicode.com")
            };
            srv.HttpClient = client;
            var users = await srv.GetUsers();
            foreach (var usr in users)
                Console.WriteLine(usr.Username);
        }
    }
}
