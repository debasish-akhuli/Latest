using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Alfresco;
using Alfresco.RepositoryWebService;

namespace DMS.UTILITY
{
    public class AlfCreateSpace
    {
        /// <summary>
        /// To create the Space in a specific location
        /// </summary>
        /// <param name="SpaceName">Node Name</param>
        /// <param name="SpaceDesc">Node Description</param>
        /// <param name="ParentForSpacePath">=NULL for Drawer/Folder, ="/app:company_home/cm:Office" for Cabinet</param>
        /// <param name="ParentUUID">=null for Cabinet</param>
        /// <param name="ParentName">=SpaceName for Cabinet</param>
        /// <param name="AdminUserID">Administrator Login ID</param>
        /// <param name="Ticket">Ticket generated after logged in by Admin</param>
        /// <returns>SpaceUUID</returns>
        public string CreateSpace(string SpaceName, string SpaceDesc,string ParentForSpacePath, string ParentUUID, string ParentName, string AdminUserID, string Ticket)
        {
            try
            {
                //Alfresco.RepositoryWebService.ParentReference
                Alfresco.RepositoryWebService.Store spacesStore = new Alfresco.RepositoryWebService.Store();
                spacesStore.scheme = Alfresco.RepositoryWebService.StoreEnum.workspace;
                spacesStore.address = "SpacesStore";

                //parent for the new space
                Alfresco.RepositoryWebService.ParentReference parentForSpace = new Alfresco.RepositoryWebService.ParentReference();
                parentForSpace.store = spacesStore;
                parentForSpace.uuid = ParentUUID;
                parentForSpace.path = ParentForSpacePath;
                parentForSpace.associationType = Constants.ASSOC_CONTAINS;
                parentForSpace.childName = "{" + Constants.NAMESPACE_CONTENT_MODEL + "}" + ParentName;

                //build properties
                Alfresco.RepositoryWebService.NamedValue[] propertiesForSpace = new Alfresco.RepositoryWebService.NamedValue[3];
                Alfresco.RepositoryWebService.NamedValue namedValue = new Alfresco.RepositoryWebService.NamedValue();
                namedValue.name = Constants.PROP_NAME;
                namedValue.value = SpaceName;
                propertiesForSpace[0] = namedValue;
                namedValue = new Alfresco.RepositoryWebService.NamedValue();
                namedValue.name = Constants.PROP_TITLE;
                namedValue.value = SpaceName;
                propertiesForSpace[1] = namedValue;
                namedValue = new Alfresco.RepositoryWebService.NamedValue();
                namedValue.name = Constants.PROP_DESCRIPTION;
                namedValue.value = SpaceDesc;
                propertiesForSpace[2] = namedValue;

                //create a space
                CMLCreate createSpace = new CMLCreate();
                createSpace.id = "2";
                createSpace.parent = parentForSpace;
                createSpace.type = Constants.TYPE_FOLDER;
                createSpace.property = propertiesForSpace;

                //build the CML object
                CML cmlAdd = new CML();
                cmlAdd.create = new CMLCreate[] { createSpace };

                //perform a CML update to create nodes
                WebServiceFactory wsF = new WebServiceFactory();
                wsF.UserName = AdminUserID;
                wsF.Ticket = Ticket;
                UpdateResult[] result = wsF.getRepositoryService().update(cmlAdd);
                return result[0].destination.uuid;
            }
            catch
            {
                throw new Exception("Name already exists in this Location!");
            }
        }
        
    }
}