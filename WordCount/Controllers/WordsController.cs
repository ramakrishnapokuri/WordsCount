using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace WordCount.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WordsController : ControllerBase
    {
        private static readonly char[] separators = { '.', '?', '!', ' ', ';', ':', ',' };

        private static ConcurrentDictionary<string, int> wordCountsdit= new ConcurrentDictionary<string, int>();

        [HttpGet]
        public ActionResult<IDictionary<string, int>> WordCounts()
        {
            if (wordCountsdit.IsEmpty)
            {
                CreateWordCount();
            }

            return wordCountsdit;
        }

        [HttpGet("topwords/{count=10}")]
        public ActionResult<IDictionary<string, int>> TopWords(int count)
        {
            if (wordCountsdit.IsEmpty)
            {
                CreateWordCount();
            }

            var result = wordCountsdit.OrderByDescending(x => x.Value).Take(count).ToDictionary(x => x.Key, x => x.Value);
            return result;
        }

        [HttpGet("wordDef/{word}")]
        public async Task<ActionResult<dynamic>> WordDefinition(string word)
        {
            if (wordCountsdit.IsEmpty)
            {
                CreateWordCount();
            }

            if (wordCountsdit.ContainsKey(word))
            {
                var result =await GetDefininitions(word);
                return result;
            }
            else
            {
                return NotFound();
            }          
        }

        private async Task<dynamic> GetDefininitions(string word)
        {
            string url = $"https://owlbot.info/api/v4/dictionary/{word}";
            using (HttpClient client = new HttpClient())
            {
                
                client.DefaultRequestHeaders.Authorization =new AuthenticationHeaderValue("Token", "609b538891053085d73a526a94fb3b0ae57ee63d");
                var result = await client.GetStringAsync(url);
                var obj = JObject.Parse(result);
                return obj["definitions"];
            }            
        }

        private void CreateWordCount()
        {       
                Parallel.ForEach(System.IO.File.ReadLines("words.txt", Encoding.UTF8), line =>
                {
                    var words = line.Split(separators, StringSplitOptions.RemoveEmptyEntries);
                    foreach (var word in words)
                    {
                        wordCountsdit.AddOrUpdate(word.ToLower(), 1, (_, x) => x + 1);
                    }
                });
            }                    
        }
    }
