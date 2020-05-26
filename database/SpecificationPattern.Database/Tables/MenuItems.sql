CREATE TABLE [dbo].[MenuItems] (
    [Id]            UNIQUEIDENTIFIER NOT NULL,
    [Name]          NVARCHAR(100)    NOT NULL,
    [Price]         DECIMAL(5,2)     NOT NULL,
    [MealType]      NVARCHAR(100)    NOT NULL,
    [CreatedAt]     DATETIME         CONSTRAINT [MenuItems_CreatedAtDefault] DEFAULT (getutcdate()) NOT NULL,
    [UpdatedAt]     DATETIME         NULL,
    [RowRevision]   ROWVERSION       NOT NULL,
    CONSTRAINT [PK_MenuItems_Id] PRIMARY KEY CLUSTERED ([Id] Asc)
);

GO