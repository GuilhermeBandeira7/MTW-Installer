USE [master]
GO
/****** Object:  Database [TelemetryHub]    Script Date: 04/28/2023 2:14:47 PM ******/
CREATE DATABASE [TelemetryHub]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'TelemetryHub', FILENAME = N'/var/opt/mssql/data/TelemetryHub.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'TelemetryHub_log', FILENAME = N'/var/opt/mssql/data/TelemetryHub_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
GO
ALTER DATABASE [TelemetryHub] SET COMPATIBILITY_LEVEL = 140
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [TelemetryHub].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [TelemetryHub] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [TelemetryHub] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [TelemetryHub] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [TelemetryHub] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [TelemetryHub] SET ARITHABORT OFF 
GO
ALTER DATABASE [TelemetryHub] SET AUTO_CLOSE ON 
GO
ALTER DATABASE [TelemetryHub] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [TelemetryHub] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [TelemetryHub] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [TelemetryHub] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [TelemetryHub] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [TelemetryHub] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [TelemetryHub] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [TelemetryHub] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [TelemetryHub] SET  ENABLE_BROKER 
GO
ALTER DATABASE [TelemetryHub] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [TelemetryHub] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [TelemetryHub] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [TelemetryHub] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [TelemetryHub] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [TelemetryHub] SET READ_COMMITTED_SNAPSHOT ON 
GO
ALTER DATABASE [TelemetryHub] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [TelemetryHub] SET RECOVERY FULL 
GO
ALTER DATABASE [TelemetryHub] SET  MULTI_USER 
GO
ALTER DATABASE [TelemetryHub] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [TelemetryHub] SET DB_CHAINING OFF 
GO
ALTER DATABASE [TelemetryHub] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [TelemetryHub] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [TelemetryHub] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [TelemetryHub] SET QUERY_STORE = OFF
GO
USE [TelemetryHub]
GO
/****** Object:  Table [dbo].[__EFMigrationsHistory]    Script Date: 04/28/2023 2:14:47 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[__EFMigrationsHistory](
	[MigrationId] [nvarchar](150) NOT NULL,
	[ProductVersion] [nvarchar](32) NOT NULL,
 CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY CLUSTERED 
(
	[MigrationId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Actions]    Script Date: 04/28/2023 2:14:47 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Actions](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](max) NOT NULL,
	[Operation] [nvarchar](max) NOT NULL,
	[Time] [int] NOT NULL,
	[Enable] [bit] NOT NULL,
	[VariableId] [bigint] NOT NULL,
	[AlarmId] [bigint] NULL,
 CONSTRAINT [PK_Actions] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ActionUser]    Script Date: 04/28/2023 2:14:47 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ActionUser](
	[ActionsId] [bigint] NOT NULL,
	[UsersId] [bigint] NOT NULL,
 CONSTRAINT [PK_ActionUser] PRIMARY KEY CLUSTERED 
(
	[ActionsId] ASC,
	[UsersId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AlarmConfigurationRecipient]    Script Date: 04/28/2023 2:14:47 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AlarmConfigurationRecipient](
	[AlarmConfigurationsId] [bigint] NOT NULL,
	[RecipientsId] [bigint] NOT NULL,
 CONSTRAINT [PK_AlarmConfigurationRecipient] PRIMARY KEY CLUSTERED 
(
	[AlarmConfigurationsId] ASC,
	[RecipientsId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AlarmConfigurations]    Script Date: 04/28/2023 2:14:47 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AlarmConfigurations](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](max) NOT NULL,
	[SmtpServer] [nvarchar](max) NOT NULL,
	[SmtpPort] [int] NOT NULL,
	[Email] [nvarchar](max) NOT NULL,
	[Password] [nvarchar](max) NOT NULL,
 CONSTRAINT [PK_AlarmConfigurations] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AlarmRecipient]    Script Date: 04/28/2023 2:14:47 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AlarmRecipient](
	[AlarmsId] [bigint] NOT NULL,
	[RecipientsId] [bigint] NOT NULL,
 CONSTRAINT [PK_AlarmRecipient] PRIMARY KEY CLUSTERED 
(
	[AlarmsId] ASC,
	[RecipientsId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Alarms]    Script Date: 04/28/2023 2:14:47 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Alarms](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](max) NOT NULL,
	[Condition] [nvarchar](max) NOT NULL,
	[Sound] [nvarchar](max) NOT NULL,
	[Value] [int] NOT NULL,
	[Hyteresis] [int] NOT NULL,
	[EnableSchedule] [bit] NOT NULL,
	[Enable] [bit] NOT NULL,
	[VariableId] [bigint] NOT NULL,
	[DisplayId] [bigint] NULL,
 CONSTRAINT [PK_Alarms] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AlarmSchedule]    Script Date: 04/28/2023 2:14:47 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AlarmSchedule](
	[AlarmsId] [bigint] NOT NULL,
	[SchedulesId] [bigint] NOT NULL,
 CONSTRAINT [PK_AlarmSchedule] PRIMARY KEY CLUSTERED 
(
	[AlarmsId] ASC,
	[SchedulesId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AlarmUser]    Script Date: 04/28/2023 2:14:47 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AlarmUser](
	[AlarmsId] [bigint] NOT NULL,
	[UsersId] [bigint] NOT NULL,
 CONSTRAINT [PK_AlarmUser] PRIMARY KEY CLUSTERED 
(
	[AlarmsId] ASC,
	[UsersId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ColorConfigs]    Script Date: 04/28/2023 2:14:47 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ColorConfigs](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[Color] [nvarchar](max) NOT NULL,
	[Start] [float] NOT NULL,
	[End] [float] NOT NULL,
	[DisplayId] [bigint] NULL,
 CONSTRAINT [PK_ColorConfigs] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Conditions]    Script Date: 04/28/2023 2:14:47 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Conditions](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[Data] [varbinary](max) NULL,
	[Rule] [nvarchar](max) NOT NULL,
	[Response] [varbinary](max) NULL,
 CONSTRAINT [PK_Conditions] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[DashboardGroup]    Script Date: 04/28/2023 2:14:47 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DashboardGroup](
	[DashboardsId] [bigint] NOT NULL,
	[GroupsId] [bigint] NOT NULL,
 CONSTRAINT [PK_DashboardGroup] PRIMARY KEY CLUSTERED 
(
	[DashboardsId] ASC,
	[GroupsId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Dashboards]    Script Date: 04/28/2023 2:14:47 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Dashboards](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[Description] [nvarchar](max) NULL,
	[Name] [nvarchar](max) NULL,
	[Enable] [bit] NOT NULL,
 CONSTRAINT [PK_Dashboards] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[DashboardUser]    Script Date: 04/28/2023 2:14:47 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DashboardUser](
	[DashboardsId] [bigint] NOT NULL,
	[UsersId] [bigint] NOT NULL,
 CONSTRAINT [PK_DashboardUser] PRIMARY KEY CLUSTERED 
(
	[DashboardsId] ASC,
	[UsersId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Devices]    Script Date: 04/28/2023 2:14:47 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Devices](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[SerialNumber] [nvarchar](max) NOT NULL,
	[Description] [nvarchar](max) NOT NULL,
	[DateTime] [datetime2](7) NOT NULL,
	[Enable] [bit] NOT NULL,
	[Name] [nvarchar](max) NOT NULL,
 CONSTRAINT [PK_Devices] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[DevicesUser]    Script Date: 04/28/2023 2:14:47 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DevicesUser](
	[DevicesId] [bigint] NOT NULL,
	[UsersId] [bigint] NOT NULL,
 CONSTRAINT [PK_DevicesUser] PRIMARY KEY CLUSTERED 
(
	[DevicesId] ASC,
	[UsersId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Displays]    Script Date: 04/28/2023 2:14:47 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Displays](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](max) NOT NULL,
	[Unit] [nvarchar](max) NOT NULL,
	[Subtitle] [nvarchar](max) NOT NULL,
	[Multiplier] [float] NOT NULL,
	[Constant] [float] NOT NULL,
	[Minimum] [float] NOT NULL,
	[Maximum] [float] NOT NULL,
	[Record] [time](7) NULL,
	[Enable] [bit] NOT NULL,
 CONSTRAINT [PK_Displays] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[DisplayUser]    Script Date: 04/28/2023 2:14:47 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DisplayUser](
	[DisplaysId] [bigint] NOT NULL,
	[UsersId] [bigint] NOT NULL,
 CONSTRAINT [PK_DisplayUser] PRIMARY KEY CLUSTERED 
(
	[DisplaysId] ASC,
	[UsersId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Groups]    Script Date: 04/28/2023 2:14:47 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Groups](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](max) NULL,
	[Description] [nvarchar](max) NULL,
	[Position] [int] NOT NULL,
	[Enable] [bit] NOT NULL,
 CONSTRAINT [PK_Groups] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[GroupUser]    Script Date: 04/28/2023 2:14:47 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[GroupUser](
	[GroupsId] [bigint] NOT NULL,
	[UsersId] [bigint] NOT NULL,
 CONSTRAINT [PK_GroupUser] PRIMARY KEY CLUSTERED 
(
	[GroupsId] ASC,
	[UsersId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Layers]    Script Date: 04/28/2023 2:14:47 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Layers](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](max) NOT NULL,
	[Color] [nvarchar](max) NOT NULL,
	[Sound] [nvarchar](max) NOT NULL,
	[Enable] [bit] NOT NULL,
 CONSTRAINT [PK_Layers] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[LayerUser]    Script Date: 04/28/2023 2:14:47 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[LayerUser](
	[LayersId] [bigint] NOT NULL,
	[UsersId] [bigint] NOT NULL,
 CONSTRAINT [PK_LayerUser] PRIMARY KEY CLUSTERED 
(
	[LayersId] ASC,
	[UsersId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Measurers]    Script Date: 04/28/2023 2:14:47 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Measurers](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](max) NOT NULL,
	[GeoPosition] [nvarchar](max) NOT NULL,
	[GeoPlan] [nvarchar](max) NOT NULL,
	[PositionX] [int] NOT NULL,
	[PositionY] [int] NOT NULL,
	[Width] [int] NOT NULL,
	[Height] [int] NOT NULL,
	[Enable] [bit] NOT NULL,
	[DisplayId] [bigint] NOT NULL,
	[GroupId] [bigint] NULL,
	[LayerId] [bigint] NULL,
	[VariableId] [bigint] NOT NULL,
 CONSTRAINT [PK_Measurers] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[MeasurerUser]    Script Date: 04/28/2023 2:14:47 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[MeasurerUser](
	[MeasurersId] [bigint] NOT NULL,
	[UsersId] [bigint] NOT NULL,
 CONSTRAINT [PK_MeasurerUser] PRIMARY KEY CLUSTERED 
(
	[MeasurersId] ASC,
	[UsersId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Recipients]    Script Date: 04/28/2023 2:14:47 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Recipients](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](max) NOT NULL,
	[Email] [nvarchar](max) NOT NULL,
	[Phone] [nvarchar](max) NOT NULL,
	[Enable] [bit] NOT NULL,
 CONSTRAINT [PK_Recipients] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[RecipientUser]    Script Date: 04/28/2023 2:14:47 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[RecipientUser](
	[RecipientsId] [bigint] NOT NULL,
	[UsersId] [bigint] NOT NULL,
 CONSTRAINT [PK_RecipientUser] PRIMARY KEY CLUSTERED 
(
	[RecipientsId] ASC,
	[UsersId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Schedules]    Script Date: 04/28/2023 2:14:47 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Schedules](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](max) NOT NULL,
	[BeginMonday] [nvarchar](max) NOT NULL,
	[EndMonday] [nvarchar](max) NOT NULL,
	[BeginTuesday] [nvarchar](max) NOT NULL,
	[EndTuesday] [nvarchar](max) NOT NULL,
	[BeginWednesday] [nvarchar](max) NOT NULL,
	[EndWednesday] [nvarchar](max) NOT NULL,
	[BeginThursday] [nvarchar](max) NOT NULL,
	[EndThursday] [nvarchar](max) NOT NULL,
	[BeginFriday] [nvarchar](max) NOT NULL,
	[EndFriday] [nvarchar](max) NOT NULL,
	[BeginSaturday] [nvarchar](max) NOT NULL,
	[EndSaturday] [nvarchar](max) NOT NULL,
	[BeginSunday] [nvarchar](max) NOT NULL,
	[EndSunday] [nvarchar](max) NOT NULL,
	[Enable] [bit] NOT NULL,
 CONSTRAINT [PK_Schedules] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ScheduleUser]    Script Date: 04/28/2023 2:14:47 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ScheduleUser](
	[SchedulesId] [bigint] NOT NULL,
	[UsersId] [bigint] NOT NULL,
 CONSTRAINT [PK_ScheduleUser] PRIMARY KEY CLUSTERED 
(
	[SchedulesId] ASC,
	[UsersId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Updaters]    Script Date: 04/28/2023 2:14:47 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Updaters](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[Time] [time](7) NOT NULL,
	[Data] [varbinary](max) NOT NULL,
 CONSTRAINT [PK_Updaters] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Users]    Script Date: 04/28/2023 2:14:47 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Users](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[Login] [nvarchar](max) NULL,
	[Password] [nvarchar](max) NULL,
	[Nickname] [nvarchar](max) NULL,
	[Privilege] [int] NOT NULL,
	[Enable] [bit] NOT NULL,
	[SocialName] [nvarchar](max) NULL,
	[CPF] [nvarchar](max) NULL,
	[CNPJ] [nvarchar](max) NULL,
	[CityInscription] [nvarchar](max) NULL,
	[ProvinceIncription] [nvarchar](max) NULL,
	[Province] [nvarchar](max) NOT NULL,
	[City] [nvarchar](max) NOT NULL,
	[Region] [nvarchar](max) NOT NULL,
	[Adress] [nvarchar](max) NOT NULL,
	[Number] [int] NOT NULL,
	[Complement] [nvarchar](max) NOT NULL,
	[CEP] [nvarchar](max) NOT NULL,
	[Email] [nvarchar](max) NOT NULL,
	[FullName] [nvarchar](max) NOT NULL,
	[Phone] [nvarchar](max) NOT NULL,
	[AlarmConfigurationId] [bigint] NOT NULL,
	[UserId] [bigint] NULL,
 CONSTRAINT [PK_Users] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[UserVariable]    Script Date: 04/28/2023 2:14:47 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UserVariable](
	[UsersId] [bigint] NOT NULL,
	[VariablesId] [bigint] NOT NULL,
 CONSTRAINT [PK_UserVariable] PRIMARY KEY CLUSTERED 
(
	[UsersId] ASC,
	[VariablesId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[VariableCodes]    Script Date: 04/28/2023 2:14:47 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[VariableCodes](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](max) NOT NULL,
 CONSTRAINT [PK_VariableCodes] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Variables]    Script Date: 04/28/2023 2:14:47 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Variables](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[Module] [nvarchar](max) NOT NULL,
	[Name] [nvarchar](max) NOT NULL,
	[Value] [nvarchar](max) NOT NULL,
	[GeoPosition] [nvarchar](max) NOT NULL,
	[Error] [float] NOT NULL,
	[DateTime] [datetime2](7) NOT NULL,
	[AlarmState] [bit] NOT NULL,
	[Enable] [bit] NOT NULL,
	[DeviceId] [bigint] NOT NULL,
	[VariableId] [bigint] NOT NULL,
	[ConditionId] [bigint] NULL,
	[UpdaterId] [bigint] NULL,
 CONSTRAINT [PK_Variables] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Index [IX_Actions_AlarmId]    Script Date: 04/28/2023 2:14:47 PM ******/
CREATE NONCLUSTERED INDEX [IX_Actions_AlarmId] ON [dbo].[Actions]
(
	[AlarmId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_Actions_VariableId]    Script Date: 04/28/2023 2:14:47 PM ******/
CREATE NONCLUSTERED INDEX [IX_Actions_VariableId] ON [dbo].[Actions]
(
	[VariableId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_ActionUser_UsersId]    Script Date: 04/28/2023 2:14:47 PM ******/
CREATE NONCLUSTERED INDEX [IX_ActionUser_UsersId] ON [dbo].[ActionUser]
(
	[UsersId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_AlarmConfigurationRecipient_RecipientsId]    Script Date: 04/28/2023 2:14:47 PM ******/
CREATE NONCLUSTERED INDEX [IX_AlarmConfigurationRecipient_RecipientsId] ON [dbo].[AlarmConfigurationRecipient]
(
	[RecipientsId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_AlarmRecipient_RecipientsId]    Script Date: 04/28/2023 2:14:47 PM ******/
CREATE NONCLUSTERED INDEX [IX_AlarmRecipient_RecipientsId] ON [dbo].[AlarmRecipient]
(
	[RecipientsId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_Alarms_DisplayId]    Script Date: 04/28/2023 2:14:47 PM ******/
CREATE NONCLUSTERED INDEX [IX_Alarms_DisplayId] ON [dbo].[Alarms]
(
	[DisplayId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_Alarms_VariableId]    Script Date: 04/28/2023 2:14:47 PM ******/
CREATE NONCLUSTERED INDEX [IX_Alarms_VariableId] ON [dbo].[Alarms]
(
	[VariableId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_AlarmSchedule_SchedulesId]    Script Date: 04/28/2023 2:14:47 PM ******/
CREATE NONCLUSTERED INDEX [IX_AlarmSchedule_SchedulesId] ON [dbo].[AlarmSchedule]
(
	[SchedulesId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_AlarmUser_UsersId]    Script Date: 04/28/2023 2:14:47 PM ******/
CREATE NONCLUSTERED INDEX [IX_AlarmUser_UsersId] ON [dbo].[AlarmUser]
(
	[UsersId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_ColorConfigs_DisplayId]    Script Date: 04/28/2023 2:14:47 PM ******/
CREATE NONCLUSTERED INDEX [IX_ColorConfigs_DisplayId] ON [dbo].[ColorConfigs]
(
	[DisplayId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_DashboardGroup_GroupsId]    Script Date: 04/28/2023 2:14:47 PM ******/
CREATE NONCLUSTERED INDEX [IX_DashboardGroup_GroupsId] ON [dbo].[DashboardGroup]
(
	[GroupsId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_DashboardUser_UsersId]    Script Date: 04/28/2023 2:14:47 PM ******/
CREATE NONCLUSTERED INDEX [IX_DashboardUser_UsersId] ON [dbo].[DashboardUser]
(
	[UsersId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_DevicesUser_UsersId]    Script Date: 04/28/2023 2:14:47 PM ******/
CREATE NONCLUSTERED INDEX [IX_DevicesUser_UsersId] ON [dbo].[DevicesUser]
(
	[UsersId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_DisplayUser_UsersId]    Script Date: 04/28/2023 2:14:47 PM ******/
CREATE NONCLUSTERED INDEX [IX_DisplayUser_UsersId] ON [dbo].[DisplayUser]
(
	[UsersId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_GroupUser_UsersId]    Script Date: 04/28/2023 2:14:47 PM ******/
CREATE NONCLUSTERED INDEX [IX_GroupUser_UsersId] ON [dbo].[GroupUser]
(
	[UsersId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_LayerUser_UsersId]    Script Date: 04/28/2023 2:14:47 PM ******/
CREATE NONCLUSTERED INDEX [IX_LayerUser_UsersId] ON [dbo].[LayerUser]
(
	[UsersId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_Measurers_DisplayId]    Script Date: 04/28/2023 2:14:47 PM ******/
CREATE NONCLUSTERED INDEX [IX_Measurers_DisplayId] ON [dbo].[Measurers]
(
	[DisplayId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_Measurers_GroupId]    Script Date: 04/28/2023 2:14:47 PM ******/
CREATE NONCLUSTERED INDEX [IX_Measurers_GroupId] ON [dbo].[Measurers]
(
	[GroupId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_Measurers_LayerId]    Script Date: 04/28/2023 2:14:47 PM ******/
CREATE NONCLUSTERED INDEX [IX_Measurers_LayerId] ON [dbo].[Measurers]
(
	[LayerId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_Measurers_VariableId]    Script Date: 04/28/2023 2:14:47 PM ******/
CREATE NONCLUSTERED INDEX [IX_Measurers_VariableId] ON [dbo].[Measurers]
(
	[VariableId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_MeasurerUser_UsersId]    Script Date: 04/28/2023 2:14:47 PM ******/
CREATE NONCLUSTERED INDEX [IX_MeasurerUser_UsersId] ON [dbo].[MeasurerUser]
(
	[UsersId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_RecipientUser_UsersId]    Script Date: 04/28/2023 2:14:47 PM ******/
CREATE NONCLUSTERED INDEX [IX_RecipientUser_UsersId] ON [dbo].[RecipientUser]
(
	[UsersId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_ScheduleUser_UsersId]    Script Date: 04/28/2023 2:14:47 PM ******/
CREATE NONCLUSTERED INDEX [IX_ScheduleUser_UsersId] ON [dbo].[ScheduleUser]
(
	[UsersId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_Users_AlarmConfigurationId]    Script Date: 04/28/2023 2:14:47 PM ******/
CREATE NONCLUSTERED INDEX [IX_Users_AlarmConfigurationId] ON [dbo].[Users]
(
	[AlarmConfigurationId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_Users_UserId]    Script Date: 04/28/2023 2:14:47 PM ******/
CREATE NONCLUSTERED INDEX [IX_Users_UserId] ON [dbo].[Users]
(
	[UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_UserVariable_VariablesId]    Script Date: 04/28/2023 2:14:47 PM ******/
CREATE NONCLUSTERED INDEX [IX_UserVariable_VariablesId] ON [dbo].[UserVariable]
(
	[VariablesId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_Variables_ConditionId]    Script Date: 04/28/2023 2:14:47 PM ******/
CREATE NONCLUSTERED INDEX [IX_Variables_ConditionId] ON [dbo].[Variables]
(
	[ConditionId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_Variables_DeviceId]    Script Date: 04/28/2023 2:14:47 PM ******/
CREATE NONCLUSTERED INDEX [IX_Variables_DeviceId] ON [dbo].[Variables]
(
	[DeviceId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_Variables_UpdaterId]    Script Date: 04/28/2023 2:14:47 PM ******/
CREATE NONCLUSTERED INDEX [IX_Variables_UpdaterId] ON [dbo].[Variables]
(
	[UpdaterId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_Variables_VariableId]    Script Date: 04/28/2023 2:14:47 PM ******/
CREATE NONCLUSTERED INDEX [IX_Variables_VariableId] ON [dbo].[Variables]
(
	[VariableId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Actions]  WITH CHECK ADD  CONSTRAINT [FK_Actions_Alarms_AlarmId] FOREIGN KEY([AlarmId])
REFERENCES [dbo].[Alarms] ([Id])
GO
ALTER TABLE [dbo].[Actions] CHECK CONSTRAINT [FK_Actions_Alarms_AlarmId]
GO
ALTER TABLE [dbo].[Actions]  WITH CHECK ADD  CONSTRAINT [FK_Actions_Variables_VariableId] FOREIGN KEY([VariableId])
REFERENCES [dbo].[Variables] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Actions] CHECK CONSTRAINT [FK_Actions_Variables_VariableId]
GO
ALTER TABLE [dbo].[ActionUser]  WITH CHECK ADD  CONSTRAINT [FK_ActionUser_Actions_ActionsId] FOREIGN KEY([ActionsId])
REFERENCES [dbo].[Actions] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[ActionUser] CHECK CONSTRAINT [FK_ActionUser_Actions_ActionsId]
GO
ALTER TABLE [dbo].[ActionUser]  WITH CHECK ADD  CONSTRAINT [FK_ActionUser_Users_UsersId] FOREIGN KEY([UsersId])
REFERENCES [dbo].[Users] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[ActionUser] CHECK CONSTRAINT [FK_ActionUser_Users_UsersId]
GO
ALTER TABLE [dbo].[AlarmConfigurationRecipient]  WITH CHECK ADD  CONSTRAINT [FK_AlarmConfigurationRecipient_AlarmConfigurations_AlarmConfigurationsId] FOREIGN KEY([AlarmConfigurationsId])
REFERENCES [dbo].[AlarmConfigurations] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AlarmConfigurationRecipient] CHECK CONSTRAINT [FK_AlarmConfigurationRecipient_AlarmConfigurations_AlarmConfigurationsId]
GO
ALTER TABLE [dbo].[AlarmConfigurationRecipient]  WITH CHECK ADD  CONSTRAINT [FK_AlarmConfigurationRecipient_Recipients_RecipientsId] FOREIGN KEY([RecipientsId])
REFERENCES [dbo].[Recipients] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AlarmConfigurationRecipient] CHECK CONSTRAINT [FK_AlarmConfigurationRecipient_Recipients_RecipientsId]
GO
ALTER TABLE [dbo].[AlarmRecipient]  WITH CHECK ADD  CONSTRAINT [FK_AlarmRecipient_Alarms_AlarmsId] FOREIGN KEY([AlarmsId])
REFERENCES [dbo].[Alarms] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AlarmRecipient] CHECK CONSTRAINT [FK_AlarmRecipient_Alarms_AlarmsId]
GO
ALTER TABLE [dbo].[AlarmRecipient]  WITH CHECK ADD  CONSTRAINT [FK_AlarmRecipient_Recipients_RecipientsId] FOREIGN KEY([RecipientsId])
REFERENCES [dbo].[Recipients] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AlarmRecipient] CHECK CONSTRAINT [FK_AlarmRecipient_Recipients_RecipientsId]
GO
ALTER TABLE [dbo].[Alarms]  WITH CHECK ADD  CONSTRAINT [FK_Alarms_Displays_DisplayId] FOREIGN KEY([DisplayId])
REFERENCES [dbo].[Displays] ([Id])
GO
ALTER TABLE [dbo].[Alarms] CHECK CONSTRAINT [FK_Alarms_Displays_DisplayId]
GO
ALTER TABLE [dbo].[Alarms]  WITH CHECK ADD  CONSTRAINT [FK_Alarms_Variables_VariableId] FOREIGN KEY([VariableId])
REFERENCES [dbo].[Variables] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Alarms] CHECK CONSTRAINT [FK_Alarms_Variables_VariableId]
GO
ALTER TABLE [dbo].[AlarmSchedule]  WITH CHECK ADD  CONSTRAINT [FK_AlarmSchedule_Alarms_AlarmsId] FOREIGN KEY([AlarmsId])
REFERENCES [dbo].[Alarms] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AlarmSchedule] CHECK CONSTRAINT [FK_AlarmSchedule_Alarms_AlarmsId]
GO
ALTER TABLE [dbo].[AlarmSchedule]  WITH CHECK ADD  CONSTRAINT [FK_AlarmSchedule_Schedules_SchedulesId] FOREIGN KEY([SchedulesId])
REFERENCES [dbo].[Schedules] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AlarmSchedule] CHECK CONSTRAINT [FK_AlarmSchedule_Schedules_SchedulesId]
GO
ALTER TABLE [dbo].[AlarmUser]  WITH CHECK ADD  CONSTRAINT [FK_AlarmUser_Alarms_AlarmsId] FOREIGN KEY([AlarmsId])
REFERENCES [dbo].[Alarms] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AlarmUser] CHECK CONSTRAINT [FK_AlarmUser_Alarms_AlarmsId]
GO
ALTER TABLE [dbo].[AlarmUser]  WITH CHECK ADD  CONSTRAINT [FK_AlarmUser_Users_UsersId] FOREIGN KEY([UsersId])
REFERENCES [dbo].[Users] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AlarmUser] CHECK CONSTRAINT [FK_AlarmUser_Users_UsersId]
GO
ALTER TABLE [dbo].[ColorConfigs]  WITH CHECK ADD  CONSTRAINT [FK_ColorConfigs_Displays_DisplayId] FOREIGN KEY([DisplayId])
REFERENCES [dbo].[Displays] ([Id])
GO
ALTER TABLE [dbo].[ColorConfigs] CHECK CONSTRAINT [FK_ColorConfigs_Displays_DisplayId]
GO
ALTER TABLE [dbo].[DashboardGroup]  WITH CHECK ADD  CONSTRAINT [FK_DashboardGroup_Dashboards_DashboardsId] FOREIGN KEY([DashboardsId])
REFERENCES [dbo].[Dashboards] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[DashboardGroup] CHECK CONSTRAINT [FK_DashboardGroup_Dashboards_DashboardsId]
GO
ALTER TABLE [dbo].[DashboardGroup]  WITH CHECK ADD  CONSTRAINT [FK_DashboardGroup_Groups_GroupsId] FOREIGN KEY([GroupsId])
REFERENCES [dbo].[Groups] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[DashboardGroup] CHECK CONSTRAINT [FK_DashboardGroup_Groups_GroupsId]
GO
ALTER TABLE [dbo].[DashboardUser]  WITH CHECK ADD  CONSTRAINT [FK_DashboardUser_Dashboards_DashboardsId] FOREIGN KEY([DashboardsId])
REFERENCES [dbo].[Dashboards] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[DashboardUser] CHECK CONSTRAINT [FK_DashboardUser_Dashboards_DashboardsId]
GO
ALTER TABLE [dbo].[DashboardUser]  WITH CHECK ADD  CONSTRAINT [FK_DashboardUser_Users_UsersId] FOREIGN KEY([UsersId])
REFERENCES [dbo].[Users] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[DashboardUser] CHECK CONSTRAINT [FK_DashboardUser_Users_UsersId]
GO
ALTER TABLE [dbo].[DevicesUser]  WITH CHECK ADD  CONSTRAINT [FK_DevicesUser_Devices_DevicesId] FOREIGN KEY([DevicesId])
REFERENCES [dbo].[Devices] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[DevicesUser] CHECK CONSTRAINT [FK_DevicesUser_Devices_DevicesId]
GO
ALTER TABLE [dbo].[DevicesUser]  WITH CHECK ADD  CONSTRAINT [FK_DevicesUser_Users_UsersId] FOREIGN KEY([UsersId])
REFERENCES [dbo].[Users] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[DevicesUser] CHECK CONSTRAINT [FK_DevicesUser_Users_UsersId]
GO
ALTER TABLE [dbo].[DisplayUser]  WITH CHECK ADD  CONSTRAINT [FK_DisplayUser_Displays_DisplaysId] FOREIGN KEY([DisplaysId])
REFERENCES [dbo].[Displays] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[DisplayUser] CHECK CONSTRAINT [FK_DisplayUser_Displays_DisplaysId]
GO
ALTER TABLE [dbo].[DisplayUser]  WITH CHECK ADD  CONSTRAINT [FK_DisplayUser_Users_UsersId] FOREIGN KEY([UsersId])
REFERENCES [dbo].[Users] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[DisplayUser] CHECK CONSTRAINT [FK_DisplayUser_Users_UsersId]
GO
ALTER TABLE [dbo].[GroupUser]  WITH CHECK ADD  CONSTRAINT [FK_GroupUser_Groups_GroupsId] FOREIGN KEY([GroupsId])
REFERENCES [dbo].[Groups] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[GroupUser] CHECK CONSTRAINT [FK_GroupUser_Groups_GroupsId]
GO
ALTER TABLE [dbo].[GroupUser]  WITH CHECK ADD  CONSTRAINT [FK_GroupUser_Users_UsersId] FOREIGN KEY([UsersId])
REFERENCES [dbo].[Users] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[GroupUser] CHECK CONSTRAINT [FK_GroupUser_Users_UsersId]
GO
ALTER TABLE [dbo].[LayerUser]  WITH CHECK ADD  CONSTRAINT [FK_LayerUser_Layers_LayersId] FOREIGN KEY([LayersId])
REFERENCES [dbo].[Layers] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[LayerUser] CHECK CONSTRAINT [FK_LayerUser_Layers_LayersId]
GO
ALTER TABLE [dbo].[LayerUser]  WITH CHECK ADD  CONSTRAINT [FK_LayerUser_Users_UsersId] FOREIGN KEY([UsersId])
REFERENCES [dbo].[Users] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[LayerUser] CHECK CONSTRAINT [FK_LayerUser_Users_UsersId]
GO
ALTER TABLE [dbo].[Measurers]  WITH CHECK ADD  CONSTRAINT [FK_Measurers_Displays_DisplayId] FOREIGN KEY([DisplayId])
REFERENCES [dbo].[Displays] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Measurers] CHECK CONSTRAINT [FK_Measurers_Displays_DisplayId]
GO
ALTER TABLE [dbo].[Measurers]  WITH CHECK ADD  CONSTRAINT [FK_Measurers_Groups_GroupId] FOREIGN KEY([GroupId])
REFERENCES [dbo].[Groups] ([Id])
GO
ALTER TABLE [dbo].[Measurers] CHECK CONSTRAINT [FK_Measurers_Groups_GroupId]
GO
ALTER TABLE [dbo].[Measurers]  WITH CHECK ADD  CONSTRAINT [FK_Measurers_Layers_LayerId] FOREIGN KEY([LayerId])
REFERENCES [dbo].[Layers] ([Id])
GO
ALTER TABLE [dbo].[Measurers] CHECK CONSTRAINT [FK_Measurers_Layers_LayerId]
GO
ALTER TABLE [dbo].[Measurers]  WITH CHECK ADD  CONSTRAINT [FK_Measurers_Variables_VariableId] FOREIGN KEY([VariableId])
REFERENCES [dbo].[Variables] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Measurers] CHECK CONSTRAINT [FK_Measurers_Variables_VariableId]
GO
ALTER TABLE [dbo].[MeasurerUser]  WITH CHECK ADD  CONSTRAINT [FK_MeasurerUser_Measurers_MeasurersId] FOREIGN KEY([MeasurersId])
REFERENCES [dbo].[Measurers] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[MeasurerUser] CHECK CONSTRAINT [FK_MeasurerUser_Measurers_MeasurersId]
GO
ALTER TABLE [dbo].[MeasurerUser]  WITH CHECK ADD  CONSTRAINT [FK_MeasurerUser_Users_UsersId] FOREIGN KEY([UsersId])
REFERENCES [dbo].[Users] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[MeasurerUser] CHECK CONSTRAINT [FK_MeasurerUser_Users_UsersId]
GO
ALTER TABLE [dbo].[RecipientUser]  WITH CHECK ADD  CONSTRAINT [FK_RecipientUser_Recipients_RecipientsId] FOREIGN KEY([RecipientsId])
REFERENCES [dbo].[Recipients] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[RecipientUser] CHECK CONSTRAINT [FK_RecipientUser_Recipients_RecipientsId]
GO
ALTER TABLE [dbo].[RecipientUser]  WITH CHECK ADD  CONSTRAINT [FK_RecipientUser_Users_UsersId] FOREIGN KEY([UsersId])
REFERENCES [dbo].[Users] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[RecipientUser] CHECK CONSTRAINT [FK_RecipientUser_Users_UsersId]
GO
ALTER TABLE [dbo].[ScheduleUser]  WITH CHECK ADD  CONSTRAINT [FK_ScheduleUser_Schedules_SchedulesId] FOREIGN KEY([SchedulesId])
REFERENCES [dbo].[Schedules] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[ScheduleUser] CHECK CONSTRAINT [FK_ScheduleUser_Schedules_SchedulesId]
GO
ALTER TABLE [dbo].[ScheduleUser]  WITH CHECK ADD  CONSTRAINT [FK_ScheduleUser_Users_UsersId] FOREIGN KEY([UsersId])
REFERENCES [dbo].[Users] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[ScheduleUser] CHECK CONSTRAINT [FK_ScheduleUser_Users_UsersId]
GO
ALTER TABLE [dbo].[Users]  WITH CHECK ADD  CONSTRAINT [FK_Users_AlarmConfigurations_AlarmConfigurationId] FOREIGN KEY([AlarmConfigurationId])
REFERENCES [dbo].[AlarmConfigurations] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Users] CHECK CONSTRAINT [FK_Users_AlarmConfigurations_AlarmConfigurationId]
GO
ALTER TABLE [dbo].[Users]  WITH CHECK ADD  CONSTRAINT [FK_Users_Users_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[Users] ([Id])
GO
ALTER TABLE [dbo].[Users] CHECK CONSTRAINT [FK_Users_Users_UserId]
GO
ALTER TABLE [dbo].[UserVariable]  WITH CHECK ADD  CONSTRAINT [FK_UserVariable_Users_UsersId] FOREIGN KEY([UsersId])
REFERENCES [dbo].[Users] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[UserVariable] CHECK CONSTRAINT [FK_UserVariable_Users_UsersId]
GO
ALTER TABLE [dbo].[UserVariable]  WITH CHECK ADD  CONSTRAINT [FK_UserVariable_Variables_VariablesId] FOREIGN KEY([VariablesId])
REFERENCES [dbo].[Variables] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[UserVariable] CHECK CONSTRAINT [FK_UserVariable_Variables_VariablesId]
GO
ALTER TABLE [dbo].[Variables]  WITH CHECK ADD  CONSTRAINT [FK_Variables_Conditions_ConditionId] FOREIGN KEY([ConditionId])
REFERENCES [dbo].[Conditions] ([Id])
GO
ALTER TABLE [dbo].[Variables] CHECK CONSTRAINT [FK_Variables_Conditions_ConditionId]
GO
ALTER TABLE [dbo].[Variables]  WITH CHECK ADD  CONSTRAINT [FK_Variables_Devices_DeviceId] FOREIGN KEY([DeviceId])
REFERENCES [dbo].[Devices] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Variables] CHECK CONSTRAINT [FK_Variables_Devices_DeviceId]
GO
ALTER TABLE [dbo].[Variables]  WITH CHECK ADD  CONSTRAINT [FK_Variables_Updaters_UpdaterId] FOREIGN KEY([UpdaterId])
REFERENCES [dbo].[Updaters] ([Id])
GO
ALTER TABLE [dbo].[Variables] CHECK CONSTRAINT [FK_Variables_Updaters_UpdaterId]
GO
ALTER TABLE [dbo].[Variables]  WITH CHECK ADD  CONSTRAINT [FK_Variables_VariableCodes_VariableId] FOREIGN KEY([VariableId])
REFERENCES [dbo].[VariableCodes] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Variables] CHECK CONSTRAINT [FK_Variables_VariableCodes_VariableId]
GO
USE [master]
GO
ALTER DATABASE [TelemetryHub] SET  READ_WRITE 
GO
