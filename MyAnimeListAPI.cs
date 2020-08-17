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
        
        
using CSMALAPI.Structures;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CSMALAPI
{
    public class MyAnimeListAPI
    {
        private string kClientId;
        private string kRedirectUrl;
        private string ClientSecret;

        private string kBaseUrl = "https://api.myanimelist.net/v2";
        private int kLibraryPageLimit = 1000;
        private int kSearchPageLimit = 100;
        //private int kSeasonPageLimit = 500;

        private string access_token;
        private string refresh_token;
        private static Random random = new Random();

        public MyAnimeListAPI(string clientID, string redirectURL, string clientSekret = null)
        {
            kClientId = clientID;
            kRedirectUrl = redirectURL;
            ClientSecret = clientSekret;
        }
        private string RequestAuthorizationCode(string kclientid, string kredirecturl)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstvuwxyz0123456789-._~";
            string code = new string(Enumerable.Repeat(chars, 64).Select(s => s[random.Next(s.Length)]).ToArray());
            string strlink = string.Format("https://myanimelist.net/v1/oauth2/authorize?response_type=code&client_id={0}&redirect_uri={1}&code_challenge={2}&code_challenge_method=plain",
                 kclientid, kredirecturl, code);
            Process.Start(strlink);
            return code;
        }
        private HttpClient BuildRequest()
        {
            HttpClient hc = new HttpClient();
            hc.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            hc.DefaultRequestHeaders.AcceptCharset.Add(new StringWithQualityHeaderValue("utf-8"));
            //hc.DefaultRequestHeaders.AcceptEncoding.Add(new StringWithQualityHeaderValue("gzip"));
            hc.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", access_token);
            return hc;

        }
        private async Task<string> RequestUserToken()
        {
            string codever = RequestAuthorizationCode(kClientId, kRedirectUrl);
            using (MALAuthForm authform = new MALAuthForm())
            {
                if (authform.ShowDialog() == DialogResult.OK)
                {
                    using (var hc = new HttpClient())
                    {
                        hc.DefaultRequestHeaders.Accept.Clear();
                        hc.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/x-www-form-urlencoded"));
                        var d = new Dictionary<string, string>();
                        d.Add("client_id", kClientId);
                        if(ClientSecret != null) d.Add("client_secret", ClientSecret);
                        d.Add("grant_type", "authorization_code");
                        d.Add("code", authform.retKey);
                        d.Add("redirect_uri", kRedirectUrl);
                        d.Add("code_verifier", codever);
                        var data = new FormUrlEncodedContent(d);
                        var response = await hc.PostAsync("https://myanimelist.net/v1/oauth2/token", data);
                        return await response.Content.ReadAsStringAsync();
                    }
                }
            }
            return "null";
        }
        public async Task startUserAuth()
        {
            a1:
            var getUserToken = await RequestUserToken();
            string jsonResult = getUserToken;
            if (jsonResult.Contains("access_token") && jsonResult.Contains("refresh_token"))
            {
                var objectz = JsonConvert.DeserializeObject<Dictionary<string, string>>(jsonResult);
                access_token = objectz["access_token"];
                refresh_token = objectz["refresh_token"];
            }
            else goto a1;

        }
        //unused
        private async Task<string> refreshToken(string token)
        {
            using (var hc = new HttpClient())
            {
                hc.DefaultRequestHeaders.Accept.Clear();
                hc.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/x-www-form-urlencoded"));
                var d = new Dictionary<string, string>();
                d.Add("client_id", kClientId);
                if (ClientSecret != null) d.Add("client_secret", ClientSecret);
                d.Add("grant_type", "refresh_token");
                d.Add("refresh_token", token);
                var data = new FormUrlEncodedContent(d);
                var response = await hc.PostAsync("https://myanimelist.net/v1/oauth2/token", data);
                return await response.Content.ReadAsStringAsync();

            }
        }
        public async Task<List<AnimeLibraryItem.Node_>> SearchAnime(string toSearch)
        {
            List<AnimeLibraryItem.Node_> sr = new List<AnimeLibraryItem.Node_>();
            var req = BuildRequest();
            string target = kBaseUrl + "/anime?";
            var enumnames = Enum.GetNames(typeof(SearchType));
            using (var content = new FormUrlEncodedContent(new Dictionary<string, string>()
                {
                    { "q", toSearch},
                    { "limit", kSearchPageLimit.ToString()},
                    { "nsfw", "True" },
                    { "fields", string.Join(",", enumnames) }
                }))
            {
                var f = await content.ReadAsStringAsync();
                var response = await req.GetAsync(target + f);
                string str = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<dynamic>(str);
                Newtonsoft.Json.Linq.JToken data = result["data"];
                foreach (var item in data)
                {
                    string val = item["node"].ToString();
                    var test = JsonConvert.DeserializeObject<Dictionary<string, object>>(val);
                    if (test.ContainsKey("end_date") && test["end_date"].ToString().Count(x => x == '-') != 2)
                        val = val.Replace('"' + "end_date" + '"' + ": " + '"' + test["end_date"].ToString() + '"', '"' + "end_date" + '"' + ": " + '"' + test["end_date"].ToString() + "-01-01" + '"');
                    if (test.ContainsKey("start_date") && test["start_date"].ToString().Count(x => x == '-') != 2)
                        val = val.Replace('"' + "start_date" + '"' + ": " + '"' + test["start_date"].ToString() + '"', '"' + "end_date" + '"' + ": " + '"' + test["start_date"].ToString() + "-01-01" + '"');
                    var searchItem = JsonConvert.DeserializeObject<AnimeLibraryItem.Node_>(val);
                    sr.Add(searchItem);
                }
            }
            return sr;
        }
        public async Task<MALUserInfo> GetUser()
        {
            var req = BuildRequest();
            var response = await req.GetAsync(kBaseUrl + "/users/@me");
            string rez = await response.Content.ReadAsStringAsync();
            var deserialized = JsonConvert.DeserializeObject<MALUserInfo>(rez);
            return deserialized;
        }
        private string GetListStatusFields()
        {
            return string.Join(",", new string[]{"comments",
         "finish_date",
         "is_rewatching",
         "num_times_rewatched",
         "num_watched_episodes",
         "score",
         "start_date",
         "status",
         "tags",
         "updated_at" });
        }
        public async Task<List<AnimeLibraryItem>> GetAnimeLibraries(uint offset)
        {
            List<AnimeLibraryItem> animeList = new List<AnimeLibraryItem>();
            var req = BuildRequest();
            string target = kBaseUrl + "/users/@me/animelist?";
            var enumnames = Enum.GetNames(typeof(SearchType));
            using (var content = new FormUrlEncodedContent(new Dictionary<string, string>()
                {
                    { "limit", kLibraryPageLimit.ToString()},
                    { "offset", (offset).ToString()},
                    { "nsfw", "True" },
                    { "fields", string.Format("{0}, list_status{{{1}}}", string.Join(",", enumnames), GetListStatusFields())}
                }))
            {
                var f = await content.ReadAsStringAsync();
                var response = await req.GetAsync(target + f);
                string str = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<dynamic>(str);
                var data = result["data"];
                //var paging = result["paging"];
                //var pagingDeserialized = JsonConvert.DeserializeObject<Dictionary<string, string>>(str.ToString());
                foreach (var item in data)
                {
                    AnimeLibraryItem deserialized = JsonConvert.DeserializeObject<AnimeLibraryItem>(item.ToString());
                    animeList.Add(deserialized);
                }
            }
            return animeList;
        }
        public async Task<bool> DeleteLibraryEntry(uint ID)
        {
            var req = BuildRequest();
            string target = kBaseUrl + "/anime/" + ID.ToString() + "/my_list_status";
            var response = await req.DeleteAsync(target);
            return response.IsSuccessStatusCode;
        }
        public async Task<AnimeLibraryItem.list_status_> UpdateLibraryEntry(AnimeLibraryItem item)
        {
            var content = new FormUrlEncodedContent(new Dictionary<string, string>()
                {
                    { "fields", GetListStatusFields() }
                });
            var f = await content.ReadAsStringAsync();
            var req = new HttpClient();
            string target = kBaseUrl + "/anime/" + item.Node.ID.ToString() + "/my_list_status?" + f;
            var reqmsg = new HttpRequestMessage() { Method = new HttpMethod("PATCH"), RequestUri = new Uri(target) };
            reqmsg.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/x-www-form-urlencoded"));
            reqmsg.Headers.AcceptCharset.Add(new StringWithQualityHeaderValue("utf-8"));
            reqmsg.Headers.Authorization = new AuthenticationHeaderValue("Bearer", access_token);
            var body = new Dictionary<string, string>();

            if(item.List_status.Num_episodes_watched != null)
                body.Add("num_watched_episodes", item.List_status.Num_episodes_watched.Value.ToString());
            if(item.List_status.Status != null)
                body.Add("status", item.List_status.Status.Value.Value.ToString().ToLower());
            if(item.List_status.Score != null)
                body.Add("score", item.List_status.Score.Value.ToString());
            if (item.List_status.start_date != null)
                body.Add("start_date", item.List_status.start_date.Value.ToString("yyyy-MM-dd"));
            if (item.List_status.finish_date != null)
                body.Add("finish_date", item.List_status.finish_date.Value.ToString("yyyy-MM-dd"));
            if (item.List_status.Is_rewatching != null)
                body.Add("is_rewatching", item.List_status.Is_rewatching.Value.ToString().ToLower());
            if(item.List_status.Num_times_rewatched != null)
                body.Add("num_times_rewatched", item.List_status.Num_times_rewatched.Value.ToString());
            if(item.List_status.Tags != null)
                body.Add("tags", string.Concat(item.List_status.Tags));
            if(item.List_status.Comments != null)
                body.Add("comments", string.Concat(item.List_status.Comments));
            var data = new FormUrlEncodedContent(body);
            reqmsg.Content = data;
            var response = await req.SendAsync(reqmsg);
            string str = await response.Content.ReadAsStringAsync();
            var deserialized = JsonConvert.DeserializeObject<AnimeLibraryItem.list_status_>(str);
            if (response.IsSuccessStatusCode) return deserialized;

            throw new Exception(str + " or idk *shrugs*");
        }
        public async Task<AnimeLibraryItem.list_status_> AddLibraryEntry(AnimeLibraryItem.Node_ item)
        {
            AnimeLibraryItem v1 = new AnimeLibraryItem();
            v1.List_status = new AnimeLibraryItem.list_status_();
            v1.Node = item;
            return await UpdateLibraryEntry(v1);
        }
        private AnimeLibraryItem.list_status_ CreateDefaultList_statusValue()
        {
            AnimeLibraryItem.list_status_ s = new AnimeLibraryItem.list_status_();
            s.Status = WatchListStatusEnum.Watching;
            s.Comments = "";
            s.finish_date = null;
            s.start_date = DateTime.Now;
            s.Is_rewatching = false;
            s.Score = 0;
            s.Num_episodes_watched = 0;
            s.Num_times_rewatched = 0;
            s.Updated_at = DateTime.Now;
            s.Tags = null;
            return s;
        }
    }
}
