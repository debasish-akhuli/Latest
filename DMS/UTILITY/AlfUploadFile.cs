using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Alfresco;
using Alfresco.RepositoryWebService;
using Alfresco.ContentWebService;
using System.IO;

namespace DMS.UTILITY
{
    public class AlfUploadFile
    {
        private Alfresco.RepositoryWebService.Store spacesStore;
        private RepositoryService repoService;
        public RepositoryService RepoService
        {
            set { repoService = value; }
        }

        public string UploadFile(string FileName, string ParentUUID, string FileExtension, byte[] bytes, ContentFormat contentFormat, string ByBrowseButton, string AdminUserID, string AdminTicket)
        {
            try
            {
                WebServiceFactory wsF = new WebServiceFactory();
                wsF.UserName = AdminUserID;
                wsF.Ticket = AdminTicket;
                repoService = wsF.getRepositoryService();
                // Initialise the reference to the spaces store
                Alfresco.RepositoryWebService.Store spacesStore = new Alfresco.RepositoryWebService.Store();
                spacesStore.scheme = Alfresco.RepositoryWebService.StoreEnum.workspace;
                spacesStore.address = "SpacesStore";

                // Create the parent reference, the company home folder
                Alfresco.RepositoryWebService.ParentReference parentReference = new Alfresco.RepositoryWebService.ParentReference();
                parentReference.store = spacesStore;
                parentReference.uuid = ParentUUID;

                parentReference.associationType = Constants.ASSOC_CONTAINS;
                parentReference.childName = Constants.createQNameString(Constants.NAMESPACE_CONTENT_MODEL, FileName);

                // Create the properties list
                NamedValue nameProperty = new NamedValue();
                nameProperty.name = Constants.PROP_NAME;
                nameProperty.value = FileName;
                nameProperty.isMultiValue = false;

                NamedValue[] properties = new NamedValue[2];
                properties[0] = nameProperty;
                nameProperty = new NamedValue();
                nameProperty.name = Constants.PROP_TITLE;
                nameProperty.value = FileName;
                nameProperty.isMultiValue = false;
                properties[1] = nameProperty;

                // Create the CML create object
                CMLCreate create = new CMLCreate();
                create.parent = parentReference;
                create.id = "1";
                create.type = Constants.TYPE_CONTENT;
                create.property = properties;

                // Create and execute the cml statement
                CML cml = new CML();
                cml.create = new CMLCreate[] { create };
                UpdateResult[] updateResult = repoService.update(cml);

                // work around to cast Alfresco.RepositoryWebService.Reference to
                // Alfresco.ContentWebService.Reference 
                Alfresco.RepositoryWebService.Reference rwsRef = updateResult[0].destination;
                Alfresco.ContentWebService.Reference newContentNode = new Alfresco.ContentWebService.Reference();
                newContentNode.path = rwsRef.path;
                newContentNode.uuid = rwsRef.uuid;
                Alfresco.ContentWebService.Store cwsStore = new Alfresco.ContentWebService.Store();
                cwsStore.address = "SpacesStore";
                spacesStore.scheme = Alfresco.RepositoryWebService.StoreEnum.workspace;
                newContentNode.store = cwsStore;

                wsF.getContentService().write(newContentNode, Constants.PROP_CONTENT, bytes, contentFormat);
                return newContentNode.uuid;
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message); //("Document Uploading Error!");
            }
        }

    }
}