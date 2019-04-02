INSERT INTO [dbo].[Users] ([UserName] ,[NormalizedUserName] ,[Email] ,[NormalizedEmail] ,[EmailConfirmed] ,[PasswordHash] ,[SecurityStamp] ,[ConcurrencyStamp] ,[PhoneNumberConfirmed] ,[TwoFactorEnabled] ,[LockoutEnabled] ,[AccessFailedCount])
VALUES
('administrator@opencatapult.net' ,'ADMINISTRATOR@OPENCATAPULT.NET' ,'administrator@opencatapult.net' ,'ADMINISTRATOR@OPENCATAPULT.NET' ,1 ,'AQAAAAEAACcQAAAAEKBBPo49hQnfSTCnZPTPvpdvqOA5YKXoS8XT6S4hbX9vVTzjKzgXGmUUKWnpOvyjhA==' ,'D4ZMGAXVOVP33V5FMDWVCZ7ZMH5R2JCK' ,NEWID() ,0 ,0 ,0 ,0),
('guest@opencatapult.net' ,'GUEST@OPENCATAPULT.NET' ,'guest@opencatapult.net' ,'GUEST@OPENCATAPULT.NET' ,1 ,'AQAAAAEAACcQAAAAEKBBPo49hQnfSTCnZPTPvpdvqOA5YKXoS8XT6S4hbX9vVTzjKzgXGmUUKWnpOvyjhA==' ,'D4ZMGAXVOVP33V5FMDWVCZ7ZMH5R2JCK' ,NEWID() ,0 ,0 ,0 ,0),
('basic1@opencatapult.net' ,'BASIC1@OPENCATAPULT.NET' ,'basic1@opencatapult.net' ,'BASIC1@OPENCATAPULT.NET' ,1 ,'AQAAAAEAACcQAAAAEKBBPo49hQnfSTCnZPTPvpdvqOA5YKXoS8XT6S4hbX9vVTzjKzgXGmUUKWnpOvyjhA==' ,'D4ZMGAXVOVP33V5FMDWVCZ7ZMH5R2JCK' ,NEWID() ,0 ,0 ,0 ,0),
('basic2@opencatapult.net' ,'BASIC2@OPENCATAPULT.NET' ,'basic2@opencatapult.net' ,'BASIC2@OPENCATAPULT.NET' ,1 ,'AQAAAAEAACcQAAAAEKBBPo49hQnfSTCnZPTPvpdvqOA5YKXoS8XT6S4hbX9vVTzjKzgXGmUUKWnpOvyjhA==' ,'D4ZMGAXVOVP33V5FMDWVCZ7ZMH5R2JCK' ,NEWID() ,0 ,0 ,0 ,0);

insert into UserProfile (Created, ConcurrencyStamp, FirstName, LastName, ApplicationUserId, IsActive) values
(GETDATE(), NEWID(), 'administrator', 'user', (select id from Users where UserName='administrator@opencatapult.net'),1),
(GETDATE(), NEWID(), 'guest', 'user', (select id from Users where UserName='guest@opencatapult.net'),1),
(GETDATE(), NEWID(), 'basic2', 'user', (select id from Users where UserName='basic1@opencatapult.net'),1),
(GETDATE(), NEWID(), 'basic2', 'user', (select id from Users where UserName='basic2@opencatapult.net'),1);

insert into UserRoles values 
((select id from Users where UserName='administrator@opencatapult.net'),1),
((select id from Users where UserName='guest@opencatapult.net'),3),
((select id from Users where UserName='basic1@opencatapult.net'),2),
((select id from Users where UserName='basic2@opencatapult.net'),2);