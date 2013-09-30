using System;
using System.Collections.Generic;
using System.Text;
using Alfresco.AuthenticationWebService;

namespace Alfresco
{
    /// <summary>
    /// Authentication Utils
    /// </summary>
    public class AuthenticationUtils
    {
        /// <summary>
        /// The current ticket stored per thread
        /// </summary>
        [ThreadStatic]
        private string currentTicket;

        /// <summary>
        /// The current user name stored per thread
        /// </summary>
        [ThreadStatic]
        private string currentUserName;

        /// <summary>
        /// The current ticket
        /// </summary>
        public string Ticket
        {
            get
            {
                return this.currentTicket;
            }
        }

        /// <summary>
        /// The current user name
        /// </summary>
        public string UserName
        {
            get
            {
                return this.currentUserName;
            }
        }

        /// <summary>
        /// Indicates whether we currently have a ticket for the current session.
        /// 
        /// NOTE: we could do with a isSessionValid method on the authentication service so that we can check whether the 
        /// stored ticket is still valid or not.
        /// </summary>
        public bool IsSessionValid
        {
            get
            {
                return (this.currentTicket != null);
            }
        }

        /// <summary>
        /// Starts the session
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        public void startSession(string userName, string password)
        {
            // Try and authenticate the user and then store the results in the thread static members
            WebServiceFactory wsF = new WebServiceFactory();
            AuthenticationResult results = wsF.getAuthenticationService().startSession(userName, password);
            this.currentTicket = results.ticket;
            this.currentUserName = results.username;
        }

        /// <summary>
        /// Ends the session
        /// </summary>
        public void endSession()
        {
            if (this.currentTicket != null)
            {
                WebServiceFactory wsF = new WebServiceFactory();
                wsF.getAuthenticationService().endSession(this.currentTicket);
                this.currentTicket = null;
                this.currentUserName = null;
            }
        }
    }
}
