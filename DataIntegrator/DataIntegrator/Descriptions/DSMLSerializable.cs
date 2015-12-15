using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace DataIntegrator.Descriptions.LDAP.DSML
{
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.42")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "urn:oasis:names:tc:DSML:2:0:core")]
    [System.Xml.Serialization.XmlRootAttribute("batchRequest", Namespace = "urn:oasis:names:tc:DSML:2:0:core", IsNullable = false)]
    public class BatchRequest
    {

        private AuthRequest authRequestField;

        private SearchRequest[] searchRequestField;

        private ModifyRequest[] modifyRequestField;

        private AddRequest[] addRequestField;

        private DelRequest[] delRequestField;

        private ModifyDNRequest[] modDNRequestField;

        private CompareRequest[] compareRequestField;

        private AbandonRequest[] abandonRequestField;

        private ExtendedRequest[] extendedRequestField;

        private string requestIDField;

        private BatchRequestProcessing processingField;

        private BatchRequestResponseOrder responseOrderField;

        private BatchRequestOnError onErrorField;

        public BatchRequest()
        {
            this.processingField = BatchRequestProcessing.sequential;
            this.responseOrderField = BatchRequestResponseOrder.sequential;
            this.onErrorField = BatchRequestOnError.exit;
        }

        /// <remarks/>
        public AuthRequest authRequest
        {
            get
            {
                return this.authRequestField;
            }
            set
            {
                this.authRequestField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("searchRequest")]
        public SearchRequest[] searchRequest
        {
            get
            {
                return this.searchRequestField;
            }
            set
            {
                this.searchRequestField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("modifyRequest")]
        public ModifyRequest[] modifyRequest
        {
            get
            {
                return this.modifyRequestField;
            }
            set
            {
                this.modifyRequestField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("addRequest")]
        public AddRequest[] addRequest
        {
            get
            {
                return this.addRequestField;
            }
            set
            {
                this.addRequestField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("delRequest")]
        public DelRequest[] delRequest
        {
            get
            {
                return this.delRequestField;
            }
            set
            {
                this.delRequestField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("modDNRequest")]
        public ModifyDNRequest[] modDNRequest
        {
            get
            {
                return this.modDNRequestField;
            }
            set
            {
                this.modDNRequestField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("compareRequest")]
        public CompareRequest[] compareRequest
        {
            get
            {
                return this.compareRequestField;
            }
            set
            {
                this.compareRequestField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("abandonRequest")]
        public AbandonRequest[] abandonRequest
        {
            get
            {
                return this.abandonRequestField;
            }
            set
            {
                this.abandonRequestField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("extendedRequest")]
        public ExtendedRequest[] extendedRequest
        {
            get
            {
                return this.extendedRequestField;
            }
            set
            {
                this.extendedRequestField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string requestID
        {
            get
            {
                return this.requestIDField;
            }
            set
            {
                this.requestIDField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        [System.ComponentModel.DefaultValueAttribute(BatchRequestProcessing.sequential)]
        public BatchRequestProcessing processing
        {
            get
            {
                return this.processingField;
            }
            set
            {
                this.processingField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        [System.ComponentModel.DefaultValueAttribute(BatchRequestResponseOrder.sequential)]
        public BatchRequestResponseOrder responseOrder
        {
            get
            {
                return this.responseOrderField;
            }
            set
            {
                this.responseOrderField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        [System.ComponentModel.DefaultValueAttribute(BatchRequestOnError.exit)]
        public BatchRequestOnError onError
        {
            get
            {
                return this.onErrorField;
            }
            set
            {
                this.onErrorField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.42")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "urn:oasis:names:tc:DSML:2:0:core")]
    public class AuthRequest : DsmlMessage
    {

        private string principalField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string principal
        {
            get
            {
                return this.principalField;
            }
            set
            {
                this.principalField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(ExtendedRequest))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(AbandonRequest))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(CompareRequest))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(ModifyDNRequest))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(DelRequest))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(AddRequest))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(ModifyRequest))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(SearchRequest))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(AuthRequest))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(LDAPResult))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(ExtendedResponse))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(SearchResultReference))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(SearchResultEntry))]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.42")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "urn:oasis:names:tc:DSML:2:0:core")]
    public class DsmlMessage
    {

        private Control[] controlField;

        private string requestIDField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("control")]
        public Control[] control
        {
            get
            {
                return this.controlField;
            }
            set
            {
                this.controlField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string requestID
        {
            get
            {
                return this.requestIDField;
            }
            set
            {
                this.requestIDField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.42")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "urn:oasis:names:tc:DSML:2:0:core")]
    public class Control
    {

        private object controlValueField;

        private string typeField;

        private bool criticalityField;

        public Control()
        {
            this.criticalityField = false;
        }

        /// <remarks/>
        public object controlValue
        {
            get
            {
                return this.controlValueField;
            }
            set
            {
                this.controlValueField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string type
        {
            get
            {
                return this.typeField;
            }
            set
            {
                this.typeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        [System.ComponentModel.DefaultValueAttribute(false)]
        public bool criticality
        {
            get
            {
                return this.criticalityField;
            }
            set
            {
                this.criticalityField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.42")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "urn:oasis:names:tc:DSML:2:0:core")]
    public class DsmlModification
    {

        private string[] valueField;

        private string nameField;

        private DsmlModificationOperation operationField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("value")]
        public string[] value
        {
            get
            {
                return this.valueField;
            }
            set
            {
                this.valueField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string name
        {
            get
            {
                return this.nameField;
            }
            set
            {
                this.nameField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public DsmlModificationOperation operation
        {
            get
            {
                return this.operationField;
            }
            set
            {
                this.operationField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.42")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "urn:oasis:names:tc:DSML:2:0:core")]
    public enum DsmlModificationOperation
    {

        /// <remarks/>
        add,

        /// <remarks/>
        delete,

        /// <remarks/>
        replace,
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.42")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "urn:oasis:names:tc:DSML:2:0:core")]
    public class AttributeDescriptions
    {

        private AttributeDescription[] attributeField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("attribute")]
        public AttributeDescription[] attribute
        {
            get
            {
                return this.attributeField;
            }
            set
            {
                this.attributeField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.42")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "urn:oasis:names:tc:DSML:2:0:core")]
    public class AttributeDescription
    {

        private string nameField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string name
        {
            get
            {
                return this.nameField;
            }
            set
            {
                this.nameField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.42")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "urn:oasis:names:tc:DSML:2:0:core")]
    public class MatchingRuleAssertion
    {

        private string valueField;

        private bool dnAttributesField;

        private string matchingRuleField;

        private string nameField;

        public MatchingRuleAssertion()
        {
            this.dnAttributesField = false;
        }

        /// <remarks/>
        public string value
        {
            get
            {
                return this.valueField;
            }
            set
            {
                this.valueField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        [System.ComponentModel.DefaultValueAttribute(false)]
        public bool dnAttributes
        {
            get
            {
                return this.dnAttributesField;
            }
            set
            {
                this.dnAttributesField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string matchingRule
        {
            get
            {
                return this.matchingRuleField;
            }
            set
            {
                this.matchingRuleField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string name
        {
            get
            {
                return this.nameField;
            }
            set
            {
                this.nameField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.42")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "urn:oasis:names:tc:DSML:2:0:core")]
    public class SubstringFilter
    {

        private string initialField;

        private string[] anyField;

        private string finalField;

        private string nameField;

        /// <remarks/>
        public string initial
        {
            get
            {
                return this.initialField;
            }
            set
            {
                this.initialField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("any")]
        public string[] any
        {
            get
            {
                return this.anyField;
            }
            set
            {
                this.anyField = value;
            }
        }

        /// <remarks/>
        public string final
        {
            get
            {
                return this.finalField;
            }
            set
            {
                this.finalField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string name
        {
            get
            {
                return this.nameField;
            }
            set
            {
                this.nameField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.42")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "urn:oasis:names:tc:DSML:2:0:core")]
    public class AttributeValueAssertion
    {

        private string valueField;

        private string nameField;

        /// <remarks/>
        public string value
        {
            get
            {
                return this.valueField;
            }
            set
            {
                this.valueField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string name
        {
            get
            {
                return this.nameField;
            }
            set
            {
                this.nameField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.42")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "urn:oasis:names:tc:DSML:2:0:core")]
    public class FilterSet
    {

        private object[] itemsField;

        private ItemsChoiceType[] itemsElementNameField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("and", typeof(FilterSet))]
        [System.Xml.Serialization.XmlElementAttribute("approxMatch", typeof(AttributeValueAssertion))]
        [System.Xml.Serialization.XmlElementAttribute("equalityMatch", typeof(AttributeValueAssertion))]
        [System.Xml.Serialization.XmlElementAttribute("extensibleMatch", typeof(MatchingRuleAssertion))]
        [System.Xml.Serialization.XmlElementAttribute("greaterOrEqual", typeof(AttributeValueAssertion))]
        [System.Xml.Serialization.XmlElementAttribute("lessOrEqual", typeof(AttributeValueAssertion))]
        [System.Xml.Serialization.XmlElementAttribute("not", typeof(Filter))]
        [System.Xml.Serialization.XmlElementAttribute("or", typeof(FilterSet))]
        [System.Xml.Serialization.XmlElementAttribute("present", typeof(AttributeDescription))]
        [System.Xml.Serialization.XmlElementAttribute("substrings", typeof(SubstringFilter))]
        [System.Xml.Serialization.XmlChoiceIdentifierAttribute("ItemsElementName")]
        public object[] Items
        {
            get
            {
                return this.itemsField;
            }
            set
            {
                this.itemsField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("ItemsElementName")]
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public ItemsChoiceType[] ItemsElementName
        {
            get
            {
                return this.itemsElementNameField;
            }
            set
            {
                this.itemsElementNameField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.42")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "urn:oasis:names:tc:DSML:2:0:core")]
    public class Filter
    {

        private object itemField;

        private ItemChoiceType itemElementNameField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("and", typeof(FilterSet))]
        [System.Xml.Serialization.XmlElementAttribute("approxMatch", typeof(AttributeValueAssertion))]
        [System.Xml.Serialization.XmlElementAttribute("equalityMatch", typeof(AttributeValueAssertion))]
        [System.Xml.Serialization.XmlElementAttribute("extensibleMatch", typeof(MatchingRuleAssertion))]
        [System.Xml.Serialization.XmlElementAttribute("greaterOrEqual", typeof(AttributeValueAssertion))]
        [System.Xml.Serialization.XmlElementAttribute("lessOrEqual", typeof(AttributeValueAssertion))]
        [System.Xml.Serialization.XmlElementAttribute("not", typeof(Filter))]
        [System.Xml.Serialization.XmlElementAttribute("or", typeof(FilterSet))]
        [System.Xml.Serialization.XmlElementAttribute("present", typeof(AttributeDescription))]
        [System.Xml.Serialization.XmlElementAttribute("substrings", typeof(SubstringFilter))]
        [System.Xml.Serialization.XmlChoiceIdentifierAttribute("ItemElementName")]
        public object Item
        {
            get
            {
                return this.itemField;
            }
            set
            {
                this.itemField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public ItemChoiceType ItemElementName
        {
            get
            {
                return this.itemElementNameField;
            }
            set
            {
                this.itemElementNameField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.42")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "urn:oasis:names:tc:DSML:2:0:core", IncludeInSchema = false)]
    public enum ItemChoiceType
    {

        /// <remarks/>
        and,

        /// <remarks/>
        approxMatch,

        /// <remarks/>
        equalityMatch,

        /// <remarks/>
        extensibleMatch,

        /// <remarks/>
        greaterOrEqual,

        /// <remarks/>
        lessOrEqual,

        /// <remarks/>
        not,

        /// <remarks/>
        or,

        /// <remarks/>
        present,

        /// <remarks/>
        substrings,
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.42")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "urn:oasis:names:tc:DSML:2:0:core", IncludeInSchema = false)]
    public enum ItemsChoiceType
    {

        /// <remarks/>
        and,

        /// <remarks/>
        approxMatch,

        /// <remarks/>
        equalityMatch,

        /// <remarks/>
        extensibleMatch,

        /// <remarks/>
        greaterOrEqual,

        /// <remarks/>
        lessOrEqual,

        /// <remarks/>
        not,

        /// <remarks/>
        or,

        /// <remarks/>
        present,

        /// <remarks/>
        substrings,
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.42")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "urn:oasis:names:tc:DSML:2:0:core")]
    public class ErrorResponse
    {

        private string messageField;

        private System.Xml.XmlElement detailField;

        private string requestIDField;

        private ErrorResponseType typeField;

        private bool typeFieldSpecified;

        /// <remarks/>
        public string message
        {
            get
            {
                return this.messageField;
            }
            set
            {
                this.messageField = value;
            }
        }

        /// <remarks/>
        public System.Xml.XmlElement detail
        {
            get
            {
                return this.detailField;
            }
            set
            {
                this.detailField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string requestID
        {
            get
            {
                return this.requestIDField;
            }
            set
            {
                this.requestIDField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public ErrorResponseType type
        {
            get
            {
                return this.typeField;
            }
            set
            {
                this.typeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool typeSpecified
        {
            get
            {
                return this.typeFieldSpecified;
            }
            set
            {
                this.typeFieldSpecified = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.42")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "urn:oasis:names:tc:DSML:2:0:core")]
    public enum ErrorResponseType
    {

        /// <remarks/>
        notAttempted,

        /// <remarks/>
        couldNotConnect,

        /// <remarks/>
        connectionClosed,

        /// <remarks/>
        malformedRequest,

        /// <remarks/>
        gatewayInternalError,

        /// <remarks/>
        authenticationFailed,

        /// <remarks/>
        unresolvableURI,

        /// <remarks/>
        other,
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.42")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "urn:oasis:names:tc:DSML:2:0:core")]
    public class ResultCode
    {

        private int codeField;

        private LDAPResultCode descrField;

        private bool descrFieldSpecified;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public int code
        {
            get
            {
                return this.codeField;
            }
            set
            {
                this.codeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public LDAPResultCode descr
        {
            get
            {
                return this.descrField;
            }
            set
            {
                this.descrField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool descrSpecified
        {
            get
            {
                return this.descrFieldSpecified;
            }
            set
            {
                this.descrFieldSpecified = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.42")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "urn:oasis:names:tc:DSML:2:0:core")]
    public enum LDAPResultCode
    {

        /// <remarks/>
        success,

        /// <remarks/>
        operationsError,

        /// <remarks/>
        protocolError,

        /// <remarks/>
        timeLimitExceeded,

        /// <remarks/>
        sizeLimitExceeded,

        /// <remarks/>
        compareFalse,

        /// <remarks/>
        compareTrue,

        /// <remarks/>
        authMethodNotSupported,

        /// <remarks/>
        strongAuthRequired,

        /// <remarks/>
        referral,

        /// <remarks/>
        adminLimitExceeded,

        /// <remarks/>
        unavailableCriticalExtension,

        /// <remarks/>
        confidentialityRequired,

        /// <remarks/>
        saslBindInProgress,

        /// <remarks/>
        noSuchAttribute,

        /// <remarks/>
        undefinedAttributeType,

        /// <remarks/>
        inappropriateMatching,

        /// <remarks/>
        constraintViolation,

        /// <remarks/>
        attributeOrValueExists,

        /// <remarks/>
        invalidAttributeSyntax,

        /// <remarks/>
        noSuchObject,

        /// <remarks/>
        aliasProblem,

        /// <remarks/>
        invalidDNSyntax,

        /// <remarks/>
        aliasDerefencingProblem,

        /// <remarks/>
        inappropriateAuthentication,

        /// <remarks/>
        invalidCredentials,

        /// <remarks/>
        insufficientAccessRights,

        /// <remarks/>
        busy,

        /// <remarks/>
        unavailable,

        /// <remarks/>
        unwillingToPerform,

        /// <remarks/>
        loopDetect,

        /// <remarks/>
        namingViolation,

        /// <remarks/>
        objectClassViolation,

        /// <remarks/>
        notAllowedOnNonLeaf,

        /// <remarks/>
        notAllowedOnRDN,

        /// <remarks/>
        entryAlreadyExists,

        /// <remarks/>
        objectClassModsProhibited,

        /// <remarks/>
        affectMultipleDSAs,

        /// <remarks/>
        other,
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.42")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "urn:oasis:names:tc:DSML:2:0:core")]
    public class DsmlAttr
    {

        private string[] valueField;

        private string nameField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("value")]
        public string[] value
        {
            get
            {
                return this.valueField;
            }
            set
            {
                this.valueField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string name
        {
            get
            {
                return this.nameField;
            }
            set
            {
                this.nameField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.42")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "urn:oasis:names:tc:DSML:2:0:core")]
    public class SearchResponse
    {

        private SearchResultEntry[] searchResultEntryField;

        private SearchResultReference[] searchResultReferenceField;

        private LDAPResult searchResultDoneField;

        private string requestIDField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("searchResultEntry")]
        public SearchResultEntry[] searchResultEntry
        {
            get
            {
                return this.searchResultEntryField;
            }
            set
            {
                this.searchResultEntryField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("searchResultReference")]
        public SearchResultReference[] searchResultReference
        {
            get
            {
                return this.searchResultReferenceField;
            }
            set
            {
                this.searchResultReferenceField = value;
            }
        }

        /// <remarks/>
        public LDAPResult searchResultDone
        {
            get
            {
                return this.searchResultDoneField;
            }
            set
            {
                this.searchResultDoneField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string requestID
        {
            get
            {
                return this.requestIDField;
            }
            set
            {
                this.requestIDField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.42")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "urn:oasis:names:tc:DSML:2:0:core")]
    public class SearchResultEntry : DsmlMessage
    {

        private DsmlAttr[] attrField;

        private string dnField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("attr")]
        public DsmlAttr[] attr
        {
            get
            {
                return this.attrField;
            }
            set
            {
                this.attrField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string dn
        {
            get
            {
                return this.dnField;
            }
            set
            {
                this.dnField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.42")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "urn:oasis:names:tc:DSML:2:0:core")]
    public class SearchResultReference : DsmlMessage
    {

        private string[] refField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("ref", DataType = "anyURI")]
        public string[] @ref
        {
            get
            {
                return this.refField;
            }
            set
            {
                this.refField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(ExtendedResponse))]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.42")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "urn:oasis:names:tc:DSML:2:0:core")]
    public class LDAPResult : DsmlMessage
    {

        private ResultCode resultCodeField;

        private string errorMessageField;

        private string[] referralField;

        private string matchedDNField;

        /// <remarks/>
        public ResultCode resultCode
        {
            get
            {
                return this.resultCodeField;
            }
            set
            {
                this.resultCodeField = value;
            }
        }

        /// <remarks/>
        public string errorMessage
        {
            get
            {
                return this.errorMessageField;
            }
            set
            {
                this.errorMessageField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("referral", DataType = "anyURI")]
        public string[] referral
        {
            get
            {
                return this.referralField;
            }
            set
            {
                this.referralField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string matchedDN
        {
            get
            {
                return this.matchedDNField;
            }
            set
            {
                this.matchedDNField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.42")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "urn:oasis:names:tc:DSML:2:0:core")]
    public class ExtendedResponse : LDAPResult
    {

        private string responseNameField;

        private object responseField;

        /// <remarks/>
        public string responseName
        {
            get
            {
                return this.responseNameField;
            }
            set
            {
                this.responseNameField = value;
            }
        }

        /// <remarks/>
        public object response
        {
            get
            {
                return this.responseField;
            }
            set
            {
                this.responseField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.42")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "urn:oasis:names:tc:DSML:2:0:core")]
    public class ExtendedRequest : DsmlMessage
    {

        private string requestNameField;

        private object requestValueField;

        /// <remarks/>
        public string requestName
        {
            get
            {
                return this.requestNameField;
            }
            set
            {
                this.requestNameField = value;
            }
        }

        /// <remarks/>
        public object requestValue
        {
            get
            {
                return this.requestValueField;
            }
            set
            {
                this.requestValueField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.42")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "urn:oasis:names:tc:DSML:2:0:core")]
    public class AbandonRequest : DsmlMessage
    {

        private string abandonIDField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string abandonID
        {
            get
            {
                return this.abandonIDField;
            }
            set
            {
                this.abandonIDField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.42")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "urn:oasis:names:tc:DSML:2:0:core")]
    public class CompareRequest : DsmlMessage
    {

        private AttributeValueAssertion assertionField;

        private string dnField;

        /// <remarks/>
        public AttributeValueAssertion assertion
        {
            get
            {
                return this.assertionField;
            }
            set
            {
                this.assertionField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string dn
        {
            get
            {
                return this.dnField;
            }
            set
            {
                this.dnField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.42")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "urn:oasis:names:tc:DSML:2:0:core")]
    public class ModifyDNRequest : DsmlMessage
    {

        private string dnField;

        private string newrdnField;

        private bool deleteoldrdnField;

        private string newSuperiorField;

        public ModifyDNRequest()
        {
            this.deleteoldrdnField = true;
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string dn
        {
            get
            {
                return this.dnField;
            }
            set
            {
                this.dnField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string newrdn
        {
            get
            {
                return this.newrdnField;
            }
            set
            {
                this.newrdnField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        [System.ComponentModel.DefaultValueAttribute(true)]
        public bool deleteoldrdn
        {
            get
            {
                return this.deleteoldrdnField;
            }
            set
            {
                this.deleteoldrdnField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string newSuperior
        {
            get
            {
                return this.newSuperiorField;
            }
            set
            {
                this.newSuperiorField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.42")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "urn:oasis:names:tc:DSML:2:0:core")]
    public class DelRequest : DsmlMessage
    {

        private string dnField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string dn
        {
            get
            {
                return this.dnField;
            }
            set
            {
                this.dnField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.42")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "urn:oasis:names:tc:DSML:2:0:core")]
    public class AddRequest : DsmlMessage
    {

        private DsmlAttr[] attrField;

        private string dnField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("attr")]
        public DsmlAttr[] attr
        {
            get
            {
                return this.attrField;
            }
            set
            {
                this.attrField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string dn
        {
            get
            {
                return this.dnField;
            }
            set
            {
                this.dnField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.42")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "urn:oasis:names:tc:DSML:2:0:core")]
    public class ModifyRequest : DsmlMessage
    {

        private DsmlModification[] modificationField;

        private string dnField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("modification")]
        public DsmlModification[] modification
        {
            get
            {
                return this.modificationField;
            }
            set
            {
                this.modificationField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string dn
        {
            get
            {
                return this.dnField;
            }
            set
            {
                this.dnField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.42")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "urn:oasis:names:tc:DSML:2:0:core")]
    public class SearchRequest : DsmlMessage
    {

        private Filter filterField;

        private AttributeDescriptions attributesField;

        private string dnField;

        private SearchRequestScope scopeField;

        private SearchRequestDerefAliases derefAliasesField;

        private uint sizeLimitField;

        private uint timeLimitField;

        private bool typesOnlyField;

        public SearchRequest()
        {
            this.sizeLimitField = ((uint)(0));
            this.timeLimitField = ((uint)(0));
            this.typesOnlyField = false;
        }

        /// <remarks/>
        public Filter filter
        {
            get
            {
                return this.filterField;
            }
            set
            {
                this.filterField = value;
            }
        }

        /// <remarks/>
        public AttributeDescriptions attributes
        {
            get
            {
                return this.attributesField;
            }
            set
            {
                this.attributesField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string dn
        {
            get
            {
                return this.dnField;
            }
            set
            {
                this.dnField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public SearchRequestScope scope
        {
            get
            {
                return this.scopeField;
            }
            set
            {
                this.scopeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public SearchRequestDerefAliases derefAliases
        {
            get
            {
                return this.derefAliasesField;
            }
            set
            {
                this.derefAliasesField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        [System.ComponentModel.DefaultValueAttribute(typeof(uint), "0")]
        public uint sizeLimit
        {
            get
            {
                return this.sizeLimitField;
            }
            set
            {
                this.sizeLimitField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        [System.ComponentModel.DefaultValueAttribute(typeof(uint), "0")]
        public uint timeLimit
        {
            get
            {
                return this.timeLimitField;
            }
            set
            {
                this.timeLimitField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        [System.ComponentModel.DefaultValueAttribute(false)]
        public bool typesOnly
        {
            get
            {
                return this.typesOnlyField;
            }
            set
            {
                this.typesOnlyField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.42")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "urn:oasis:names:tc:DSML:2:0:core")]
    public enum SearchRequestScope
    {

        /// <remarks/>
        baseObject,

        /// <remarks/>
        singleLevel,

        /// <remarks/>
        wholeSubtree,
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.42")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "urn:oasis:names:tc:DSML:2:0:core")]
    public enum SearchRequestDerefAliases
    {

        /// <remarks/>
        neverDerefAliases,

        /// <remarks/>
        derefInSearching,

        /// <remarks/>
        derefFindingBaseObj,

        /// <remarks/>
        derefAlways,
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.42")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "urn:oasis:names:tc:DSML:2:0:core")]
    public enum BatchRequestProcessing
    {

        /// <remarks/>
        sequential,

        /// <remarks/>
        parallel,
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.42")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "urn:oasis:names:tc:DSML:2:0:core")]
    public enum BatchRequestResponseOrder
    {

        /// <remarks/>
        sequential,

        /// <remarks/>
        unordered,
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.42")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "urn:oasis:names:tc:DSML:2:0:core")]
    public enum BatchRequestOnError
    {

        /// <remarks/>
        resume,

        /// <remarks/>
        exit,
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.42")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "urn:oasis:names:tc:DSML:2:0:core")]
    [System.Xml.Serialization.XmlRootAttribute("batchResponse", Namespace = "urn:oasis:names:tc:DSML:2:0:core", IsNullable = false)]
    public class BatchResponse
    {

        private SearchResponse[] searchResponseField;

        private LDAPResult[] authResponseField;

        private LDAPResult[] modifyResponseField;

        private LDAPResult[] addResponseField;

        private LDAPResult[] delResponseField;

        private LDAPResult[] modDNResponseField;

        private LDAPResult[] compareResponseField;

        private ExtendedResponse[] extendedResponseField;

        private ErrorResponse[] errorResponseField;

        private string requestIDField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("searchResponse")]
        public SearchResponse[] searchResponse
        {
            get
            {
                return this.searchResponseField;
            }
            set
            {
                this.searchResponseField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("authResponse")]
        public LDAPResult[] authResponse
        {
            get
            {
                return this.authResponseField;
            }
            set
            {
                this.authResponseField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("modifyResponse")]
        public LDAPResult[] modifyResponse
        {
            get
            {
                return this.modifyResponseField;
            }
            set
            {
                this.modifyResponseField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("addResponse")]
        public LDAPResult[] addResponse
        {
            get
            {
                return this.addResponseField;
            }
            set
            {
                this.addResponseField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("delResponse")]
        public LDAPResult[] delResponse
        {
            get
            {
                return this.delResponseField;
            }
            set
            {
                this.delResponseField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("modDNResponse")]
        public LDAPResult[] modDNResponse
        {
            get
            {
                return this.modDNResponseField;
            }
            set
            {
                this.modDNResponseField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("compareResponse")]
        public LDAPResult[] compareResponse
        {
            get
            {
                return this.compareResponseField;
            }
            set
            {
                this.compareResponseField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("extendedResponse")]
        public ExtendedResponse[] extendedResponse
        {
            get
            {
                return this.extendedResponseField;
            }
            set
            {
                this.extendedResponseField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("errorResponse")]
        public ErrorResponse[] errorResponse
        {
            get
            {
                return this.errorResponseField;
            }
            set
            {
                this.errorResponseField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string requestID
        {
            get
            {
                return this.requestIDField;
            }
            set
            {
                this.requestIDField = value;
            }
        }
    }

}
