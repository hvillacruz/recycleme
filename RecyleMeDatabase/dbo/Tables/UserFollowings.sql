CREATE TABLE [dbo].[UserFollowings] (
    [Id]              BIGINT         IDENTITY (1, 1) NOT NULL,
    [FollowingId]     NVARCHAR (128) NULL,
    [FollowingUserId] NVARCHAR (128) NULL,
    [ModifiedDate]    DATETIME       NOT NULL,
    CONSTRAINT [PK_dbo.UserFollowings] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_dbo.UserFollowings_dbo.Users_FollowingId] FOREIGN KEY ([FollowingId]) REFERENCES [dbo].[Users] ([UserId]),
    CONSTRAINT [FK_dbo.UserFollowings_dbo.Users_FollowingUserId] FOREIGN KEY ([FollowingUserId]) REFERENCES [dbo].[Users] ([UserId])
);


GO
CREATE NONCLUSTERED INDEX [IX_FollowingId]
    ON [dbo].[UserFollowings]([FollowingId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_FollowingUserId]
    ON [dbo].[UserFollowings]([FollowingUserId] ASC);

