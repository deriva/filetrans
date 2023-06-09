 
/****** Object:  Table [dbo].[PublicFileDir]    Script Date: 2023/4/4 19:59:02 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PublicFileDir](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Path] [varchar](128) NULL,
	[FileType] [int] NULL,
	[CompressName] [varchar](128) NULL,
	[Status] [int] NULL,
	[SiteName] [varchar](64) NULL,
	[PublicNo] [varchar](64) NULL,
 CONSTRAINT [PK_PublicFileDir] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PublicToSite]    Script Date: 2023/4/4 19:59:02 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PublicToSite](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[PublicNo] [varchar](64) NOT NULL,
	[PublicSiteName] [varchar](64) NOT NULL,
	[ServerNo] [varchar](64) NOT NULL,
	[ServerSiteName] [varchar](64) NOT NULL,
 CONSTRAINT [PK_PublicToSite] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ServerApp]    Script Date: 2023/4/4 19:59:02 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ServerApp](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[No] [varchar](64) NOT NULL,
	[ServerName] [varchar](64) NOT NULL,
	[IP] [varchar](64) NOT NULL,
	[Port] [varchar](64) NOT NULL,
	[Env] [int] NOT NULL,
	[UpdateTime] [datetime] NOT NULL,
 CONSTRAINT [PK_ServerApp] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[SiteInfo]    Script Date: 2023/4/4 19:59:02 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SiteInfo](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[SiteName] [varchar](64) NULL,
	[ServerIP] [varchar](64) NULL,
	[No] [varchar](64) NULL,
	[SiteDir] [varchar](64) NULL,
	[Status] [int] NULL,
	[UpdateTime] [datetime] NULL,
	[PublicTime] [datetime] NULL,
	[IsCompress] [int] NULL,
	[Port] [varchar](64) NULL,
	[Protocol] [varchar](64) NULL,
	[Env] [int] NULL,
	[CLRVeersion] [varchar](16) NULL,
 CONSTRAINT [PK_SiteInfo] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[SitePublicExcludeDir]    Script Date: 2023/4/4 19:59:02 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SitePublicExcludeDir](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[No] [varchar](64) NULL,
	[Dir] [varchar](128) NULL,
	[PublicNo] [varchar](64) NULL,
 CONSTRAINT [PK_SitePublicExcludeDir] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[SiteVirtualDir]    Script Date: 2023/4/4 19:59:02 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SiteVirtualDir](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[SiteName] [varchar](64) NOT NULL,
	[SiteNo] [varchar](64) NOT NULL,
	[VirtualName] [varchar](64) NOT NULL,
	[VirtualDir] [varchar](512) NOT NULL,
 CONSTRAINT [PK_SiteVirtualDir] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[WebSiteBinding]    Script Date: 2023/4/4 19:59:02 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[WebSiteBinding](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[No] [varchar](64) NOT NULL,
	[Port] [varchar](64) NOT NULL,
	[ServerIP] [varchar](64) NOT NULL,
	[Protocol] [varchar](64) NOT NULL,
	[HostName] [varchar](64) NOT NULL,
 CONSTRAINT [PK_WebSiteBinding] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'发布文件目录' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'PublicFileDir', @level2type=N'COLUMN',@level2name=N'Id'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'发布的路径' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'PublicFileDir', @level2type=N'COLUMN',@level2name=N'Path'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'文件类型:0目录 1文件' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'PublicFileDir', @level2type=N'COLUMN',@level2name=N'FileType'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'压缩文件名' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'PublicFileDir', @level2type=N'COLUMN',@level2name=N'CompressName'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'状态' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'PublicFileDir', @level2type=N'COLUMN',@level2name=N'Status'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'站点名称' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'PublicFileDir', @level2type=N'COLUMN',@level2name=N'SiteName'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'发布站点对服务站点' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'PublicToSite', @level2type=N'COLUMN',@level2name=N'Id'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'发布站点编号' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'PublicToSite', @level2type=N'COLUMN',@level2name=N'PublicNo'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'发布站点名称' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'PublicToSite', @level2type=N'COLUMN',@level2name=N'PublicSiteName'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'服务站点编号' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'PublicToSite', @level2type=N'COLUMN',@level2name=N'ServerNo'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'服务端站点名称' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'PublicToSite', @level2type=N'COLUMN',@level2name=N'ServerSiteName'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'服务端' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ServerApp', @level2type=N'COLUMN',@level2name=N'Id'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'站点编号' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ServerApp', @level2type=N'COLUMN',@level2name=N'No'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'1生产端 2测试端' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ServerApp', @level2type=N'COLUMN',@level2name=N'Env'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'更新实际' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ServerApp', @level2type=N'COLUMN',@level2name=N'UpdateTime'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'站点信息' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SiteInfo', @level2type=N'COLUMN',@level2name=N'Id'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'站点信息' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SiteInfo', @level2type=N'COLUMN',@level2name=N'SiteName'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'服务器' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SiteInfo', @level2type=N'COLUMN',@level2name=N'ServerIP'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'编号' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SiteInfo', @level2type=N'COLUMN',@level2name=N'No'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'站点目录' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SiteInfo', @level2type=N'COLUMN',@level2name=N'SiteDir'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'状态:1正常  0停止' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SiteInfo', @level2type=N'COLUMN',@level2name=N'Status'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'更新时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SiteInfo', @level2type=N'COLUMN',@level2name=N'UpdateTime'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'发布时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SiteInfo', @level2type=N'COLUMN',@level2name=N'PublicTime'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'是否压缩' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SiteInfo', @level2type=N'COLUMN',@level2name=N'IsCompress'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'端口' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SiteInfo', @level2type=N'COLUMN',@level2name=N'Port'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'协议:http/https' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SiteInfo', @level2type=N'COLUMN',@level2name=N'Protocol'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'生成环境:1生成环境 2测试环境' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SiteInfo', @level2type=N'COLUMN',@level2name=N'Env'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'版本:,v2.0,v4.0' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SiteInfo', @level2type=N'COLUMN',@level2name=N'CLRVeersion'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'站点发布排除的目录' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SitePublicExcludeDir', @level2type=N'COLUMN',@level2name=N'Id'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'站点编号' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SitePublicExcludeDir', @level2type=N'COLUMN',@level2name=N'No'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'排除的目录' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SitePublicExcludeDir', @level2type=N'COLUMN',@level2name=N'Dir'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'站点的虚拟目录' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SiteVirtualDir', @level2type=N'COLUMN',@level2name=N'Id'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'站点名称' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SiteVirtualDir', @level2type=N'COLUMN',@level2name=N'SiteName'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'站点编号' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SiteVirtualDir', @level2type=N'COLUMN',@level2name=N'SiteNo'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'虚拟目录名称' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SiteVirtualDir', @level2type=N'COLUMN',@level2name=N'VirtualName'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'虚拟目录路径' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SiteVirtualDir', @level2type=N'COLUMN',@level2name=N'VirtualDir'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'站点绑定' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'WebSiteBinding', @level2type=N'COLUMN',@level2name=N'Id'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'编号' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'WebSiteBinding', @level2type=N'COLUMN',@level2name=N'No'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'端口' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'WebSiteBinding', @level2type=N'COLUMN',@level2name=N'Port'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'服务器' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'WebSiteBinding', @level2type=N'COLUMN',@level2name=N'ServerIP'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'协议:http/https' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'WebSiteBinding', @level2type=N'COLUMN',@level2name=N'Protocol'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'主机名' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'WebSiteBinding', @level2type=N'COLUMN',@level2name=N'HostName'
GO
