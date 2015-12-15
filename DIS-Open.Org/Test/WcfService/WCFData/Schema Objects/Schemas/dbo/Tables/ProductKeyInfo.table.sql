CREATE TABLE [dbo].[ProductKeyInfo] (
    [ProductKeyID]               BIGINT           NOT NULL,
    [ProductKey]                 VARCHAR (29)     NOT NULL,
    [HardwareID]                 VARCHAR (128)    NULL,
    [EndItemPartNumber]          VARCHAR (18)     NULL,
    [OEMPartNumber]              VARCHAR (18)     NULL,
    [OEMAdditionalInfo]          XML              NULL,
    [SoldToCustomerName]         VARCHAR (30)     NULL,
    [OrderUniqueID]              UNIQUEIDENTIFIER NULL,
    [SoldToCustomerID]           VARCHAR (10)     NOT NULL,
    [CallOffReferenceNumber]     VARCHAR (20)     NULL,
    [OEMPONumber]                VARCHAR (30)     NULL,
    [OEMPOLineNumber]            INT              NULL,
    [MSOrderNumber]              VARCHAR (20)     NULL,
    [MSOrderLineNumber]          INT              NULL,
    [LicensablePartNumber]       VARCHAR (18)     NULL,
    [Quantity]                   INT              NULL,
    [ProductKeyState]            VARCHAR (20)     NULL,
    [SKUID]                      VARCHAR (50)     NULL,
    [ReturnReasonCode]           VARCHAR (10)     NULL,
    [ActionCode]                 VARCHAR (1)      NULL,
    [CreatedBy]                  VARCHAR (30)     NULL,
    [CreatedDate]                DATETIME         NULL,
    [ModifiedBy]                 VARCHAR (30)     NULL,
    [ModifiedDate]               DATETIME         NULL,
    [FulfillmentResendIndicator] BIT              NULL,
    [FulfillmentNumber]          CHAR (10)        NULL,
    [FulfilledDateUTC]           DATETIME         NULL,
    [FulfillmentCreateDateUTC]   DATETIME         NULL,
    [OEMPODateUTC]               DATETIME         NULL,
    [ShipToCustomerID]           VARCHAR (10)     NOT NULL,
    [ShipToCustomerName]         VARCHAR (30)     NULL,
    [LicensableName]             VARCHAR (50)     NULL,
    [CallOffLineNumber]          VARCHAR (10)     NULL,
    [DeliveryNumber]             VARCHAR (10)     NULL,
    [DeliveryLineItemNumber]     VARCHAR (10)     NULL,
    [Status]                     BIT              NULL,
    [ProductKeyStateID]          TINYINT          NOT NULL
);





