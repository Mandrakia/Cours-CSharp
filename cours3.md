# Accès base de donnée et première Web API



### Structure d'une application réelle .Net

Jusqu'à présent nous n'avons travaillé que sur des applications console Mono Projet. En .Net et dans tous les langages le but est d'avoir des composants/bouts de code réutilisable dans plusieurs applications ou projets.

Pour ce faire en C# il existe un type de projet `library`

Pour la suite de ce cours il est grandement recommandé d'utiliser Visual Studio, cela évitera de nombreuses commandes consoles.
Nous allons quand même les détailler ici

```powershell
mkdir WebApi
cd WebApi
dotnet new sln -n SampleWebApi
dotnet new classlib -o SampleWebApi.Common
dotnet new classlib -o SampleWebApi.PostgreSql
dotnet new classlib -o SampleWebApi.JsonProxy
dotnet new webapi -o SampleWebApi.WebApi
dotnet sln SampleWebApi.sln add SampleWebApi.Common\SampleWebApi.Common.csproj --solution-folder Libraries
dotnet sln SampleWebApi.sln add SampleWebApi.PostgreSql\SampleWebApi.PostgreSql.csproj --solution-folder Libraries
dotnet sln SampleWebApi.sln add SampleWebApi.JsonProxy\SampleWebApi.JsonProxy.csproj --solution-folder Libraries
dotnet sln SampleWebApi.sln add SampleWebApi.WebApi\SampleWebApi.WebApi.csproj

```

Ce qui devrait vous donner ceci dans Visual Studio : 

![image-20210102131611459](https://i.imgur.com/v5PKSNP.png)

Dans `Common`nous mettrons les modèles et les interfaces utilisées
Dans `JsonProxy` nous mettrons notre provider développé a l'exercice 4
Dans `PostgreSql` nous mettrons notre premier projet utilisant **Entity Framework**

Et enfin dans `WebApi` se trouvera nos routes, nos contrôleurs etc.

### Première étape définir les modèles

Avant de coder quoique ce soit il est essentiel de définir quels sont les objets que nous allons manipuler en base de donnée (Sql,Json,Csv ou autre)
Ici nous allons utiliser le jeu de donnée utilisé précédemment dans l'exercice 4 donc les modèles devraient déja être faits mais au cas où :

#### User.cs

```c#
namespace SampleWebApi.Common.Entities
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public Address Address { get; set; }
        public string Phone { get; set; }
        public string Website { get; set; }
        public Company Company { get; set; }
    }
    public class Address
    {
        public string Street { get; set; }
        public string Suite { get; set; }
        public string City { get; set; }
        public string ZipCode { get; set; }
        public GeoLocation Geo { get; set; }
    }
    public class GeoLocation
    {
        public decimal Lat { get; set; }
        public decimal Lng { get; set; }
    }
    public class Company
    {
        public string Name { get; set; }
        public string CatchPhrase { get; set; }
        public string Bs { get; set; }
    }
}
```

#### Post.cs

```c#
namespace SampleWebApi.Common.Entities
{
    public class Post
    {
        public int UserId { get; set; }
        public int Id { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
    }
}
```

#### Comment.cs

```c#
namespace SampleWebApi.Common.Entities
{
    public class Comment
    {
        public int PostId { get; set; }
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Body { get; set; }
    }
}
```

Ce sont les modèles tels que trouvés dans l'API REST Json que nous avons utilisé pour l'exercice 4.

Nous allons les modifier un peu pour rajouter ce que l'on appelle des propriétés de navigation.
Par exemple un utilisateur est l'auteur de plusieurs posts. Donc nous allons ajouter une propriété à la classe User

```c#
public List<Post> Posts {get;set;}
```

Idem un post a plusieurs commentaires donc 

```c#
public List<Comment> Comments {get;set;}
```

Un post a un auteur donc : 

```c#
public User User {get;set;}
```

Et un commentaire a un post donc 

```c#
public Post Post {get;set;}
```



### Interfaces

Maintenant que nous avons nos modèles nous allons définir quelles actions nous souhaitons réaliser

#### `IUserService.cs`

```c#
namespace SampleWebApi.Common.Interfaces
{
    public interface IUserService
    {
        Task<List<User>> GetShortUsers();
        Task<User> GetFullUser(int id);
    }
}
```

#### `IPostService.cs`

```c#
namespace SampleWebApi.Common.Interfaces
{
    public interface IPostService
    {
        Task<List<Post>> GetShortPosts();
        Task<Post> GetFullPost(int id);
    }
}
```

Par convention les interfaces commencent par la lettre i majuscule. **I**Disposable **I**Enumerable etc.

Notre projet `Common`devrait ressembler à cela maintenant : 

![image-20210102150155976](https://i.imgur.com/i9qZZyL.png)

### Implémentation de l'interface dans JsonProxy

#### `JsonUserService.cs`

```c#
namespace SampleWebApi.JsonProxy
{
    public class JsonUserService : IUserService
    {
        public HttpClient HttpClient {get;set;}
        
        public JsonUserService(IHttpClientFactory factory) //Expliqué plus tard
        {
            HttpClient = factory.CreateClient("JsonPlaceholder"); //Expliqué plus tard
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
    }
}
```

#### `JsonPostService.cs`

```c#
namespace SampleWebApi.JsonProxy
{
    public class JsonPostService : IPostService
    {
        public JsonPostService(IHttpClientFactory factory)
        {
            HttpClient = factory.CreateClient("JsonPlaceholder");
        }

        public HttpClient HttpClient { get; }

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
    }
}
```

### Notre WebAPI : ENFIN !

#### Structure d'un projet WebApi ou Asp.Net en général :

![image-20210102151756096](https://i.imgur.com/f2i0Ivh.png)

Le fichier program : n'y touchons pas pour le moment(voir jamais) on peut y ajouter des options telles que le nombre max de connections simultanées etc mais ça sera pour bien bien plus tard lorsque vous voudrez fine tune votre application en production.

le fichier `startup.cs`

```c#
namespace SampleWebApi.WebApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        //Va configurer tout ce qui est service et injection de dépendances.
        public void ConfigureServices(IServiceCollection services)
        {
			//Essentiel : Va configurer/enregistrer nos controlleurs
            services.AddControllers();
            //Swagger, toujours utile.
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "SampleWebApi.WebApi", Version = "v1" });
            });
        }
		//Va configurer tout ce qui est dans le pipeline/intercepteur dans les requetes HTTP
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            //Bloc pour l'environnement de dev
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "SampleWebApi.WebApi v1"));
            }

            app.UseHttpsRedirection();

            

            //Essentiel : Va mapper les ends points avec les bons controlleurs.
            app.UseRouting();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
```

Ensuite il reste les controllers