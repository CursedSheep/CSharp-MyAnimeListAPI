/**********************************************************************************
        Copyright (C) 2020  CursedSheep

        This program is free software: you can redistribute it and/or modify
        it under the terms of the GNU General Public License as published by
        the Free Software Foundation, either version 3 of the License, or
        (at your option) any later version.

        This program is distributed in the hope that it will be useful,
        but WITHOUT ANY WARRANTY; without even the implied warranty of
        MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
        GNU General Public License for more details.
**********************************************************************************/  
        
        
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSMALAPI.Structures
{
    public struct main_picture
    {
        public string medium;
        public string large;
    }
    public struct searchResult
    {
        public uint ID;
        public string Title;
        public main_picture main_Picture;
    }
    public struct MALUserInfo
    {
        public uint ID;
        public string Name;
        public string Location;
        public DateTime Joined_At;
    }
    public struct alternative_titles
    {
        public string[] Synonyms;
        public string EN;
        public UnicodeString JA;
    }
    public struct UnicodeString
    {
        public string Value { private set; get; }
        public UnicodeString(string str)
        {
            Value = System.Uri.UnescapeDataString(str);
        }
        public static implicit operator UnicodeString(string str)  => new UnicodeString(str);
    }
    public struct Genre
    {
        public uint ID;
        public GenreType Name;
    }
    public struct Studio
    {
        public uint ID;
        public string Name;
    }
    public class AnimeLibraryItem
    {
        public Node_ Node;
        public list_status_ List_status;
        public struct Node_
        {
            public uint ID;
            public string Title;
            public main_picture Main_Picture;
            public alternative_titles Alternative_Titles;
            public uint Average_episode_duration;
            public DateTime End_date;
            public Genre[] Genres;
            public float Mean;
            public MediaTypeEnum MediaType;
            public uint Num_episodes;
            public uint Popularity;
            public string Rating;
            public DateTime Start_date;
            public string Status;
            public Studio[] Studios;
            public string Synopsis;
        }
        public struct list_status_
        {
            public WatchListStatus? Status;
            public uint? Score;
            public uint? Num_episodes_watched;
            public bool? Is_rewatching;
            public DateTime? Updated_at;
            public string Comments;
            public DateTime? finish_date;
            public uint? Num_times_rewatched;
            public DateTime? start_date;
            public string[] Tags;
        }
    }
    public struct GenreType
    {
        public GenreTypeEnum Value;
        public GenreType(string type)
        {
            var genretypelist = Enum.GetValues(typeof(GenreTypeEnum)).Cast<GenreTypeEnum>().ToList();
            string enumName = type.Replace(" ", "").Replace("-", "_");

            if (genretypelist.Exists(x => x.ToString().Equals(enumName)))
                 Value = genretypelist.First(x => x.ToString().Equals(enumName));
            else Value = GenreTypeEnum.Unknown;

        }
        public GenreType(GenreTypeEnum type) => Value = type;
        public static implicit operator GenreType(string str) => new GenreType(str);
        public static implicit operator GenreType(GenreTypeEnum type) => new GenreType(type);
    }
    public struct WatchListStatus
    {
        public WatchListStatusEnum Value;
        public WatchListStatus(string type)
        {
            var WatchListStatuslist = Enum.GetValues(typeof(WatchListStatusEnum)).Cast<WatchListStatusEnum>().ToList();
            string enumName = type.Replace(" ", "").Replace("-", "_");

            if (WatchListStatuslist.Exists(x => x.ToString().ToLower().Equals(enumName)))
                Value = WatchListStatuslist.First(x => x.ToString().ToLower().Equals(enumName));
            else Value = WatchListStatusEnum.Unknown;
        }
        public WatchListStatus(WatchListStatusEnum type) => Value = type;
        public static implicit operator WatchListStatus(string str) => new WatchListStatus(str);
        public static implicit operator WatchListStatus(WatchListStatusEnum type) => new WatchListStatus(type);
    }

    public enum WatchListStatusEnum
    {
        Watching,
        Completed,
        On_Hold,
        Dropped,
        Plan_To_Watch,
        Unknown
    }
    public enum MediaTypeEnum
    {
        TV,
        Movie,
        OVA,
        Special
    }
    public enum GenreTypeEnum
    {
        Action,
        Adventure,
        Cars,
        Comedy,
        Dementia,
        Demons,
        Drama,
        Ecchi,
        Fantasy,
        Game,
        Harem,
        Hentai,
        Historical,
        Horror,
        Josei,
        Kids,
        Magic,
        Martial,
        Mecha,
        Military,
        Music,
        Mystery,
        Parody,
        Police,
        Psychological,
        Romance,
        Samurai,
        School,
        Sci_Fi,
        Seinen,
        Shoujo,
        Shoujo_Ai,
        Shounen ,
        Shounen_Ai ,
        Slice_of_Life ,
        Space ,
        Sports ,
        Super_Power,
        Supernatural,
        Thriller,
        Vampire,
        Yaoi,
        Yuri,
        Unknown
    }
    public enum SearchType
    {
        alternative_titles,
        average_episode_duration,
        end_date,
        genres,
        id,
        main_picture,
        mean,
        media_type,
        num_episodes,
        popularity,
        rating,
        start_date,
        status,
        studios,
        synopsis,
        title
    }
}
