using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;

namespace DevMagicMemesWebApi.Contracts
{
    /// <summary>
    /// Meme
    /// </summary>
    public partial class Meme
    {
        /// <summary>
        /// Data unique identifier.
        /// </summary>
        [Key]
        public int Id { get; set; }

        [Required]
        public string Image { get; set; } = String.Empty;

        public string? Description { get; set; }

        public virtual ICollection<Tag>? Tags { get; set; }

        [Display(Name = "Tags")]
        public virtual ICollection<Tag.MemesMap> TagsMaps { get; set; } = new HashSet<Tag.MemesMap>();
    }
}
