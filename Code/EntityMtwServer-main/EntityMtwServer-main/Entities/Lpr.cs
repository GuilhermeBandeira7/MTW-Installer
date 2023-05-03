using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace EntityMtwServer.Entities
{
    public class Lprs
    {
        public long Id { get; set; }
        public string LprName { get; set; } = string.Empty;
        public string CountryCode { get; set; } = string.Empty;
        public bool RecognizeMovement { get; set; }
        public int DatabaseTime { get; set; }
        public int RefreshTime { get; set; }
        public int FalseTime { get; set; }
        public int Threads { get; set; }
        public int Fps { get; set; }
        public int ResultConfirmation { get; set; }
        public int Precision { get; set; }
        public int Rotation { get; set; }
        public int MaxCharHeight { get; set; }
        public int MinCharHeight { get; set; }
        public int ImageTime { get; set; }
        public bool ContextUrl { get; set; }
        public int x1 { get; set; }
        public int x2 { get; set; }
        public int x3 { get; set; }
        public int x4 { get; set; }
        public int y1 { get; set; }
        public int y2 { get; set; }
        public int y3 { get; set; }
        public int y4 { get; set; }
        public string? ZoneX { get; set; } 
        public string? ZoneY { get; set; }
        public bool? LprContextVideo { get; set; }
        public bool? LprContextPhoto { get;set; }
        public int? PlateRecognitionOffset { get; set; }
        public int? RecordRecognitionOffset { get; set; }  

        [ForeignKey("EquipmentId")]
        public Equipment? LprCamera { get; set; } = null;
        [ForeignKey("OriginId")]
        public Origin? Origin { get; set; } = null;
        [ForeignKey("ControllerId")]
        public Equipment? Controller { get; set; } = null;
        [ForeignKey("AcessId")]
        public Equipment? Acess { get; set; } = null;
        [ForeignKey("Context1Id")]
        public Equipment? Context1 { get; set; } = null;
        [ForeignKey("Context2Id")]
        public Equipment? Context2 { get; set; } = null;
        [ForeignKey("Context3Id")]
        public Equipment? Context3 { get; set; } = null;
        [ForeignKey("Context4Id")]
        public Equipment? Context4 { get; set; } = null;
        [ForeignKey("UserAlarmId")]
        public UserAlarm? UserAlarm { get; set; } = null;
        [ForeignKey("LprContextId")]
        public Record? LprContext { get; set; } = null;
    }
}
