using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using Pokemon.Models;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace Pokemon.Controllers
{
    public class PokemonController : Controller
    {
        private readonly HttpClient _httpClient;

        public PokemonController()
        {
            _httpClient = new HttpClient
            {
                BaseAddress = new System.Uri("https://pokeapi.co/api/v2/")
            };
        }

        public async Task<IActionResult> Index()
        {
            var response = await _httpClient.GetAsync("pokemon?limit=20");
            var content = await response.Content.ReadAsStringAsync();
            var json = JObject.Parse(content);

            var results = json["results"].ToObject<List<JObject>>();
            var pokemons = new List<Pokemon>();

            foreach (var result in results)
            {
                var name = result["name"].ToString();
                var pokemonDetailsResponse = await _httpClient.GetAsync($"pokemon/{name}");
                var pokemonDetailsContent = await pokemonDetailsResponse.Content.ReadAsStringAsync();
                var pokemonDetailsJson = JObject.Parse(pokemonDetailsContent);

                var pokemon = new Pokemon
                {
                    Name = name,
                    Abilities = pokemonDetailsJson["abilities"].ToObject<List<JObject>>()
                                   .ConvertAll(a => a["ability"]["name"].ToString()),
                    Moves = pokemonDetailsJson["moves"].ToObject<List<JObject>>()
                                .ConvertAll(m => m["move"]["name"].ToString())
                };
                pokemons.Add(pokemon);
            }

            return View(pokemons);
        }

        public async Task<IActionResult> Details(string name)
        {
            var response = await _httpClient.GetAsync($"pokemon/{name}");
            var content = await response.Content.ReadAsStringAsync();
            var json = JObject.Parse(content);

            var pokemon = new Pokemon
            {
                Name = json["name"].ToString(),
                Abilities = json["abilities"].ToObject<List<JObject>>()
                               .ConvertAll(a => a["ability"]["name"].ToString()),
                Moves = json["moves"].ToObject<List<JObject>>()
                            .ConvertAll(m => m["move"]["name"].ToString())
            };

            return View(pokemon);
        }
    }
}
