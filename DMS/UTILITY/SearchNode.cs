using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DMS.BAL;
using System.Data;
using System.Data.SqlClient;
using Alfresco.RepositoryWebService;
using Alfresco;
using DMS.UTILITY;

namespace DMS.UTILITY
{
    public class SearchNode
    {
        private Alfresco.RepositoryWebService.Store spacesStore;
        private RepositoryService repoService;

        public RepositoryService RepoService
        {
            set { repoService = value; }
        }

        public string ExistNode(string ParentNodeUUID, string NodeName,string AdminUserID,string AdminLoginTicket)
        {
            try
            {
                WebServiceFactory wsF = new WebServiceFactory();
                wsF.UserName = AdminUserID;
                wsF.Ticket = AdminLoginTicket;
                this.repoService = wsF.getRepositoryService();

                this.spacesStore = new Alfresco.RepositoryWebService.Store();
                this.spacesStore.scheme = Alfresco.RepositoryWebService.StoreEnum.workspace;
                this.spacesStore.address = "SpacesStore";

                Alfresco.RepositoryWebService.Reference reference = new Alfresco.RepositoryWebService.Reference();
                reference.store = this.spacesStore;
                reference.uuid = ParentNodeUUID;

                string ExistFlag = "";
                QueryResult result = this.repoService.queryChildren(reference);
                if (result.resultSet.rows != null)
                {
                    foreach (ResultSetRow row in result.resultSet.rows)
                    {
                        foreach (Alfresco.RepositoryWebService.NamedValue namedValue in row.columns)
                        {
                            if (namedValue.value == "{}" + NodeName)
                            {
                                ExistFlag = row.node.id;
                            }
                        }
                    }
                }
                return ExistFlag;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}