CREATE DATABASE BuyAndSell
GO
USE [BuyAndSell]
GO
/****** Object:  Table [dbo].[Transactions]    Script Date: 05/07/2017 21:45:15 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Transactions](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[TransactionId] [int] NOT NULL,
	[ProductName] [nvarchar](50) NOT NULL,
	[BuyPrice] [int] NOT NULL,
	[SellPrice] [int] NOT NULL,
	[Quantity] [int] NOT NULL,
	[Total] [int] NOT NULL,
	[Revenue] [int] NOT NULL,
	[Date] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_Transactions] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TransactionId]    Script Date: 05/07/2017 21:45:15 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TransactionId](
	[TransactionId] [int] NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TProducts]    Script Date: 05/07/2017 21:45:15 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TProducts](
	[ProductId] [int] IDENTITY(1,1) NOT NULL,
	[ProductName] [nchar](20) NOT NULL,
	[BuyPrice] [bigint] NOT NULL,
	[SellPrice] [bigint] NOT NULL,
	[Quantity] [bigint] NULL,
	[ProductImage] [image] NULL,
PRIMARY KEY CLUSTERED 
(
	[ProductName] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Stock]    Script Date: 05/07/2017 21:45:15 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Stock](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[ProductName] [nvarchar](50) NOT NULL,
	[Stock] [int] NULL,
 CONSTRAINT [PK_Stock] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  StoredProcedure [dbo].[sp_insert_transaction_id]    Script Date: 05/07/2017 21:45:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [dbo].[sp_insert_transaction_id]
@TransactionId int As
BEGIN
INSERT INTO TransactionId VALUES(@TransactionId)
END
GO
/****** Object:  StoredProcedure [dbo].[sp_insert_transaction]    Script Date: 05/07/2017 21:45:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
Create PROC [dbo].[sp_insert_transaction]
@TransactionId int,
@ProductName nvarchar(50),
@BuyPrice int,
@SellPrice int,
@Quantity int,
@Total int,
@Revenue int,
@Date nvarchar(50) As
BEGIN
INSERT INTO Transactions VALUES(@TransactionId,@ProductName,@BuyPrice,@SellPrice,@Quantity,@Total,@Revenue,@Date)
END
GO
/****** Object:  StoredProcedure [dbo].[sp_insert_stock]    Script Date: 05/07/2017 21:45:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [dbo].[sp_insert_stock]
@ProductName nchar(20),
@Quantity int AS
BEGIN
INSERT INTO Stock VALUES(@ProductName,@Quantity)
END
GO
/****** Object:  StoredProcedure [dbo].[sp_insert_data]    Script Date: 05/07/2017 21:45:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [dbo].[sp_insert_data]
@ProductName nchar(20),
@BuyPrice bigint,
@SellPrice bigint,
@Quantity bigint,
@ProductImage image AS
BEGIN
INSERT INTO TProducts VALUES(@ProductName,@BuyPrice,@SellPrice,@Quantity,@ProductImage)
END
GO
/****** Object:  Default [DC_ProductInfo_Quantity]    Script Date: 05/07/2017 21:45:15 ******/
ALTER TABLE [dbo].[TProducts] ADD  CONSTRAINT [DC_ProductInfo_Quantity]  DEFAULT ((0)) FOR [Quantity]
GO
insert into TransactionId (TransactionId) values (1)
GO
