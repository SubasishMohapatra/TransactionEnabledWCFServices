USE [Bank]
GO
/****** Object:  Table [dbo].[Account]    Script Date: 21-May-18 3:48:18 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Account](
	[AccountID] [int] NOT NULL,
	[CustomerName] [nchar](10) NOT NULL,
	[Balance] [int] NOT NULL
) ON [PRIMARY]

GO
INSERT [dbo].[Account] ([AccountID], [CustomerName], [Balance]) VALUES (1000, N'Subasish  ', 800)
INSERT [dbo].[Account] ([AccountID], [CustomerName], [Balance]) VALUES (1001, N'Debashish ', 6000)
