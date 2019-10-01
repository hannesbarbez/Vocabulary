namespace Vocabulary.Logic
{
    /// <summary>
    /// Defines a <see cref="Word"/>.
    /// </summary>
    public class Word
    {
        /// <summary>
        /// Defines the first language.
        /// </summary>
        public string Language0 { get; set; }

        /// <summary>
        /// Defines the second language.
        /// </summary>
        public string Language1 { get; set; }

        /// <summary>
        /// Constructs a new <see cref="Word"/>.
        /// </summary>
        public Word()
        {
            Language0 = string.Empty;
            Language1 = string.Empty;
        }
    }
}
