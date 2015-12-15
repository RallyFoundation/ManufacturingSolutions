CREATE TABLE [dbo].[ProductKeyInfo] (
    [ProductKeyID]               BIGINT           NOT NULL,
    [ProductKey]                 NVARCHAR (50)    NULL,
    [ProductKeyStateID]          TINYINT          NOT NULL,
    [ProductKeyState]            NVARCHAR (30)    NULL,
    [HardwareID]                 NVARCHAR (512)   NULL,
    [OEMPartNumber]              NVARCHAR (35)    NULL,
    [SoldToCustomerName]         NVARCHAR (80)    NULL,
    [OrderUniqueID]              UNIQUEIDENTIFIER NULL,
    [SoldToCustomerID]           CHAR (10) COLLATE SQL_Latin1_General_CP1_CS_AS       NULL,
    [CallOffReferenceNumber]     NVARCHAR (35)    NULL,
    [OEMPONumber]                NVARCHAR (35)    NULL,
    [MSOrderNumber]              NVARCHAR (10)    NULL,
    [LicensablePartNumber]       NVARCHAR (16)    NULL,
    [Quantity]                   INT              NULL,
    [SKUID]                      NVARCHAR (50)    NULL,
    [ReturnReasonCode]           NVARCHAR (10)    NULL,
    [CreatedDate]                DATETIME         NULL,
    [ModifiedDate]               DATETIME         NULL,
    [MSOrderLineNumber]          INT              NULL,
    [OEMPODateUTC]               DATETIME         NULL,
    [ShipToCustomerID]           CHAR (10) COLLATE SQL_Latin1_General_CP1_CS_AS       NULL,
    [ShipToCustomerName]         NVARCHAR (80)    NULL,
    [LicensableName]             NVARCHAR (40)    NULL,
    [OEMPOLineNumber]            NVARCHAR (6)     NULL,
    [CallOffLineNumber]          NVARCHAR (6)     NULL,
    [FulfillmentResendIndicator] BIT              NULL,
    [FulfillmentNumber]          CHAR (10)        NULL,
    [FulfilledDateUTC]           DATETIME         NULL,
    [FulfillmentCreateDateUTC]   DATETIME         NULL,
    [EndItemPartNumber]          NVARCHAR (18)    NULL,
    [ZPC_MODEL_SKU]              NVARCHAR (64)    NULL,
    [ZMANUF_GEO_LOC]             NVARCHAR (10)    NULL,
    [ZPGM_ELIG_VALUES]           NVARCHAR (48)    NULL,
    [ZOEM_EXT_ID]                NVARCHAR (16)    NULL,
    [ZCHANNEL_REL_ID]            NVARCHAR (32)       NULL,
	[ZFRM_FACTOR_CL1]            nvarchar (64) NULL,
    [ZFRM_FACTOR_CL2]            nvarchar (64) NULL,
    [ZSCREEN_SIZE]               nvarchar (32) NULL,
    [ZTOUCH_SCREEN]              nvarchar (32) NULL,
	[SerialNumber]               NVARCHAR (36) NULL,
    [TrackingInfo]               NVARCHAR (1024)  NULL,
	[Tags] [nvarchar](200) NULL,
	[Description] [nvarchar](500) NULL,
);












 




