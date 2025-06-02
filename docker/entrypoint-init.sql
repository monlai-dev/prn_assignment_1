USE [master]
GO

CREATE DATABASE [FUNewsManagement]
GO

USE [FUNewsManagement]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Category](
	[CategoryID] [smallint] IDENTITY(1,1) NOT NULL,
	[CategoryName] [nvarchar](100) NOT NULL,
	[CategoryDesciption] [nvarchar](250) NOT NULL,
	[ParentCategoryID] [smallint] NULL,
	[IsActive] [bit] NULL,
 CONSTRAINT [PK_Category] PRIMARY KEY CLUSTERED 
(
	[CategoryID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[NewsArticle](
	[NewsArticleID] [nvarchar](20) NOT NULL,
	[NewsTitle] [nvarchar](400) NULL,
	[Headline] [nvarchar](150) NOT NULL,
	[CreatedDate] [datetime] NULL,
	[NewsContent] [nvarchar](4000) NULL,
	[NewsSource] [nvarchar](400) NULL,
	[CategoryID] [smallint] NULL,
	[NewsStatus] [bit] NULL,
	[CreatedByID] [smallint] NULL,
	[UpdatedByID] [smallint] NULL,
	[ModifiedDate] [datetime] NULL,
 CONSTRAINT [PK_NewsArticle] PRIMARY KEY CLUSTERED 
(
	[NewsArticleID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[NewsTag](
	[NewsArticleID] [nvarchar](20) NOT NULL,
	[TagID] [int] NOT NULL,
 CONSTRAINT [PK_NewsTag] PRIMARY KEY CLUSTERED 
(
	[NewsArticleID] ASC,
	[TagID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[SystemAccount](
	[AccountID] [smallint] NOT NULL,
	[AccountName] [nvarchar](100) NULL,
	[AccountEmail] [nvarchar](70) NULL,
	[AccountRole] [int] NULL,
	[AccountPassword] [nvarchar](70) NULL,
 CONSTRAINT [PK_SystemAccount] PRIMARY KEY CLUSTERED 
(
	[AccountID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO


SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Tag](
	[TagID] [int] NOT NULL,
	[TagName] [nvarchar](50) NULL,
	[Note] [nvarchar](400) NULL,
 CONSTRAINT [PK_HashTag] PRIMARY KEY CLUSTERED 
(
	[TagID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[Category] ON 
GO


INSERT [dbo].[Category] ([CategoryID], [CategoryName], [CategoryDesciption], [ParentCategoryID], [IsActive]) VALUES (1, N'Academic news', N'This category can include articles about research findings, faculty appointments and promotions, and other academic-related announcements.', 1, 1)
GO
INSERT [dbo].[Category] ([CategoryID], [CategoryName], [CategoryDesciption], [ParentCategoryID], [IsActive]) VALUES (2, N'Student Affairs', N'This category can include articles about student activities, events, and initiatives, such as student clubs, organizations and sports.', 2, 1)
GO
INSERT [dbo].[Category] ([CategoryID], [CategoryName], [CategoryDesciption], [ParentCategoryID], [IsActive]) VALUES (3, N'Campus Safety', N'This category can include articles about incidents and safety measures implemented on campus to ensure the safety of students and faculty.', 3, 1)
GO
INSERT [dbo].[Category] ([CategoryID], [CategoryName], [CategoryDesciption], [ParentCategoryID], [IsActive]) VALUES (4, N'Alumni News', N'This category can include articles about the achievements and accomplishments of former students and alumni, such as graduations, job promotions and career successes.', 4, 1)
GO
INSERT [dbo].[Category] ([CategoryID], [CategoryName], [CategoryDesciption], [ParentCategoryID], [IsActive]) VALUES (5, N'Capstone Project News', N'This category is typically a comprehensive and detailed report created as part of an academic or professional capstone project. ', 5, 0)
GO
SET IDENTITY_INSERT [dbo].[Category] OFF
GO


INSERT [dbo].[NewsArticle] ([NewsArticleID], [NewsTitle], [Headline], [CreatedDate], [NewsContent], [NewsSource], [CategoryID], [NewsStatus], [CreatedByID], [UpdatedByID], [ModifiedDate]) VALUES (N'1', N'University FU Celebrates Success of Alumni in Various Fields', N'University FU Celebrates Success of Alumni in Various Fields', CAST(N'2024-05-05T00:00:00.000' AS DateTime), N'University FU recently commemorated the achievements of its esteemed alumni who have excelled in a multitude of fields, showcasing the impact of the institution''s education on their professional journeys.

Diverse Success Stories: From successful entrepreneurs to renowned artists, University X''s alumni have made significant strides in various industries, reflecting the versatility of the education provided.

Networking Opportunities: The university''s strong alumni network played a crucial role in fostering connections and collaborations among graduates, contributing to their success.

Alumni Contributions: Many alumni have also given back to the university through mentorship programs, scholarships, and guest lectures, enriching the current student experience.', N'N/A', 4, 1, 1, 1, CAST(N'2024-05-05T00:00:00.000' AS DateTime))
GO
INSERT [dbo].[NewsArticle] ([NewsArticleID], [NewsTitle], [Headline], [CreatedDate], [NewsContent], [NewsSource], [CategoryID], [NewsStatus], [CreatedByID], [UpdatedByID], [ModifiedDate]) VALUES (N'2', N'Alumni Association Launches Mentorship Program for Recent Graduates', N'Alumni Association Launches Mentorship Program for Recent Graduates', CAST(N'2024-05-05T00:00:00.000' AS DateTime), N'The Alumni Association of University FU recently unveiled a new mentorship program aimed at providing support and guidance to recent graduates as they navigate the transition from academia to the professional world.

The program pairs recent graduates with experienced alumni mentors based on their career interests and goals, ensuring tailored guidance for each mentee.

In addition to one-on-one mentorship, the program offers workshops on resume building, interview preparation, and networking strategies to equip graduates with essential skills for success.

The mentorship program also facilitates networking events where mentees can expand their professional connections and learn from alumni who have excelled in their respective fields.

By fostering a supportive network of alumni mentors, the program aims to empower recent graduates to navigate the challenges of the job market, enhance their professional growth, and build lasting connections within their industries.

The launch of this mentorship program by the Alumni Association of University Y underscores the commitment to nurturing the success of its graduates beyond graduation, creating a strong community of support and guidance for the next generation of professionals.', N'Internet', 4, 1, 1, 1, CAST(N'2024-05-05T00:00:00.000' AS DateTime))
GO
INSERT [dbo].[NewsArticle] ([NewsArticleID], [NewsTitle], [Headline], [CreatedDate], [NewsContent], [NewsSource], [CategoryID], [NewsStatus], [CreatedByID], [UpdatedByID], [ModifiedDate]) VALUES (N'3', N'Academic Department Announces Groundbreaking Initiatives and Program Enhancements', N'Academic Department Announces Groundbreaking Initiatives and Program Enhancements', CAST(N'2024-05-05T00:00:00.000' AS DateTime), N'The Software Engineering Department at FU has unveiled a series of transformative initiatives and program enhancements aimed at enriching the academic experience and fostering innovation in Software Development.

The department has established [specific research centers or facilities] dedicated to advancing knowledge and addressing pressing challenges in Software Development.

Faculty Promotions: Several esteemed faculty members have been promoted to key leadership positions, reflecting their commitment to academic excellence and their vision for shaping the future of Software Development.

The academic programs within the department have undergone enhancements to incorporate the latest developments and equip students with practical skills and knowledge relevant to current industry demands.

These initiatives are poised to position the Software Engineering Department as a hub of innovation and academic rigor, attracting top talent and fostering groundbreaking research and learning experiences.
', N'N/A', 1, 1, 2, 2, CAST(N'2024-05-05T00:00:00.000' AS DateTime))
GO
INSERT [dbo].[NewsArticle] ([NewsArticleID], [NewsTitle], [Headline], [CreatedDate], [NewsContent], [NewsSource], [CategoryID], [NewsStatus], [CreatedByID], [UpdatedByID], [ModifiedDate]) VALUES (N'6', N'FU Student Council Organizes Annual Tech Innovation Fair', N'FU Student Council Organizes Annual Tech Innovation Fair', CAST(N'2024-05-10T00:00:00.000' AS DateTime), N'The FU Student Council successfully organized the Annual Tech Innovation Fair, showcasing cutting-edge projects and fostering collaboration among students from various disciplines.

The event featured over 50 student projects ranging from mobile applications to IoT devices, demonstrating the creativity and technical skills of FU students.

Industry professionals and faculty members served as judges, providing valuable feedback and mentorship opportunities for participating students.

Networking sessions allowed students to connect with potential employers and explore internship opportunities in the tech industry.

The fair concluded with an awards ceremony recognizing outstanding projects in categories such as Best Innovation, Most Practical Solution, and People''s Choice Award.

This annual event continues to strengthen the bond between academia and industry while inspiring the next generation of tech innovators.', N'Student Council Press Release', 2, 1, 3, 3, CAST(N'2024-05-10T00:00:00.000' AS DateTime))
GO
INSERT [dbo].[NewsArticle] ([NewsArticleID], [NewsTitle], [Headline], [CreatedDate], [NewsContent], [NewsSource], [CategoryID], [NewsStatus], [CreatedByID], [UpdatedByID], [ModifiedDate]) VALUES (N'7', N'Enhanced Campus Security Measures Implemented Following Safety Review', N'Enhanced Campus Security Measures Implemented Following Safety Review', CAST(N'2024-05-15T00:00:00.000' AS DateTime), N'FU has implemented comprehensive security enhancements across campus following a thorough safety review conducted by external security consultants and internal safety committees.

New security cameras have been installed in strategic locations throughout the campus, providing 24/7 monitoring capabilities with advanced facial recognition technology.

Emergency call stations have been upgraded and additional units installed in previously underserved areas, ensuring students and staff can quickly access help when needed.

The campus security team has been expanded with additional trained personnel, and all security staff have completed advanced emergency response training.

A new mobile safety app has been launched, allowing students and staff to report incidents, request escorts, and receive real-time safety alerts.

Regular safety drills and awareness programs will be conducted to ensure the entire campus community is prepared for various emergency scenarios.

These measures reflect FU''s unwavering commitment to maintaining a safe and secure learning environment for all members of the university community.', N'Campus Security Department', 3, 1, 4, 4, CAST(N'2024-05-15T00:00:00.000' AS DateTime))
GO
INSERT [dbo].[NewsArticle] ([NewsArticleID], [NewsTitle], [Headline], [CreatedDate], [NewsContent], [NewsSource], [CategoryID], [NewsStatus], [CreatedByID], [UpdatedByID], [ModifiedDate]) VALUES (N'8', N'Distinguished Alumni Receives International Recognition for Environmental Research', N'Distinguished Alumni Receives International Recognition for Environmental Research', CAST(N'2024-05-20T00:00:00.000' AS DateTime), N'Dr. Sarah Chen, a 2015 graduate of FU''s Environmental Science program, has been awarded the prestigious Global Environmental Leadership Award for her groundbreaking research on sustainable water management systems.

Dr. Chen''s innovative work on bio-remediation techniques has been implemented in over 20 countries, helping communities access clean water while reducing environmental impact.

Her research team at the International Water Research Institute has developed cost-effective solutions that have benefited over 2 million people in developing nations.

The award ceremony, held in Geneva, recognized Dr. Chen''s contributions to addressing global water scarcity challenges through scientific innovation and community engagement.

During her acceptance speech, Dr. Chen credited her foundational education at FU for inspiring her passion for environmental sustainability and providing the research skills necessary for her success.

FU''s Environmental Science Department plans to establish the Sarah Chen Fellowship in her honor, supporting future students pursuing research in sustainable water technologies.

This recognition highlights the global impact of FU alumni and reinforces the university''s commitment to addressing pressing environmental challenges through education and research.', N'International Environmental Journal', 4, 1, 1, 1, CAST(N'2024-05-20T00:00:00.000' AS DateTime))
GO
INSERT [dbo].[NewsArticle] ([NewsArticleID], [NewsTitle], [Headline], [CreatedDate], [NewsContent], [NewsSource], [CategoryID], [NewsStatus], [CreatedByID], [UpdatedByID], [ModifiedDate]) VALUES (N'9', N'Computer Science Department Launches AI Ethics Research Initiative', N'Computer Science Department Launches AI Ethics Research Initiative', CAST(N'2024-05-25T00:00:00.000' AS DateTime), N'The Computer Science Department at FU has announced the launch of a comprehensive AI Ethics Research Initiative, addressing the growing need for responsible artificial intelligence development and deployment.

The initiative brings together faculty from computer science, philosophy, law, and social sciences to examine the ethical implications of AI technologies in various sectors.

Research focus areas include algorithmic bias detection, privacy-preserving machine learning, and the societal impact of automated decision-making systems.

The department has secured $2.5 million in funding from the National Science Foundation and industry partners to support this three-year research program.

Graduate students will have opportunities to participate in interdisciplinary research projects and attend specialized workshops on AI ethics and responsible technology development.

Industry collaboration agreements with leading tech companies will provide real-world case studies and ensure research findings have practical applications.

The initiative aims to establish FU as a leading voice in AI ethics research and contribute to the development of industry standards for responsible AI practices.

Public seminars and community outreach programs will help educate the broader community about AI ethics and promote informed discussions about technology''s role in society.', N'University Research Office', 1, 1, 2, 2, CAST(N'2024-05-25T00:00:00.000' AS DateTime))
GO
INSERT [dbo].[NewsArticle] ([NewsArticleID], [NewsTitle], [Headline], [CreatedDate], [NewsContent], [NewsSource], [CategoryID], [NewsStatus], [CreatedByID], [UpdatedByID], [ModifiedDate]) VALUES (N'10', N'FU Robotics Club Wins National Championship in Autonomous Vehicle Competition', N'FU Robotics Club Wins National Championship in Autonomous Vehicle Competition', CAST(N'2024-06-01T00:00:00.000' AS DateTime), N'The FU Robotics Club has achieved remarkable success by winning first place in the National Autonomous Vehicle Competition, defeating teams from over 100 universities across the country.

The winning team, consisting of 12 undergraduate students from engineering and computer science programs, spent eight months developing their autonomous vehicle prototype.

Their innovative solution incorporated advanced computer vision algorithms, machine learning models for real-time decision making, and sophisticated sensor fusion techniques.

The competition required vehicles to navigate complex urban scenarios, including traffic intersections, pedestrian crossings, and dynamic obstacles, all without human intervention.

FU''s vehicle demonstrated superior performance in safety protocols, achieving a perfect safety score while maintaining competitive speed and efficiency metrics.

Team captain Maria Rodriguez credited the success to collaborative teamwork, mentorship from faculty advisors, and access to state-of-the-art laboratory facilities.

The victory comes with a $50,000 prize that will be used to fund future robotics projects and expand the club''s research capabilities.

This achievement highlights FU''s excellence in STEM education and the practical application of theoretical knowledge in competitive real-world scenarios.', N'National Robotics Association', 2, 1, 3, 3, CAST(N'2024-06-01T00:00:00.000' AS DateTime))
GO
INSERT [dbo].[NewsArticle] ([NewsArticleID], [NewsTitle], [Headline], [CreatedDate], [NewsContent], [NewsSource], [CategoryID], [NewsStatus], [CreatedByID], [UpdatedByID], [ModifiedDate]) VALUES (N'11', N'Campus Emergency Response System Upgrade Completed Successfully', N'Campus Emergency Response System Upgrade Completed Successfully', CAST(N'2024-06-05T00:00:00.000' AS DateTime), N'FU has successfully completed a comprehensive upgrade of its campus emergency response system, significantly enhancing the university''s ability to respond to various emergency situations.

The new system integrates multiple communication channels including SMS alerts, email notifications, campus-wide PA announcements, and a dedicated mobile application.

Advanced weather monitoring equipment has been installed to provide early warnings for severe weather conditions, ensuring timely evacuation procedures when necessary.

Integration with local emergency services allows for faster response times and better coordination during crisis situations.

The upgraded system includes multilingual support to ensure all international students and staff receive critical information in their preferred language.

Regular testing protocols have been established, with monthly system checks and quarterly campus-wide emergency drills to maintain readiness.

Training sessions for faculty and staff on the new emergency procedures have been completed, with online resources available for ongoing reference.

The investment in this upgraded system demonstrates FU''s proactive approach to campus safety and commitment to protecting the entire university community.', N'Emergency Management Office', 3, 1, 5, 5, CAST(N'2024-06-05T00:00:00.000' AS DateTime))
GO
INSERT [dbo].[NewsArticle] ([NewsArticleID], [NewsTitle], [Headline], [CreatedDate], [NewsContent], [NewsSource], [CategoryID], [NewsStatus], [CreatedByID], [UpdatedByID], [ModifiedDate]) VALUES (N'12', N'Alumni Entrepreneur Establishes $1M Scholarship Fund for Underprivileged Students', N'Alumni Entrepreneur Establishes $1M Scholarship Fund for Underprivileged Students', CAST(N'2024-06-10T00:00:00.000' AS DateTime), N'Successful entrepreneur and FU alumnus, James Mitchell (Class of 2010), has established a $1 million scholarship fund to support underprivileged students pursuing degrees in business and technology fields.

The James Mitchell Opportunity Scholarship will provide full tuition coverage for up to 20 students annually, with additional support for textbooks, housing, and living expenses.

Selection criteria focus on academic potential, financial need, and demonstrated commitment to using education as a tool for positive community impact.

Mitchell, who founded three successful tech startups after graduating from FU, credits his university education with providing the foundation for his entrepreneurial success.

The scholarship program includes mentorship opportunities, with Mitchell and other successful alumni providing guidance to scholarship recipients throughout their academic journey.

Recipients will also have access to internship opportunities at Mitchell''s companies and partner organizations, providing valuable real-world experience.

The first cohort of scholarship recipients will be selected for the fall 2024 semester, with applications opening next month.

This generous contribution reflects the strong bond between FU and its alumni community, demonstrating the lasting impact of quality education on graduates'' desire to give back.', N'Alumni Relations Office', 4, 1, 1, 1, CAST(N'2024-06-10T00:00:00.000' AS DateTime))
GO

GO
INSERT [dbo].[NewsArticle] ([NewsArticleID], [NewsTitle], [Headline], [CreatedDate], [NewsContent], [NewsSource], [CategoryID], [NewsStatus], [CreatedByID], [UpdatedByID], [ModifiedDate]) VALUES (N'4', N'Renowned Scholar Appointed as Head of AI Department at FU', N'Renowned Scholar Appointed as Head of AI Department at FU', CAST(N'2024-05-05T00:00:00.000' AS DateTime), N'FU proudly announces the appointment of David Nitzevet, a distinguished scholar in Machine Learning, to the prestigious position of Head of AI Department, underscoring the institution''s commitment to academic excellence and leadership.

David Nitzevet brings a wealth of experience and expertise to the role, with a notable track record of the development of deep learning algorithms and the application of machine learning in healthcare, finance, and marketing. In accepting the appointment, David Nitzevet expressed a vision to develop new algorithmic models, improve data preprocessing techniques, and enhance the interpretability of machine learning models, align with the university''s mission to drive innovation and excellence in Machine Learning.

The appointment is expected to foster collaborations and initiatives that will enrich the academic and research landscape of the university and beyond.

The addition of David Nitzevet to the AI Department faculty elevates the institution''s academic standing and promises to inspire students, scholars, and professionals in Machine Learning. The appointment reaffirms the university''s dedication to recruiting top-tier talent and nurturing an environment where academic distinction thrives.
', N'N/A', 1, 1, 2, 2, CAST(N'2024-05-05T00:00:00.000' AS DateTime))
GO
INSERT [dbo].[NewsArticle] ([NewsArticleID], [NewsTitle], [Headline], [CreatedDate], [NewsContent], [NewsSource], [CategoryID], [NewsStatus], [CreatedByID], [UpdatedByID], [ModifiedDate]) VALUES (N'5', N'New Research Findings Shed Light on STEM', N'New Research Findings Shed Light on STEM', CAST(N'2024-05-05T00:00:00.000' AS DateTime), N'Groundbreaking research conducted by the Research Department of FU has unveiled significant findings in the field of STEM, offering fresh insights that could revolutionize current understanding and practices.

The success of this research is attributed to the collaborative efforts of a multidisciplinary team, showcasing the institution''s commitment to fostering cutting-edge research. The newly uncovered knowledge has the potential to influence Math, Engineering, Technology and shape future research endeavors, positioning the Research Department of FU as a leader in the advancement of STEM.

The research findings stand as a testament to the institution''s dedication to impactful research and its contribution to the global knowledge base in STEM.', N'N/A', 1, 1, 2, 2, CAST(N'2024-05-05T00:00:00.000' AS DateTime))
GO


INSERT [dbo].[NewsTag] ([NewsArticleID], [TagID]) VALUES (N'1', 5)
GO
INSERT [dbo].[NewsTag] ([NewsArticleID], [TagID]) VALUES (N'1', 7)
GO
INSERT [dbo].[NewsTag] ([NewsArticleID], [TagID]) VALUES (N'1', 9)
GO
INSERT [dbo].[NewsTag] ([NewsArticleID], [TagID]) VALUES (N'2', 5)
GO
INSERT [dbo].[NewsTag] ([NewsArticleID], [TagID]) VALUES (N'2', 7)
GO
INSERT [dbo].[NewsTag] ([NewsArticleID], [TagID]) VALUES (N'2', 8)
GO
INSERT [dbo].[NewsTag] ([NewsArticleID], [TagID]) VALUES (N'2', 9)
GO
INSERT [dbo].[NewsTag] ([NewsArticleID], [TagID]) VALUES (N'3', 1)
GO
INSERT [dbo].[NewsTag] ([NewsArticleID], [TagID]) VALUES (N'3', 8)
GO
INSERT [dbo].[NewsTag] ([NewsArticleID], [TagID]) VALUES (N'3', 9)
GO
INSERT [dbo].[NewsTag] ([NewsArticleID], [TagID]) VALUES (N'4', 1)
GO
INSERT [dbo].[NewsTag] ([NewsArticleID], [TagID]) VALUES (N'4', 4)
GO
INSERT [dbo].[NewsTag] ([NewsArticleID], [TagID]) VALUES (N'4', 8)
GO
INSERT [dbo].[NewsTag] ([NewsArticleID], [TagID]) VALUES (N'4', 9)
GO
INSERT [dbo].[NewsTag] ([NewsArticleID], [TagID]) VALUES (N'5', 2)
GO
INSERT [dbo].[NewsTag] ([NewsArticleID], [TagID]) VALUES (N'5', 3)
GO
INSERT [dbo].[NewsTag] ([NewsArticleID], [TagID]) VALUES (N'5', 4)
GO
INSERT [dbo].[NewsTag] ([NewsArticleID], [TagID]) VALUES (N'5', 6)
GO
INSERT [dbo].[SystemAccount] ([AccountID], [AccountName], [AccountEmail], [AccountRole], [AccountPassword]) VALUES (1, N'Emma William', N'EmmaWilliam@FUNewsManagement.org', 2, N'@1')
GO
INSERT [dbo].[SystemAccount] ([AccountID], [AccountName], [AccountEmail], [AccountRole], [AccountPassword]) VALUES (2, N'Olivia James', N'OliviaJames@FUNewsManagement.org', 2, N'@1')
GO
INSERT [dbo].[SystemAccount] ([AccountID], [AccountName], [AccountEmail], [AccountRole], [AccountPassword]) VALUES (3, N'Isabella David', N'IsabellaDavid@FUNewsManagement.org', 1, N'@1')
GO
INSERT [dbo].[SystemAccount] ([AccountID], [AccountName], [AccountEmail], [AccountRole], [AccountPassword]) VALUES (4, N'Michael Charlotte', N'MichaelCharlotte@FUNewsManagement.org', 1, N'@1')
GO
INSERT [dbo].[SystemAccount] ([AccountID], [AccountName], [AccountEmail], [AccountRole], [AccountPassword]) VALUES (5, N'Steve Paris', N'SteveParis@FUNewsManagement.org', 1, N'@1')
GO

INSERT [dbo].[Tag] ([TagID], [TagName], [Note]) VALUES (1, N'Education', N'Education Note')
GO
INSERT [dbo].[Tag] ([TagID], [TagName], [Note]) VALUES (2, N'Technology', N'Technology Note')
GO
INSERT [dbo].[Tag] ([TagID], [TagName], [Note]) VALUES (3, N'Research', N'Research Note')
GO
INSERT [dbo].[Tag] ([TagID], [TagName], [Note]) VALUES (4, N'Innovation', N'Innovation Note')
GO
INSERT [dbo].[Tag] ([TagID], [TagName], [Note]) VALUES (5, N'Campus Life', N'Campus Life Note')
GO
INSERT [dbo].[Tag] ([TagID], [TagName], [Note]) VALUES (6, N'Faculty', N'Faculty Achievements')
GO
INSERT [dbo].[Tag] ([TagID], [TagName], [Note]) VALUES (7, N'Alumni ', N'Alumni News')
GO
INSERT [dbo].[Tag] ([TagID], [TagName], [Note]) VALUES (8, N'Events', N'University Events')
GO
INSERT [dbo].[Tag] ([TagID], [TagName], [Note]) VALUES (9, N'Resources', N'Campus Resources')
GO
ALTER TABLE [dbo].[Category]  WITH CHECK ADD  CONSTRAINT [FK_Category_Category] FOREIGN KEY([ParentCategoryID])
REFERENCES [dbo].[Category] ([CategoryID])
GO
ALTER TABLE [dbo].[Category] CHECK CONSTRAINT [FK_Category_Category]
GO
ALTER TABLE [dbo].[NewsArticle]  WITH CHECK ADD  CONSTRAINT [FK_NewsArticle_Category] FOREIGN KEY([CategoryID])
REFERENCES [dbo].[Category] ([CategoryID])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[NewsArticle] CHECK CONSTRAINT [FK_NewsArticle_Category]
GO
ALTER TABLE [dbo].[NewsArticle]  WITH CHECK ADD  CONSTRAINT [FK_NewsArticle_SystemAccount] FOREIGN KEY([CreatedByID])
REFERENCES [dbo].[SystemAccount] ([AccountID])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[NewsArticle] CHECK CONSTRAINT [FK_NewsArticle_SystemAccount]
GO
ALTER TABLE [dbo].[NewsTag]  WITH CHECK ADD  CONSTRAINT [FK_NewsTag_NewsArticle] FOREIGN KEY([NewsArticleID])
REFERENCES [dbo].[NewsArticle] ([NewsArticleID])
GO
ALTER TABLE [dbo].[NewsTag] CHECK CONSTRAINT [FK_NewsTag_NewsArticle]
GO
ALTER TABLE [dbo].[NewsTag]  WITH CHECK ADD  CONSTRAINT [FK_NewsTag_Tag] FOREIGN KEY([TagID])
REFERENCES [dbo].[Tag] ([TagID])
GO
ALTER TABLE [dbo].[NewsTag] CHECK CONSTRAINT [FK_NewsTag_Tag]
GO
USE [master]
GO
ALTER DATABASE [FUNewsManagementFall2024] SET  READ_WRITE 
GO
