USE [master]
GO
/****** Object:  Database [NewAssetDB]    Script Date: 7/24/2022 12:24:05 PM ******/
CREATE DATABASE [NewAssetDB]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'NewAssetDB', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL14.MSSQLSERVER\MSSQL\DATA\NewAssetDB.mdf' , SIZE = 73728KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'NewAssetDB_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL14.MSSQLSERVER\MSSQL\DATA\NewAssetDB_log.ldf' , SIZE = 139264KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
GO
ALTER DATABASE [NewAssetDB] SET COMPATIBILITY_LEVEL = 130
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [NewAssetDB].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [NewAssetDB] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [NewAssetDB] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [NewAssetDB] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [NewAssetDB] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [NewAssetDB] SET ARITHABORT OFF 
GO
ALTER DATABASE [NewAssetDB] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [NewAssetDB] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [NewAssetDB] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [NewAssetDB] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [NewAssetDB] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [NewAssetDB] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [NewAssetDB] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [NewAssetDB] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [NewAssetDB] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [NewAssetDB] SET  DISABLE_BROKER 
GO
ALTER DATABASE [NewAssetDB] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [NewAssetDB] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [NewAssetDB] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [NewAssetDB] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [NewAssetDB] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [NewAssetDB] SET READ_COMMITTED_SNAPSHOT ON 
GO
ALTER DATABASE [NewAssetDB] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [NewAssetDB] SET RECOVERY FULL 
GO
ALTER DATABASE [NewAssetDB] SET  MULTI_USER 
GO
ALTER DATABASE [NewAssetDB] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [NewAssetDB] SET DB_CHAINING OFF 
GO
ALTER DATABASE [NewAssetDB] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [NewAssetDB] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [NewAssetDB] SET DELAYED_DURABILITY = DISABLED 
GO
EXEC sys.sp_db_vardecimal_storage_format N'NewAssetDB', N'ON'
GO
ALTER DATABASE [NewAssetDB] SET QUERY_STORE = OFF
GO
USE [NewAssetDB]
GO
ALTER DATABASE SCOPED CONFIGURATION SET IDENTITY_CACHE = ON;
GO
ALTER DATABASE SCOPED CONFIGURATION SET LEGACY_CARDINALITY_ESTIMATION = OFF;
GO
ALTER DATABASE SCOPED CONFIGURATION FOR SECONDARY SET LEGACY_CARDINALITY_ESTIMATION = PRIMARY;
GO
ALTER DATABASE SCOPED CONFIGURATION SET MAXDOP = 0;
GO
ALTER DATABASE SCOPED CONFIGURATION FOR SECONDARY SET MAXDOP = PRIMARY;
GO
ALTER DATABASE SCOPED CONFIGURATION SET PARAMETER_SNIFFING = ON;
GO
ALTER DATABASE SCOPED CONFIGURATION FOR SECONDARY SET PARAMETER_SNIFFING = PRIMARY;
GO
ALTER DATABASE SCOPED CONFIGURATION SET QUERY_OPTIMIZER_HOTFIXES = OFF;
GO
ALTER DATABASE SCOPED CONFIGURATION FOR SECONDARY SET QUERY_OPTIMIZER_HOTFIXES = PRIMARY;
GO
USE [NewAssetDB]
GO
/****** Object:  Table [dbo].[__EFMigrationsHistory]    Script Date: 7/24/2022 12:24:06 PM ******/
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
/****** Object:  Table [dbo].[ApplicationTypes]    Script Date: 7/24/2022 12:24:06 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ApplicationTypes](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Code] [nvarchar](5) NULL,
	[Name] [nvarchar](50) NULL,
	[NameAr] [nvarchar](50) NULL,
 CONSTRAINT [PK_ApplicationTypes] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AspNetRoleClaims]    Script Date: 7/24/2022 12:24:06 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AspNetRoleClaims](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[RoleId] [nvarchar](450) NOT NULL,
	[ClaimType] [nvarchar](max) NULL,
	[ClaimValue] [nvarchar](max) NULL,
 CONSTRAINT [PK_AspNetRoleClaims] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AspNetRoles]    Script Date: 7/24/2022 12:24:06 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AspNetRoles](
	[Id] [nvarchar](450) NOT NULL,
	[Discriminator] [nvarchar](max) NOT NULL,
	[RoleCategoryId] [int] NULL,
	[DisplayName] [nvarchar](50) NULL,
	[Name] [nvarchar](256) NULL,
	[NormalizedName] [nvarchar](256) NULL,
	[ConcurrencyStamp] [nvarchar](max) NULL,
 CONSTRAINT [PK_AspNetRoles] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AspNetUserClaims]    Script Date: 7/24/2022 12:24:06 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AspNetUserClaims](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[UserId] [nvarchar](450) NOT NULL,
	[ClaimType] [nvarchar](max) NULL,
	[ClaimValue] [nvarchar](max) NULL,
 CONSTRAINT [PK_AspNetUserClaims] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AspNetUserLogins]    Script Date: 7/24/2022 12:24:06 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AspNetUserLogins](
	[LoginProvider] [nvarchar](450) NOT NULL,
	[ProviderKey] [nvarchar](450) NOT NULL,
	[ProviderDisplayName] [nvarchar](max) NULL,
	[UserId] [nvarchar](450) NOT NULL,
 CONSTRAINT [PK_AspNetUserLogins] PRIMARY KEY CLUSTERED 
(
	[LoginProvider] ASC,
	[ProviderKey] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AspNetUserRoles]    Script Date: 7/24/2022 12:24:06 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AspNetUserRoles](
	[UserId] [nvarchar](450) NOT NULL,
	[RoleId] [nvarchar](450) NOT NULL,
 CONSTRAINT [PK_AspNetUserRoles] PRIMARY KEY CLUSTERED 
(
	[UserId] ASC,
	[RoleId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AspNetUsers]    Script Date: 7/24/2022 12:24:06 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AspNetUsers](
	[Id] [nvarchar](450) NOT NULL,
	[GovernorateId] [int] NOT NULL,
	[CityId] [int] NOT NULL,
	[OrganizationId] [int] NOT NULL,
	[SubOrganizationId] [int] NOT NULL,
	[HospitalId] [int] NOT NULL,
	[RoleId] [nvarchar](450) NULL,
	[RoleCategoryId] [int] NOT NULL,
	[SupplierId] [int] NULL,
	[CommetieeMemberId] [int] NULL,
	[UserName] [nvarchar](256) NULL,
	[NormalizedUserName] [nvarchar](256) NULL,
	[Email] [nvarchar](256) NULL,
	[NormalizedEmail] [nvarchar](256) NULL,
	[EmailConfirmed] [bit] NOT NULL,
	[PasswordHash] [nvarchar](max) NULL,
	[SecurityStamp] [nvarchar](max) NULL,
	[ConcurrencyStamp] [nvarchar](max) NULL,
	[PhoneNumber] [nvarchar](max) NULL,
	[PhoneNumberConfirmed] [bit] NOT NULL,
	[TwoFactorEnabled] [bit] NOT NULL,
	[LockoutEnd] [datetimeoffset](7) NULL,
	[LockoutEnabled] [bit] NOT NULL,
	[AccessFailedCount] [int] NOT NULL,
 CONSTRAINT [PK_AspNetUsers] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AspNetUserTokens]    Script Date: 7/24/2022 12:24:06 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AspNetUserTokens](
	[UserId] [nvarchar](450) NOT NULL,
	[LoginProvider] [nvarchar](450) NOT NULL,
	[Name] [nvarchar](450) NOT NULL,
	[Value] [nvarchar](max) NULL,
 CONSTRAINT [PK_AspNetUserTokens] PRIMARY KEY CLUSTERED 
(
	[UserId] ASC,
	[LoginProvider] ASC,
	[Name] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AssetDetailAttachments]    Script Date: 7/24/2022 12:24:06 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AssetDetailAttachments](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[AssetDetailId] [int] NULL,
	[FileName] [nvarchar](200) NULL,
	[Title] [nvarchar](100) NULL,
	[HospitalId] [int] NULL,
 CONSTRAINT [PK_AssetDetailAttachments] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AssetDetails]    Script Date: 7/24/2022 12:24:06 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AssetDetails](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Code] [nvarchar](15) NULL,
	[Price] [decimal](18, 2) NULL,
	[SerialNumber] [nvarchar](20) NULL,
	[Remarks] [nvarchar](max) NULL,
	[Barcode] [nvarchar](20) NULL,
	[PurchaseDate] [datetime2](7) NULL,
	[DepreciationRate] [decimal](18, 2) NULL,
	[CostCenter] [nvarchar](50) NULL,
	[InstallationDate] [datetime2](7) NULL,
	[OperationDate] [datetime2](7) NULL,
	[ReceivingDate] [datetime2](7) NULL,
	[PONumber] [nvarchar](50) NULL,
	[WarrantyStart] [date] NULL,
	[WarrantyEnd] [date] NULL,
	[WarrantyExpires] [nvarchar](50) NULL,
	[MasterAssetId] [int] NULL,
	[BuildingId] [int] NULL,
	[FloorId] [int] NULL,
	[RoomId] [int] NULL,
	[HospitalId] [int] NULL,
	[DepartmentId] [int] NULL,
	[SupplierId] [int] NULL,
	[QrFilePath] [nvarchar](255) NULL,
 CONSTRAINT [PK_AssetDetails] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AssetMovements]    Script Date: 7/24/2022 12:24:06 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AssetMovements](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[AssetDetailId] [int] NOT NULL,
	[MovementDate] [datetime2](7) NULL,
	[BuildingId] [int] NULL,
	[FloorId] [int] NULL,
	[RoomId] [int] NULL,
	[MoveDesc] [nvarchar](500) NULL,
	[HospitalId] [int] NULL,
 CONSTRAINT [PK_AssetMovements] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AssetOwners]    Script Date: 7/24/2022 12:24:06 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AssetOwners](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[EmployeeId] [int] NULL,
	[AssetDetailId] [int] NULL,
	[HospitalId] [int] NULL,
 CONSTRAINT [PK_AssetOwners] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AssetPeriorities]    Script Date: 7/24/2022 12:24:06 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AssetPeriorities](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](max) NULL,
	[NameAr] [nvarchar](max) NULL,
 CONSTRAINT [PK_AssetPeriorities] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AssetStatus]    Script Date: 7/24/2022 12:24:06 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AssetStatus](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Code] [nvarchar](5) NULL,
	[Name] [nvarchar](50) NULL,
	[NameAr] [nvarchar](50) NULL,
 CONSTRAINT [PK_AssetStatus] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AssetStatusTransactions]    Script Date: 7/24/2022 12:24:06 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AssetStatusTransactions](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[AssetDetailId] [int] NULL,
	[AssetStatusId] [int] NULL,
	[StatusDate] [datetime2](7) NULL,
	[HospitalId] [int] NULL,
 CONSTRAINT [PK_AssetStatusTransactions] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AssetWorkOrderTasks]    Script Date: 7/24/2022 12:24:06 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AssetWorkOrderTasks](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](max) NULL,
	[NameAr] [nvarchar](max) NULL,
	[Code] [nvarchar](max) NULL,
	[MasterAssetId] [int] NOT NULL,
 CONSTRAINT [PK_AssetWorkOrderTasks] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Brands]    Script Date: 7/24/2022 12:24:06 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Brands](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Code] [nvarchar](5) NULL,
	[Name] [nvarchar](50) NULL,
	[NameAr] [nvarchar](50) NULL,
 CONSTRAINT [PK_Brands] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Buildings]    Script Date: 7/24/2022 12:24:06 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Buildings](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Code] [nvarchar](5) NULL,
	[Name] [nvarchar](50) NULL,
	[NameAr] [nvarchar](50) NULL,
	[Brief] [nvarchar](500) NULL,
	[BriefAr] [nvarchar](500) NULL,
	[HospitalId] [int] NULL,
 CONSTRAINT [PK_Buildings] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Categories]    Script Date: 7/24/2022 12:24:06 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Categories](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Code] [nvarchar](5) NULL,
	[Name] [nvarchar](100) NULL,
	[NameAr] [nvarchar](100) NULL,
	[CategoryTypeId] [int] NULL,
 CONSTRAINT [PK_Categories] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[CategoryTypes]    Script Date: 7/24/2022 12:24:06 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CategoryTypes](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Code] [nvarchar](5) NULL,
	[Name] [nvarchar](100) NULL,
	[NameAr] [nvarchar](100) NULL,
 CONSTRAINT [PK_CategoryTypes] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Cities]    Script Date: 7/24/2022 12:24:06 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Cities](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Code] [nvarchar](5) NULL,
	[Name] [nvarchar](50) NULL,
	[NameAr] [nvarchar](50) NULL,
	[GovernorateId] [int] NULL,
	[Latitude] [decimal](18, 8) NULL,
	[Longtitude] [decimal](18, 8) NULL,
 CONSTRAINT [PK_Cities] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Classifications]    Script Date: 7/24/2022 12:24:06 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Classifications](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Code] [nvarchar](5) NULL,
	[Name] [nvarchar](50) NULL,
	[NameAr] [nvarchar](50) NULL,
 CONSTRAINT [PK_Classifications] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[CommetieeMembers]    Script Date: 7/24/2022 12:24:06 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CommetieeMembers](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Code] [nvarchar](5) NULL,
	[Name] [nvarchar](max) NULL,
	[NameAr] [nvarchar](max) NULL,
	[Mobile] [nvarchar](max) NULL,
	[Website] [nvarchar](2083) NULL,
	[EMail] [nvarchar](320) NULL,
	[Address] [nvarchar](max) NULL,
	[AddressAr] [nvarchar](max) NULL,
 CONSTRAINT [PK_CommetieeMembers] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ContractAttachments]    Script Date: 7/24/2022 12:24:06 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ContractAttachments](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[MasterContractId] [int] NULL,
	[DocumentName] [nvarchar](100) NULL,
	[FileName] [nvarchar](25) NULL,
	[HospitalId] [int] NULL,
 CONSTRAINT [PK_ContractAttachments] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ContractDetails]    Script Date: 7/24/2022 12:24:06 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ContractDetails](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[MasterContractId] [int] NULL,
	[AssetDetailId] [int] NULL,
	[HasSpareParts] [bit] NULL,
	[ContractDate] [datetime2](7) NULL,
	[ResponseTime] [int] NULL,
	[HospitalId] [int] NULL,
 CONSTRAINT [PK_ContractDetails] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Departments]    Script Date: 7/24/2022 12:24:06 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Departments](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Code] [nvarchar](5) NULL,
	[Name] [nvarchar](50) NULL,
	[NameAr] [nvarchar](50) NULL,
 CONSTRAINT [PK_Departments] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ECRIS]    Script Date: 7/24/2022 12:24:06 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ECRIS](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Code] [nvarchar](5) NULL,
	[Name] [nvarchar](100) NULL,
	[NameAr] [nvarchar](100) NULL,
 CONSTRAINT [PK_ECRIS] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Employees]    Script Date: 7/24/2022 12:24:06 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Employees](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Code] [nvarchar](5) NULL,
	[Name] [nvarchar](50) NULL,
	[NameAr] [nvarchar](50) NULL,
	[CardId] [nvarchar](14) NULL,
	[Phone] [nvarchar](15) NULL,
	[WhatsApp] [nvarchar](15) NULL,
	[Dob] [datetime2](7) NULL,
	[EmpImg] [nvarchar](100) NULL,
	[Email] [nvarchar](320) NULL,
	[Address] [nvarchar](max) NULL,
	[AddressAr] [nvarchar](max) NULL,
	[GenderId] [int] NULL,
	[HospitalId] [int] NULL,
	[DepartmentId] [int] NULL,
	[ClassificationId] [int] NULL,
 CONSTRAINT [PK_Employees] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Engineers]    Script Date: 7/24/2022 12:24:06 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Engineers](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Code] [nvarchar](15) NULL,
	[Name] [nvarchar](50) NULL,
	[NameAr] [nvarchar](50) NULL,
	[CardId] [nvarchar](14) NULL,
	[Phone] [nvarchar](15) NULL,
	[WhatsApp] [nvarchar](15) NULL,
	[Dob] [datetime2](7) NULL,
	[EngImg] [nvarchar](100) NULL,
	[Email] [nvarchar](320) NULL,
	[Address] [nvarchar](max) NULL,
	[AddressAr] [nvarchar](max) NULL,
	[GenderId] [int] NULL,
 CONSTRAINT [PK_Engineers] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Floors]    Script Date: 7/24/2022 12:24:06 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Floors](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Code] [nvarchar](5) NULL,
	[Name] [nvarchar](50) NULL,
	[NameAr] [nvarchar](50) NULL,
	[BuildingId] [int] NULL,
	[HospitalId] [int] NULL,
 CONSTRAINT [PK_Floors] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Governorates]    Script Date: 7/24/2022 12:24:07 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Governorates](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Code] [nvarchar](5) NULL,
	[Name] [nvarchar](50) NULL,
	[NameAr] [nvarchar](50) NULL,
	[Population] [decimal](18, 3) NULL,
	[Area] [decimal](18, 5) NULL,
	[Latitude] [decimal](18, 8) NULL,
	[Longtitude] [decimal](18, 8) NULL,
 CONSTRAINT [PK_Governorates_1] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[HospitalApplicationAttachments]    Script Date: 7/24/2022 12:24:07 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[HospitalApplicationAttachments](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[HospitalReasonTransactionId] [int] NULL,
	[Title] [nvarchar](100) NULL,
	[FileName] [nvarchar](25) NULL,
	[HospitalId] [int] NULL,
 CONSTRAINT [PK_HospitalApplicationAttachments] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[HospitalApplications]    Script Date: 7/24/2022 12:24:07 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[HospitalApplications](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[AppNumber] [nvarchar](50) NULL,
	[AssetId] [int] NULL,
	[StatusId] [int] NULL,
	[UserId] [nvarchar](450) NULL,
	[AppTypeId] [int] NULL,
	[AppDate] [datetime] NULL,
	[DueDate] [datetime] NULL,
	[ActionDate] [datetime] NULL,
	[Comment] [nvarchar](500) NULL,
	[HospitalId] [int] NULL,
 CONSTRAINT [PK_HospitalExecludes] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[HospitalDepartments]    Script Date: 7/24/2022 12:24:07 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[HospitalDepartments](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[HospitalId] [int] NULL,
	[DepartmentId] [int] NULL,
	[IsActive] [bit] NULL,
 CONSTRAINT [PK_HospitalDepartments] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[HospitalEngineers]    Script Date: 7/24/2022 12:24:07 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[HospitalEngineers](
	[Id] [int] NOT NULL,
	[EngId] [int] NULL,
	[HospId] [int] NULL,
 CONSTRAINT [PK_HospitalEngineers] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[HospitalExecludeReasons]    Script Date: 7/24/2022 12:24:07 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[HospitalExecludeReasons](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Code] [nvarchar](5) NULL,
	[Name] [nvarchar](50) NULL,
	[NameAr] [nvarchar](50) NULL,
 CONSTRAINT [PK_HospitalExecludeReasons] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[HospitalHoldReasons]    Script Date: 7/24/2022 12:24:07 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[HospitalHoldReasons](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Code] [nvarchar](5) NULL,
	[Name] [nvarchar](50) NULL,
	[NameAr] [nvarchar](50) NULL,
 CONSTRAINT [PK_HospitalHoldReasons] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[HospitalReasonTransactions]    Script Date: 7/24/2022 12:24:07 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[HospitalReasonTransactions](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[HospitalApplicationId] [int] NULL,
	[ReasonId] [int] NULL,
	[HospitalId] [int] NULL,
 CONSTRAINT [PK_HospitalReasonTrasactions] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Hospitals]    Script Date: 7/24/2022 12:24:07 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Hospitals](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Code] [nvarchar](5) NULL,
	[Name] [nvarchar](50) NULL,
	[NameAr] [nvarchar](50) NULL,
	[Email] [nvarchar](320) NULL,
	[Mobile] [nvarchar](20) NULL,
	[ManagerName] [nvarchar](50) NULL,
	[ManagerNameAr] [nvarchar](50) NULL,
	[Latitude] [float] NULL,
	[Longtitude] [float] NULL,
	[Address] [nvarchar](max) NULL,
	[AddressAr] [nvarchar](max) NULL,
	[GovernorateId] [int] NULL,
	[CityId] [int] NULL,
	[OrganizationId] [int] NULL,
	[SubOrganizationId] [int] NULL,
 CONSTRAINT [PK_Hospitals] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[HospitalSupplierStatuses]    Script Date: 7/24/2022 12:24:07 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[HospitalSupplierStatuses](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](50) NULL,
	[NameAr] [nvarchar](50) NULL,
	[Color] [nvarchar](50) NULL,
	[Icon] [nvarchar](50) NULL,
 CONSTRAINT [PK_HospitalSupplierStatus] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[MasterAssetAttachments]    Script Date: 7/24/2022 12:24:07 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[MasterAssetAttachments](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[MasterAssetId] [int] NOT NULL,
	[Title] [nvarchar](100) NULL,
	[FileName] [nvarchar](25) NULL,
 CONSTRAINT [PK_MasterAssetAttachments] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[MasterAssetComponents]    Script Date: 7/24/2022 12:24:07 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[MasterAssetComponents](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Code] [nvarchar](5) NULL,
	[Name] [nvarchar](100) NULL,
	[NameAr] [nvarchar](100) NULL,
	[PartNo] [nvarchar](20) NULL,
	[MasterAssetId] [int] NULL,
	[Price] [decimal](18, 2) NULL,
	[Description] [nvarchar](max) NULL,
	[DescriptionAr] [nvarchar](max) NULL,
 CONSTRAINT [PK_MasterAssetComponents] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[MasterAssets]    Script Date: 7/24/2022 12:24:07 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[MasterAssets](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](100) NULL,
	[NameAr] [nvarchar](100) NULL,
	[Code] [nvarchar](5) NULL,
	[Description] [nvarchar](max) NULL,
	[DescriptionAr] [nvarchar](max) NULL,
	[ExpectedLifeTime] [int] NULL,
	[ECRIId] [int] NULL,
	[ModelNumber] [nvarchar](20) NULL,
	[VersionNumber] [nvarchar](20) NULL,
	[PeriorityId] [int] NULL,
	[OriginId] [int] NULL,
	[BrandId] [int] NULL,
	[CategoryId] [int] NULL,
	[SubCategoryId] [int] NULL,
	[Length] [float] NULL,
	[Height] [float] NULL,
	[Width] [float] NULL,
	[Weight] [float] NULL,
	[Power] [nvarchar](10) NULL,
	[Voltage] [nvarchar](10) NULL,
	[Ampair] [nvarchar](10) NULL,
	[Frequency] [nvarchar](10) NULL,
	[ElectricRequirement] [nvarchar](10) NULL,
	[PMColor] [nvarchar](10) NULL,
	[PMBGColor] [nvarchar](10) NULL,
	[PMTimeId] [int] NULL,
	[AssetImg] [nvarchar](50) NULL,
 CONSTRAINT [PK_MasterAssets] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[MasterContracts]    Script Date: 7/24/2022 12:24:07 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[MasterContracts](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Serial] [nvarchar](50) NULL,
	[Subject] [nvarchar](100) NULL,
	[ContractDate] [date] NULL,
	[From] [date] NULL,
	[To] [date] NULL,
	[Cost] [decimal](18, 2) NULL,
	[SupplierId] [int] NULL,
	[HospitalId] [int] NULL,
 CONSTRAINT [PK_MasterContracts] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Organizations]    Script Date: 7/24/2022 12:24:07 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Organizations](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Code] [nvarchar](5) NULL,
	[Name] [nvarchar](50) NULL,
	[NameAr] [nvarchar](50) NULL,
	[Address] [nvarchar](max) NULL,
	[AddressAr] [nvarchar](max) NULL,
	[Email] [nvarchar](320) NULL,
	[Mobile] [nvarchar](20) NULL,
	[DirectorName] [nvarchar](50) NULL,
	[DirectorNameAr] [nvarchar](50) NULL,
 CONSTRAINT [PK_Organizations] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Origins]    Script Date: 7/24/2022 12:24:07 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Origins](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Code] [nvarchar](5) NULL,
	[Name] [nvarchar](50) NULL,
	[NameAr] [nvarchar](50) NULL,
 CONSTRAINT [PK_Origins] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Phase]    Script Date: 7/24/2022 12:24:07 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Phase](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[PhaseName] [nvarchar](max) NULL,
 CONSTRAINT [PK_Phase] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PMAssetTasks]    Script Date: 7/24/2022 12:24:07 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PMAssetTasks](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](50) NULL,
	[NameAr] [nvarchar](50) NULL,
	[MasterAssetId] [int] NULL,
 CONSTRAINT [PK_PMAssetTasks] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PMAssetTaskSchedules]    Script Date: 7/24/2022 12:24:07 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PMAssetTaskSchedules](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[PMAssetTimeId] [int] NULL,
	[PMAssetTaskId] [int] NULL,
	[HospitalId] [int] NULL,
 CONSTRAINT [PK_PMTaskSchedules] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PMAssetTimes]    Script Date: 7/24/2022 12:24:07 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PMAssetTimes](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[AssetDetailId] [int] NULL,
	[PMDate] [date] NULL,
	[HospitalId] [int] NULL,
 CONSTRAINT [PK_PMAssetTimes] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PMTimes]    Script Date: 7/24/2022 12:24:07 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PMTimes](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](50) NULL,
	[NameAr] [nvarchar](50) NULL,
 CONSTRAINT [PK_PMTimes] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Problems]    Script Date: 7/24/2022 12:24:07 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Problems](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Code] [nvarchar](max) NULL,
	[Name] [nvarchar](max) NULL,
	[NameAr] [nvarchar](max) NULL,
	[MasterAssetId] [int] NULL,
 CONSTRAINT [PK_Problems] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Request]    Script Date: 7/24/2022 12:24:07 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Request](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Subject] [nvarchar](100) NULL,
	[RequestCode] [nvarchar](15) NULL,
	[Description] [nvarchar](max) NULL,
	[RequestDate] [datetime2](7) NULL,
	[RequestModeId] [int] NULL,
	[SubProblemId] [int] NULL,
	[AssetDetailId] [int] NULL,
	[RequestPeriorityId] [int] NULL,
	[RequestTypeId] [int] NULL,
	[CreatedById] [nvarchar](450) NULL,
	[IsOpened] [bit] NULL,
	[HospitalId] [int] NULL,
 CONSTRAINT [PK_Request] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[RequestDocument]    Script Date: 7/24/2022 12:24:07 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[RequestDocument](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[DocumentName] [nvarchar](100) NULL,
	[FileName] [nvarchar](25) NULL,
	[RequestTrackingId] [int] NULL,
	[HospitalId] [int] NULL,
 CONSTRAINT [PK_RequestDocument] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[RequestMode]    Script Date: 7/24/2022 12:24:07 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[RequestMode](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](50) NULL,
	[NameAr] [nvarchar](50) NULL,
 CONSTRAINT [PK_RequestMode] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[RequestPeriority]    Script Date: 7/24/2022 12:24:07 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[RequestPeriority](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](50) NULL,
	[NameAr] [nvarchar](50) NULL,
	[Color] [nvarchar](10) NULL,
	[Icon] [nvarchar](30) NULL,
 CONSTRAINT [PK_RequestPeriority] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[RequestPhase]    Script Date: 7/24/2022 12:24:07 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[RequestPhase](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[PhaseId] [int] NOT NULL,
	[RequestId] [int] NOT NULL,
	[EmployeeId] [int] NOT NULL,
 CONSTRAINT [PK_RequestPhase] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[RequestStatus]    Script Date: 7/24/2022 12:24:07 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[RequestStatus](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](50) NULL,
	[NameAr] [nvarchar](50) NULL,
	[Color] [nvarchar](50) NULL,
	[Icon] [nvarchar](50) NULL,
 CONSTRAINT [PK_RequestStatus] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[RequestTracking]    Script Date: 7/24/2022 12:24:07 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[RequestTracking](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Description] [nvarchar](max) NULL,
	[DescriptionDate] [datetime2](7) NULL,
	[RequestStatusId] [int] NULL,
	[RequestId] [int] NULL,
	[CreatedById] [nvarchar](450) NULL,
	[IsOpened] [bit] NULL,
	[HospitalId] [int] NULL,
 CONSTRAINT [PK_RequestTracking] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[RequestTypes]    Script Date: 7/24/2022 12:24:07 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[RequestTypes](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Code] [nvarchar](max) NULL,
	[Name] [nvarchar](max) NULL,
	[NameAr] [nvarchar](max) NULL,
 CONSTRAINT [PK_RequestTypes] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[RoleCategories]    Script Date: 7/24/2022 12:24:08 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[RoleCategories](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](50) NULL,
	[NameAr] [nvarchar](50) NULL,
	[OrderId] [int] NULL,
 CONSTRAINT [PK_RoleCategories] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Rooms]    Script Date: 7/24/2022 12:24:08 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Rooms](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Code] [nvarchar](5) NULL,
	[Name] [nvarchar](50) NULL,
	[NameAr] [nvarchar](50) NULL,
	[FloorId] [int] NULL,
	[HospitalId] [int] NULL,
 CONSTRAINT [PK_Rooms] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[SubCategories]    Script Date: 7/24/2022 12:24:08 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SubCategories](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Code] [nvarchar](5) NULL,
	[Name] [nvarchar](50) NULL,
	[NameAr] [nvarchar](50) NULL,
	[CategoryId] [int] NULL,
 CONSTRAINT [PK_SubCategories] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[SubOrganizations]    Script Date: 7/24/2022 12:24:08 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SubOrganizations](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Code] [nvarchar](5) NULL,
	[Name] [nvarchar](50) NULL,
	[NameAr] [nvarchar](50) NULL,
	[Address] [nvarchar](max) NULL,
	[AddressAr] [nvarchar](max) NULL,
	[Email] [nvarchar](320) NULL,
	[Mobile] [nvarchar](20) NULL,
	[DirectorName] [nvarchar](50) NULL,
	[DirectorNameAr] [nvarchar](50) NULL,
	[OrganizationId] [int] NOT NULL,
 CONSTRAINT [PK_SubOrganizations] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[SubProblems]    Script Date: 7/24/2022 12:24:08 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SubProblems](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Code] [nvarchar](max) NULL,
	[Name] [nvarchar](max) NULL,
	[NameAr] [nvarchar](max) NULL,
	[ProblemId] [int] NULL,
 CONSTRAINT [PK_SubProblems] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[SupplierExecludeAssets]    Script Date: 7/24/2022 12:24:08 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SupplierExecludeAssets](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[AppTypeId] [int] NULL,
	[AssetId] [int] NULL,
	[StatusId] [int] NULL,
	[UserId] [nvarchar](450) NULL,
	[Date] [datetime] NULL,
	[ExecludeDate] [datetime] NULL,
	[ExNumber] [nvarchar](50) NULL,
	[ActionDate] [datetime] NULL,
	[Comment] [nvarchar](500) NULL,
	[HospitalId] [int] NULL,
 CONSTRAINT [PK_SupplierExecludes] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[SupplierExecludeAttachments]    Script Date: 7/24/2022 12:24:08 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SupplierExecludeAttachments](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[SupplierExecludeId] [int] NULL,
	[FileName] [nvarchar](25) NULL,
	[Title] [nvarchar](100) NULL,
	[HospitalId] [int] NULL,
 CONSTRAINT [PK_SupplierExecludeAttachments] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[SupplierExecludeReasons]    Script Date: 7/24/2022 12:24:08 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SupplierExecludeReasons](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](50) NULL,
	[NameAr] [nvarchar](50) NULL,
 CONSTRAINT [PK_SupplierExecludeReasons] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[SupplierExecludes]    Script Date: 7/24/2022 12:24:08 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SupplierExecludes](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[SupplierExecludeAssetId] [int] NULL,
	[ReasonId] [int] NULL,
	[HospitalId] [int] NULL,
 CONSTRAINT [PK_SupplierExecludes_1] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[SupplierHoldReasons]    Script Date: 7/24/2022 12:24:08 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SupplierHoldReasons](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Code] [nvarchar](5) NULL,
	[Name] [nvarchar](150) NULL,
	[NameAr] [nvarchar](150) NULL,
 CONSTRAINT [PK_SupplierHoldReasons] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Suppliers]    Script Date: 7/24/2022 12:24:08 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Suppliers](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Code] [nvarchar](5) NULL,
	[Name] [nvarchar](max) NULL,
	[NameAr] [nvarchar](max) NULL,
	[Mobile] [nvarchar](max) NULL,
	[Website] [nvarchar](2083) NULL,
	[EMail] [nvarchar](320) NULL,
	[Address] [nvarchar](max) NULL,
	[AddressAr] [nvarchar](max) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[VisitAttachments]    Script Date: 7/24/2022 12:24:08 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[VisitAttachments](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[VisitId] [int] NULL,
	[FileName] [nvarchar](25) NULL,
	[Title] [nvarchar](100) NULL,
 CONSTRAINT [PK_VisitAttachments] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Visits]    Script Date: 7/24/2022 12:24:08 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Visits](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[EngineerId] [int] NULL,
	[HospitalId] [int] NULL,
	[VisitDate] [datetime2](7) NULL,
	[VisitTypeId] [int] NULL,
	[VisitDescr] [nvarchar](max) NULL,
	[StatusId] [int] NULL,
	[Code] [nvarchar](20) NULL,
 CONSTRAINT [PK_Visits] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[VisitTypes]    Script Date: 7/24/2022 12:24:08 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[VisitTypes](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[TypeDesc] [nvarchar](max) NULL,
	[Code] [nvarchar](15) NULL,
	[Name] [nvarchar](50) NULL,
	[NameAr] [nvarchar](50) NULL,
 CONSTRAINT [PK_VisitTypes] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[WorkOrderAssigns]    Script Date: 7/24/2022 12:24:08 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[WorkOrderAssigns](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[WOTId] [int] NULL,
	[UserId] [nvarchar](450) NULL,
	[SupplierId] [int] NULL,
	[Notes] [nvarchar](max) NULL,
	[CreatedBy] [nvarchar](450) NULL,
	[CreatedDate] [date] NULL,
 CONSTRAINT [PK_WorkOrderAssigns] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[WorkOrderAttachments]    Script Date: 7/24/2022 12:24:08 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[WorkOrderAttachments](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[DocumentName] [nvarchar](100) NULL,
	[FileName] [nvarchar](25) NULL,
	[WorkOrderTrackingId] [int] NULL,
	[HospitalId] [int] NULL,
 CONSTRAINT [PK_WorkOrderAttachments] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[WorkOrderPeriorities]    Script Date: 7/24/2022 12:24:08 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[WorkOrderPeriorities](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Code] [nvarchar](max) NULL,
	[Name] [nvarchar](max) NULL,
	[NameAr] [nvarchar](max) NULL,
 CONSTRAINT [PK_WorkOrderPeriorities] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[WorkOrders]    Script Date: 7/24/2022 12:24:08 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[WorkOrders](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Subject] [nvarchar](max) NULL,
	[WorkOrderNumber] [nvarchar](max) NULL,
	[CreationDate] [datetime2](7) NULL,
	[PlannedStartDate] [datetime2](7) NULL,
	[PlannedEndDate] [datetime2](7) NULL,
	[ActualStartDate] [datetime2](7) NULL,
	[ActualEndDate] [datetime2](7) NULL,
	[Note] [nvarchar](max) NULL,
	[CreatedById] [nvarchar](450) NULL,
	[WorkOrderPeriorityId] [int] NULL,
	[WorkOrderTypeId] [int] NULL,
	[RequestId] [int] NULL,
	[HospitalId] [int] NULL,
 CONSTRAINT [PK_WorkOrders] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[WorkOrderStatuses]    Script Date: 7/24/2022 12:24:08 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[WorkOrderStatuses](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Code] [nvarchar](50) NULL,
	[Name] [nvarchar](50) NULL,
	[NameAr] [nvarchar](50) NULL,
	[Color] [nvarchar](10) NULL,
	[Icon] [nvarchar](30) NULL,
 CONSTRAINT [PK_WorkOrderStatuses] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[WorkOrderTasks]    Script Date: 7/24/2022 12:24:08 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[WorkOrderTasks](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Comment] [nvarchar](max) NULL,
	[AssetWorkOrderTaskId] [int] NOT NULL,
	[WorkOrderId] [int] NOT NULL,
 CONSTRAINT [PK_WorkOrderTasks] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[WorkOrderTrackings]    Script Date: 7/24/2022 12:24:08 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[WorkOrderTrackings](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[WorkOrderDate] [datetime2](7) NULL,
	[CreationDate] [datetime2](7) NULL,
	[Notes] [nvarchar](max) NULL,
	[CreatedById] [nvarchar](450) NULL,
	[AssignedTo] [nvarchar](450) NULL,
	[WorkOrderStatusId] [int] NULL,
	[WorkOrderId] [int] NULL,
	[ActualStartDate] [datetime2](7) NULL,
	[ActualEndDate] [datetime2](7) NULL,
	[PlannedStartDate] [date] NULL,
	[PlannedEndDate] [date] NULL,
	[HospitalId] [int] NULL,
 CONSTRAINT [PK_WorkOrderTrackings] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[WorkOrderTypes]    Script Date: 7/24/2022 12:24:08 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[WorkOrderTypes](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Code] [nvarchar](max) NULL,
	[Name] [nvarchar](max) NULL,
	[NameAr] [nvarchar](max) NULL,
 CONSTRAINT [PK_WorkOrderTypes] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_AspNetRoleClaims_RoleId]    Script Date: 7/24/2022 12:24:08 PM ******/
CREATE NONCLUSTERED INDEX [IX_AspNetRoleClaims_RoleId] ON [dbo].[AspNetRoleClaims]
(
	[RoleId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [RoleNameIndex]    Script Date: 7/24/2022 12:24:08 PM ******/
CREATE UNIQUE NONCLUSTERED INDEX [RoleNameIndex] ON [dbo].[AspNetRoles]
(
	[NormalizedName] ASC
)
WHERE ([NormalizedName] IS NOT NULL)
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_AspNetUserClaims_UserId]    Script Date: 7/24/2022 12:24:08 PM ******/
CREATE NONCLUSTERED INDEX [IX_AspNetUserClaims_UserId] ON [dbo].[AspNetUserClaims]
(
	[UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_AspNetUserLogins_UserId]    Script Date: 7/24/2022 12:24:08 PM ******/
CREATE NONCLUSTERED INDEX [IX_AspNetUserLogins_UserId] ON [dbo].[AspNetUserLogins]
(
	[UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_AspNetUserRoles_RoleId]    Script Date: 7/24/2022 12:24:08 PM ******/
CREATE NONCLUSTERED INDEX [IX_AspNetUserRoles_RoleId] ON [dbo].[AspNetUserRoles]
(
	[RoleId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [EmailIndex]    Script Date: 7/24/2022 12:24:08 PM ******/
CREATE NONCLUSTERED INDEX [EmailIndex] ON [dbo].[AspNetUsers]
(
	[NormalizedEmail] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [UserNameIndex]    Script Date: 7/24/2022 12:24:08 PM ******/
CREATE UNIQUE NONCLUSTERED INDEX [UserNameIndex] ON [dbo].[AspNetUsers]
(
	[NormalizedUserName] ASC
)
WHERE ([NormalizedUserName] IS NOT NULL)
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
ALTER TABLE [dbo].[AssetWorkOrderTasks] ADD  DEFAULT ((0)) FOR [MasterAssetId]
GO
ALTER TABLE [dbo].[Request] ADD  CONSTRAINT [DF_Request_IsOpened]  DEFAULT ((0)) FOR [IsOpened]
GO
ALTER TABLE [dbo].[RequestTracking] ADD  CONSTRAINT [DF_RequestTracking_IsOpened]  DEFAULT ((0)) FOR [IsOpened]
GO
ALTER TABLE [dbo].[WorkOrderTasks] ADD  DEFAULT ((0)) FOR [AssetWorkOrderTaskId]
GO
ALTER TABLE [dbo].[WorkOrderTasks] ADD  DEFAULT ((0)) FOR [WorkOrderId]
GO
ALTER TABLE [dbo].[WorkOrderTrackings] ADD  CONSTRAINT [DF__WorkOrder__WorkO__2E26C93A]  DEFAULT ((0)) FOR [WorkOrderId]
GO
ALTER TABLE [dbo].[AspNetRoleClaims]  WITH CHECK ADD  CONSTRAINT [FK_AspNetRoleClaims_AspNetRoles_RoleId] FOREIGN KEY([RoleId])
REFERENCES [dbo].[AspNetRoles] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AspNetRoleClaims] CHECK CONSTRAINT [FK_AspNetRoleClaims_AspNetRoles_RoleId]
GO
ALTER TABLE [dbo].[AspNetUserClaims]  WITH CHECK ADD  CONSTRAINT [FK_AspNetUserClaims_AspNetUsers_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[AspNetUsers] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AspNetUserClaims] CHECK CONSTRAINT [FK_AspNetUserClaims_AspNetUsers_UserId]
GO
ALTER TABLE [dbo].[AspNetUserLogins]  WITH CHECK ADD  CONSTRAINT [FK_AspNetUserLogins_AspNetUsers_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[AspNetUsers] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AspNetUserLogins] CHECK CONSTRAINT [FK_AspNetUserLogins_AspNetUsers_UserId]
GO
ALTER TABLE [dbo].[AspNetUserRoles]  WITH CHECK ADD  CONSTRAINT [FK_AspNetUserRoles_AspNetRoles_RoleId] FOREIGN KEY([RoleId])
REFERENCES [dbo].[AspNetRoles] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AspNetUserRoles] CHECK CONSTRAINT [FK_AspNetUserRoles_AspNetRoles_RoleId]
GO
ALTER TABLE [dbo].[AspNetUserRoles]  WITH CHECK ADD  CONSTRAINT [FK_AspNetUserRoles_AspNetUsers_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[AspNetUsers] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AspNetUserRoles] CHECK CONSTRAINT [FK_AspNetUserRoles_AspNetUsers_UserId]
GO
ALTER TABLE [dbo].[AspNetUserTokens]  WITH CHECK ADD  CONSTRAINT [FK_AspNetUserTokens_AspNetUsers_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[AspNetUsers] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AspNetUserTokens] CHECK CONSTRAINT [FK_AspNetUserTokens_AspNetUsers_UserId]
GO
ALTER TABLE [dbo].[AssetDetails]  WITH CHECK ADD  CONSTRAINT [FK_AssetDetails_Buildings] FOREIGN KEY([BuildingId])
REFERENCES [dbo].[Buildings] ([Id])
GO
ALTER TABLE [dbo].[AssetDetails] CHECK CONSTRAINT [FK_AssetDetails_Buildings]
GO
ALTER TABLE [dbo].[AssetDetails]  WITH CHECK ADD  CONSTRAINT [FK_AssetDetails_Floors] FOREIGN KEY([FloorId])
REFERENCES [dbo].[Floors] ([Id])
GO
ALTER TABLE [dbo].[AssetDetails] CHECK CONSTRAINT [FK_AssetDetails_Floors]
GO
ALTER TABLE [dbo].[AssetDetails]  WITH CHECK ADD  CONSTRAINT [FK_AssetDetails_Rooms] FOREIGN KEY([RoomId])
REFERENCES [dbo].[Rooms] ([Id])
GO
ALTER TABLE [dbo].[AssetDetails] CHECK CONSTRAINT [FK_AssetDetails_Rooms]
GO
ALTER TABLE [dbo].[AssetMovements]  WITH CHECK ADD  CONSTRAINT [FK_AssetMovements_Buildings] FOREIGN KEY([BuildingId])
REFERENCES [dbo].[Buildings] ([Id])
GO
ALTER TABLE [dbo].[AssetMovements] CHECK CONSTRAINT [FK_AssetMovements_Buildings]
GO
ALTER TABLE [dbo].[AssetMovements]  WITH CHECK ADD  CONSTRAINT [FK_AssetMovements_Floors] FOREIGN KEY([FloorId])
REFERENCES [dbo].[Floors] ([Id])
GO
ALTER TABLE [dbo].[AssetMovements] CHECK CONSTRAINT [FK_AssetMovements_Floors]
GO
ALTER TABLE [dbo].[AssetMovements]  WITH CHECK ADD  CONSTRAINT [FK_AssetMovements_Rooms] FOREIGN KEY([RoomId])
REFERENCES [dbo].[Rooms] ([Id])
GO
ALTER TABLE [dbo].[AssetMovements] CHECK CONSTRAINT [FK_AssetMovements_Rooms]
GO
ALTER TABLE [dbo].[AssetStatusTransactions]  WITH CHECK ADD  CONSTRAINT [FK_AssetStatusTransactions_AssetStatus] FOREIGN KEY([AssetStatusId])
REFERENCES [dbo].[AssetStatus] ([Id])
GO
ALTER TABLE [dbo].[AssetStatusTransactions] CHECK CONSTRAINT [FK_AssetStatusTransactions_AssetStatus]
GO
ALTER TABLE [dbo].[Cities]  WITH CHECK ADD  CONSTRAINT [FK_Cities_Governorates] FOREIGN KEY([GovernorateId])
REFERENCES [dbo].[Governorates] ([Id])
GO
ALTER TABLE [dbo].[Cities] CHECK CONSTRAINT [FK_Cities_Governorates]
GO
ALTER TABLE [dbo].[ContractAttachments]  WITH CHECK ADD  CONSTRAINT [FK_ContractAttachments_MasterContracts] FOREIGN KEY([MasterContractId])
REFERENCES [dbo].[MasterContracts] ([Id])
GO
ALTER TABLE [dbo].[ContractAttachments] CHECK CONSTRAINT [FK_ContractAttachments_MasterContracts]
GO
ALTER TABLE [dbo].[ContractDetails]  WITH CHECK ADD  CONSTRAINT [FK_ContractDetails_MasterContracts] FOREIGN KEY([MasterContractId])
REFERENCES [dbo].[MasterContracts] ([Id])
GO
ALTER TABLE [dbo].[ContractDetails] CHECK CONSTRAINT [FK_ContractDetails_MasterContracts]
GO
ALTER TABLE [dbo].[Floors]  WITH CHECK ADD  CONSTRAINT [FK_Floors_Buildings] FOREIGN KEY([BuildingId])
REFERENCES [dbo].[Buildings] ([Id])
GO
ALTER TABLE [dbo].[Floors] CHECK CONSTRAINT [FK_Floors_Buildings]
GO
ALTER TABLE [dbo].[HospitalApplicationAttachments]  WITH CHECK ADD  CONSTRAINT [FK_HospitalApplicationAttachments_HospitalReasonTransactions] FOREIGN KEY([HospitalReasonTransactionId])
REFERENCES [dbo].[HospitalReasonTransactions] ([Id])
GO
ALTER TABLE [dbo].[HospitalApplicationAttachments] CHECK CONSTRAINT [FK_HospitalApplicationAttachments_HospitalReasonTransactions]
GO
ALTER TABLE [dbo].[HospitalApplications]  WITH CHECK ADD  CONSTRAINT [FK_HospitalApplications_ApplicationTypes] FOREIGN KEY([AppTypeId])
REFERENCES [dbo].[ApplicationTypes] ([Id])
GO
ALTER TABLE [dbo].[HospitalApplications] CHECK CONSTRAINT [FK_HospitalApplications_ApplicationTypes]
GO
ALTER TABLE [dbo].[HospitalApplications]  WITH CHECK ADD  CONSTRAINT [FK_HospitalApplications_HospitalSupplierStatuses] FOREIGN KEY([StatusId])
REFERENCES [dbo].[HospitalSupplierStatuses] ([Id])
GO
ALTER TABLE [dbo].[HospitalApplications] CHECK CONSTRAINT [FK_HospitalApplications_HospitalSupplierStatuses]
GO
ALTER TABLE [dbo].[HospitalApplications]  WITH CHECK ADD  CONSTRAINT [FK_HospitalExecludeAssets_AspNetUsers] FOREIGN KEY([UserId])
REFERENCES [dbo].[AspNetUsers] ([Id])
GO
ALTER TABLE [dbo].[HospitalApplications] CHECK CONSTRAINT [FK_HospitalExecludeAssets_AspNetUsers]
GO
ALTER TABLE [dbo].[HospitalDepartments]  WITH CHECK ADD  CONSTRAINT [FK_HospitalDepartments_Departments] FOREIGN KEY([DepartmentId])
REFERENCES [dbo].[Departments] ([Id])
GO
ALTER TABLE [dbo].[HospitalDepartments] CHECK CONSTRAINT [FK_HospitalDepartments_Departments]
GO
ALTER TABLE [dbo].[HospitalEngineers]  WITH CHECK ADD  CONSTRAINT [FK_HospitalEngineers_Hospitals] FOREIGN KEY([HospId])
REFERENCES [dbo].[Hospitals] ([Id])
GO
ALTER TABLE [dbo].[HospitalEngineers] CHECK CONSTRAINT [FK_HospitalEngineers_Hospitals]
GO
ALTER TABLE [dbo].[HospitalReasonTransactions]  WITH CHECK ADD  CONSTRAINT [FK_HospitalReasonTrasactions_HospitalApplications] FOREIGN KEY([HospitalApplicationId])
REFERENCES [dbo].[HospitalApplications] ([Id])
GO
ALTER TABLE [dbo].[HospitalReasonTransactions] CHECK CONSTRAINT [FK_HospitalReasonTrasactions_HospitalApplications]
GO
ALTER TABLE [dbo].[Hospitals]  WITH CHECK ADD  CONSTRAINT [FK_Hospitals_Cities] FOREIGN KEY([CityId])
REFERENCES [dbo].[Cities] ([Id])
GO
ALTER TABLE [dbo].[Hospitals] CHECK CONSTRAINT [FK_Hospitals_Cities]
GO
ALTER TABLE [dbo].[Hospitals]  WITH CHECK ADD  CONSTRAINT [FK_Hospitals_Governorates] FOREIGN KEY([GovernorateId])
REFERENCES [dbo].[Governorates] ([Id])
GO
ALTER TABLE [dbo].[Hospitals] CHECK CONSTRAINT [FK_Hospitals_Governorates]
GO
ALTER TABLE [dbo].[Hospitals]  WITH CHECK ADD  CONSTRAINT [FK_Hospitals_Organizations] FOREIGN KEY([OrganizationId])
REFERENCES [dbo].[Organizations] ([Id])
GO
ALTER TABLE [dbo].[Hospitals] CHECK CONSTRAINT [FK_Hospitals_Organizations]
GO
ALTER TABLE [dbo].[Hospitals]  WITH CHECK ADD  CONSTRAINT [FK_Hospitals_SubOrganizations] FOREIGN KEY([SubOrganizationId])
REFERENCES [dbo].[SubOrganizations] ([Id])
GO
ALTER TABLE [dbo].[Hospitals] CHECK CONSTRAINT [FK_Hospitals_SubOrganizations]
GO
ALTER TABLE [dbo].[MasterAssets]  WITH CHECK ADD  CONSTRAINT [FK_MasterAssets_ECRIS] FOREIGN KEY([ECRIId])
REFERENCES [dbo].[ECRIS] ([Id])
GO
ALTER TABLE [dbo].[MasterAssets] CHECK CONSTRAINT [FK_MasterAssets_ECRIS]
GO
ALTER TABLE [dbo].[MasterAssets]  WITH CHECK ADD  CONSTRAINT [FK_MasterAssets_Origins] FOREIGN KEY([OriginId])
REFERENCES [dbo].[Origins] ([Id])
GO
ALTER TABLE [dbo].[MasterAssets] CHECK CONSTRAINT [FK_MasterAssets_Origins]
GO
ALTER TABLE [dbo].[MasterAssets]  WITH CHECK ADD  CONSTRAINT [FK_MasterAssets_SubCategories] FOREIGN KEY([SubCategoryId])
REFERENCES [dbo].[SubCategories] ([Id])
GO
ALTER TABLE [dbo].[MasterAssets] CHECK CONSTRAINT [FK_MasterAssets_SubCategories]
GO
ALTER TABLE [dbo].[PMAssetTaskSchedules]  WITH CHECK ADD  CONSTRAINT [FK_PMAssetTaskSchedules_PMAssetTasks] FOREIGN KEY([PMAssetTaskId])
REFERENCES [dbo].[PMAssetTasks] ([Id])
GO
ALTER TABLE [dbo].[PMAssetTaskSchedules] CHECK CONSTRAINT [FK_PMAssetTaskSchedules_PMAssetTasks]
GO
ALTER TABLE [dbo].[PMAssetTaskSchedules]  WITH CHECK ADD  CONSTRAINT [FK_PMAssetTaskSchedules_PMAssetTimes] FOREIGN KEY([PMAssetTimeId])
REFERENCES [dbo].[PMAssetTimes] ([Id])
GO
ALTER TABLE [dbo].[PMAssetTaskSchedules] CHECK CONSTRAINT [FK_PMAssetTaskSchedules_PMAssetTimes]
GO
ALTER TABLE [dbo].[Request]  WITH CHECK ADD  CONSTRAINT [FK_Request_AspNetUsers_CreatedById] FOREIGN KEY([CreatedById])
REFERENCES [dbo].[AspNetUsers] ([Id])
GO
ALTER TABLE [dbo].[Request] CHECK CONSTRAINT [FK_Request_AspNetUsers_CreatedById]
GO
ALTER TABLE [dbo].[Request]  WITH CHECK ADD  CONSTRAINT [FK_Request_RequestMode_RequestModeId] FOREIGN KEY([RequestModeId])
REFERENCES [dbo].[RequestMode] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Request] CHECK CONSTRAINT [FK_Request_RequestMode_RequestModeId]
GO
ALTER TABLE [dbo].[Request]  WITH CHECK ADD  CONSTRAINT [FK_Request_RequestPeriority_RequestPeriorityId] FOREIGN KEY([RequestPeriorityId])
REFERENCES [dbo].[RequestPeriority] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Request] CHECK CONSTRAINT [FK_Request_RequestPeriority_RequestPeriorityId]
GO
ALTER TABLE [dbo].[Request]  WITH CHECK ADD  CONSTRAINT [FK_Request_RequestTypes_RequestTypeId] FOREIGN KEY([RequestTypeId])
REFERENCES [dbo].[RequestTypes] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Request] CHECK CONSTRAINT [FK_Request_RequestTypes_RequestTypeId]
GO
ALTER TABLE [dbo].[Request]  WITH CHECK ADD  CONSTRAINT [FK_Request_SubProblems_SubProblemId] FOREIGN KEY([SubProblemId])
REFERENCES [dbo].[SubProblems] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Request] CHECK CONSTRAINT [FK_Request_SubProblems_SubProblemId]
GO
ALTER TABLE [dbo].[RequestDocument]  WITH CHECK ADD  CONSTRAINT [FK_RequestDocument_RequestTracking_RequestTrackingId] FOREIGN KEY([RequestTrackingId])
REFERENCES [dbo].[RequestTracking] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[RequestDocument] CHECK CONSTRAINT [FK_RequestDocument_RequestTracking_RequestTrackingId]
GO
ALTER TABLE [dbo].[RequestPhase]  WITH CHECK ADD  CONSTRAINT [FK_RequestPhase_Employees_EmployeeId] FOREIGN KEY([EmployeeId])
REFERENCES [dbo].[Employees] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[RequestPhase] CHECK CONSTRAINT [FK_RequestPhase_Employees_EmployeeId]
GO
ALTER TABLE [dbo].[RequestPhase]  WITH CHECK ADD  CONSTRAINT [FK_RequestPhase_Phase_PhaseId] FOREIGN KEY([PhaseId])
REFERENCES [dbo].[Phase] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[RequestPhase] CHECK CONSTRAINT [FK_RequestPhase_Phase_PhaseId]
GO
ALTER TABLE [dbo].[RequestPhase]  WITH CHECK ADD  CONSTRAINT [FK_RequestPhase_Request_RequestId] FOREIGN KEY([RequestId])
REFERENCES [dbo].[Request] ([Id])
GO
ALTER TABLE [dbo].[RequestPhase] CHECK CONSTRAINT [FK_RequestPhase_Request_RequestId]
GO
ALTER TABLE [dbo].[RequestTracking]  WITH CHECK ADD  CONSTRAINT [FK_RequestTracking_AspNetUsers_CreatedById] FOREIGN KEY([CreatedById])
REFERENCES [dbo].[AspNetUsers] ([Id])
GO
ALTER TABLE [dbo].[RequestTracking] CHECK CONSTRAINT [FK_RequestTracking_AspNetUsers_CreatedById]
GO
ALTER TABLE [dbo].[RequestTracking]  WITH CHECK ADD  CONSTRAINT [FK_RequestTracking_Request_RequestId] FOREIGN KEY([RequestId])
REFERENCES [dbo].[Request] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[RequestTracking] CHECK CONSTRAINT [FK_RequestTracking_Request_RequestId]
GO
ALTER TABLE [dbo].[RequestTracking]  WITH CHECK ADD  CONSTRAINT [FK_RequestTracking_RequestStatus_RequestStatusId] FOREIGN KEY([RequestStatusId])
REFERENCES [dbo].[RequestStatus] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[RequestTracking] CHECK CONSTRAINT [FK_RequestTracking_RequestStatus_RequestStatusId]
GO
ALTER TABLE [dbo].[Rooms]  WITH CHECK ADD  CONSTRAINT [FK_Rooms_Floors] FOREIGN KEY([FloorId])
REFERENCES [dbo].[Floors] ([Id])
GO
ALTER TABLE [dbo].[Rooms] CHECK CONSTRAINT [FK_Rooms_Floors]
GO
ALTER TABLE [dbo].[SubProblems]  WITH CHECK ADD  CONSTRAINT [FK_SubProblems_Problems_ProblemId] FOREIGN KEY([ProblemId])
REFERENCES [dbo].[Problems] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[SubProblems] CHECK CONSTRAINT [FK_SubProblems_Problems_ProblemId]
GO
ALTER TABLE [dbo].[SupplierExecludeAssets]  WITH CHECK ADD  CONSTRAINT [FK_SupplierExecludeAssets_ApplicationTypes] FOREIGN KEY([AppTypeId])
REFERENCES [dbo].[ApplicationTypes] ([Id])
GO
ALTER TABLE [dbo].[SupplierExecludeAssets] CHECK CONSTRAINT [FK_SupplierExecludeAssets_ApplicationTypes]
GO
ALTER TABLE [dbo].[SupplierExecludeAssets]  WITH CHECK ADD  CONSTRAINT [FK_SupplierExecludeAssets_AspNetUsers] FOREIGN KEY([UserId])
REFERENCES [dbo].[AspNetUsers] ([Id])
GO
ALTER TABLE [dbo].[SupplierExecludeAssets] CHECK CONSTRAINT [FK_SupplierExecludeAssets_AspNetUsers]
GO
ALTER TABLE [dbo].[SupplierExecludeAssets]  WITH CHECK ADD  CONSTRAINT [FK_SupplierExecludeAssets_HospitalSupplierStatuses] FOREIGN KEY([StatusId])
REFERENCES [dbo].[HospitalSupplierStatuses] ([Id])
GO
ALTER TABLE [dbo].[SupplierExecludeAssets] CHECK CONSTRAINT [FK_SupplierExecludeAssets_HospitalSupplierStatuses]
GO
ALTER TABLE [dbo].[SupplierExecludes]  WITH CHECK ADD  CONSTRAINT [FK_SupplierExecludes_SupplierExecludeAssets] FOREIGN KEY([SupplierExecludeAssetId])
REFERENCES [dbo].[SupplierExecludeAssets] ([Id])
GO
ALTER TABLE [dbo].[SupplierExecludes] CHECK CONSTRAINT [FK_SupplierExecludes_SupplierExecludeAssets]
GO
ALTER TABLE [dbo].[SupplierExecludes]  WITH CHECK ADD  CONSTRAINT [FK_SupplierExecludes_SupplierExecludeReasons] FOREIGN KEY([ReasonId])
REFERENCES [dbo].[SupplierExecludeReasons] ([Id])
GO
ALTER TABLE [dbo].[SupplierExecludes] CHECK CONSTRAINT [FK_SupplierExecludes_SupplierExecludeReasons]
GO
ALTER TABLE [dbo].[VisitAttachments]  WITH CHECK ADD  CONSTRAINT [FK_VisitAttachments_Visits] FOREIGN KEY([VisitId])
REFERENCES [dbo].[Visits] ([Id])
GO
ALTER TABLE [dbo].[VisitAttachments] CHECK CONSTRAINT [FK_VisitAttachments_Visits]
GO
ALTER TABLE [dbo].[Visits]  WITH CHECK ADD  CONSTRAINT [FK_Visits_Hospitals] FOREIGN KEY([HospitalId])
REFERENCES [dbo].[Hospitals] ([Id])
GO
ALTER TABLE [dbo].[Visits] CHECK CONSTRAINT [FK_Visits_Hospitals]
GO
ALTER TABLE [dbo].[Visits]  WITH CHECK ADD  CONSTRAINT [FK_Visits_VisitTypes] FOREIGN KEY([VisitTypeId])
REFERENCES [dbo].[VisitTypes] ([Id])
GO
ALTER TABLE [dbo].[Visits] CHECK CONSTRAINT [FK_Visits_VisitTypes]
GO
ALTER TABLE [dbo].[WorkOrderAssigns]  WITH CHECK ADD  CONSTRAINT [FK_WorkOrderAssigns_AspNetUsers] FOREIGN KEY([UserId])
REFERENCES [dbo].[AspNetUsers] ([Id])
GO
ALTER TABLE [dbo].[WorkOrderAssigns] CHECK CONSTRAINT [FK_WorkOrderAssigns_AspNetUsers]
GO
ALTER TABLE [dbo].[WorkOrderAttachments]  WITH CHECK ADD  CONSTRAINT [FK_WorkOrderAttachments_WorkOrderTrackings_WorkOrderTrackingId] FOREIGN KEY([WorkOrderTrackingId])
REFERENCES [dbo].[WorkOrderTrackings] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[WorkOrderAttachments] CHECK CONSTRAINT [FK_WorkOrderAttachments_WorkOrderTrackings_WorkOrderTrackingId]
GO
ALTER TABLE [dbo].[WorkOrders]  WITH CHECK ADD  CONSTRAINT [FK_WorkOrders_AspNetUsers_CreatedById] FOREIGN KEY([CreatedById])
REFERENCES [dbo].[AspNetUsers] ([Id])
GO
ALTER TABLE [dbo].[WorkOrders] CHECK CONSTRAINT [FK_WorkOrders_AspNetUsers_CreatedById]
GO
ALTER TABLE [dbo].[WorkOrders]  WITH CHECK ADD  CONSTRAINT [FK_WorkOrders_Request_RequestId] FOREIGN KEY([RequestId])
REFERENCES [dbo].[Request] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[WorkOrders] CHECK CONSTRAINT [FK_WorkOrders_Request_RequestId]
GO
ALTER TABLE [dbo].[WorkOrders]  WITH CHECK ADD  CONSTRAINT [FK_WorkOrders_WorkOrderPeriorities_WorkOrderPeriorityId] FOREIGN KEY([WorkOrderPeriorityId])
REFERENCES [dbo].[WorkOrderPeriorities] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[WorkOrders] CHECK CONSTRAINT [FK_WorkOrders_WorkOrderPeriorities_WorkOrderPeriorityId]
GO
ALTER TABLE [dbo].[WorkOrders]  WITH CHECK ADD  CONSTRAINT [FK_WorkOrders_WorkOrderTypes_WorkOrderTypeId] FOREIGN KEY([WorkOrderTypeId])
REFERENCES [dbo].[WorkOrderTypes] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[WorkOrders] CHECK CONSTRAINT [FK_WorkOrders_WorkOrderTypes_WorkOrderTypeId]
GO
ALTER TABLE [dbo].[WorkOrderTasks]  WITH CHECK ADD  CONSTRAINT [FK_WorkOrderTasks_AssetWorkOrderTasks_AssetWorkOrderTaskId] FOREIGN KEY([AssetWorkOrderTaskId])
REFERENCES [dbo].[AssetWorkOrderTasks] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[WorkOrderTasks] CHECK CONSTRAINT [FK_WorkOrderTasks_AssetWorkOrderTasks_AssetWorkOrderTaskId]
GO
ALTER TABLE [dbo].[WorkOrderTasks]  WITH CHECK ADD  CONSTRAINT [FK_WorkOrderTasks_WorkOrders_WorkOrderId] FOREIGN KEY([WorkOrderId])
REFERENCES [dbo].[WorkOrders] ([Id])
GO
ALTER TABLE [dbo].[WorkOrderTasks] CHECK CONSTRAINT [FK_WorkOrderTasks_WorkOrders_WorkOrderId]
GO
ALTER TABLE [dbo].[WorkOrderTrackings]  WITH CHECK ADD  CONSTRAINT [FK_WorkOrderTrackings_AspNetUsers_CreatedById] FOREIGN KEY([CreatedById])
REFERENCES [dbo].[AspNetUsers] ([Id])
GO
ALTER TABLE [dbo].[WorkOrderTrackings] CHECK CONSTRAINT [FK_WorkOrderTrackings_AspNetUsers_CreatedById]
GO
ALTER TABLE [dbo].[WorkOrderTrackings]  WITH CHECK ADD  CONSTRAINT [FK_WorkOrderTrackings_WorkOrders_WorkOrderId] FOREIGN KEY([WorkOrderId])
REFERENCES [dbo].[WorkOrders] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[WorkOrderTrackings] CHECK CONSTRAINT [FK_WorkOrderTrackings_WorkOrders_WorkOrderId]
GO
ALTER TABLE [dbo].[WorkOrderTrackings]  WITH CHECK ADD  CONSTRAINT [FK_WorkOrderTrackings_WorkOrderStatuses_WorkOrderStatusId] FOREIGN KEY([WorkOrderStatusId])
REFERENCES [dbo].[WorkOrderStatuses] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[WorkOrderTrackings] CHECK CONSTRAINT [FK_WorkOrderTrackings_WorkOrderStatuses_WorkOrderStatusId]
GO
USE [master]
GO
ALTER DATABASE [NewAssetDB] SET  READ_WRITE 
GO
