using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;

namespace DevMagicMemesWebApi.Contracts
{
    /// <summary>
    /// Tag
    /// </summary>
    public partial class Tag
    {
        /// <summary>
        /// Data unique identifier.
        /// </summary>
        [Key]
        public int Id { get; set; }

        public string? Title { get; set; }

        public virtual ICollection<Meme>? Memes { get; set; }

        [Display(Name = "Memes")]
        public virtual ICollection<Tag.MemesMap> MemesMaps { get; set; } = new HashSet<Tag.MemesMap>();

        public class MemesMap
        {
            public int TagId { get; set; }

            public int MemeId { get; set; }
        }
    }
}
