using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityMtwServer.Entities
{
    public class Profile
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public bool LprRecord { get; set; }
        public bool AcessRecord { get; set; }
        public bool AcessPermanent { get; set; }
        public bool AcessVisitor { get; set; }
        public bool AcessModel { get; set; }
        public bool AcessPeriod { get; set; }
        public bool AcessSchedule { get; set; }
        public bool AcessOrigin { get; set; }
        public bool AcessAction { get; set; }
        public bool AcessRestrictedPlate { get; set; }
        public bool AcessLpr { get; set; }
        public bool AcessAnalyzer { get; set; }
        public bool AcessCameraControl { get; set; }
        public bool AcessMasterEye { get; set; }
        public bool AcessTelemetry { get; set; }
        public bool AcessAlarm { get; set; }
        public bool AcessRecordVideo { get; set; }
        public bool AcessAddEquipament { get; set; }
        public bool AcessRemoveEquipment { get; set; }
        public bool AcessEditEquipment { get; set; }
        public bool AcessAddGroup { get; set; }
        public bool AcessRemoveGroup { get; set; }
        public bool AcessEditGroup { get; set; }


    }
}
