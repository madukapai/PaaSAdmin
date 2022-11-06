CREATE SEQUENCE IISWebSitesId
AS BIGINT
START WITH 0
INCREMENT BY 1
NO CYCLE
NO CACHE
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