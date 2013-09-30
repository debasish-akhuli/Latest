using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Configuration;
using System.Data.SqlClient;
using DMS.UTILITY;

namespace DMS
{
    public class UserRights
    {
        /// <summary>
        /// This function is used to set the permissions for Cabinet, Drawer, Folder, Document during creation / uploading
        /// </summary>
        /// <param name="NodeUUID">UUID for the Node (Cabinet/Drawer/Folder/Document)</param>
        /// <param name="NodeType">(Cabinet/Drawer/Folder/Document)</param>
        /// <returns></returns>
        /// V->View, M->Modify, D->Delete, X->Blocked
        /// (V means only View), (M means View and Modify), (D means View, Modify and Delete), (X means No permission or Blocked)
        public bool SetPermissions(string NodeUUID, string NodeType, string UserID, string Permission,string CompCode)
        {
            try
            {
                ClassStoreProc ObjClassStoreProc = new ClassStoreProc();
                DataSet ds01 = new DataSet();
                string result;
                // Set permission for Super Admin
                result = ObjClassStoreProc.UserRightsInsert(NodeUUID, NodeType, "admin", "D", CompCode);
                // Set permission for Admin
                ds01.Reset();
                ds01 = ObjClassStoreProc.AdminUserListBasedOnCompCode(CompCode);
                if (ds01.Tables[0].Rows.Count > 0)
                {
                    result = ObjClassStoreProc.UserRightsInsert(NodeUUID, NodeType, ds01.Tables[0].Rows[0][0].ToString(), "D", CompCode);
                }
                // Set permission for Normal Users
                ds01.Reset();
                ds01 = ObjClassStoreProc.NormalUserListBasedOnCompCode(CompCode);
                if (ds01.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < ds01.Tables[0].Rows.Count; i++)
                    {
                        result = ObjClassStoreProc.UserRightsInsert(NodeUUID, NodeType, ds01.Tables[0].Rows[i][0].ToString(), Permission, CompCode);
                    }
                }
                // Set permission for the User who has created this Node
                result = ObjClassStoreProc.UserRightsUpdate4UploadingUser(NodeUUID, NodeType, UserID, "D", CompCode);

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool InsertPermissionSingleData(string NodeUUID, string NodeType, string UserID, string Permission, string CompCode)
        {
            try
            {
                ClassStoreProc ObjClassStoreProc = new ClassStoreProc();
                string result = ObjClassStoreProc.UserRightsInsert(NodeUUID, NodeType, UserID, Permission, CompCode);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public string FetchDefaultPermission(string CabinetUUID)
        {
            try
            {
                SqlConnection con = Utility.GetConnection();
                SqlCommand cmd = null;
                con.Open();

                cmd = new SqlCommand("FetchDefaultPermission", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add("@NodeUUID", SqlDbType.NVarChar, 255);
                cmd.Parameters["@NodeUUID"].Value = CabinetUUID;

                DataSet ds = new DataSet();
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                adapter.Fill(ds);
                Utility.CloseConnection(con);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    return ds.Tables[0].Rows[0][0].ToString();
                }
                else
                {
                    return "X";
                }
            }
            catch (Exception ex)
            {
                return "X";
            }
        }

        public DataSet FetchPermission(string NodeUUID, string CompCode)
        {
            try
            {
                SqlConnection con = Utility.GetConnection();
                SqlCommand cmd = null;
                con.Open();

                cmd = new SqlCommand("sp_FetchPermission", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add("@NodeUUID", SqlDbType.NVarChar, 255);
                cmd.Parameters["@NodeUUID"].Value = NodeUUID;

                cmd.Parameters.Add("@CompCode", SqlDbType.NVarChar, 8);
                cmd.Parameters["@CompCode"].Value = CompCode;

                DataSet ds = new DataSet();
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                adapter.Fill(ds);
                Utility.CloseConnection(con);
                return ds;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public string FetchPermission4User(string NodeUUID, string UserID)
        {
            try
            {
                SqlConnection con = Utility.GetConnection();
                SqlCommand cmd = null;
                con.Open();

                cmd = new SqlCommand("FetchPermission4User", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add("@UserID", SqlDbType.NVarChar, 20);
                cmd.Parameters["@UserID"].Value = UserID;

                cmd.Parameters.Add("@NodeUUID", SqlDbType.NVarChar, 255);
                cmd.Parameters["@NodeUUID"].Value = NodeUUID;

                DataSet ds = new DataSet();
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                adapter.Fill(ds);
                Utility.CloseConnection(con);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    return ds.Tables[0].Rows[0][0].ToString();
                }
                else
                {
                    return "X";
                }
            }
            catch (Exception ex)
            {
                return "X";
            }
        }

        /// <summary>
        /// This function is used to set the permissions when one new user is created
        /// For this, all the cabinets, drawers, folders and documents will be set to "X" for that user.
        /// </summary>
        /// <param name="UserID"></param>
        /// <returns></returns>        
        public bool SetUserPermissions(string UserID, string CompCode)
        {
            try
            {
                SqlConnection con = Utility.GetConnection();
                SqlCommand cmd = null;
                DataSet ds01 = new DataSet();
                DataSet ds02 = new DataSet();
                DataSet ds03 = new DataSet();
                DataSet ds04 = new DataSet();
                string result = "X";
                UserRights RightsObj = new UserRights();

                con.Open();
                // For Cabinet
                cmd = new SqlCommand("select cab_uuid from cabinet_mast where CompCode='" + CompCode + "'", con);
                SqlDataAdapter adapter01 = new SqlDataAdapter(cmd);
                adapter01.Fill(ds01);
                if (ds01.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < ds01.Tables[0].Rows.Count; i++)
                    {
                        result = RightsObj.FetchDefaultPermission(ds01.Tables[0].Rows[i][0].ToString());
                        cmd = new SqlCommand("insert into UserRights(NodeUUID,NodeType,UserID,Permission,CompCode) values('" + ds01.Tables[0].Rows[i][0].ToString() + "','Cabinet','" + UserID + "','" + result + "','" + CompCode + "')", con);
                        cmd.ExecuteNonQuery();
                        //cmd = new SqlCommand("insert into PermissionLog(NodeUUID,NodeType,UserID,Permission,CompCode,UpdatedOn) values('" + ds01.Tables[0].Rows[i][0].ToString() + "','Cabinet','" + UserID + "','" + result + "','" + CompCode + "','" + DateTime.Now + "')", con);
                        //cmd.ExecuteNonQuery();
                        // Drawer
                        cmd = new SqlCommand("select drw_uuid from drawer_mast where cab_uuid='" + ds01.Tables[0].Rows[i][0].ToString() + "'", con);
                        SqlDataAdapter adapter02 = new SqlDataAdapter(cmd);
                        ds02.Reset();
                        adapter02.Fill(ds02);
                        if (ds02.Tables[0].Rows.Count > 0)
                        {
                            for (int j = 0; j < ds02.Tables[0].Rows.Count; j++)
                            {
                                cmd = new SqlCommand("insert into UserRights(NodeUUID,NodeType,UserID,Permission,CompCode) values('" + ds02.Tables[0].Rows[j][0].ToString() + "','Drawer','" + UserID + "','" + result + "','" + CompCode + "')", con);
                                cmd.ExecuteNonQuery();
                                // Folder
                                cmd = new SqlCommand("select fld_uuid from folder_mast where drw_uuid='" + ds02.Tables[0].Rows[j][0].ToString() + "'", con);
                                SqlDataAdapter adapter03 = new SqlDataAdapter(cmd);
                                ds03.Reset();
                                adapter03.Fill(ds03);
                                if (ds03.Tables[0].Rows.Count > 0)
                                {
                                    for (int k = 0; k < ds03.Tables[0].Rows.Count; k++)
                                    {
                                        cmd = new SqlCommand("insert into UserRights(NodeUUID,NodeType,UserID,Permission,CompCode) values('" + ds03.Tables[0].Rows[k][0].ToString() + "','Folder','" + UserID + "','" + result + "','" + CompCode + "')", con);
                                        cmd.ExecuteNonQuery();
                                        // Document
                                        cmd = new SqlCommand("select uuid from doc_mast where fld_uuid='" + ds03.Tables[0].Rows[k][0].ToString() + "'", con);
                                        SqlDataAdapter adapter04 = new SqlDataAdapter(cmd);
                                        ds04.Reset();
                                        adapter04.Fill(ds04);
                                        if (ds04.Tables[0].Rows.Count > 0)
                                        {
                                            for (int l = 0; l < ds04.Tables[0].Rows.Count; l++)
                                            {
                                                cmd = new SqlCommand("insert into UserRights(NodeUUID,NodeType,UserID,Permission,CompCode) values('" + ds04.Tables[0].Rows[l][0].ToString() + "','Document','" + UserID + "','" + result + "','" + CompCode + "')", con);
                                                cmd.ExecuteNonQuery();
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                
                Utility.CloseConnection(con);

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool SetUserNoPermission(string UserID,string CompCode)
        {
            try
            {
                SqlConnection con = Utility.GetConnection();
                SqlCommand cmd = null;
                DataSet ds01 = new DataSet();
                DataSet ds02 = new DataSet();
                DataSet ds03 = new DataSet();
                DataSet ds04 = new DataSet();

                con.Open();
                // For Cabinet
                cmd = new SqlCommand("select cab_uuid from cabinet_mast where CompCode='" + CompCode + "'", con);
                SqlDataAdapter adapter01 = new SqlDataAdapter(cmd);
                adapter01.Fill(ds01);
                if (ds01.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < ds01.Tables[0].Rows.Count; i++)
                    {
                        cmd = new SqlCommand("insert into UserRights(NodeUUID,NodeType,UserID,Permission,CompCode) values('" + ds01.Tables[0].Rows[i][0].ToString() + "','Cabinet','" + UserID + "','X','" + CompCode + "')", con);
                        cmd.ExecuteNonQuery();
                        //cmd = new SqlCommand("insert into PermissionLog(NodeUUID,NodeType,UserID,Permission,CompCode,UpdatedOn) values('" + ds01.Tables[0].Rows[i][0].ToString() + "','Cabinet','" + UserID + "','X','" + CompCode + "','" + DateTime.Now + "')", con);
                        //cmd.ExecuteNonQuery();
                    }
                }

                // For Drawer
                cmd = new SqlCommand("select drw_uuid from drawer_mast where CompCode='" + CompCode + "'", con);
                SqlDataAdapter adapter02 = new SqlDataAdapter(cmd);
                adapter02.Fill(ds02);
                if (ds02.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < ds02.Tables[0].Rows.Count; i++)
                    {
                        cmd = new SqlCommand("insert into UserRights(NodeUUID,NodeType,UserID,Permission,CompCode) values('" + ds02.Tables[0].Rows[i][0].ToString() + "','Drawer','" + UserID + "','X','" + CompCode + "')", con);
                        cmd.ExecuteNonQuery();
                    }
                }

                // For Folder
                cmd = new SqlCommand("select fld_uuid from folder_mast where CompCode='" + CompCode + "'", con);
                SqlDataAdapter adapter03 = new SqlDataAdapter(cmd);
                adapter03.Fill(ds03);
                if (ds03.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < ds03.Tables[0].Rows.Count; i++)
                    {
                        cmd = new SqlCommand("insert into UserRights(NodeUUID,NodeType,UserID,Permission,CompCode) values('" + ds03.Tables[0].Rows[i][0].ToString() + "','Folder','" + UserID + "','X','" + CompCode + "')", con);
                        cmd.ExecuteNonQuery();
                    }
                }

                // For Document
                cmd = new SqlCommand("select uuid from doc_mast where CompCode='" + CompCode + "'", con);
                SqlDataAdapter adapter04 = new SqlDataAdapter(cmd);
                adapter04.Fill(ds04);
                if (ds04.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < ds04.Tables[0].Rows.Count; i++)
                    {
                        cmd = new SqlCommand("insert into UserRights(NodeUUID,NodeType,UserID,Permission,CompCode) values('" + ds04.Tables[0].Rows[i][0].ToString() + "','Document','" + UserID + "','X','" + CompCode + "')", con);
                        cmd.ExecuteNonQuery();
                    }
                }
                Utility.CloseConnection(con);

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        /// <summary>
        /// This function is used to update the permission when the settings is set from User Rights Module
        /// </summary>
        /// <param name="NodeUUID">UUID for the Node (Cabinet/Drawer/Folder/Document)</param>
        /// <param name="NodeType">(Cabinet/Drawer/Folder/Document)</param>
        /// <returns></returns>
        /// V->View, M->Modify, D->Delete, X->Blocked
        /// (V means only View), (M means View and Modify), (D means View, Modify and Delete), (X means No permission or Blocked)
        public bool UpdatePermissions(string NodeUUID, string NodeType,string UserID,string Permission)
        {
            try
            {
                SqlConnection con = Utility.GetConnection();
                SqlCommand cmd = null;
                con.Open();
                cmd = new SqlCommand("update UserRights set NodeUUID='" + NodeUUID + "',NodeType='" + NodeType + "',UserID='" + UserID + "',Permission='" + Permission + "' where NodeUUID='" + NodeUUID + "' and UserID='" + UserID + "'", con);
                cmd.ExecuteNonQuery();
                Utility.CloseConnection(con);

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool UpdatePermissions4Doc(string NodeUUID, string NodeType, string UserID, string Permission)
        {
            try
            {
                SqlConnection con = Utility.GetConnection();
                SqlCommand cmd = null;
                con.Open();
                cmd = new SqlCommand("update UserRights set Permission='" + Permission + "' where NodeUUID='" + NodeUUID + "' and NodeType='Document' and UserID!='admin'", con);
                cmd.ExecuteNonQuery();
                cmd = new SqlCommand("update UserRights set Permission='D' where NodeUUID='" + NodeUUID + "' and NodeType='Document' and UserID='admin'", con);
                cmd.ExecuteNonQuery();
                Utility.CloseConnection(con);

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }


        public DataSet FetchPermissions(string NodeUUID, string UserID)
        {
            try
            {
                SqlConnection con = Utility.GetConnection();
                SqlCommand cmd = null;
                DataSet ds04 = new DataSet();
                con.Open();
                cmd = new SqlCommand("select NodeType,Permission from UserRights where NodeUUID='" + NodeUUID + "' and UserID='" + UserID + "'", con);
                SqlDataAdapter adapter04 = new SqlDataAdapter(cmd);
                adapter04.Fill(ds04);
                Utility.CloseConnection(con);
                if (ds04.Tables[0].Rows.Count > 0)
                {
                    return ds04;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

    }
}