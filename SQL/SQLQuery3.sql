SET IDENTITY_INSERT [UserProfile] ON
INSERT INTO [UserProfile] (
	[Id], [FirstName], [LastName], [DisplayName], [Email], [CreateDateTime], [ImageLocation], [UserTypeId])
VALUES (2, 'Chris', 'Denmark', 'Chris', 'chris@example.com', SYSDATETIME(), NULL, 1),
		(3, 'Christine', 'Doza', 'Christine', 'christine@example.com', SYSDATETIME(), NULL, 1),
		(4, 'Pazia', 'Ramirez', 'Pazia', 'pazia@example.com', SYSDATETIME(), NULL, 1),
		(5, 'Barry', 'Shovlin', 'Barry', 'barry@example.com', SYSDATETIME(), NULL, 1);
SET IDENTITY_INSERT [UserProfile] OFF