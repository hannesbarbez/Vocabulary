using System.Collections.Generic;

namespace Vocabulary.Logic
{
    public class Dictionary
    {
        public string Language0;
        public string Language1;
        public List<Word> Wordlist;
        public bool Randomize;
        public bool Question0to1;

        public Dictionary()
        {
            this.Language0 = "";
            this.Language1 = "";
            this.Wordlist = new List<Word>();
            this.Randomize = false;
            this.Question0to1 = true;
        }

        public Dictionary(string Language0, string Language1)
        {
            this.Language0 = Language0;
            this.Language1 = Language1;
            this.Wordlist = new List<Word>();
            this.Randomize = false;
            this.Question0to1 = true;
        }
    }
}
