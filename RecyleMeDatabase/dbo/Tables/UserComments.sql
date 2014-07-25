CREATE TABLE [dbo].[UserComments] (
    [Id]              BIGINT         IDENTITY (1, 1) NOT NULL,
    [CommenterId]     NVARCHAR (128) NULL,
    [CommentedUserId] NVARCHAR (128) NULL,
    [ModifiedDate]    DATETIME       CONSTRAINT [DF_dbo.UserComments_ModifiedDate] DEFAULT (getdate()) NULL,
    [Comment]         NVARCHAR (MAX) NULL,
    CONSTRAINT [PK_dbo.UserComments] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_dbo.UserComments_dbo.Users_CommentedUserId] FOREIGN KEY ([CommentedUserId]) REFERENCES [dbo].[Users] ([UserId]),
    CONSTRAINT [FK_dbo.UserComments_dbo.Users_CommenterId] FOREIGN KEY ([CommenterId]) REFERENCES [dbo].[Users] ([UserId])
);


GO
CREATE NONCLUSTERED INDEX [IX_CommenterId]
    ON [dbo].[UserComments]([CommenterId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_CommentedUserId]
    ON [dbo].[UserComments]([CommentedUserId] ASC);

