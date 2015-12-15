-- =============================================
-- Script Template
-- =============================================

/****** Object:  Table [dbo].[ProductKeyInfo]    Script Date: 05/18/2011 14:05:00 ******/
-- Generate pretend keys for testing. 
SET NOCOUNT ON 
  
DECLARE @OEMPartNumber VARCHAR(16), 
        @CreateDate DATETIME, 
        @CreateBy   VARCHAR(50),
        @OEMPONumber VARCHAR(30),
        @OEMPODateUTC DATETIME,
        @MSOrderNumber VARCHAR(20),
        @SoldToCustomerID VARCHAR(10),
        @SoldToCustomerName VARCHAR(30),
        @ShipToCustomerID VARCHAR(10),
        @ShipToCustomerName VARCHAR(30),
        @CallOffReferenceNumber VARCHAR(20),
        @LicensablePartNumber VARCHAR(18),
        @FulfillmentResendIndicator BIT,
        @Status BIT,
        @ProductKeyStateID tinyint,
        @Quantity INT,
        @EndItemPartNumber VARCHAR(18),
        @MSOrderLineNumber INT,
        @SKUID VARCHAR(16),
        @KeyNbrE INT, 
        @KeyNbrD INT 
  
SELECT  @OEMPartNumber = '3TC-00172' 
        , @CreateDate = GETDATE() 
        , @CreateBy   = 'TEST' 
        , @OEMPONumber = 'OEMPONO1' 
        , @OEMPODateUTC = GETDATE()  
        , @MSOrderNumber = '1234567890' 
        , @SoldToCustomerID = '' 
        , @SoldToCustomerName = 'Microsoft Corporation' 
        , @ShipToCustomerID = '' 
        , @ShipToCustomerName = 'Microsoft China, LSH' 
        , @CallOffReferenceNumber = 'TPI_Reference_Num_1' 
        , @LicensablePartNumber = 'G0A-' 
        , @FulfillmentResendIndicator = 0 
        , @Status = NULL 
        , @ProductKeyStateID = 0 
        , @Quantity = 0 
        , @EndItemPartNumber = 'WN7-00617'
        , @MSOrderLineNumber = 10
        , @SKUID = 'X18-79385'
        , @KeyNbrE = 0
        , @KeyNbrD = 0
  
  
WHILE @KeyNbrD <= 99
BEGIN 
    WHILE @KeyNbrE <= 99
    BEGIN 
      INSERT dbo.productKeyInfo (   
          ProductKeyId 
        , ProductKey 
        , OEMPartNumber
        , OEMPONumber 
        , OEMPODateUTC 
        , MSOrderNumber 
        , SoldToCustomerID 
        , SoldToCustomerName 
        , ShipToCustomerID 
        , ShipToCustomerName
        , CallOffReferenceNumber 
        , LicensablePartNumber 
        , FulfillmentResendIndicator 
        , [Status]
        , ProductKeyStateID 
        , Quantity
        , EndItemPartNumber
        , MSOrderLineNumber
        , SKUID
        , CreatedBy 
        , CreatedDate 
        , ModifiedBy 
        , ModifiedDate 
      ) 
      SELECT  (@KeyNbrD * 100000) + @KeyNbrE + 1
            , 'AAAAA-BBBBB-CCCCC-' +  RIGHT('00000' + CONVERT(VARCHAR, @KeyNbrD), 5) + '-' +  RIGHT('00000' + CONVERT(VARCHAR, @KeyNbrE), 5) 
            , @OEMPartNumber 
            , @OEMPONumber 
            , @OEMPODateUTC 
            , @MSOrderNumber 
            , @SoldToCustomerID 
            , @SoldToCustomerName 
            , @ShipToCustomerID 
            , @ShipToCustomerName
            , @CallOffReferenceNumber 
            , @LicensablePartNumber + RIGHT('00000' + CONVERT(VARCHAR, @KeyNbrD), 5)
            , @FulfillmentResendIndicator 
            , @Status
            , @ProductKeyStateID
            , @Quantity
            , @EndItemPartNumber
            , @MSOrderLineNumber
            , @SKUID
            , @CreateBy, @CreateDate, NULL, NULL 
  
      SET @KeyNbrE = @KeyNbrE + 1 
    END 
    SET @KeyNbrE = 0
    SET @KeyNbrD = @KeyNbrD + 1 
END 

