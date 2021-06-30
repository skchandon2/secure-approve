--Secure approval db
use secureapproval;
Go
--Drop tables
IF OBJECT_ID('workflow.ApproveRejectActions', 'U') IS NOT NULL
	DROP table workflow.ApproveRejectActions
GO
IF OBJECT_ID('workflow.RequestForm', 'U') IS NOT NULL
	DROP table workflow.RequestForm
GO
IF OBJECT_ID('userInfo.appUsers', 'U') IS NOT NULL
	DROP table userInfo.appUsers
GO
IF OBJECT_ID('userInfo.appRoles', 'U') IS NOT NULL
	DROP table userInfo.appRoles
GO



--Drop schemas
IF EXISTS (SELECT * FROM sys.schemas WHERE name = N'workflow')
	DROP schema workflow;
GO
IF EXISTS (SELECT * FROM sys.schemas WHERE name = N'userInfo')
	DROP schema userInfo;
GO

--schema
GO 
CREATE SCHEMA workflow;
GO 
CREATE SCHEMA userInfo;

--approval request table
GO
CREATE TABLE workflow.RequestForm
(
	requestFormId BIGINT IDENTITY(1,1) PRIMARY KEY,
	requestorEmail NVARCHAR(300) NOT NULL,
	requestorUsername NVARCHAR(300) NULL,
	requestReason NVARCHAR(1000) Not Null,
	requestedAmount decimal(10,2) Not Null,
	isSubmitted bit Not Null default 0,
	requestDate DateTime Null DEFAULT GETDATE(),
	
	
)
	--index
	CREATE NONCLUSTERED INDEX [NonClustered_RequestFormEmail] ON workflow.RequestForm
	(
		requestorEmail
	)

	CREATE NONCLUSTERED INDEX [NonClustered_RequestFormAmount] ON workflow.RequestForm
	(
		requestedAmount
	)

--approval actions
GO
CREATE TABLE workflow.ApproveRejectActions
(
	actionId BIGINT IDENTITY(1,1) PRIMARY KEY,
	requestFormId BIGINT NOT NULL,
	requestorEmail NVARCHAR(300) NOT NULL,
	approverEmail NVARCHAR(300) NOT NULL,
	approverUsername NVARCHAR(300) NULL,
	requestReason NVARCHAR(1000) Not Null,
	requestedAmount decimal(10,2) Not Null,
	requestDate DateTime Null DEFAULT GETDATE(),
	isApproved bit null,
	isRejected bit null,
	approvalDate DateTime Null,
	userDataHash varchar(64), --SHA2_256Length
	approvalRejectWithUserDataHash varchar(64)
	
)
ALTER TABLE workflow.ApproveRejectActions ADD CONSTRAINT fk_requestId FOREIGN KEY (requestFormId) REFERENCES workflow.RequestForm(requestFormId);

	--index
	CREATE NONCLUSTERED INDEX [NonClustered_approvalrequestByRequestFormId] ON workflow.ApproveRejectActions
	(
		requestFormId
	)
	CREATE NONCLUSTERED INDEX [NonClustered_approvalrequestByApproverEmail] ON workflow.ApproveRejectActions
	(
		approverEmail
	)
	CREATE NONCLUSTERED INDEX [NonClustered_approvalrequestByRequestorEmail] ON workflow.ApproveRejectActions
	(
		requestorEmail
	)
	CREATE NONCLUSTERED INDEX [NonClustered_approvalrequestByRequestAmount] ON workflow.ApproveRejectActions
	(
		requestedAmount
	)
	CREATE NONCLUSTERED INDEX [NonClustered_approvalrequestByisApproved] ON workflow.ApproveRejectActions
	(
		isApproved
	)
	CREATE NONCLUSTERED INDEX [NonClustered_approvalrequestByisRejected] ON workflow.ApproveRejectActions
	(
		isRejected
	)
	CREATE NONCLUSTERED INDEX [NonClustered_approvalrequestByApprovalDate] ON workflow.ApproveRejectActions
	(
		approvalDate
	)
--app roles
Create Table userInfo.appRoles
(
	roleid int identity(1,1) primary key,
	rolename nvarchar(200) not null,

)

--user table
Create Table userInfo.appUsers
(
	userid int identity(1,1) primary key,
	username nvarchar(200) not null,
	userpass varchar(64) null,
	useremail nvarchar(200) not null,
	userRoleId int not null
)
ALTER TABLE userInfo.appUsers ADD CONSTRAINT fk_userroleid FOREIGN KEY (userRoleId) REFERENCES userInfo.appRoles(roleid);

	--index
	CREATE NONCLUSTERED INDEX [NonClustered_userInfoAppUsersUsername] ON userInfo.appUsers
	(
		username
	)

	CREATE NONCLUSTERED INDEX [NonClustered_userInfoAppUsersRoles] ON userInfo.appUsers
	(
		userRoleId
	)

-------------------
insert into userInfo.approles(rolename) values('Admin')
insert into userInfo.approles(rolename) values('Approver')
insert into userInfo.approles(rolename) values('User')

insert into userInfo.appUsers(username, useremail, userRoleId, userpass) 
	values('skchandon', 'skchandon@test.com', (select roleid from userInfo.appRoles where rolename='Admin'),'937e8d5fbb48bd4949536cd65b8d35c426b80d2f830c5c308e2cdec422ae2244')

insert into userInfo.appUsers(username, useremail, userRoleId, userpass) 
	values('testapprov', 'testapprov@test.com', (select roleid from userInfo.appRoles where rolename='Approver'), '937e8d5fbb48bd4949536cd65b8d35c426b80d2f830c5c308e2cdec422ae2244')

insert into userInfo.appUsers(username, useremail, userRoleId, userpass) 
	values('testuser', 'testuser@test.com', (select roleid from userInfo.appRoles where rolename='User'), '937e8d5fbb48bd4949536cd65b8d35c426b80d2f830c5c308e2cdec422ae2244')