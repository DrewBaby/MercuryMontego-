﻿using Microsoft.AspNetCore.Mvc;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using IGDB;
using IGDB.Models;
using System.Collections.Generic;
using System;

namespace GameClubProject.Controllers
{
    public class GameQueryController : Controller
    {
        // 
        // GET: /HelloWorld/

        //public string Index()
        //{
        //    return "This is my default action...";
        //}

        private protected string IGDB_CLIENT_ID = "suir6rwr94fhs2s3x8jcyfqfjpd6lj";
        private protected string IGDB_CLIENT_SECRET = "a07fcbs0o7est8f2p9o6qpk09t5zfw";     
        private protected IGDBClient _api;

        //private readonly MvcMovieContext _context;

        //public MoviesController(MvcMovieContext context)
        //{
        //    _context = context;
        //}

        // --- List of views go here ---
        public async Task<IActionResult> IGDBQueryAsync()
        {

            _api = new IGDB.IGDBClient(IGDB_CLIENT_ID, IGDB_CLIENT_SECRET);
            var games = await _api.QueryAsync<Game>(IGDBClient.Endpoints.Games, query: "fields *; limit 50;");

            //foreach (var game in games)
            //{
            //    game.Id(mbox => mbox.ID == id);
            //    _ = game.Name; // Thief
            //}

            //ViewData [] = "Hello " + name;
            //ViewData["NumTimes"] = numTimes;

            return View(games);
        }

        public async Task<IActionResult> IndexAsync()
        {
            const int REEL_LENGTH = 15;

            _api = new IGDB.IGDBClient(IGDB_CLIENT_ID, IGDB_CLIENT_SECRET);
            IndexReel model = new IndexReel();
            string coverImageIDSet;
            Random random = new Random();
            long?[] genreId = new long?[2];

            // Select 2 genres at random from the IGDB Genre table
            Genre[] listOfGenres = await _api.QueryAsync<Genre>(IGDBClient.Endpoints.Genres, query: "fields *; limit 50; sort id asc;");
            List<long?> genreList = new List<long?>();
            foreach (Genre gameGenre in listOfGenres)
            {
                genreList.Add(gameGenre.Id);
            }
            do
            {
                int index = random.Next(genreList.Count);
                if (genreId[0] == null) genreId[0] = genreList[index];
                if (genreList[index] != genreId[0]) genreId[1] = genreList[index];
            } while (genreId[0] == null || genreId[1] == null);
            // Pass array of 2 selected genres to api queries to get the top 10 games
            // for that genre and the associated cover image details
            for (int x = 0; x < 2; x++)
            {
                Game[] gamesByGenre = await _api.QueryAsync<Game>(IGDBClient.Endpoints.Games, query: "fields id, name, cover, rating; where cover != null & rating != null & genres = (" + genreId[x] + "); sort rating desc; limit "+ REEL_LENGTH +";");
                coverImageIDSet = GenerateCoverImageIDSet(gamesByGenre);
                Cover[] gameCoverByGenre = await _api.QueryAsync<Cover>(IGDBClient.Endpoints.Covers, query: "fields alpha_channel, animated, checksum, game, height, image_id, url, width; where id = (" + coverImageIDSet + "); limit " + REEL_LENGTH + ";");
                gameCoverByGenre = ReorderCoverImageArrayByGameArray(gamesByGenre, gameCoverByGenre);
                if (x == 0)
                {
                    model.GenreATitle = listOfGenres[Array.IndexOf(genreList.ToArray(), genreId[x])].Name;
                    model.GenreAGames = MergeDataSets(gamesByGenre, gameCoverByGenre);
                }
                if (x == 1)
                {
                    model.GenreBTitle = listOfGenres[Array.IndexOf(genreList.ToArray(), genreId[x])].Name;
                    model.GenreBGames = MergeDataSets(gamesByGenre, gameCoverByGenre);
                }
            }

            // Get 10 games with highest ratings and their cover pictures
            Game[] topGames = await _api.QueryAsync<Game>(IGDBClient.Endpoints.Games, query: "fields id, name, cover, genres, rating; where cover != null & rating != null; sort rating desc; limit "+ REEL_LENGTH +";");
            coverImageIDSet = GenerateCoverImageIDSet(topGames);
            Cover[] covers = await _api.QueryAsync<Cover>(IGDBClient.Endpoints.Covers, query: "fields alpha_channel, animated, checksum, game, height, image_id, url, width; where id = ("+ coverImageIDSet + "); limit " + REEL_LENGTH + ";");
            covers = ReorderCoverImageArrayByGameArray(topGames, covers);
            model.TopRatedGames = MergeDataSets(topGames, covers);

            // Example games: 10 games with the highest ratings
            // Example covers: covers associated with the 10 games above
            // Example coverImageIDSet: 71, 84491, 97021, 111232, 117024, 43615, 54330, 83799, 98723, 81698
            // Example data returned for image api call:
            // id = 71
            // game = 70 :Ref ID for gameID
            // url = //images.igdb.com/igdb/image/upload/t_thumb/ndfzbf3xvuuchijx7v1c.jpg
            // height = 347
            // width = 288
            // alphaChannel = false
            // animated = false
            // checksum = 144656d4-5cfd-86a4-d785-89f7fcb8bb96

            // Returns model which consists of 3 sets of games and their associated cover images
            // TopGames = top rated games
            // GenreAGames & GenreBGames = top rated games by genre (genres chosen at random on page load)
            return View(model);
            //return View(covers);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            _api = new IGDB.IGDBClient(IGDB_CLIENT_ID, IGDB_CLIENT_SECRET);
            Game[] game = await _api.QueryAsync<Game>(IGDBClient.Endpoints.Games, query: "fields *; where id ="+ id +"; limit 1;");
            Game gameDetail = game[0];
            return View(gameDetail);
        }

        public IActionResult About()
        {
            return View();
        }

        public string Welcome(string name, int numTimes = 1)
        {
            return HtmlEncoder.Default.Encode($"Hello {name}, NumTimes is: {numTimes}");
        }

        // Helper methods 
        //
        // Generates a concatenated string of IDs for cover images associated with the games passed to the method
        // Example result set: 71, 84491, 97021, 111232, 117024, 43615, 54330, 83799, 98723, 81698
        private string GenerateCoverImageIDSet(Game[] gameSet)
        {
            string coverImageIDSet = "";
            for (int x = 0; x < gameSet.Length; x++)
            {
                long? CoverID = gameSet[x].Cover.Id;
                if (x < gameSet.Length - 1)
                {
                    coverImageIDSet = coverImageIDSet + CoverID + ", ";
                }
                else
                {
                    coverImageIDSet = coverImageIDSet + CoverID;
                }
            }
            return coverImageIDSet;
        }

        // Reorders an array of Cover so that a game in Game[] will accurately 
        // associate it's cover image details in Cover[]. 
        private Cover[] ReorderCoverImageArrayByGameArray(Game[] arrayToMatch, Cover[] arrayReorder)
        {
            Cover[] arrayOrdered = new Cover[arrayReorder.Length];
            long?[] searchArray = new long?[arrayReorder.Length];
            for (int x = 0; x < searchArray.Length; x++)
            {
                searchArray[x] = arrayReorder[x].Game.Id;
            }
            for (int x = 0; x < arrayToMatch.Length; x++)
            {
                long? indexToFind = arrayToMatch[x].Id;
                long? index;
                index = Array.IndexOf(searchArray, indexToFind);
                int indexInt = Convert.ToInt32(index);
                arrayOrdered[x] = arrayReorder[indexInt];
            }
            return arrayOrdered;
        }

        // Nice generates an array of ReelItems that consist of a video game and it's associated
        // cover image details
        private ReelItem[] MergeDataSets(Game[] game, Cover[] cover)
        {
            ReelItem[] gameReel = new ReelItem[game.Length];
            for (int y = 0; y < game.Length; y++)
            {
                ReelItem item = new ReelItem(game[y], cover[y]);
                gameReel[y] = item;
            }
            return gameReel;
        }
    }

    // Helper classes to build and send more complex data objects to views
    public class IndexReel
    {
        public IEnumerable<ReelItem> TopRatedGames { get; set; }
        public string GenreATitle { get; set; }
        public string GenreBTitle { get; set; }
        public IEnumerable<ReelItem> GenreAGames { get; set; }
        public IEnumerable<ReelItem> GenreBGames { get; set; }

    }

    public class ReelItem
    {
        public Game Game { get; set; }
        public Cover Cover { get; set; }

        public ReelItem(Game game, Cover cover)
        {
            Game = game;
            Cover = cover;
        }
    }
}