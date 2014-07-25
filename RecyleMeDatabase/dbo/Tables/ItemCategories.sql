CREATE TABLE [dbo].[ItemCategories] (
    [Id]   INT            IDENTITY (1, 1) NOT NULL,
    [Name] NVARCHAR (MAX) NULL,
    CONSTRAINT [PK_dbo.ItemCategories] PRIMARY KEY CLUSTERED ([Id] ASC)
);

