USE [NewAssetDB]
GO
INSERT [dbo].[AspNetRoles] ([Id], [Discriminator], [RoleCategoryId], [DisplayName], [Name], [NormalizedName], [ConcurrencyStamp]) VALUES (N'23c7042f-86c3-4800-b743-271336d574af', N'ApplicationRole', 2, N'Top Level Manager', N'TLManager', N'TLMANAGER', N'63358d01-6816-432f-9583-dec363c09fc9')
INSERT [dbo].[AspNetRoles] ([Id], [Discriminator], [RoleCategoryId], [DisplayName], [Name], [NormalizedName], [ConcurrencyStamp]) VALUES (N'45824027-5955-4304-85a0-4ad495d1cfd6', N'ApplicationRole', 1, N'Top Level Manager UPA', N'TLUPAManager', N'TLUPAMANAGER', N'2f653698-73c8-493f-b5a1-b1241e4bad1d')
INSERT [dbo].[AspNetRoles] ([Id], [Discriminator], [RoleCategoryId], [DisplayName], [Name], [NormalizedName], [ConcurrencyStamp]) VALUES (N'6cb5eb34-07ec-4c19-a580-73b874edf6dc', N'ApplicationRole', 5, N'Reviewer', N'SRReviewer', N'SRREVIEWER', N'012d1ee9-f24c-43f4-89a3-fef582c78d03')
INSERT [dbo].[AspNetRoles] ([Id], [Discriminator], [RoleCategoryId], [DisplayName], [Name], [NormalizedName], [ConcurrencyStamp]) VALUES (N'8d1a729e-decd-4b26-b11f-b684bf3c1d10', N'ApplicationRole', 1, N'Service Requests approval', N'UPASRAcceptance', N'UPASRACCEPTANCE', N'f2ed48ca-d598-4b39-9fa7-f05647410a1b')
INSERT [dbo].[AspNetRoles] ([Id], [Discriminator], [RoleCategoryId], [DisplayName], [Name], [NormalizedName], [ConcurrencyStamp]) VALUES (N'a34e430c-594a-4867-9189-6f190da13ab2', N'ApplicationRole', 5, N'Top Level Manager Hospital', N'TLHospitalManager', N'TLHOSPITALMANAGER', N'8282d2cd-612d-4ec2-a69e-926395742c77')
INSERT [dbo].[AspNetRoles] ([Id], [Discriminator], [RoleCategoryId], [DisplayName], [Name], [NormalizedName], [ConcurrencyStamp]) VALUES (N'afe3907b-14d2-4909-85e2-d3c30b45adf6', N'ApplicationRole', 2, N'Accept SR', N'OrgSRAcceptance', N'ORGSRACCEPTANCE', N'00f8b5a1-1fd5-45b2-940e-3936b4a9d467')
INSERT [dbo].[AspNetRoles] ([Id], [Discriminator], [RoleCategoryId], [DisplayName], [Name], [NormalizedName], [ConcurrencyStamp]) VALUES (N'bff2de0c-0471-44e5-9d9a-531643be082e', N'ApplicationRole', 5, N'Data Entry', N'DE', N'DE', N'2ff3ce32-3964-4b30-b919-1e58b61a323f')
INSERT [dbo].[AspNetRoles] ([Id], [Discriminator], [RoleCategoryId], [DisplayName], [Name], [NormalizedName], [ConcurrencyStamp]) VALUES (N'c0398f6e-c75c-4fe9-981a-b182e2503686', N'ApplicationRole', 4, N'Top Level Manager City', N'TLCityManager', N'TLCITYMANAGER', N'7e3d829e-81a8-410b-990a-954bdb091d0e')
INSERT [dbo].[AspNetRoles] ([Id], [Discriminator], [RoleCategoryId], [DisplayName], [Name], [NormalizedName], [ConcurrencyStamp]) VALUES (N'c476e1c5-f4fa-4b49-84d9-5d5f71c31d60', N'ApplicationRole', 5, N'Service Requests Creator', N'SRCreator', N'SRCREATOR', N'449bbda3-9eba-4d1a-a3b7-d6842baf1196')
INSERT [dbo].[AspNetRoles] ([Id], [Discriminator], [RoleCategoryId], [DisplayName], [Name], [NormalizedName], [ConcurrencyStamp]) VALUES (N'df288d91-09ed-462d-a3da-e08e4ecc498e', N'ApplicationRole', 1, N'Admin', N'Admin', N'ADMIN', N'81e4e363-87b0-496b-ad59-932d8f573a6e')
INSERT [dbo].[AspNetRoles] ([Id], [Discriminator], [RoleCategoryId], [DisplayName], [Name], [NormalizedName], [ConcurrencyStamp]) VALUES (N'f9f5e66c-d51f-4a6e-abb3-8b2efbbac0f7', N'ApplicationRole', 3, N'Top Level Manager Governorate', N'TLGovManager', N'TLGOVMANAGER', N'd29c0dce-a28a-403b-9945-79eca71eec4d')
INSERT [dbo].[AspNetUsers] ([Id], [GovernorateId], [CityId], [OrganizationId], [SubOrganizationId], [HospitalId], [RoleId], [RoleCategoryId], [UserName], [NormalizedUserName], [Email], [NormalizedEmail], [EmailConfirmed], [PasswordHash], [SecurityStamp], [ConcurrencyStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEnd], [LockoutEnabled], [AccessFailedCount]) VALUES (N'26c8d274-2dee-4051-8056-e6d8211e4da8', 0, 0, 0, 0, 0, N'df288d91-09ed-462d-a3da-e08e4ecc498e', 1, N'Admin', N'ADMIN', N'admin@domain.com', N'ADMIN@DOMAIN.COM', 0, N'AQAAAAEAACcQAAAAEGTuKMXJq4oMS9SPql89TFFZGxnbWnw87BHqTxTKSwLPsweiKqGpI3AshbW7ZPqARQ==', N'C5L3QOE6FHHJ7TNCPXYW5V4J3M4KVS7P', N'6440ca95-c5e5-4133-835f-970ccbc8ef02', N'01114406203', 0, 0, NULL, 1, 0)
SET IDENTITY_INSERT [dbo].[AspNetUserClaims] ON 

INSERT [dbo].[AspNetUserClaims] ([Id], [UserId], [ClaimType], [ClaimValue]) VALUES (1, N'26c8d274-2dee-4051-8056-e6d8211e4da8', N'http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name', N'Admin')
INSERT [dbo].[AspNetUserClaims] ([Id], [UserId], [ClaimType], [ClaimValue]) VALUES (2, N'26c8d274-2dee-4051-8056-e6d8211e4da8', N'http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress', N'admin@domain.com')
INSERT [dbo].[AspNetUserClaims] ([Id], [UserId], [ClaimType], [ClaimValue]) VALUES (3, N'26c8d274-2dee-4051-8056-e6d8211e4da8', N'key', N'value')
INSERT [dbo].[AspNetUserClaims] ([Id], [UserId], [ClaimType], [ClaimValue]) VALUES (4, N'26c8d274-2dee-4051-8056-e6d8211e4da8', N'jti', N'7f748d2c-ea7d-4fa0-9423-b67f7dff5642')
INSERT [dbo].[AspNetUserClaims] ([Id], [UserId], [ClaimType], [ClaimValue]) VALUES (5, N'26c8d274-2dee-4051-8056-e6d8211e4da8', N'http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name', N'Admin')
INSERT [dbo].[AspNetUserClaims] ([Id], [UserId], [ClaimType], [ClaimValue]) VALUES (6, N'26c8d274-2dee-4051-8056-e6d8211e4da8', N'http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress', N'admin@domain.com')
INSERT [dbo].[AspNetUserClaims] ([Id], [UserId], [ClaimType], [ClaimValue]) VALUES (7, N'26c8d274-2dee-4051-8056-e6d8211e4da8', N'key', N'value')
INSERT [dbo].[AspNetUserClaims] ([Id], [UserId], [ClaimType], [ClaimValue]) VALUES (8, N'26c8d274-2dee-4051-8056-e6d8211e4da8', N'jti', N'6722b090-d5f3-4926-973a-1080d2448bbd')
INSERT [dbo].[AspNetUserClaims] ([Id], [UserId], [ClaimType], [ClaimValue]) VALUES (9, N'26c8d274-2dee-4051-8056-e6d8211e4da8', N'http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name', N'Admin')
INSERT [dbo].[AspNetUserClaims] ([Id], [UserId], [ClaimType], [ClaimValue]) VALUES (10, N'26c8d274-2dee-4051-8056-e6d8211e4da8', N'http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress', N'admin@domain.com')
INSERT [dbo].[AspNetUserClaims] ([Id], [UserId], [ClaimType], [ClaimValue]) VALUES (11, N'26c8d274-2dee-4051-8056-e6d8211e4da8', N'key', N'value')
INSERT [dbo].[AspNetUserClaims] ([Id], [UserId], [ClaimType], [ClaimValue]) VALUES (12, N'26c8d274-2dee-4051-8056-e6d8211e4da8', N'jti', N'6fdc2d91-46eb-4895-b69a-bc738af97c16')
INSERT [dbo].[AspNetUserClaims] ([Id], [UserId], [ClaimType], [ClaimValue]) VALUES (13, N'26c8d274-2dee-4051-8056-e6d8211e4da8', N'http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name', N'Admin')
INSERT [dbo].[AspNetUserClaims] ([Id], [UserId], [ClaimType], [ClaimValue]) VALUES (14, N'26c8d274-2dee-4051-8056-e6d8211e4da8', N'http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress', N'admin@domain.com')
INSERT [dbo].[AspNetUserClaims] ([Id], [UserId], [ClaimType], [ClaimValue]) VALUES (15, N'26c8d274-2dee-4051-8056-e6d8211e4da8', N'key', N'value')
INSERT [dbo].[AspNetUserClaims] ([Id], [UserId], [ClaimType], [ClaimValue]) VALUES (16, N'26c8d274-2dee-4051-8056-e6d8211e4da8', N'jti', N'3e7bfac3-5075-4a79-995b-f612af79f61a')
INSERT [dbo].[AspNetUserClaims] ([Id], [UserId], [ClaimType], [ClaimValue]) VALUES (17, N'26c8d274-2dee-4051-8056-e6d8211e4da8', N'http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name', N'Admin')
INSERT [dbo].[AspNetUserClaims] ([Id], [UserId], [ClaimType], [ClaimValue]) VALUES (19, N'26c8d274-2dee-4051-8056-e6d8211e4da8', N'http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress', N'admin@domain.com')
INSERT [dbo].[AspNetUserClaims] ([Id], [UserId], [ClaimType], [ClaimValue]) VALUES (22, N'26c8d274-2dee-4051-8056-e6d8211e4da8', N'key', N'value')
INSERT [dbo].[AspNetUserClaims] ([Id], [UserId], [ClaimType], [ClaimValue]) VALUES (23, N'26c8d274-2dee-4051-8056-e6d8211e4da8', N'jti', N'f3722b61-69c9-4647-b6ac-1bcb34396831')
SET IDENTITY_INSERT [dbo].[AspNetUserClaims] OFF
INSERT [dbo].[__EFMigrationsHistory] ([MigrationId], [ProductVersion]) VALUES (N'20210926094719_ddd', N'5.0.9')
SET IDENTITY_INSERT [dbo].[AssetPeriorities] ON 

INSERT [dbo].[AssetPeriorities] ([Id], [Name], [NameAr]) VALUES (1, N'High', N'عالي')
INSERT [dbo].[AssetPeriorities] ([Id], [Name], [NameAr]) VALUES (2, N'Medium', N'وسط')
INSERT [dbo].[AssetPeriorities] ([Id], [Name], [NameAr]) VALUES (3, N'Low', N'منخفض')
SET IDENTITY_INSERT [dbo].[AssetPeriorities] OFF
SET IDENTITY_INSERT [dbo].[Brands] ON 

INSERT [dbo].[Brands] ([Id], [Code], [Name], [NameAr]) VALUES (1, N'01', N'Philips', N'فيليبس')
INSERT [dbo].[Brands] ([Id], [Code], [Name], [NameAr]) VALUES (2, N'02', N'LG', N'ال جي')
SET IDENTITY_INSERT [dbo].[Brands] OFF
SET IDENTITY_INSERT [dbo].[Categories] ON 

INSERT [dbo].[Categories] ([Id], [Code], [Name], [NameAr]) VALUES (1, N'01', N'General', N'عام')
INSERT [dbo].[Categories] ([Id], [Code], [Name], [NameAr]) VALUES (2, N'02', N'DC Shock', N'صدمات')
SET IDENTITY_INSERT [dbo].[Categories] OFF
SET IDENTITY_INSERT [dbo].[Cities] ON 

INSERT [dbo].[Cities] ([Id], [Code], [Name], [NameAr], [GovernorateId]) VALUES (1, N'01', N'Cairo City', N'القاهرة', 1)
INSERT [dbo].[Cities] ([Id], [Code], [Name], [NameAr], [GovernorateId]) VALUES (2, N'02', N'Giza', N'الجيزة', 1)
INSERT [dbo].[Cities] ([Id], [Code], [Name], [NameAr], [GovernorateId]) VALUES (3, N'03', N'Borg El Arab', N'برج العرب', 3)
INSERT [dbo].[Cities] ([Id], [Code], [Name], [NameAr], [GovernorateId]) VALUES (4, N'04', N'Gleem', N'جليم', 3)
INSERT [dbo].[Cities] ([Id], [Code], [Name], [NameAr], [GovernorateId]) VALUES (5, N'05', N'East alex', N'شرق الاسكندرية', 3)
INSERT [dbo].[Cities] ([Id], [Code], [Name], [NameAr], [GovernorateId]) VALUES (6, N'06', N'Nagaa hamady', N'نجع حمادي', 4)
SET IDENTITY_INSERT [dbo].[Cities] OFF
SET IDENTITY_INSERT [dbo].[Governorates] ON 

INSERT [dbo].[Governorates] ([Id], [Code], [Name], [NameAr]) VALUES (1, N'01', N'Cairo', N'القاهرة')
INSERT [dbo].[Governorates] ([Id], [Code], [Name], [NameAr]) VALUES (2, N'02', N'Alexandria', N'الاسكندرية')
INSERT [dbo].[Governorates] ([Id], [Code], [Name], [NameAr]) VALUES (3, N'03', N'Qena', N'قنا')
SET IDENTITY_INSERT [dbo].[Governorates] OFF
SET IDENTITY_INSERT [dbo].[MasterAssets] ON 

INSERT [dbo].[MasterAssets] ([Id], [Name], [NameAr], [Code], [Description], [DescriptionAr], [ExpectedLifeTime], [UPACode], [ModelNumber], [VersionNumber], [PeriorityId], [OriginId], [BrandId], [CategoryId], [SubCategoryId], [Length], [Height], [Width], [Weight]) VALUES (1, N'Asset 1', N'Asset 1 Ar', N'01', N'sdsfdg', N'fgfdhj', 10, N'ee123', N'kkdsfd5685454', N'fdrf5434424', 1, NULL, 1, 1, 1, 300, 100, 200, 400)
SET IDENTITY_INSERT [dbo].[MasterAssets] OFF
SET IDENTITY_INSERT [dbo].[Organizations] ON 

INSERT [dbo].[Organizations] ([Id], [Code], [Name], [NameAr], [Address], [AddressAr], [Email], [Mobile], [DirectorName], [DirectorNameAr]) VALUES (1, N'01', N'Police Hospitals', N'مسنشفيات الشرطة', N'', N'', N'', N'', N'', N'')
INSERT [dbo].[Organizations] ([Id], [Code], [Name], [NameAr], [Address], [AddressAr], [Email], [Mobile], [DirectorName], [DirectorNameAr]) VALUES (2, N'03', N'Medical Insurance institute', N'هيئة التأمين الصحي', N'', N'', N'', N'', N'', N'')
INSERT [dbo].[Organizations] ([Id], [Code], [Name], [NameAr], [Address], [AddressAr], [Email], [Mobile], [DirectorName], [DirectorNameAr]) VALUES (3, N'10', N'Rail Ways', N' هيئة السكك الحديد', N'', N'', N'', N'', N'', N'')
SET IDENTITY_INSERT [dbo].[Organizations] OFF
SET IDENTITY_INSERT [dbo].[Origins] ON 

INSERT [dbo].[Origins] ([Id], [Code], [Name], [NameAr]) VALUES (1, N'AF', N'Afghanistan ', N'أفغانستان')
INSERT [dbo].[Origins] ([Id], [Code], [Name], [NameAr]) VALUES (2, N'AL', N'Albania ', N'ألبانيا')
INSERT [dbo].[Origins] ([Id], [Code], [Name], [NameAr]) VALUES (3, N'DZ', N'Algeria ', N'الجزائر')
INSERT [dbo].[Origins] ([Id], [Code], [Name], [NameAr]) VALUES (4, N'AS', N'American Samoa ', N'ساموا الأمريكية')
INSERT [dbo].[Origins] ([Id], [Code], [Name], [NameAr]) VALUES (5, N'AD', N'Andorra ', N'أندورا')
INSERT [dbo].[Origins] ([Id], [Code], [Name], [NameAr]) VALUES (6, N'AO', N'Angola ', N'أنجولا')
INSERT [dbo].[Origins] ([Id], [Code], [Name], [NameAr]) VALUES (7, N'AI', N'Anguilla ', N'أنجويلا')
INSERT [dbo].[Origins] ([Id], [Code], [Name], [NameAr]) VALUES (8, N'AQ', N'Antarctica', N'أنتراكتيكا')
INSERT [dbo].[Origins] ([Id], [Code], [Name], [NameAr]) VALUES (9, N'AG', N'Antigua and Barbuda ', N'أنتيغوا وباربودا')
INSERT [dbo].[Origins] ([Id], [Code], [Name], [NameAr]) VALUES (10, N'AR', N'Argentina ', N'الأرجنتين')
INSERT [dbo].[Origins] ([Id], [Code], [Name], [NameAr]) VALUES (11, N'AM', N'Armenia ', N'أرمينيا')
INSERT [dbo].[Origins] ([Id], [Code], [Name], [NameAr]) VALUES (12, N'AW', N'Aruba ', N'أروبا')
INSERT [dbo].[Origins] ([Id], [Code], [Name], [NameAr]) VALUES (13, N'AU', N'Australia ', N'أستراليا')
INSERT [dbo].[Origins] ([Id], [Code], [Name], [NameAr]) VALUES (14, N'AT', N'Austria ', N'النمسا')
INSERT [dbo].[Origins] ([Id], [Code], [Name], [NameAr]) VALUES (15, N'AZ', N'Azerbaijan ', N'أذربيجان')
INSERT [dbo].[Origins] ([Id], [Code], [Name], [NameAr]) VALUES (16, N'BS', N'Bahamas ', N'البهاما')
INSERT [dbo].[Origins] ([Id], [Code], [Name], [NameAr]) VALUES (17, N'BH', N'Bahrain ', N'البحرين')
INSERT [dbo].[Origins] ([Id], [Code], [Name], [NameAr]) VALUES (18, N'BD', N'Bangladesh ', N'بنغلاديش')
INSERT [dbo].[Origins] ([Id], [Code], [Name], [NameAr]) VALUES (19, N'BB', N'Barbados ', N'باربادوس')
INSERT [dbo].[Origins] ([Id], [Code], [Name], [NameAr]) VALUES (20, N'BY', N'Belarus ', N'بيلاروسيا')
INSERT [dbo].[Origins] ([Id], [Code], [Name], [NameAr]) VALUES (21, N'BE', N'Belgium ', N'بلجيكا')
INSERT [dbo].[Origins] ([Id], [Code], [Name], [NameAr]) VALUES (22, N'BZ', N'Belize ', N'بليز')
INSERT [dbo].[Origins] ([Id], [Code], [Name], [NameAr]) VALUES (23, N'BJ', N'Benin ', N'البينين')
INSERT [dbo].[Origins] ([Id], [Code], [Name], [NameAr]) VALUES (24, N'BM', N'Bermuda ', N'برمودا')
INSERT [dbo].[Origins] ([Id], [Code], [Name], [NameAr]) VALUES (25, N'BT', N'Bhutan ', N'بوتان')
INSERT [dbo].[Origins] ([Id], [Code], [Name], [NameAr]) VALUES (26, N'BO', N'Bolivia ', N'بوليفيا')
INSERT [dbo].[Origins] ([Id], [Code], [Name], [NameAr]) VALUES (27, N'BA', N'Bosnia and Herzegovina ', N'البوسنة والهرسك')
INSERT [dbo].[Origins] ([Id], [Code], [Name], [NameAr]) VALUES (28, N'BW', N'Botswana ', N'بوتسوانا')
INSERT [dbo].[Origins] ([Id], [Code], [Name], [NameAr]) VALUES (29, N'BR', N'Brazil ', N'البرازيل')
INSERT [dbo].[Origins] ([Id], [Code], [Name], [NameAr]) VALUES (30, N'IO', N'British Indian Ocean Territory ', N'إقليم المحيط الهندي البريطاني')
INSERT [dbo].[Origins] ([Id], [Code], [Name], [NameAr]) VALUES (31, N'VG', N'British Virgin Islands ', N'جزر فيرجين البريطانية')
INSERT [dbo].[Origins] ([Id], [Code], [Name], [NameAr]) VALUES (32, N'BN', N'Brunei ', N'بروناي')
INSERT [dbo].[Origins] ([Id], [Code], [Name], [NameAr]) VALUES (33, N'BG', N'Bulgaria ', N'بلغاريا')
INSERT [dbo].[Origins] ([Id], [Code], [Name], [NameAr]) VALUES (34, N'BF', N'Burkina Faso ', N'بوركينا فاسو')
INSERT [dbo].[Origins] ([Id], [Code], [Name], [NameAr]) VALUES (35, N'MM', N'Burma (Myanmar) ', N'بورما - ميانمار')
INSERT [dbo].[Origins] ([Id], [Code], [Name], [NameAr]) VALUES (36, N'BI', N'Burundi ', N'بوروندي')
INSERT [dbo].[Origins] ([Id], [Code], [Name], [NameAr]) VALUES (37, N'KH', N'Cambodia ', N'كمبوديا')
INSERT [dbo].[Origins] ([Id], [Code], [Name], [NameAr]) VALUES (38, N'CM', N'Cameroon ', N'الكاميرون')
INSERT [dbo].[Origins] ([Id], [Code], [Name], [NameAr]) VALUES (39, N'CA', N'Canada ', N'كندا')
INSERT [dbo].[Origins] ([Id], [Code], [Name], [NameAr]) VALUES (40, N'CV', N'Cape Verde ', N'كاب فيردي ( الرأس الأخضر)')
INSERT [dbo].[Origins] ([Id], [Code], [Name], [NameAr]) VALUES (41, N'KY', N'Cayman Islands ', N'جزر كايمان')
INSERT [dbo].[Origins] ([Id], [Code], [Name], [NameAr]) VALUES (42, N'CF', N'Central African Republic ', N'جمهورية أفريقيا الوسطي')
INSERT [dbo].[Origins] ([Id], [Code], [Name], [NameAr]) VALUES (43, N'TD', N'Chad ', N'تشاد')
INSERT [dbo].[Origins] ([Id], [Code], [Name], [NameAr]) VALUES (44, N'CL', N'Chile ', N'شيلي')
INSERT [dbo].[Origins] ([Id], [Code], [Name], [NameAr]) VALUES (45, N'CN', N'China ', N'الصين')
INSERT [dbo].[Origins] ([Id], [Code], [Name], [NameAr]) VALUES (46, N'CX', N'Christmas Island ', N'جزر كريسماس')
INSERT [dbo].[Origins] ([Id], [Code], [Name], [NameAr]) VALUES (47, N'CC', N'Cocos (Keeling) Islands ', N'جزر كوكوس')
INSERT [dbo].[Origins] ([Id], [Code], [Name], [NameAr]) VALUES (48, N'CO', N'Colombia ', N'كولومبيا')
INSERT [dbo].[Origins] ([Id], [Code], [Name], [NameAr]) VALUES (49, N'KM', N'Comoros ', N'جزر القُمُر')
INSERT [dbo].[Origins] ([Id], [Code], [Name], [NameAr]) VALUES (50, N'CK', N'Cook Islands ', N'جزر كوك')
INSERT [dbo].[Origins] ([Id], [Code], [Name], [NameAr]) VALUES (51, N'CR', N'Costa Rica ', N'كوستا ريكا')
INSERT [dbo].[Origins] ([Id], [Code], [Name], [NameAr]) VALUES (52, N'CI', N'Côte d''Ivoire', N'كوت ديفوار')
INSERT [dbo].[Origins] ([Id], [Code], [Name], [NameAr]) VALUES (53, N'HR', N'Croatia ', N'كرواتيا ')
INSERT [dbo].[Origins] ([Id], [Code], [Name], [NameAr]) VALUES (54, N'CU', N'Cuba ', N'كوبا')
INSERT [dbo].[Origins] ([Id], [Code], [Name], [NameAr]) VALUES (55, N'CY', N'Cyprus ', N'قبرص')
INSERT [dbo].[Origins] ([Id], [Code], [Name], [NameAr]) VALUES (56, N'CZ', N'Czech Republic ', N'جمهورية التشيك')
INSERT [dbo].[Origins] ([Id], [Code], [Name], [NameAr]) VALUES (57, N'ZR', N'Democratic Republic of the Congo (Zaire)', N'جمهورية الكونغو الديمقراطية ')
INSERT [dbo].[Origins] ([Id], [Code], [Name], [NameAr]) VALUES (58, N'DK', N'Denmark ', N'الدنمارك / الدانمرك')
INSERT [dbo].[Origins] ([Id], [Code], [Name], [NameAr]) VALUES (59, N'DJ', N'Djibouti ', N'جيبوتي')
INSERT [dbo].[Origins] ([Id], [Code], [Name], [NameAr]) VALUES (60, N'DM', N'Dominica ', N'دومنيكا')
INSERT [dbo].[Origins] ([Id], [Code], [Name], [NameAr]) VALUES (61, N'DO', N'Dominican Republic ', N'جمهورية الدومنيكان')
INSERT [dbo].[Origins] ([Id], [Code], [Name], [NameAr]) VALUES (62, N'TP', N'East Timor', N'تيمور الشرقية')
INSERT [dbo].[Origins] ([Id], [Code], [Name], [NameAr]) VALUES (63, N'EC', N'Ecuador ', N'اكوادور')
INSERT [dbo].[Origins] ([Id], [Code], [Name], [NameAr]) VALUES (64, N'EG', N'Egypt ', N'مصر')
INSERT [dbo].[Origins] ([Id], [Code], [Name], [NameAr]) VALUES (65, N'SV', N'El Salvador ', N'السلفادور')
INSERT [dbo].[Origins] ([Id], [Code], [Name], [NameAr]) VALUES (66, N'GQ', N'Equatorial Guinea ', N'غينيا الاستوائية')
INSERT [dbo].[Origins] ([Id], [Code], [Name], [NameAr]) VALUES (67, N'ER', N'Eritrea ', N'إريتريا')
INSERT [dbo].[Origins] ([Id], [Code], [Name], [NameAr]) VALUES (68, N'EE', N'Estonia ', N'إستونيا')
INSERT [dbo].[Origins] ([Id], [Code], [Name], [NameAr]) VALUES (69, N'ET', N'Ethiopia ', N'إثيوبيا')
INSERT [dbo].[Origins] ([Id], [Code], [Name], [NameAr]) VALUES (70, N'GB', N'European Union', N'European Union')
INSERT [dbo].[Origins] ([Id], [Code], [Name], [NameAr]) VALUES (71, N'FK', N'Falkland Islands (Islas Malvinas)', N'جزر فوكلاند')
INSERT [dbo].[Origins] ([Id], [Code], [Name], [NameAr]) VALUES (72, N'FO', N'Faroe Islands ', N'جزر فارو')
INSERT [dbo].[Origins] ([Id], [Code], [Name], [NameAr]) VALUES (73, N'FM', N'Federated States of Micronesia', N'ميكرونيزيا ')
INSERT [dbo].[Origins] ([Id], [Code], [Name], [NameAr]) VALUES (74, N'FJ', N'Fiji ', N'فيجي')
INSERT [dbo].[Origins] ([Id], [Code], [Name], [NameAr]) VALUES (75, N'FI', N'Finland ', N'فنلندا')
INSERT [dbo].[Origins] ([Id], [Code], [Name], [NameAr]) VALUES (76, N'FR', N'France ', N'فرنسا')
INSERT [dbo].[Origins] ([Id], [Code], [Name], [NameAr]) VALUES (77, N'GF', N'French Guiana', N'جيانا الفرنسية')
INSERT [dbo].[Origins] ([Id], [Code], [Name], [NameAr]) VALUES (78, N'PF', N'French Polynesia ', N'بولينزيا الفرنسية')
INSERT [dbo].[Origins] ([Id], [Code], [Name], [NameAr]) VALUES (79, N'GA', N'Gabon ', N'غابون')
INSERT [dbo].[Origins] ([Id], [Code], [Name], [NameAr]) VALUES (80, N'GM', N'Gambia ', N'غامبيا')
INSERT [dbo].[Origins] ([Id], [Code], [Name], [NameAr]) VALUES (81, NULL, N'Gaza Strip ', N'قطاع غزة')
INSERT [dbo].[Origins] ([Id], [Code], [Name], [NameAr]) VALUES (82, N'GE', N'Georgia ', N'جورجيا')
INSERT [dbo].[Origins] ([Id], [Code], [Name], [NameAr]) VALUES (83, N'DE', N'Germany ', N'ألمانيا')
INSERT [dbo].[Origins] ([Id], [Code], [Name], [NameAr]) VALUES (84, N'GH', N'Ghana ', N'غانا')
INSERT [dbo].[Origins] ([Id], [Code], [Name], [NameAr]) VALUES (85, N'GI', N'Gibraltar ', N'جبل طارق')
INSERT [dbo].[Origins] ([Id], [Code], [Name], [NameAr]) VALUES (86, N'GR', N'Greece ', N'اليونان')
INSERT [dbo].[Origins] ([Id], [Code], [Name], [NameAr]) VALUES (87, N'GL', N'Greenland ', N'جرينلاند')
INSERT [dbo].[Origins] ([Id], [Code], [Name], [NameAr]) VALUES (88, N'GD', N'Grenada ', N'جرينادا')
INSERT [dbo].[Origins] ([Id], [Code], [Name], [NameAr]) VALUES (89, N'GP', N'Guadeloupe', NULL)
INSERT [dbo].[Origins] ([Id], [Code], [Name], [NameAr]) VALUES (90, N'GU', N'Guam', N'جوام')
INSERT [dbo].[Origins] ([Id], [Code], [Name], [NameAr]) VALUES (91, N'GT', N'Guatemala ', N'جواتيمالا')
INSERT [dbo].[Origins] ([Id], [Code], [Name], [NameAr]) VALUES (92, N'GN', N'Guinea ', N'غينيا')
INSERT [dbo].[Origins] ([Id], [Code], [Name], [NameAr]) VALUES (93, N'GW', N'Guinea-Bissau ', N'غينيا بيساو')
INSERT [dbo].[Origins] ([Id], [Code], [Name], [NameAr]) VALUES (94, N'GY', N'Guyana ', N'جيانا')
INSERT [dbo].[Origins] ([Id], [Code], [Name], [NameAr]) VALUES (95, N'HT', N'Haiti ', N'هايتي')
INSERT [dbo].[Origins] ([Id], [Code], [Name], [NameAr]) VALUES (96, N'VA', N'Holy See (Vatican City) ', N'الفاتيكان')
INSERT [dbo].[Origins] ([Id], [Code], [Name], [NameAr]) VALUES (97, N'HN', N'Honduras ', N'هندوراس')
INSERT [dbo].[Origins] ([Id], [Code], [Name], [NameAr]) VALUES (98, N'HK', N'Hong Kong ', N'هونغ كونغ')
INSERT [dbo].[Origins] ([Id], [Code], [Name], [NameAr]) VALUES (99, N'HU', N'Hungary ', N'المجر/هنغاريا')
GO
INSERT [dbo].[Origins] ([Id], [Code], [Name], [NameAr]) VALUES (100, N'IS', N'Iceland ', N'آيسلندا')
INSERT [dbo].[Origins] ([Id], [Code], [Name], [NameAr]) VALUES (101, N'IN', N'India ', N'الهند')
INSERT [dbo].[Origins] ([Id], [Code], [Name], [NameAr]) VALUES (102, N'ID', N'Indonesia ', N'إندونيسيا')
INSERT [dbo].[Origins] ([Id], [Code], [Name], [NameAr]) VALUES (103, N'IR', N'Iran ', N'ايران')
INSERT [dbo].[Origins] ([Id], [Code], [Name], [NameAr]) VALUES (104, N'IQ', N'Iraq ', N'العراق')
INSERT [dbo].[Origins] ([Id], [Code], [Name], [NameAr]) VALUES (105, N'IE', N'Ireland ', N'ايرلاند')
INSERT [dbo].[Origins] ([Id], [Code], [Name], [NameAr]) VALUES (106, N'IM', N'Isle of Man', N'جزر مان')
INSERT [dbo].[Origins] ([Id], [Code], [Name], [NameAr]) VALUES (107, N'IL', N'Israel ', N'اسرائيل')
INSERT [dbo].[Origins] ([Id], [Code], [Name], [NameAr]) VALUES (108, N'IT', N'Italy ', N'ايطاليا')
INSERT [dbo].[Origins] ([Id], [Code], [Name], [NameAr]) VALUES (109, N'JM', N'Jamaica ', N'جامايكا')
INSERT [dbo].[Origins] ([Id], [Code], [Name], [NameAr]) VALUES (110, N'JP', N'Japan ', N'اليابان')
INSERT [dbo].[Origins] ([Id], [Code], [Name], [NameAr]) VALUES (111, N'JE', N'Jersey ', N'جيرسي')
INSERT [dbo].[Origins] ([Id], [Code], [Name], [NameAr]) VALUES (112, N'JO', N'Jordan ', N'الاردن')
INSERT [dbo].[Origins] ([Id], [Code], [Name], [NameAr]) VALUES (113, N'KZ', N'Kazakhstan ', N'كازاخستان')
INSERT [dbo].[Origins] ([Id], [Code], [Name], [NameAr]) VALUES (114, N'KE', N'Kenya ', N'كينيا')
INSERT [dbo].[Origins] ([Id], [Code], [Name], [NameAr]) VALUES (115, N'KI', N'Kiribati ', N'كيريباتي')
INSERT [dbo].[Origins] ([Id], [Code], [Name], [NameAr]) VALUES (116, N'KW', N'Kuwait ', N'الكويت')
INSERT [dbo].[Origins] ([Id], [Code], [Name], [NameAr]) VALUES (117, N'KG', N'Kyrgyzstan ', N'قرغيزستان')
INSERT [dbo].[Origins] ([Id], [Code], [Name], [NameAr]) VALUES (118, N'LA', N'Laos ', N'لاوس')
INSERT [dbo].[Origins] ([Id], [Code], [Name], [NameAr]) VALUES (119, N'LV', N'Latvia ', N'لاتفيا')
INSERT [dbo].[Origins] ([Id], [Code], [Name], [NameAr]) VALUES (120, N'LB', N'Lebanon ', N'لبنان')
INSERT [dbo].[Origins] ([Id], [Code], [Name], [NameAr]) VALUES (121, N'LS', N'Lesotho ', N'ليسوتو')
INSERT [dbo].[Origins] ([Id], [Code], [Name], [NameAr]) VALUES (122, N'LR', N'Liberia ', N'ليبيريا')
INSERT [dbo].[Origins] ([Id], [Code], [Name], [NameAr]) VALUES (123, N'LY', N'Libya ', N'ليبيا')
INSERT [dbo].[Origins] ([Id], [Code], [Name], [NameAr]) VALUES (124, N'LI', N'Liechtenstein ', N'ليختنشتاين /  ليشتنشتاين')
INSERT [dbo].[Origins] ([Id], [Code], [Name], [NameAr]) VALUES (125, N'LT', N'Lithuania ', N'لتوانيا')
INSERT [dbo].[Origins] ([Id], [Code], [Name], [NameAr]) VALUES (126, N'LU', N'Luxembourg ', N'لكسمبورج')
INSERT [dbo].[Origins] ([Id], [Code], [Name], [NameAr]) VALUES (127, N'MO', N'Macau ', N'ماكاو')
INSERT [dbo].[Origins] ([Id], [Code], [Name], [NameAr]) VALUES (128, N'MK', N'Macedonia ', N'مقدونيا')
INSERT [dbo].[Origins] ([Id], [Code], [Name], [NameAr]) VALUES (129, N'MG', N'Madagascar ', N'مدغشقر')
INSERT [dbo].[Origins] ([Id], [Code], [Name], [NameAr]) VALUES (130, N'MW', N'Malawi ', N'مالاوي')
INSERT [dbo].[Origins] ([Id], [Code], [Name], [NameAr]) VALUES (131, N'MY', N'Malaysia ', N'ماليزيا')
INSERT [dbo].[Origins] ([Id], [Code], [Name], [NameAr]) VALUES (132, N'MV', N'Maldives ', N'مالديف')
INSERT [dbo].[Origins] ([Id], [Code], [Name], [NameAr]) VALUES (133, N'ML', N'Mali ', N'مالي')
INSERT [dbo].[Origins] ([Id], [Code], [Name], [NameAr]) VALUES (134, N'MT', N'Malta ', N'مالطة')
INSERT [dbo].[Origins] ([Id], [Code], [Name], [NameAr]) VALUES (135, N'MH', N'Marshall Islands ', N'جزر مارشال')
INSERT [dbo].[Origins] ([Id], [Code], [Name], [NameAr]) VALUES (136, N'MQ', N'Martinique', N'مارتينيك')
INSERT [dbo].[Origins] ([Id], [Code], [Name], [NameAr]) VALUES (137, N'MR', N'Mauritania ', N'موريتانيا')
INSERT [dbo].[Origins] ([Id], [Code], [Name], [NameAr]) VALUES (138, N'MU', N'Mauritius ', N'موريشيوس')
INSERT [dbo].[Origins] ([Id], [Code], [Name], [NameAr]) VALUES (139, N'YT', N'Mayotte ', N'مايوت')
INSERT [dbo].[Origins] ([Id], [Code], [Name], [NameAr]) VALUES (140, N'MX', N'Mexico ', N'مكسيك')
INSERT [dbo].[Origins] ([Id], [Code], [Name], [NameAr]) VALUES (141, N'MD', N'Moldova ', N'مولدوفا')
INSERT [dbo].[Origins] ([Id], [Code], [Name], [NameAr]) VALUES (142, N'MC', N'Monaco ', N'موناكو')
INSERT [dbo].[Origins] ([Id], [Code], [Name], [NameAr]) VALUES (143, N'MN', N'Mongolia ', N'منغوليا ')
INSERT [dbo].[Origins] ([Id], [Code], [Name], [NameAr]) VALUES (144, N'ME', N'Montenegro ', N'الجبل الأسود (مونتينيغرو)')
INSERT [dbo].[Origins] ([Id], [Code], [Name], [NameAr]) VALUES (145, N'MS', N'Montserrat ', N'مونتسيرات')
INSERT [dbo].[Origins] ([Id], [Code], [Name], [NameAr]) VALUES (146, N'MA', N'Morocco ', N'المغرب')
INSERT [dbo].[Origins] ([Id], [Code], [Name], [NameAr]) VALUES (147, N'MZ', N'Mozambique ', N'موزامبيق')
INSERT [dbo].[Origins] ([Id], [Code], [Name], [NameAr]) VALUES (148, N'NA', N'Namibia ', N'ناميبيا')
INSERT [dbo].[Origins] ([Id], [Code], [Name], [NameAr]) VALUES (149, N'NR', N'Nauru ', N'ناورو')
INSERT [dbo].[Origins] ([Id], [Code], [Name], [NameAr]) VALUES (150, N'NP', N'Nepal ', N'نيبال')
INSERT [dbo].[Origins] ([Id], [Code], [Name], [NameAr]) VALUES (151, N'NL', N'Netherlands ', N'هولندا')
INSERT [dbo].[Origins] ([Id], [Code], [Name], [NameAr]) VALUES (152, N'AN', N'Netherlands Antilles ', N'جزر الأنتيل الهولندية')
INSERT [dbo].[Origins] ([Id], [Code], [Name], [NameAr]) VALUES (153, N'NC', N'New Caledonia ', N'كاليدونيا الجديدة')
INSERT [dbo].[Origins] ([Id], [Code], [Name], [NameAr]) VALUES (154, N'NZ', N'New Zealand ', N'نيوزيلندا')
INSERT [dbo].[Origins] ([Id], [Code], [Name], [NameAr]) VALUES (155, N'NI', N'Nicaragua ', N'نيكاراجوا')
INSERT [dbo].[Origins] ([Id], [Code], [Name], [NameAr]) VALUES (156, N'NE', N'Niger ', N'نيجر')
INSERT [dbo].[Origins] ([Id], [Code], [Name], [NameAr]) VALUES (157, N'NG', N'Nigeria ', N'نيجيريا')
INSERT [dbo].[Origins] ([Id], [Code], [Name], [NameAr]) VALUES (158, N'NU', N'Niue ', N'نيوي')
INSERT [dbo].[Origins] ([Id], [Code], [Name], [NameAr]) VALUES (159, N'NF', N'Norfolk Island ', N'جزر نورفولك')
INSERT [dbo].[Origins] ([Id], [Code], [Name], [NameAr]) VALUES (160, N'KP', N'North Korea ', N'كوريا الشمالية')
INSERT [dbo].[Origins] ([Id], [Code], [Name], [NameAr]) VALUES (161, N'MP', N'Northern Mariana Islands ', N'جزر ماريانا الشمالية')
INSERT [dbo].[Origins] ([Id], [Code], [Name], [NameAr]) VALUES (162, N'NO', N'Norway ', N'نرويج')
INSERT [dbo].[Origins] ([Id], [Code], [Name], [NameAr]) VALUES (163, N'OM', N'Oman ', N'عمان')
INSERT [dbo].[Origins] ([Id], [Code], [Name], [NameAr]) VALUES (164, N'PK', N'Pakistan ', N'باكستان')
INSERT [dbo].[Origins] ([Id], [Code], [Name], [NameAr]) VALUES (165, N'PW', N'Palau ', N'بالاو')
INSERT [dbo].[Origins] ([Id], [Code], [Name], [NameAr]) VALUES (166, N'--', N'Palestine', N'فلسطين')
INSERT [dbo].[Origins] ([Id], [Code], [Name], [NameAr]) VALUES (167, N'PA', N'Panama', N'بنما')
INSERT [dbo].[Origins] ([Id], [Code], [Name], [NameAr]) VALUES (168, N'PG', N'Papua New Guinea ', N'بابوا غينيا الجديدة')
INSERT [dbo].[Origins] ([Id], [Code], [Name], [NameAr]) VALUES (169, N'PY', N'Paraguay', N'باراجواي')
INSERT [dbo].[Origins] ([Id], [Code], [Name], [NameAr]) VALUES (170, N'PE', N'Peru ', N'بيرو')
INSERT [dbo].[Origins] ([Id], [Code], [Name], [NameAr]) VALUES (171, N'PH', N'Philippines ', N'الفلبين')
INSERT [dbo].[Origins] ([Id], [Code], [Name], [NameAr]) VALUES (172, N'PN', N'Pitcairn Islands ', N'جزر بيتكارين')
INSERT [dbo].[Origins] ([Id], [Code], [Name], [NameAr]) VALUES (173, N'PL', N'Poland', N'بولندا')
INSERT [dbo].[Origins] ([Id], [Code], [Name], [NameAr]) VALUES (174, N'PT', N'Portugal ', N'برتغال')
INSERT [dbo].[Origins] ([Id], [Code], [Name], [NameAr]) VALUES (175, N'PR', N'Puerto Rico', N'بورتوريكو')
INSERT [dbo].[Origins] ([Id], [Code], [Name], [NameAr]) VALUES (176, N'QA', N'Qatar ', N'قطر')
INSERT [dbo].[Origins] ([Id], [Code], [Name], [NameAr]) VALUES (177, N'CG', N'Republic of the Congo ', N'جمهورية الكونغو')
INSERT [dbo].[Origins] ([Id], [Code], [Name], [NameAr]) VALUES (178, N'RE', N'Reunion', N'رينونيون')
INSERT [dbo].[Origins] ([Id], [Code], [Name], [NameAr]) VALUES (179, N'RO', N'Romania ', N'رومانيا')
INSERT [dbo].[Origins] ([Id], [Code], [Name], [NameAr]) VALUES (180, N'RU', N'Russia ', N'روسيا')
INSERT [dbo].[Origins] ([Id], [Code], [Name], [NameAr]) VALUES (181, N'RW', N'Rwanda ', N'رواندا')
INSERT [dbo].[Origins] ([Id], [Code], [Name], [NameAr]) VALUES (182, N'BL', N'Saint Barthelemy ', N'سان بارتليمي')
INSERT [dbo].[Origins] ([Id], [Code], [Name], [NameAr]) VALUES (183, N'SH', N'Saint Helena ', N'سانت هيلانا')
INSERT [dbo].[Origins] ([Id], [Code], [Name], [NameAr]) VALUES (184, N'KN', N'Saint Kitts and Nevis ', N'سانت كيتس ونيفيس')
INSERT [dbo].[Origins] ([Id], [Code], [Name], [NameAr]) VALUES (185, N'LC', N'Saint Lucia ', N'سانت لوسيا')
INSERT [dbo].[Origins] ([Id], [Code], [Name], [NameAr]) VALUES (186, N'MF', N'Saint Martin ', N'سانت مارتن')
INSERT [dbo].[Origins] ([Id], [Code], [Name], [NameAr]) VALUES (187, N'PM', N'Saint Pierre and Miquelon ', N'سان بيار وميكلون')
INSERT [dbo].[Origins] ([Id], [Code], [Name], [NameAr]) VALUES (188, N'VC', N'Saint Vincent and the Grenadines ', N'سانت فنسينت والجرينادينز')
INSERT [dbo].[Origins] ([Id], [Code], [Name], [NameAr]) VALUES (189, N'SM', N'San Marino ', N'سان مارينو')
INSERT [dbo].[Origins] ([Id], [Code], [Name], [NameAr]) VALUES (190, N'ST', N'Sao Tome and Principe', N'ساو تومي وبرنسيبي')
INSERT [dbo].[Origins] ([Id], [Code], [Name], [NameAr]) VALUES (191, N'SA', N'Saudi Arabia ', N'السعودية')
INSERT [dbo].[Origins] ([Id], [Code], [Name], [NameAr]) VALUES (192, N'SN', N'Senegal ', N'السنغال')
INSERT [dbo].[Origins] ([Id], [Code], [Name], [NameAr]) VALUES (193, N'RS', N'Serbia ', N'صربيا')
INSERT [dbo].[Origins] ([Id], [Code], [Name], [NameAr]) VALUES (194, N'SC', N'Seychelles ', N'سيشيل')
INSERT [dbo].[Origins] ([Id], [Code], [Name], [NameAr]) VALUES (195, N'SL', N'Sierra Leone ', N'سيراليون')
INSERT [dbo].[Origins] ([Id], [Code], [Name], [NameAr]) VALUES (196, N'SG', N'Singapore ', N'سنغافورة ')
INSERT [dbo].[Origins] ([Id], [Code], [Name], [NameAr]) VALUES (197, N'SK', N'Slovakia ', N'سلوفاكيا ')
INSERT [dbo].[Origins] ([Id], [Code], [Name], [NameAr]) VALUES (198, N'SI', N'Slovenia', N'سلوفينيا')
INSERT [dbo].[Origins] ([Id], [Code], [Name], [NameAr]) VALUES (199, N'SB', N'Solomon Islands', N'جزر سليمان')
GO
INSERT [dbo].[Origins] ([Id], [Code], [Name], [NameAr]) VALUES (200, N'SO', N'Somalia', N'الصومال')
INSERT [dbo].[Origins] ([Id], [Code], [Name], [NameAr]) VALUES (201, N'ZA', N'South Africa', N'جنور أفريقيا')
INSERT [dbo].[Origins] ([Id], [Code], [Name], [NameAr]) VALUES (202, N'KR', N'South Korea ', N'كوريا الجنوبية')
INSERT [dbo].[Origins] ([Id], [Code], [Name], [NameAr]) VALUES (203, N'SS', N'South Sudan', N'السودان الجنوبي')
INSERT [dbo].[Origins] ([Id], [Code], [Name], [NameAr]) VALUES (204, N'ES', N'Spain ', N'اسبانيا')
INSERT [dbo].[Origins] ([Id], [Code], [Name], [NameAr]) VALUES (205, N'LK', N'Sri Lanka', N'سريلانكا')
INSERT [dbo].[Origins] ([Id], [Code], [Name], [NameAr]) VALUES (206, N'SD', N'Sudan ', N'السودان')
INSERT [dbo].[Origins] ([Id], [Code], [Name], [NameAr]) VALUES (207, N'SR', N'Suriname', N'سورينام')
INSERT [dbo].[Origins] ([Id], [Code], [Name], [NameAr]) VALUES (208, N'SJ', N'Svalbard ', N'سفالبارد')
INSERT [dbo].[Origins] ([Id], [Code], [Name], [NameAr]) VALUES (209, N'SZ', N'Swaziland', N'سوازيلند')
INSERT [dbo].[Origins] ([Id], [Code], [Name], [NameAr]) VALUES (210, N'SE', N'Sweden', N'سويد')
INSERT [dbo].[Origins] ([Id], [Code], [Name], [NameAr]) VALUES (211, N'CH', N'Switzerland', N'سويسرا')
INSERT [dbo].[Origins] ([Id], [Code], [Name], [NameAr]) VALUES (212, N'SY', N'Syria ', N'سوريا')
INSERT [dbo].[Origins] ([Id], [Code], [Name], [NameAr]) VALUES (213, N'TW', N'Taiwan', N'تايوان')
INSERT [dbo].[Origins] ([Id], [Code], [Name], [NameAr]) VALUES (214, N'TJ', N'Tajikistan', N'طاجيكستان ')
INSERT [dbo].[Origins] ([Id], [Code], [Name], [NameAr]) VALUES (215, N'TZ', N'Tanzania', N'جمهورية تنزانيا المتحدة')
INSERT [dbo].[Origins] ([Id], [Code], [Name], [NameAr]) VALUES (216, N'TH', N'Thailand', N'تايلاند')
INSERT [dbo].[Origins] ([Id], [Code], [Name], [NameAr]) VALUES (217, N'BS', N'The Bahamas', N'جزر البهاما')
INSERT [dbo].[Origins] ([Id], [Code], [Name], [NameAr]) VALUES (218, N'TP', N'Timor-Leste', N'تيمور ليشتي')
INSERT [dbo].[Origins] ([Id], [Code], [Name], [NameAr]) VALUES (219, N'TG', N'Togo', N'توغو')
INSERT [dbo].[Origins] ([Id], [Code], [Name], [NameAr]) VALUES (220, N'TK', N'Tokelau', N'توكيلاو ')
INSERT [dbo].[Origins] ([Id], [Code], [Name], [NameAr]) VALUES (221, N'TO', N'Tonga ', N'تونغا')
INSERT [dbo].[Origins] ([Id], [Code], [Name], [NameAr]) VALUES (222, N'TT', N'Trinidad and Tobago', N'ترينيدادا وتوباجو')
INSERT [dbo].[Origins] ([Id], [Code], [Name], [NameAr]) VALUES (223, N'TN', N'Tunisia', N'تونس')
INSERT [dbo].[Origins] ([Id], [Code], [Name], [NameAr]) VALUES (224, N'TR', N'Turkey', N'تركيا')
INSERT [dbo].[Origins] ([Id], [Code], [Name], [NameAr]) VALUES (225, N'TM', N'Turkmenistan', N'تركمانستان')
INSERT [dbo].[Origins] ([Id], [Code], [Name], [NameAr]) VALUES (226, N'TC', N'Turks and Caicos Islands', N'جزر توركس وكايكوس')
INSERT [dbo].[Origins] ([Id], [Code], [Name], [NameAr]) VALUES (227, N'TV', N'Tuvalu', N'توفالو')
INSERT [dbo].[Origins] ([Id], [Code], [Name], [NameAr]) VALUES (228, N'UG', N'Uganda', N'أوغندا')
INSERT [dbo].[Origins] ([Id], [Code], [Name], [NameAr]) VALUES (229, N'UA', N'Ukraine', N'أوكرانيا')
INSERT [dbo].[Origins] ([Id], [Code], [Name], [NameAr]) VALUES (230, N'AE', N'United Arab Emirates ', N'الإمارات')
INSERT [dbo].[Origins] ([Id], [Code], [Name], [NameAr]) VALUES (231, N'GB', N'United Kingdom ', N'المملكة المتحدة')
INSERT [dbo].[Origins] ([Id], [Code], [Name], [NameAr]) VALUES (232, N'US', N'United States of America', N'الولايات المتحدة')
INSERT [dbo].[Origins] ([Id], [Code], [Name], [NameAr]) VALUES (233, N'UY', N'Uruguay', N'اوروجواي')
INSERT [dbo].[Origins] ([Id], [Code], [Name], [NameAr]) VALUES (234, N'VG', N'US Virgin Islands ', N'جزر فيرجين الأمريكية')
INSERT [dbo].[Origins] ([Id], [Code], [Name], [NameAr]) VALUES (235, N'UZ', N'Uzbekistan', N'أوزبكستان')
INSERT [dbo].[Origins] ([Id], [Code], [Name], [NameAr]) VALUES (236, N'VU', N'Vanuatu', N'فانواتو')
INSERT [dbo].[Origins] ([Id], [Code], [Name], [NameAr]) VALUES (237, N'VE', N'Venezuela', N'فنزويلا')
INSERT [dbo].[Origins] ([Id], [Code], [Name], [NameAr]) VALUES (238, N'VN', N'Vietnam', N'فييتنام')
INSERT [dbo].[Origins] ([Id], [Code], [Name], [NameAr]) VALUES (239, N'WF', N'Wallis and Futuna', N'والس وفوتونا')
INSERT [dbo].[Origins] ([Id], [Code], [Name], [NameAr]) VALUES (240, N'WS', N'Wester Samoa ', N'ساموا الغربية')
INSERT [dbo].[Origins] ([Id], [Code], [Name], [NameAr]) VALUES (241, N'EH', N'Western Sahara', N'الصحراء الغربية')
INSERT [dbo].[Origins] ([Id], [Code], [Name], [NameAr]) VALUES (242, N'YE', N'Yemen', N'اليمن')
INSERT [dbo].[Origins] ([Id], [Code], [Name], [NameAr]) VALUES (243, N'YU', N'Yugoslavia', N'يوغسلافيا')
INSERT [dbo].[Origins] ([Id], [Code], [Name], [NameAr]) VALUES (244, N'ZM', N'Zambia', N'زامبيا')
INSERT [dbo].[Origins] ([Id], [Code], [Name], [NameAr]) VALUES (245, N'ZW', N'Zimbabwe', N'زمبابوي')
SET IDENTITY_INSERT [dbo].[Origins] OFF
SET IDENTITY_INSERT [dbo].[RoleCategories] ON 

INSERT [dbo].[RoleCategories] ([Id], [Name], [NameAr]) VALUES (1, N'Administration', N'الإدارة')
INSERT [dbo].[RoleCategories] ([Id], [Name], [NameAr]) VALUES (2, N'Organization', N'الهيئات')
INSERT [dbo].[RoleCategories] ([Id], [Name], [NameAr]) VALUES (3, N'Governorates', N'المحافظات')
INSERT [dbo].[RoleCategories] ([Id], [Name], [NameAr]) VALUES (4, N'Cities', N'المدن')
INSERT [dbo].[RoleCategories] ([Id], [Name], [NameAr]) VALUES (5, N'Hospitals', N'المستشفيات')
SET IDENTITY_INSERT [dbo].[RoleCategories] OFF
SET IDENTITY_INSERT [dbo].[SubCategories] ON 

INSERT [dbo].[SubCategories] ([Id], [Code], [Name], [NameAr], [CategoryIdId]) VALUES (1, N'01', N'Sub Category 1', N'Sub Category 1 Ar', 1)
SET IDENTITY_INSERT [dbo].[SubCategories] OFF
SET IDENTITY_INSERT [dbo].[SubOrganizations] ON 

INSERT [dbo].[SubOrganizations] ([Id], [Code], [Name], [NameAr], [Address], [AddressAr], [Email], [Mobile], [DirectorName], [DirectorNameAr], [OrganizationId]) VALUES (1, N'01', N'Prison Sector', N'قطاع السجون', N'', N'', N'', N'', N'', N'', 1)
INSERT [dbo].[SubOrganizations] ([Id], [Code], [Name], [NameAr], [Address], [AddressAr], [Email], [Mobile], [DirectorName], [DirectorNameAr], [OrganizationId]) VALUES (2, N'02', N'Sub Org2', N'Sub Org2 Ar', N'', N'', N'', N'', N'', N'', 1)
INSERT [dbo].[SubOrganizations] ([Id], [Code], [Name], [NameAr], [Address], [AddressAr], [Email], [Mobile], [DirectorName], [DirectorNameAr], [OrganizationId]) VALUES (3, N'03', N'Medical Insurance sub org1', N'التأمين الصحي - الفرع', N'', N'', N'', N'', N'', N'', 4)
INSERT [dbo].[SubOrganizations] ([Id], [Code], [Name], [NameAr], [Address], [AddressAr], [Email], [Mobile], [DirectorName], [DirectorNameAr], [OrganizationId]) VALUES (4, N'11', N'Rail Ways Hospitals', N' مستشفى هيئة السكك الحديد ', N'', N'', N'', N'', N'', N'', 8)
SET IDENTITY_INSERT [dbo].[SubOrganizations] OFF
