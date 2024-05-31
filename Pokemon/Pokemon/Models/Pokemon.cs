using System.Collections.Generic;

namespace Pokemon.Models
{
    public class Pokemon
    {
        public string Name { get; set; }
        public List<string> Abilities { get; set; }
        public List<string> Moves { get; set; }
    }
}
