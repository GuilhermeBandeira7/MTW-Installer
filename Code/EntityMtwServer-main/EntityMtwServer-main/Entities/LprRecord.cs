using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EntityMtwServer.Entities
{
    public class LprRecord
    {
        [Browsable(false)]
        public long Id { get; set; }

        [Required]
        [Display(ShortName = "Placa", Name = "Placa")]
        public string Plate { get; set; } = string.Empty;

        [Required]
        [Display(ShortName = "Data e Hora", Name = "Data e Hora")]
        public DateTime DateTime { get; set; }

        [Required]
        [Display(ShortName = "Autorização", Name = "Autorização")]
        public string Authorization { get; set; } = string.Empty;

        [Browsable(false)]
        public bool Warning { get; set; }
        [Browsable(false)]
        public bool Alert { get; set; }
        [Browsable(false)]
        public bool Email { get; set; }
        [Browsable(false)]
        public bool Digital { get; set; }
        [Browsable(false)]
        public bool Processed { get; set; }
        [Browsable(false)]
        public string Description { get; set; } = string.Empty;

        [Browsable(false)]
        [NotMapped]
        public List<string> Files { get; set; } = new List<string>();

        [Browsable(false)]
        [ForeignKey("LprId")]
        public Lprs? Lpr { get; set; }

        [Browsable(false)]
        [ForeignKey("VisitorId")]
        public Visitor? Visitor { get; set; }

        [Browsable(false)]
        [ForeignKey("PermanentId")]
        public Permanent? Permanent { get; set; }

        [Browsable(false)]
        [ForeignKey("OriginId")]
        public Origin? Origin { get; set; }
    }
}
