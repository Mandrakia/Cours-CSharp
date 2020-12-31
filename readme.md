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

Comme expliqué plus haut les tableaux ont une taille **fixe** en C# il n'y a donc par de méthode Push ou Splice

Pour obtenir la taille d'un tableau il faut appeler la propriété **Length**

Pour un tableau multidimensionnel il faut appeler **GetLength(dimension) **par exemple : 

```c#
var grid = new int[100,50];
var lengthX = grid.GetLength(0);
var lengthY = grid.GetLength(1);
```

#### Les listes

En C# les listes sont ce qui se rapproche le plus d'un tableau Javascript.
Contrairement aux tableaux les listes ont une taille variable on peut y ajouter et retirer des éléments, comme tout en C# les listes sont fortement typées

```c#
var myList = new List<int>();
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
var myDict = new Dictionary<string,int>();
myDict["toto"] = 5;
var result = myDict["toto"]; //Très très rapide
if(myDict.ContainsKey("toto"))
{
    //On a toto 
}
```

### Deuxième programme / Exercice : 15 minutes



### Les mots clés public/private/static/protected/abstract

#### Public/Private/Protected

##### Pour une classe

Public indique que la classe peut être appelée et utilisée par tout projet 
A l'inverse Private indique que cette classe ne peut être utilisée que par son propre projet
Le défaut est **Private**

##### Pour un élément de classe

Public indique qu'il peut etre appelé par toute autre classe
Private indique qu'il ne peut-être appelé que par des instances de cette classe
Protected indique qu'il ne peut-être appelé que par des instances de cette classe ou d'une classe qui en hérite

#### Static

##### Pour un élement de classe

Il indique qu'il n'y a pas besoin d'instance de la classe pour etre appelé. Une variable (proprieté ou champs) statique n'aura qu'une seule valeur pour tout le programme.
Pour une fonction c'est le même principe il n'y aura pas besoin d'instance de la classe pour l'appeler.

##### Pour une classe

Cela indique que tous les élements de la classe doivent etre static et qu'il est impossible de créer une instance de la classe avec le mot clé **new**

