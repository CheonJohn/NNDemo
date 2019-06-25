USE [NNLife]
GO

/****** オブジェクト: Table [dbo].[recipes] スクリプト日付: 2019/06/25 2:11:25 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[recipes] (
    [id]          INT           IDENTITY (1, 1) NOT NULL,
    [title]       VARCHAR (100) NOT NULL,
    [making_time] VARCHAR (100) NOT NULL,
    [serves]      VARCHAR (100) NOT NULL,
    [ingredients] VARCHAR (300) NOT NULL,
    [cost]        INT           NOT NULL,
    [updated_at]  DATETIME      NOT NULL,
    [created_at]  DATETIME      NOT NULL
);


