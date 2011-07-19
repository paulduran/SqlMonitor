/****** Object:  Table [dbo].[Queries]    Script Date: 01/21/2011 16:13:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Queries](
	[QueryId] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](4000) NOT NULL,
	[DatabaseName] [nvarchar](4000) NOT NULL,
	[QueryText] [nvarchar](4000) NOT NULL,
	[CronExpression] [nvarchar](4000) NOT NULL,
	[LastRun] [datetime] NULL,
	[NextRun] [datetime] NULL,
	[ThresholdMilliseconds] [int] NOT NULL,
	[AlertIfAboveThreshold] [bit] NOT NULL,
	[AlertEmailTo] [nvarchar](4000) NULL,
PRIMARY KEY CLUSTERED 
(
	[QueryId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[EdmMetadata]    Script Date: 01/21/2011 16:13:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[EdmMetadata](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[ModelHash] [nvarchar](4000) NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[QueryResults]    Script Date: 01/21/2011 16:13:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[QueryResults](
	[QueryResultId] [int] IDENTITY(1,1) NOT NULL,
	[StartDate] [datetime] NOT NULL,
	[Success] [bit] NOT NULL,
	[ErrorMessage] [nvarchar](4000) NULL,
	[Duration] [time](7) NOT NULL,
	[NumberOfResults] [int] NOT NULL,
	[QueryQueryId] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[QueryResultId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  ForeignKey [QueryResult_Query]    Script Date: 01/21/2011 16:13:16 ******/
ALTER TABLE [dbo].[QueryResults]  WITH CHECK ADD  CONSTRAINT [QueryResult_Query] FOREIGN KEY([QueryQueryId])
REFERENCES [dbo].[Queries] ([QueryId])
GO
ALTER TABLE [dbo].[QueryResults] CHECK CONSTRAINT [QueryResult_Query]
GO