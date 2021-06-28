--Secure approval db

--Drop tables
IF OBJECT_ID('workflow.RequestForm', 'U') IS NOT NULL
	DROP table workflow.RequestForm
GO
IF OBJECT_ID('workflow.ApproveRejectActions', 'U') IS NOT NULL
	DROP table workflow.ApproveRejectActions
GO

--Drop schemas
IF EXISTS (SELECT * FROM sys.schemas WHERE name = N'workflow')
	DROP schema workflow;
GO

--schema
GO 
CREATE SCHEMA workflow;

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
	userDataHash binary(64), --SHA2_512Length
	approvalRejectWithUserDataHash binary(64)
	
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