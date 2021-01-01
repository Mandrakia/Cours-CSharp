# Cours c# avancé

### Classes , Interfaces et héritage/Implémentation

Comme nous l'avons vu précédemment en C# tout code appartient à une classe, et toute variable est soit une instance de classe comme une Liste<T> par exemple soit une structure(Nous verrons la différence juste après) 

L'héritage permet à une classe B d'avoir tout ce que A faisait mais de rajouter d'autre fonctionnalités.
Par exemple prenons le cas d'une scène 3D dans un jeu vidéo:

```c#
public class Objet{
    public (int X,int Y,int Z) Position {get;set;}
	public float Orientation {get;set;}
}
```

Tout objet de ma scène aura ces propriétés la. Une position (un triplet de coordonnées x,y,z) et une orientation en radian.

```c#
public class ObjetAvecRendu : Objet {
    public Model Model3D {get;set;}
}
```

Ici ce sont les objets qui sont affichés dans la scène ils auront donc besoin d'un model 3D pour être affichés.

```c#
public abstract class Personnage : ObjetAvecRendu{
    public float Vitesse {get;set;}
    public int Hp {get;set;}
    public int Armor {get;set;}
    
    public void Déplacer(){
        //Savant calcul entre position / orientation et vitesse
    }
}
```

Ici ce sont les personnages du jeu (monstre et joueur)

```c#
public class Monstre : Personnage{
	public IAEngine MoteurIA {get;set;}
}
public class Joueur : Personnage{
    public InputController InputController {get;set;}
}
```

Enfin voila, un Monstre héritera de toutes les propriétés de ses parents, et des parents de ses parents etc.

#### Abstract

Une classe abstract veut dire qu'elle ne peut être utilisée comme telle, et nécessite d'être héritée.
Dans notre exemple, la classe **Personnage** est abstract car n'avons pas a l'utiliser tel quelle.
Un personnage est soit un Monstre soit un Joueur. Pourtant les 2 peuvent bénéficier de la fonction **Déplacer**

#### Interfaces

Comme dit plus haut, une interface est un contrat que doit remplir une classe.
En C# une classe ne peut hériter que d'une seule autre classe mais peut implémenter de nombreuses interfaces.

Pour bien comprendre l'utilité des interfaces nous allons nous baser sur un exemple concret.

[Json database](https://jsonplaceholder.typicode.com/)

Nous avons actuellement une base de donnée accessible en REST et notre grosse application Web qui va l'utiliser et afficher les données.

Cela pourrait ressembler a ça : 

```C#
public class BlogRestReader{
    public List<Post> GetPosts(){
    	//Requete GET, puis deserialization JSON (Plus tard dans le cours)
    }
    public Post GetPost(int id){
        //Meme topo
    }
    public List<Comments>(int postId)
    {
        //Retourne les commentaires d'un post
    }
}
public class Program{
    static public void Main()
    {
        var blogReader = new BlogRestReader();
        var post = blogReader.GetPost(1);
        var comments = blogReader.GetComments(1);
    }
}

```

L'exemple est très concis car en réalité le nombre d'appels sera bien plus grand mais cela suffit a illustrer notre propos.
Imaginons que demain au lieu d'être une base de donnée REST, nous passons en MongoDB ou en PostgreSQL

Dans l'exemple du dessus, le plus simple serait de refaire une classe copié/collée de BlogRestReader et de réimplémenter toutes ses méthodes une à une et espérer que tout fonctionne. Pire il se pourrait que la fonction GetComments soit complétement rendue caduque parce que sur MongoDB les commentaires seront directement inclus dans le post.

Le mieux ici aurai été d'utiliser une interface : 

```c#
public interface IBlogRepository
{
    Post GetFullPost(int id);
    IEnumerable<Post> GetPosts();
}
```

Le mot clef publique n'est pas nécessaire, toute méthode d'interface est faite pour être publique de toute façon.

Il suffit d'implémenter notre interface , pour ce faire il suffit de taper :

```c#
public class BlogRestReader : IBlogRepository
{
    
}
public class BlogSQLReader : IBlogRepository
{
    
}
```

Puis de faire clic droit sur IBlogRepository et cliquer sur Implémenter interface. Un squelette de toutes les fonctions de l'interface va apparaitre.

Dans notre classe d'utilisation nous pourrons du coup écrire : 

```c#
static public void Main()
{
    IBlogRepository repo = isSQL ? new BlogSQLReader : new BlogRestReader;
    var posts = repo.GetPosts();
}
```

L'appelant n'a rien à savoir de l'implémentation (SQL ou REST) , la seule chose qu'il sait et doit savoir c'est qu'il y a une méthode GetPosts().

### Async/Await

En C# il y a un concept de Task qui est une surcouche aux threads.

S'il y a besoin d'executer un long job par exemple, le code ressemblera au suivant : 

```c#
static public void Main(){
    var job = Task.Run(LongJob);
    Console.ReadLine(); //Bloque le programme pour pas qu'on sorte
}
static public void LongJob(){
    //Grosse loop qui prend du temps
}
```

Ici il n'y a pas de retour a notre LongJob donc nous n'avons pas a l'attendre, par contre l'appel Task.Run(LongJob) n'est pas bloquant, le code de LongJob s'executera dans un autre thread.(Tout ceci est gérer par le TPL)

Si par contre la fonction avait un retour par exemple : 

```c#
static public async void Main()
{
    var job = LongJob();
    var result = await job;
    Console.WriteLine(result);
}
static public async Task<int> LongJob(){
    //Gros calcul 
    await Task.Delay(15000); //Simulation d'une attente de 15 secondes
    return 15;
}
```

Ici nous avons un gros calcul qui va durer 15 secondes et afficher le résultat. Il n'y a pas d'interret a le rendre asynchrone dans notre cas mais :

```c#
static public async void Main(){
    var job1 = Job1(); //Job très long
    var job2 = Job2(); //Job plus court et nécessaire a job4
    var job3 = Job3(); //Job court aussi et necessaire a job5
    Task job4;
    Task job5;
    var lstTask = new List<Task>(){job2,job3};
    while(lstTask.Count > 0){
        var tsk = await Task.WhenAny(lstTask); //Ici on libère le thread
        if(tsk == job2) //Le meme thread ou un autre reprend le travail
            job4 = Job4();
        else if(tsk == job3)
            job5 == Job5();
        lstTask.Remove(tsk);
    }
    var listAllJobs = new List<Task>(){job1,job4,job5};
    await Task.WhenAll(listAllJobs);
    Console.WriteLine("Tous les jobs sont complétés");
    
    //Il nous faut tous les job complétés pour avancer.
    
}
```

Ici le temps pris pour exécuter les 5 jobs sera bien plus court que si l'on avait exécuté chaque tache les unes a la suite des autres.

Async/Await est principalement utilisé dans les 3 cas suivants : 

- Comme l'exemple au dessus ou il y a des taches indépendantes/dépendantes
- Lorsque l'on travaille sur une interface graphique (Jeux video, Application mobile, Desktop) et que l'on ne doit pas bloquer le thread principal
- Lorsque l'on travaille sur un serveur web (Asp.Net)

L'atout principal du mot clef **await** est qu'il libère le thread tant qu'il n'y a pas de résultat. Ce thread peut donc etre recyclé et utilisé a d'autre fins (Gérer de nouvelles connections, gérer le rendering 3D etc.) et dans certains cas c'est ce même thread ou un autre qui prendra la continuation du parcours.

Par exemple sur un serveur Web qui pour notre exemple est bloqué a 2 threads.

Si au temps 0, il y a 2 connections qui se font nos 2 threads sont occupés et donc on ne peut plus recevoir d'autres connections.
Au temps 1, chacune des connexion doit effectuer une requête en base de donnée qui prendre 10 secondes
Au temps 11, la requête est finie on peut à nouveau accepter de nouvelles connections.

C'est comme cela que ça se passe sans le mot clef await.

Si par contre la requête en base de donnée était async et donc que l'on pouvait l'await. Pendant ces 10 secondes les 2 threads auraient pu gérer de nombreuses requêtes.

C'est pourquoi la grande majorité des fonction I/O en C# ont une implémentation Async. Il suffit de regarder si le nom de la méthode se fini en Async.

Notre tout premier exemple devient donc : 

```c#
static public async Task Main(){
    var lines = await File.ReadAllLinesAsync("exercice2.txt");
}
```

### Exercice 4 : 

Implémenter l'interface suivante en se basant sur le service REST écrit plus haut.

```c#
public interface IBlogRepository
{
    Post GetFullPost(int id);
    IEnumerable<Post> GetPosts();
}
```

Pour executer un appel réseau HTTP il faut utiliser : 

```c#
var client = new HttpClient()
{
    BaseAddress = new Uri("https://jsonplaceholder.typicode.com")
};
```

Il va falloir réutiliser cet objet car il a un certains cout en ressources. (On reviendra dessus plus tard)
Ensuite pour faire un appel GET et le désérialiser directement dans une classe : 

```c#
User user = await client.GetFromJsonAsync<User>("users/1");//Necessite la classe User évidemment.
```





### Modificateurs d'accessibilités

#### Public/Private/Protected

##### Pour une classe

- **Public** indique que la classe peut être appelée et utilisée par tout projet 
- **Private** indique que cette classe ne peut être utilisée que par son propre projet
- Le défaut est **Private**

##### Pour un élément de classe

- **Public** indique qu'il peut être appelé par toute autre classe
- **Private** indique qu'il ne peut-être appelé que par des instances de cette classe
- **Protected** indique qu'il ne peut-être appelé que par des instances de cette classe ou d'une classe qui en hérite
- Le défaut est **Private**

#### Static

##### Pour un élément de classe

Il indique qu'il n'y a pas besoin d'instance de la classe pour être appelé. Une variable (propriété ou champs) statique n'aura qu'une seule valeur pour tout le programme.
Pour une fonction c'est le même principe il n'y aura pas besoin d'instance de la classe pour l'appeler.

##### Pour une classe

Cela indique que tous les éléments de la classe doivent être static et qu'il est impossible de créer une instance de la classe avec le mot clé **new**