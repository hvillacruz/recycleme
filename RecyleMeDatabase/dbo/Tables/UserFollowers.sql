CREATE TABLE [dbo].[UserFollowers] (
    [Id]             BIGINT         IDENTITY (1, 1) NOT NULL,
    [FollowerId]     NVARCHAR (128) NULL,
    [FollowedUserId] NVARCHAR (128) NULL,
    [ModifiedDate]   DATETIME       NOT NULL,
    CONSTRAINT [PK_dbo.UserFollowers] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_dbo.UserFollowers_dbo.Users_FollowedUserId] FOREIGN KEY ([FollowedUserId]) REFERENCES [dbo].[Users] ([UserId]),
    CONSTRAINT [FK_dbo.UserFollowers_dbo.Users_FollowerId] FOREIGN KEY ([FollowerId]) REFERENCES [dbo].[Users] ([UserId])
);






GO
CREATE NONCLUSTERED INDEX [IX_FollowerId]
    ON [dbo].[UserFollowers]([FollowerId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_FollowedUserId]
    ON [dbo].[UserFollowers]([FollowedUserId] ASC);


GO
CREATE TRIGGER [dbo].[INSERTUSERFOLLOWERSTRIGGER] ON [dbo].[UserFollowers]
FOR INSERT
AS

INSERT INTO NOTIFICATIONS
        (OWNERID,SENDERID,TYPE,TITLE,URLID,ISDELETED,ISREAD,MODIFIEDDATE)
    SELECT
        FOLLOWEDUSERID,FOLLOWERID,2,'Like',FOLLOWERID,0,0,GETDATE()
        FROM INSERTED