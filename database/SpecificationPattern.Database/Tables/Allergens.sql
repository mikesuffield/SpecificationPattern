CREATE TABLE [dbo].[Allergens] (
    [Id]            UNIQUEIDENTIFIER NOT NULL,
    [MenuItemId]    UNIQUEIDENTIFIER NOT NULL,
    [AllergenType]  NVARCHAR(100)    NOT NULL,
    [CreatedAt]     DATETIME         CONSTRAINT [Allergens_CreatedAtDefault] DEFAULT (getutcdate()) NOT NULL,
    [UpdatedAt]     DATETIME         NULL,
    [RowRevision]   ROWVERSION       NOT NULL,
    CONSTRAINT [PK_Allergens_Id] PRIMARY KEY CLUSTERED ([Id] Asc),
    CONSTRAINT [FK_Allergens_MenuItemId_To_MenuItems_Id] FOREIGN KEY ([MenuItemId]) REFERENCES [dbo].[MenuItems] ([Id])
);

GO