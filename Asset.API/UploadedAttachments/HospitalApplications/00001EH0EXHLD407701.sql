USE [master]
GO
/****** Object:  Database [BiomedicalDB]    Script Date: 8/2/2021 4:44:08 PM ******/
CREATE DATABASE [BiomedicalDB]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'BiomedicalDB', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL14.MSSQLSERVER\MSSQL\DATA\BiomedicalDB.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'BiomedicalDB_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL14.MSSQLSERVER\MSSQL\DATA\BiomedicalDB_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
GO
ALTER DATABASE [BiomedicalDB] SET COMPATIBILITY_LEVEL = 140
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [BiomedicalDB].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [BiomedicalDB] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [BiomedicalDB] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [BiomedicalDB] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [BiomedicalDB] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [BiomedicalDB] SET ARITHABORT OFF 
GO
ALTER DATABASE [BiomedicalDB] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [BiomedicalDB] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [BiomedicalDB] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [BiomedicalDB] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [BiomedicalDB] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [BiomedicalDB] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [BiomedicalDB] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [BiomedicalDB] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [BiomedicalDB] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [BiomedicalDB] SET  ENABLE_BROKER 
GO
ALTER DATABASE [BiomedicalDB] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [BiomedicalDB] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [BiomedicalDB] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [BiomedicalDB] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [BiomedicalDB] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [BiomedicalDB] SET READ_COMMITTED_SNAPSHOT ON 
GO
ALTER DATABASE [BiomedicalDB] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [BiomedicalDB] SET RECOVERY FULL 
GO
ALTER DATABASE [BiomedicalDB] SET  MULTI_USER 
GO
ALTER DATABASE [BiomedicalDB] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [BiomedicalDB] SET DB_CHAINING OFF 
GO
ALTER DATABASE [BiomedicalDB] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [BiomedicalDB] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [BiomedicalDB] SET DELAYED_DURABILITY = DISABLED 
GO
EXEC sys.sp_db_vardecimal_storage_format N'BiomedicalDB', N'ON'
GO
ALTER DATABASE [BiomedicalDB] SET QUERY_STORE = OFF
GO
USE [BiomedicalDB]
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
USE [BiomedicalDB]
GO
/****** Object:  Table [dbo].[__EFMigrationsHistory]    Script Date: 8/2/2021 4:44:08 PM ******/
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
/****** Object:  Table [dbo].[AspNetRoleClaims]    Script Date: 8/2/2021 4:44:08 PM ******/
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
/****** Object:  Table [dbo].[AspNetRoles]    Script Date: 8/2/2021 4:44:08 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AspNetRoles](
	[Id] [nvarchar](450) NOT NULL,
	[Name] [nvarchar](256) NULL,
	[NormalizedName] [nvarchar](256) NULL,
	[ConcurrencyStamp] [nvarchar](max) NULL,
 CONSTRAINT [PK_AspNetRoles] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AspNetUserClaims]    Script Date: 8/2/2021 4:44:08 PM ******/
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
/****** Object:  Table [dbo].[AspNetUserLogins]    Script Date: 8/2/2021 4:44:08 PM ******/
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
/****** Object:  Table [dbo].[AspNetUserRoles]    Script Date: 8/2/2021 4:44:08 PM ******/
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
/****** Object:  Table [dbo].[AspNetUsers]    Script Date: 8/2/2021 4:44:08 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AspNetUsers](
	[Id] [nvarchar](450) NOT NULL,
	[NameAr] [nvarchar](max) NULL,
	[Mobile] [nvarchar](max) NULL,
	[Phone] [nvarchar](max) NULL,
	[Code] [nvarchar](max) NULL,
	[HealthCareUnitId] [int] NULL,
	[HealthdirId] [int] NULL,
	[HealthDistrictId] [int] NULL,
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
	[OrganizationId] [int] NULL,
 CONSTRAINT [PK_AspNetUsers] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AspNetUserTokens]    Script Date: 8/2/2021 4:44:08 PM ******/
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
/****** Object:  Table [dbo].[Attachments]    Script Date: 8/2/2021 4:44:08 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Attachments](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[FileName] [nvarchar](max) NULL,
 CONSTRAINT [PK_Attachments] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Contracts]    Script Date: 8/2/2021 4:44:09 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Contracts](
	[ContractId] [int] IDENTITY(1,1) NOT NULL,
	[Subject] [nvarchar](max) NULL,
	[ContractCode] [nvarchar](max) NULL,
	[StartDate] [datetime2](7) NULL,
	[EndDate] [datetime2](7) NULL,
	[HealthCareUnitId] [int] NOT NULL,
	[ManufacturerId] [int] NULL,
	[SupplierId] [int] NULL,
 CONSTRAINT [PK_Contracts] PRIMARY KEY CLUSTERED 
(
	[ContractId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Departments]    Script Date: 8/2/2021 4:44:09 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Departments](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[DepartmentName] [nvarchar](max) NULL,
	[DepartmentNameAr] [nvarchar](max) NULL,
	[DepartmentCode] [nvarchar](max) NULL,
 CONSTRAINT [PK_Departments] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[DepreciationTypes]    Script Date: 8/2/2021 4:44:09 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DepreciationTypes](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Type] [nvarchar](max) NULL,
 CONSTRAINT [PK_DepreciationTypes] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Ecris]    Script Date: 8/2/2021 4:44:09 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Ecris](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[ecri_code] [nvarchar](max) NULL,
 CONSTRAINT [PK_Ecris] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Employee]    Script Date: 8/2/2021 4:44:09 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Employee](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[EmployeeName] [nvarchar](max) NULL,
	[EmployeeNameAr] [nvarchar](max) NULL,
	[Email] [nvarchar](max) NULL,
	[Mobile] [nvarchar](max) NULL,
	[Phone] [nvarchar](max) NULL,
	[EmployeeCode] [nvarchar](max) NULL,
	[HealthCareUnitId] [int] NOT NULL,
	[HealthDirectoryId] [int] NOT NULL,
	[HealthDistrictId] [int] NOT NULL,
 CONSTRAINT [PK_Employee] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Equiments]    Script Date: 8/2/2021 4:44:09 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Equiments](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[EquipmentCode] [nvarchar](max) NULL,
	[EquipmentMasterCode] [nvarchar](max) NULL,
	[EquipmentName] [nvarchar](max) NULL,
	[EquipmentNameAr] [nvarchar](max) NULL,
	[EquipmentType] [nvarchar](max) NULL,
	[InstallationDate] [datetime2](7) NOT NULL,
	[HealthCareUnitId] [int] NOT NULL,
	[Remarks] [nvarchar](max) NULL,
	[SerialNumber] [nvarchar](max) NULL,
	[InternalCode] [nvarchar](max) NULL,
	[Barcode] [nvarchar](max) NULL,
	[PurchaseDate] [datetime2](7) NOT NULL,
	[SupplierId] [int] NOT NULL,
	[EquipmentStatuSId] [int] NOT NULL,
	[Price] [decimal](18, 2) NOT NULL,
	[WarrantyExpires] [datetime2](7) NOT NULL,
	[Length] [real] NOT NULL,
	[Height] [real] NOT NULL,
	[Weight] [real] NOT NULL,
	[Color] [nvarchar](max) NULL,
	[ColorAr] [nvarchar](max) NULL,
	[CustomizedField] [nvarchar](max) NULL,
	[DepartmentId] [int] NOT NULL,
	[Room] [int] NOT NULL,
	[Floor] [int] NOT NULL,
	[ProductionYear] [datetime2](7) NOT NULL,
	[HealthDirectoryId] [int] NOT NULL,
	[MasterEquipmentId] [int] NOT NULL,
	[HealthDistrictId] [int] NOT NULL,
 CONSTRAINT [PK_Equiments] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Equipment_EquipmentCoverage]    Script Date: 8/2/2021 4:44:09 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Equipment_EquipmentCoverage](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[EquipmentId] [int] NOT NULL,
	[EquipmentCoverageId] [int] NOT NULL,
 CONSTRAINT [PK_Equipment_EquipmentCoverage] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[EquipmentAttachments]    Script Date: 8/2/2021 4:44:09 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[EquipmentAttachments](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[FileName] [nvarchar](max) NULL,
	[EquipmentId] [int] NULL,
 CONSTRAINT [PK_EquipmentAttachments] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[EquipmentCategories]    Script Date: 8/2/2021 4:44:09 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[EquipmentCategories](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[CategoryCode] [nvarchar](max) NULL,
	[CategoryName] [nvarchar](max) NULL,
	[CategoryNameAr] [nvarchar](max) NULL,
	[HealthCareUnitId] [int] NOT NULL,
	[CategoryDescription] [nvarchar](max) NULL,
	[CategoryDescriptionAr] [nvarchar](max) NULL,
 CONSTRAINT [PK_EquipmentCategories] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[EquipmentCoverages]    Script Date: 8/2/2021 4:44:09 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[EquipmentCoverages](
	[equipmentCoverageId] [int] IDENTITY(1,1) NOT NULL,
	[ResponseTime] [real] NOT NULL,
	[Description] [nvarchar](max) NULL,
	[DescriptionAr] [nvarchar](max) NULL,
	[NumberOfVisits] [int] NOT NULL,
	[SparePartId] [int] NULL,
	[ContractId] [int] NOT NULL,
 CONSTRAINT [PK_EquipmentCoverages] PRIMARY KEY CLUSTERED 
(
	[equipmentCoverageId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[equipmentEmployees]    Script Date: 8/2/2021 4:44:09 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[equipmentEmployees](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[EquipmentId] [int] NOT NULL,
	[UserId] [nvarchar](450) NULL,
 CONSTRAINT [PK_equipmentEmployees] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[EquipmentStatus]    Script Date: 8/2/2021 4:44:09 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[EquipmentStatus](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Status] [nvarchar](max) NULL,
	[StatusAr] [nvarchar](max) NULL,
 CONSTRAINT [PK_EquipmentStatus] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[EquipmentSubCategories]    Script Date: 8/2/2021 4:44:09 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[EquipmentSubCategories](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[SubCategoryName] [nvarchar](max) NULL,
	[SubCategoryNameAr] [nvarchar](max) NULL,
	[EquipmentCategoryId] [int] NOT NULL,
 CONSTRAINT [PK_EquipmentSubCategories] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[HealthCareUnits]    Script Date: 8/2/2021 4:44:09 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[HealthCareUnits](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[HealthCareUnitCode] [nvarchar](max) NULL,
	[HealthCareUnitName] [nvarchar](max) NULL,
	[HealthCareUnitNameAr] [nvarchar](max) NULL,
	[Address] [nvarchar](max) NULL,
	[Director] [nvarchar](max) NULL,
	[Phone] [nvarchar](max) NULL,
	[Mobile] [nvarchar](max) NULL,
	[Email] [nvarchar](max) NULL,
	[Long] [float] NULL,
	[Lat] [float] NULL,
	[HealthDistrictId] [int] NOT NULL,
	[HealthDirectoryId] [int] NOT NULL,
	[organizationId] [int] NOT NULL,
 CONSTRAINT [PK_HealthCareUnits] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[HealthDirectories]    Script Date: 8/2/2021 4:44:09 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[HealthDirectories](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[HealthDirectoryCode] [nvarchar](max) NULL,
	[HealthDirectoryName] [nvarchar](max) NULL,
	[HealthDirectoryNameAr] [nvarchar](max) NULL,
 CONSTRAINT [PK_HealthDirectories] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[HealthDistricts]    Script Date: 8/2/2021 4:44:09 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[HealthDistricts](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[HealthDirectoryId] [int] NOT NULL,
	[HealthDistrictCode] [nvarchar](max) NULL,
	[HealthDistrictName] [nvarchar](max) NULL,
	[HealthDistrictNameAr] [nvarchar](max) NULL,
 CONSTRAINT [PK_HealthDistricts] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Languages]    Script Date: 8/2/2021 4:44:09 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Languages](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](max) NULL,
 CONSTRAINT [PK_Languages] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[MaintenanceServices]    Script Date: 8/2/2021 4:44:09 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[MaintenanceServices](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[problem] [nvarchar](max) NULL,
	[PartCost] [float] NOT NULL,
	[LaborCost] [float] NOT NULL,
 CONSTRAINT [PK_MaintenanceServices] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Manufacturers]    Script Date: 8/2/2021 4:44:09 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Manufacturers](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[ManufacturerName] [nvarchar](max) NULL,
	[ManufacturerNameAr] [nvarchar](max) NULL,
 CONSTRAINT [PK_Manufacturers] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[masterEquipmentAttachments]    Script Date: 8/2/2021 4:44:09 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[masterEquipmentAttachments](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[FileName] [nvarchar](max) NULL,
	[MasterEquipmentId] [int] NOT NULL,
 CONSTRAINT [PK_masterEquipmentAttachments] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[masterEquipments]    Script Date: 8/2/2021 4:44:09 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[masterEquipments](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](max) NULL,
	[NameAr] [nvarchar](max) NULL,
	[MasterCode] [nvarchar](max) NULL,
	[EquipmentCategoryId] [int] NOT NULL,
	[EquipmentSubCategoryId] [int] NULL,
	[ManufacturerId] [int] NOT NULL,
	[PriorityId] [int] NOT NULL,
	[EquipmentDescription] [nvarchar](max) NULL,
	[EquipmentDescriptionAr] [nvarchar](max) NULL,
	[ExpectedLifeTime] [int] NOT NULL,
	[UpaCode] [nvarchar](max) NULL,
	[ModelNumber] [nvarchar](max) NULL,
	[VersionNumber] [nvarchar](max) NULL,
	[OriginId] [int] NOT NULL,
 CONSTRAINT [PK_masterEquipments] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Modes]    Script Date: 8/2/2021 4:44:09 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Modes](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[RequestMode] [nvarchar](max) NULL,
	[RequestModeAr] [nvarchar](max) NULL,
 CONSTRAINT [PK_Modes] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[organizations]    Script Date: 8/2/2021 4:44:09 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[organizations](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](max) NULL,
	[NameAr] [nvarchar](max) NULL,
 CONSTRAINT [PK_organizations] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Origins]    Script Date: 8/2/2021 4:44:09 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Origins](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[OriginCode] [nvarchar](max) NULL,
	[ArabicName] [nvarchar](max) NULL,
	[EnglishName] [nvarchar](max) NULL,
 CONSTRAINT [PK_Origins] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Priority]    Script Date: 8/2/2021 4:44:09 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Priority](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[PriorityLevel] [nvarchar](max) NULL,
	[PriorityLevelAr] [nvarchar](max) NULL,
	[Description] [nvarchar](max) NULL,
	[DescriptionAr] [nvarchar](max) NULL,
 CONSTRAINT [PK_Priority] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[requestStatuses]    Script Date: 8/2/2021 4:44:09 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[requestStatuses](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[status] [nvarchar](max) NULL,
	[statusAr] [nvarchar](max) NULL,
	[color] [nvarchar](max) NULL,
 CONSTRAINT [PK_requestStatuses] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ServiceRequest]    Script Date: 8/2/2021 4:44:10 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ServiceRequest](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[ServiceRequestCode] [nvarchar](max) NULL,
	[ServiceRequestTitle] [nvarchar](max) NULL,
	[ServiceRequestTitleAr] [nvarchar](max) NULL,
	[ProblemDescription] [nvarchar](max) NULL,
	[ProblemDescriptionAr] [nvarchar](max) NULL,
	[ServiceRequestDate] [datetime2](7) NOT NULL,
	[ServiceRequestTime] [time](7) NOT NULL,
	[EmployeeId] [int] NOT NULL,
	[EmployeeEmail] [nvarchar](max) NULL,
	[EmployeeName] [nvarchar](max) NULL,
	[RequestType] [nvarchar](max) NULL,
	[RequestTypeAr] [nvarchar](max) NULL,
	[EquipmentId] [int] NOT NULL,
	[EquipmentCode] [nvarchar](max) NULL,
	[EquipmentName] [nvarchar](max) NULL,
	[ModeId] [int] NOT NULL,
	[RequestMode] [nvarchar](max) NULL,
	[PriorityId] [int] NOT NULL,
	[PriorityLevel] [nvarchar](max) NULL,
	[HealthDirectoryId] [int] NOT NULL,
	[HealthDistrictId] [int] NOT NULL,
 CONSTRAINT [PK_ServiceRequest] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[serviceRequestAttachments]    Script Date: 8/2/2021 4:44:10 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[serviceRequestAttachments](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[FileName] [nvarchar](max) NULL,
	[ServiceRequestId] [int] NOT NULL,
 CONSTRAINT [PK_serviceRequestAttachments] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[spareParts]    Script Date: 8/2/2021 4:44:10 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[spareParts](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[SparePartCode] [nvarchar](max) NULL,
	[SparePartName] [nvarchar](max) NULL,
	[SparePartNameAr] [nvarchar](max) NULL,
 CONSTRAINT [PK_spareParts] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Suppliers]    Script Date: 8/2/2021 4:44:10 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Suppliers](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[SupplierCode] [nvarchar](max) NULL,
	[SupplierName] [nvarchar](max) NULL,
	[SupplierNameAr] [nvarchar](max) NULL,
 CONSTRAINT [PK_Suppliers] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Vendors]    Script Date: 8/2/2021 4:44:10 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Vendors](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[VendorName] [nvarchar](max) NULL,
	[VendorNameAr] [nvarchar](max) NULL,
	[VendorCode] [nvarchar](max) NULL,
 CONSTRAINT [PK_Vendors] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[workOrderAttachments]    Script Date: 8/2/2021 4:44:10 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[workOrderAttachments](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[FileName] [nvarchar](max) NULL,
	[WorkOrderId] [int] NOT NULL,
 CONSTRAINT [PK_workOrderAttachments] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[WorkOrders]    Script Date: 8/2/2021 4:44:10 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[WorkOrders](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[WorkOrderDate] [datetime2](7) NOT NULL,
	[PriorityId] [int] NOT NULL,
	[EmployeeId] [int] NULL,
	[VendorId] [int] NULL,
	[RequestStatusId] [int] NOT NULL,
	[EquipmentId] [int] NOT NULL,
	[ServiceRequestId] [int] NULL,
	[MaintenanceId] [int] NOT NULL,
	[SparePartId] [int] NOT NULL,
 CONSTRAINT [PK_WorkOrders] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
INSERT [dbo].[__EFMigrationsHistory] ([MigrationId], [ProductVersion]) VALUES (N'20210802000158_EDITING-user', N'5.0.5')
INSERT [dbo].[__EFMigrationsHistory] ([MigrationId], [ProductVersion]) VALUES (N'20210802153342_user', N'5.0.5')
INSERT [dbo].[__EFMigrationsHistory] ([MigrationId], [ProductVersion]) VALUES (N'20210802154538_user-v2', N'5.0.5')
INSERT [dbo].[__EFMigrationsHistory] ([MigrationId], [ProductVersion]) VALUES (N'20210802155304_origin', N'5.0.5')
INSERT [dbo].[AspNetRoles] ([Id], [Name], [NormalizedName], [ConcurrencyStamp]) VALUES (N'10e77657-7ab7-4a15-8325-c12051d37248', N'Hospital', N'HOSPITAL', N'd7c47697-7eea-4e77-ad6c-9da4d0792051')
INSERT [dbo].[AspNetRoles] ([Id], [Name], [NormalizedName], [ConcurrencyStamp]) VALUES (N'3829c7e4-8657-45fa-b394-153c89221ab6', N'Directorate', N'DIRECTORATE', N'9bf6f9d9-7d04-4dfd-9f71-53d0e79169cb')
INSERT [dbo].[AspNetRoles] ([Id], [Name], [NormalizedName], [ConcurrencyStamp]) VALUES (N'4ba4bc35-f637-4083-bb7a-1bf06f93de43', N'DataEntry', N'DATAENTRY', N'532d1d6f-216b-4272-8662-1eb1d40c3ade')
INSERT [dbo].[AspNetRoles] ([Id], [Name], [NormalizedName], [ConcurrencyStamp]) VALUES (N'54e431cd-addc-46a3-87fc-306498503616', N'Technician', N'TECHNICIAN', N'1b90080e-09cd-4739-ae0f-15b25fa78ef7')
INSERT [dbo].[AspNetRoles] ([Id], [Name], [NormalizedName], [ConcurrencyStamp]) VALUES (N'8bf06a33-4851-4f1b-8602-de85eca25389', N'Minister', N'MINISTER', N'54a0d07b-da68-4916-a4f1-11390688590d')
INSERT [dbo].[AspNetRoles] ([Id], [Name], [NormalizedName], [ConcurrencyStamp]) VALUES (N'ac2e7d15-7b92-4263-861d-2014138c033c', N'Admin', N'ADMIN', N'd807c827-b012-4eaf-872d-66734af636eb')
INSERT [dbo].[AspNetRoles] ([Id], [Name], [NormalizedName], [ConcurrencyStamp]) VALUES (N'bfa1a789-353c-4a21-b184-7c8936ec3ca4', N'District', N'DISTRICT', N'996f88cb-57e9-4308-a872-eef914e9e817')
INSERT [dbo].[AspNetUserRoles] ([UserId], [RoleId]) VALUES (N'7252b92c-5f12-47a2-a20a-a50be1124505', N'10e77657-7ab7-4a15-8325-c12051d37248')
INSERT [dbo].[AspNetUserRoles] ([UserId], [RoleId]) VALUES (N'709984f7-8bab-4aad-bcd9-077812d1357a', N'3829c7e4-8657-45fa-b394-153c89221ab6')
INSERT [dbo].[AspNetUserRoles] ([UserId], [RoleId]) VALUES (N'f214ce7e-af25-47a0-95b8-20f29276201e', N'54e431cd-addc-46a3-87fc-306498503616')
INSERT [dbo].[AspNetUserRoles] ([UserId], [RoleId]) VALUES (N'615bb99a-cb15-4b4b-b59a-a681614e1aca', N'ac2e7d15-7b92-4263-861d-2014138c033c')
INSERT [dbo].[AspNetUserRoles] ([UserId], [RoleId]) VALUES (N'0566eb1f-94f6-4ce0-894a-c25d48bcd90d', N'bfa1a789-353c-4a21-b184-7c8936ec3ca4')
INSERT [dbo].[AspNetUserRoles] ([UserId], [RoleId]) VALUES (N'f8d65704-d05c-4039-97e5-fe92ceb8cfa6', N'bfa1a789-353c-4a21-b184-7c8936ec3ca4')
INSERT [dbo].[AspNetUsers] ([Id], [NameAr], [Mobile], [Phone], [Code], [HealthCareUnitId], [HealthdirId], [HealthDistrictId], [UserName], [NormalizedUserName], [Email], [NormalizedEmail], [EmailConfirmed], [PasswordHash], [SecurityStamp], [ConcurrencyStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEnd], [LockoutEnabled], [AccessFailedCount], [OrganizationId]) VALUES (N'0566eb1f-94f6-4ce0-894a-c25d48bcd90d', N'lll', N'12441', N'12441', N'444', NULL, 1, 1, N'mariam', N'MARIAM', N'mar@gmail.com', N'MAR@GMAIL.COM', 0, N'AQAAAAEAACcQAAAAENgOlcxyvPR5W6wgmH3y4xyVPD6PAmI+XjpZ4HRkVTmTnVnNndjudCQWsVlYGPD1cw==', N'3LP4XD2GOPEDW6S2QD3X5AR4ZRPGWIOO', N'5cd50a51-872c-434a-ba59-6005a6f97f51', NULL, 0, 0, NULL, 1, 0, NULL)
INSERT [dbo].[AspNetUsers] ([Id], [NameAr], [Mobile], [Phone], [Code], [HealthCareUnitId], [HealthdirId], [HealthDistrictId], [UserName], [NormalizedUserName], [Email], [NormalizedEmail], [EmailConfirmed], [PasswordHash], [SecurityStamp], [ConcurrencyStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEnd], [LockoutEnabled], [AccessFailedCount], [OrganizationId]) VALUES (N'615bb99a-cb15-4b4b-b59a-a681614e1aca', N'يمني', N'12441', N'12441', N'444', NULL, NULL, NULL, N'yomna', N'YOMNA', N'yomnaayman@gmail.com', N'YOMNAAYMAN@GMAIL.COM', 0, N'AQAAAAEAACcQAAAAEMile7Dr1NusIBBGj/kwlgJnxUGzBlcXrsXIeHQDubBn/3f18CDa/ySvm7LWxj6b+w==', N'22EF6VYLOBLJRYJM7JA2DFOWB24CLVV7', N'b996d854-884c-49bd-8676-4231f043d2b4', NULL, 0, 0, NULL, 1, 0, NULL)
INSERT [dbo].[AspNetUsers] ([Id], [NameAr], [Mobile], [Phone], [Code], [HealthCareUnitId], [HealthdirId], [HealthDistrictId], [UserName], [NormalizedUserName], [Email], [NormalizedEmail], [EmailConfirmed], [PasswordHash], [SecurityStamp], [ConcurrencyStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEnd], [LockoutEnabled], [AccessFailedCount], [OrganizationId]) VALUES (N'709984f7-8bab-4aad-bcd9-077812d1357a', N'بسمة', N'12441', N'12441', N'444', NULL, 1, NULL, N'basma', N'BASMA', N'bass@gmail.com', N'BASS@GMAIL.COM', 0, N'AQAAAAEAACcQAAAAEM65Ez1w2hiL9XfzGh+FeLFscYos37yDApr1Jd5QdwAHfgYK/bDno61iWIoQdLBx7g==', N'VRJITNQ7WDU7FZQUOJMTSWAFMZ5BVOEI', N'ec192046-6e32-4594-8958-cc27ea76e880', NULL, 0, 0, NULL, 1, 0, NULL)
INSERT [dbo].[AspNetUsers] ([Id], [NameAr], [Mobile], [Phone], [Code], [HealthCareUnitId], [HealthdirId], [HealthDistrictId], [UserName], [NormalizedUserName], [Email], [NormalizedEmail], [EmailConfirmed], [PasswordHash], [SecurityStamp], [ConcurrencyStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEnd], [LockoutEnabled], [AccessFailedCount], [OrganizationId]) VALUES (N'7252b92c-5f12-47a2-a20a-a50be1124505', N'esess', N'+13906820', N'+13906820', NULL, 4, 17, 21, N'esraa', N'ESRAA', N'esraa@gmail.com', N'ESRAA@GMAIL.COM', 0, N'AQAAAAEAACcQAAAAEPT8cOiYCgm3U/La7+8WUihtAC0HNPznm66kZ6UN1UbAZHUiVX9jZHYrtyKpEC5sGg==', N'AATS6KGHVN4YHPMPECW4SVI5R2W5A2TM', N'33444e3c-2e11-42dc-8959-9575252d0959', NULL, 0, 0, NULL, 1, 0, 1)
INSERT [dbo].[AspNetUsers] ([Id], [NameAr], [Mobile], [Phone], [Code], [HealthCareUnitId], [HealthdirId], [HealthDistrictId], [UserName], [NormalizedUserName], [Email], [NormalizedEmail], [EmailConfirmed], [PasswordHash], [SecurityStamp], [ConcurrencyStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEnd], [LockoutEnabled], [AccessFailedCount], [OrganizationId]) VALUES (N'f214ce7e-af25-47a0-95b8-20f29276201e', N'asss', N'+13906820', N'+13906820', NULL, 4, 17, 21, N'asss', N'ASSS', N'asmaa@gmail.com', N'ASMAA@GMAIL.COM', 0, N'AQAAAAEAACcQAAAAEF9J6MIcd2dcKrEmyJaxpsdnQ3YFWh1yUvIqn28ZRSBif/4H4seRUz/aKPvIChUt6w==', N'ZNOTNFSVNIQGF3PSU4JVRCZEDGLRRDV6', N'b61c7f52-201e-45c2-ad0c-afab3410ec40', NULL, 0, 0, NULL, 1, 0, 1)
INSERT [dbo].[AspNetUsers] ([Id], [NameAr], [Mobile], [Phone], [Code], [HealthCareUnitId], [HealthdirId], [HealthDistrictId], [UserName], [NormalizedUserName], [Email], [NormalizedEmail], [EmailConfirmed], [PasswordHash], [SecurityStamp], [ConcurrencyStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEnd], [LockoutEnabled], [AccessFailedCount], [OrganizationId]) VALUES (N'f8d65704-d05c-4039-97e5-fe92ceb8cfa6', N'lll', N'12441', N'12441', N'444', NULL, 2, NULL, N'nada', N'NADA', N'nada@gmail.com', N'NADA@GMAIL.COM', 0, N'AQAAAAEAACcQAAAAEH+GM7eE8tIRHdAwXz2ilLXfS7yHnSbWgQcgoSnNILd3yjq4UHl2QmE0YCxnhtvmOg==', N'DXMDXSSGLXMZTPFB3V6V6VYEYQYIS6HI', N'f4769b06-0c3b-4827-98d6-114c40723250', NULL, 0, 0, NULL, 1, 0, NULL)
SET IDENTITY_INSERT [dbo].[Departments] ON 

INSERT [dbo].[Departments] ([Id], [DepartmentName], [DepartmentNameAr], [DepartmentCode]) VALUES (1, N'Medical Record Department (RD1)', N'yy', N'')
SET IDENTITY_INSERT [dbo].[Departments] OFF
SET IDENTITY_INSERT [dbo].[Equiments] ON 

INSERT [dbo].[Equiments] ([Id], [EquipmentCode], [EquipmentMasterCode], [EquipmentName], [EquipmentNameAr], [EquipmentType], [InstallationDate], [HealthCareUnitId], [Remarks], [SerialNumber], [InternalCode], [Barcode], [PurchaseDate], [SupplierId], [EquipmentStatuSId], [Price], [WarrantyExpires], [Length], [Height], [Weight], [Color], [ColorAr], [CustomizedField], [DepartmentId], [Room], [Floor], [ProductionYear], [HealthDirectoryId], [MasterEquipmentId], [HealthDistrictId]) VALUES (1, N' 7050', N'a1ww', N'dump', N'', NULL, CAST(N'2021-09-07T00:00:00.0000000' AS DateTime2), 4, N'', N'22', N'a1ww  7050', N'ppp', CAST(N'2021-08-21T00:00:00.0000000' AS DateTime2), 1, 1, CAST(5000.00 AS Decimal(18, 2)), CAST(N'0001-01-01T00:00:00.0000000' AS DateTime2), 22, 22, 22, NULL, N'', NULL, 1, 0, 0, CAST(N'0001-01-01T00:00:00.0000000' AS DateTime2), 17, 1, 142)
SET IDENTITY_INSERT [dbo].[Equiments] OFF
SET IDENTITY_INSERT [dbo].[EquipmentCategories] ON 

INSERT [dbo].[EquipmentCategories] ([Id], [CategoryCode], [CategoryName], [CategoryNameAr], [HealthCareUnitId], [CategoryDescription], [CategoryDescriptionAr]) VALUES (3, NULL, N'gggg', NULL, 3, NULL, NULL)
SET IDENTITY_INSERT [dbo].[EquipmentCategories] OFF
SET IDENTITY_INSERT [dbo].[EquipmentStatus] ON 

INSERT [dbo].[EquipmentStatus] ([Id], [Status], [StatusAr]) VALUES (1, N'med', N'سيئ')
SET IDENTITY_INSERT [dbo].[EquipmentStatus] OFF
SET IDENTITY_INSERT [dbo].[HealthCareUnits] ON 

INSERT [dbo].[HealthCareUnits] ([Id], [HealthCareUnitCode], [HealthCareUnitName], [HealthCareUnitNameAr], [Address], [Director], [Phone], [Mobile], [Email], [Long], [Lat], [HealthDistrictId], [HealthDirectoryId], [organizationId]) VALUES (3, N'NASRCITY', NULL, NULL, NULL, NULL, NULL, NULL, NULL, 1, 2, 4, 2, 2)
INSERT [dbo].[HealthCareUnits] ([Id], [HealthCareUnitCode], [HealthCareUnitName], [HealthCareUnitNameAr], [Address], [Director], [Phone], [Mobile], [Email], [Long], [Lat], [HealthDistrictId], [HealthDirectoryId], [organizationId]) VALUES (4, N'OOO', N'durable', N'مدينة نصر', N'', N'ihab', N'+13906820', N'+13906820', N'ahmed@gmail.com', 0, 0, 142, 17, 1)
SET IDENTITY_INSERT [dbo].[HealthCareUnits] OFF
SET IDENTITY_INSERT [dbo].[HealthDirectories] ON 

INSERT [dbo].[HealthDirectories] ([Id], [HealthDirectoryCode], [HealthDirectoryName], [HealthDirectoryNameAr]) VALUES (1, N'01', N'Cairo', N'القاهرة')
INSERT [dbo].[HealthDirectories] ([Id], [HealthDirectoryCode], [HealthDirectoryName], [HealthDirectoryNameAr]) VALUES (2, N'02', N'Giza', N'الجيزة')
INSERT [dbo].[HealthDirectories] ([Id], [HealthDirectoryCode], [HealthDirectoryName], [HealthDirectoryNameAr]) VALUES (3, N'03', N'Alexandria', N'الأسكندرية')
INSERT [dbo].[HealthDirectories] ([Id], [HealthDirectoryCode], [HealthDirectoryName], [HealthDirectoryNameAr]) VALUES (4, N'04', N'Dakahlia', N'الدقهلية')
INSERT [dbo].[HealthDirectories] ([Id], [HealthDirectoryCode], [HealthDirectoryName], [HealthDirectoryNameAr]) VALUES (5, N'05', N'Red Sea', N'البحر الأحمر')
INSERT [dbo].[HealthDirectories] ([Id], [HealthDirectoryCode], [HealthDirectoryName], [HealthDirectoryNameAr]) VALUES (6, N'06', N'Beheira', N'البحيرة')
INSERT [dbo].[HealthDirectories] ([Id], [HealthDirectoryCode], [HealthDirectoryName], [HealthDirectoryNameAr]) VALUES (7, N'07', N'Fayoum', N'الفيوم')
INSERT [dbo].[HealthDirectories] ([Id], [HealthDirectoryCode], [HealthDirectoryName], [HealthDirectoryNameAr]) VALUES (8, N'08', N'Gharbiya', N'الغربية')
INSERT [dbo].[HealthDirectories] ([Id], [HealthDirectoryCode], [HealthDirectoryName], [HealthDirectoryNameAr]) VALUES (9, N'09', N'Ismailia', N'الإسماعلية')
INSERT [dbo].[HealthDirectories] ([Id], [HealthDirectoryCode], [HealthDirectoryName], [HealthDirectoryNameAr]) VALUES (10, N'010', N'Monofia', N'المنوفية')
INSERT [dbo].[HealthDirectories] ([Id], [HealthDirectoryCode], [HealthDirectoryName], [HealthDirectoryNameAr]) VALUES (11, N'011', N'Minya', N'المنيا')
INSERT [dbo].[HealthDirectories] ([Id], [HealthDirectoryCode], [HealthDirectoryName], [HealthDirectoryNameAr]) VALUES (12, N'012', N'Qaliubiya', N'القليوبية')
INSERT [dbo].[HealthDirectories] ([Id], [HealthDirectoryCode], [HealthDirectoryName], [HealthDirectoryNameAr]) VALUES (13, N'013', N'New Valley', N'الوادي الجديد')
INSERT [dbo].[HealthDirectories] ([Id], [HealthDirectoryCode], [HealthDirectoryName], [HealthDirectoryNameAr]) VALUES (14, N'014', N'Suez', N'السويس')
INSERT [dbo].[HealthDirectories] ([Id], [HealthDirectoryCode], [HealthDirectoryName], [HealthDirectoryNameAr]) VALUES (15, N'015', N'Aswan', N'اسوان')
INSERT [dbo].[HealthDirectories] ([Id], [HealthDirectoryCode], [HealthDirectoryName], [HealthDirectoryNameAr]) VALUES (16, N'016', N'Assiut', N'اسيوط')
INSERT [dbo].[HealthDirectories] ([Id], [HealthDirectoryCode], [HealthDirectoryName], [HealthDirectoryNameAr]) VALUES (17, N'017', N'Beni Suef', N'بني سويف')
INSERT [dbo].[HealthDirectories] ([Id], [HealthDirectoryCode], [HealthDirectoryName], [HealthDirectoryNameAr]) VALUES (18, N'018', N'Port Said', N'بورسعيد')
INSERT [dbo].[HealthDirectories] ([Id], [HealthDirectoryCode], [HealthDirectoryName], [HealthDirectoryNameAr]) VALUES (19, N'019', N'Damietta', N'دمياط')
INSERT [dbo].[HealthDirectories] ([Id], [HealthDirectoryCode], [HealthDirectoryName], [HealthDirectoryNameAr]) VALUES (20, N'020', N'Sharkia', N'الشرقية')
INSERT [dbo].[HealthDirectories] ([Id], [HealthDirectoryCode], [HealthDirectoryName], [HealthDirectoryNameAr]) VALUES (21, N'021', N'South Sinai', N'جنوب سيناء')
INSERT [dbo].[HealthDirectories] ([Id], [HealthDirectoryCode], [HealthDirectoryName], [HealthDirectoryNameAr]) VALUES (22, N'022', N'Kafr Al sheikh', N'كفر الشيخ')
INSERT [dbo].[HealthDirectories] ([Id], [HealthDirectoryCode], [HealthDirectoryName], [HealthDirectoryNameAr]) VALUES (23, N'023', N'Matrouh', N'مطروح')
INSERT [dbo].[HealthDirectories] ([Id], [HealthDirectoryCode], [HealthDirectoryName], [HealthDirectoryNameAr]) VALUES (24, N'024', N'Luxor', N'الأقصر')
INSERT [dbo].[HealthDirectories] ([Id], [HealthDirectoryCode], [HealthDirectoryName], [HealthDirectoryNameAr]) VALUES (25, N'025', N'Qena', N'قنا')
INSERT [dbo].[HealthDirectories] ([Id], [HealthDirectoryCode], [HealthDirectoryName], [HealthDirectoryNameAr]) VALUES (26, N'026', N'North Sinai', N'شمال سيناء')
INSERT [dbo].[HealthDirectories] ([Id], [HealthDirectoryCode], [HealthDirectoryName], [HealthDirectoryNameAr]) VALUES (27, N'027', N'Sohag', N'سوهاج')
SET IDENTITY_INSERT [dbo].[HealthDirectories] OFF
SET IDENTITY_INSERT [dbo].[HealthDistricts] ON 

INSERT [dbo].[HealthDistricts] ([Id], [HealthDirectoryId], [HealthDistrictCode], [HealthDistrictName], [HealthDistrictNameAr]) VALUES (1, 1, N'', N'Cairo', N'القاهره')
INSERT [dbo].[HealthDistricts] ([Id], [HealthDirectoryId], [HealthDistrictCode], [HealthDistrictName], [HealthDistrictNameAr]) VALUES (2, 2, N'', N'Giza', N'الجيزة')
INSERT [dbo].[HealthDistricts] ([Id], [HealthDirectoryId], [HealthDistrictCode], [HealthDistrictName], [HealthDistrictNameAr]) VALUES (3, 2, N'', N'Sixth of October', N'السادس من أكتوبر')
INSERT [dbo].[HealthDistricts] ([Id], [HealthDirectoryId], [HealthDistrictCode], [HealthDistrictName], [HealthDistrictNameAr]) VALUES (4, 2, N'', N'Sheikh Zayed', N'الشيخ زايد')
INSERT [dbo].[HealthDistricts] ([Id], [HealthDirectoryId], [HealthDistrictCode], [HealthDistrictName], [HealthDistrictNameAr]) VALUES (5, 2, N'', N'Hawamdiyah', N'الحوامدية')
INSERT [dbo].[HealthDistricts] ([Id], [HealthDirectoryId], [HealthDistrictCode], [HealthDistrictName], [HealthDistrictNameAr]) VALUES (6, 2, N'', N'Al Badrasheen', N'البدرشين')
INSERT [dbo].[HealthDistricts] ([Id], [HealthDirectoryId], [HealthDistrictCode], [HealthDistrictName], [HealthDistrictNameAr]) VALUES (7, 2, N'', N'Saf', N'الصف')
INSERT [dbo].[HealthDistricts] ([Id], [HealthDirectoryId], [HealthDistrictCode], [HealthDistrictName], [HealthDistrictNameAr]) VALUES (8, 2, N'', N'Atfih', N'أطفيح')
INSERT [dbo].[HealthDistricts] ([Id], [HealthDirectoryId], [HealthDistrictCode], [HealthDistrictName], [HealthDistrictNameAr]) VALUES (9, 2, N'', N'Al Ayat', N'العياط')
INSERT [dbo].[HealthDistricts] ([Id], [HealthDirectoryId], [HealthDistrictCode], [HealthDistrictName], [HealthDistrictNameAr]) VALUES (10, 2, N'', N'Al-Bawaiti', N'الباويطي')
INSERT [dbo].[HealthDistricts] ([Id], [HealthDirectoryId], [HealthDistrictCode], [HealthDistrictName], [HealthDistrictNameAr]) VALUES (11, 2, N'', N'ManshiyetAl Qanater', N'منشأة القناطر')
INSERT [dbo].[HealthDistricts] ([Id], [HealthDirectoryId], [HealthDistrictCode], [HealthDistrictName], [HealthDistrictNameAr]) VALUES (12, 2, N'', N'Oaseem', N'أوسيم')
INSERT [dbo].[HealthDistricts] ([Id], [HealthDirectoryId], [HealthDistrictCode], [HealthDistrictName], [HealthDistrictNameAr]) VALUES (13, 2, N'', N'Kerdasa', N'كرداسة')
INSERT [dbo].[HealthDistricts] ([Id], [HealthDirectoryId], [HealthDistrictCode], [HealthDistrictName], [HealthDistrictNameAr]) VALUES (14, 2, N'', N'Abu Nomros', N'أبو النمرس')
INSERT [dbo].[HealthDistricts] ([Id], [HealthDirectoryId], [HealthDistrictCode], [HealthDistrictName], [HealthDistrictNameAr]) VALUES (15, 2, N'', N'Kafr Ghati', N'كفر غطاطي')
INSERT [dbo].[HealthDistricts] ([Id], [HealthDirectoryId], [HealthDistrictCode], [HealthDistrictName], [HealthDistrictNameAr]) VALUES (16, 2, N'', N'Manshiyet Al Bakari', N'منشأة البكاري')
INSERT [dbo].[HealthDistricts] ([Id], [HealthDirectoryId], [HealthDistrictCode], [HealthDistrictName], [HealthDistrictNameAr]) VALUES (17, 3, N'', N'Alexandria', N'الأسكندرية')
INSERT [dbo].[HealthDistricts] ([Id], [HealthDirectoryId], [HealthDistrictCode], [HealthDistrictName], [HealthDistrictNameAr]) VALUES (18, 3, N'', N'Burj Al Arab', N'برج العرب')
INSERT [dbo].[HealthDistricts] ([Id], [HealthDirectoryId], [HealthDistrictCode], [HealthDistrictName], [HealthDistrictNameAr]) VALUES (19, 3, N'', N'New Burj Al Arab', N'برج العرب الجديدة')
INSERT [dbo].[HealthDistricts] ([Id], [HealthDirectoryId], [HealthDistrictCode], [HealthDistrictName], [HealthDistrictNameAr]) VALUES (20, 4, N'', N'Mansoura', N'المنصورة')
INSERT [dbo].[HealthDistricts] ([Id], [HealthDirectoryId], [HealthDistrictCode], [HealthDistrictName], [HealthDistrictNameAr]) VALUES (21, 4, N'', N'Talkha', N'طلخا')
INSERT [dbo].[HealthDistricts] ([Id], [HealthDirectoryId], [HealthDistrictCode], [HealthDistrictName], [HealthDistrictNameAr]) VALUES (22, 4, N'', N'Mitt Ghamr', N'ميت غمر')
INSERT [dbo].[HealthDistricts] ([Id], [HealthDirectoryId], [HealthDistrictCode], [HealthDistrictName], [HealthDistrictNameAr]) VALUES (23, 4, N'', N'Dekernes', N'دكرنس')
INSERT [dbo].[HealthDistricts] ([Id], [HealthDirectoryId], [HealthDistrictCode], [HealthDistrictName], [HealthDistrictNameAr]) VALUES (24, 4, N'', N'Aga', N'أجا')
INSERT [dbo].[HealthDistricts] ([Id], [HealthDirectoryId], [HealthDistrictCode], [HealthDistrictName], [HealthDistrictNameAr]) VALUES (25, 4, N'', N'Menia El Nasr', N'منية النصر')
INSERT [dbo].[HealthDistricts] ([Id], [HealthDirectoryId], [HealthDistrictCode], [HealthDistrictName], [HealthDistrictNameAr]) VALUES (26, 4, N'', N'Sinbillawin', N'السنبلاوين')
INSERT [dbo].[HealthDistricts] ([Id], [HealthDirectoryId], [HealthDistrictCode], [HealthDistrictName], [HealthDistrictNameAr]) VALUES (27, 4, N'', N'El Kurdi', N'الكردي')
INSERT [dbo].[HealthDistricts] ([Id], [HealthDirectoryId], [HealthDistrictCode], [HealthDistrictName], [HealthDistrictNameAr]) VALUES (28, 4, N'', N'Bani Ubaid', N'بني عبيد')
INSERT [dbo].[HealthDistricts] ([Id], [HealthDirectoryId], [HealthDistrictCode], [HealthDistrictName], [HealthDistrictNameAr]) VALUES (29, 4, N'', N'Al Manzala', N'المنزلة')
INSERT [dbo].[HealthDistricts] ([Id], [HealthDirectoryId], [HealthDistrictCode], [HealthDistrictName], [HealthDistrictNameAr]) VALUES (30, 4, N'', N'Tami al amdid', N'تمي الأمديد')
INSERT [dbo].[HealthDistricts] ([Id], [HealthDirectoryId], [HealthDistrictCode], [HealthDistrictName], [HealthDistrictNameAr]) VALUES (31, 4, N'', N'aljamalia', N'الجمالية')
INSERT [dbo].[HealthDistricts] ([Id], [HealthDirectoryId], [HealthDistrictCode], [HealthDistrictName], [HealthDistrictNameAr]) VALUES (32, 4, N'', N'Sherbin', N'شربين')
INSERT [dbo].[HealthDistricts] ([Id], [HealthDirectoryId], [HealthDistrictCode], [HealthDistrictName], [HealthDistrictNameAr]) VALUES (33, 4, N'', N'Mataria', N'المطرية')
INSERT [dbo].[HealthDistricts] ([Id], [HealthDirectoryId], [HealthDistrictCode], [HealthDistrictName], [HealthDistrictNameAr]) VALUES (34, 4, N'', N'Belqas', N'بلقاس')
INSERT [dbo].[HealthDistricts] ([Id], [HealthDirectoryId], [HealthDistrictCode], [HealthDistrictName], [HealthDistrictNameAr]) VALUES (35, 4, N'', N'Meet Salsil', N'ميت سلسيل')
INSERT [dbo].[HealthDistricts] ([Id], [HealthDirectoryId], [HealthDistrictCode], [HealthDistrictName], [HealthDistrictNameAr]) VALUES (36, 4, N'', N'Gamasa', N'جمصة')
INSERT [dbo].[HealthDistricts] ([Id], [HealthDirectoryId], [HealthDistrictCode], [HealthDistrictName], [HealthDistrictNameAr]) VALUES (37, 4, N'', N'Mahalat Damana', N'محلة دمنة')
INSERT [dbo].[HealthDistricts] ([Id], [HealthDirectoryId], [HealthDistrictCode], [HealthDistrictName], [HealthDistrictNameAr]) VALUES (38, 4, N'', N'Nabroh', N'نبروه')
INSERT [dbo].[HealthDistricts] ([Id], [HealthDirectoryId], [HealthDistrictCode], [HealthDistrictName], [HealthDistrictNameAr]) VALUES (39, 1, N'', N'Hurghada', N'الغردقة')
INSERT [dbo].[HealthDistricts] ([Id], [HealthDirectoryId], [HealthDistrictCode], [HealthDistrictName], [HealthDistrictNameAr]) VALUES (40, 1, N'', N'Ras Ghareb', N'رأس غارب')
INSERT [dbo].[HealthDistricts] ([Id], [HealthDirectoryId], [HealthDistrictCode], [HealthDistrictName], [HealthDistrictNameAr]) VALUES (41, 1, N'', N'Safaga', N'سفاجا')
INSERT [dbo].[HealthDistricts] ([Id], [HealthDirectoryId], [HealthDistrictCode], [HealthDistrictName], [HealthDistrictNameAr]) VALUES (42, 1, N'', N'El Qusiar', N'القصير')
INSERT [dbo].[HealthDistricts] ([Id], [HealthDirectoryId], [HealthDistrictCode], [HealthDistrictName], [HealthDistrictNameAr]) VALUES (43, 1, N'', N'Marsa Alam', N'مرسى علم')
INSERT [dbo].[HealthDistricts] ([Id], [HealthDirectoryId], [HealthDistrictCode], [HealthDistrictName], [HealthDistrictNameAr]) VALUES (44, 1, N'', N'Shalatin', N'الشلاتين')
INSERT [dbo].[HealthDistricts] ([Id], [HealthDirectoryId], [HealthDistrictCode], [HealthDistrictName], [HealthDistrictNameAr]) VALUES (45, 1, N'', N'Halaib', N'حلايب')
INSERT [dbo].[HealthDistricts] ([Id], [HealthDirectoryId], [HealthDistrictCode], [HealthDistrictName], [HealthDistrictNameAr]) VALUES (46, 6, N'', N'Damanhour', N'دمنهور')
INSERT [dbo].[HealthDistricts] ([Id], [HealthDirectoryId], [HealthDistrictCode], [HealthDistrictName], [HealthDistrictNameAr]) VALUES (47, 6, N'', N'Kafr El Dawar', N'كفر الدوار')
INSERT [dbo].[HealthDistricts] ([Id], [HealthDirectoryId], [HealthDistrictCode], [HealthDistrictName], [HealthDistrictNameAr]) VALUES (48, 6, N'', N'Rashid', N'رشيد')
INSERT [dbo].[HealthDistricts] ([Id], [HealthDirectoryId], [HealthDistrictCode], [HealthDistrictName], [HealthDistrictNameAr]) VALUES (49, 6, N'', N'Edco', N'إدكو')
INSERT [dbo].[HealthDistricts] ([Id], [HealthDirectoryId], [HealthDistrictCode], [HealthDistrictName], [HealthDistrictNameAr]) VALUES (50, 6, N'', N'Abu al-Matamir', N'أبو المطامير')
INSERT [dbo].[HealthDistricts] ([Id], [HealthDirectoryId], [HealthDistrictCode], [HealthDistrictName], [HealthDistrictNameAr]) VALUES (51, 6, N'', N'Abu Homs', N'أبو حمص')
INSERT [dbo].[HealthDistricts] ([Id], [HealthDirectoryId], [HealthDistrictCode], [HealthDistrictName], [HealthDistrictNameAr]) VALUES (52, 6, N'', N'Delengat', N'الدلنجات')
INSERT [dbo].[HealthDistricts] ([Id], [HealthDirectoryId], [HealthDistrictCode], [HealthDistrictName], [HealthDistrictNameAr]) VALUES (53, 6, N'', N'Mahmoudiyah', N'المحمودية')
INSERT [dbo].[HealthDistricts] ([Id], [HealthDirectoryId], [HealthDistrictCode], [HealthDistrictName], [HealthDistrictNameAr]) VALUES (54, 6, N'', N'Rahmaniyah', N'الرحمانية')
INSERT [dbo].[HealthDistricts] ([Id], [HealthDirectoryId], [HealthDistrictCode], [HealthDistrictName], [HealthDistrictNameAr]) VALUES (55, 6, N'', N'Itai Baroud', N'إيتاي البارود')
INSERT [dbo].[HealthDistricts] ([Id], [HealthDirectoryId], [HealthDistrictCode], [HealthDistrictName], [HealthDistrictNameAr]) VALUES (56, 6, N'', N'Housh Eissa', N'حوش عيسى')
INSERT [dbo].[HealthDistricts] ([Id], [HealthDirectoryId], [HealthDistrictCode], [HealthDistrictName], [HealthDistrictNameAr]) VALUES (57, 6, N'', N'Shubrakhit', N'شبراخيت')
INSERT [dbo].[HealthDistricts] ([Id], [HealthDirectoryId], [HealthDistrictCode], [HealthDistrictName], [HealthDistrictNameAr]) VALUES (58, 6, N'', N'Kom Hamada', N'كوم حمادة')
INSERT [dbo].[HealthDistricts] ([Id], [HealthDirectoryId], [HealthDistrictCode], [HealthDistrictName], [HealthDistrictNameAr]) VALUES (59, 6, N'', N'Badr', N'بدر')
INSERT [dbo].[HealthDistricts] ([Id], [HealthDirectoryId], [HealthDistrictCode], [HealthDistrictName], [HealthDistrictNameAr]) VALUES (60, 6, N'', N'Wadi Natrun', N'وادي النطرون')
INSERT [dbo].[HealthDistricts] ([Id], [HealthDirectoryId], [HealthDistrictCode], [HealthDistrictName], [HealthDistrictNameAr]) VALUES (61, 6, N'', N'New Nubaria', N'النوبارية الجديدة')
INSERT [dbo].[HealthDistricts] ([Id], [HealthDirectoryId], [HealthDistrictCode], [HealthDistrictName], [HealthDistrictNameAr]) VALUES (62, 7, N'', N'Fayoum', N'الفيوم')
INSERT [dbo].[HealthDistricts] ([Id], [HealthDirectoryId], [HealthDistrictCode], [HealthDistrictName], [HealthDistrictNameAr]) VALUES (63, 7, N'', N'Fayoum El Gedida', N'الفيوم الجديدة')
INSERT [dbo].[HealthDistricts] ([Id], [HealthDirectoryId], [HealthDistrictCode], [HealthDistrictName], [HealthDistrictNameAr]) VALUES (64, 7, N'', N'Tamiya', N'طامية')
INSERT [dbo].[HealthDistricts] ([Id], [HealthDirectoryId], [HealthDistrictCode], [HealthDistrictName], [HealthDistrictNameAr]) VALUES (65, 7, N'', N'Snores', N'سنورس')
INSERT [dbo].[HealthDistricts] ([Id], [HealthDirectoryId], [HealthDistrictCode], [HealthDistrictName], [HealthDistrictNameAr]) VALUES (66, 7, N'', N'Etsa', N'إطسا')
INSERT [dbo].[HealthDistricts] ([Id], [HealthDirectoryId], [HealthDistrictCode], [HealthDistrictName], [HealthDistrictNameAr]) VALUES (67, 7, N'', N'Epschway', N'إبشواي')
INSERT [dbo].[HealthDistricts] ([Id], [HealthDirectoryId], [HealthDistrictCode], [HealthDistrictName], [HealthDistrictNameAr]) VALUES (68, 7, N'', N'Yusuf El Sediaq', N'يوسف الصديق')
INSERT [dbo].[HealthDistricts] ([Id], [HealthDirectoryId], [HealthDistrictCode], [HealthDistrictName], [HealthDistrictNameAr]) VALUES (69, 8, N'', N'Tanta', N'طنطا')
INSERT [dbo].[HealthDistricts] ([Id], [HealthDirectoryId], [HealthDistrictCode], [HealthDistrictName], [HealthDistrictNameAr]) VALUES (70, 8, N'', N'Al Mahalla Al Kobra', N'المحلة الكبرى')
INSERT [dbo].[HealthDistricts] ([Id], [HealthDirectoryId], [HealthDistrictCode], [HealthDistrictName], [HealthDistrictNameAr]) VALUES (71, 8, N'', N'Kafr El Zayat', N'كفر الزيات')
INSERT [dbo].[HealthDistricts] ([Id], [HealthDirectoryId], [HealthDistrictCode], [HealthDistrictName], [HealthDistrictNameAr]) VALUES (72, 8, N'', N'Zefta', N'زفتى')
INSERT [dbo].[HealthDistricts] ([Id], [HealthDirectoryId], [HealthDistrictCode], [HealthDistrictName], [HealthDistrictNameAr]) VALUES (73, 8, N'', N'El Santa', N'السنطة')
INSERT [dbo].[HealthDistricts] ([Id], [HealthDirectoryId], [HealthDistrictCode], [HealthDistrictName], [HealthDistrictNameAr]) VALUES (74, 8, N'', N'Qutour', N'قطور')
INSERT [dbo].[HealthDistricts] ([Id], [HealthDirectoryId], [HealthDistrictCode], [HealthDistrictName], [HealthDistrictNameAr]) VALUES (75, 8, N'', N'Basion', N'بسيون')
INSERT [dbo].[HealthDistricts] ([Id], [HealthDirectoryId], [HealthDistrictCode], [HealthDistrictName], [HealthDistrictNameAr]) VALUES (76, 8, N'', N'Samannoud', N'سمنود')
INSERT [dbo].[HealthDistricts] ([Id], [HealthDirectoryId], [HealthDistrictCode], [HealthDistrictName], [HealthDistrictNameAr]) VALUES (77, 9, N'', N'Ismailia', N'الإسماعيلية')
INSERT [dbo].[HealthDistricts] ([Id], [HealthDirectoryId], [HealthDistrictCode], [HealthDistrictName], [HealthDistrictNameAr]) VALUES (78, 9, N'', N'Fayed', N'فايد')
INSERT [dbo].[HealthDistricts] ([Id], [HealthDirectoryId], [HealthDistrictCode], [HealthDistrictName], [HealthDistrictNameAr]) VALUES (79, 9, N'', N'Qantara Sharq', N'القنطرة شرق ')
INSERT [dbo].[HealthDistricts] ([Id], [HealthDirectoryId], [HealthDistrictCode], [HealthDistrictName], [HealthDistrictNameAr]) VALUES (80, 9, N'', N'Qantara Gharb', N'القنطرة غرب')
INSERT [dbo].[HealthDistricts] ([Id], [HealthDirectoryId], [HealthDistrictCode], [HealthDistrictName], [HealthDistrictNameAr]) VALUES (81, 9, N'', N'El Tal El Kabier', N'التل الكبير')
INSERT [dbo].[HealthDistricts] ([Id], [HealthDirectoryId], [HealthDistrictCode], [HealthDistrictName], [HealthDistrictNameAr]) VALUES (82, 9, N'', N'Abu Sawir', N'أبو صوير')
INSERT [dbo].[HealthDistricts] ([Id], [HealthDirectoryId], [HealthDistrictCode], [HealthDistrictName], [HealthDistrictNameAr]) VALUES (83, 9, N'', N'Kasasien El Gedida', N'القصاصين الجديدة')
INSERT [dbo].[HealthDistricts] ([Id], [HealthDirectoryId], [HealthDistrictCode], [HealthDistrictName], [HealthDistrictNameAr]) VALUES (84, 10, N'', N'Shbeen El Koom', N'شبين الكوم')
INSERT [dbo].[HealthDistricts] ([Id], [HealthDirectoryId], [HealthDistrictCode], [HealthDistrictName], [HealthDistrictNameAr]) VALUES (85, 10, N'', N'Sadat City', N'مدينة السادات ')
INSERT [dbo].[HealthDistricts] ([Id], [HealthDirectoryId], [HealthDistrictCode], [HealthDistrictName], [HealthDistrictNameAr]) VALUES (86, 10, N'', N'Menouf', N'منوف')
INSERT [dbo].[HealthDistricts] ([Id], [HealthDirectoryId], [HealthDistrictCode], [HealthDistrictName], [HealthDistrictNameAr]) VALUES (87, 10, N'', N'Sars El-Layan', N'سرس الليان')
INSERT [dbo].[HealthDistricts] ([Id], [HealthDirectoryId], [HealthDistrictCode], [HealthDistrictName], [HealthDistrictNameAr]) VALUES (88, 10, N'', N'Ashmon', N'أشمون')
INSERT [dbo].[HealthDistricts] ([Id], [HealthDirectoryId], [HealthDistrictCode], [HealthDistrictName], [HealthDistrictNameAr]) VALUES (89, 10, N'', N'Al Bagor', N'الباجور')
INSERT [dbo].[HealthDistricts] ([Id], [HealthDirectoryId], [HealthDistrictCode], [HealthDistrictName], [HealthDistrictNameAr]) VALUES (90, 10, N'', N'Quesna', N'قويسنا')
INSERT [dbo].[HealthDistricts] ([Id], [HealthDirectoryId], [HealthDistrictCode], [HealthDistrictName], [HealthDistrictNameAr]) VALUES (91, 10, N'', N'Berkat El Saba', N'بركة السبع')
INSERT [dbo].[HealthDistricts] ([Id], [HealthDirectoryId], [HealthDistrictCode], [HealthDistrictName], [HealthDistrictNameAr]) VALUES (92, 10, N'', N'Tala', N'تلا')
INSERT [dbo].[HealthDistricts] ([Id], [HealthDirectoryId], [HealthDistrictCode], [HealthDistrictName], [HealthDistrictNameAr]) VALUES (93, 10, N'', N'Al Shohada', N'الشهداء')
INSERT [dbo].[HealthDistricts] ([Id], [HealthDirectoryId], [HealthDistrictCode], [HealthDistrictName], [HealthDistrictNameAr]) VALUES (94, 11, N'', N'Minya', N'المنيا')
INSERT [dbo].[HealthDistricts] ([Id], [HealthDirectoryId], [HealthDistrictCode], [HealthDistrictName], [HealthDistrictNameAr]) VALUES (95, 11, N'', N'Minya El Gedida', N'المنيا الجديدة')
INSERT [dbo].[HealthDistricts] ([Id], [HealthDirectoryId], [HealthDistrictCode], [HealthDistrictName], [HealthDistrictNameAr]) VALUES (96, 11, N'', N'El Adwa', N'العدوة')
INSERT [dbo].[HealthDistricts] ([Id], [HealthDirectoryId], [HealthDistrictCode], [HealthDistrictName], [HealthDistrictNameAr]) VALUES (97, 11, N'', N'Magagha', N'مغاغة')
INSERT [dbo].[HealthDistricts] ([Id], [HealthDirectoryId], [HealthDistrictCode], [HealthDistrictName], [HealthDistrictNameAr]) VALUES (98, 11, N'', N'Bani Mazar', N'بني مزار')
INSERT [dbo].[HealthDistricts] ([Id], [HealthDirectoryId], [HealthDistrictCode], [HealthDistrictName], [HealthDistrictNameAr]) VALUES (99, 11, N'', N'Mattay', N'مطاي')
GO
INSERT [dbo].[HealthDistricts] ([Id], [HealthDirectoryId], [HealthDistrictCode], [HealthDistrictName], [HealthDistrictNameAr]) VALUES (100, 11, N'', N'Samalut', N'سمالوط')
INSERT [dbo].[HealthDistricts] ([Id], [HealthDirectoryId], [HealthDistrictCode], [HealthDistrictName], [HealthDistrictNameAr]) VALUES (101, 11, N'', N'Madinat El Fekria', N'المدينة الفكرية')
INSERT [dbo].[HealthDistricts] ([Id], [HealthDirectoryId], [HealthDistrictCode], [HealthDistrictName], [HealthDistrictNameAr]) VALUES (102, 11, N'', N'Meloy', N'ملوي')
INSERT [dbo].[HealthDistricts] ([Id], [HealthDirectoryId], [HealthDistrictCode], [HealthDistrictName], [HealthDistrictNameAr]) VALUES (103, 11, N'', N'Deir Mawas', N'دير مواس')
INSERT [dbo].[HealthDistricts] ([Id], [HealthDirectoryId], [HealthDistrictCode], [HealthDistrictName], [HealthDistrictNameAr]) VALUES (104, 12, N'', N'Banha', N'بنها')
INSERT [dbo].[HealthDistricts] ([Id], [HealthDirectoryId], [HealthDistrictCode], [HealthDistrictName], [HealthDistrictNameAr]) VALUES (105, 12, N'', N'Qalyub', N'قليوب')
INSERT [dbo].[HealthDistricts] ([Id], [HealthDirectoryId], [HealthDistrictCode], [HealthDistrictName], [HealthDistrictNameAr]) VALUES (106, 12, N'', N'Shubra Al Khaimah', N'العدوة')
INSERT [dbo].[HealthDistricts] ([Id], [HealthDirectoryId], [HealthDistrictCode], [HealthDistrictName], [HealthDistrictNameAr]) VALUES (107, 12, N'', N'Al Qanater Charity', N'مغاغة')
INSERT [dbo].[HealthDistricts] ([Id], [HealthDirectoryId], [HealthDistrictCode], [HealthDistrictName], [HealthDistrictNameAr]) VALUES (108, 12, N'', N'Khanka', N'بني مزار')
INSERT [dbo].[HealthDistricts] ([Id], [HealthDirectoryId], [HealthDistrictCode], [HealthDistrictName], [HealthDistrictNameAr]) VALUES (109, 12, N'', N'Tukh', N'مطاي')
INSERT [dbo].[HealthDistricts] ([Id], [HealthDirectoryId], [HealthDistrictCode], [HealthDistrictName], [HealthDistrictNameAr]) VALUES (110, 12, N'', N'Qaha', N'سمالوط')
INSERT [dbo].[HealthDistricts] ([Id], [HealthDirectoryId], [HealthDistrictCode], [HealthDistrictName], [HealthDistrictNameAr]) VALUES (111, 12, N'', N'Obour', N'المدينة الفكرية')
INSERT [dbo].[HealthDistricts] ([Id], [HealthDirectoryId], [HealthDistrictCode], [HealthDistrictName], [HealthDistrictNameAr]) VALUES (112, 12, N'', N'Khosous', N'ملوي')
INSERT [dbo].[HealthDistricts] ([Id], [HealthDirectoryId], [HealthDistrictCode], [HealthDistrictName], [HealthDistrictNameAr]) VALUES (113, 12, N'', N'Shibin Al Qanater', N'دير مواس')
INSERT [dbo].[HealthDistricts] ([Id], [HealthDirectoryId], [HealthDistrictCode], [HealthDistrictName], [HealthDistrictNameAr]) VALUES (114, 13, N'', N'El Kharga', N'الخارجة')
INSERT [dbo].[HealthDistricts] ([Id], [HealthDirectoryId], [HealthDistrictCode], [HealthDistrictName], [HealthDistrictNameAr]) VALUES (115, 13, N'', N'Paris', N'باريس')
INSERT [dbo].[HealthDistricts] ([Id], [HealthDirectoryId], [HealthDistrictCode], [HealthDistrictName], [HealthDistrictNameAr]) VALUES (116, 13, N'', N'Mout', N'موط')
INSERT [dbo].[HealthDistricts] ([Id], [HealthDirectoryId], [HealthDistrictCode], [HealthDistrictName], [HealthDistrictNameAr]) VALUES (117, 13, N'', N'Farafra', N'الفرافرة')
INSERT [dbo].[HealthDistricts] ([Id], [HealthDirectoryId], [HealthDistrictCode], [HealthDistrictName], [HealthDistrictNameAr]) VALUES (118, 13, N'', N'Balat', N'بلاط')
INSERT [dbo].[HealthDistricts] ([Id], [HealthDirectoryId], [HealthDistrictCode], [HealthDistrictName], [HealthDistrictNameAr]) VALUES (119, 14, N'', N'Suez', N'السويس')
INSERT [dbo].[HealthDistricts] ([Id], [HealthDirectoryId], [HealthDistrictCode], [HealthDistrictName], [HealthDistrictNameAr]) VALUES (120, 15, N'', N'Aswan', N'أسوان')
INSERT [dbo].[HealthDistricts] ([Id], [HealthDirectoryId], [HealthDistrictCode], [HealthDistrictName], [HealthDistrictNameAr]) VALUES (121, 15, N'', N'Aswan El Gedida', N'أسوان الجديدة')
INSERT [dbo].[HealthDistricts] ([Id], [HealthDirectoryId], [HealthDistrictCode], [HealthDistrictName], [HealthDistrictNameAr]) VALUES (122, 15, N'', N'Drau', N'دراو')
INSERT [dbo].[HealthDistricts] ([Id], [HealthDirectoryId], [HealthDistrictCode], [HealthDistrictName], [HealthDistrictNameAr]) VALUES (123, 15, N'', N'Kom Ombo', N'كوم أمبو')
INSERT [dbo].[HealthDistricts] ([Id], [HealthDirectoryId], [HealthDistrictCode], [HealthDistrictName], [HealthDistrictNameAr]) VALUES (124, 15, N'', N'Nasr Al Nuba', N'نصر النوبة')
INSERT [dbo].[HealthDistricts] ([Id], [HealthDirectoryId], [HealthDistrictCode], [HealthDistrictName], [HealthDistrictNameAr]) VALUES (125, 15, N'', N'Kalabsha', N'كلابشة')
INSERT [dbo].[HealthDistricts] ([Id], [HealthDirectoryId], [HealthDistrictCode], [HealthDistrictName], [HealthDistrictNameAr]) VALUES (126, 15, N'', N'Edfu', N'إدفو')
INSERT [dbo].[HealthDistricts] ([Id], [HealthDirectoryId], [HealthDistrictCode], [HealthDistrictName], [HealthDistrictNameAr]) VALUES (127, 15, N'', N'Al-Radisiyah', N'الرديسية')
INSERT [dbo].[HealthDistricts] ([Id], [HealthDirectoryId], [HealthDistrictCode], [HealthDistrictName], [HealthDistrictNameAr]) VALUES (128, 15, N'', N'Al Basilia', N'البصيلية')
INSERT [dbo].[HealthDistricts] ([Id], [HealthDirectoryId], [HealthDistrictCode], [HealthDistrictName], [HealthDistrictNameAr]) VALUES (129, 15, N'', N'Al Sibaeia', N'السباعية')
INSERT [dbo].[HealthDistricts] ([Id], [HealthDirectoryId], [HealthDistrictCode], [HealthDistrictName], [HealthDistrictNameAr]) VALUES (130, 15, N'', N'Abo Simbl Al Siyahia', N'ابوسمبل السياحية')
INSERT [dbo].[HealthDistricts] ([Id], [HealthDirectoryId], [HealthDistrictCode], [HealthDistrictName], [HealthDistrictNameAr]) VALUES (131, 16, N'', N'Assiut', N'أسيوط')
INSERT [dbo].[HealthDistricts] ([Id], [HealthDirectoryId], [HealthDistrictCode], [HealthDistrictName], [HealthDistrictNameAr]) VALUES (132, 16, N'', N'Assiut El Gedidaa', N'أسيوط الجديدة')
INSERT [dbo].[HealthDistricts] ([Id], [HealthDirectoryId], [HealthDistrictCode], [HealthDistrictName], [HealthDistrictNameAr]) VALUES (133, 16, N'', N'Dayrout', N'ديروط')
INSERT [dbo].[HealthDistricts] ([Id], [HealthDirectoryId], [HealthDistrictCode], [HealthDistrictName], [HealthDistrictNameAr]) VALUES (134, 16, N'', N'Manfalut', N'منفلوط')
INSERT [dbo].[HealthDistricts] ([Id], [HealthDirectoryId], [HealthDistrictCode], [HealthDistrictName], [HealthDistrictNameAr]) VALUES (135, 16, N'', N'Qusiya', N'القوصية')
INSERT [dbo].[HealthDistricts] ([Id], [HealthDirectoryId], [HealthDistrictCode], [HealthDistrictName], [HealthDistrictNameAr]) VALUES (136, 16, N'', N'Abnoub', N'أبنوب')
INSERT [dbo].[HealthDistricts] ([Id], [HealthDirectoryId], [HealthDistrictCode], [HealthDistrictName], [HealthDistrictNameAr]) VALUES (137, 16, N'', N'Abu Tig', N'أبو تيج')
INSERT [dbo].[HealthDistricts] ([Id], [HealthDirectoryId], [HealthDistrictCode], [HealthDistrictName], [HealthDistrictNameAr]) VALUES (138, 16, N'', N'El Ghanaim', N'الغنايم')
INSERT [dbo].[HealthDistricts] ([Id], [HealthDirectoryId], [HealthDistrictCode], [HealthDistrictName], [HealthDistrictNameAr]) VALUES (139, 16, N'', N'Sahel Selim', N'ساحل سليم')
INSERT [dbo].[HealthDistricts] ([Id], [HealthDirectoryId], [HealthDistrictCode], [HealthDistrictName], [HealthDistrictNameAr]) VALUES (140, 16, N'', N'El Badari', N'البداري')
INSERT [dbo].[HealthDistricts] ([Id], [HealthDirectoryId], [HealthDistrictCode], [HealthDistrictName], [HealthDistrictNameAr]) VALUES (141, 16, N'', N'Sidfa', N'صدفا')
INSERT [dbo].[HealthDistricts] ([Id], [HealthDirectoryId], [HealthDistrictCode], [HealthDistrictName], [HealthDistrictNameAr]) VALUES (142, 17, N'', N'Bani Sweif', N'بني سويف')
INSERT [dbo].[HealthDistricts] ([Id], [HealthDirectoryId], [HealthDistrictCode], [HealthDistrictName], [HealthDistrictNameAr]) VALUES (143, 17, N'', N'Beni Suef El Gedida', N'بني سويف الجديدة')
INSERT [dbo].[HealthDistricts] ([Id], [HealthDirectoryId], [HealthDistrictCode], [HealthDistrictName], [HealthDistrictNameAr]) VALUES (144, 17, N'', N'Al Wasta', N'الواسطى')
INSERT [dbo].[HealthDistricts] ([Id], [HealthDirectoryId], [HealthDistrictCode], [HealthDistrictName], [HealthDistrictNameAr]) VALUES (145, 17, N'', N'Naser', N'ناصر')
INSERT [dbo].[HealthDistricts] ([Id], [HealthDirectoryId], [HealthDistrictCode], [HealthDistrictName], [HealthDistrictNameAr]) VALUES (146, 17, N'', N'Ehnasia', N'إهناسيا')
INSERT [dbo].[HealthDistricts] ([Id], [HealthDirectoryId], [HealthDistrictCode], [HealthDistrictName], [HealthDistrictNameAr]) VALUES (147, 17, N'', N'beba', N'ببا')
INSERT [dbo].[HealthDistricts] ([Id], [HealthDirectoryId], [HealthDistrictCode], [HealthDistrictName], [HealthDistrictNameAr]) VALUES (148, 17, N'', N'Fashn', N'الفشن')
INSERT [dbo].[HealthDistricts] ([Id], [HealthDirectoryId], [HealthDistrictCode], [HealthDistrictName], [HealthDistrictNameAr]) VALUES (149, 17, N'', N'Somasta', N'سمسطا')
INSERT [dbo].[HealthDistricts] ([Id], [HealthDirectoryId], [HealthDistrictCode], [HealthDistrictName], [HealthDistrictNameAr]) VALUES (150, 18, N'', N'PorSaid', N'بورسعيد')
INSERT [dbo].[HealthDistricts] ([Id], [HealthDirectoryId], [HealthDistrictCode], [HealthDistrictName], [HealthDistrictNameAr]) VALUES (151, 18, N'', N'PorFouad', N'بورفؤاد')
INSERT [dbo].[HealthDistricts] ([Id], [HealthDirectoryId], [HealthDistrictCode], [HealthDistrictName], [HealthDistrictNameAr]) VALUES (152, 19, N'', N'Damietta', N'دمياط')
INSERT [dbo].[HealthDistricts] ([Id], [HealthDirectoryId], [HealthDistrictCode], [HealthDistrictName], [HealthDistrictNameAr]) VALUES (153, 19, N'', N'New Damietta', N'دمياط الجديدة')
INSERT [dbo].[HealthDistricts] ([Id], [HealthDirectoryId], [HealthDistrictCode], [HealthDistrictName], [HealthDistrictNameAr]) VALUES (154, 19, N'', N'Ras El Bar', N'رأس البر')
INSERT [dbo].[HealthDistricts] ([Id], [HealthDirectoryId], [HealthDistrictCode], [HealthDistrictName], [HealthDistrictNameAr]) VALUES (155, 19, N'', N'Faraskour', N'فارسكور')
INSERT [dbo].[HealthDistricts] ([Id], [HealthDirectoryId], [HealthDistrictCode], [HealthDistrictName], [HealthDistrictNameAr]) VALUES (156, 19, N'', N'Zarqa', N'الزرقا')
INSERT [dbo].[HealthDistricts] ([Id], [HealthDirectoryId], [HealthDistrictCode], [HealthDistrictName], [HealthDistrictNameAr]) VALUES (157, 19, N'', N'alsaru', N'السرو')
INSERT [dbo].[HealthDistricts] ([Id], [HealthDirectoryId], [HealthDistrictCode], [HealthDistrictName], [HealthDistrictNameAr]) VALUES (158, 19, N'', N'alruwda', N'الروضة')
INSERT [dbo].[HealthDistricts] ([Id], [HealthDirectoryId], [HealthDistrictCode], [HealthDistrictName], [HealthDistrictNameAr]) VALUES (159, 19, N'', N'Kafr El-Batikh', N'كفر البطيخ')
INSERT [dbo].[HealthDistricts] ([Id], [HealthDirectoryId], [HealthDistrictCode], [HealthDistrictName], [HealthDistrictNameAr]) VALUES (160, 19, N'', N'Azbet Al Burg', N'عزبة البرج')
INSERT [dbo].[HealthDistricts] ([Id], [HealthDirectoryId], [HealthDistrictCode], [HealthDistrictName], [HealthDistrictNameAr]) VALUES (161, 19, N'', N'Meet Abou Ghalib', N'ميت أبو غالب')
INSERT [dbo].[HealthDistricts] ([Id], [HealthDirectoryId], [HealthDistrictCode], [HealthDistrictName], [HealthDistrictNameAr]) VALUES (162, 19, N'', N'Kafr Saad', N'كفر سعد')
INSERT [dbo].[HealthDistricts] ([Id], [HealthDirectoryId], [HealthDistrictCode], [HealthDistrictName], [HealthDistrictNameAr]) VALUES (163, 20, N'', N'Zagazig', N'الزقازيق')
INSERT [dbo].[HealthDistricts] ([Id], [HealthDirectoryId], [HealthDistrictCode], [HealthDistrictName], [HealthDistrictNameAr]) VALUES (164, 20, N'', N'Al Ashr Men Ramadan', N'العاشر من رمضان')
INSERT [dbo].[HealthDistricts] ([Id], [HealthDirectoryId], [HealthDistrictCode], [HealthDistrictName], [HealthDistrictNameAr]) VALUES (165, 20, N'', N'Minya Al Qamh', N'منيا القمح')
INSERT [dbo].[HealthDistricts] ([Id], [HealthDirectoryId], [HealthDistrictCode], [HealthDistrictName], [HealthDistrictNameAr]) VALUES (166, 20, N'', N'Belbeis', N'بلبيس')
INSERT [dbo].[HealthDistricts] ([Id], [HealthDirectoryId], [HealthDistrictCode], [HealthDistrictName], [HealthDistrictNameAr]) VALUES (167, 20, N'', N'Mashtoul El Souq', N'مشتول السوق')
INSERT [dbo].[HealthDistricts] ([Id], [HealthDirectoryId], [HealthDistrictCode], [HealthDistrictName], [HealthDistrictNameAr]) VALUES (168, 20, N'', N'Qenaiat', N'القنايات')
INSERT [dbo].[HealthDistricts] ([Id], [HealthDirectoryId], [HealthDistrictCode], [HealthDistrictName], [HealthDistrictNameAr]) VALUES (169, 20, N'', N'Abu Hammad', N'أبو حماد')
INSERT [dbo].[HealthDistricts] ([Id], [HealthDirectoryId], [HealthDistrictCode], [HealthDistrictName], [HealthDistrictNameAr]) VALUES (170, 20, N'', N'El Qurain', N'القرين')
INSERT [dbo].[HealthDistricts] ([Id], [HealthDirectoryId], [HealthDistrictCode], [HealthDistrictName], [HealthDistrictNameAr]) VALUES (171, 20, N'', N'Hehia', N'ههيا')
INSERT [dbo].[HealthDistricts] ([Id], [HealthDirectoryId], [HealthDistrictCode], [HealthDistrictName], [HealthDistrictNameAr]) VALUES (172, 20, N'', N'Abu Kabir', N'أبو كبير')
INSERT [dbo].[HealthDistricts] ([Id], [HealthDirectoryId], [HealthDistrictCode], [HealthDistrictName], [HealthDistrictNameAr]) VALUES (173, 20, N'', N'Faccus', N'فاقوس')
INSERT [dbo].[HealthDistricts] ([Id], [HealthDirectoryId], [HealthDistrictCode], [HealthDistrictName], [HealthDistrictNameAr]) VALUES (174, 20, N'', N'El Salihia El Gedida', N'الصالحية الجديدة')
INSERT [dbo].[HealthDistricts] ([Id], [HealthDirectoryId], [HealthDistrictCode], [HealthDistrictName], [HealthDistrictNameAr]) VALUES (175, 20, N'', N'Al Ibrahimiyah', N'الإبراهيمية')
INSERT [dbo].[HealthDistricts] ([Id], [HealthDirectoryId], [HealthDistrictCode], [HealthDistrictName], [HealthDistrictNameAr]) VALUES (176, 20, N'', N'Deirb Negm', N'ديرب نجم')
INSERT [dbo].[HealthDistricts] ([Id], [HealthDirectoryId], [HealthDistrictCode], [HealthDistrictName], [HealthDistrictNameAr]) VALUES (177, 20, N'', N'Kafr Saqr', N'كفر صقر')
INSERT [dbo].[HealthDistricts] ([Id], [HealthDirectoryId], [HealthDistrictCode], [HealthDistrictName], [HealthDistrictNameAr]) VALUES (178, 20, N'', N'Awlad Saqr', N'أولاد صقر')
INSERT [dbo].[HealthDistricts] ([Id], [HealthDirectoryId], [HealthDistrictCode], [HealthDistrictName], [HealthDistrictNameAr]) VALUES (179, 20, N'', N'Husseiniya', N'الحسينية')
INSERT [dbo].[HealthDistricts] ([Id], [HealthDirectoryId], [HealthDistrictCode], [HealthDistrictName], [HealthDistrictNameAr]) VALUES (180, 20, N'', N'san alhajar alqablia', N'صان الحجر القبلية')
INSERT [dbo].[HealthDistricts] ([Id], [HealthDirectoryId], [HealthDistrictCode], [HealthDistrictName], [HealthDistrictNameAr]) VALUES (181, 20, N'', N'Manshayat Abu Omar', N'منشأة أبو عمر')
INSERT [dbo].[HealthDistricts] ([Id], [HealthDirectoryId], [HealthDistrictCode], [HealthDistrictName], [HealthDistrictNameAr]) VALUES (182, 21, N'', N'Al Toor', N'الطور')
INSERT [dbo].[HealthDistricts] ([Id], [HealthDirectoryId], [HealthDistrictCode], [HealthDistrictName], [HealthDistrictNameAr]) VALUES (183, 21, N'', N'Sharm El-Shaikh', N'شرم الشيخ')
INSERT [dbo].[HealthDistricts] ([Id], [HealthDirectoryId], [HealthDistrictCode], [HealthDistrictName], [HealthDistrictNameAr]) VALUES (184, 21, N'', N'Dahab', N'دهب')
INSERT [dbo].[HealthDistricts] ([Id], [HealthDirectoryId], [HealthDistrictCode], [HealthDistrictName], [HealthDistrictNameAr]) VALUES (185, 21, N'', N'Nuweiba', N'نويبع')
INSERT [dbo].[HealthDistricts] ([Id], [HealthDirectoryId], [HealthDistrictCode], [HealthDistrictName], [HealthDistrictNameAr]) VALUES (186, 21, N'', N'Taba', N'طابا')
INSERT [dbo].[HealthDistricts] ([Id], [HealthDirectoryId], [HealthDistrictCode], [HealthDistrictName], [HealthDistrictNameAr]) VALUES (187, 21, N'', N'Saint Catherine', N'سانت كاترين')
INSERT [dbo].[HealthDistricts] ([Id], [HealthDirectoryId], [HealthDistrictCode], [HealthDistrictName], [HealthDistrictNameAr]) VALUES (188, 21, N'', N'Abu Redis', N'أبو رديس')
INSERT [dbo].[HealthDistricts] ([Id], [HealthDirectoryId], [HealthDistrictCode], [HealthDistrictName], [HealthDistrictNameAr]) VALUES (189, 21, N'', N'Abu Zenaima', N'أبو زنيمة')
INSERT [dbo].[HealthDistricts] ([Id], [HealthDirectoryId], [HealthDistrictCode], [HealthDistrictName], [HealthDistrictNameAr]) VALUES (190, 21, N'', N'Ras Sidr', N'رأس سدر')
INSERT [dbo].[HealthDistricts] ([Id], [HealthDirectoryId], [HealthDistrictCode], [HealthDistrictName], [HealthDistrictNameAr]) VALUES (191, 22, N'', N'Kafr El Sheikh', N'كفر الشيخ')
INSERT [dbo].[HealthDistricts] ([Id], [HealthDirectoryId], [HealthDistrictCode], [HealthDistrictName], [HealthDistrictNameAr]) VALUES (192, 22, N'', N'Desouq', N'دسوق')
INSERT [dbo].[HealthDistricts] ([Id], [HealthDirectoryId], [HealthDistrictCode], [HealthDistrictName], [HealthDistrictNameAr]) VALUES (193, 22, N'', N'Fooh', N'فوه')
INSERT [dbo].[HealthDistricts] ([Id], [HealthDirectoryId], [HealthDistrictCode], [HealthDistrictName], [HealthDistrictNameAr]) VALUES (194, 22, N'', N'Metobas', N'مطوبس')
INSERT [dbo].[HealthDistricts] ([Id], [HealthDirectoryId], [HealthDistrictCode], [HealthDistrictName], [HealthDistrictNameAr]) VALUES (195, 22, N'', N'Baltim', N'بلطيم')
INSERT [dbo].[HealthDistricts] ([Id], [HealthDirectoryId], [HealthDistrictCode], [HealthDistrictName], [HealthDistrictNameAr]) VALUES (196, 22, N'', N'Masief Baltim', N'مصيف بلطيم')
INSERT [dbo].[HealthDistricts] ([Id], [HealthDirectoryId], [HealthDistrictCode], [HealthDistrictName], [HealthDistrictNameAr]) VALUES (197, 22, N'', N'Hamol', N'الحامول')
INSERT [dbo].[HealthDistricts] ([Id], [HealthDirectoryId], [HealthDistrictCode], [HealthDistrictName], [HealthDistrictNameAr]) VALUES (198, 22, N'', N'Bella', N'بيلا')
INSERT [dbo].[HealthDistricts] ([Id], [HealthDirectoryId], [HealthDistrictCode], [HealthDistrictName], [HealthDistrictNameAr]) VALUES (199, 22, N'', N'Riyadh', N'الرياض')
GO
INSERT [dbo].[HealthDistricts] ([Id], [HealthDirectoryId], [HealthDistrictCode], [HealthDistrictName], [HealthDistrictNameAr]) VALUES (200, 22, N'', N'Sidi Salm', N'سيدي سالم')
SET IDENTITY_INSERT [dbo].[HealthDistricts] OFF
SET IDENTITY_INSERT [dbo].[Manufacturers] ON 

INSERT [dbo].[Manufacturers] ([Id], [ManufacturerName], [ManufacturerNameAr]) VALUES (1, N'united Kingdom', N'انجلترا')
SET IDENTITY_INSERT [dbo].[Manufacturers] OFF
SET IDENTITY_INSERT [dbo].[masterEquipments] ON 

INSERT [dbo].[masterEquipments] ([Id], [Name], [NameAr], [MasterCode], [EquipmentCategoryId], [EquipmentSubCategoryId], [ManufacturerId], [PriorityId], [EquipmentDescription], [EquipmentDescriptionAr], [ExpectedLifeTime], [UpaCode], [ModelNumber], [VersionNumber], [OriginId]) VALUES (1, N'dump', N'مضخات', N'a1ww', 3, NULL, 1, 1, NULL, N'', 5, N'iiiiiiiiiii', N'11', N'2', 1)
SET IDENTITY_INSERT [dbo].[masterEquipments] OFF
SET IDENTITY_INSERT [dbo].[organizations] ON 

INSERT [dbo].[organizations] ([Id], [Name], [NameAr]) VALUES (1, N'yyy', NULL)
INSERT [dbo].[organizations] ([Id], [Name], [NameAr]) VALUES (2, N'pppppppppppp', NULL)
SET IDENTITY_INSERT [dbo].[organizations] OFF
SET IDENTITY_INSERT [dbo].[Origins] ON 

INSERT [dbo].[Origins] ([Id], [OriginCode], [ArabicName], [EnglishName]) VALUES (1, N'127', NULL, NULL)
INSERT [dbo].[Origins] ([Id], [OriginCode], [ArabicName], [EnglishName]) VALUES (2, N'AF', N'Afghanistan', N'أفغانستان')
INSERT [dbo].[Origins] ([Id], [OriginCode], [ArabicName], [EnglishName]) VALUES (3, N'AL', N'Albania', N'ألبانيا')
INSERT [dbo].[Origins] ([Id], [OriginCode], [ArabicName], [EnglishName]) VALUES (4, N'DZ', N'Algeria', N'الجزائر')
INSERT [dbo].[Origins] ([Id], [OriginCode], [ArabicName], [EnglishName]) VALUES (5, N'AS', N'American Samoa', N'American Samoa')
SET IDENTITY_INSERT [dbo].[Origins] OFF
SET IDENTITY_INSERT [dbo].[Priority] ON 

INSERT [dbo].[Priority] ([Id], [PriorityLevel], [PriorityLevelAr], [Description], [DescriptionAr]) VALUES (1, N'high', N';;p', N'', N'')
SET IDENTITY_INSERT [dbo].[Priority] OFF
SET IDENTITY_INSERT [dbo].[Suppliers] ON 

INSERT [dbo].[Suppliers] ([Id], [SupplierCode], [SupplierName], [SupplierNameAr]) VALUES (1, N'', N'new', N'جديد2')
SET IDENTITY_INSERT [dbo].[Suppliers] OFF
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_AspNetRoleClaims_RoleId]    Script Date: 8/2/2021 4:44:10 PM ******/
CREATE NONCLUSTERED INDEX [IX_AspNetRoleClaims_RoleId] ON [dbo].[AspNetRoleClaims]
(
	[RoleId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [RoleNameIndex]    Script Date: 8/2/2021 4:44:10 PM ******/
CREATE UNIQUE NONCLUSTERED INDEX [RoleNameIndex] ON [dbo].[AspNetRoles]
(
	[NormalizedName] ASC
)
WHERE ([NormalizedName] IS NOT NULL)
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_AspNetUserClaims_UserId]    Script Date: 8/2/2021 4:44:10 PM ******/
CREATE NONCLUSTERED INDEX [IX_AspNetUserClaims_UserId] ON [dbo].[AspNetUserClaims]
(
	[UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_AspNetUserLogins_UserId]    Script Date: 8/2/2021 4:44:10 PM ******/
CREATE NONCLUSTERED INDEX [IX_AspNetUserLogins_UserId] ON [dbo].[AspNetUserLogins]
(
	[UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_AspNetUserRoles_RoleId]    Script Date: 8/2/2021 4:44:10 PM ******/
CREATE NONCLUSTERED INDEX [IX_AspNetUserRoles_RoleId] ON [dbo].[AspNetUserRoles]
(
	[RoleId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [EmailIndex]    Script Date: 8/2/2021 4:44:10 PM ******/
CREATE NONCLUSTERED INDEX [EmailIndex] ON [dbo].[AspNetUsers]
(
	[NormalizedEmail] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_AspNetUsers_HealthCareUnitId]    Script Date: 8/2/2021 4:44:10 PM ******/
CREATE NONCLUSTERED INDEX [IX_AspNetUsers_HealthCareUnitId] ON [dbo].[AspNetUsers]
(
	[HealthCareUnitId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_AspNetUsers_HealthdirId]    Script Date: 8/2/2021 4:44:10 PM ******/
CREATE NONCLUSTERED INDEX [IX_AspNetUsers_HealthdirId] ON [dbo].[AspNetUsers]
(
	[HealthdirId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_AspNetUsers_HealthDistrictId]    Script Date: 8/2/2021 4:44:10 PM ******/
CREATE NONCLUSTERED INDEX [IX_AspNetUsers_HealthDistrictId] ON [dbo].[AspNetUsers]
(
	[HealthDistrictId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_AspNetUsers_OrganizationId]    Script Date: 8/2/2021 4:44:10 PM ******/
CREATE NONCLUSTERED INDEX [IX_AspNetUsers_OrganizationId] ON [dbo].[AspNetUsers]
(
	[OrganizationId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [UserNameIndex]    Script Date: 8/2/2021 4:44:10 PM ******/
CREATE UNIQUE NONCLUSTERED INDEX [UserNameIndex] ON [dbo].[AspNetUsers]
(
	[NormalizedUserName] ASC
)
WHERE ([NormalizedUserName] IS NOT NULL)
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_Contracts_HealthCareUnitId]    Script Date: 8/2/2021 4:44:10 PM ******/
CREATE NONCLUSTERED INDEX [IX_Contracts_HealthCareUnitId] ON [dbo].[Contracts]
(
	[HealthCareUnitId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_Contracts_ManufacturerId]    Script Date: 8/2/2021 4:44:10 PM ******/
CREATE NONCLUSTERED INDEX [IX_Contracts_ManufacturerId] ON [dbo].[Contracts]
(
	[ManufacturerId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_Contracts_SupplierId]    Script Date: 8/2/2021 4:44:10 PM ******/
CREATE NONCLUSTERED INDEX [IX_Contracts_SupplierId] ON [dbo].[Contracts]
(
	[SupplierId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_Employee_HealthCareUnitId]    Script Date: 8/2/2021 4:44:10 PM ******/
CREATE NONCLUSTERED INDEX [IX_Employee_HealthCareUnitId] ON [dbo].[Employee]
(
	[HealthCareUnitId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_Employee_HealthDirectoryId]    Script Date: 8/2/2021 4:44:10 PM ******/
CREATE NONCLUSTERED INDEX [IX_Employee_HealthDirectoryId] ON [dbo].[Employee]
(
	[HealthDirectoryId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_Employee_HealthDistrictId]    Script Date: 8/2/2021 4:44:10 PM ******/
CREATE NONCLUSTERED INDEX [IX_Employee_HealthDistrictId] ON [dbo].[Employee]
(
	[HealthDistrictId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_Equiments_DepartmentId]    Script Date: 8/2/2021 4:44:10 PM ******/
CREATE NONCLUSTERED INDEX [IX_Equiments_DepartmentId] ON [dbo].[Equiments]
(
	[DepartmentId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_Equiments_EquipmentStatuSId]    Script Date: 8/2/2021 4:44:10 PM ******/
CREATE NONCLUSTERED INDEX [IX_Equiments_EquipmentStatuSId] ON [dbo].[Equiments]
(
	[EquipmentStatuSId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_Equiments_HealthCareUnitId]    Script Date: 8/2/2021 4:44:10 PM ******/
CREATE NONCLUSTERED INDEX [IX_Equiments_HealthCareUnitId] ON [dbo].[Equiments]
(
	[HealthCareUnitId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_Equiments_HealthDirectoryId]    Script Date: 8/2/2021 4:44:10 PM ******/
CREATE NONCLUSTERED INDEX [IX_Equiments_HealthDirectoryId] ON [dbo].[Equiments]
(
	[HealthDirectoryId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_Equiments_HealthDistrictId]    Script Date: 8/2/2021 4:44:10 PM ******/
CREATE NONCLUSTERED INDEX [IX_Equiments_HealthDistrictId] ON [dbo].[Equiments]
(
	[HealthDistrictId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_Equiments_MasterEquipmentId]    Script Date: 8/2/2021 4:44:10 PM ******/
CREATE NONCLUSTERED INDEX [IX_Equiments_MasterEquipmentId] ON [dbo].[Equiments]
(
	[MasterEquipmentId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_Equiments_SupplierId]    Script Date: 8/2/2021 4:44:10 PM ******/
CREATE NONCLUSTERED INDEX [IX_Equiments_SupplierId] ON [dbo].[Equiments]
(
	[SupplierId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_Equipment_EquipmentCoverage_EquipmentCoverageId]    Script Date: 8/2/2021 4:44:10 PM ******/
CREATE NONCLUSTERED INDEX [IX_Equipment_EquipmentCoverage_EquipmentCoverageId] ON [dbo].[Equipment_EquipmentCoverage]
(
	[EquipmentCoverageId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_Equipment_EquipmentCoverage_EquipmentId]    Script Date: 8/2/2021 4:44:10 PM ******/
CREATE NONCLUSTERED INDEX [IX_Equipment_EquipmentCoverage_EquipmentId] ON [dbo].[Equipment_EquipmentCoverage]
(
	[EquipmentId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_EquipmentCategories_HealthCareUnitId]    Script Date: 8/2/2021 4:44:10 PM ******/
CREATE NONCLUSTERED INDEX [IX_EquipmentCategories_HealthCareUnitId] ON [dbo].[EquipmentCategories]
(
	[HealthCareUnitId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_EquipmentCoverages_ContractId]    Script Date: 8/2/2021 4:44:10 PM ******/
CREATE NONCLUSTERED INDEX [IX_EquipmentCoverages_ContractId] ON [dbo].[EquipmentCoverages]
(
	[ContractId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_EquipmentCoverages_SparePartId]    Script Date: 8/2/2021 4:44:10 PM ******/
CREATE NONCLUSTERED INDEX [IX_EquipmentCoverages_SparePartId] ON [dbo].[EquipmentCoverages]
(
	[SparePartId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_equipmentEmployees_EquipmentId]    Script Date: 8/2/2021 4:44:10 PM ******/
CREATE NONCLUSTERED INDEX [IX_equipmentEmployees_EquipmentId] ON [dbo].[equipmentEmployees]
(
	[EquipmentId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_equipmentEmployees_UserId]    Script Date: 8/2/2021 4:44:10 PM ******/
CREATE NONCLUSTERED INDEX [IX_equipmentEmployees_UserId] ON [dbo].[equipmentEmployees]
(
	[UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_EquipmentSubCategories_EquipmentCategoryId]    Script Date: 8/2/2021 4:44:10 PM ******/
CREATE NONCLUSTERED INDEX [IX_EquipmentSubCategories_EquipmentCategoryId] ON [dbo].[EquipmentSubCategories]
(
	[EquipmentCategoryId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_HealthCareUnits_HealthDirectoryId]    Script Date: 8/2/2021 4:44:10 PM ******/
CREATE NONCLUSTERED INDEX [IX_HealthCareUnits_HealthDirectoryId] ON [dbo].[HealthCareUnits]
(
	[HealthDirectoryId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_HealthCareUnits_HealthDistrictId]    Script Date: 8/2/2021 4:44:10 PM ******/
CREATE NONCLUSTERED INDEX [IX_HealthCareUnits_HealthDistrictId] ON [dbo].[HealthCareUnits]
(
	[HealthDistrictId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_HealthCareUnits_organizationId]    Script Date: 8/2/2021 4:44:10 PM ******/
CREATE NONCLUSTERED INDEX [IX_HealthCareUnits_organizationId] ON [dbo].[HealthCareUnits]
(
	[organizationId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_HealthDistricts_HealthDirectoryId]    Script Date: 8/2/2021 4:44:10 PM ******/
CREATE NONCLUSTERED INDEX [IX_HealthDistricts_HealthDirectoryId] ON [dbo].[HealthDistricts]
(
	[HealthDirectoryId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_masterEquipments_EquipmentCategoryId]    Script Date: 8/2/2021 4:44:10 PM ******/
CREATE NONCLUSTERED INDEX [IX_masterEquipments_EquipmentCategoryId] ON [dbo].[masterEquipments]
(
	[EquipmentCategoryId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_masterEquipments_EquipmentSubCategoryId]    Script Date: 8/2/2021 4:44:10 PM ******/
CREATE NONCLUSTERED INDEX [IX_masterEquipments_EquipmentSubCategoryId] ON [dbo].[masterEquipments]
(
	[EquipmentSubCategoryId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_masterEquipments_ManufacturerId]    Script Date: 8/2/2021 4:44:10 PM ******/
CREATE NONCLUSTERED INDEX [IX_masterEquipments_ManufacturerId] ON [dbo].[masterEquipments]
(
	[ManufacturerId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_masterEquipments_OriginId]    Script Date: 8/2/2021 4:44:10 PM ******/
CREATE NONCLUSTERED INDEX [IX_masterEquipments_OriginId] ON [dbo].[masterEquipments]
(
	[OriginId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_masterEquipments_PriorityId]    Script Date: 8/2/2021 4:44:10 PM ******/
CREATE NONCLUSTERED INDEX [IX_masterEquipments_PriorityId] ON [dbo].[masterEquipments]
(
	[PriorityId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_ServiceRequest_EmployeeId]    Script Date: 8/2/2021 4:44:10 PM ******/
CREATE NONCLUSTERED INDEX [IX_ServiceRequest_EmployeeId] ON [dbo].[ServiceRequest]
(
	[EmployeeId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_ServiceRequest_EquipmentId]    Script Date: 8/2/2021 4:44:10 PM ******/
CREATE NONCLUSTERED INDEX [IX_ServiceRequest_EquipmentId] ON [dbo].[ServiceRequest]
(
	[EquipmentId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_ServiceRequest_HealthDirectoryId]    Script Date: 8/2/2021 4:44:10 PM ******/
CREATE NONCLUSTERED INDEX [IX_ServiceRequest_HealthDirectoryId] ON [dbo].[ServiceRequest]
(
	[HealthDirectoryId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_ServiceRequest_HealthDistrictId]    Script Date: 8/2/2021 4:44:10 PM ******/
CREATE NONCLUSTERED INDEX [IX_ServiceRequest_HealthDistrictId] ON [dbo].[ServiceRequest]
(
	[HealthDistrictId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_ServiceRequest_ModeId]    Script Date: 8/2/2021 4:44:10 PM ******/
CREATE NONCLUSTERED INDEX [IX_ServiceRequest_ModeId] ON [dbo].[ServiceRequest]
(
	[ModeId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_ServiceRequest_PriorityId]    Script Date: 8/2/2021 4:44:10 PM ******/
CREATE NONCLUSTERED INDEX [IX_ServiceRequest_PriorityId] ON [dbo].[ServiceRequest]
(
	[PriorityId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_WorkOrders_EmployeeId]    Script Date: 8/2/2021 4:44:10 PM ******/
CREATE NONCLUSTERED INDEX [IX_WorkOrders_EmployeeId] ON [dbo].[WorkOrders]
(
	[EmployeeId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_WorkOrders_EquipmentId]    Script Date: 8/2/2021 4:44:10 PM ******/
CREATE NONCLUSTERED INDEX [IX_WorkOrders_EquipmentId] ON [dbo].[WorkOrders]
(
	[EquipmentId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_WorkOrders_MaintenanceId]    Script Date: 8/2/2021 4:44:10 PM ******/
CREATE NONCLUSTERED INDEX [IX_WorkOrders_MaintenanceId] ON [dbo].[WorkOrders]
(
	[MaintenanceId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_WorkOrders_PriorityId]    Script Date: 8/2/2021 4:44:10 PM ******/
CREATE NONCLUSTERED INDEX [IX_WorkOrders_PriorityId] ON [dbo].[WorkOrders]
(
	[PriorityId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_WorkOrders_RequestStatusId]    Script Date: 8/2/2021 4:44:10 PM ******/
CREATE NONCLUSTERED INDEX [IX_WorkOrders_RequestStatusId] ON [dbo].[WorkOrders]
(
	[RequestStatusId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_WorkOrders_ServiceRequestId]    Script Date: 8/2/2021 4:44:10 PM ******/
CREATE NONCLUSTERED INDEX [IX_WorkOrders_ServiceRequestId] ON [dbo].[WorkOrders]
(
	[ServiceRequestId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_WorkOrders_SparePartId]    Script Date: 8/2/2021 4:44:10 PM ******/
CREATE NONCLUSTERED INDEX [IX_WorkOrders_SparePartId] ON [dbo].[WorkOrders]
(
	[SparePartId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_WorkOrders_VendorId]    Script Date: 8/2/2021 4:44:10 PM ******/
CREATE NONCLUSTERED INDEX [IX_WorkOrders_VendorId] ON [dbo].[WorkOrders]
(
	[VendorId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
ALTER TABLE [dbo].[AspNetRoleClaims]  WITH CHECK ADD  CONSTRAINT [FK_AspNetRoleClaims_AspNetRoles_RoleId] FOREIGN KEY([RoleId])
REFERENCES [dbo].[AspNetRoles] ([Id])
GO
ALTER TABLE [dbo].[AspNetRoleClaims] CHECK CONSTRAINT [FK_AspNetRoleClaims_AspNetRoles_RoleId]
GO
ALTER TABLE [dbo].[AspNetUserClaims]  WITH CHECK ADD  CONSTRAINT [FK_AspNetUserClaims_AspNetUsers_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[AspNetUsers] ([Id])
GO
ALTER TABLE [dbo].[AspNetUserClaims] CHECK CONSTRAINT [FK_AspNetUserClaims_AspNetUsers_UserId]
GO
ALTER TABLE [dbo].[AspNetUserLogins]  WITH CHECK ADD  CONSTRAINT [FK_AspNetUserLogins_AspNetUsers_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[AspNetUsers] ([Id])
GO
ALTER TABLE [dbo].[AspNetUserLogins] CHECK CONSTRAINT [FK_AspNetUserLogins_AspNetUsers_UserId]
GO
ALTER TABLE [dbo].[AspNetUserRoles]  WITH CHECK ADD  CONSTRAINT [FK_AspNetUserRoles_AspNetRoles_RoleId] FOREIGN KEY([RoleId])
REFERENCES [dbo].[AspNetRoles] ([Id])
GO
ALTER TABLE [dbo].[AspNetUserRoles] CHECK CONSTRAINT [FK_AspNetUserRoles_AspNetRoles_RoleId]
GO
ALTER TABLE [dbo].[AspNetUserRoles]  WITH CHECK ADD  CONSTRAINT [FK_AspNetUserRoles_AspNetUsers_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[AspNetUsers] ([Id])
GO
ALTER TABLE [dbo].[AspNetUserRoles] CHECK CONSTRAINT [FK_AspNetUserRoles_AspNetUsers_UserId]
GO
ALTER TABLE [dbo].[AspNetUsers]  WITH CHECK ADD  CONSTRAINT [FK_AspNetUsers_HealthCareUnits_HealthCareUnitId] FOREIGN KEY([HealthCareUnitId])
REFERENCES [dbo].[HealthCareUnits] ([Id])
GO
ALTER TABLE [dbo].[AspNetUsers] CHECK CONSTRAINT [FK_AspNetUsers_HealthCareUnits_HealthCareUnitId]
GO
ALTER TABLE [dbo].[AspNetUsers]  WITH CHECK ADD  CONSTRAINT [FK_AspNetUsers_HealthDirectories_HealthdirId] FOREIGN KEY([HealthdirId])
REFERENCES [dbo].[HealthDirectories] ([Id])
GO
ALTER TABLE [dbo].[AspNetUsers] CHECK CONSTRAINT [FK_AspNetUsers_HealthDirectories_HealthdirId]
GO
ALTER TABLE [dbo].[AspNetUsers]  WITH CHECK ADD  CONSTRAINT [FK_AspNetUsers_HealthDistricts_HealthDistrictId] FOREIGN KEY([HealthDistrictId])
REFERENCES [dbo].[HealthDistricts] ([Id])
GO
ALTER TABLE [dbo].[AspNetUsers] CHECK CONSTRAINT [FK_AspNetUsers_HealthDistricts_HealthDistrictId]
GO
ALTER TABLE [dbo].[AspNetUsers]  WITH CHECK ADD  CONSTRAINT [FK_AspNetUsers_organizations_OrganizationId] FOREIGN KEY([OrganizationId])
REFERENCES [dbo].[organizations] ([Id])
GO
ALTER TABLE [dbo].[AspNetUsers] CHECK CONSTRAINT [FK_AspNetUsers_organizations_OrganizationId]
GO
ALTER TABLE [dbo].[AspNetUserTokens]  WITH CHECK ADD  CONSTRAINT [FK_AspNetUserTokens_AspNetUsers_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[AspNetUsers] ([Id])
GO
ALTER TABLE [dbo].[AspNetUserTokens] CHECK CONSTRAINT [FK_AspNetUserTokens_AspNetUsers_UserId]
GO
ALTER TABLE [dbo].[Contracts]  WITH CHECK ADD  CONSTRAINT [FK_Contracts_HealthCareUnits_HealthCareUnitId] FOREIGN KEY([HealthCareUnitId])
REFERENCES [dbo].[HealthCareUnits] ([Id])
GO
ALTER TABLE [dbo].[Contracts] CHECK CONSTRAINT [FK_Contracts_HealthCareUnits_HealthCareUnitId]
GO
ALTER TABLE [dbo].[Contracts]  WITH CHECK ADD  CONSTRAINT [FK_Contracts_Manufacturers_ManufacturerId] FOREIGN KEY([ManufacturerId])
REFERENCES [dbo].[Manufacturers] ([Id])
GO
ALTER TABLE [dbo].[Contracts] CHECK CONSTRAINT [FK_Contracts_Manufacturers_ManufacturerId]
GO
ALTER TABLE [dbo].[Contracts]  WITH CHECK ADD  CONSTRAINT [FK_Contracts_Suppliers_SupplierId] FOREIGN KEY([SupplierId])
REFERENCES [dbo].[Suppliers] ([Id])
GO
ALTER TABLE [dbo].[Contracts] CHECK CONSTRAINT [FK_Contracts_Suppliers_SupplierId]
GO
ALTER TABLE [dbo].[Employee]  WITH CHECK ADD  CONSTRAINT [FK_Employee_HealthCareUnits_HealthCareUnitId] FOREIGN KEY([HealthCareUnitId])
REFERENCES [dbo].[HealthCareUnits] ([Id])
GO
ALTER TABLE [dbo].[Employee] CHECK CONSTRAINT [FK_Employee_HealthCareUnits_HealthCareUnitId]
GO
ALTER TABLE [dbo].[Employee]  WITH CHECK ADD  CONSTRAINT [FK_Employee_HealthDirectories_HealthDirectoryId] FOREIGN KEY([HealthDirectoryId])
REFERENCES [dbo].[HealthDirectories] ([Id])
GO
ALTER TABLE [dbo].[Employee] CHECK CONSTRAINT [FK_Employee_HealthDirectories_HealthDirectoryId]
GO
ALTER TABLE [dbo].[Employee]  WITH CHECK ADD  CONSTRAINT [FK_Employee_HealthDistricts_HealthDistrictId] FOREIGN KEY([HealthDistrictId])
REFERENCES [dbo].[HealthDistricts] ([Id])
GO
ALTER TABLE [dbo].[Employee] CHECK CONSTRAINT [FK_Employee_HealthDistricts_HealthDistrictId]
GO
ALTER TABLE [dbo].[Equiments]  WITH CHECK ADD  CONSTRAINT [FK_Equiments_Departments_DepartmentId] FOREIGN KEY([DepartmentId])
REFERENCES [dbo].[Departments] ([Id])
GO
ALTER TABLE [dbo].[Equiments] CHECK CONSTRAINT [FK_Equiments_Departments_DepartmentId]
GO
ALTER TABLE [dbo].[Equiments]  WITH CHECK ADD  CONSTRAINT [FK_Equiments_EquipmentStatus_EquipmentStatuSId] FOREIGN KEY([EquipmentStatuSId])
REFERENCES [dbo].[EquipmentStatus] ([Id])
GO
ALTER TABLE [dbo].[Equiments] CHECK CONSTRAINT [FK_Equiments_EquipmentStatus_EquipmentStatuSId]
GO
ALTER TABLE [dbo].[Equiments]  WITH CHECK ADD  CONSTRAINT [FK_Equiments_HealthCareUnits_HealthCareUnitId] FOREIGN KEY([HealthCareUnitId])
REFERENCES [dbo].[HealthCareUnits] ([Id])
GO
ALTER TABLE [dbo].[Equiments] CHECK CONSTRAINT [FK_Equiments_HealthCareUnits_HealthCareUnitId]
GO
ALTER TABLE [dbo].[Equiments]  WITH CHECK ADD  CONSTRAINT [FK_Equiments_HealthDirectories_HealthDirectoryId] FOREIGN KEY([HealthDirectoryId])
REFERENCES [dbo].[HealthDirectories] ([Id])
GO
ALTER TABLE [dbo].[Equiments] CHECK CONSTRAINT [FK_Equiments_HealthDirectories_HealthDirectoryId]
GO
ALTER TABLE [dbo].[Equiments]  WITH CHECK ADD  CONSTRAINT [FK_Equiments_HealthDistricts_HealthDistrictId] FOREIGN KEY([HealthDistrictId])
REFERENCES [dbo].[HealthDistricts] ([Id])
GO
ALTER TABLE [dbo].[Equiments] CHECK CONSTRAINT [FK_Equiments_HealthDistricts_HealthDistrictId]
GO
ALTER TABLE [dbo].[Equiments]  WITH CHECK ADD  CONSTRAINT [FK_Equiments_masterEquipments_MasterEquipmentId] FOREIGN KEY([MasterEquipmentId])
REFERENCES [dbo].[masterEquipments] ([Id])
GO
ALTER TABLE [dbo].[Equiments] CHECK CONSTRAINT [FK_Equiments_masterEquipments_MasterEquipmentId]
GO
ALTER TABLE [dbo].[Equiments]  WITH CHECK ADD  CONSTRAINT [FK_Equiments_Suppliers_SupplierId] FOREIGN KEY([SupplierId])
REFERENCES [dbo].[Suppliers] ([Id])
GO
ALTER TABLE [dbo].[Equiments] CHECK CONSTRAINT [FK_Equiments_Suppliers_SupplierId]
GO
ALTER TABLE [dbo].[Equipment_EquipmentCoverage]  WITH CHECK ADD  CONSTRAINT [FK_Equipment_EquipmentCoverage_Equiments_EquipmentId] FOREIGN KEY([EquipmentId])
REFERENCES [dbo].[Equiments] ([Id])
GO
ALTER TABLE [dbo].[Equipment_EquipmentCoverage] CHECK CONSTRAINT [FK_Equipment_EquipmentCoverage_Equiments_EquipmentId]
GO
ALTER TABLE [dbo].[Equipment_EquipmentCoverage]  WITH CHECK ADD  CONSTRAINT [FK_Equipment_EquipmentCoverage_EquipmentCoverages_EquipmentCoverageId] FOREIGN KEY([EquipmentCoverageId])
REFERENCES [dbo].[EquipmentCoverages] ([equipmentCoverageId])
GO
ALTER TABLE [dbo].[Equipment_EquipmentCoverage] CHECK CONSTRAINT [FK_Equipment_EquipmentCoverage_EquipmentCoverages_EquipmentCoverageId]
GO
ALTER TABLE [dbo].[EquipmentAttachments]  WITH CHECK ADD  CONSTRAINT [FK_EquipmentAttachments_Equiments_EquipmentId] FOREIGN KEY([EquipmentId])
REFERENCES [dbo].[Equiments] ([Id])
GO
ALTER TABLE [dbo].[EquipmentAttachments] CHECK CONSTRAINT [FK_EquipmentAttachments_Equiments_EquipmentId]
GO
ALTER TABLE [dbo].[EquipmentCategories]  WITH CHECK ADD  CONSTRAINT [FK_EquipmentCategories_HealthCareUnits_HealthCareUnitId] FOREIGN KEY([HealthCareUnitId])
REFERENCES [dbo].[HealthCareUnits] ([Id])
GO
ALTER TABLE [dbo].[EquipmentCategories] CHECK CONSTRAINT [FK_EquipmentCategories_HealthCareUnits_HealthCareUnitId]
GO
ALTER TABLE [dbo].[EquipmentCoverages]  WITH CHECK ADD  CONSTRAINT [FK_EquipmentCoverages_Contracts_ContractId] FOREIGN KEY([ContractId])
REFERENCES [dbo].[Contracts] ([ContractId])
GO
ALTER TABLE [dbo].[EquipmentCoverages] CHECK CONSTRAINT [FK_EquipmentCoverages_Contracts_ContractId]
GO
ALTER TABLE [dbo].[EquipmentCoverages]  WITH CHECK ADD  CONSTRAINT [FK_EquipmentCoverages_spareParts_SparePartId] FOREIGN KEY([SparePartId])
REFERENCES [dbo].[spareParts] ([Id])
GO
ALTER TABLE [dbo].[EquipmentCoverages] CHECK CONSTRAINT [FK_EquipmentCoverages_spareParts_SparePartId]
GO
ALTER TABLE [dbo].[equipmentEmployees]  WITH CHECK ADD  CONSTRAINT [FK_equipmentEmployees_AspNetUsers_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[AspNetUsers] ([Id])
GO
ALTER TABLE [dbo].[equipmentEmployees] CHECK CONSTRAINT [FK_equipmentEmployees_AspNetUsers_UserId]
GO
ALTER TABLE [dbo].[equipmentEmployees]  WITH CHECK ADD  CONSTRAINT [FK_equipmentEmployees_Equiments_EquipmentId] FOREIGN KEY([EquipmentId])
REFERENCES [dbo].[Equiments] ([Id])
GO
ALTER TABLE [dbo].[equipmentEmployees] CHECK CONSTRAINT [FK_equipmentEmployees_Equiments_EquipmentId]
GO
ALTER TABLE [dbo].[EquipmentSubCategories]  WITH CHECK ADD  CONSTRAINT [FK_EquipmentSubCategories_EquipmentCategories_EquipmentCategoryId] FOREIGN KEY([EquipmentCategoryId])
REFERENCES [dbo].[EquipmentCategories] ([Id])
GO
ALTER TABLE [dbo].[EquipmentSubCategories] CHECK CONSTRAINT [FK_EquipmentSubCategories_EquipmentCategories_EquipmentCategoryId]
GO
ALTER TABLE [dbo].[HealthCareUnits]  WITH CHECK ADD  CONSTRAINT [FK_HealthCareUnits_HealthDirectories_HealthDirectoryId] FOREIGN KEY([HealthDirectoryId])
REFERENCES [dbo].[HealthDirectories] ([Id])
GO
ALTER TABLE [dbo].[HealthCareUnits] CHECK CONSTRAINT [FK_HealthCareUnits_HealthDirectories_HealthDirectoryId]
GO
ALTER TABLE [dbo].[HealthCareUnits]  WITH CHECK ADD  CONSTRAINT [FK_HealthCareUnits_HealthDistricts_HealthDistrictId] FOREIGN KEY([HealthDistrictId])
REFERENCES [dbo].[HealthDistricts] ([Id])
GO
ALTER TABLE [dbo].[HealthCareUnits] CHECK CONSTRAINT [FK_HealthCareUnits_HealthDistricts_HealthDistrictId]
GO
ALTER TABLE [dbo].[HealthCareUnits]  WITH CHECK ADD  CONSTRAINT [FK_HealthCareUnits_organizations_organizationId] FOREIGN KEY([organizationId])
REFERENCES [dbo].[organizations] ([Id])
GO
ALTER TABLE [dbo].[HealthCareUnits] CHECK CONSTRAINT [FK_HealthCareUnits_organizations_organizationId]
GO
ALTER TABLE [dbo].[HealthDistricts]  WITH CHECK ADD  CONSTRAINT [FK_HealthDistricts_HealthDirectories_HealthDirectoryId] FOREIGN KEY([HealthDirectoryId])
REFERENCES [dbo].[HealthDirectories] ([Id])
GO
ALTER TABLE [dbo].[HealthDistricts] CHECK CONSTRAINT [FK_HealthDistricts_HealthDirectories_HealthDirectoryId]
GO
ALTER TABLE [dbo].[masterEquipmentAttachments]  WITH CHECK ADD  CONSTRAINT [FK_masterEquipmentAttachments_masterEquipments_MasterEquipmentId] FOREIGN KEY([MasterEquipmentId])
REFERENCES [dbo].[masterEquipments] ([Id])
GO
ALTER TABLE [dbo].[masterEquipmentAttachments] CHECK CONSTRAINT [FK_masterEquipmentAttachments_masterEquipments_MasterEquipmentId]
GO
ALTER TABLE [dbo].[masterEquipments]  WITH CHECK ADD  CONSTRAINT [FK_masterEquipments_EquipmentCategories_EquipmentCategoryId] FOREIGN KEY([EquipmentCategoryId])
REFERENCES [dbo].[EquipmentCategories] ([Id])
GO
ALTER TABLE [dbo].[masterEquipments] CHECK CONSTRAINT [FK_masterEquipments_EquipmentCategories_EquipmentCategoryId]
GO
ALTER TABLE [dbo].[masterEquipments]  WITH CHECK ADD  CONSTRAINT [FK_masterEquipments_EquipmentSubCategories_EquipmentSubCategoryId] FOREIGN KEY([EquipmentSubCategoryId])
REFERENCES [dbo].[EquipmentSubCategories] ([Id])
GO
ALTER TABLE [dbo].[masterEquipments] CHECK CONSTRAINT [FK_masterEquipments_EquipmentSubCategories_EquipmentSubCategoryId]
GO
ALTER TABLE [dbo].[masterEquipments]  WITH CHECK ADD  CONSTRAINT [FK_masterEquipments_Manufacturers_ManufacturerId] FOREIGN KEY([ManufacturerId])
REFERENCES [dbo].[Manufacturers] ([Id])
GO
ALTER TABLE [dbo].[masterEquipments] CHECK CONSTRAINT [FK_masterEquipments_Manufacturers_ManufacturerId]
GO
ALTER TABLE [dbo].[masterEquipments]  WITH CHECK ADD  CONSTRAINT [FK_masterEquipments_Origins_OriginId] FOREIGN KEY([OriginId])
REFERENCES [dbo].[Origins] ([Id])
GO
ALTER TABLE [dbo].[masterEquipments] CHECK CONSTRAINT [FK_masterEquipments_Origins_OriginId]
GO
ALTER TABLE [dbo].[masterEquipments]  WITH CHECK ADD  CONSTRAINT [FK_masterEquipments_Priority_PriorityId] FOREIGN KEY([PriorityId])
REFERENCES [dbo].[Priority] ([Id])
GO
ALTER TABLE [dbo].[masterEquipments] CHECK CONSTRAINT [FK_masterEquipments_Priority_PriorityId]
GO
ALTER TABLE [dbo].[ServiceRequest]  WITH CHECK ADD  CONSTRAINT [FK_ServiceRequest_Employee_EmployeeId] FOREIGN KEY([EmployeeId])
REFERENCES [dbo].[Employee] ([Id])
GO
ALTER TABLE [dbo].[ServiceRequest] CHECK CONSTRAINT [FK_ServiceRequest_Employee_EmployeeId]
GO
ALTER TABLE [dbo].[ServiceRequest]  WITH CHECK ADD  CONSTRAINT [FK_ServiceRequest_Equiments_EquipmentId] FOREIGN KEY([EquipmentId])
REFERENCES [dbo].[Equiments] ([Id])
GO
ALTER TABLE [dbo].[ServiceRequest] CHECK CONSTRAINT [FK_ServiceRequest_Equiments_EquipmentId]
GO
ALTER TABLE [dbo].[ServiceRequest]  WITH CHECK ADD  CONSTRAINT [FK_ServiceRequest_HealthDirectories_HealthDirectoryId] FOREIGN KEY([HealthDirectoryId])
REFERENCES [dbo].[HealthDirectories] ([Id])
GO
ALTER TABLE [dbo].[ServiceRequest] CHECK CONSTRAINT [FK_ServiceRequest_HealthDirectories_HealthDirectoryId]
GO
ALTER TABLE [dbo].[ServiceRequest]  WITH CHECK ADD  CONSTRAINT [FK_ServiceRequest_HealthDistricts_HealthDistrictId] FOREIGN KEY([HealthDistrictId])
REFERENCES [dbo].[HealthDistricts] ([Id])
GO
ALTER TABLE [dbo].[ServiceRequest] CHECK CONSTRAINT [FK_ServiceRequest_HealthDistricts_HealthDistrictId]
GO
ALTER TABLE [dbo].[ServiceRequest]  WITH CHECK ADD  CONSTRAINT [FK_ServiceRequest_Modes_ModeId] FOREIGN KEY([ModeId])
REFERENCES [dbo].[Modes] ([Id])
GO
ALTER TABLE [dbo].[ServiceRequest] CHECK CONSTRAINT [FK_ServiceRequest_Modes_ModeId]
GO
ALTER TABLE [dbo].[ServiceRequest]  WITH CHECK ADD  CONSTRAINT [FK_ServiceRequest_Priority_PriorityId] FOREIGN KEY([PriorityId])
REFERENCES [dbo].[Priority] ([Id])
GO
ALTER TABLE [dbo].[ServiceRequest] CHECK CONSTRAINT [FK_ServiceRequest_Priority_PriorityId]
GO
ALTER TABLE [dbo].[serviceRequestAttachments]  WITH CHECK ADD  CONSTRAINT [FK_serviceRequestAttachments_ServiceRequest_ServiceRequestId] FOREIGN KEY([ServiceRequestId])
REFERENCES [dbo].[ServiceRequest] ([Id])
GO
ALTER TABLE [dbo].[serviceRequestAttachments] CHECK CONSTRAINT [FK_serviceRequestAttachments_ServiceRequest_ServiceRequestId]
GO
ALTER TABLE [dbo].[workOrderAttachments]  WITH CHECK ADD  CONSTRAINT [FK_workOrderAttachments_WorkOrders_WorkOrderId] FOREIGN KEY([WorkOrderId])
REFERENCES [dbo].[WorkOrders] ([Id])
GO
ALTER TABLE [dbo].[workOrderAttachments] CHECK CONSTRAINT [FK_workOrderAttachments_WorkOrders_WorkOrderId]
GO
ALTER TABLE [dbo].[WorkOrders]  WITH CHECK ADD  CONSTRAINT [FK_WorkOrders_Employee_EmployeeId] FOREIGN KEY([EmployeeId])
REFERENCES [dbo].[Employee] ([Id])
GO
ALTER TABLE [dbo].[WorkOrders] CHECK CONSTRAINT [FK_WorkOrders_Employee_EmployeeId]
GO
ALTER TABLE [dbo].[WorkOrders]  WITH CHECK ADD  CONSTRAINT [FK_WorkOrders_Equiments_EquipmentId] FOREIGN KEY([EquipmentId])
REFERENCES [dbo].[Equiments] ([Id])
GO
ALTER TABLE [dbo].[WorkOrders] CHECK CONSTRAINT [FK_WorkOrders_Equiments_EquipmentId]
GO
ALTER TABLE [dbo].[WorkOrders]  WITH CHECK ADD  CONSTRAINT [FK_WorkOrders_MaintenanceServices_MaintenanceId] FOREIGN KEY([MaintenanceId])
REFERENCES [dbo].[MaintenanceServices] ([Id])
GO
ALTER TABLE [dbo].[WorkOrders] CHECK CONSTRAINT [FK_WorkOrders_MaintenanceServices_MaintenanceId]
GO
ALTER TABLE [dbo].[WorkOrders]  WITH CHECK ADD  CONSTRAINT [FK_WorkOrders_Priority_PriorityId] FOREIGN KEY([PriorityId])
REFERENCES [dbo].[Priority] ([Id])
GO
ALTER TABLE [dbo].[WorkOrders] CHECK CONSTRAINT [FK_WorkOrders_Priority_PriorityId]
GO
ALTER TABLE [dbo].[WorkOrders]  WITH CHECK ADD  CONSTRAINT [FK_WorkOrders_requestStatuses_RequestStatusId] FOREIGN KEY([RequestStatusId])
REFERENCES [dbo].[requestStatuses] ([Id])
GO
ALTER TABLE [dbo].[WorkOrders] CHECK CONSTRAINT [FK_WorkOrders_requestStatuses_RequestStatusId]
GO
ALTER TABLE [dbo].[WorkOrders]  WITH CHECK ADD  CONSTRAINT [FK_WorkOrders_ServiceRequest_ServiceRequestId] FOREIGN KEY([ServiceRequestId])
REFERENCES [dbo].[ServiceRequest] ([Id])
GO
ALTER TABLE [dbo].[WorkOrders] CHECK CONSTRAINT [FK_WorkOrders_ServiceRequest_ServiceRequestId]
GO
ALTER TABLE [dbo].[WorkOrders]  WITH CHECK ADD  CONSTRAINT [FK_WorkOrders_spareParts_SparePartId] FOREIGN KEY([SparePartId])
REFERENCES [dbo].[spareParts] ([Id])
GO
ALTER TABLE [dbo].[WorkOrders] CHECK CONSTRAINT [FK_WorkOrders_spareParts_SparePartId]
GO
ALTER TABLE [dbo].[WorkOrders]  WITH CHECK ADD  CONSTRAINT [FK_WorkOrders_Vendors_VendorId] FOREIGN KEY([VendorId])
REFERENCES [dbo].[Vendors] ([Id])
GO
ALTER TABLE [dbo].[WorkOrders] CHECK CONSTRAINT [FK_WorkOrders_Vendors_VendorId]
GO
USE [master]
GO
ALTER DATABASE [BiomedicalDB] SET  READ_WRITE 
GO
