DECLARE @IdentityOutput table ( ID bigint )
DECLARE @IdentityValue bigint

BEGIN

INSERT INTO AUTHROLE(NAME) VALUES('Admin');
INSERT INTO AUTHROLE(NAME) VALUES('User');

INSERT INTO IMAGECONFIGURATIONS(SplashImageId, SectionImage1Id, SectionImage2Id, SectionImage3Id, Randomize) VALUES(0,0,0,0,1);

INSERT INTO PLANS(Name, Description, BannerHeader, BannerColour, ImagePathLarge, TargetGender, CreatedDate, Active) OUTPUT INSERTED.Id 
INTO @IdentityOutput 
VALUES('Shortcut to Shred','Get ready to burn fat, boost confidence and get in the best shape of your life! With as little cardio as possible and an enjoyable diet, get ready for.. The Shortcut to Shred!','Get Shredded!',0,'/Content/Images/Plans/ShortcutToShred.jpg',0, SYSDATETIME(),1);
select @IdentityValue = (select max(ID) from @IdentityOutput);

INSERT INTO ITEMS(PlanId, Name, Description, ImagePath, Price, ItemCategory, CreatedDate, Duration, Discriminator, ItemDiscontinued)
VALUES(@IdentityValue,'Shortcut to Shred','4 Week Shred','/Content/Images/Plans/ShortcutToShred.jpg',19.99,0, SYSDATETIME(),4,'PlanOption',0);
INSERT INTO ITEMS(PlanId, Name, Description, ImagePath, Price, ItemCategory, CreatedDate, Duration, Discriminator, ItemDiscontinued)
VALUES(@IdentityValue,'Shortcut to Shred','8 Week Shred','/Content/Images/Plans/ShortcutToShred.jpg',29.99,0, SYSDATETIME(),8,'PlanOption',0);
INSERT INTO ITEMS(PlanId, Name, Description, ImagePath, Price, ItemCategory, CreatedDate, Duration, Discriminator, ItemDiscontinued)
VALUES(@IdentityValue,'Shortcut to Shred','12 Week Shred','/Content/Images/Plans/ShortcutToShred.jpg',39.99,0, SYSDATETIME(),12,'PlanOption',0);

INSERT INTO PLANS(Name, Description, BannerHeader, BannerColour, ImagePathLarge, TargetGender, CreatedDate, Active) 
OUTPUT INSERTED.Id INTO @IdentityOutput 
VALUES('The Hulk Effect','You want it all! Solid muscle, with impressive strength to back it up. Combining compound strength training with a bodybuilding approach.. GET BIGGER, GET STRONGER!','Get Shredded!',0,'/Content/Images/Plans/TheHulkEffect.jpg',0, SYSDATETIME(),1);
select @IdentityValue = (select max(ID) from @IdentityOutput);

INSERT INTO ITEMS(PlanId, Name, Description, ImagePath, Price, ItemCategory, CreatedDate, Duration, Discriminator, ItemDiscontinued)
VALUES(@IdentityValue,'The Hulk Effect','4 Week Plan','/Content/Images/Plans/TheHulkEffect.jpg',19.99,0, SYSDATETIME(),4,'PlanOption',0);
INSERT INTO ITEMS(PlanId, Name, Description, ImagePath, Price, ItemCategory, CreatedDate, Duration, Discriminator, ItemDiscontinued)
VALUES(@IdentityValue,'The Hulk Effect','8 Week Plan','/Content/Images/Plans/TheHulkEffect.jpg',29.99,0, SYSDATETIME(),8,'PlanOption',0);
INSERT INTO ITEMS(PlanId, Name, Description, ImagePath, Price, ItemCategory, CreatedDate, Duration, Discriminator, ItemDiscontinued)
VALUES(@IdentityValue,'The Hulk Effect','12 Week Plan','/Content/Images/Plans/TheHulkEffect.jpg',39.99,0, SYSDATETIME(),12,'PlanOption',0);

INSERT INTO PLANS(Name, Description, BannerHeader, BannerColour, ImagePathLarge, TargetGender, CreatedDate, Active) 
OUTPUT INSERTED.Id INTO @IdentityOutput 
VALUES('Project Mass','Project Mass is a next level training, nutrition, and supplement program, designed to build as much muscle as possible, in the shortest space of time.. This is how you grow!','Muscle Building Blueprint',0,'/Content/Images/Plans/ProjectMass.jpg',0, SYSDATETIME(),1);
select @IdentityValue = (select max(ID) from @IdentityOutput);

INSERT INTO ITEMS(PlanId, Name, Description, ImagePath, Price, ItemCategory, CreatedDate, Duration, Discriminator, ItemDiscontinued)
VALUES(@IdentityValue,'Project Mass','4 Week Plan','/Content/Images/Plans/ProjectMass.jpg',19.99,0, SYSDATETIME(),4,'PlanOption',0);
INSERT INTO ITEMS(PlanId, Name, Description, ImagePath, Price, ItemCategory, CreatedDate, Duration, Discriminator, ItemDiscontinued)
VALUES(@IdentityValue,'Project Mass','8 Week Plan','/Content/Images/Plans/ProjectMass.jpg',29.99,0, SYSDATETIME(),8,'PlanOption',0);
INSERT INTO ITEMS(PlanId, Name, Description, ImagePath, Price, ItemCategory, CreatedDate, Duration, Discriminator, ItemDiscontinued)
VALUES(@IdentityValue,'Project Mass','12 Week Plan','/Content/Images/Plans/ProjectMass.jpg',39.99,0, SYSDATETIME(),12,'PlanOption',0);

INSERT INTO PLANS(Name, Description, BannerHeader, BannerColour, ImagePathLarge, TargetGender, CreatedDate, Active) 
OUTPUT INSERTED.Id INTO @IdentityOutput 
VALUES('Wonder Woman Physique','Shape up, gain strength, gain confidence.. Look wonderful!','Shape & Strength Shortcut',0,'/Content/Images/Plans/WonderWomanPhysique.jpg',1, SYSDATETIME(),1);
select @IdentityValue = (select max(ID) from @IdentityOutput);

INSERT INTO ITEMS(PlanId, Name, Description, ImagePath, Price, ItemCategory, CreatedDate, Duration, Discriminator, ItemDiscontinued)
VALUES(@IdentityValue,'Wonder Woman Physique','4 Week Plan','/Content/Images/Plans/WonderWomanPhysique.jpg',19.99,0, SYSDATETIME(),14,'PlanOption',0);
INSERT INTO ITEMS(PlanId, Name, Description, ImagePath, Price, ItemCategory, CreatedDate, Duration, Discriminator, ItemDiscontinued)
VALUES(@IdentityValue,'Wonder Woman Physique','8 Week Plan','/Content/Images/Plans/WonderWomanPhysique.jpg',29.99,0, SYSDATETIME(),8,'PlanOption',0);
INSERT INTO ITEMS(PlanId, Name, Description, ImagePath, Price, ItemCategory, CreatedDate, Duration, Discriminator, ItemDiscontinued)
VALUES(@IdentityValue,'Wonder Woman Physique','12 Week Plan','/Content/Images/Plans/WonderWomanPhysique.jpg',39.99,0, SYSDATETIME(),12,'PlanOption',0);

INSERT INTO PLANS(Name, Description, BannerHeader, BannerColour, ImagePathLarge, TargetGender, CreatedDate, Active) 
OUTPUT INSERTED.Id INTO @IdentityOutput 
VALUES('Ultimate Booty Builder','Specified training to get the best results in the shortest space of time.. Get ready to blow up that butt!','Booty Building Trainer',0,'/Content/Images/Plans/UltimateBootyBuilder.jpg',1, SYSDATETIME(),1);
select @IdentityValue = (select max(ID) from @IdentityOutput);

INSERT INTO ITEMS(PlanId, Name, Description, ImagePath, Price, ItemCategory, CreatedDate, Duration, Discriminator, ItemDiscontinued)
VALUES(@IdentityValue,'Ultimate Booty Builder','4 Week Plan','/Content/Images/Plans/UltimateBootyBuilder.jpg',19.99,0, SYSDATETIME(),14,'PlanOption',0);
INSERT INTO ITEMS(PlanId, Name, Description, ImagePath, Price, ItemCategory, CreatedDate, Duration, Discriminator, ItemDiscontinued)
VALUES(@IdentityValue,'Ultimate Booty Builder','8 Week Plan','/Content/Images/Plans/UltimateBootyBuilder.jpg',29.99,0, SYSDATETIME(),8,'PlanOption',0);
INSERT INTO ITEMS(PlanId, Name, Description, ImagePath, Price, ItemCategory, CreatedDate, Duration, Discriminator, ItemDiscontinued)
VALUES(@IdentityValue,'Ultimate Booty Builder','12 Week Plan','/Content/Images/Plans/UltimateBootyBuilder.jpg',39.99,0, SYSDATETIME(),12,'PlanOption',0);

INSERT INTO PLANS(Name, Description, BannerHeader, BannerColour, ImagePathLarge, TargetGender, CreatedDate, Active) 
OUTPUT INSERTED.Id INTO @IdentityOutput 
VALUES('Beach Body','Holiday booked? Summer on the way? Dust off those bikinis and get the body you''ve always wanted!','Bikini ready, in no time!',0,'/Content/Images/Plans/BeachBody.jpg',1, SYSDATETIME(),1);
select @IdentityValue = (select max(ID) from @IdentityOutput);

INSERT INTO ITEMS(PlanId, Name, Description, ImagePath, Price, ItemCategory, CreatedDate, Duration, Discriminator, ItemDiscontinued)
VALUES(@IdentityValue,'Beach Body','4 Week Plan','/Content/Images/Plans/BeachBody.jpg',19.99,0, SYSDATETIME(),14,'PlanOption',0);
INSERT INTO ITEMS(PlanId, Name, Description, ImagePath, Price, ItemCategory, CreatedDate, Duration, Discriminator, ItemDiscontinued)
VALUES(@IdentityValue,'Beach Body','8 Week Plan','/Content/Images/Plans/BeachBody.jpg',29.99,0, SYSDATETIME(),8,'PlanOption',0);
INSERT INTO ITEMS(PlanId, Name, Description, ImagePath, Price, ItemCategory, CreatedDate, Duration, Discriminator, ItemDiscontinued)
VALUES(@IdentityValue,'Beach Body','12 Week Plan','/Content/Images/Plans/BeachBody.jpg',39.99,0, SYSDATETIME(),12,'PlanOption',0);

--BLOGS
INSERT INTO BLOGS(Title, SubHeader, CreatedDate, Content, ImagePath, Active) 
OUTPUT INSERTED.Id INTO @IdentityOutput 
VALUES('My First Blog','Welcome to Joel Scott Fitness',SYSDATETIME(),'Lorem ipsum dolor sit amet, consectetur adipiscing elit. Praesent maximus pretium ante id elementum. Etiam sit amet dapibus purus. Curabitur posuere dui vel porta aliquet. Etiam commodo dolor ligula, in tempor ante eleifend eget. Aliquam erat volutpat. Ut euismod porttitor leo. Pellentesque habitant morbi tristique senectus et netus et malesuada fames ac turpis egestas. Pellentesque tempor nisi nec lobortis rhoncus. In molestie purus eget tristique consequat.','/Content/Images/Blogs/Desert.jpg',1);
select @IdentityValue = (select max(ID) from @IdentityOutput);

INSERT INTO BLOGIMAGES(BlogId, ImagePath, CaptionTitle, Caption, CaptionColour) 
VALUES(@IdentityValue,'/Content/Images/Blogs/ladies1.jpg','My First Blog Image','Welcome to my first blog image',1);
INSERT INTO BLOGIMAGES(BlogId, ImagePath, CaptionTitle, Caption, CaptionColour) 
VALUES(@IdentityValue,'/Content/Images/Blogs/ladies2.jpg','My Second Blog Image','Welcome to my second blog image',1);
INSERT INTO BLOGIMAGES(BlogId, ImagePath, CaptionTitle, Caption, CaptionColour) 
VALUES(@IdentityValue,'/Content/Images/Blogs/ladies3.jpg','My Third Blog Image','This was interesting',1);

INSERT INTO BLOGS(Title, SubHeader, CreatedDate, Content, ImagePath, Active) 
OUTPUT INSERTED.Id INTO @IdentityOutput 
VALUES('Here We Go Again!','My Second Blog',SYSDATETIME(),'Lorem ipsum dolor sit amet, consectetur adipiscing elit. Praesent maximus pretium ante id elementum. Etiam sit amet dapibus purus. Curabitur posuere dui vel porta aliquet. Etiam commodo dolor ligula, in tempor ante eleifend eget. Aliquam erat volutpat. Ut euismod porttitor leo. Pellentesque habitant morbi tristique senectus et netus et malesuada fames ac turpis egestas. Pellentesque tempor nisi nec lobortis rhoncus. In molestie purus eget tristique consequat.','/Content/Images/Blogs/Chrys.jpg',1);
select @IdentityValue = (select max(ID) from @IdentityOutput);

INSERT INTO BLOGS(Title, SubHeader, CreatedDate, Content, ImagePath, Active) 
OUTPUT INSERTED.Id INTO @IdentityOutput 
VALUES('My First Blog','Welcome to Joel Scott Fitness',SYSDATETIME(),'Lorem ipsum dolor sit amet, consectetur adipiscing elit. Praesent maximus pretium ante id elementum. Etiam sit amet dapibus purus. Curabitur posuere dui vel porta aliquet. Etiam commodo dolor ligula, in tempor ante eleifend eget. Aliquam erat volutpat. Ut euismod porttitor leo. Pellentesque habitant morbi tristique senectus et netus et malesuada fames ac turpis egestas. Pellentesque tempor nisi nec lobortis rhoncus. In molestie purus eget tristique consequat.','/Content/Images/Blogs/Desert.jpg',1);
select @IdentityValue = (select max(ID) from @IdentityOutput);

INSERT INTO BLOGS(Title, SubHeader, CreatedDate, Content, ImagePath, Active) 
OUTPUT INSERTED.Id INTO @IdentityOutput 
VALUES('Here We Go Again!','My Second Blog',SYSDATETIME(),'Lorem ipsum dolor sit amet, consectetur adipiscing elit. Praesent maximus pretium ante id elementum. Etiam sit amet dapibus purus. Curabitur posuere dui vel porta aliquet. Etiam commodo dolor ligula, in tempor ante eleifend eget. Aliquam erat volutpat. Ut euismod porttitor leo. Pellentesque habitant morbi tristique senectus et netus et malesuada fames ac turpis egestas. Pellentesque tempor nisi nec lobortis rhoncus. In molestie purus eget tristique consequat.','/Content/Images/Blogs/Chrys.jpg',1);
select @IdentityValue = (select max(ID) from @IdentityOutput);

INSERT INTO BLOGS(Title, SubHeader, CreatedDate, Content, ImagePath, Active) 
OUTPUT INSERTED.Id INTO @IdentityOutput 
VALUES('My First Blog','Welcome to Joel Scott Fitness',SYSDATETIME(),'Lorem ipsum dolor sit amet, consectetur adipiscing elit. Praesent maximus pretium ante id elementum. Etiam sit amet dapibus purus. Curabitur posuere dui vel porta aliquet. Etiam commodo dolor ligula, in tempor ante eleifend eget. Aliquam erat volutpat. Ut euismod porttitor leo. Pellentesque habitant morbi tristique senectus et netus et malesuada fames ac turpis egestas. Pellentesque tempor nisi nec lobortis rhoncus. In molestie purus eget tristique consequat.','/Content/Images/Blogs/Desert.jpg',1);
select @IdentityValue = (select max(ID) from @IdentityOutput);

INSERT INTO BLOGS(Title, SubHeader, CreatedDate, Content, ImagePath, Active) 
OUTPUT INSERTED.Id INTO @IdentityOutput 
VALUES('Here We Go Again!','My Second Blog',SYSDATETIME(),'Lorem ipsum dolor sit amet, consectetur adipiscing elit. Praesent maximus pretium ante id elementum. Etiam sit amet dapibus purus. Curabitur posuere dui vel porta aliquet. Etiam commodo dolor ligula, in tempor ante eleifend eget. Aliquam erat volutpat. Ut euismod porttitor leo. Pellentesque habitant morbi tristique senectus et netus et malesuada fames ac turpis egestas. Pellentesque tempor nisi nec lobortis rhoncus. In molestie purus eget tristique consequat.','/Content/Images/Blogs/Chrys.jpg',1);
select @IdentityValue = (select max(ID) from @IdentityOutput);

-- IMAGES
INSERT INTO IMAGES(ImagePath) VALUES('/Content/Images/BGZD3738.JPG');
INSERT INTO IMAGES(ImagePath) VALUES('/Content/Images/BXJA2014.JPG');
INSERT INTO IMAGES(ImagePath) VALUES('/Content/Images/CZOB9407.JPG');
INSERT INTO IMAGES(ImagePath) VALUES('/Content/Images/FIOI1024.JPG');
INSERT INTO IMAGES(ImagePath) VALUES('/Content/Images/IMG_00001.JPG');
INSERT INTO IMAGES(ImagePath) VALUES('/Content/Images/IMG_00002.JPG');
INSERT INTO IMAGES(ImagePath) VALUES('/Content/Images/IMG_00003.JPG');
INSERT INTO IMAGES(ImagePath) VALUES('/Content/Images/IMG_00004.JPG');
INSERT INTO IMAGES(ImagePath) VALUES('/Content/Images/IMG_00005.JPG');
INSERT INTO IMAGES(ImagePath) VALUES('/Content/Images/IMG_00006.JPG');
INSERT INTO IMAGES(ImagePath) VALUES('/Content/Images/IMG_00007.JPG');
INSERT INTO IMAGES(ImagePath) VALUES('/Content/Images/IMG_00008.JPG');
INSERT INTO IMAGES(ImagePath) VALUES('/Content/Images/IMG_00010.JPG');
INSERT INTO IMAGES(ImagePath) VALUES('/Content/Images/IMG_00011.JPG');
INSERT INTO IMAGES(ImagePath) VALUES('/Content/Images/IMG_00012.JPG');
INSERT INTO IMAGES(ImagePath) VALUES('/Content/Images/IMG_00013.JPG');
INSERT INTO IMAGES(ImagePath) VALUES('/Content/Images/IMG_00014.JPG');
INSERT INTO IMAGES(ImagePath) VALUES('/Content/Images/IMG_00015.JPG');
INSERT INTO IMAGES(ImagePath) VALUES('/Content/Images/IMG_00016.JPG');
INSERT INTO IMAGES(ImagePath) VALUES('/Content/Images/IMG_00017.JPG');
INSERT INTO IMAGES(ImagePath) VALUES('/Content/Images/IMG_00018.JPG');
INSERT INTO IMAGES(ImagePath) VALUES('/Content/Images/IMG_00019.JPG');
INSERT INTO IMAGES(ImagePath) VALUES('/Content/Images/IMG_00020.JPG');
INSERT INTO IMAGES(ImagePath) VALUES('/Content/Images/IMG_00021.JPG');
INSERT INTO IMAGES(ImagePath) VALUES('/Content/Images/IMG_6173.JPG');
INSERT INTO IMAGES(ImagePath) VALUES('/Content/Images/IMG_6313.jpg');
INSERT INTO IMAGES(ImagePath) VALUES('/Content/Images/UWBL7758.JPG');


END;
GO







