using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using System.Xml.Schema;

/// <summary>
/// Summary description for SchemaValidationListener
/// </summary>
public class SchemaValidationListener
{
	public SchemaValidationListener()
	{
		//
		// TODO: Add constructor logic here
		//
	}

    private int schemaValidationErrorCount;
    private string schemaValidationErrorMessage;

    public int ErrorCount
    {
        get
        {
            return this.schemaValidationErrorCount;
        }
    }

    public string ErrorMessage
    {
        get
        {
            return this.schemaValidationErrorMessage;
        }
    }

    public void OnSchemaValidating(object sender, ValidationEventArgs e)
    {
        this.schemaValidationErrorCount++;
        this.schemaValidationErrorMessage += e.Message;
        this.schemaValidationErrorMessage += "\r\n";
    }
}