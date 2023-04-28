USE [master]
GO
/****** Object:  Database [MasterServer]    Script Date: 04/28/2023 11:21:37 AM ******/
CREATE DATABASE [MasterServer]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'MasterServer', FILENAME = N'/var/opt/mssql/data/MasterServer.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'MasterServer_log', FILENAME = N'/var/opt/mssql/data/MasterServer_log.ldf' , SIZE = 3976KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
GO
ALTER DATABASE [MasterServer] SET COMPATIBILITY_LEVEL = 140
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [MasterServer].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [MasterServer] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [MasterServer] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [MasterServer] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [MasterServer] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [MasterServer] SET ARITHABORT OFF 
GO
ALTER DATABASE [MasterServer] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [MasterServer] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [MasterServer] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [MasterServer] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [MasterServer] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [MasterServer] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [MasterServer] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [MasterServer] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [MasterServer] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [MasterServer] SET  DISABLE_BROKER 
GO
ALTER DATABASE [MasterServer] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [MasterServer] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [MasterServer] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [MasterServer] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [MasterServer] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [MasterServer] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [MasterServer] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [MasterServer] SET RECOVERY SIMPLE 
GO
ALTER DATABASE [MasterServer] SET  MULTI_USER 
GO
ALTER DATABASE [MasterServer] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [MasterServer] SET DB_CHAINING OFF 
GO
ALTER DATABASE [MasterServer] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [MasterServer] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [MasterServer] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [MasterServer] SET QUERY_STORE = OFF
GO
USE [MasterServer]
GO
/****** Object:  User [MTW]    Script Date: 04/28/2023 11:21:37 AM ******/
/*CREATE USER [MTW] FOR LOGIN [MTW] WITH DEFAULT_SCHEMA=[dbo]
GO
ALTER ROLE [db_owner] ADD MEMBER [MTW]
GO */
/****** Object:  Table [dbo].[Acoes]    Script Date: 04/28/2023 11:21:37 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Acoes](
	[id] [bigint] IDENTITY(1,1) NOT NULL,
	[Titulo] [varchar](50) NOT NULL,
	[Autorizacao] [varchar](200) NOT NULL,
	[idOrigem] [bigint] NULL,
	[TipoPessoa] [nvarchar](50) NOT NULL,
	[Placa] [nvarchar](20) NOT NULL,
	[Acao] [nvarchar](50) NOT NULL,
	[Parametros] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_Acoes] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/*PAREI AQUI*/

/****** Object:  Table [dbo].[AlarmeCameraControl]    Script Date: 04/28/2023 11:21:37 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AlarmeCameraControl](
	[idEquipamento] [bigint] NOT NULL,
	[alertaEntradaLigada] [bit] NOT NULL,
	[alertaEntradaDesligada] [bit] NOT NULL,
	[alertaSaidaLigada] [bit] NOT NULL,
	[alertaSaidaDesligada] [bit] NOT NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AlarmeLpr]    Script Date: 04/28/2023 11:21:37 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AlarmeLpr](
	[idEquipamento] [bigint] NOT NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AlarmeMastereye]    Script Date: 04/28/2023 11:21:37 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AlarmeMastereye](
	[idEquipamento] [bigint] NOT NULL,
	[servidorSmtp] [varchar](100) NOT NULL,
	[portaSmtp] [varchar](100) NOT NULL,
	[emailRemetente] [varchar](100) NOT NULL,
	[emailsDestinatarios] [varchar](500) NOT NULL,
	[senhaRemetente] [varchar](100) NOT NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AlarmeTelemetria]    Script Date: 04/28/2023 11:21:37 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AlarmeTelemetria](
	[idEquipamento] [bigint] NOT NULL,
	[Nome] [nvarchar](50) NULL,
	[Online] [bit] NOT NULL,
	[EntradaCameraAtiva] [bit] NOT NULL,
	[EntradaCamera] [bit] NOT NULL,
	[SaidaCameraAtiva] [bit] NOT NULL,
	[SaidaCamera] [bit] NOT NULL,
	[Entrada1Slot0Ativa] [bit] NOT NULL,
	[Entrada1Slot0] [bit] NOT NULL,
	[Saida1Slot0Ativa] [bit] NOT NULL,
	[Saida1Slot0] [bit] NOT NULL,
	[Dc1Slot0Min] [int] NOT NULL,
	[Dc1Slot0Max] [int] NOT NULL,
	[Dc2Slot0Min] [int] NOT NULL,
	[Dc2Slot0Max] [int] NOT NULL,
	[TempSlot0Min] [int] NOT NULL,
	[TempSlot0Max] [int] NOT NULL,
	[HumSlot0Min] [int] NOT NULL,
	[HumSlot0Max] [int] NOT NULL,
	[Entrada1Slot2Ativa] [bit] NOT NULL,
	[Entrada1Slot2] [bit] NOT NULL,
	[Saida1Slot2Ativa] [bit] NOT NULL,
	[Saida1Slot2] [bit] NOT NULL,
	[Entrada2Slot2Ativa] [bit] NOT NULL,
	[Entrada2Slot2] [bit] NOT NULL,
	[Saida2Slot2Ativa] [bit] NOT NULL,
	[Saida2Slot2] [bit] NOT NULL,
	[Entrada3Slot2Ativa] [bit] NOT NULL,
	[Entrada3Slot2] [bit] NOT NULL,
	[Saida3Slot2Ativa] [bit] NOT NULL,
	[Saida3Slot2] [bit] NOT NULL,
	[Dc1Slot2Min] [int] NOT NULL,
	[Dc1Slot2Max] [int] NOT NULL,
	[Dc2Slot2Min] [int] NOT NULL,
	[Dc2Slot2Max] [int] NOT NULL,
	[Ac1Slot2Min] [int] NOT NULL,
	[Ac1Slot2Max] [int] NOT NULL,
	[Ac2Slot2Min] [int] NOT NULL,
	[Ac2Slot2Max] [int] NOT NULL,
	[Ac3Slot2Min] [int] NOT NULL,
	[Ac3Slot2Max] [int] NOT NULL,
	[Entrada1Slot3Ativa] [bit] NOT NULL,
	[Entrada1Slot3] [bit] NOT NULL,
	[Saida1Slot3Ativa] [bit] NOT NULL,
	[Saida1Slot3] [bit] NOT NULL,
	[Entrada2Slot3Ativa] [bit] NOT NULL,
	[Entrada2Slot3] [bit] NOT NULL,
	[Saida2Slot3Ativa] [bit] NOT NULL,
	[Saida2Slot3] [bit] NOT NULL,
	[Entrada3Slot3Ativa] [bit] NOT NULL,
	[Entrada3Slot3] [bit] NOT NULL,
	[Saida3Slot3Ativa] [bit] NOT NULL,
	[Saida3Slot3] [bit] NOT NULL,
	[Dc1Slot3Min] [int] NOT NULL,
	[Dc1Slot3Max] [int] NOT NULL,
	[Dc2Slot3Min] [int] NOT NULL,
	[Dc2Slot3Max] [int] NOT NULL,
	[Ac1Slot3Min] [int] NOT NULL,
	[Ac1Slot3Max] [int] NOT NULL,
	[Ac2Slot3Min] [int] NOT NULL,
	[Ac2Slot3Max] [int] NOT NULL,
	[Ac3Slot3Min] [int] NOT NULL,
	[Ac3Slot3Max] [int] NOT NULL,
	[Entrada1Slot4Ativa] [bit] NOT NULL,
	[Entrada1Slot4] [bit] NOT NULL,
	[Saida1Slot4Ativa] [bit] NOT NULL,
	[Saida1Slot4] [bit] NOT NULL,
	[Entrada2Slot4Ativa] [bit] NOT NULL,
	[Entrada2Slot4] [bit] NOT NULL,
	[Saida2Slot4Ativa] [bit] NOT NULL,
	[Saida2Slot4] [bit] NOT NULL,
	[Entrada3Slot4Ativa] [bit] NOT NULL,
	[Entrada3Slot4] [bit] NOT NULL,
	[Saida3Slot4Ativa] [bit] NOT NULL,
	[Saida3Slot4] [bit] NOT NULL,
	[Dc1Slot4Min] [int] NOT NULL,
	[Dc1Slot4Max] [int] NOT NULL,
	[Dc2Slot4Min] [int] NOT NULL,
	[Dc2Slot4Max] [int] NOT NULL,
	[Ac1Slot4Min] [int] NOT NULL,
	[Ac1Slot4Max] [int] NOT NULL,
	[Ac2Slot4Min] [int] NOT NULL,
	[Ac2Slot4Max] [int] NOT NULL,
	[Ac3Slot4Min] [int] NOT NULL,
	[Ac3Slot4Max] [int] NOT NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AlarmeUsuario]    Script Date: 04/28/2023 11:21:37 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AlarmeUsuario](
	[idUsuario] [bigint] NOT NULL,
	[SmtpServer] [varchar](200) NOT NULL,
	[SmtpPort] [int] NOT NULL,
	[EmailEnvio] [nvarchar](200) NOT NULL,
	[SenhaEmail] [nvarchar](200) NOT NULL,
	[Destinatario] [nvarchar](max) NULL,
	[SmsCheck] [bit] NULL,
	[Telefone] [nvarchar](max) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[CameraControl]    Script Date: 04/28/2023 11:21:37 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CameraControl](
	[idEquipamento] [bigint] NOT NULL,
	[idTelemetria] [bigint] NULL,
	[Modelo] [nvarchar](50) NOT NULL,
	[PortaComando] [int] NOT NULL,
	[PortaMidia] [int] NOT NULL,
	[NomeEntradaLigada] [varchar](100) NOT NULL,
	[NomeEntradaDesligada] [varchar](100) NOT NULL,
	[NomeSaidaLigada] [varchar](100) NOT NULL,
	[NomeSaidaDesligada] [varchar](100) NOT NULL,
	[CorEntradaLigada] [varchar](100) NOT NULL,
	[CorEntradaDesligada] [varchar](100) NOT NULL,
	[CorSaidaLigada] [varchar](100) NOT NULL,
	[CorSaidaDesligada] [varchar](100) NOT NULL,
	[SaidaPulso] [bit] NOT NULL,
	[SaidaPeriodo] [int] NOT NULL,
	[StatusEntrada] [bit] NOT NULL,
	[StatusSaida] [bit] NOT NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[DVC]    Script Date: 04/28/2023 11:21:37 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DVC](
	[idEquipamento] [bigint] NOT NULL,
	[NumeroSerie] [varchar](100) NOT NULL,
	[Funcao] [varchar](100) NOT NULL,
	[Apelido] [varchar](100) NOT NULL,
	[Servidor] [bigint] NOT NULL,
	[SO] [varchar](100) NULL,
	[Video] [bit] NULL,
	[Audio] [bit] NULL,
	[FluxoPermanente] [bit] NULL,
	[DvcStatus] [bit] NULL,
	[DvcStatusDateTime] [datetime] NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Equipamentos]    Script Date: 04/28/2023 11:21:37 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Equipamentos](
	[id] [bigint] IDENTITY(1,1) NOT NULL,
	[Nome] [varchar](50) NOT NULL,
	[Ip] [varchar](50) NOT NULL,
	[Usuario] [varchar](50) NOT NULL,
	[Senha] [varchar](50) NOT NULL,
	[Tipo] [varchar](50) NULL,
	[RtspPrimario] [varchar](max) NULL,
	[RtspSecundario] [varchar](max) NULL,
	[RtspStreamPrimario] [varchar](max) NULL,
	[RtspStreamSecundario] [varchar](max) NULL,
	[Estado] [varchar](20) NULL,
	[DataHora] [datetime] NULL,
 CONSTRAINT [PK_Equipaments] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Eventos]    Script Date: 04/28/2023 11:21:37 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Eventos](
	[idEquipamento] [bigint] NOT NULL,
	[Criacao] [datetime] NOT NULL,
	[TempoHabilitado] [bigint] NOT NULL,
	[TempoDesabilitado] [bigint] NOT NULL,
	[TempoLigado] [bigint] NOT NULL,
	[TempoDesligado] [bigint] NOT NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Gravacao]    Script Date: 04/28/2023 11:21:37 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Gravacao](
	[idEquipamento] [bigint] NOT NULL,
	[CaminhoGravacao] [varchar](max) NOT NULL,
	[Duracao] [int] NULL,
	[UsuarioRemoto] [varchar](50) NULL,
	[SenhaRemoto] [varchar](50) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Grupos]    Script Date: 04/28/2023 11:21:37 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Grupos](
	[id] [bigint] IDENTITY(1,1) NOT NULL,
	[Nome] [varchar](50) NOT NULL,
	[Tempo] [int] NOT NULL,
	[Subgrupo] [nvarchar](max) NULL,
	[Equipamentos] [varchar](max) NULL,
 CONSTRAINT [PK_Groups] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[HistoricoDeAcoes]    Script Date: 04/28/2023 11:21:37 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[HistoricoDeAcoes](
	[id] [bigint] IDENTITY(1,1) NOT NULL,
	[idAcoes] [bigint] NOT NULL,
	[idRegistros] [bigint] NOT NULL,
	[Dados] [varchar](500) NOT NULL,
	[DataHistorico] [datetime] NOT NULL,
 CONSTRAINT [PK_HistoricoDeAcoes] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Horarios]    Script Date: 04/28/2023 11:21:37 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Horarios](
	[id] [bigint] IDENTITY(1,1) NOT NULL,
	[Titulo] [varchar](30) NOT NULL,
	[SegHI] [datetime] NULL,
	[SegHF] [datetime] NULL,
	[TerHI] [datetime] NULL,
	[TerHF] [datetime] NULL,
	[QuaHI] [datetime] NULL,
	[QuaHF] [datetime] NULL,
	[QuiHI] [datetime] NULL,
	[QuiHF] [datetime] NULL,
	[SexHI] [datetime] NULL,
	[SexHF] [datetime] NULL,
	[SabHI] [datetime] NULL,
	[SabHF] [datetime] NULL,
	[DomHI] [datetime] NULL,
	[DomHF] [datetime] NULL,
 CONSTRAINT [PK_Horarios] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ImagemOCR]    Script Date: 04/28/2023 11:21:37 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ImagemOCR](
	[id] [bigint] IDENTITY(1,1) NOT NULL,
	[idEquipamento] [bigint] NOT NULL,
	[Arquivo] [varchar](250) NOT NULL,
 CONSTRAINT [PK_ImagemOCR] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Lpr]    Script Date: 04/28/2023 11:21:37 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Lpr](
	[idEquipamento] [bigint] NOT NULL,
	[NomeLpr] [varchar](50) NOT NULL,
	[idOrigens] [bigint] NOT NULL,
	[idControle] [bigint] NOT NULL,
	[idAcesso] [bigint] NOT NULL,
	[CodigoPais] [varchar](100) NOT NULL,
	[ReconhecerMovimento] [bit] NOT NULL,
	[TempoBanco] [int] NOT NULL,
	[TempoEntrePlacas] [int] NOT NULL,
	[TempoPlacaFalsa] [int] NOT NULL,
	[Threads] [int] NOT NULL,
	[Fps] [int] NOT NULL,
	[ConfirmacaoResultado] [int] NOT NULL,
	[Precisao] [int] NOT NULL,
	[Rotacao] [int] NOT NULL,
	[TamanhoMinimoLetra] [int] NOT NULL,
	[TamanhoMaximoLetra] [int] NOT NULL,
	[FtpContexto] [bit] NOT NULL,
	[IpContexto] [varchar](100) NOT NULL,
	[TempoImagem] [int] NOT NULL,
	[UrlContexto] [bit] NOT NULL,
	[idContexto1] [bigint] NULL,
	[idContexto2] [bigint] NULL,
	[idContexto3] [bigint] NULL,
	[idContexto4] [bigint] NULL,
	[HabilitarZona] [bit] NOT NULL,
	[x1] [int] NOT NULL,
	[x2] [int] NOT NULL,
	[x3] [int] NOT NULL,
	[x4] [int] NOT NULL,
	[y1] [int] NOT NULL,
	[y2] [int] NOT NULL,
	[y3] [int] NOT NULL,
	[y4] [int] NOT NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Mapas]    Script Date: 04/28/2023 11:21:37 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Mapas](
	[id] [bigint] IDENTITY(1,1) NOT NULL,
	[Nome] [varchar](50) NOT NULL,
	[Imagem] [varbinary](max) NOT NULL,
	[CaminhoImagem] [varchar](max) NULL,
	[Grupos] [varchar](max) NOT NULL,
	[Equipamentos] [varchar](max) NOT NULL,
	[EquipamentosCad] [varchar](max) NOT NULL,
	[ParOrdenado] [varchar](max) NOT NULL,
 CONSTRAINT [PK_Mapas] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[MasterEye]    Script Date: 04/28/2023 11:21:37 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[MasterEye](
	[idEquipamento] [bigint] NOT NULL,
	[Mapas] [nvarchar](max) NOT NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[MensagemTelemetria]    Script Date: 04/28/2023 11:21:37 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[MensagemTelemetria](
	[id] [bigint] IDENTITY(1,1) NOT NULL,
	[idTelemetria] [bigint] NOT NULL,
	[Slot] [int] NOT NULL,
	[Mensagem] [nvarchar](max) NOT NULL,
	[Data] [nvarchar](50) NULL,
 CONSTRAINT [PK_MensagemTelemetria] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ModeloPlacas]    Script Date: 04/28/2023 11:21:37 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ModeloPlacas](
	[id] [bigint] IDENTITY(1,1) NOT NULL,
	[Tipo] [varchar](20) NOT NULL,
	[TamanhoMenssagem] [int] NOT NULL,
	[Chave] [nvarchar](20) NULL,
	[ModeloMensagem] [nvarchar](max) NULL,
 CONSTRAINT [PK_BoardModel] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ModeloVeiculo]    Script Date: 04/28/2023 11:21:37 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ModeloVeiculo](
	[id] [bigint] IDENTITY(1,1) NOT NULL,
	[MarcaVeiculo] [nvarchar](50) NOT NULL,
	[Titulo] [varchar](50) NOT NULL,
 CONSTRAINT [PK_ModeloVeiculo] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[OCR]    Script Date: 04/28/2023 11:21:37 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[OCR](
	[idEquipamento] [bigint] NOT NULL,
	[idTelemetria] [bigint] NULL,
	[CaminhoGravacao] [varchar](250) NOT NULL,
	[Leitor] [varchar](50) NOT NULL,
	[EscalaLargura] [int] NOT NULL,
	[EscalAltura] [int] NOT NULL,
	[Variancia] [int] NOT NULL,
	[Limiar] [int] NOT NULL,
	[CasasDecimais] [int] NOT NULL,
	[Espessura] [int] NOT NULL,
	[ProporcaoUm] [int] NOT NULL,
	[Digitos] [int] NOT NULL,
	[SepararDigitor] [bit] NOT NULL,
	[Zonas] [int] NULL,
	[x1] [varchar](100) NOT NULL,
	[x2] [varchar](100) NOT NULL,
	[x3] [varchar](100) NOT NULL,
	[x4] [varchar](100) NOT NULL,
	[y1] [varchar](100) NOT NULL,
	[y2] [varchar](100) NOT NULL,
	[y3] [varchar](100) NOT NULL,
	[y4] [varchar](100) NOT NULL,
	[DeletarImagem] [bit] NOT NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Ocupacao]    Script Date: 04/28/2023 11:21:37 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Ocupacao](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[DataHora] [datetime] NOT NULL,
	[Ocupacao] [bigint] NOT NULL,
 CONSTRAINT [PK_Ocupacao] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Origens]    Script Date: 04/28/2023 11:21:37 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Origens](
	[id] [bigint] IDENTITY(1,1) NOT NULL,
	[Titulo] [varchar](50) NOT NULL,
	[Tipo] [varchar](20) NOT NULL,
	[CodigoOrigem] [varchar](50) NOT NULL,
	[idTelemetria] [bigint] NOT NULL,
 CONSTRAINT [PK_Origens] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Perfil]    Script Date: 04/28/2023 11:21:37 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Perfil](
	[id] [bigint] IDENTITY(1,1) NOT NULL,
	[Nome] [varchar](50) NOT NULL,
	[AcessoRegistros] [bit] NOT NULL,
	[AcessoPermanentes] [bit] NOT NULL,
	[AcessoVisitantes] [bit] NOT NULL,
	[AcessoModelo] [bit] NOT NULL,
	[AcessoPeriodos] [bit] NOT NULL,
	[AcessoHorarios] [bit] NOT NULL,
	[AcessoOrigens] [bit] NOT NULL,
	[AcessoAcoes] [bit] NOT NULL,
	[AcessoPlacasRestritas] [bit] NOT NULL,
	[AcessoLpr] [bit] NOT NULL,
	[AcessoAnalisador] [bit] NOT NULL,
	[AcessoControladorCamera] [bit] NOT NULL,
	[AcessoMasterEye] [bit] NULL,
	[AcessoTelemetria] [bit] NULL,
	[AcessoAlarmes] [bit] NULL,
	[AcessoGravacao] [bit] NULL,
	[AcessoAdicionarEquipamento] [bit] NOT NULL,
	[AcessoRemoverEquipamento] [bit] NOT NULL,
	[AcessoEditarEquipamento] [bit] NOT NULL,
	[AcessoAdicionarGrupo] [bit] NOT NULL,
	[AcessoRemoverGrupo] [bit] NOT NULL,
	[AcessoEditarGrupo] [bit] NOT NULL,
 CONSTRAINT [PK_idPerfil] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Periodos]    Script Date: 04/28/2023 11:21:37 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Periodos](
	[id] [bigint] IDENTITY(1,1) NOT NULL,
	[Titulo] [varchar](30) NOT NULL,
	[Horas] [int] NOT NULL,
 CONSTRAINT [PK_Periodos] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Permanentes]    Script Date: 04/28/2023 11:21:37 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Permanentes](
	[id] [bigint] IDENTITY(1,1) NOT NULL,
	[idHorarios] [bigint] NOT NULL,
	[idModeloVeiculo] [bigint] NOT NULL,
	[Nome] [varchar](50) NOT NULL,
	[Cargo] [varchar](30) NULL,
	[Placa] [varchar](10) NOT NULL,
	[Matricula] [varchar](10) NOT NULL,
	[ValidadeDe] [datetime] NULL,
	[ValidadeAte] [datetime] NULL,
 CONSTRAINT [PK_Permanentes] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
 CONSTRAINT [AK_Permanentes_Matricula] UNIQUE NONCLUSTERED 
(
	[Matricula] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
 CONSTRAINT [AK_Permanentes_Placa] UNIQUE NONCLUSTERED 
(
	[Placa] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PlacasRestritas]    Script Date: 04/28/2023 11:21:37 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PlacasRestritas](
	[id] [bigint] IDENTITY(1,1) NOT NULL,
	[Placa] [varchar](10) NOT NULL,
	[Descricao] [varchar](200) NOT NULL,
	[ValidadeDe] [datetime] NULL,
	[ValidadeAte] [datetime] NULL,
 CONSTRAINT [PK_PlacasRestritas] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Registros]    Script Date: 04/28/2023 11:21:37 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Registros](
	[id] [bigint] IDENTITY(1,1) NOT NULL,
	[idEquipamento] [bigint] NULL,
	[idVisitantes] [bigint] NULL,
	[idPermanentes] [bigint] NULL,
	[idOrigens] [bigint] NOT NULL,
	[Placa] [varchar](10) NOT NULL,
	[DataHora] [datetime] NOT NULL,
	[StatusAutorizacao] [varchar](3) NOT NULL,
	[Aviso] [bit] NOT NULL,
	[Alerta] [bit] NOT NULL,
	[Email] [bit] NOT NULL,
	[Digital] [bit] NOT NULL,
	[Processado] [bit] NULL,
 CONSTRAINT [PK_Registros] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Servers]    Script Date: 04/28/2023 11:21:37 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Servers](
	[idEquipamento] [bigint] NOT NULL,
	[ServidorTelemetria] [bit] NOT NULL,
	[ServidorLpr] [bit] NOT NULL,
	[ServidorMasterEye] [bit] NOT NULL,
	[ServidorDigifort] [bit] NOT NULL,
	[ServidorGravacao] [bit] NOT NULL,
	[ServidorSessao] [bit] NOT NULL,
	[ServidorRtsp] [bit] NOT NULL,
	[Equipamentos] [varchar](max) NOT NULL,
	[Grupos] [varchar](max) NOT NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Sessoes]    Script Date: 04/28/2023 11:21:37 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Sessoes](
	[id] [bigint] IDENTITY(1,1) NOT NULL,
	[Grupo] [bigint] NOT NULL,
	[Participantes] [varchar](max) NULL,
	[Nome] [varchar](100) NOT NULL,
	[Inicio] [datetime] NOT NULL,
	[Fim] [datetime] NOT NULL,
	[Gravacao] [bit] NULL,
	[LocalGravacao] [varchar](max) NULL,
	[Estado] [bit] NOT NULL,
	[Dono] [bigint] NOT NULL,
	[Descricao] [varchar](max) NOT NULL,
	[RtspServer] [bigint] NOT NULL,
	[RtspPrincipal] [varchar](max) NOT NULL,
	[RtspSecundario] [varchar](max) NOT NULL,
	[Requisicoes] [varchar](max) NOT NULL,
	[Cor] [int] NULL,
 CONSTRAINT [PK_Sessoes] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Telemetria]    Script Date: 04/28/2023 11:21:37 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Telemetria](
	[idEquipamento] [bigint] NOT NULL,
	[NumeroSerie] [nvarchar](max) NOT NULL,
	[GatewayIp] [nvarchar](max) NOT NULL,
	[GatewayPort] [nvarchar](max) NOT NULL,
	[ServerRemoto] [varchar](30) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Usuario]    Script Date: 04/28/2023 11:21:37 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Usuario](
	[id] [bigint] IDENTITY(1,1) NOT NULL,
	[Login] [varchar](50) NOT NULL,
	[Nome] [varchar](50) NOT NULL,
	[Senha] [varchar](200) NOT NULL,
	[idPerfil] [bigint] NOT NULL,
	[Imagem] [varchar](max) NULL,
	[Ativo] [bit] NOT NULL,
	[Grupos] [varchar](max) NULL,
	[Equipamentos] [varchar](max) NULL,
	[Mapas] [varchar](max) NULL,
 CONSTRAINT [PK_idUsuario] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Visitantes]    Script Date: 04/28/2023 11:21:37 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Visitantes](
	[id] [bigint] IDENTITY(1,1) NOT NULL,
	[idModeloVeiculo] [bigint] NOT NULL,
	[idPeriodos] [bigint] NOT NULL,
	[Placa] [varchar](10) NOT NULL,
	[Identificador] [varchar](20) NOT NULL,
	[DataAutorizacao] [datetime] NOT NULL,
	[Nome] [varchar](50) NULL,
 CONSTRAINT [PK_Visitantes] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Acoes] ADD  CONSTRAINT [DF_Acoes_Ordem]  DEFAULT ((0)) FOR [idOrigem]
GO
ALTER TABLE [dbo].[Acoes]  WITH CHECK ADD  CONSTRAINT [FK_Acoes_Origens] FOREIGN KEY([idOrigem])
REFERENCES [dbo].[Origens] ([id])
GO
ALTER TABLE [dbo].[Acoes] CHECK CONSTRAINT [FK_Acoes_Origens]
GO
ALTER TABLE [dbo].[AlarmeCameraControl]  WITH CHECK ADD  CONSTRAINT [FK_AlarmeCameraControl_Equipamentos] FOREIGN KEY([idEquipamento])
REFERENCES [dbo].[Equipamentos] ([id])
GO
ALTER TABLE [dbo].[AlarmeCameraControl] CHECK CONSTRAINT [FK_AlarmeCameraControl_Equipamentos]
GO
ALTER TABLE [dbo].[AlarmeLpr]  WITH CHECK ADD  CONSTRAINT [FK_AlarmeLpr_Equipamentos] FOREIGN KEY([idEquipamento])
REFERENCES [dbo].[Equipamentos] ([id])
GO
ALTER TABLE [dbo].[AlarmeLpr] CHECK CONSTRAINT [FK_AlarmeLpr_Equipamentos]
GO
ALTER TABLE [dbo].[AlarmeMastereye]  WITH CHECK ADD  CONSTRAINT [FK_AlarmeMastereye_Equipamentos] FOREIGN KEY([idEquipamento])
REFERENCES [dbo].[Equipamentos] ([id])
GO
ALTER TABLE [dbo].[AlarmeMastereye] CHECK CONSTRAINT [FK_AlarmeMastereye_Equipamentos]
GO
ALTER TABLE [dbo].[CameraControl]  WITH CHECK ADD  CONSTRAINT [FK_CameraControl_Equipamentos] FOREIGN KEY([idEquipamento])
REFERENCES [dbo].[Equipamentos] ([id])
GO
ALTER TABLE [dbo].[CameraControl] CHECK CONSTRAINT [FK_CameraControl_Equipamentos]
GO
ALTER TABLE [dbo].[DVC]  WITH CHECK ADD  CONSTRAINT [FK_DVC_Equipamentos] FOREIGN KEY([idEquipamento])
REFERENCES [dbo].[Equipamentos] ([id])
GO
ALTER TABLE [dbo].[DVC] CHECK CONSTRAINT [FK_DVC_Equipamentos]
GO
ALTER TABLE [dbo].[Gravacao]  WITH CHECK ADD  CONSTRAINT [FK_Gravacao_Equipamentos] FOREIGN KEY([idEquipamento])
REFERENCES [dbo].[Equipamentos] ([id])
GO
ALTER TABLE [dbo].[Gravacao] CHECK CONSTRAINT [FK_Gravacao_Equipamentos]
GO
ALTER TABLE [dbo].[HistoricoDeAcoes]  WITH CHECK ADD  CONSTRAINT [FK_HistoricoDeAcoes_Acoes] FOREIGN KEY([idAcoes])
REFERENCES [dbo].[Acoes] ([id])
GO
ALTER TABLE [dbo].[HistoricoDeAcoes] CHECK CONSTRAINT [FK_HistoricoDeAcoes_Acoes]
GO
ALTER TABLE [dbo].[HistoricoDeAcoes]  WITH CHECK ADD  CONSTRAINT [FK_HistoricoDeAcoes_Registros] FOREIGN KEY([idRegistros])
REFERENCES [dbo].[Registros] ([id])
GO
ALTER TABLE [dbo].[HistoricoDeAcoes] CHECK CONSTRAINT [FK_HistoricoDeAcoes_Registros]
GO
ALTER TABLE [dbo].[Lpr]  WITH CHECK ADD  CONSTRAINT [FK_Coletores_Origens] FOREIGN KEY([idControle])
REFERENCES [dbo].[Equipamentos] ([id])
GO
ALTER TABLE [dbo].[Lpr] CHECK CONSTRAINT [FK_Coletores_Origens]
GO
ALTER TABLE [dbo].[Lpr]  WITH CHECK ADD  CONSTRAINT [FK_Lpr_Equipamentos] FOREIGN KEY([idAcesso])
REFERENCES [dbo].[Equipamentos] ([id])
GO
ALTER TABLE [dbo].[Lpr] CHECK CONSTRAINT [FK_Lpr_Equipamentos]
GO
ALTER TABLE [dbo].[Lpr]  WITH CHECK ADD  CONSTRAINT [FK_Lpr_Equipamentos1] FOREIGN KEY([idControle])
REFERENCES [dbo].[Equipamentos] ([id])
GO
ALTER TABLE [dbo].[Lpr] CHECK CONSTRAINT [FK_Lpr_Equipamentos1]
GO
ALTER TABLE [dbo].[Lpr]  WITH CHECK ADD  CONSTRAINT [FK_Lpr_Equipamentos3] FOREIGN KEY([idContexto1])
REFERENCES [dbo].[Equipamentos] ([id])
GO
ALTER TABLE [dbo].[Lpr] CHECK CONSTRAINT [FK_Lpr_Equipamentos3]
GO
ALTER TABLE [dbo].[Lpr]  WITH CHECK ADD  CONSTRAINT [FK_Lpr_Equipamentos4] FOREIGN KEY([idContexto2])
REFERENCES [dbo].[Equipamentos] ([id])
GO
ALTER TABLE [dbo].[Lpr] CHECK CONSTRAINT [FK_Lpr_Equipamentos4]
GO
ALTER TABLE [dbo].[Lpr]  WITH CHECK ADD  CONSTRAINT [FK_Lpr_Equipamentos5] FOREIGN KEY([idContexto3])
REFERENCES [dbo].[Equipamentos] ([id])
GO
ALTER TABLE [dbo].[Lpr] CHECK CONSTRAINT [FK_Lpr_Equipamentos5]
GO
ALTER TABLE [dbo].[Lpr]  WITH CHECK ADD  CONSTRAINT [FK_Lpr_Equipamentos6] FOREIGN KEY([idContexto4])
REFERENCES [dbo].[Equipamentos] ([id])
GO
ALTER TABLE [dbo].[Lpr] CHECK CONSTRAINT [FK_Lpr_Equipamentos6]
GO
ALTER TABLE [dbo].[MasterEye]  WITH CHECK ADD  CONSTRAINT [FK_MasterEye_Equipamentos] FOREIGN KEY([idEquipamento])
REFERENCES [dbo].[Equipamentos] ([id])
GO
ALTER TABLE [dbo].[MasterEye] CHECK CONSTRAINT [FK_MasterEye_Equipamentos]
GO
ALTER TABLE [dbo].[Permanentes]  WITH CHECK ADD  CONSTRAINT [FK_Permanentes_Horarios] FOREIGN KEY([idHorarios])
REFERENCES [dbo].[Horarios] ([id])
GO
ALTER TABLE [dbo].[Permanentes] CHECK CONSTRAINT [FK_Permanentes_Horarios]
GO
ALTER TABLE [dbo].[Permanentes]  WITH CHECK ADD  CONSTRAINT [FK_Permanentes_ModeloVeiculo] FOREIGN KEY([idModeloVeiculo])
REFERENCES [dbo].[ModeloVeiculo] ([id])
GO
ALTER TABLE [dbo].[Permanentes] CHECK CONSTRAINT [FK_Permanentes_ModeloVeiculo]
GO
ALTER TABLE [dbo].[Registros]  WITH CHECK ADD  CONSTRAINT [FK_Registros_Origens] FOREIGN KEY([idOrigens])
REFERENCES [dbo].[Origens] ([id])
GO
ALTER TABLE [dbo].[Registros] CHECK CONSTRAINT [FK_Registros_Origens]
GO
ALTER TABLE [dbo].[Registros]  WITH CHECK ADD  CONSTRAINT [FK_Registros_Permanentes] FOREIGN KEY([idPermanentes])
REFERENCES [dbo].[Permanentes] ([id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Registros] CHECK CONSTRAINT [FK_Registros_Permanentes]
GO
ALTER TABLE [dbo].[Registros]  WITH CHECK ADD  CONSTRAINT [FK_Registros_Visitantes] FOREIGN KEY([idVisitantes])
REFERENCES [dbo].[Visitantes] ([id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Registros] CHECK CONSTRAINT [FK_Registros_Visitantes]
GO
ALTER TABLE [dbo].[Usuario]  WITH CHECK ADD  CONSTRAINT [FK_Usuario_Perfil] FOREIGN KEY([idPerfil])
REFERENCES [dbo].[Perfil] ([id])
GO
ALTER TABLE [dbo].[Usuario] CHECK CONSTRAINT [FK_Usuario_Perfil]
GO
ALTER TABLE [dbo].[Visitantes]  WITH CHECK ADD  CONSTRAINT [FK_Visitantes_ModeloVeiculo] FOREIGN KEY([idModeloVeiculo])
REFERENCES [dbo].[ModeloVeiculo] ([id])
GO
ALTER TABLE [dbo].[Visitantes] CHECK CONSTRAINT [FK_Visitantes_ModeloVeiculo]
GO
ALTER TABLE [dbo].[Visitantes]  WITH CHECK ADD  CONSTRAINT [FK_Visitantes_Periodos] FOREIGN KEY([idPeriodos])
REFERENCES [dbo].[Periodos] ([id])
GO
ALTER TABLE [dbo].[Visitantes] CHECK CONSTRAINT [FK_Visitantes_Periodos]
GO
USE [master]
GO
ALTER DATABASE [MasterServer] SET  READ_WRITE 
GO
