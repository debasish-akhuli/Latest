using System;
using System.Collections.Generic;
using System.Text;
using Alfresco.AuthenticationWebService;
using Alfresco.RepositoryWebService;
using Alfresco.ContentWebService;
using Alfresco.AccessControlWebService;
using Alfresco.ActionWebService;
using Alfresco.AdministrationWebService;
using Alfresco.AuthoringWebService;
using Alfresco.ClassificationWebService;
using Alfresco.DictionaryServiceWebService;
using Microsoft.Web.Services3;
using Microsoft.Web.Services3.Security;
using Microsoft.Web.Services3.Security.Tokens;
using Microsoft.Web.Services3.Security.Utility;

namespace Alfresco
{
    /// <summary>
    /// Web Service Factory
    /// 
    /// Convenience class that provides instances of the web service classes with the specified end-point 
    /// set and the security header added based on the information set in the AuthenticationUtils set (where
    /// appropriate)
    /// </summary>
    public class WebServiceFactory
    {
        /** Default endpoint address **/
        private const string DEFAULT_ENDPOINT_ADDRESS = "http://localhost:8080/alfresco";

        /** Current endpoint address **/
        private string endPointAddress = DEFAULT_ENDPOINT_ADDRESS;

        /** Service addresses */
        private const string AUTHENTICATION_SERVICE_ADDRESS = "/api/AuthenticationService";
        private const string REPOSITORY_SERVICE_ADDRESS = "/api/RepositoryService";
        private const string CONTENT_SERVICE_ADDRESS = "/api/ContentService";
        private const string AUTHORING_SERVICE_ADDRESS = "/api/AuthoringService";
        private const string CLASSIFICATION_SERVICE_ADDRESS = "/api/ClassificationService";
        private const string ACTION_SERVICE_ADDRESS = "/api/ActionService";
        private const string ACCESS_CONTROL_SERVICE_ADDRESS = "/api/AccessControlService";
        private const string ADMINISTRATION_SERVICE_ADDRESS = "/api/AdministrationService";
        private const string DICTIONARY_SERVICE_ADDRESS = "/api/DictionaryService";

        /** Authentication details **/

        private string userName;
        private string ticket;

        public string Ticket
        {
            get
            {
                return ticket;
            }
            set
            {
                ticket = value;
            }
        }

        public string UserName
        {
            get
            {
                return userName;
            }
            set
            {
                userName = value;
            }
        }

        /// <summary>
        /// Set the end point address used for all web service call's here-in
        /// </summary>
        /// <param name="endPointAddress"></param>
        /// <returns></returns>
        public void setEndpointAddress(String endPointAddress)
        {
            this.endPointAddress = endPointAddress;
        }

        /// <summary>
        /// Get the current end point address
        /// </summary>
        /// <returns></returns>
        public String getEndpointAddress()
        {
            return endPointAddress;
        }

        private void addSecurityHeader(Microsoft.Web.Services3.WebServicesClientProtocol service)
        {
            UsernameToken userToken = new UsernameToken(userName, ticket, (PasswordOption)2);
            service.RequestSoapContext.Security.Timestamp.TtlInSeconds = (long)300;
            service.RequestSoapContext.Security.Tokens.Add(userToken);
        }

        /// <summary>
        /// Get the authentication service
        /// </summary>
        /// <returns></returns>
        public AuthenticationService getAuthenticationService()
        {
            return getAuthenticationService(getEndpointAddress());
        }

        /// <summary>
        /// Get the authentication service
        /// </summary>
        /// <param name="endPointAddress"></param>
        /// <returns></returns>
        public AuthenticationService getAuthenticationService(String endPointAddress)
        {
            AuthenticationService authenticationService = new AuthenticationService();
            authenticationService.Url = endPointAddress + AUTHENTICATION_SERVICE_ADDRESS;
            return authenticationService;
        }

        /// <summary>
        /// Get the respoitory service 
        /// To call this service first create object of <WebServiceFactory>
        /// By using that object assign value to the <Ticket> and <UserName> property
        /// By using that object call <getRepositoryService> method
        /// </summary>
        /// <returns></returns>
        public RepositoryService getRepositoryService()
        {
            return getRepositoryService(getEndpointAddress());
        }

        /// <summary>
        /// Get the repository service
        /// </summary>
        /// <param name="endPointAddress"></param>
        /// <returns></returns>
        public RepositoryService getRepositoryService(String endPointAddress)
        {
            RepositoryService repositoryService = new RepositoryService();
            repositoryService.Url = endPointAddress + REPOSITORY_SERVICE_ADDRESS;
            addSecurityHeader(repositoryService);
            return repositoryService;
        }

        /// <summary>
        /// Get the content service
        /// </summary>
        /// <returns></returns>
        public ContentService getContentService()
        {
            return getContentService(getEndpointAddress());
        }

        /// <summary>
        /// Get the content service
        /// </summary>
        /// <param name="endPointAddress"></param>
        /// <returns></returns>
        public ContentService getContentService(String endPointAddress)
        {
            ContentService contentService = new ContentService();
            contentService.Url = endPointAddress + CONTENT_SERVICE_ADDRESS;
            addSecurityHeader(contentService);
            return contentService;
        }

        public AuthoringService getAuthoringService()
        {
            return getAuthoringService(getEndpointAddress());
        }

        public AuthoringService getAuthoringService(String endPointAddress)
        {
            AuthoringService authoringService = new AuthoringService();
            authoringService.Url = endPointAddress + AUTHORING_SERVICE_ADDRESS;
            addSecurityHeader(authoringService);
            return authoringService;
        }

        public ClassificationService getClassificationService()
        {
            return getClassificationService(getEndpointAddress());
        }

        public ClassificationService getClassificationService(String endPointAddress)
        {
            ClassificationService classificationService = new ClassificationService();
            classificationService.Url = endPointAddress + CLASSIFICATION_SERVICE_ADDRESS;
            addSecurityHeader(classificationService);
            return classificationService;
        }

        public ActionService getActionService()
        {
            return getActionService(getEndpointAddress());
        }

        public ActionService getActionService(String endPointAddress)
        {
            ActionService actionService = new ActionService();
            actionService.Url = endPointAddress + ACTION_SERVICE_ADDRESS;
            addSecurityHeader(actionService);
            return actionService;
        }

        public AccessControlService getAccessControlService()
        {
            return getAccessControlService(getEndpointAddress());
        }

        public AccessControlService getAccessControlService(String endPointAddress)
        {
            AccessControlService accessControlService = new AccessControlService();
            accessControlService.Url = endPointAddress + ACCESS_CONTROL_SERVICE_ADDRESS;
            addSecurityHeader(accessControlService);
            return accessControlService;
        }

        public AdministrationService getAdministrationService()
        {
            return getAdministrationService(getEndpointAddress());
        }

        public AdministrationService getAdministrationService(String endPointAddress)
        {
            AdministrationService administrationService = new AdministrationService();
            administrationService.Url = endPointAddress + ADMINISTRATION_SERVICE_ADDRESS;
            addSecurityHeader(administrationService);
            return administrationService;
        }

        public DictionaryService getDictionaryService()
        {
            return getDictionaryService(getEndpointAddress());
        }

        public DictionaryService getDictionaryService(String endPointAddress)
        {
            DictionaryService dictionaryService = new DictionaryService();
            dictionaryService.Url = endPointAddress + DICTIONARY_SERVICE_ADDRESS;
            addSecurityHeader(dictionaryService);
            return dictionaryService;
        }
    }
}
