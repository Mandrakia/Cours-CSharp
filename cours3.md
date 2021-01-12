# Accès base de donnée et première Web API



### Structure d'une application réelle .Net

Jusqu'à présent nous n'avons travaillé que sur des applications console Mono Projet. En .Net et dans tous les langages le but est d'avoir des composants/bouts de code réutilisable dans plusieurs applications ou projets.

Pour ce faire en C# il existe un type de projet `library`

Pour la suite de ce cours il est grandement recommandé d'utiliser Visual Studio, cela évitera de nombreuses commandes consoles.
Nous allons quand même les détailler ici

```powershell
mkdir WebApi
cd WebApi
dotnet new sln -n SimpleShop
dotnet new classlib -o SimpleShop.Common
dotnet new classlib -o SimpleShop.PostgreSql
dotnet new webapi -o SimpleShop.WebApi
dotnet sln SimpleShop.sln add SimpleShop.Common\SimpleShop.Common.csproj --solution-folder Libraries
dotnet sln SimpleShop.sln add SimpleShop.PostgreSql\SimpleShop.PostgreSql.csproj --solution-folder Libraries
dotnet sln SimpleShop.sln add SimpleShop.WebApi\SimpleShop.WebApi.csproj
dotnet add SimpleShop.PostgreSql\SimpleShop.PostgreSql.csproj reference SimpleShop.Common\SimpleShop.Common.csproj
dotnet add SimpleShop.WebApi\SimpleShop.WebApi.csproj reference SimpleShop.Common\SimpleShop.Common.csproj
dotnet add SimpleShop.WebApi\SimpleShop.WebApi.csproj reference SimpleShop.PostgreSql\SimpleShop.PostgreSql.csproj
```

Ce qui devrait vous donner ceci dans Visual Studio Code : 

![image-20210112164044050](https://i.imgur.com/4Mmzb4x.png)

Dans `Common`nous mettrons les modèles et les interfaces utilisées
Dans `PostgreSql` nous mettrons notre premier projet utilisant **Entity Framework**

Et enfin dans `WebApi` se trouvera nos routes, nos contrôleurs etc.

### Première étape définir les modèles

Pour la suite de ce cours nous allons nous baser sur : [Ce site d'un bar à jeu parisien](https://goodgameparis.fr/jeux/)

Avant de coder quoique ce soit il est essentiel de définir quels sont les objets que nous allons manipuler en base de donnée (Sql,Json,Csv ou autre)

#### Product.cs

```c#
namespace SimpleShop.Common.Entities
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int MinPlayer { get; set; }
        public int MaxPlayer { get; set; }
        public int MinDuration { get; set; }
        public int MaxDuration { get; set; }
        public decimal? Price { get; set; }
        public List<Tag> Tags { get; set;}
        public Difficulty Difficulty { get; set; }
    }
}
```

#### Difficulty.cs

```c#
namespace SimpleShop.Common.Enums
{
    public enum Difficulty{
        Facile = 0,
        Intermédiaire=1,
        Expert=2
    }
}
```



#### Tag.cs

```c#
namespace SimpleShop.Common.Entities
{
    public class Tag
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public List<Product> Products { get; set; }
    }
}
```



### Interfaces

Maintenant que nous avons nos modèles nous allons définir quelles actions nous souhaitons réaliser

#### `IGamesService.cs`

```c#
namespace SimpleShop.Common.Interfaces
{
    public interface IGamesService
    {
        Task<Product> GetProduct(int id);
        Task<List<Product>> GetAllProducts();
        Task<List<Product>> GetFilteredProducts(ProductSearchQuery query);
    }
}
```

Par convention les interfaces commencent par la lettre i majuscule. **I**Disposable **I**Enumerable etc.

Notre projet `Common`devrait ressembler à cela maintenant : 

![image-20210112173335076](https://i.imgur.com/2AQL222.png)

### Création de notre DB Context : Étape finale vers notre base de donnée

Pour commencer nous allons installer le package EntityFrameworkCore dans notre projet SimpleShop.PostgreSql

Pour faciliter les choses on va installer un plugin : 

![image-20210112174517677](https://i.imgur.com/TeZMxf9.png)



Une fois le plugin installé : ctrl shift P ou simplement F1 pour afficher les commandes VS Code et tapez Nuget , sélectionnez nuget Add.

Tapez EntityFramework puis ensuite sélectionnez `Microsoft.EntityFrameworkCore` et la dernière version et enfin sélectionnez le projet PostgresSql



Ensuite faites de même pour :

EntityFrameworkCore dans WebApi

EntityFrameworkCode.Design dans WebApi

Npgsql.EntityFrameworkCode.PostgreSQL dans WebApi et dans PostgreSQL



Le DbContext est l'objet qui va représenter notre base de donnée, il aura un `DbSet<T>` par entité, pour nous ça sera `Products` et `Tags` 
C'est dans le DbContext que l'on va définir aussi le schéma. Quelle colonne porte quelle clé, quelle colonne ou jeu de colonne est indexé etc.



```c#
using System;
using Microsoft.EntityFrameworkCore;
using SimpleShop.Common.Entities;

namespace SimpleShop.PostgreSql
{
    public class SimpleShopContext : DbContext
    {
        public DbSet<Product> Products { get; set; }
        public DbSet<Tag> Tags { get; set; }

        public SimpleShopContext(DbContextOptions<SimpleShopContext> options) : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>(x =>
            {
                x.HasKey(a => a.Id);

                x.HasMany(a => a.Tags)
                    .WithMany(a => a.Products);
            });
        }
    }
}

```



Ici nous définissons nos deux DbSet : Products et Tags et nous indiquons dans le schéma que la clé de Product est Id (Il le déduit tout seul mais c'est pour l'exemple).
Nous indiquons aussi une relation many-to-many entre product et tag.

#### Deuxieme étape la migration

Maintenant que notre schéma est prêt nous allons l'historiser. C'est à dire nous allons sauvegarder ce schéma pour le versionner.

Pour ce faire il est nécessaire que l'outil sache sur quel système de base de donnée on va appliquer ce schéma. 
La méthode la plus simple dans notre cas est de configurer l'utilisation de notre DbContext dans notre WebApi.

#### `Startup.cs`

Rajoutez dans `ConfigureServices` : 

```c#
services.AddDbContext<SimpleShopContext>(x=>{
    x.UseNpgsql("Host=localhost;Database=SimpleShopSolo;Username=postgres;Password=azerty");
});
```



Une fois que c'est fait nous pouvons historiser notre version de la base de donnée avec :

```powershell
dotnet tool install --global dotnet-ef  #a n'executer qu'une seule fois, installe le tool en global
cd SimpleShop.PostgreSql
dotnet ef migrations add "Modele initial" -s ..\SimpleShop.WebApi\

```

Sauvegarde notre modele sous le nom "Modele initial" en nous basant sur la configuration définie dans la WebApi

```powershell
dotnet ef database update -s ..\SimpleShop.WebApi\
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