using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EntityMtwServer.Migrations
{
    public partial class initialDB : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CameraControlAlarms",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EquipmentId = table.Column<long>(type: "bigint", nullable: false),
                    InOnAlarm = table.Column<bool>(type: "bit", nullable: false),
                    InOffAlarm = table.Column<bool>(type: "bit", nullable: false),
                    OutOnAlarm = table.Column<bool>(type: "bit", nullable: false),
                    OutOffAlarm = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CameraControlAlarms", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CurriculumCourses",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CurriculumCourses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Equipment",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Ip = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    User = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Tipo = table.Column<int>(type: "int", nullable: false),
                    PrimaryRtsp = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SencondaryRtsp = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PrimaryStreamingRtsp = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SencondaryStreamingRtsp = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Status = table.Column<bool>(type: "bit", nullable: false),
                    DateTime = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Equipment", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Groups",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Time = table.Column<int>(type: "int", nullable: false),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    GroupId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Groups", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Groups_Groups_GroupId",
                        column: x => x.GroupId,
                        principalTable: "Groups",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Periods",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Hours = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Periods", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Profiles",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LprRecord = table.Column<bool>(type: "bit", nullable: false),
                    AcessRecord = table.Column<bool>(type: "bit", nullable: false),
                    AcessPermanent = table.Column<bool>(type: "bit", nullable: false),
                    AcessVisitor = table.Column<bool>(type: "bit", nullable: false),
                    AcessModel = table.Column<bool>(type: "bit", nullable: false),
                    AcessPeriod = table.Column<bool>(type: "bit", nullable: false),
                    AcessSchedule = table.Column<bool>(type: "bit", nullable: false),
                    AcessOrigin = table.Column<bool>(type: "bit", nullable: false),
                    AcessAction = table.Column<bool>(type: "bit", nullable: false),
                    AcessRestrictedPlate = table.Column<bool>(type: "bit", nullable: false),
                    AcessLpr = table.Column<bool>(type: "bit", nullable: false),
                    AcessAnalyzer = table.Column<bool>(type: "bit", nullable: false),
                    AcessCameraControl = table.Column<bool>(type: "bit", nullable: false),
                    AcessMasterEye = table.Column<bool>(type: "bit", nullable: false),
                    AcessTelemetry = table.Column<bool>(type: "bit", nullable: false),
                    AcessAlarm = table.Column<bool>(type: "bit", nullable: false),
                    AcessRecordVideo = table.Column<bool>(type: "bit", nullable: false),
                    AcessAddEquipament = table.Column<bool>(type: "bit", nullable: false),
                    AcessRemoveEquipment = table.Column<bool>(type: "bit", nullable: false),
                    AcessEditEquipment = table.Column<bool>(type: "bit", nullable: false),
                    AcessAddGroup = table.Column<bool>(type: "bit", nullable: false),
                    AcessRemoveGroup = table.Column<bool>(type: "bit", nullable: false),
                    AcessEditGroup = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Profiles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RestrictedPlates",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Plate = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StartValidDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndValidDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RestrictedPlates", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Schedules",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MondayStartTime = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MondayEndTime = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TuesdayStartTime = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TuesdayEndTime = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    WednesdayStartTime = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    WednesdayEndTime = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ThursdayStartTime = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ThursdayEndTime = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FridayStartTime = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FridayEndTime = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SaturdayStartTime = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SaturdayEndTime = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SundayStartTime = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SundayEndTime = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MondayRemote = table.Column<bool>(type: "bit", nullable: false),
                    TuesdayRemote = table.Column<bool>(type: "bit", nullable: false),
                    WednesdayRemote = table.Column<bool>(type: "bit", nullable: false),
                    ThursdayRemote = table.Column<bool>(type: "bit", nullable: false),
                    FridayRemote = table.Column<bool>(type: "bit", nullable: false),
                    SaturdayRemote = table.Column<bool>(type: "bit", nullable: false),
                    SundayRemote = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Schedules", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Teams",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Teams", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TelemetryAlarms",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Online = table.Column<bool>(type: "bit", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Alarm = table.Column<bool>(type: "bit", nullable: false),
                    CameraInActive = table.Column<bool>(type: "bit", nullable: false),
                    CameraIn = table.Column<bool>(type: "bit", nullable: false),
                    CameraInState = table.Column<bool>(type: "bit", nullable: false),
                    CameraOutAcitve = table.Column<bool>(type: "bit", nullable: false),
                    CameraOut = table.Column<bool>(type: "bit", nullable: false),
                    CameraOutState = table.Column<bool>(type: "bit", nullable: false),
                    Slot0In1Active = table.Column<bool>(type: "bit", nullable: false),
                    Slot0In1 = table.Column<bool>(type: "bit", nullable: false),
                    Slot0In1State = table.Column<bool>(type: "bit", nullable: false),
                    Slot0Out1Active = table.Column<bool>(type: "bit", nullable: false),
                    Slot0Out1 = table.Column<bool>(type: "bit", nullable: false),
                    Slot0Out1State = table.Column<bool>(type: "bit", nullable: false),
                    Slot0Dc1Min = table.Column<int>(type: "int", nullable: false),
                    Slot0Dc1Max = table.Column<int>(type: "int", nullable: false),
                    Slot0Dc1State = table.Column<bool>(type: "bit", nullable: false),
                    Slot0Dc2Min = table.Column<int>(type: "int", nullable: false),
                    Slot0Dc2Max = table.Column<int>(type: "int", nullable: false),
                    Slot0Dc2State = table.Column<bool>(type: "bit", nullable: false),
                    Slot0TempMin = table.Column<int>(type: "int", nullable: false),
                    Slot0TempMax = table.Column<int>(type: "int", nullable: false),
                    Slot0TempState = table.Column<bool>(type: "bit", nullable: false),
                    Slot0HumMin = table.Column<int>(type: "int", nullable: false),
                    Slot0HumMax = table.Column<int>(type: "int", nullable: false),
                    Slot0HumState = table.Column<bool>(type: "bit", nullable: false),
                    Slot2In1Active = table.Column<bool>(type: "bit", nullable: false),
                    Slot2In1 = table.Column<bool>(type: "bit", nullable: false),
                    Slot2In1State = table.Column<bool>(type: "bit", nullable: false),
                    Slot2Out1Active = table.Column<bool>(type: "bit", nullable: false),
                    Slot2Out1 = table.Column<bool>(type: "bit", nullable: false),
                    Slot2Out1State = table.Column<bool>(type: "bit", nullable: false),
                    Slot2In2Active = table.Column<bool>(type: "bit", nullable: false),
                    Slot2In2 = table.Column<bool>(type: "bit", nullable: false),
                    Slot2In2State = table.Column<bool>(type: "bit", nullable: false),
                    Slot2Out2Active = table.Column<bool>(type: "bit", nullable: false),
                    Slot2Out2 = table.Column<bool>(type: "bit", nullable: false),
                    Slot2Out2State = table.Column<bool>(type: "bit", nullable: false),
                    Slot2In3Active = table.Column<bool>(type: "bit", nullable: false),
                    Slot2In3 = table.Column<bool>(type: "bit", nullable: false),
                    Slot2In3State = table.Column<bool>(type: "bit", nullable: false),
                    Slot2Out3Active = table.Column<bool>(type: "bit", nullable: false),
                    Slot2Out3 = table.Column<bool>(type: "bit", nullable: false),
                    Slot2Out3State = table.Column<bool>(type: "bit", nullable: false),
                    Slot2Dc1Min = table.Column<int>(type: "int", nullable: false),
                    Slot2Dc1Max = table.Column<int>(type: "int", nullable: false),
                    Slot2Dc1State = table.Column<bool>(type: "bit", nullable: false),
                    Slot2Dc2Min = table.Column<int>(type: "int", nullable: false),
                    Slot2Dc2Max = table.Column<int>(type: "int", nullable: false),
                    Slot2Dc2State = table.Column<bool>(type: "bit", nullable: false),
                    Slot2Ac1Min = table.Column<int>(type: "int", nullable: false),
                    Slot2Ac1Max = table.Column<int>(type: "int", nullable: false),
                    Slot2Ac1State = table.Column<bool>(type: "bit", nullable: false),
                    Slot2Ac2Min = table.Column<int>(type: "int", nullable: false),
                    Slot2Ac2Max = table.Column<int>(type: "int", nullable: false),
                    Slot2Ac2State = table.Column<bool>(type: "bit", nullable: false),
                    Slot2Ac3Max = table.Column<int>(type: "int", nullable: false),
                    Slot2Ac3Min = table.Column<int>(type: "int", nullable: false),
                    Slot2Ac3State = table.Column<bool>(type: "bit", nullable: false),
                    Slot3In1Active = table.Column<bool>(type: "bit", nullable: false),
                    Slot3In1 = table.Column<bool>(type: "bit", nullable: false),
                    Slot3In1State = table.Column<bool>(type: "bit", nullable: false),
                    Slot3Out1Active = table.Column<bool>(type: "bit", nullable: false),
                    Slot3Out1 = table.Column<bool>(type: "bit", nullable: false),
                    Slot3Out1State = table.Column<bool>(type: "bit", nullable: false),
                    Slot3In2Active = table.Column<bool>(type: "bit", nullable: false),
                    Slot3In2 = table.Column<bool>(type: "bit", nullable: false),
                    Slot3In2State = table.Column<bool>(type: "bit", nullable: false),
                    Slot3Out2Active = table.Column<bool>(type: "bit", nullable: false),
                    Slot3Out2 = table.Column<bool>(type: "bit", nullable: false),
                    Slot3Out2State = table.Column<bool>(type: "bit", nullable: false),
                    Slot3In3Active = table.Column<bool>(type: "bit", nullable: false),
                    Slot3In3 = table.Column<bool>(type: "bit", nullable: false),
                    Slot3In3State = table.Column<bool>(type: "bit", nullable: false),
                    Slot3Out3Active = table.Column<bool>(type: "bit", nullable: false),
                    Slot3Out3 = table.Column<bool>(type: "bit", nullable: false),
                    Slot3Out3State = table.Column<bool>(type: "bit", nullable: false),
                    Slot3Dc1Min = table.Column<int>(type: "int", nullable: false),
                    Slot3Dc1Max = table.Column<int>(type: "int", nullable: false),
                    Slot3Dc1State = table.Column<bool>(type: "bit", nullable: false),
                    Slot3Dc2Min = table.Column<int>(type: "int", nullable: false),
                    Slot3Dc2Max = table.Column<int>(type: "int", nullable: false),
                    Slot3Dc2State = table.Column<bool>(type: "bit", nullable: false),
                    Slot3Ac1Min = table.Column<int>(type: "int", nullable: false),
                    Slot3Ac1Max = table.Column<int>(type: "int", nullable: false),
                    Slot3Ac1State = table.Column<bool>(type: "bit", nullable: false),
                    Slot3Ac2Min = table.Column<int>(type: "int", nullable: false),
                    Slot3Ac2Max = table.Column<int>(type: "int", nullable: false),
                    Slot3Ac2State = table.Column<bool>(type: "bit", nullable: false),
                    Slot3Ac3Max = table.Column<int>(type: "int", nullable: false),
                    Slot3Ac3Min = table.Column<int>(type: "int", nullable: false),
                    Slot3Ac3State = table.Column<bool>(type: "bit", nullable: false),
                    Slot4In1Active = table.Column<bool>(type: "bit", nullable: false),
                    Slot4In1 = table.Column<bool>(type: "bit", nullable: false),
                    Slot4In1State = table.Column<bool>(type: "bit", nullable: false),
                    Slot4Out1Active = table.Column<bool>(type: "bit", nullable: false),
                    Slot4Out1 = table.Column<bool>(type: "bit", nullable: false),
                    Slot4Out1State = table.Column<bool>(type: "bit", nullable: false),
                    Slot4In2Active = table.Column<bool>(type: "bit", nullable: false),
                    Slot4In2 = table.Column<bool>(type: "bit", nullable: false),
                    Slot4In2State = table.Column<bool>(type: "bit", nullable: false),
                    Slot4Out2Active = table.Column<bool>(type: "bit", nullable: false),
                    Slot4Out2 = table.Column<bool>(type: "bit", nullable: false),
                    Slot4Out2State = table.Column<bool>(type: "bit", nullable: false),
                    Slot4In3Active = table.Column<bool>(type: "bit", nullable: false),
                    Slot4In3 = table.Column<bool>(type: "bit", nullable: false),
                    Slot4In3State = table.Column<bool>(type: "bit", nullable: false),
                    Slot4Out3Active = table.Column<bool>(type: "bit", nullable: false),
                    Slot4Out3 = table.Column<bool>(type: "bit", nullable: false),
                    Slot4Out3State = table.Column<bool>(type: "bit", nullable: false),
                    Slot4Dc1Min = table.Column<int>(type: "int", nullable: false),
                    Slot4Dc1Max = table.Column<int>(type: "int", nullable: false),
                    Slot4Dc1State = table.Column<bool>(type: "bit", nullable: false),
                    Slot4Dc2Min = table.Column<int>(type: "int", nullable: false),
                    Slot4Dc2Max = table.Column<int>(type: "int", nullable: false),
                    Slot4Dc2State = table.Column<bool>(type: "bit", nullable: false),
                    Slot4Ac1Min = table.Column<int>(type: "int", nullable: false),
                    Slot4Ac1Max = table.Column<int>(type: "int", nullable: false),
                    Slot4Ac1State = table.Column<bool>(type: "bit", nullable: false),
                    Slot4Ac2Min = table.Column<int>(type: "int", nullable: false),
                    Slot4Ac2Max = table.Column<int>(type: "int", nullable: false),
                    Slot4Ac2State = table.Column<bool>(type: "bit", nullable: false),
                    Slot4Ac3Max = table.Column<int>(type: "int", nullable: false),
                    Slot4Ac3Min = table.Column<int>(type: "int", nullable: false),
                    Slot4Ac3State = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TelemetryAlarms", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TypeFields",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Field = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TypeFields", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserAlarms",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SmtpServer = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SmtpPort = table.Column<int>(type: "int", nullable: false),
                    OriginEmail = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TargetEmail = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TargetTelephone = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SmsCheck = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserAlarms", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Vehicles",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Brand = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Vehicles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Courses",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Duration = table.Column<int>(type: "int", nullable: false),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CurriculumCourseId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Courses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Courses_CurriculumCourses_CurriculumCourseId",
                        column: x => x.CurriculumCourseId,
                        principalTable: "CurriculumCourses",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Record",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false),
                    RecordPath = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Duration = table.Column<int>(type: "int", nullable: false),
                    RemoteUser = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RemotePassword = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Record", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Record_Equipment_Id",
                        column: x => x.Id,
                        principalTable: "Equipment",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Server",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false),
                    TelemetryServer = table.Column<bool>(type: "bit", nullable: false),
                    LprServer = table.Column<bool>(type: "bit", nullable: false),
                    MasterEyeServer = table.Column<bool>(type: "bit", nullable: false),
                    DigifortServer = table.Column<bool>(type: "bit", nullable: false),
                    RecordServer = table.Column<bool>(type: "bit", nullable: false),
                    SessionServer = table.Column<bool>(type: "bit", nullable: false),
                    RtspServer = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Server", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Server_Equipment_Id",
                        column: x => x.Id,
                        principalTable: "Equipment",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Telemetry",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false),
                    SerialNumber = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Telemetry", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Telemetry_Equipment_Id",
                        column: x => x.Id,
                        principalTable: "Equipment",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "EquipmentGroup",
                columns: table => new
                {
                    EquipmentsId = table.Column<long>(type: "bigint", nullable: false),
                    GroupsId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EquipmentGroup", x => new { x.EquipmentsId, x.GroupsId });
                    table.ForeignKey(
                        name: "FK_EquipmentGroup_Equipment_EquipmentsId",
                        column: x => x.EquipmentsId,
                        principalTable: "Equipment",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_EquipmentGroup_Groups_GroupsId",
                        column: x => x.GroupsId,
                        principalTable: "Groups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Registration = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Login = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Image = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Active = table.Column<bool>(type: "bit", nullable: false),
                    ProfileId = table.Column<long>(type: "bigint", nullable: true),
                    TeamId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Users_Profiles_ProfileId",
                        column: x => x.ProfileId,
                        principalTable: "Profiles",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Users_Teams_TeamId",
                        column: x => x.TeamId,
                        principalTable: "Teams",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Permanents",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ScheduleId = table.Column<long>(type: "bigint", nullable: false),
                    VehicleId = table.Column<long>(type: "bigint", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    JobTitle = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Plate = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Registration = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StartValidDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndValidDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Permanents", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Permanents_Schedules_ScheduleId",
                        column: x => x.ScheduleId,
                        principalTable: "Schedules",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_Permanents_Vehicles_VehicleId",
                        column: x => x.VehicleId,
                        principalTable: "Vehicles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "Visitors",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    VehicleId = table.Column<long>(type: "bigint", nullable: false),
                    PeriodId = table.Column<long>(type: "bigint", nullable: false),
                    Plate = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AuthorizationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Visitors", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Visitors_Periods_PeriodId",
                        column: x => x.PeriodId,
                        principalTable: "Periods",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_Visitors_Vehicles_VehicleId",
                        column: x => x.VehicleId,
                        principalTable: "Vehicles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "EquipmentServer",
                columns: table => new
                {
                    ServerEquipmentsId = table.Column<long>(type: "bigint", nullable: false),
                    ServersId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EquipmentServer", x => new { x.ServerEquipmentsId, x.ServersId });
                    table.ForeignKey(
                        name: "FK_EquipmentServer_Equipment_ServerEquipmentsId",
                        column: x => x.ServerEquipmentsId,
                        principalTable: "Equipment",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_EquipmentServer_Server_ServersId",
                        column: x => x.ServersId,
                        principalTable: "Server",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "GroupServer",
                columns: table => new
                {
                    ServerGroupsId = table.Column<long>(type: "bigint", nullable: false),
                    ServersId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GroupServer", x => new { x.ServerGroupsId, x.ServersId });
                    table.ForeignKey(
                        name: "FK_GroupServer_Groups_ServerGroupsId",
                        column: x => x.ServerGroupsId,
                        principalTable: "Groups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_GroupServer_Server_ServersId",
                        column: x => x.ServersId,
                        principalTable: "Server",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "CameraControl",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false),
                    Model = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CommandPort = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MediaPort = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CameraInOnName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CameraInOffName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CameraOutOnName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CameraOutOffName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CameraInOnColor = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CameraInOffColor = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CameraOutOnColor = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CameraOutOffColor = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CameraOutputPulse = table.Column<bool>(type: "bit", nullable: false),
                    CameraOutputPeriod = table.Column<int>(type: "int", nullable: false),
                    CameraInStatus = table.Column<bool>(type: "bit", nullable: false),
                    CameraOutStatus = table.Column<bool>(type: "bit", nullable: false),
                    alarmTime = table.Column<int>(type: "int", nullable: false),
                    TelemetryId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CameraControl", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CameraControl_Equipment_Id",
                        column: x => x.Id,
                        principalTable: "Equipment",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_CameraControl_Telemetry_TelemetryId",
                        column: x => x.TelemetryId,
                        principalTable: "Telemetry",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "DVC",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false),
                    SerialNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Function = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OperationalSystem = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    VideoEnable = table.Column<bool>(type: "bit", nullable: false),
                    AudioEnable = table.Column<bool>(type: "bit", nullable: false),
                    PermanentStream = table.Column<bool>(type: "bit", nullable: false),
                    StatusDateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TelemetryId = table.Column<long>(type: "bigint", nullable: true),
                    ServerId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DVC", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DVC_Equipment_Id",
                        column: x => x.Id,
                        principalTable: "Equipment",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_DVC_Server_ServerId",
                        column: x => x.ServerId,
                        principalTable: "Server",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_DVC_Telemetry_TelemetryId",
                        column: x => x.TelemetryId,
                        principalTable: "Telemetry",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Gateways",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GatewayIp = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    GatewayPort = table.Column<int>(type: "int", nullable: false),
                    ServerRemoto = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TelemetryId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Gateways", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Gateways_Telemetry_TelemetryId",
                        column: x => x.TelemetryId,
                        principalTable: "Telemetry",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Origins",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OriginCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TelemetryId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Origins", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Origins_Telemetry_TelemetryId",
                        column: x => x.TelemetryId,
                        principalTable: "Telemetry",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "TelemetryMessages",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TelemetryId = table.Column<long>(type: "bigint", nullable: false),
                    Slot = table.Column<int>(type: "int", nullable: false),
                    Message = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TelemetryMessages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TelemetryMessages_Telemetry_TelemetryId",
                        column: x => x.TelemetryId,
                        principalTable: "Telemetry",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "EquipmentUser",
                columns: table => new
                {
                    EquipmentsId = table.Column<long>(type: "bigint", nullable: false),
                    UsersId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EquipmentUser", x => new { x.EquipmentsId, x.UsersId });
                    table.ForeignKey(
                        name: "FK_EquipmentUser_Equipment_EquipmentsId",
                        column: x => x.EquipmentsId,
                        principalTable: "Equipment",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_EquipmentUser_Users_UsersId",
                        column: x => x.UsersId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "GroupUser",
                columns: table => new
                {
                    GroupsId = table.Column<long>(type: "bigint", nullable: false),
                    UsersId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GroupUser", x => new { x.GroupsId, x.UsersId });
                    table.ForeignKey(
                        name: "FK_GroupUser_Groups_GroupsId",
                        column: x => x.GroupsId,
                        principalTable: "Groups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_GroupUser_Users_UsersId",
                        column: x => x.UsersId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "Classes",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CourseId = table.Column<long>(type: "bigint", nullable: true),
                    CurriculumCourseId = table.Column<long>(type: "bigint", nullable: true),
                    Theme = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Color = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    State = table.Column<bool>(type: "bit", nullable: false),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StartDateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    InstructorId = table.Column<long>(type: "bigint", nullable: true),
                    ScheduleId = table.Column<long>(type: "bigint", nullable: true),
                    InstructorDeviceId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Classes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Classes_Courses_CourseId",
                        column: x => x.CourseId,
                        principalTable: "Courses",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Classes_CurriculumCourses_CurriculumCourseId",
                        column: x => x.CurriculumCourseId,
                        principalTable: "CurriculumCourses",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Classes_DVC_InstructorDeviceId",
                        column: x => x.InstructorDeviceId,
                        principalTable: "DVC",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Classes_Schedules_ScheduleId",
                        column: x => x.ScheduleId,
                        principalTable: "Schedules",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Classes_Users_InstructorId",
                        column: x => x.InstructorId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Actions",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Authorization = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TypePerson = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Plate = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ActionValue = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Parameters = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OriginId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Actions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Actions_Origins_OriginId",
                        column: x => x.OriginId,
                        principalTable: "Origins",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Lprs",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LprName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OriginId = table.Column<long>(type: "bigint", nullable: false),
                    ControllerId = table.Column<long>(type: "bigint", nullable: false),
                    AcessId = table.Column<long>(type: "bigint", nullable: false),
                    CountryCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RecognizeMovement = table.Column<bool>(type: "bit", nullable: false),
                    DatabaseTime = table.Column<int>(type: "int", nullable: false),
                    RefreshTime = table.Column<int>(type: "int", nullable: false),
                    FalseTime = table.Column<int>(type: "int", nullable: false),
                    Threads = table.Column<int>(type: "int", nullable: false),
                    Fps = table.Column<int>(type: "int", nullable: false),
                    ResultConfirmation = table.Column<int>(type: "int", nullable: false),
                    Precision = table.Column<int>(type: "int", nullable: false),
                    Rotation = table.Column<int>(type: "int", nullable: false),
                    MaxCharHeight = table.Column<int>(type: "int", nullable: false),
                    MinCharHeight = table.Column<int>(type: "int", nullable: false),
                    ImageTime = table.Column<int>(type: "int", nullable: false),
                    ContextUrl = table.Column<bool>(type: "bit", nullable: false),
                    Context1Id = table.Column<long>(type: "bigint", nullable: false),
                    Context2Id = table.Column<long>(type: "bigint", nullable: false),
                    Context3Id = table.Column<long>(type: "bigint", nullable: false),
                    Context4Id = table.Column<long>(type: "bigint", nullable: false),
                    x1 = table.Column<int>(type: "int", nullable: false),
                    x2 = table.Column<int>(type: "int", nullable: false),
                    x3 = table.Column<int>(type: "int", nullable: false),
                    x4 = table.Column<int>(type: "int", nullable: false),
                    y1 = table.Column<int>(type: "int", nullable: false),
                    y2 = table.Column<int>(type: "int", nullable: false),
                    y3 = table.Column<int>(type: "int", nullable: false),
                    y4 = table.Column<int>(type: "int", nullable: false),
                    EquipmentId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Lprs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Lprs_Equipment_AcessId",
                        column: x => x.AcessId,
                        principalTable: "Equipment",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_Lprs_Equipment_Context1Id",
                        column: x => x.Context1Id,
                        principalTable: "Equipment",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_Lprs_Equipment_Context2Id",
                        column: x => x.Context2Id,
                        principalTable: "Equipment",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_Lprs_Equipment_Context3Id",
                        column: x => x.Context3Id,
                        principalTable: "Equipment",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_Lprs_Equipment_Context4Id",
                        column: x => x.Context4Id,
                        principalTable: "Equipment",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_Lprs_Equipment_ControllerId",
                        column: x => x.ControllerId,
                        principalTable: "Equipment",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_Lprs_Equipment_EquipmentId",
                        column: x => x.EquipmentId,
                        principalTable: "Equipment",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Lprs_Origins_OriginId",
                        column: x => x.OriginId,
                        principalTable: "Origins",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "Sessions",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StartDateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Record = table.Column<bool>(type: "bit", nullable: false),
                    RecordPath = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Status = table.Column<bool>(type: "bit", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MainRtsp = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SubRtsp = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Requisition = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Color = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Live = table.Column<bool>(type: "bit", nullable: false),
                    Filter = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ClassId = table.Column<long>(type: "bigint", nullable: true),
                    InstructorId = table.Column<long>(type: "bigint", nullable: true),
                    InstructorDeviceId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sessions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Sessions_Classes_ClassId",
                        column: x => x.ClassId,
                        principalTable: "Classes",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Sessions_DVC_InstructorDeviceId",
                        column: x => x.InstructorDeviceId,
                        principalTable: "DVC",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Sessions_Users_InstructorId",
                        column: x => x.InstructorId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Students",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Grade = table.Column<int>(type: "int", nullable: false),
                    LiveHours = table.Column<double>(type: "float", nullable: false),
                    RemoteHours = table.Column<double>(type: "float", nullable: false),
                    LiveClass = table.Column<int>(type: "int", nullable: false),
                    RemoteClass = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<long>(type: "bigint", nullable: true),
                    ClassId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Students", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Students_Classes_ClassId",
                        column: x => x.ClassId,
                        principalTable: "Classes",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Students_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "LprRecords",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Plate = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Authorization = table.Column<bool>(type: "bit", nullable: false),
                    Warning = table.Column<bool>(type: "bit", nullable: false),
                    Alert = table.Column<bool>(type: "bit", nullable: false),
                    Email = table.Column<bool>(type: "bit", nullable: false),
                    Digital = table.Column<bool>(type: "bit", nullable: false),
                    Processed = table.Column<bool>(type: "bit", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LprId = table.Column<long>(type: "bigint", nullable: false),
                    VisitorId = table.Column<long>(type: "bigint", nullable: true),
                    PermanentId = table.Column<long>(type: "bigint", nullable: true),
                    OriginId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LprRecords", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LprRecords_Lprs_LprId",
                        column: x => x.LprId,
                        principalTable: "Lprs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_LprRecords_Origins_OriginId",
                        column: x => x.OriginId,
                        principalTable: "Origins",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_LprRecords_Permanents_PermanentId",
                        column: x => x.PermanentId,
                        principalTable: "Permanents",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_LprRecords_Visitors_VisitorId",
                        column: x => x.VisitorId,
                        principalTable: "Visitors",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "EquipmentSession",
                columns: table => new
                {
                    EquipmentsId = table.Column<long>(type: "bigint", nullable: false),
                    SessionsId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EquipmentSession", x => new { x.EquipmentsId, x.SessionsId });
                    table.ForeignKey(
                        name: "FK_EquipmentSession_Equipment_EquipmentsId",
                        column: x => x.EquipmentsId,
                        principalTable: "Equipment",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_EquipmentSession_Sessions_SessionsId",
                        column: x => x.SessionsId,
                        principalTable: "Sessions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "Events",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Duration = table.Column<double>(type: "float", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SessionId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Events", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Events_Sessions_SessionId",
                        column: x => x.SessionId,
                        principalTable: "Sessions",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "GroupSession",
                columns: table => new
                {
                    CellsId = table.Column<long>(type: "bigint", nullable: false),
                    SessionsId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GroupSession", x => new { x.CellsId, x.SessionsId });
                    table.ForeignKey(
                        name: "FK_GroupSession_Groups_CellsId",
                        column: x => x.CellsId,
                        principalTable: "Groups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_GroupSession_Sessions_SessionsId",
                        column: x => x.SessionsId,
                        principalTable: "Sessions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "SessionStudent",
                columns: table => new
                {
                    AttendedClassesId = table.Column<long>(type: "bigint", nullable: false),
                    StudentsId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SessionStudent", x => new { x.AttendedClassesId, x.StudentsId });
                    table.ForeignKey(
                        name: "FK_SessionStudent_Sessions_AttendedClassesId",
                        column: x => x.AttendedClassesId,
                        principalTable: "Sessions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_SessionStudent_Students_StudentsId",
                        column: x => x.StudentsId,
                        principalTable: "Students",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Actions_OriginId",
                table: "Actions",
                column: "OriginId");

            migrationBuilder.CreateIndex(
                name: "IX_CameraControl_TelemetryId",
                table: "CameraControl",
                column: "TelemetryId");

            migrationBuilder.CreateIndex(
                name: "IX_Classes_CourseId",
                table: "Classes",
                column: "CourseId");

            migrationBuilder.CreateIndex(
                name: "IX_Classes_CurriculumCourseId",
                table: "Classes",
                column: "CurriculumCourseId");

            migrationBuilder.CreateIndex(
                name: "IX_Classes_InstructorDeviceId",
                table: "Classes",
                column: "InstructorDeviceId");

            migrationBuilder.CreateIndex(
                name: "IX_Classes_InstructorId",
                table: "Classes",
                column: "InstructorId");

            migrationBuilder.CreateIndex(
                name: "IX_Classes_ScheduleId",
                table: "Classes",
                column: "ScheduleId");

            migrationBuilder.CreateIndex(
                name: "IX_Courses_CurriculumCourseId",
                table: "Courses",
                column: "CurriculumCourseId");

            migrationBuilder.CreateIndex(
                name: "IX_DVC_ServerId",
                table: "DVC",
                column: "ServerId");

            migrationBuilder.CreateIndex(
                name: "IX_DVC_TelemetryId",
                table: "DVC",
                column: "TelemetryId");

            migrationBuilder.CreateIndex(
                name: "IX_EquipmentGroup_GroupsId",
                table: "EquipmentGroup",
                column: "GroupsId");

            migrationBuilder.CreateIndex(
                name: "IX_EquipmentServer_ServersId",
                table: "EquipmentServer",
                column: "ServersId");

            migrationBuilder.CreateIndex(
                name: "IX_EquipmentSession_SessionsId",
                table: "EquipmentSession",
                column: "SessionsId");

            migrationBuilder.CreateIndex(
                name: "IX_EquipmentUser_UsersId",
                table: "EquipmentUser",
                column: "UsersId");

            migrationBuilder.CreateIndex(
                name: "IX_Events_SessionId",
                table: "Events",
                column: "SessionId");

            migrationBuilder.CreateIndex(
                name: "IX_Gateways_TelemetryId",
                table: "Gateways",
                column: "TelemetryId");

            migrationBuilder.CreateIndex(
                name: "IX_Groups_GroupId",
                table: "Groups",
                column: "GroupId");

            migrationBuilder.CreateIndex(
                name: "IX_GroupServer_ServersId",
                table: "GroupServer",
                column: "ServersId");

            migrationBuilder.CreateIndex(
                name: "IX_GroupSession_SessionsId",
                table: "GroupSession",
                column: "SessionsId");

            migrationBuilder.CreateIndex(
                name: "IX_GroupUser_UsersId",
                table: "GroupUser",
                column: "UsersId");

            migrationBuilder.CreateIndex(
                name: "IX_LprRecords_LprId",
                table: "LprRecords",
                column: "LprId");

            migrationBuilder.CreateIndex(
                name: "IX_LprRecords_OriginId",
                table: "LprRecords",
                column: "OriginId");

            migrationBuilder.CreateIndex(
                name: "IX_LprRecords_PermanentId",
                table: "LprRecords",
                column: "PermanentId");

            migrationBuilder.CreateIndex(
                name: "IX_LprRecords_VisitorId",
                table: "LprRecords",
                column: "VisitorId");

            migrationBuilder.CreateIndex(
                name: "IX_Lprs_AcessId",
                table: "Lprs",
                column: "AcessId");

            migrationBuilder.CreateIndex(
                name: "IX_Lprs_Context1Id",
                table: "Lprs",
                column: "Context1Id");

            migrationBuilder.CreateIndex(
                name: "IX_Lprs_Context2Id",
                table: "Lprs",
                column: "Context2Id");

            migrationBuilder.CreateIndex(
                name: "IX_Lprs_Context3Id",
                table: "Lprs",
                column: "Context3Id");

            migrationBuilder.CreateIndex(
                name: "IX_Lprs_Context4Id",
                table: "Lprs",
                column: "Context4Id");

            migrationBuilder.CreateIndex(
                name: "IX_Lprs_ControllerId",
                table: "Lprs",
                column: "ControllerId");

            migrationBuilder.CreateIndex(
                name: "IX_Lprs_EquipmentId",
                table: "Lprs",
                column: "EquipmentId");

            migrationBuilder.CreateIndex(
                name: "IX_Lprs_OriginId",
                table: "Lprs",
                column: "OriginId");

            migrationBuilder.CreateIndex(
                name: "IX_Origins_TelemetryId",
                table: "Origins",
                column: "TelemetryId");

            migrationBuilder.CreateIndex(
                name: "IX_Permanents_ScheduleId",
                table: "Permanents",
                column: "ScheduleId");

            migrationBuilder.CreateIndex(
                name: "IX_Permanents_VehicleId",
                table: "Permanents",
                column: "VehicleId");

            migrationBuilder.CreateIndex(
                name: "IX_Sessions_ClassId",
                table: "Sessions",
                column: "ClassId");

            migrationBuilder.CreateIndex(
                name: "IX_Sessions_InstructorDeviceId",
                table: "Sessions",
                column: "InstructorDeviceId");

            migrationBuilder.CreateIndex(
                name: "IX_Sessions_InstructorId",
                table: "Sessions",
                column: "InstructorId");

            migrationBuilder.CreateIndex(
                name: "IX_SessionStudent_StudentsId",
                table: "SessionStudent",
                column: "StudentsId");

            migrationBuilder.CreateIndex(
                name: "IX_Students_ClassId",
                table: "Students",
                column: "ClassId");

            migrationBuilder.CreateIndex(
                name: "IX_Students_UserId",
                table: "Students",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_TelemetryMessages_TelemetryId",
                table: "TelemetryMessages",
                column: "TelemetryId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_ProfileId",
                table: "Users",
                column: "ProfileId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_TeamId",
                table: "Users",
                column: "TeamId");

            migrationBuilder.CreateIndex(
                name: "IX_Visitors_PeriodId",
                table: "Visitors",
                column: "PeriodId");

            migrationBuilder.CreateIndex(
                name: "IX_Visitors_VehicleId",
                table: "Visitors",
                column: "VehicleId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Actions");

            migrationBuilder.DropTable(
                name: "CameraControl");

            migrationBuilder.DropTable(
                name: "CameraControlAlarms");

            migrationBuilder.DropTable(
                name: "EquipmentGroup");

            migrationBuilder.DropTable(
                name: "EquipmentServer");

            migrationBuilder.DropTable(
                name: "EquipmentSession");

            migrationBuilder.DropTable(
                name: "EquipmentUser");

            migrationBuilder.DropTable(
                name: "Events");

            migrationBuilder.DropTable(
                name: "Gateways");

            migrationBuilder.DropTable(
                name: "GroupServer");

            migrationBuilder.DropTable(
                name: "GroupSession");

            migrationBuilder.DropTable(
                name: "GroupUser");

            migrationBuilder.DropTable(
                name: "LprRecords");

            migrationBuilder.DropTable(
                name: "Record");

            migrationBuilder.DropTable(
                name: "RestrictedPlates");

            migrationBuilder.DropTable(
                name: "SessionStudent");

            migrationBuilder.DropTable(
                name: "TelemetryAlarms");

            migrationBuilder.DropTable(
                name: "TelemetryMessages");

            migrationBuilder.DropTable(
                name: "TypeFields");

            migrationBuilder.DropTable(
                name: "UserAlarms");

            migrationBuilder.DropTable(
                name: "Groups");

            migrationBuilder.DropTable(
                name: "Lprs");

            migrationBuilder.DropTable(
                name: "Permanents");

            migrationBuilder.DropTable(
                name: "Visitors");

            migrationBuilder.DropTable(
                name: "Sessions");

            migrationBuilder.DropTable(
                name: "Students");

            migrationBuilder.DropTable(
                name: "Origins");

            migrationBuilder.DropTable(
                name: "Periods");

            migrationBuilder.DropTable(
                name: "Vehicles");

            migrationBuilder.DropTable(
                name: "Classes");

            migrationBuilder.DropTable(
                name: "Courses");

            migrationBuilder.DropTable(
                name: "DVC");

            migrationBuilder.DropTable(
                name: "Schedules");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "CurriculumCourses");

            migrationBuilder.DropTable(
                name: "Server");

            migrationBuilder.DropTable(
                name: "Telemetry");

            migrationBuilder.DropTable(
                name: "Profiles");

            migrationBuilder.DropTable(
                name: "Teams");

            migrationBuilder.DropTable(
                name: "Equipment");
        }
    }
}
