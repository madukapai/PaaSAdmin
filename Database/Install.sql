CREATE SEQUENCE IISWebSitesId AS BIGINT START WITH 0 INCREMENT BY 1 NO CYCLE NO CACHE
GO

CREATE TABLE PaaSWebSites (
  IISWebSitesId BIGINT NOT NULL
 ,WebSiteName NVARCHAR(50) NOT NULL
 ,Product NVARCHAR(50) NOT NULL
 ,Port INT NOT NULL
 ,IP NVARCHAR(15) NOT NULL
 ,Domain NVARCHAR(50) NOT NULL
 ,UserName NVARCHAR(50) NOT NULL
 ,UserPassword NVARCHAR(500) NOT NULL
 ,PhysicalPath NVARCHAR(200) NOT NULL
 ,MaxMemoryGB DECIMAL NOT NULL
 ,MaxInstance INT NOT NULL
 ,RecycleMinutes INT NOT NULL
 ,RuntimeVersion VARCHAR(10) NOT NULL
 ,IsEnable32Bit BIT NOT NULL
 ,CONSTRAINT PK_PaaSWebSites_IISWebSitesId PRIMARY KEY CLUSTERED (IISWebSitesId)
)
GO


CREATE TABLE PaaSConfig (
  ConfigId int NOT NULL,
  ConfigName nvarchar(max) NOT NULL,
  ConfigValue nvarchar(max) NOT NULL,
  Description nvarchar(max) NOT NULL,
  CONSTRAINT PK_PaaS_Config PRIMARY KEY CLUSTERED (ConfigId)
)
GO

CREATE SEQUENCE ConfigId AS BIGINT START WITH 0 INCREMENT BY 1 NO CYCLE NO CACHE 
GO

INSERT INTO PaaSConfig VALUES (0, 'SystemName', 'PaaS Admin', '');

CREATE TABLE [dbo].[PaaSDeviceHealth](
	[PaaSDeviceHealthId] [bigint] NOT NULL,
	[ReportUtcDateTime] [datetime] NOT NULL,
	[CpuUsagePercent] [decimal](5, 2) NOT NULL,
	[MemoryUsagePercent] [decimal](5, 2) NOT NULL,
	[MemoryTotalGB] [decimal](8, 2) NOT NULL,
	[MemoryUsageGB] [decimal](8, 2) NOT NULL,
	[DiskUsagePercent] [decimal](5, 2) NOT NULL,
 CONSTRAINT [PK_PaaSDeviceHealth] PRIMARY KEY CLUSTERED 
(
	[PaaSDeviceHealthId] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

CREATE TABLE [dbo].[PaaSDeviceHealthApp](
	[PaaSDeviceHealthAppId] [bigint] NOT NULL,
	[PaaSDeviceHealthId] [bigint] NOT NULL,
	[PID] [int] NOT NULL,
	[ApplicationPool] [nvarchar](50) NOT NULL,
	[CpuUsagePercent] [decimal](5, 2) NOT NULL,
	[MemoryUsageMB] [decimal](8, 2) NOT NULL,
 CONSTRAINT [PK_PaaSDeviceHealthApp] PRIMARY KEY CLUSTERED 
(
	[PaaSDeviceHealthAppId] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

CREATE TABLE [dbo].[PaaSDeviceHealthCpu](
	[PaaSDeviceHealthCpuId] [bigint] NOT NULL,
	[PaaSDeviceHealthId] [bigint] NOT NULL,
	[Core] [int] NOT NULL,
	[CpuUsagePercent] [decimal](5, 2) NOT NULL,
 CONSTRAINT [PK_PaaSDeviceHealthCpu] PRIMARY KEY CLUSTERED 
(
	[PaaSDeviceHealthCpuId] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

CREATE TABLE [dbo].[PaaSDeviceHealthDisk](
	[PaaSDeviceHealthDiskId] [bigint] NOT NULL,
	[PaaSDeviceHealthId] [bigint] NOT NULL,
	[PhysicalName] [nvarchar](50) NOT NULL,
	[DriveName] [nvarchar](250) NOT NULL,
	[DriveFormat] [nvarchar](30) NOT NULL,
	[TotalGB] [decimal](8, 2) NOT NULL,
	[UsageGB] [decimal](8, 2) NOT NULL,
	[FreeGB] [decimal](8, 2) NOT NULL,
 CONSTRAINT [PK_PaaSDeviceHealthDisk] PRIMARY KEY CLUSTERED 
(
	[PaaSDeviceHealthDiskId] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

CREATE TABLE [dbo].[PaaSDeviceHealthMemory](
	[PaaSDeviceHealthMemoryId] [bigint] NOT NULL,
	[PaaSDeviceHealthId] [bigint] NOT NULL,
	[TotalGB] [decimal](8, 2) NOT NULL,
	[UsageGB] [decimal](5, 2) NOT NULL,
 CONSTRAINT [PK_PaaSDeviceHealthMemory] PRIMARY KEY CLUSTERED 
(
	[PaaSDeviceHealthMemoryId] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO


CREATE SEQUENCE PaaSDeviceHealthId AS BIGINT START WITH 0 INCREMENT BY 1 NO CYCLE NO CACHE
CREATE SEQUENCE PaaSDeviceHealthAppId AS BIGINT START WITH 0 INCREMENT BY 1 NO CYCLE NO CACHE
CREATE SEQUENCE PaaSDeviceHealthCpuId AS BIGINT START WITH 0 INCREMENT BY 1 NO CYCLE NO CACHE
CREATE SEQUENCE PaaSDeviceHealthDiskId AS BIGINT START WITH 0 INCREMENT BY 1 NO CYCLE NO CACHE
CREATE SEQUENCE PaaSDeviceHealthMemoryId AS BIGINT START WITH 0 INCREMENT BY 1 NO CYCLE NO CACHE
GO