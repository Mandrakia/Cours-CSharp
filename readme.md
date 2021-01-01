# Cours C#

### Origine

Le C# est un langage développé par Microsoft au début des années 2000 pour faire face au Java.
Tout comme le Java il s'appuie sur une VM et un Garbage collector. Il est fortement typé.

### Pourquoi le C#

Pour tout !
A l'heure actuelle on peut réellement tout faire en C#.

- Des jeux vidéos avec Unity PC et mobile (Earthstone, Call of duty mobile, Among us...)
- Des applications mobiles avec Xamarin
- Des Backends web ou pages SSR avec Asp.Net 
- Des frontend webs responsive et dynamiques avec Blazor et les web assemblies

### Installer .Net Core et créer notre premier programme

Pour installer .Net core pour votre plateforme il faut se rendre à l'adresse suivante : 

[.Net Core SDK 5.0]: https://dotnet.microsoft.com/download/dotnet/5.0

Une fois installé : 

```bash
~ > mkdir cours-csharp
~ > cd cours-csharp
~/cours-csharp > dotnet new console
~/cours-csharp > dotnet run
```





### Structure d'un programme C#

Un programme C# est composé au minimum de 2 fichiers, un fichier projet en .csproj et un fichier code source en .cs

HelloWorld.csproj

```xml
<Project Sdk="Microsoft.NET.Sdk">

  <PropertyGroup>
    <OutputType>Exe</OutputType>
    <TargetFramework>net5.0</TargetFramework>
    <RootNamespace>cours_csharp</RootNamespace>
  </PropertyGroup>

</Project>
```

Program.cs

```c#
using System;

namespace cours_csharp
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
        }
    }
}
```


Le fichier projet défini quel est le framework utilisé ainsi que quels packages doivent être importés. S'il y a des fichiers qui doivent être bundled/exportés avec les fichiers binaires (comme les input.txt de advent of code) on les retrouvera aussi dans le fichier projet.

### Structure d'un fichier code source .cs

Un fichier source C# se compose de 2 grands éléments : 

- Des déclarations using
- Un namespace

En C# tout code doit se trouver dans une méthode(fonction) qui elle même doit se trouver dans une classe qui elle même doit se trouver dans un namespace.



Un namespace est juste une arborescence (comme un dossier) pour organiser son code. 
Par exemple System.IO contiendra toutes les classes qui ont un rapport avec l'entrée/sortie. Donc les flux (stream) , les classes de manipulation de fichier de répertoires etc.



Toute classe est donc appelable de la manière **{namespace}.{classe}**, par exemple System.IO.File pour manipuler des fichiers.
Vu que cela est long à écrire ils y a les usings pour cela.

```c#
using System.IO;
```

Va indiquer que les classes que l'on va appeler peuvent aussi se trouver dans le namespace System.IO en plus du namespace dans lequel on travaille.
Dorénavant on pourra aussi taper File tout court pour appeler System.IO.File

### Les classes

En C# tout est basé sur les classes (objets).

Une classe peut comporter 3 types d'éléments : 

- Des champs (fields)
- Des propriétés
- Des méthodes(fonctions)

#### Les champs :

Ce sont des variables au niveau de la classe par exemple:

```c#
int myCounter = 0;
```

Ici nous déclarons un champs myCounter de type int (nombre entier). Nous l'initialisons à 0

#### Les propriétés

Ce sont comme des champs dans le sens ou on peut leur attribuer une valeur et obtenir cette valeur mais on peut customiser ces 2 actions.

```c#
private int _prop;
public int Prop
{
    get
    {
        return _prop;
    }
    set
    {
        _prop = value;
    }
}
public int Prop2 { get; set; }
```

Dans la propriété Prop nous cachons le champs _prop mais sont fonctionnement reste le même.
Pour définir une proprieté qui fonctionnera comme un champs nous pouvons faire comme dans Prop2

Mais quel est intérêt des Propriétés comparé aux champs ? 

- Les get/set peuvent avoir des accessibilités différentes : par exemple un get public et un set private
- Le fait que le get/set soit des fonctions permet par exemple de modifier 2 variables a la fois ou de trigger un event dans le set.

#### Les méthodes

C'est la ou se trouve la logique du programme la déclaration d'une méthode est la suivante : 

```c#
public int Multiply(int a,int b)
{
    return a*b;
}
```

Ici nous déclarons une méthode **Multiply** qui attend 2 arguments a et b (tous deux int) et qui retournera un int

### Value vs Reference type

En C# toute variable est soit une structure(**struct**) soit un objet(**class**) les 2 se comportent différemment lorsqu'il sont passés en argument

```c#
static public void Main(){
    var nombre = 2;
    ModifierNombre(nombre);
    Console.WriteLine(nombre); // Affichera 2.
}
static public ModifierNombre(int nombre)
{
    nombre = nombre + 2;
}
```

Lorsqu'une structure est passée en argument de fonction (ici un integer 2), c'est une copie qui est envoyée et non une référence vers la variable.
Toute modification n'impactera donc pas le scope de l'appelant.

A l'inverse pour les classes se sont des références qui sont envoyées a la fonction (pointeurs...) donc : 

```c#
public class Test{
    public string Nom {get;set;}
}
public class Program{
    static public void Main(){
        var toto = new Test(){ Nom = "Bob"};
        ModifyName(toto);
        Console.WriteLine(toto.Nom); // Barbara
    }
    static void ModifyNom(Test t){
        t.Nom = "Barbara";
    }
}
```

Ici nous avons aussi vu l'initialisation d'un objet grâce au raccourci : 

```c#
var bob = new Test(){ Nom = "Toto"};
```

Il est cependant possible d'envoyer une structure en tant que référence pour cela la fonction et l'appel doivent utiliser le mot clef ref

```c#
public static void Main(){
    var nb = 2;
    Modify(ref nb);
    Console.WriteLine(nb); // 4
}
public static void Modify(ref int toto)
{
    toto +=2;
}
```



### Premier programme / Exercice (Temps : 15 min)

Comme vu au dessus pour écrire sur la console il faut taper : 

```c#
Console.WriteLine("Mon texte");
```

Pour lire sur la console il faut écrire : 

```c#
var line = Console.ReadLine();
```

Pour convertir une string en int il faut faire : 

```c#
var number = int.Parse(line);
```

Pour tenter de convertir un string en int (et donc avec gestion d'erreur) il faut faire :

```c#
if(int.TryParse(line,out var number)){
    //Good
}
else{
	//Not good
}
```

Pour générer une chiffre aléatoire entre 1 et 100 : 

```c#
var randomNumber = new Random().Next(1,100);
```

A partir de tout cela faire un programme qui va choisir aléatoirement un nombre entre 1 et 500.
L'utilisateur devra deviner ce nombre, si le "guess" est trop grand alors le programme devra l'indiquer, idem si c'est trop petit.
Si le nombre est deviné alors il faudra indiquer : "Youpi ! " et écrire en combien de tentatives l'utilisateur y est parvenu. (Le programme arrêtera a ce moment la).

Points bonus si vous gérez le fait que l'utilisateur est parfois stupide et peut rentrer autre chose qu'un nombre.



### Les collections du framework .Net

Tous les programmes doivent gérer à un moment ou un autre une liste éléments. Que cela soit des records dans une base de donnée, ou une liste de taches a effectuer, ou tout simplement une chaine de charactère qui n'est jamais qu'un tableau de charactère.

#### Les tableaux

En c# les tableaux ont une taille fixe donnée lors de l'initialisation par exemple :

```c#
var myTable = new int[10];
```

Ici nous initialisons un tableau de 10 integers (tous initialisés avec la valeur par défaut 0)

Si nous voulions donner des valeurs directement lors de l'initialisation nous pouvions faire : 

```c#
int[] myTable = new {1,2,3,4};
var myTable = new int[]{1,2,3,4};
```

Le mot clé **var** n'est qu'un raccourci, il peut s'écrire en lieu du type lorsque le type est "devinable"

```c#
var myArray = new int[4];
int[] myArray2 = new int[4];
```

Ces 2 exemples sont exactement pareils, la majorité du temps pour les exemples prochains nous écriront **var**

Chaque valeur d'un tableau est indexée et donc peut se récupérer grâce à son index

```c#
var myArray = new int[]{1,2,3,4};
Console.WriteLine(myArray[0]);
```

Cela écrira 1.

Comme expliqué plus haut les tableaux ont une taille **fixe** en C# il n'y a donc pas de méthode Push ou Splice

Pour obtenir la taille d'un tableau il faut appeler la propriété **Length**

Pour un tableau multidimensionnel il faut appeler **GetLength(dimension) **par exemple : 

```c#
var grid = new int[100,50];
var lengthX = grid.GetLength(0);
var lengthY = grid.GetLength(1);
```

#### Les listes

En C# les listes sont ce qui se rapproche le plus d'un tableau Javascript.
Contrairement aux tableaux les listes ont une taille variable on peut y ajouter et retirer des éléments, comme tout en C# les listes sont fortement typées (Il existe une variante non typée mais à ne jamais utiliser !! bouhou)

```c#
var myList = new List<int>(){1,2,3,4,5,6};
var myListString = new List<string>(){"abc","def","ghi","jkl"};
```

Comme les tableaux les éléments sont indexés et donc accessibles via **liste[index]**

```c#
var myListString = new List<string>(){"abc","def","ghi","jkl"};
Console.WriteLine(myListString[1]); //Ecrira : def
```

Pour ajouter des éléments ou en retirer :

```c#
var myListString = new List<string>(){"abc","def","ghi","jkl"};
myListString.Add("mno");
myListString.Remove("abc");// DANGEREUX va énumérer sur toute la liste pour le trouver
myListString.RemoveAt(0);
var result = myListString.Contains("abc"); // DANGEREUX va énumérer sur toute la liste pour le trouver
```

Pour obtenir la taille d'une liste il faut appeler la propriété **Count**

#### Le Dictionnaire / HashSet

Le HashSet se comporte comme une liste mais ou chaque élément est unique

```c#
var myHashString = new HashSet<string>(){"abc","def","ghi","jkl"};
Console.WriteLine(myHashString.Count); // 4
myHashString.Add("abc");
Console.WriteLine(myHashString.Count); // 4
```

Trouver un élément dans un Hashset est **EXTRÊMEMENT RAPIDE**

```c#
var myHashString = new HashSet<string>(){"abc","def","ghi","jkl"};
myHashString.Remove("abc"); // Très très rapide, pas besoin d'énumerer sur tous les éléments
var result = myHashString.Contains("abc"); // Idem
```

Le dictionnaire permet d'associer une clef avec une valeur et bénéficie des performances du HashSet

```c#
var myDict = new Dictionary<string,int>(); // La clef est de type STRING et la valeur de type INT
myDict["toto"] = 5;
var result = myDict["toto"]; //Très très rapide
if(myDict.ContainsKey("toto"))
{
    //On a toto 
}
```

### Deuxième programme / Exercice : 15 minutes

Pour ouvrir un fichier et en lire son contenu une des méthodes est : 

```c#
var pt = File.ReadAllLines("exercice2.txt");
```

Cela retournera un tableau de string donc **string[]**

Dans le répo GitHub se trouve le fichier exercice2.txt. Le but de l'exercice est d'afficher dans la console la somme des chiffres.



### `IEnumerable<T>` et Linq

En C# quasiment toutes les collections implémentent l'interface `IEnumerable<T>`

Nous reviendrons plus tard sur ce qu'est une interface mais pour simplifier il s'agit d'un contrat que tout classe qui l'implémente se doit de remplir.

En C# toute classe qui implémente IEnumerable ou sa variante fortement typée (Générique) `IEnumerable<T>` peut rentrer dans une boucle foreach.

```c#
var chaine = "Vive le C#";
foreach(var chara in chaine)
{
    Console.Write(chara);
}
// Ouputs : Vive le C#
```

Mais ce n'est pas tout, il y en a en C# quelque chose qui s'appelle des méthodes d'extensions.
Par exemple si vous trouvez qu'une classe ou une interface du framework manque d'une methode que vous souhaiteriez vous pouvez l'ajouter vous meme

```c#
static public string RotateLeft(this string input,int steps = 1) //Exemple de méthode utile dans Advent Of Code par exemple...
{
    //abcdef devient bcdefa
    return input.Substring(steps,input.Length-steps) + input.Substring(0, steps);
}

static void Main()
{
    var result = "abcdef".RotateLeft();
}
```

Ce qui nous ammène au Linq. C'est un set de méthode d'extension qui s'applique aux IEnumerable<T>

Toutes les méthodes suivantes ne vont rien appliquer tant que le resultat n'est pas consumé par : 

- Foreach
- .ToList() ou .ToArray() ou autre variante

#### Select

```c#
var liste = new List<string>(){"1","2","3","4"};
var numbers = liste.Select(a=> int.Parse(a));
```

Select va appliquer à chaque élément de la liste la méthode fournie et retourner un IEnumerable<int> au lieu du IEnumerable<string> que nous avions au début. Nous aurions pu aussi écrire : 

```c#
var numbers = liste.Select(int.Parse);
```

Car la signature de int.Parse est la même que celle attendue par Select. Une fonction qui prend une string en paramètre et qui retourne quelque chose (donc non void)

#### Where

```c#
var liste = new List<string>(){"1","2","3","4"};
var numbers = liste.Select(a=> int.Parse(a));
var numbSup2 = numbers.Where(a=> a >= 2);
```

Ici nous ne prenons que les nombres supérieurs ou égal a 2. 
**Notez qu'aucune action n'a été encore prise car le résultat numbSup2 et numbers n'ont pas encore été consumés.**

#### Sum / Min / Max / Average

Ces fonctions sont assez explicites

```c#
var liste = new List<string>(){"1","2","3","4"};
var numbers = liste.Select(a=> int.Parse(a));
var sum = numbers.Sum();
```

#### Fonctions IEnumerable<T> 

Une fonction peut être elle même un IEnumerable<T> grâce au mot clé **yield return**

```c#
static public IEnumerable<string> GetNames()
{
    yield return "Toto";
    Console.WriteLine("Tata appelée");
    yield return "Tata";
    Console.WriteLine("Tonton appelé");
    yield return "Tonton";
}
static void Main()
{
    var res = GetNames();
    var toto = res.First(); // Aucune sortie console ne s'effectuera car l'énumération s'arretera au premier résultat.
}
```

Evidemment ce n'est pas flagrant dans un exemple comme ça mais cette fonctionnalité est essentielle dans de nombreux cas.
Nous verrons un exemple concret plus tard.

###  Entrée/Sortie (I/O) et streams

La plupart des échanges informatiques sont basés sur des flux ce qu'on appelle Stream.
Lorsque l'on lit ou écrit un fichier, on lit ou écrit sur un flux de donnée que l'OS puis le contrôleur disque va transformer en réelle modification du système sous-jacent.

Il en va de même lorsque l'on effectue un appel réseau comme HTTP la réponse et la requête sont tous deux des streams.

La majorité de ces flux sont cachés par le framework et il n'est pas nécessaire la majorité du temps de comprendre comment cela fonctionne.
Par exemple pour l'exercice 1 vous avez lu un tableau de ligne a partir d'un fichier sans ne jamais avoir à manipuler des flux.
Voila comment la fonction .ReadAllLines peut être implémentée avec les flux:

```c#
static public List<string> GetContent()
{
    var res = new List<string>(); // Notre résultat
    using(var fs = File.OpenRead("exercice2.txt")) // On ouvre le fichier. Le mot clé using sera expliqué plus bas
    {
        StreamReader str = new StreamReader(fs); //On a fs un flux de donnée binaire, on passe à str un flux de chaine de charactère
        var line = string.Empty;
        while((line = str.ReadLine())!= null) // Tant que l'on peut lire "line" et que line n'est pas null (a ne pas confondre avec string.Empty)
        {
            res.Add(line); //On ajoute la ligne au résultat
        }
    }
    return res;
}
```

Voila notre propre implémentation basique de ReadAllLines. 

Imaginez les flux comme un tableau ou on ne peut qu'incrémenter l'index. Très peu de flux permettent de revenir en arrière.
Un flux réseau ne permettra jamais de revenir en arrière par exemple. Une fois que la donnée est reçue, on ne va pas demander a l'émetteur "ah au fait revient un peu en arrière stp j'ai merdé"

Il y a des flux d'entrée et des flux de sortie. En entrée on va effectuer des opérations comme Read() ReadBytes() etc...

A l'inverse dans un flux de sortie on va écrire avec Write() WriteBytes etc.

#### Mot-clé Using

Dans l'exemple au dessus nous utilisons le mot-clé using.
Son utilisation indique qu'à la fin du bloc (y compris en cas d'exception) on doit appeler la méthode .Dispose() de l'objet (ici fs)

La méthode **dispose** est présente sur tout objet qui implémente l'interface IDisposable.

Cela permet au programme de savoir : "J'en ai fini avec toi, libère toutes les ressources associées".

Dans le cas de notre fichier, vous avez déja du avoir un message d'erreur lorsque vous voulez effacer un fichier : "Ce fichier est utilisé par un programme en cours d'execution et ne peut etre effacé". Lorsque l'on modifie un fichier ou qu'on le lit, on effectue ce que l'on appelle un file Lock pour indiquer a l'os que l'on travaille dessus.
Ce lock empêche entre autre l'effacement du fichier.

Pour en revenir a notre using, le using indique justement a l'os de libérer ce lock car on en a fini avec.

```c#
void ExempleBadPractice()
{
    var fs = File.OpenRead("exercice2.txt");
    // bla bla bla
    fs.Dispose();
}
```

Dans la majorité des cas cela fera le même travaille que notre using au dessus. Sauf que s'il y a une exception, le dispose ne sera jamais appelé et notre fichier ne sera jamais libéré. (<u>Et j'en parle même pas pour délivré</u>)

L'implémentation exacte de using est : 

```c#
void Exemple()
{
    var fs = File.OpenRead("exercice2.txt")
    try
    {
        // bla bla bla
    }
    finally
    {
        fs.Dispose();
    }
}
```

### Manipulation de strings

Comme dans tous les langages le framework .Net offre de nombreuses fonctions de manipulation de **string**

Nous n'allons détailler que celles les plus utilisées.

```c#
static void Main(){
    var text = "abc def ghi";
    var subText = text.Substring(0,3); // abc, je prend 3 charactères commençant a l'index 0
    subText = text.Substring(4); // def ghi , je prend tous les charactères a partir de l'index 4
    var splits = text.Split(' '); // tableau ["abc","def","ghi"]
    var joinedString = splits.Aggregate((a,b)=> a + " " + b); // on revient a "abc def ghi"
}
```



### Exercice 3 ensemble : 45 min

Avec tout ce que l'on a vu jusqu'a présent nous sommes capables de créer un lecteur de fichier CSV performant.

Pour être considéré comme performant notre lecteur CSV doit etre capable de lire/analyser les données sans jamais les charger intégralement en mémoire.

Pour rappel le CSV (Comma separated values) est un format de fichier très populaire car il permet de stocker de très grandes quantités de donnée et d'être lu à la volée.

Nous nous baserons sur le fichier csv [trouvable ici](http://eforexcel.com/wp/downloads-18-sample-csv-files-data-sets-for-testing-sales/) le 5millions évidemment.

