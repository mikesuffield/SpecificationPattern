/*
Post-Deployment Script Template							
--------------------------------------------------------------------------------------
 This file contains SQL statements that will be appended to the build script.		
 Use SQLCMD syntax to include a file in the post-deployment script.			
 Example:      :r .\myfile.sql								
 Use SQLCMD syntax to reference a variable in the post-deployment script.		
 Example:      :setvar TableName MyTable							
               SELECT * FROM [$(TableName)]					
--------------------------------------------------------------------------------------
*/

/* STEP 1 - Clear tables */

DELETE FROM [dbo].[Allergens]   
GO

DELETE FROM [dbo].[MenuItems]   
GO



/* STEP 2 - populate tables with seeded data */

INSERT INTO [dbo].[MenuItems] ([Id], [Name], [Price], [MealType])
VALUES
('B48AF7A0-065D-46F8-A904-D723D9ECCA6D', 'Chicken Fried Rice', '5.50', 'Main'),
('65D3913D-784A-4982-BCAA-37B343579E0A', 'Spare Ribs in Peking Sauce', '5.00', 'Starter'),
('EF762E07-2A43-464C-BB63-4CBA94A7F3A4', 'Crispy Beef with Chilli', '4.90', 'Main'),
('27255B77-BAD4-4EA4-97D6-0D2CACFB821F', 'Lemon Chicken', '4.90', 'Main'),
('E95E91FF-7891-4A6D-A583-C0F6D3AB15A5', 'Vegetarian Spring Rolls', '2.20', 'Starter'),
('2B616C74-CD82-45F1-A995-ADB146227B28', 'Thai Fish Cakes', '3.00', 'Starter'),
('899240C6-A220-47AB-8DD4-6AF44D0EAAE6', '1/4 Crispy Aromatic Duck', '7.70', 'Main'),
('ABBF8B26-2056-4484-A8D4-037108D5176A', 'Chicken Satay Skewers', '4.80', 'Starter'),
('1a0701ac-7ea2-4f88-a414-17265a2b4df9', 'Banana Fritters', '3.60', 'Dessert')


INSERT INTO [dbo].[Allergens] ([Id], [MenuItemId], [Name])
VALUES
('BAC64D13-2088-41F3-8245-FAC595F35D89', 'B48AF7A0-065D-46F8-A904-D723D9ECCA6D', 'Cereals containing gluten'),
('FF785646-9336-4002-91F1-3F60B9A74417', 'B48AF7A0-065D-46F8-A904-D723D9ECCA6D', 'Eggs'),
('6271056D-55AB-44B4-B9C8-112643C2D76E', 'B48AF7A0-065D-46F8-A904-D723D9ECCA6D', 'Sesame'),
('0026037B-3429-4C04-AAD6-A7CBBFE4F501', 'B48AF7A0-065D-46F8-A904-D723D9ECCA6D', 'Soya'),
('3fb8e617-151e-4d31-bac5-fb1e4926eb1b', '65D3913D-784A-4982-BCAA-37B343579E0A', 'Sesame'),
('C17456AD-E6FF-468F-8877-50D321BB2BD5', '65D3913D-784A-4982-BCAA-37B343579E0A', 'Soya'),
('A39CF5D0-561A-4AF9-A71A-D974627FD4D1', 'EF762E07-2A43-464C-BB63-4CBA94A7F3A4', 'Soya'),
('73917248-39FF-42E4-B768-0EADACC49B4A', 'E95E91FF-7891-4A6D-A583-C0F6D3AB15A5', 'Eggs'),
('CA3932AC-5126-4AD8-8F91-DF4910FAB98B', 'E95E91FF-7891-4A6D-A583-C0F6D3AB15A5', 'Lupin'),
('74648BF1-03F0-49FE-B2D6-A4964C52F42E', 'E95E91FF-7891-4A6D-A583-C0F6D3AB15A5', 'Milk'),
('EA75BA62-0804-4068-9BD5-58BDF8863B57', 'E95E91FF-7891-4A6D-A583-C0F6D3AB15A5', 'Soya'),
('5047d423-f9ee-4061-b5b7-fc4a733f1c04', '2B616C74-CD82-45F1-A995-ADB146227B28', 'Crustaceans'),
('384e1f22-61bb-4280-97e7-5cb9f2c77881', '2B616C74-CD82-45F1-A995-ADB146227B28', 'Fish'),
('d693871c-2ab5-4357-9c45-e00ddf7ef04d', '899240C6-A220-47AB-8DD4-6AF44D0EAAE6', 'Soya'),
('d3be0293-0e4a-4183-b0c2-6e08e2e1dcbd', 'ABBF8B26-2056-4484-A8D4-037108D5176A', 'Peanuts'),
('320bb992-bf9b-41ee-b3f6-a91ad73f813b', 'ABBF8B26-2056-4484-A8D4-037108D5176A', 'Soya'),
('35dbe361-e306-4815-8fb3-59a878943c71', '1a0701ac-7ea2-4f88-a414-17265a2b4df9', 'Eggs'),
('f027c621-d610-4cfe-91a6-5e6afb6f2a0a', '1a0701ac-7ea2-4f88-a414-17265a2b4df9', 'Milk'),
('1ac70dc0-80ca-4abe-8c44-0e49025785f3', '1a0701ac-7ea2-4f88-a414-17265a2b4df9', 'Lupin')