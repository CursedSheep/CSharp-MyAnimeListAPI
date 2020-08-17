# CSharp-MyAnimeListAPI
Give credits to me as you use this ;D

### Introduction
Poorly written MyAnimeList API in C#
The project is based from [erengy/taiga].
Features available in the API is only for Anime List.

### Features
   - Search Anime
   - Read your Anime list
   - Add/Remove/Update Anime in your Anime list
### How to setup
Go to your MyAnimeList then goto Account Settings -> API -> Create ID.
Then fill up the forms then submit. More info in setting up API [here].
### How to use

Initialize API and start Authorization by writing
```CSharp
var api = new MyAnimeListAPI("Your ClientID", "Your Redirect", "Your ClientSecret (Optional)");
await api.startUserAuth();
```
Sample usage
```CSharp
var api = new MyAnimeListAPI("Youur Client ID", "Your Redirect Link", "Your Client secret (Optional)");
await api.startUserAuth();
//Search Anime
List<AnimeLibraryItem.Node_> searchresults = await api.SearchAnime("Hello World");

//Add Anime to list
await api.AddLibraryEntry(searchresults[0]);
Console.WriteLine("Anime " + searchresults[0].Title + " added to list!");

//Get Anime list
List<AnimeLibraryItem> AnimeList = await api.GetAnimeLibraries(0);
AnimeLibraryItem helloworldAnime = AnimeList.First(x => x.Node.Title == "Hello World");

//Update Anime Info from library
helloworldAnime.List_status.Comments = "test Comment";
await api.UpdateLibraryEntry(helloworldAnime);
Console.WriteLine("Anime " + helloworldAnime.Node.Title + " Updated!");
//Delete Anime from library
await api.DeleteLibraryEntry(helloworldAnime.Node.ID);
Console.WriteLine("Anime " + helloworldAnime.Node.Title + " Removed rom list!");
```


   [erengy/taiga]: <https://github.com/erengy/taiga>
   [here]: <https://myanimelist.net/blog.php?eid=835707>
