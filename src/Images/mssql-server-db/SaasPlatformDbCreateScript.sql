CREATE DATABASE [SaaSPlatform]
 CONTAINMENT = NONE
 WITH CATALOG_COLLATION = DATABASE_DEFAULT
GO

USE [SaaSPlatform]
GO
/****** Object:  Schema [dataIntegration]    Script Date: 7/14/2024 11:21:58 AM ******/
CREATE SCHEMA [dataIntegration]
GO
/****** Object:  Schema [deployment]    Script Date: 7/14/2024 11:21:58 AM ******/
CREATE SCHEMA [deployment]
GO
/****** Object:  Schema [users]    Script Date: 7/14/2024 11:21:58 AM ******/
CREATE SCHEMA [users]
GO
/****** Object:  Table [dataIntegration].[__EFMigrationsHistory]    Script Date: 7/14/2024 11:21:58 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dataIntegration].[__EFMigrationsHistory](
	[MigrationId] [nvarchar](150) NOT NULL,
	[ProductVersion] [nvarchar](32) NOT NULL,
 CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY CLUSTERED 
(
	[MigrationId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dataIntegration].[CustomerDbModels]    Script Date: 7/14/2024 11:21:58 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dataIntegration].[CustomerDbModels](
	[CustomerId] [nvarchar](450) NOT NULL,
	[DbModel] [nvarchar](max) NOT NULL,
 CONSTRAINT [PK_CustomerDbModels] PRIMARY KEY CLUSTERED 
(
	[CustomerId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dataIntegration].[DatabaseConnectionConfigurations]    Script Date: 7/14/2024 11:21:58 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dataIntegration].[DatabaseConnectionConfigurations](
	[CustomerId] [nvarchar](450) NOT NULL,
	[Provider] [int] NOT NULL,
	[ConnectionString] [nvarchar](max) NOT NULL,
 CONSTRAINT [PK_DatabaseConnectionConfigurations] PRIMARY KEY CLUSTERED 
(
	[CustomerId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dataIntegration].[SchemaMappings]    Script Date: 7/14/2024 11:21:58 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dataIntegration].[SchemaMappings](
	[CustomerId] [nvarchar](220) NOT NULL,
	[TargetDbTableId] [int] NOT NULL,
	[Mapping] [nvarchar](max) NOT NULL,
	[IsComplete] [bit] NOT NULL,
 CONSTRAINT [PK_SchemaMappings] PRIMARY KEY CLUSTERED 
(
	[CustomerId] ASC,
	[TargetDbTableId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dataIntegration].[TargetDbTables]    Script Date: 7/14/2024 11:21:58 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dataIntegration].[TargetDbTables](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Schema] [nvarchar](max) NOT NULL,
	[TableName] [nvarchar](max) NOT NULL,
	[TableModel] [nvarchar](max) NOT NULL,
 CONSTRAINT [PK_TargetDbTables] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [deployment].[__EFMigrationsHistory]    Script Date: 7/14/2024 11:21:58 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [deployment].[__EFMigrationsHistory](
	[MigrationId] [nvarchar](150) NOT NULL,
	[ProductVersion] [nvarchar](32) NOT NULL,
 CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY CLUSTERED 
(
	[MigrationId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [deployment].[MetabaseDeployments]    Script Date: 7/14/2024 11:21:58 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [deployment].[MetabaseDeployments](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[CustomerId] [nvarchar](450) NOT NULL,
	[UrlPath] [nvarchar](255) NOT NULL,
	[Image] [nvarchar](max) NOT NULL,
	[InstanceName] [nvarchar](max) NOT NULL,
 CONSTRAINT [PK_MetabaseDeployments] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [users].[AspNetRoleClaims]    Script Date: 7/14/2024 11:21:58 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [users].[AspNetRoleClaims](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[RoleId] [nvarchar](220) NOT NULL,
	[ClaimType] [nvarchar](max) NULL,
	[ClaimValue] [nvarchar](max) NULL,
 CONSTRAINT [PK_AspNetRoleClaims] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [users].[AspNetRoles]    Script Date: 7/14/2024 11:21:58 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [users].[AspNetRoles](
	[Id] [nvarchar](220) NOT NULL,
	[Name] [nvarchar](256) NULL,
	[NormalizedName] [nvarchar](256) NULL,
	[ConcurrencyStamp] [nvarchar](max) NULL,
 CONSTRAINT [PK_AspNetRoles] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [users].[AspNetUserClaims]    Script Date: 7/14/2024 11:21:58 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [users].[AspNetUserClaims](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[UserId] [nvarchar](220) NOT NULL,
	[ClaimType] [nvarchar](max) NULL,
	[ClaimValue] [nvarchar](max) NULL,
 CONSTRAINT [PK_AspNetUserClaims] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [users].[AspNetUserLogins]    Script Date: 7/14/2024 11:21:58 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [users].[AspNetUserLogins](
	[LoginProvider] [nvarchar](220) NOT NULL,
	[ProviderKey] [nvarchar](220) NOT NULL,
	[ProviderDisplayName] [nvarchar](max) NULL,
	[UserId] [nvarchar](220) NOT NULL,
 CONSTRAINT [PK_AspNetUserLogins] PRIMARY KEY CLUSTERED 
(
	[LoginProvider] ASC,
	[ProviderKey] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [users].[AspNetUserRoles]    Script Date: 7/14/2024 11:21:58 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [users].[AspNetUserRoles](
	[UserId] [nvarchar](220) NOT NULL,
	[RoleId] [nvarchar](220) NOT NULL,
 CONSTRAINT [PK_AspNetUserRoles] PRIMARY KEY CLUSTERED 
(
	[UserId] ASC,
	[RoleId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [users].[AspNetUsers]    Script Date: 7/14/2024 11:21:58 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [users].[AspNetUsers](
	[Id] [nvarchar](220) NOT NULL,
	[Name] [nvarchar](70) NOT NULL,
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
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [users].[AspNetUserTokens]    Script Date: 7/14/2024 11:21:58 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [users].[AspNetUserTokens](
	[UserId] [nvarchar](220) NOT NULL,
	[LoginProvider] [nvarchar](100) NOT NULL,
	[Name] [nvarchar](100) NOT NULL,
	[Value] [nvarchar](max) NULL,
 CONSTRAINT [PK_AspNetUserTokens] PRIMARY KEY CLUSTERED 
(
	[UserId] ASC,
	[LoginProvider] ASC,
	[Name] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
INSERT [dataIntegration].[__EFMigrationsHistory] ([MigrationId], [ProductVersion]) VALUES (N'20240606155536_AddDbConnectionConfiguration', N'9.0.0-preview.5.24306.3')
INSERT [dataIntegration].[__EFMigrationsHistory] ([MigrationId], [ProductVersion]) VALUES (N'20240606162853_RenameUserIdToCostumerId', N'9.0.0-preview.5.24306.3')
INSERT [dataIntegration].[__EFMigrationsHistory] ([MigrationId], [ProductVersion]) VALUES (N'20240606201506_ChangeDataIntegrationName', N'9.0.0-preview.5.24306.3')
INSERT [dataIntegration].[__EFMigrationsHistory] ([MigrationId], [ProductVersion]) VALUES (N'20240608154659_AddCostumerDbModels', N'9.0.0-preview.5.24306.3')
INSERT [dataIntegration].[__EFMigrationsHistory] ([MigrationId], [ProductVersion]) VALUES (N'20240612154825_AddTargetDbTablesAndSchemaMappings', N'9.0.0-preview.5.24306.3')
INSERT [dataIntegration].[__EFMigrationsHistory] ([MigrationId], [ProductVersion]) VALUES (N'20240612212540_AddTargetDbTablesData', N'9.0.0-preview.5.24306.3')
INSERT [dataIntegration].[__EFMigrationsHistory] ([MigrationId], [ProductVersion]) VALUES (N'20240613214934_AddIsSchemaMappingCompleteProperty', N'9.0.0-preview.5.24306.3')
INSERT [dataIntegration].[__EFMigrationsHistory] ([MigrationId], [ProductVersion]) VALUES (N'20240617150729_UpdateTargetDbModel', N'9.0.0-preview.5.24306.3')
INSERT [dataIntegration].[__EFMigrationsHistory] ([MigrationId], [ProductVersion]) VALUES (N'20240702130159_UpdateTargetDBModelDataType', N'9.0.0-preview.5.24306.3')
INSERT [dataIntegration].[__EFMigrationsHistory] ([MigrationId], [ProductVersion]) VALUES (N'20240705210937_FixCostumerTypo', N'9.0.0-preview.5.24306.3')
INSERT [dataIntegration].[__EFMigrationsHistory] ([MigrationId], [ProductVersion]) VALUES (N'20240705211320_FixCostumerTypo2', N'9.0.0-preview.5.24306.3')
GO
SET IDENTITY_INSERT [dataIntegration].[TargetDbTables] ON 

INSERT [dataIntegration].[TargetDbTables] ([Id], [Schema], [TableName], [TableModel]) VALUES (1, N'dbo', N'Employees', N'{"name":"Employees","schema":"dbo","primaryKeys":[{"name":"Id","dataType":{"type":"simple","simpleType":"Integer","isNullable":false}}],"columns":[{"name":"Id","dataType":{"type":"simple","simpleType":"Integer","isNullable":false}},{"name":"PersonalID","dataType":{"type":"simple","simpleType":"Integer","isNullable":false}},{"name":"ExternalId","dataType":{"type":"simple","simpleType":"Integer","isNullable":false}},{"name":"FirstName","dataType":{"type":"nVarCharMax","isNullable":false}},{"name":"Lastname","dataType":{"type":"nVarCharMax","isNullable":false}}],"description":null}')
INSERT [dataIntegration].[TargetDbTables] ([Id], [Schema], [TableName], [TableModel]) VALUES (2, N'dbo', N'Workplaces', N'{"name":"Workplaces","schema":"dbo","primaryKeys":[{"name":"Id","dataType":{"type":"simple","simpleType":"Integer","isNullable":false}}],"columns":[{"name":"Id","dataType":{"type":"simple","simpleType":"Integer","isNullable":false}},{"name":"Label","dataType":{"type":"nVarCharMax","isNullable":false}},{"name":"Name","dataType":{"type":"nVarCharMax","isNullable":true}}],"description":null}')
INSERT [dataIntegration].[TargetDbTables] ([Id], [Schema], [TableName], [TableModel]) VALUES (3, N'dbo', N'WorkReports', N'{"name":"WorkReports","schema":"dbo","primaryKeys":[{"name":"ID","dataType":{"type":"simple","simpleType":"Integer","isNullable":false}}],"columns":[{"name":"ID","dataType":{"type":"simple","simpleType":"Integer","isNullable":false}},{"name":"OrderId","dataType":{"type":"simple","simpleType":"Integer","isNullable":true}},{"name":"ProductionOperationId","dataType":{"type":"simple","simpleType":"Integer","isNullable":true}},{"name":"Quantity","dataType":{"type":"simple","simpleType":"Decimal","isNullable":false}},{"name":"ExpectedTime","dataType":{"type":"simple","simpleType":"Decimal","isNullable":false}},{"name":"DateTime","dataType":{"type":"simple","simpleType":"Datetime","isNullable":false}},{"name":"ProductType","dataType":{"type":"nVarCharMax","isNullable":true}},{"name":"WorkerId","dataType":{"type":"simple","simpleType":"Integer","isNullable":true}},{"name":"WorkPlaceId","dataType":{"type":"simple","simpleType":"Integer","isNullable":true}}],"description":null}')
SET IDENTITY_INSERT [dataIntegration].[TargetDbTables] OFF
GO
INSERT [deployment].[__EFMigrationsHistory] ([MigrationId], [ProductVersion]) VALUES (N'20240706075746_AddMetabaseDeployments', N'9.0.0-preview.5.24306.3')
GO
INSERT [users].[AspNetRoles] ([Id], [Name], [NormalizedName], [ConcurrencyStamp]) VALUES (N'da360ae6-f8d6-4fca-a542-0eac0d26f9d1', N'Admin', N'ADMIN', NULL)
INSERT [users].[AspNetRoles] ([Id], [Name], [NormalizedName], [ConcurrencyStamp]) VALUES (N'fb03ded4-41b7-4e16-a5e6-4ca2e59b2b37', N'Customer', N'CUSTOMER', NULL)
GO
INSERT [users].[AspNetUserRoles] ([UserId], [RoleId]) VALUES (N'7c321bc4-9223-4658-9321-711c490778f1', N'da360ae6-f8d6-4fca-a542-0eac0d26f9d1')
GO
INSERT [users].[AspNetUsers] ([Id], [Name], [UserName], [NormalizedUserName], [Email], [NormalizedEmail], [EmailConfirmed], [PasswordHash], [SecurityStamp], [ConcurrencyStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEnd], [LockoutEnabled], [AccessFailedCount]) VALUES (N'7c321bc4-9223-4658-9321-711c490778f1', N'Default Admin', N'admin@admin.cz', N'ADMIN@ADMIN.CZ', N'admin@admin.cz', N'ADMIN@ADMIN.CZ', 1, N'AQAAAAIAAYagAAAAEExXFRNY+sDK1+aiJypi7JcsiHYYqDIlElI8ZNSbc645sltVJuA1TFobZj5sy8wwiA==', N'QHYF3RUQOVNYLVCX26ORAA3FB5DMAZDA', N'0f46e8a3-45c7-4db0-8886-5723df850e4c', NULL, 0, 0, NULL, 1, 0)
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [AK_MetabaseDeployments_CustomerId]    Script Date: 7/14/2024 11:21:58 AM ******/
ALTER TABLE [deployment].[MetabaseDeployments] ADD  CONSTRAINT [AK_MetabaseDeployments_CustomerId] UNIQUE NONCLUSTERED 
(
	[CustomerId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
ALTER TABLE [dataIntegration].[SchemaMappings] ADD  DEFAULT (CONVERT([bit],(0))) FOR [IsComplete]
GO
ALTER TABLE [users].[AspNetRoleClaims]  WITH CHECK ADD  CONSTRAINT [FK_AspNetRoleClaims_AspNetRoles_RoleId] FOREIGN KEY([RoleId])
REFERENCES [users].[AspNetRoles] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [users].[AspNetRoleClaims] CHECK CONSTRAINT [FK_AspNetRoleClaims_AspNetRoles_RoleId]
GO
ALTER TABLE [users].[AspNetUserClaims]  WITH CHECK ADD  CONSTRAINT [FK_AspNetUserClaims_AspNetUsers_UserId] FOREIGN KEY([UserId])
REFERENCES [users].[AspNetUsers] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [users].[AspNetUserClaims] CHECK CONSTRAINT [FK_AspNetUserClaims_AspNetUsers_UserId]
GO
ALTER TABLE [users].[AspNetUserLogins]  WITH CHECK ADD  CONSTRAINT [FK_AspNetUserLogins_AspNetUsers_UserId] FOREIGN KEY([UserId])
REFERENCES [users].[AspNetUsers] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [users].[AspNetUserLogins] CHECK CONSTRAINT [FK_AspNetUserLogins_AspNetUsers_UserId]
GO
ALTER TABLE [users].[AspNetUserRoles]  WITH CHECK ADD  CONSTRAINT [FK_AspNetUserRoles_AspNetRoles_RoleId] FOREIGN KEY([RoleId])
REFERENCES [users].[AspNetRoles] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [users].[AspNetUserRoles] CHECK CONSTRAINT [FK_AspNetUserRoles_AspNetRoles_RoleId]
GO
ALTER TABLE [users].[AspNetUserRoles]  WITH CHECK ADD  CONSTRAINT [FK_AspNetUserRoles_AspNetUsers_UserId] FOREIGN KEY([UserId])
REFERENCES [users].[AspNetUsers] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [users].[AspNetUserRoles] CHECK CONSTRAINT [FK_AspNetUserRoles_AspNetUsers_UserId]
GO
ALTER TABLE [users].[AspNetUserTokens]  WITH CHECK ADD  CONSTRAINT [FK_AspNetUserTokens_AspNetUsers_UserId] FOREIGN KEY([UserId])
REFERENCES [users].[AspNetUsers] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [users].[AspNetUserTokens] CHECK CONSTRAINT [FK_AspNetUserTokens_AspNetUsers_UserId]
GO
