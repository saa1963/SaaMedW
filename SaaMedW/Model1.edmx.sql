
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, 2012 and Azure
-- --------------------------------------------------
-- Date Created: 05/30/2018 09:11:16
-- Generated from EDMX file: C:\Users\soshin.OPER\source\Repos\SaaMedW\SaaMedW\Model1.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [SaaMed];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[FK_Benefit_ToTable]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Benefit] DROP CONSTRAINT [FK_Benefit_ToTable];
GO
IF OBJECT_ID(N'[dbo].[FK_Graphic_Personal]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Graphic] DROP CONSTRAINT [FK_Graphic_Personal];
GO
IF OBJECT_ID(N'[dbo].[FK_Invoice_Person]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Invoice] DROP CONSTRAINT [FK_Invoice_Person];
GO
IF OBJECT_ID(N'[dbo].[FK_Invoice_Visit]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Invoice] DROP CONSTRAINT [FK_Invoice_Visit];
GO
IF OBJECT_ID(N'[dbo].[FK_Person_DocumentType]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Person] DROP CONSTRAINT [FK_Person_DocumentType];
GO
IF OBJECT_ID(N'[dbo].[FK_Specialty_ToSpecialty]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Specialty] DROP CONSTRAINT [FK_Specialty_ToSpecialty];
GO
IF OBJECT_ID(N'[dbo].[FK_Table_Benefit]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[VisitBenefit] DROP CONSTRAINT [FK_Table_Benefit];
GO
IF OBJECT_ID(N'[dbo].[FK_Table_Personal]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[PersonalSpecialty] DROP CONSTRAINT [FK_Table_Personal];
GO
IF OBJECT_ID(N'[dbo].[FK_Table_Specialty]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[PersonalSpecialty] DROP CONSTRAINT [FK_Table_Specialty];
GO
IF OBJECT_ID(N'[dbo].[FK_Table_ToTable]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[InvoiceDetail] DROP CONSTRAINT [FK_Table_ToTable];
GO
IF OBJECT_ID(N'[dbo].[FK_Table_Visit]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[VisitBenefit] DROP CONSTRAINT [FK_Table_Visit];
GO
IF OBJECT_ID(N'[dbo].[FK_Visit_ToPerson]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Visit] DROP CONSTRAINT [FK_Visit_ToPerson];
GO
IF OBJECT_ID(N'[dbo].[FK_Visit_ToPersonal]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Visit] DROP CONSTRAINT [FK_Visit_ToPersonal];
GO

-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[Benefit]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Benefit];
GO
IF OBJECT_ID(N'[dbo].[DocumentType]', 'U') IS NOT NULL
    DROP TABLE [dbo].[DocumentType];
GO
IF OBJECT_ID(N'[dbo].[Graphic]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Graphic];
GO
IF OBJECT_ID(N'[dbo].[Invoice]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Invoice];
GO
IF OBJECT_ID(N'[dbo].[InvoiceDetail]', 'U') IS NOT NULL
    DROP TABLE [dbo].[InvoiceDetail];
GO
IF OBJECT_ID(N'[dbo].[Person]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Person];
GO
IF OBJECT_ID(N'[dbo].[Personal]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Personal];
GO
IF OBJECT_ID(N'[dbo].[PersonalSpecialty]', 'U') IS NOT NULL
    DROP TABLE [dbo].[PersonalSpecialty];
GO
IF OBJECT_ID(N'[dbo].[Specialty]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Specialty];
GO
IF OBJECT_ID(N'[dbo].[Users]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Users];
GO
IF OBJECT_ID(N'[dbo].[Visit]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Visit];
GO
IF OBJECT_ID(N'[dbo].[VisitBenefit]', 'U') IS NOT NULL
    DROP TABLE [dbo].[VisitBenefit];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'DocumentType'
CREATE TABLE [dbo].[DocumentType] (
    [Id] int  NOT NULL,
    [Name] nvarchar(300)  NOT NULL
);
GO

-- Creating table 'Person'
CREATE TABLE [dbo].[Person] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [LastName] nvarchar(50)  NOT NULL,
    [FirstName] nvarchar(50)  NOT NULL,
    [MiddleName] nvarchar(50)  NULL,
    [BirthDate] datetime  NULL,
    [Phone] nchar(10)  NULL,
    [Sex] int  NULL,
    [Inn] nvarchar(12)  NULL,
    [Snils] nchar(11)  NULL,
    [DocumentTypeId] int  NULL,
    [DocSeria] nvarchar(50)  NULL,
    [DocNumber] nvarchar(50)  NULL,
    [AddressSubject] nvarchar(50)  NULL,
    [AddressRaion] nvarchar(50)  NULL,
    [AddressCity] nvarchar(50)  NULL,
    [AddressPunkt] nvarchar(50)  NULL,
    [AddressStreet] nvarchar(50)  NULL,
    [AddressHouse] nvarchar(50)  NULL,
    [AddressFlat] nvarchar(50)  NULL,
    [Mestnost] int  NULL,
    [CreateDate] datetime  NOT NULL
);
GO

-- Creating table 'Users'
CREATE TABLE [dbo].[Users] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Fio] nvarchar(50)  NOT NULL,
    [Login] nvarchar(50)  NOT NULL,
    [Password] varbinary(50)  NULL,
    [Role] int  NOT NULL,
    [Disabled] bit  NOT NULL
);
GO

-- Creating table 'Specialty'
CREATE TABLE [dbo].[Specialty] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(100)  NOT NULL,
    [ParentId] int  NULL
);
GO

-- Creating table 'Personal'
CREATE TABLE [dbo].[Personal] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Fio] nvarchar(50)  NOT NULL,
    [Active] bit  NOT NULL
);
GO

-- Creating table 'Graphic'
CREATE TABLE [dbo].[Graphic] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [PersonalId] int  NOT NULL,
    [Dt] datetime  NOT NULL,
    [H1] int  NOT NULL,
    [M1] int  NOT NULL,
    [H2] int  NOT NULL,
    [M2] int  NOT NULL
);
GO

-- Creating table 'Benefit'
CREATE TABLE [dbo].[Benefit] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(100)  NOT NULL,
    [Price] decimal(19,4)  NOT NULL,
    [SpecialtyId] int  NOT NULL,
    [Duration] int  NOT NULL
);
GO

-- Creating table 'Invoice'
CREATE TABLE [dbo].[Invoice] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Dt] datetime  NOT NULL,
    [Status] int  NOT NULL,
    [PersonId] int  NOT NULL,
    [Sm] decimal(19,4)  NOT NULL,
    [VisitId] int  NULL
);
GO

-- Creating table 'InvoiceDetail'
CREATE TABLE [dbo].[InvoiceDetail] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [BenefitName] nvarchar(100)  NOT NULL,
    [Kol] int  NOT NULL,
    [Price] decimal(19,4)  NOT NULL,
    [Sm] decimal(19,4)  NOT NULL,
    [InvoiceId] int  NOT NULL
);
GO

-- Creating table 'Visit'
CREATE TABLE [dbo].[Visit] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [PersonId] int  NOT NULL,
    [PersonalId] int  NOT NULL,
    [Dt] datetime  NOT NULL,
    [Status] int  NOT NULL,
    [Duration] int  NOT NULL
);
GO

-- Creating table 'VisitBenefit'
CREATE TABLE [dbo].[VisitBenefit] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [VisitId] int  NOT NULL,
    [BenefitId] int  NOT NULL,
    [Status] int  NOT NULL,
    [Kol] int  NOT NULL
);
GO

-- Creating table 'PersonalSpecialty'
CREATE TABLE [dbo].[PersonalSpecialty] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [PersonalId] int  NOT NULL,
    [SpecialtyId] int  NOT NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [Id] in table 'DocumentType'
ALTER TABLE [dbo].[DocumentType]
ADD CONSTRAINT [PK_DocumentType]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Person'
ALTER TABLE [dbo].[Person]
ADD CONSTRAINT [PK_Person]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Users'
ALTER TABLE [dbo].[Users]
ADD CONSTRAINT [PK_Users]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Specialty'
ALTER TABLE [dbo].[Specialty]
ADD CONSTRAINT [PK_Specialty]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Personal'
ALTER TABLE [dbo].[Personal]
ADD CONSTRAINT [PK_Personal]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Graphic'
ALTER TABLE [dbo].[Graphic]
ADD CONSTRAINT [PK_Graphic]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Benefit'
ALTER TABLE [dbo].[Benefit]
ADD CONSTRAINT [PK_Benefit]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Invoice'
ALTER TABLE [dbo].[Invoice]
ADD CONSTRAINT [PK_Invoice]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'InvoiceDetail'
ALTER TABLE [dbo].[InvoiceDetail]
ADD CONSTRAINT [PK_InvoiceDetail]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Visit'
ALTER TABLE [dbo].[Visit]
ADD CONSTRAINT [PK_Visit]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'VisitBenefit'
ALTER TABLE [dbo].[VisitBenefit]
ADD CONSTRAINT [PK_VisitBenefit]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'PersonalSpecialty'
ALTER TABLE [dbo].[PersonalSpecialty]
ADD CONSTRAINT [PK_PersonalSpecialty]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- Creating foreign key on [DocumentTypeId] in table 'Person'
ALTER TABLE [dbo].[Person]
ADD CONSTRAINT [FK_Person_DocumentType]
    FOREIGN KEY ([DocumentTypeId])
    REFERENCES [dbo].[DocumentType]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Person_DocumentType'
CREATE INDEX [IX_FK_Person_DocumentType]
ON [dbo].[Person]
    ([DocumentTypeId]);
GO

-- Creating foreign key on [PersonalId] in table 'Graphic'
ALTER TABLE [dbo].[Graphic]
ADD CONSTRAINT [FK_Graphic_Personal]
    FOREIGN KEY ([PersonalId])
    REFERENCES [dbo].[Personal]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Graphic_Personal'
CREATE INDEX [IX_FK_Graphic_Personal]
ON [dbo].[Graphic]
    ([PersonalId]);
GO

-- Creating foreign key on [SpecialtyId] in table 'Benefit'
ALTER TABLE [dbo].[Benefit]
ADD CONSTRAINT [FK_Benefit_ToTable]
    FOREIGN KEY ([SpecialtyId])
    REFERENCES [dbo].[Specialty]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Benefit_ToTable'
CREATE INDEX [IX_FK_Benefit_ToTable]
ON [dbo].[Benefit]
    ([SpecialtyId]);
GO

-- Creating foreign key on [BenefitId] in table 'VisitBenefit'
ALTER TABLE [dbo].[VisitBenefit]
ADD CONSTRAINT [FK_Table_Benefit]
    FOREIGN KEY ([BenefitId])
    REFERENCES [dbo].[Benefit]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Table_Benefit'
CREATE INDEX [IX_FK_Table_Benefit]
ON [dbo].[VisitBenefit]
    ([BenefitId]);
GO

-- Creating foreign key on [PersonId] in table 'Invoice'
ALTER TABLE [dbo].[Invoice]
ADD CONSTRAINT [FK_Invoice_Person]
    FOREIGN KEY ([PersonId])
    REFERENCES [dbo].[Person]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Invoice_Person'
CREATE INDEX [IX_FK_Invoice_Person]
ON [dbo].[Invoice]
    ([PersonId]);
GO

-- Creating foreign key on [InvoiceId] in table 'InvoiceDetail'
ALTER TABLE [dbo].[InvoiceDetail]
ADD CONSTRAINT [FK_Table_ToTable]
    FOREIGN KEY ([InvoiceId])
    REFERENCES [dbo].[Invoice]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Table_ToTable'
CREATE INDEX [IX_FK_Table_ToTable]
ON [dbo].[InvoiceDetail]
    ([InvoiceId]);
GO

-- Creating foreign key on [PersonId] in table 'Visit'
ALTER TABLE [dbo].[Visit]
ADD CONSTRAINT [FK_Visit_ToPerson]
    FOREIGN KEY ([PersonId])
    REFERENCES [dbo].[Person]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Visit_ToPerson'
CREATE INDEX [IX_FK_Visit_ToPerson]
ON [dbo].[Visit]
    ([PersonId]);
GO

-- Creating foreign key on [PersonalId] in table 'Visit'
ALTER TABLE [dbo].[Visit]
ADD CONSTRAINT [FK_Visit_ToPersonal]
    FOREIGN KEY ([PersonalId])
    REFERENCES [dbo].[Personal]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Visit_ToPersonal'
CREATE INDEX [IX_FK_Visit_ToPersonal]
ON [dbo].[Visit]
    ([PersonalId]);
GO

-- Creating foreign key on [VisitId] in table 'VisitBenefit'
ALTER TABLE [dbo].[VisitBenefit]
ADD CONSTRAINT [FK_Table_Visit]
    FOREIGN KEY ([VisitId])
    REFERENCES [dbo].[Visit]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Table_Visit'
CREATE INDEX [IX_FK_Table_Visit]
ON [dbo].[VisitBenefit]
    ([VisitId]);
GO

-- Creating foreign key on [PersonalId] in table 'PersonalSpecialty'
ALTER TABLE [dbo].[PersonalSpecialty]
ADD CONSTRAINT [FK_Table_Personal]
    FOREIGN KEY ([PersonalId])
    REFERENCES [dbo].[Personal]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Table_Personal'
CREATE INDEX [IX_FK_Table_Personal]
ON [dbo].[PersonalSpecialty]
    ([PersonalId]);
GO

-- Creating foreign key on [SpecialtyId] in table 'PersonalSpecialty'
ALTER TABLE [dbo].[PersonalSpecialty]
ADD CONSTRAINT [FK_Table_Specialty]
    FOREIGN KEY ([SpecialtyId])
    REFERENCES [dbo].[Specialty]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Table_Specialty'
CREATE INDEX [IX_FK_Table_Specialty]
ON [dbo].[PersonalSpecialty]
    ([SpecialtyId]);
GO

-- Creating foreign key on [VisitId] in table 'Invoice'
ALTER TABLE [dbo].[Invoice]
ADD CONSTRAINT [FK_Invoice_Visit1]
    FOREIGN KEY ([VisitId])
    REFERENCES [dbo].[Visit]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Invoice_Visit1'
CREATE INDEX [IX_FK_Invoice_Visit1]
ON [dbo].[Invoice]
    ([VisitId]);
GO

-- Creating foreign key on [ParentId] in table 'Specialty'
ALTER TABLE [dbo].[Specialty]
ADD CONSTRAINT [FK_Specialty_ToSpecialty]
    FOREIGN KEY ([ParentId])
    REFERENCES [dbo].[Specialty]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Specialty_ToSpecialty'
CREATE INDEX [IX_FK_Specialty_ToSpecialty]
ON [dbo].[Specialty]
    ([ParentId]);
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------