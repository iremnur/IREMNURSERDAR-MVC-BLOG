SET IDENTITY_INSERT [dbo].[Posts] ON
INSERT INTO [dbo].[Posts] ([ID], [postName], [Title], [DateTime], [Body]) VALUES (4, NULL, N'aa', N'2015-01-01 00:00:00', N'asd asd ASD')
INSERT INTO [dbo].[Posts] ([ID], [postName], [Title], [DateTime], [Body]) VALUES (5, NULL, N'Merhaba', N'2014-03-03 00:00:00', N'Welcome to irem''s blog')
INSERT INTO [dbo].[Posts] ([ID], [postName], [Title], [DateTime], [Body]) VALUES (6, NULL, N'News', N'2015-05-07 00:00:00', N'To keep things simple we are not going to build the commenting system instead we are going to use Disqus. I encourage you to build the commenting system by yourself and that would be a good exercise for you.

We are going to use ASP.NET MVC 4 to develop the application. I''m not good at Entity Framework and we are going to use Fluent NHibernate/NHibernate combo to build the data-access system. You could use Entity Framework if you like. Finally, we are going to use Ninject for dependency injection because of it''s simplicity.

In the first part of the series we are going to build the basic infrastructure of the blog. We are going to create the necessary model classes, data access components, controllers and views. At the end of this part we have a working blog where we can see the latest posts, read a complete post, browse posts based upon a category or tag and even search for interested posts.

In the second part, we are going to build an admin console to manage our posts, tags and categories.')
INSERT INTO [dbo].[Posts] ([ID], [postName], [Title], [DateTime], [Body]) VALUES (7, NULL, N'Neler oluyor hayatta', N'2015-05-20 00:00:00', N'ASP.NET is great for building standards-based websites with HTML5, CSS3, and JavaScript. ASP.NET supports three approaches for making web sites. ASP.NET Web Forms uses controls and an event-model for component-based development. ASP.NET MVC values separation of concerns and enables easier test-driven development. ASP.NET Web Pages prefers a single page model that mixes code and HTML markup. You can mix and match these techniques within one application depending on your needs - it''s all One ASP.NET.')
INSERT INTO [dbo].[Posts] ([ID], [postName], [Title], [DateTime], [Body]) VALUES (8, NULL, N'Naber', N'2015-05-21 00:00:00', N'Naber gelmedi senden bi haber..')
INSERT INTO [dbo].[Posts] ([ID], [postName], [Title], [DateTime], [Body]) VALUES (9, NULL, N'Date Time sız', N'2015-05-22 00:00:00', N'heyy')
SET IDENTITY_INSERT [dbo].[Posts] OFF
