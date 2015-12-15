using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Security.Principal;
using System.IdentityModel.Selectors;
using System.Security.Cryptography.X509Certificates;
using Microsoft.ServiceModel.Web;
using System.ServiceModel;
using System.ServiceModel.Security;
using System.ServiceModel.Channels;
using System.IdentityModel.Policy;
using System.IdentityModel.Claims;
using System.ServiceModel.Activation;

namespace WcfService {
    public class MockHostFactory : ServiceHostFactory {
        public static string CustomerName { get; set; }

        protected override ServiceHost CreateServiceHost(Type serviceType,
                               Uri[] baseAddresses) {
            var serviceHost = new WebServiceHost2(serviceType, true, baseAddresses);
            serviceHost.Interceptors.Add(new MockRequestInterceptor());
            return serviceHost;
        }
    }

    public class MockRequestInterceptor : RequestInterceptor {
        public MockRequestInterceptor()
            : base(false) {
        }

        public override void ProcessRequest(ref RequestContext requestContext) {
            if (requestContext.RequestMessage.Properties.Security == null) {
                requestContext.RequestMessage.Properties.Security = new SecurityMessageProperty();
            }

            requestContext.RequestMessage.Properties.Security.ServiceSecurityContext = new ServiceSecurityContext(new List<IAuthorizationPolicy> {
                new CertAuthPolicy(new GenericPrincipal(new GenericIdentity(MockHostFactory.CustomerName), new string[] {}))
            }.AsReadOnly());
        }
    }

    public class CertAuthPolicy : IAuthorizationPolicy {
        private readonly IPrincipal principal;

        private readonly string policyId = Guid.NewGuid().ToString();

        public CertAuthPolicy(IPrincipal principal)
        {
            this.principal = principal;
        }

        public string Id
        {
            get { return policyId; }
        }

        public bool Evaluate(EvaluationContext evaluationContext, ref object state)
        {
            evaluationContext.AddClaimSet(this, new DefaultClaimSet(Claim.CreateNameClaim(principal.Identity.Name)));
            evaluationContext.Properties["Identities"] = new List<IIdentity>(new[] { principal.Identity });
            evaluationContext.Properties["Principal"] = principal;
            return true;
        }

        public ClaimSet Issuer
        {
            get { return ClaimSet.System; }
        }
    }

    public class MockX509CertificateValidator : X509CertificateValidator {
        public override void Validate(X509Certificate2 certificate) {
            // Check that there is a certificate.
            if (certificate == null) {
                throw new ArgumentNullException("certificate");
            }

            //MockHostFactory.CustomerName = certificate.Subject;
            MockHostFactory.CustomerName = "1234560001";
        }
    }
}